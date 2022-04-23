using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileExplorer_MichalRac.Commands
{
    [ValueConversion(typeof(bool), typeof(Direction))]
    public class DirectionBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Direction casted = Direction.Ascending;

            switch (parameter)
            {
                case "a":
                    casted = Direction.Ascending;
                    break;
                case "d":
                    casted = Direction.Descending;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return (Direction)value == casted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((bool)value == true)
                switch (parameter)
                {
                    case "a": return Direction.Ascending;
                    case "d": return Direction.Descending;
                    default: throw new NotImplementedException();
                }
            return null;
        }
    }
}
