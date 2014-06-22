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

namespace MapApplication.MapControls
{
    public class DegreeColorConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int degree;
            if (!Int32.TryParse(value.ToString(),out degree))
                return null;

            switch (degree)
            {
                case 0:
                    return new SolidColorBrush(Color.FromArgb(255,0,200,0));
                case 1:
                    return new SolidColorBrush(Colors.Yellow);
                case 2:
                    return new SolidColorBrush(Colors.Orange);
                case 3:
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
             // throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HasSurveyDataToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            if (value!=null )
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
          //  throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DegreeAlarmColorBrightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int CURRENT_DEGREE = System.Convert.ToInt32(value);
            RadialGradientBrush gb = parameter as RadialGradientBrush;
            
            switch (CURRENT_DEGREE)
            {
                case 0:
                
                    //return gb;
                    RadialGradientBrush rb= new RadialGradientBrush();
                    //rb.GradientStops.Add(new GradientStop() { Color = Colors.Transparent, Offset = 0 });
                    return rb;
                case 1:
                    foreach (GradientStop s in gb.GradientStops)
                    {
                        Color c = Colors.Yellow;
                        c.A = s.Color.A;
                        s.Color = c;
                    }
                    return gb;
                case 2:
                    foreach (GradientStop s in gb.GradientStops)
                    {
                        Color c = Colors.Orange;
                        c.A = s.Color.A;
                        s.Color = c;
                    }
                    return gb;
                case 3:
                   foreach (GradientStop s in gb.GradientStops)
                    {
                        Color c = Colors.Red;
                        c.A = s.Color.A;
                        s.Color = c;
                    }
                    return gb;
                 
                  
                default:
                    return new RadialGradientBrush();
            }

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
