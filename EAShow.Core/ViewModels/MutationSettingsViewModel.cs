using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace EAShow.Core.ViewModels
{
    public class MutationSettingsViewModel : Screen
    {
        private bool _isCoefficient1Included;
        private bool _isCoefficient2Included;
        private double? _coefficient1;
        private double? _coefficient2;

        public bool IsCoefficient1Included
        {
            get => _isCoefficient1Included;
            set => Set(oldValue: ref _isCoefficient1Included, newValue: value, nameof(IsCoefficient1Included));
        }

        public bool IsCoefficient2Included
        {
            get => _isCoefficient2Included;
            set => Set(oldValue: ref _isCoefficient2Included, newValue: value, nameof(IsCoefficient2Included));
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
    }
}
