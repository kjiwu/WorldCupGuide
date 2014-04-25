using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WorldCupGuide.Models;

namespace WorldCupGuide.Converters
{
    public class CountryFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string code = value.ToString() ?? String.Empty;
            if (!String.IsNullOrEmpty(code))
            {
                char[] codeInfo = code.ToCharArray();
                string uri = String.Format("/Assets/Images/CFlag/{0}/{1}.png", codeInfo[0], code.ToUpper());
                return new BitmapImage(new Uri(uri, UriKind.Relative));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
