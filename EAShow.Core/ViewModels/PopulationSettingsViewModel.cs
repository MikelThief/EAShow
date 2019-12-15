using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Abstractions.Interfaces;
using EAShow.Shared.Events;
using EAShow.Shared.Models;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class PopulationSettingsViewModel : Screen, IPreset
    {
        private short _enabledCount;
        private bool _isPopulation1Included;
        private bool _isPopulation2Included;
        private decimal _population1;
        private decimal _population2;

        private readonly IEventAggregator _eventAggregator;

        public PopulationSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Population1 = 100;
            Population2 = 100;

            return base.OnInitializeAsync(cancellationToken: cancellationToken);
        }

        public short EnabledCount
        {
            get => _enabledCount;
            private set => Set(oldValue: ref _enabledCount, newValue: value, nameof(EnabledCount));
        }

        public bool IsPopulation1Included
        {
            get => _isPopulation1Included;
            set
            {
                Set(oldValue: ref _isPopulation1Included, newValue: value, nameof(IsPopulation1Included));
                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;

                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public bool IsPopulation2Included
        {
            get => _isPopulation2Included;
            set
            {
                Set(oldValue: ref _isPopulation2Included, newValue: value, nameof(IsPopulation2Included));
                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;

                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public decimal Population1
        {
            get => _population1;
            set => Set(oldValue: ref _population1, newValue: value, nameof(Population1));
        }

        public decimal Population2
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
