using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileExplorer_MichalRac.Commands
{
    [ValueConversion(typeof(bool), typeof(SortBy))]
    public class SortByBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SortBy casted = SortBy.Name;

            switch (parameter)
            {
                case "n":
                    casted = SortBy.Name;
                    break;
                case "s":
                    casted = SortBy.Size;
                    break;
                case "e":
                    casted = SortBy.Extension;
                    break;
                case "m":
                    casted = SortBy.ModifyDate;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return (SortBy)value == casted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
                switch (parameter)
                {
                    case "n": return SortBy.Name;
                    case "s": return SortBy.Size;
                    case "e": return SortBy.Extension;
                    case "m": return SortBy.ModifyDate;
                    default: throw new NotImplementedException();
                }
            return null;
        }
    }
}
