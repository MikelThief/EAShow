using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Abstractions.Interfaces;
using EAShow.Core.Core.Events;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class SelectionSettingsViewModel : Screen, IPreset
    {
        private short _enabledCount;
        private bool _isSelection1Included;
        private bool _isSelection2Included;
        private List<Selections> _SelectionInts;
        private Selections _Selection1;
        private Selections _Selection2;

        private readonly IEventAggregator _eventAggregator;

        public bool IsSelection1Included
        {
            get => _isSelection1Included;
            set
            {
                Set(oldValue: ref _isSelection1Included, newValue: value, propertyName: nameof(IsSelection1Included));
                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;

                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public bool IsSelection2Included
        {
            get => _isSelection2Included;
            set
            {
                Set(oldValue: ref _isSelection2Included, newValue: value, propertyName: nameof(IsSelection2Included));
                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;

                NotifyTask.Create(asyncAction: PublishEnabledCount);
            }
        }

        public Selections Selection1
        {
            get => _Selection1;
            set => Set(oldValue: ref _Selection1, newValue: value, propertyName: nameof(Selection1));
        }

        public Selections Selection2
        {
            get => _Selection2;
            set => Set(oldValue: ref _Selection2, newValue: value, propertyName: nameof(Selection2));
        }

        public List<Selections> SelectionInts
        {
            get => _SelectionInts;
            set => Set(oldValue: ref _SelectionInts, newValue: value, propertyName: nameof(SelectionInts));
        }

        public short EnabledCount
        {
            get => _enabledCount;
            private set => Set(oldValue: ref _enabledCount, newValue: value, nameof(EnabledCount));
        }

        public SelectionSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            SelectionInts = new List<Selections>(collection: EnumHelper.GetValuesAsReadOnlyCollection<Selections>());
        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Selection1 = SelectionInts[0];
            Selection2 = SelectionInts[0];

            return base.OnInitializeAsync(cancellationToken: cancellationToken);
        }

        public async Task PublishEnabledCount()
        {
            byte count = default;

            if (IsSelection1Included)
                count++;
            if (IsSelection2Included)
                count++;

            await _eventAggregator.PublishOnUIThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Selection,
                Count = count
            });
        }
    }
}
