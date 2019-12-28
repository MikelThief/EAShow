using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Events;
using EAShow.Shared.Models;
using EAShow.Core.Helpers;
using EAShow.Infrastructure.Commands;
using EAShow.Infrastructure.Commands.DelegateCommand;
using EAShow.Shared.Models.DTOs;
using LiteDB;

namespace EAShow.Core.ViewModels
{
    public class MainViewModel : Screen, IHandle<PresetEnabledCountChangedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        private short _mutationsCount;
        private short _populationsCount;
        private short _selectionsCount;
        private short _crossoversCount;
        private string _profileName;

        public MainViewModel(
            MutationSettingsViewModel mutationSettingsViewModel,
            PopulationSettingsViewModel populationSettingsViewModel,
            CrossoverSettingsViewModel crossoverSettingsViewModel,
            SelectionSettingsViewModel selectionSettingsViewModel,
            IEventAggregator eventAggregator)
        {

            MutationSettingsViewModel = mutationSettingsViewModel;
            PopulationSettingsViewModel = populationSettingsViewModel;
            CrossoverSettingsViewModel = crossoverSettingsViewModel;
            SelectionSettingsViewModel = selectionSettingsViewModel;
            _eventAggregator = eventAggregator;
            SaveProfileCommand = new DelegateCommand(executeMethod: SaveProfileDto, canExecuteMethod: CanSaveProfile);


            SelectionSettingsViewModel.ConductWith(parent: this);
            CrossoverSettingsViewModel.ConductWith(parent: this);
            PopulationSettingsViewModel.ConductWith(parent: this);
            MutationSettingsViewModel.ConductWith(parent: this);
        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            SaveProfileCommand.ObservesProperty(() => MutationsCount, () => PopulationsCount, () => CrossoversCount, () => SelectionsCount);
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.SubscribeOnBackgroundThread(subscriber: this);
            return base.OnActivateAsync(cancellationToken);
        }

        public MutationSettingsViewModel MutationSettingsViewModel { get; }
        public PopulationSettingsViewModel PopulationSettingsViewModel { get; }
        public CrossoverSettingsViewModel CrossoverSettingsViewModel { get; }
        public SelectionSettingsViewModel SelectionSettingsViewModel { get; }

        public DelegateCommand SaveProfileCommand { get; private set; }

        public short MutationsCount
        {
            get => _mutationsCount;
            private set => Set(oldValue: ref _mutationsCount, newValue: value, propertyName: nameof(MutationsCount));
        }

        public short SelectionsCount
        {
            get => _selectionsCount;
            private set => Set(oldValue: ref _selectionsCount, newValue: value, propertyName: nameof(SelectionsCount));
        }

        public short PopulationsCount
        {
            get => _populationsCount;
            private set => Set(oldValue: ref _populationsCount, newValue: value, propertyName: nameof(PopulationsCount));
        }

        public short CrossoversCount
        {
            get => _crossoversCount;
            private set => Set(oldValue: ref _crossoversCount, newValue: value, propertyName: nameof(CrossoversCount));
        }

        public string ProfileName
        {
            get => _profileName;
            set
            {
                Set(oldValue: ref _profileName, newValue: value.Trim(), propertyName: nameof(ProfileName));
            }
        }

        public Task HandleAsync(PresetEnabledCountChangedEvent message, CancellationToken cancellationToken)
        {

            OnUIThread(() =>
            {
                switch (message.Preset)
                {
                    case Presets.Mutation:
                        MutationsCount = message.Count;
                        break;
                    case Presets.Crossover:
                        CrossoversCount = message.Count;
                        break;
                    case Presets.Selection:
                        SelectionsCount = message.Count;
                        break;
                    case Presets.Population:
                        PopulationsCount = message.Count;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(paramName: nameof(message.Preset),
                            actualValue: message.Preset, message: "Presets doesn't contain this value");
                }
            });

            return Task.CompletedTask;
        }

        public void SaveProfileDto()
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetRoamingDbConnectionString()))
            {
                var selections = GetSelections();
                var crossovers = GetCrossovers();
                var mutations = GetMutations();
                var populations = GetPopulations();

                var profile = new PresetsProfile(id: ObjectId.NewObjectId(), name: ProfileName, mutations: mutations.ToList(),
                    crossovers: crossovers.ToList(), selections: selections.ToList(),
                    populations: populations.ToList());

                db.Insert(entity: ProfileDbDto.From(presetsProfileEntity: profile));
            }

            _eventAggregator.PublishOnBackgroundThreadAsync(message: new PresetResetRequestedEvent(), CancellationToken.None);
            ProfileName = string.Empty;
        }

        private bool CanSaveProfile() =>
            CrossoversCount > 0 &&
            SelectionsCount > 0 &&
            PopulationsCount > 0 &&
            MutationsCount > 0;

        private IEnumerable<Mutation> GetMutations()
        {
            if (MutationSettingsViewModel.IsMutation1Included)
                yield return new Mutation(value: MutationSettingsViewModel.Mutation1);
            if (MutationSettingsViewModel.IsMutation2Included)
                yield return new Mutation(value: MutationSettingsViewModel.Mutation2);
        }

        private IEnumerable<Selection> GetSelections()
        {
            if (SelectionSettingsViewModel.IsSelection1Included)
                yield return new Selection(value: SelectionSettingsViewModel.Selection1);
            if (SelectionSettingsViewModel.IsSelection2Included)
                yield return new Selection(value: SelectionSettingsViewModel.Selection2);
        }

        private IEnumerable<Population> GetPopulations()
        {
            if (PopulationSettingsViewModel.IsPopulation1Included)
                yield return new Population(value: (int)PopulationSettingsViewModel.Population1);
            if (PopulationSettingsViewModel.IsPopulation2Included)
                yield return new Population(value: (int)PopulationSettingsViewModel.Population2);
        }

        private IEnumerable<Crossover> GetCrossovers()
        {
            if (CrossoverSettingsViewModel.IsCrossover1Included)
                yield return new Crossover(value: CrossoverSettingsViewModel.Crossover1);
            if (CrossoverSettingsViewModel.IsCrossover2Included)
                yield return new Crossover(value: CrossoverSettingsViewModel.Crossover2);
        }
    }
}
