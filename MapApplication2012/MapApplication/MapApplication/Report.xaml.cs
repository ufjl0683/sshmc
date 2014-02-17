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
using System.Windows.Navigation;

namespace MapApplication
{
    public partial class Report : Page
    {
        public Report()
        {
            InitializeComponent();
        }

        string UserID ;
        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          UserID=  this.NavigationContext.QueryString["userid"].ToString();

          DataService.SSHMCDataServiceClient client = new DataService.SSHMCDataServiceClient();
            client.GettblReportNotifiedCompleted+=(s,a)=>
                {
                    if (a.Error != null)
                     

                        return;


                    var q = from n in a.Result orderby n.TYPE, n.DATATIME  descending 
                            group n by new {n.SITE_ID,  n.SITE_NAME }     into g  
                            select new BindingData { SITE_NAME=g.Key.SITE_NAME,Count= g.Count(),
                            Items=g.ToArray() ,
                            SITE_ID=g.Key.SITE_ID
                            };

                    this.lstSite.ItemsSource =q ;
                   

                };
           client.GettblReportNotifiedAsync(UserID);

        
        }

        WrapPanel wrap;
        private void WrapPanel_Loaded(object sender, RoutedEventArgs e)
        {
            wrap = sender as WrapPanel;
            wrap.Height = lstReport.ActualHeight;
            wrap.Width = lstReport.ActualWidth;
        }

        private void ListBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             
                if (wrap != null)
                {
                    wrap.Width = lstReport.ActualWidth;
                    wrap.Height = lstReport.ActualHeight;
                }
               
             
        }
        
        private void lstReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataService.vwReportNotified vwNotifier=lstReport.SelectedItem as   DataService.vwReportNotified;
            string url = "https://docs.google.com/viewer?url=" + vwNotifier.URL + "&embedded=true";
            url = vwNotifier.URL;
           // System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url, UriKind.Absolute), "_blank", "toolbar=no,location=no,status=no,menubar=no,resizable=yes,location");
       
          this.NavigationService.Navigate(  new Uri("/Controls/PDFViewer.xaml?url="+url,UriKind.Relative));
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
            base.OnNavigatedFrom(e);
          
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    

    }

    public class BindingData
    {
        public string SITE_NAME { get; set; }
        public string SITE_ID { get; set; }
        public int Count { get; set; }
       public DataService.vwReportNotified[] Items{get;set;}
    }
}
