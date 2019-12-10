using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace EAShow.Core.ViewModels
{
    public class EAViewModel : Screen
    {
        private short _index;

        public short Index
        {
            get => _index;
            set => Set(oldValue: ref _index, newValue: value, propertyName: nameof(Index));
        }
    }
}
