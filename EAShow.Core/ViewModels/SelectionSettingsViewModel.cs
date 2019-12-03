using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;

namespace EAShow.Core.ViewModels
{
    public class SelectionSettingsViewModel : Screen
    {
        private bool _isSelection1Included;
        private bool _isSelection2Included;
        private List<Selections> _SelectionInts;
        private Selections _Selection1;
        private Selections _Selection2;

        public bool IsSelection1Included
        {
            get => _isSelection1Included;
            set => Set(oldValue: ref _isSelection1Included, newValue: value, propertyName: nameof(IsSelection1Included));
        }

        public bool IsSelection2Included
        {
            get => _isSelection2Included;
            set => Set(oldValue: ref _isSelection2Included, newValue: value, propertyName: nameof(IsSelection2Included));
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

        public SelectionSettingsViewModel()
        {
            SelectionInts = new List<Selections>(collection: EnumHelper.GetValuesAsReadOnlyCollection<Selections>());
        }
    }
}
