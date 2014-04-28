using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorldCupGuide.Models;

namespace WorldCupGuide.Converters
{
    public class CountryNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string code = value.ToString() ?? String.Empty;
            if (Constants.CountryCode.ContainsKey(code.ToUpper()))
            {
                return Constants.CountryCode[code];
            }
            else
            {
                return code;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
