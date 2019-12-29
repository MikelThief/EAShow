using EAShow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace EAShow.Infrastructure.Converters
{
    public class CrossoversEnumToResourceConverter : IValueConverter
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
                        message: nameof(CrossoversEnumToResourceConverter) + "cannot process the value.");
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
