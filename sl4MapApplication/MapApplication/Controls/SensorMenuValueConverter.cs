using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using MapApplication.Web;

namespace MapApplication.Controls
{
    public class SensorMenuValueConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int valueinx = System.Convert.ToInt32(parameter);
            
           vwSensorDegree SensorData=value as  vwSensorDegree;
           if (SensorData.CURRENT_DEGREE == -1 || SensorData.ISVALID == "N")
               return "NA";
           else
           {
              // double retvalue = 0;
               switch(valueinx )
               {
                   case 0:
                        return SensorData.VALUE0;
                   case 1:
                        return SensorData.VALUE1;
                   case 2:
                        return SensorData.VALUE2;
                   default:
                        return SensorData.VALUE0;
               }
           }
           // throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
