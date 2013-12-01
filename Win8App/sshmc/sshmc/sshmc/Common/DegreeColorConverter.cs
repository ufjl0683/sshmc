using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace sshmc.Common
{
   public   class DegreeColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           int degree=System.Convert.ToInt32(value);
           switch (degree)
           {
               case 0:
                   return new SolidColorBrush(Windows.UI.Colors.Green);
               case 1:
                   return new SolidColorBrush(Windows.UI.Colors.Yellow);
               case 2:
                   return new SolidColorBrush(Windows.UI.Colors.Orange);
               case 3:
                   return new SolidColorBrush(Windows.UI.Colors.Red);
               case -1:
                   return new SolidColorBrush(Windows.UI.Colors.Gray);
               default:
                   return new SolidColorBrush(Windows.UI.Colors.Purple);
           }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
