using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
 
using System.Windows.Input;
 
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using sshmc.Service;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.Foundation;
namespace sshmc.Controls
{
    public partial class CCTV : UserControl
    {   
        HttpClient httpClient = new HttpClient();
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
       async  void BeginReadCCTV()
        {
            
            IsBeginRead = true;
            ISExit = false;
            tblCCTV cctvinfo=null;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                   () =>
                   {
                       cctvinfo = this.DataContext as tblCCTV;
                   });

             
           // client=new HttpClient();
            while (!ISExit)
            {
                try
                {
                    Uri uri = new Uri("http://192.192.161.3/" + cctvinfo.REF_CCTV_ID.Trim() + ".jpg?" + rnd.Next(), UriKind.Absolute);

                    using (httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(0.5);
                        var contentBytes = await httpClient.GetByteArrayAsync(uri);

                   
                        var ims =  new InMemoryRandomAccessStream();

                        var dataWriter = new DataWriter(ims);
                        dataWriter.WriteBytes(contentBytes);
                        await dataWriter.StoreAsync();
                       //ims.seak 0
                     ims.Seek(0 );
                       
                       await Dispatcher.RunAsync( Windows.UI.Core.CoreDispatcherPriority.Normal,()=>
                            {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.SetSource(ims );
                       

                        this.imgCCTV.Source = bitmap;
                        imginx = (imginx + 1) % 90000;
                        this.RunFrameRate.Text = imginx.ToString();
                            });
                    }
                    
                }
                catch (Exception ex)
                {
                 //   this.textBlock1.Text = ex.Message;
                }
                // BitmapImage img = new BitmapImage();
                // img.SetSource(stream);

              

               
            }
        
        }

       
        //void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
        //        Dispatcher.BeginInvoke(() =>
        //        {
        //            try
        //            {
        //                bmp.SetSource(e.Result);
        //                imgCCTV.Source = bmp;

        //            }
        //            catch
        //            {

        //            }
        //            finally
        //            {
        //                if (!ISExit)
        //                    BeginReadCCTV();
        //            }
        //        }
        //        );

        //    }
        //    catch
        //    { ;}
        //}

       private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
       {
           if (!IsDesignTime()  && this.DataContext !=null)
           {
               IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
                 (workItem) =>
            {


              BeginReadCCTV();
            });
            
           }

       }

       private void UserControl_Unloaded(object sender, RoutedEventArgs e)
       {
           ISExit = true;
       }

       public bool IsDesignTime()
       {
           return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
       }
    }
}
