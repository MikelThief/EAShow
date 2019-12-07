using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Events;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;
using LiteDB;

namespace EAShow.Core.ViewModels
{
    public class MainViewModel : Screen, IHandle<PresetEnabledCountChangedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        private byte _mutationsCount;
        private byte _populationsCount;
        private byte _selectionsCount;
        private byte _crossoversCount;

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
            _eventAggregator.SubscribeOnUIThread(subscriber: this);
        }

        public MutationSettingsViewModel MutationSettingsViewModel { get; }
        public PopulationSettingsViewModel PopulationSettingsViewModel { get; }
        public CrossoverSettingsViewModel CrossoverSettingsViewModel { get; }
        public SelectionSettingsViewModel SelectionSettingsViewModel { get; }

        public byte MutationsCount
        {
            get => _mutationsCount;
            private set => Set(oldValue: ref _mutationsCount, newValue: value, propertyName: nameof(MutationsCount));
        }

        public byte SelectionsCount
        {
            get => _selectionsCount;
            private set => Set(oldValue: ref _selectionsCount, newValue: value, propertyName: nameof(SelectionsCount));
        }

        public byte PopulationsCount
        {
            get => _populationsCount;
            private set => Set(oldValue: ref _populationsCount, newValue: value, propertyName: nameof(PopulationsCount));
        }

        public byte CrossoversCount
        {
            get => _crossoversCount;
            private set => Set(oldValue: ref _crossoversCount, newValue: value, propertyName: nameof(CrossoversCount));
        }


        public Task HandleAsync(PresetEnabledCountChangedEvent message, CancellationToken cancellationToken)
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

            return Task.CompletedTask;
        }

        public void SaveProfile()
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetConnectionString()))
            {
                var selections = GetSelections();
                var crossovers = GetCrossovers();
                var mutations = GetMutations();
                var populations = GetPopulations();

                var profile = new Profile
                {
                    Mutations = mutations.ToList(),
                    Selections = selections.ToList(),
                    Crossovers = crossovers.ToList(),
                    Populations = populations.ToList()
                };

                db.Insert<Profile>(entity: profile);
            }
        }

        private IEnumerable<Mutation> GetMutations()
        {
            if (MutationSettingsViewModel.IsMutation1Included)
                yield return Mutation.From(item: MutationSettingsViewModel.Mutation1.Value);
            if (MutationSettingsViewModel.IsMutation2Included)
                yield return Mutation.From(item: MutationSettingsViewModel.Mutation2.Value);
        }

        private IEnumerable<Selection> GetSelections()
        {
            if (SelectionSettingsViewModel.IsSelection1Included)
                yield return Selection.From(item: SelectionSettingsViewModel.Selection1);
            if (SelectionSettingsViewModel.IsSelection2Included)
                yield return Selection.From(item: SelectionSettingsViewModel.Selection2);
        }

        private IEnumerable<Population> GetPopulations()
        {
            if (PopulationSettingsViewModel.IsPopulation1Included)
                yield return Population.From(item: (int) PopulationSettingsViewModel.Population1);
            if (PopulationSettingsViewModel.IsPopulation2Included)
                yield return Population.From(item: (int) PopulationSettingsViewModel.Population2);
        }

        private IEnumerable<Crossover> GetCrossovers()
        {
            if (CrossoverSettingsViewModel.IsCrossover1Included)
                yield return Crossover.From(item: CrossoverSettingsViewModel.Crossover1);
            if (CrossoverSettingsViewModel.IsCrossover2Included)
                yield return Crossover.From(item: CrossoverSettingsViewModel.Crossover2);
        }
    }
}
