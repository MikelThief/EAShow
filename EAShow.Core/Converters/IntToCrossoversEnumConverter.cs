using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;
using EAShow.Shared.Models;
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
                case Crossovers.Uniform:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "CrossoversEnum_Uniform");
                case Crossovers.OnePoint:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "CrossoversEnum_OnePoint");
                case Crossovers.ThreeParent:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "CrossoversEnum_ThreeParent");
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
