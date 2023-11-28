using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyNote.Utils
{
    public class ContentTrimConverter : IValueConverter
    {
        // You can set your desired maximum length for content display
        private const int MaxContentLength = 50; // Adjust this value as needed

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string content)
            {
                if (content.Length > MaxContentLength)
                {
                    return content.Substring(0, MaxContentLength) + "...";
                }
                return content;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
