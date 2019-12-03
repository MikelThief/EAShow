using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAShow.Core.Core.Models;

namespace EAShow.Core.Helpers
{
    public class EnumHelper
    {
        public static ReadOnlyCollection<Crossovers> AllCrossoversValues()
        {
            return Array.AsReadOnly((Crossovers[])Enum.GetValues(typeof(Crossovers)));
        }
    }
}
