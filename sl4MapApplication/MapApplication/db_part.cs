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

namespace MapApplication.Web
{
    public partial class vwSensorDegree
    {

       public string SENSOR_TYPE_CHAR
        {
            get
            {
                switch (this.SENSOR_TYPE.ToUpper().Trim())
                {
                    case "TILE":
                        return "T";
                    case "GPS":
                        return "G";
                    case "CAM":
                        return "C";
                    case "LASER":
                        return "L";
                    default:
                        return "?";

                }
            }
        }
       public SolidColorBrush AlarmColorBright
       {
           get
           {
               switch (this.CURRENT_DEGREE)
               {
                   case 0:
                       return null;
                   case 1:
                       return new SolidColorBrush(Colors.Yellow);
                   case 2:
                       return new SolidColorBrush(Colors.Orange);
                   case 3:
                       return new SolidColorBrush(Colors.Red);
                   default:
                       return null;
               }
           }
       }
       public SolidColorBrush AlarmColorLight
       {
           get
           {
               Color color;
               switch (this.CURRENT_DEGREE)
               {
                   case 0:
                       return null;
                   case 1:
                       color = Colors.Yellow;
                       color.A = 0;
                       return new SolidColorBrush(color);
                   case 2:
                         color = Colors.Orange;
                         color.A = 0;
                       return new SolidColorBrush(color);
                   case 3:
                         color = Colors.Red;
                         color.A = 0;
                         return new SolidColorBrush(color);
                   default:
                       return null;
               }
           }
       }

      
        
    }

    public partial class tblCCTV
    {
        int _cctv_inx = 0;
        public int CCTV_INX
        {
            get
            {
                return _cctv_inx;
            }
            set
            {
                _cctv_inx = value;
                this.RaisePropertyChanged("CCTV_INX");
            }
        }

    }
}
