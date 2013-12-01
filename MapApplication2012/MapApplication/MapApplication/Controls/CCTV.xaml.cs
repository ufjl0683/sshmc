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
using MapApplication.Web;
using System.ComponentModel;

namespace MapApplication.Controls
{
    public partial class CCTV : UserControl
    {
        WebClient client = new WebClient();
        bool IsBeginRead = false;
        bool ISExit = false;
        Random rnd = new Random();
        public CCTV()
        {
            InitializeComponent();

        }

      
        int imginx = 0;
        public void SwitchCCTV ()
        {
            if (this.DataContext == null || this.IsBeginRead)
                return;
           
            BeginReadCCTV();
           // this.Visibility = System.Windows.Visibility.Visible;
        }
        public void DisMiss()
        {
            this.IsBeginRead = false;
            this.DataContext = null;
           // this.Visibility = Visibility.Collapsed;
        }
         void BeginReadCCTV()
        {
            if (this.DataContext == null  )
                return;
            IsBeginRead = true;
            tblCCTV   cctvinfo= this.DataContext as tblCCTV;
            client=new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(new Uri("http://192.192.161.3/"+cctvinfo.REF_CCTV_ID.Trim()+".jpg?"+rnd.Next(), UriKind.Absolute));
            imginx = (imginx+1)% 90000;
            cctvinfo.CCTV_INX = imginx;
          //  cctvinfo.CCTV_INX = (cctvinfo.CCTV_INX+1) % 90000;
        }
        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        bmp.SetSource(e.Result);
                        imgCCTV.Source = bmp;

                    }
                    catch
                    {

                    }
                    finally
                    {
                        if (!ISExit)
                            BeginReadCCTV();
                    }
                }
                );

            }
            catch
            { ;}
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
             if(!IsDesignTime())
                BeginReadCCTV();
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ISExit = true;
        }

        public bool IsDesignTime()
        {
            return DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual);
        }
    }
}
