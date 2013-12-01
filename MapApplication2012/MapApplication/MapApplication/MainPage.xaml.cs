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
using System.ServiceModel.DomainServices.Client;

namespace MapApplication
{
    public partial class MainPage : Page
    {
        string CustomerID;
        string UserID;
        public MainPage()
        {
            CheckLogin();
            InitializeComponent();
          
        }

        void CheckLogin()
        {
            try
            {
                if (!System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("CustomerID"))
                {
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(
                           "Default.aspx", UriKind.Relative), "_self");
                    return;
                }


                CustomerID = System.Windows.Browser.HtmlPage.Document.QueryString["CustomerID"];
                UserID = System.Windows.Browser.HtmlPage.Document.QueryString["UserID"];
                // MessageBox.Show(CustomerID);

                MapApplication.Web.DbContext context = new Web.DbContext();
                InvokeOperation<bool> inv = context.IsUserLogin(CustomerID);
                inv.Completed += (s, a) =>
                {
                    if (!inv.Value)
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(
                            "Default.aspx", UriKind.Relative), "_self");
                    }
                    else
                    {
                        this.sSHMC_MapControl1.Initial(CustomerID,UserID);
                    }


                };


            }
            catch(Exception ex)
            { 
               MessageBox.Show(ex.Message) ;}
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
          //  CheckLogin();
        }

        private void sSHMC_MapControl1_OnClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Report.xaml?userid="+this.UserID, UriKind.Relative));
       //     System.Windows.Navigation.NavigationService.Navigate();
        }
    }
}
