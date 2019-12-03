using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;
using EAShow.Core.Views;

namespace EAShow.Core.Converters
{
    public class IntToCrossoversEnumConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((Crossovers)value)
            {
                case Crossovers.Both:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "CrossoversEnum_Both");
                case Crossovers.OnlyX:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "CrossoversEnum_OnlyX");
                case Crossovers.OnlyY:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "CrossoversEnum_OnlyY");
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(value), actualValue: value,
                        message: nameof(IntToCrossoversEnumConverter) + "cannot process the value.");
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
