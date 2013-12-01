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

namespace ControlProject
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //DiagramChildWindows diagramchildwindows = new DiagramChildWindows(3);
            
            //diagramchildwindows.Show();

            FloatableWindow tempFW = new FloatableWindow();//TheTemplatedOne
            //f1.ShowDialog();
            tempFW.DialogResult = true;
            tempFW.Width = this.LayoutRoot.ActualWidth * 0.9;
            tempFW.Height = this.LayoutRoot.ActualHeight * 0.9;
            tempFW.Title = "";                           //窗口标题
            tempFW.HasCloseButton = true;                            //是否显示X按钮
            tempFW.ParentLayoutRoot = this.LayoutRoot;                   //父容器可以是Gird、Canvas等 
            tempFW.Content = new DiagramChatControl(3) ;  //窗口内容，可以是文字，也可以是UserControl等
            tempFW.ResizeMode = ResizeMode.CanResize;
            tempFW.ShowDialog();
            //f.Height = 100;
            //f.Width = 100;
            //f.Background = new SolidColorBrush(Colors.Yellow);
            //f.ShowDialog();
        }
    }
}
