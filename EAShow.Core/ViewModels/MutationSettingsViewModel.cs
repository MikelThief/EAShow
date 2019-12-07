using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Events;
using EAShow.Core.Core.Models;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class MutationSettingsViewModel : Screen
    {
        private bool _isMutation1Included;
        private bool _isMutation2Included;
        private double? _coefficient1;
        private double? _coefficient2;

        private readonly IEventAggregator _eventAggregator;

        public MutationSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool IsMutation1Included
        {
            get => _isMutation1Included;
            set
            {
                Set(oldValue: ref _isMutation1Included, newValue: value, nameof(IsMutation1Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public bool IsMutation2Included
        {
            get => _isMutation2Included;
            set
            {
                Set(oldValue: ref _isMutation2Included, newValue: value, nameof(IsMutation2Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public double? Coefficient1
        {
            get => _coefficient1;
            set => Set(oldValue: ref _coefficient1, newValue: value, nameof(Coefficient1));
        }

        public double? Coefficient2
        {
            get => _coefficient2;
            set => Set(oldValue: ref _coefficient2, newValue: value, nameof(Coefficient2));
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
