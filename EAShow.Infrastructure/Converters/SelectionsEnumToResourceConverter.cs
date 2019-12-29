using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;
using EAShow.Shared.Models;

namespace EAShow.Infrastructure.Converters
{
    public class SelectionsEnumToResourceConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((Selections)value)
            {
                case Selections.Elite:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "SelectionsEnum_Elite");
                case Selections.Roulette:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "SelectionsEnum_Roulette");
                case Selections.Tournament:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "SelectionsEnum_Tournament");
                case Selections.StohasticUniversalSampling:
                    return ResourceLoader.GetForCurrentView().GetString(resource: "SelectionsEnum_StohasticUniversalSampling");
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(value), actualValue: value,
                        message: nameof(SelectionsEnumToResourceConverter) + "cannot process the value.");
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
