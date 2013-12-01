using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace sshmc.Common
{
   public  class DegreeToImageSourceConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int degree = int.Parse(value.ToString());

            ImageSource imgsrc;
            
            switch(degree)
            {
                case 0:
                    imgsrc = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Images/greenpin.png"));
                    break;
                case 1:
                    imgsrc = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Images/yellowpin.png"));
                    break;
                case 2:
                    imgsrc = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Images/orangepin.png"));
                    break;
                case 3:
                    imgsrc = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Images/redpin.png"));
                    break;
                default:
                
                    imgsrc = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Images/graypin.png"));
                    break;
            }
            return imgsrc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
