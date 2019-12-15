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
    public class MutationSettingsViewModel : Screen, IPreset
    {
        private short _enabledCount;
        private bool _isMutation1Included;
        private bool _isMutation2Included;
        private decimal _mutation1;
        private decimal _mutation2;

        private readonly IEventAggregator _eventAggregator;

        public MutationSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await RestoreDefaultsAsync();
            await base.OnInitializeAsync(cancellationToken: cancellationToken);
        }

        public short EnabledCount
        {
            get => _enabledCount;
            private set
            {
                Set(oldValue: ref _enabledCount, newValue: value, nameof(EnabledCount));
                NotifyTask.Create(asyncAction: PublishEnabledCountAsync);
            }
        }

        public bool IsMutation1Included
        {
            get => _isMutation1Included;
            set
            {
                Set(oldValue: ref _isMutation1Included, newValue: value, nameof(IsMutation1Included));
                RefreshEnabledCount();
            }
        }

        public bool IsMutation2Included
        {
            get => _isMutation2Included;
            set
            {
                Set(oldValue: ref _isMutation2Included, newValue: value, nameof(IsMutation2Included));
                RefreshEnabledCount();
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

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.SubscribeOnUIThread(subscriber: this);
            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(subscriber: this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public Task PublishEnabledCountAsync()
        {
            return _eventAggregator.PublishOnBackgroundThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Mutation,
                Count = EnabledCount
            });
        }

        public Task RestoreDefaultsAsync()
        {
            Mutation1 = 0.1M;
            Mutation2 = 0.1M;
            IsMutation1Included = false;
            IsMutation2Included = false;
            return Task.CompletedTask;
        }

        public Task HandleAsync(PresetResetRequestedEvent message, CancellationToken cancellationToken)
        {
            return Execute.OnUIThreadAsync(RestoreDefaultsAsync);
        }

        private void RefreshEnabledCount()
        {
            short count = default;
            if (IsMutation1Included)
                count++;
            if (IsMutation2Included)
                count++;

            EnabledCount = count;
        }
    }
}
