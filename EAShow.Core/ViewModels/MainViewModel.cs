using System;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Events;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;

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
                    throw new ArgumentOutOfRangeException(paramName: nameof(message.Preset), actualValue: message.Preset, message: "Presets doesn't contain this value");
            }

            return Task.CompletedTask;
        }
    }
}
