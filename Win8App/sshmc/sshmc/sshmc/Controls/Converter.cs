using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
namespace sshmc.Controls
{
    public class DegreeColorConverter : IValueConverter
    {

        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    int degree;
        //    if (!Int32.TryParse(value.ToString(), out degree))
        //        return null;

        //    switch (degree)
        //    {
        //        case 0:
        //            return new SolidColorBrush(Color.FromArgb(255, 0, 200, 0));
        //        case 1:
        //            return new SolidColorBrush(Colors.Yellow);
        //        case 2:
        //            return new SolidColorBrush(Colors.Orange);
        //        case 3:
        //            return new SolidColorBrush(Colors.Red);
        //        default:
        //            return new SolidColorBrush(Colors.Gray);
        //    }
        //    // throw new NotImplementedException();
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int degree;
            if (!Int32.TryParse(value.ToString(), out degree))
                return null;

            switch (degree)
            {
                case 0:
                    return new SolidColorBrush(Color.FromArgb(255, 0, 200, 0));
                case 1:
                    return new SolidColorBrush(Colors.Yellow);
                case 2:
                    return new SolidColorBrush(Colors.Orange);
                case 3:
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
          //  throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    public class DegreeAlarmColorBrightConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{

        //    throw new NotImplementedException();
        //  //  int CURRENT_DEGREE = System.Convert.ToInt32(value);
            
        //    //System.Windows.Graphic RadialGradientBrush gb = parameter as RadialGradientBrush;

        //    //switch (CURRENT_DEGREE)
        //    //{
        //    //    case 0:

        //    //        //return gb;
        //    //        RadialGradientBrush rb = new RadialGradientBrush();
        //    //        //rb.GradientStops.Add(new GradientStop() { Color = Colors.Transparent, Offset = 0 });
        //    //        return rb;
        //    //    case 1:
        //    //        foreach (GradientStop s in gb.GradientStops)
        //    //        {
        //    //            Color c = Colors.Yellow;
        //    //            c.A = s.Color.A;
        //    //            s.Color = c;
        //    //        }
        //    //        return gb;
        //    //    case 2:
        //    //        foreach (GradientStop s in gb.GradientStops)
        //    //        {
        //    //            Color c = Colors.Orange;
        //    //            c.A = s.Color.A;
        //    //            s.Color = c;
        //    //        }
        //    //        return gb;
        //    //    case 3:
        //    //        foreach (GradientStop s in gb.GradientStops)
        //    //        {
        //    //            Color c = Colors.Red;
        //    //            c.A = s.Color.A;
        //    //            s.Color = c;
        //    //        }
        //    //        return gb;


        //    //    default:
        //    //        return new RadialGradientBrush();
        //  //  }


        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
