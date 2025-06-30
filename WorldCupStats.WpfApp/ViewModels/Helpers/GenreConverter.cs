using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorldCupStats.Data.Models;

namespace WorldCupStats.WpfApp.ViewModels.Helpers
{
    class GenreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Genre genre)
            {
                return genre switch
                {
                    Genre.Men => "Men",
                    Genre.Women => "Women",
                    _ => genre.ToString(),
                };
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str switch
                {
                    "Men" => Genre.Men,
                    "Women" => Genre.Women,
                    _ => Genre.Men,
                };
            }
            return Genre.Men;
        }
    }
}
