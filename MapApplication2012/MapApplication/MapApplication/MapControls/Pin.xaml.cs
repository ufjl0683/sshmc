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

namespace MapApplication.MapControls
{
    public partial class Pin : UserControl
    {
        public Pin()
        {
            InitializeComponent();
        }

        public void SetBlind()
        {
            Storyboard board = this.Resources["stbBlind"] as Storyboard;
            board.Stop();
            board.Begin();
        }
        public void StopBlind()
        {
            Storyboard board = this.Resources["stbBlind"] as Storyboard;
            board.Stop();
        }

        public void SetNotifyDataVisibility(bool visible)
        {
            if (visible)
                this.notifyData.Visibility = System.Windows.Visibility.Visible;
            else
                this.notifyData.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
