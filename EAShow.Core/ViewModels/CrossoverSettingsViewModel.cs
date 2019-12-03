using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace EAShow.Core.ViewModels
{
    public class CrossoverSettingsViewModel : Screen
    {
        private bool _isCrossover1Included;
        private bool _isCrossover2Included;

        public bool IsCrossover1Included
        {
            get => _isCrossover1Included;
            set => Set(oldValue: ref _isCrossover1Included, newValue: value, nameof(IsCrossover1Included));
        }

        public bool IsCrossover2Included
        {
            get => _isCrossover2Included;
            set => Set(oldValue: ref _isCrossover2Included, newValue: value, nameof(IsCrossover2Included));
        }
    }
}
