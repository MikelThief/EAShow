using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Abstractions.Interfaces;
using EAShow.Shared.Events;
using EAShow.Shared.Models;
using EAShow.Core.Helpers;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class CrossoverSettingsViewModel : Screen, IPreset
    {
        private short _enabledCount;
        private bool _isCrossover1Included;
        private bool _isCrossover2Included;
        private List<Crossovers> _crossoverInts;
        private Crossovers _crossover1;
        private Crossovers _crossover2;

        private readonly IEventAggregator _eventAggregator;

        public short EnabledCount
        {
            get => _enabledCount;
            private set
            {
                Set(oldValue: ref _enabledCount, newValue: value, nameof(EnabledCount));
                NotifyTask.Create(asyncAction: PublishEnabledCountAsync);
            }
        }

        public bool IsCrossover1Included
        {
            get => _isCrossover1Included;
            set
            {
                Set(oldValue: ref _isCrossover1Included, newValue: value, propertyName: nameof(IsCrossover1Included));
               RefreshEnabledCount();
            }
        }

        public bool IsCrossover2Included
        {
            get => _isCrossover2Included;
            set
            {
                Set(oldValue: ref _isCrossover2Included, newValue: value, propertyName: nameof(IsCrossover2Included));
                RefreshEnabledCount();
            }
        }

        public Crossovers Crossover1
        {
            get => _crossover1;
            set => Set(oldValue: ref _crossover1, newValue: value, propertyName: nameof(Crossover1));
        }

        public Crossovers Crossover2
        {
            get => _crossover2;
            set => Set(oldValue: ref _crossover2, newValue: value, propertyName: nameof(Crossover2));
        }

        public List<Crossovers> CrossoverInts
        {
            get => _crossoverInts;
            private set => Set(oldValue: ref _crossoverInts, newValue: value, propertyName: nameof(CrossoverInts));
        }

        public CrossoverSettingsViewModel(IEventAggregator eventAggregator)
        {
            CrossoverInts = new List<Crossovers>(collection: EnumHelper.GetValuesAsReadOnlyCollection<Crossovers>());
            _eventAggregator = eventAggregator;
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await RestoreDefaultsAsync();
            await base.OnInitializeAsync(cancellationToken);
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

        public async Task PublishEnabledCountAsync()
        {
            await _eventAggregator.PublishOnUIThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Crossover,
                Count = EnabledCount
            });
        }

        public Task RestoreDefaultsAsync()
        {
            Crossover1 = CrossoverInts[0];
            Crossover2 = CrossoverInts[0];
            IsCrossover1Included = false;
            IsCrossover2Included = false;
            return Task.CompletedTask;
        }

        public Task HandleAsync(PresetResetRequestedEvent message, CancellationToken cancellationToken)
        {
            return RestoreDefaultsAsync();
        }

        private void RefreshEnabledCount()
        {
            short count = default;
            if (IsCrossover1Included)
                count++;
            if (IsCrossover2Included)
                count++;

            EnabledCount = count;
        }
    }
}
