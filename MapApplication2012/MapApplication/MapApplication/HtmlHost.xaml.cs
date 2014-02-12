using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MapApplication
{
    public partial class HtmlHost : UserControl
    {
        private const string IFRAME =
         @"<iframe height=""100%"" width=""100%"" marginheight=""1"" marginwidth=""1"" src=""{0}""></iframe>";

        private const string ATTR_INNER_HTML = "innerHTML";
        private const string ATTR_LEFT = "left";
        private const string ATTR_TOP = "top";
        private const string ATTR_WIDTH = "width";
        private const string ATTR_HEIGHT = "height";
        private const string ATTR_VISIBILITY = "visibility";
        private const string VISIBLE = "visible";
        private const string HIDDEN = "hidden";
        private const string PX = "{0}px";
        private HtmlElement _div;

        private double _width, _height;

        public static DependencyProperty HostDivProperty = DependencyProperty.Register(
            "HostDiv",
            typeof(string),
            typeof(HtmlHost),
            null);

        public string HostDiv
        {
            get { return GetValue(HostDivProperty).ToString(); }

            set
            {
                SetValue(HostDivProperty, value);
                if (!DesignerProperties.IsInDesignTool)
                {
                    _div = HtmlPage.Document.GetElementById(value);
                }
            }
        }

        public static DependencyProperty UrlProperty = DependencyProperty.Register(
            "Url",
            typeof(string),
            typeof(HtmlHost),
            null);

        public string Url
        {
            get { return GetValue(UrlProperty).ToString(); }

            set
            {
                SetValue(UrlProperty, value);
                if (!DesignerProperties.IsInDesignTool)
                {
                    _div.SetProperty(ATTR_INNER_HTML, string.Format(IFRAME, value));
                }
            }
        }

        public HtmlHost()
        {
            InitializeComponent();

            
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }
            Loaded += (o, e) =>
            {
                _width = (Width==Double.NaN)?ActualWidth:Width;
                _height = (Height==Double.NaN)?ActualWidth:Height;
                if (_div != null)
                {
                    Show();
                }
            };

        }


        public void SetHostDivVisible(bool IsVisible)
        {
            _div.SetAttribute(ATTR_VISIBILITY, IsVisible ? VISIBLE : HIDDEN);
        }
        public void Show()
        {
            _div.RemoveStyleAttribute(ATTR_VISIBILITY);
            _div.SetStyleAttribute(ATTR_VISIBILITY, VISIBLE);
          // Application.Current.Host.Content.Resized += Content_Resized;
            LayoutRoot.SizeChanged += LayoutRoot_SizeChanged;
            _Resize();
        }

     //   bool CanResize = false;
        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Width - _width) > 30 || Math.Abs(e.NewSize.Height - _height) > 30 || Double.IsNaN(_width)|| Double.IsNaN(_height) )
            {
              //  CanResize = true;
                _width = e.NewSize.Width;
                _height = e.NewSize.Height;
              _Resize();
            }
            //else
            //    CanResize = false;
         
         

           
        }

        private void Content_Resized(object sender, System.EventArgs e)
        {
          
            _Resize();
        }
        private void _Resize()
        {
            var gt = LayoutRoot.TransformToVisual(Application.Current.RootVisual);
            var offset = gt.Transform(new Point(0, 0));
            _div.RemoveStyleAttribute(ATTR_LEFT);
            _div.RemoveStyleAttribute(ATTR_TOP);
            _div.RemoveStyleAttribute(ATTR_WIDTH);
            _div.RemoveStyleAttribute(ATTR_HEIGHT);

            _div.SetStyleAttribute(ATTR_LEFT, string.Format(PX, offset.X));
            _div.SetStyleAttribute(ATTR_TOP, string.Format(PX, offset.Y));
            _div.SetStyleAttribute(ATTR_WIDTH, string.Format(PX, LayoutRoot.ActualWidth));
            _div.SetStyleAttribute(ATTR_HEIGHT,
                                    string.Format(PX, LayoutRoot.ActualHeight));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _div.SetStyleAttribute(ATTR_VISIBILITY, "hidden");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Content_Resized(null, null);
        }

    }
}
