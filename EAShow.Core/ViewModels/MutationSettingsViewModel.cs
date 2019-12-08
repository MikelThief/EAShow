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
    public class MutationSettingsViewModel : Screen, IPreset
    {
        private byte _enabledCount;
        private bool _isMutation1Included;
        private bool _isMutation2Included;
        private decimal _mutation1;
        private decimal _mutation2;

        private readonly IEventAggregator _eventAggregator;

        public MutationSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public byte EnabledCount
        {
            get => _enabledCount;
            set => Set(oldValue: ref _enabledCount, newValue: value, nameof(EnabledCount));
        }

        public bool IsMutation1Included
        {
            get => _isMutation1Included;
            set
            {
                Set(oldValue: ref _isMutation1Included, newValue: value, nameof(IsMutation1Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);

                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;
            }
        }

        public bool IsMutation2Included
        {
            get => _isMutation2Included;
            set
            {
                Set(oldValue: ref _isMutation2Included, newValue: value, nameof(IsMutation2Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);

                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;
            }
        }

        public decimal Mutation1
        {
            get => _mutation1;
            set => Set(oldValue: ref _mutation1, newValue: value, nameof(Mutation1));
        }

        public decimal Mutation2
        {
            get => _mutation2;
            set => Set(oldValue: ref _mutation2, newValue: value, nameof(Mutation2));
        }

        public async Task PublishEnabledCount()
        {
            byte count = default;

            if (IsMutation1Included)
                count++;
            if (IsMutation2Included)
                count++;

            await _eventAggregator.PublishOnUIThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Mutation,
                Count = count
            });
        }
    }
}
