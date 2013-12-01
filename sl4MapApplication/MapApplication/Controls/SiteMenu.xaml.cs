using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MapApplication.Controls
{
    public partial class SiteMenu : UserControl
    {
        public SiteMenu()
        {
            InitializeComponent();
        }

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.txtSiteName.Width= this.Width;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
           // this.border.BorderBrush = new SolidColorBrush(Colors.Purple);
            this.buldingPic.Visibility = System.Windows.Visibility.Visible;
        }

        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            this.buldingPic.Visibility = System.Windows.Visibility.Collapsed;
           // this.border.BorderBrush = null;
        }
    }
}
