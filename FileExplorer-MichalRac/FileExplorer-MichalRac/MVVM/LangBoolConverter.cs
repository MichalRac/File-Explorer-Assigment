namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(string))]
    public class LangBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((string) value == (string) parameter) 
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool) value == true)
                return (string) parameter;
            return null;
        }
    }
}
