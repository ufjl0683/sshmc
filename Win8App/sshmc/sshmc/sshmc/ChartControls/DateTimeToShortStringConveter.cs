using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace sshmc.ChartControls
{
   public class DateTimeToShortStringConveter:IValueConverter
    {


        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime dt =Convert.ToDateTime( value) ;
            return dt.ToString("yyyy/MM/dd");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
