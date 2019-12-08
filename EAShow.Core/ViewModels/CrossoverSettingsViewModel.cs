using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Abstractions.Interfaces;
using EAShow.Core.Core.Events;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class CrossoverSettingsViewModel : Screen, IPreset
    {
        private byte _enabledCount;
        private bool _isCrossover1Included;
        private bool _isCrossover2Included;
        private List<Crossovers> _crossoverInts;
        private Crossovers _crossover1;
        private Crossovers _crossover2;

        private readonly IEventAggregator _eventAggregator;

        public byte EnabledCount
        {
            get => _enabledCount;
            private set => Set(oldValue: ref _enabledCount, newValue: value, nameof(EnabledCount));
        }

        public bool IsCrossover1Included
        {
            get => _isCrossover1Included;
            set
            {
                Set(oldValue: ref _isCrossover1Included, newValue: value, propertyName: nameof(IsCrossover1Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);

                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;
            }
        }

        public bool IsCrossover2Included
        {
            get => _isCrossover2Included;
            set
            {
                Set(oldValue: ref _isCrossover2Included, newValue: value, propertyName: nameof(IsCrossover2Included));
                NotifyTask.Create(asyncAction: PublishEnabledCount);

                if (value)
                    EnabledCount++;
                else
                    EnabledCount--;
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
            set => Set(oldValue: ref _crossoverInts, newValue: value, propertyName: nameof(CrossoverInts));
        }

        public CrossoverSettingsViewModel(IEventAggregator eventAggregator)
        {
            CrossoverInts = new List<Crossovers>(collection: EnumHelper.GetValuesAsReadOnlyCollection<Crossovers>());
            _eventAggregator = eventAggregator;
        }

        public async Task PublishEnabledCount()
        {
            await _eventAggregator.PublishOnUIThreadAsync(message: new PresetEnabledCountChangedEvent
            {
                Preset = Presets.Crossover,
                Count = EnabledCount
            });
        }
    }
}
