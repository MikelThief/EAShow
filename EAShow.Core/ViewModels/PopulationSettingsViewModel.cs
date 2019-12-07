using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Abstractions.Interfaces;
using EAShow.Core.Core.Events;
using EAShow.Core.Core.Models;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class PopulationSettingsViewModel : Screen, IPreset
    {
        private bool _isPopulation1Included;
        private bool _isPopulation2Included;
        private double? _population1;
        private double? _population2;

        private readonly IEventAggregator _eventAggregator;

        public PopulationSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool IsPopulation1Included
        {
            get => _isPopulation1Included;
            set
            {
                Set(oldValue: ref _isPopulation1Included, newValue: value, nameof(IsPopulation1Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public bool IsPopulation2Included
        {
            get => _isPopulation2Included;
            set
            {
                Set(oldValue: ref _isPopulation2Included, newValue: value, nameof(IsPopulation2Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public double? Population1
        {
            get => _population1;
            set => Set(oldValue: ref _population1, newValue: value, nameof(Population1));
        }

        public double? Population2
        {
            get => _population2;
            set => Set(oldValue: ref _population2, newValue: value, nameof(Population2));
        }

        public async Task PublishEnabledCount()
        {
            byte count = default;

            if (IsPopulation1Included)
                count++;
            if (IsPopulation2Included)
                count++;

            await _eventAggregator.PublishOnUIThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Population,
                Count = count
            });
        }
    }
}
