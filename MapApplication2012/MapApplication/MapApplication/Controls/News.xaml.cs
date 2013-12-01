using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class News : UserControl
    {
        public News()
        {
            InitializeComponent();
            //this.webbrowser.Navigate(new Uri("http://192.192.161.4/sshmc/News.html",UriKind.Absolute));

            //this.webbrowser.LoadCompleted += (s, a) =>
            //    {
            //        InsertNews();
            //    };
          
            
           
                
        }


       

        //void InsertNews()
        //{
        //    DataService.SSHMCDataServiceClient client = new DataService.SSHMCDataServiceClient();

        //    client.GetDisasterInfoAsync();
        //    client.GetDisasterInfoCompleted += (s, a) =>
        //    {
        //        if (a.Error != null)
        //        {
        //            MessageBox.Show(a.Error.Message);
        //            return;
        //        }
        //        System.Collections.ObjectModel.ObservableCollection<DataService.tblPre_disasterNotified> preDisasterInfos = a.Result;
        //        foreach (DataService.tblPre_disasterNotified info in preDisasterInfos)
        //        {
        //            string classtype = "side";

        //            if (info.PRE_ADMONISH_CLASS.Contains("雨"))
        //                classtype = "main2";
        //            else if (info.PRE_ADMONISH_CLASS.Contains("地震") || info.PRE_ADMONISH_CLASS.Contains("颱風"))
        //                classtype = "side";

        //            this.webbrowser.InvokeScript("InsertNews", new string[] { info.TITLE, ((DateTime)info.TIMESTAMP).ToString("yyyy/MM/dd hh:mm:ss"), info.CONTENT, classtype });
        //        }
        //    };
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // InertNews();
        }

        private void webbrowser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          //  InertNews();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var rs = Application.GetResourceStream(new Uri("Controls/News.html", UriKind.Relative));
            //using (StreamReader sr = new StreamReader(rs.Stream))
            //{
            //    this.webbrowser.NavigateToString(sr.ReadToEnd());
            //}
        }

        private void BlockArrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (this.Parent as Grid).Children.Remove(this); 
        }

        
          
			// TODO: 在此新增事件處理常式執行項目。
         
    }
}
