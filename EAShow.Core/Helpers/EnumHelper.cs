using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAShow.Shared.Models;

namespace EAShow.Core.Helpers
{
    public class EnumHelper
    {
        public static ReadOnlyCollection<T> GetValuesAsReadOnlyCollection<T>() where T : struct, IConvertible => Array.AsReadOnly(array: (T[])Enum.GetValues(enumType: typeof(T)));
    }
}
