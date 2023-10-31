using System;
using System.Globalization;
using System.Windows.Data;

namespace Guages.ControLib.Converter
{
    public class RadiusToDiameterConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double dblVal = (double)value;

            return (dblVal * 2);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}