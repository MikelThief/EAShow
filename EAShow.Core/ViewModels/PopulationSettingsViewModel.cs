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

        public bool IsPopulation1Included
        {
            get => _isPopulation1Included;
            set
            {
                Set(oldValue: ref _isPopulation1Included, newValue: value, nameof(IsPopulation1Included));
                RefreshEnabledCount();
            }
        }

        public bool IsPopulation2Included
        {
            get => _isPopulation2Included;
            set
            {
                Set(oldValue: ref _isPopulation2Included, newValue: value, nameof(IsPopulation2Included));
                RefreshEnabledCount();
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

        public Task PublishEnabledCountAsync()
        {
            return _eventAggregator.PublishOnUIThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Population,
                Count = EnabledCount
            });
        }

        public Task RestoreDefaultsAsync()
        {
            Population1 = 100;
            Population2 = 100;
            /* Compared to Mutation, some shit is going on here. There are no differences!
               This is not expected to work, but it does! */

            IsPopulation1Included = false;
            IsPopulation2Included = false;
            RefreshEnabledCount();
            return Task.CompletedTask;
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

        public Task HandleAsync(PresetResetRequestedEvent message, CancellationToken cancellationToken)
        {
            return RestoreDefaultsAsync();
        }

        private void RefreshEnabledCount()
        {
            short count = default;
            if (IsPopulation1Included)
                count++;
            if (IsPopulation2Included)
                count++;

            EnabledCount = count;
        }
    }
}
