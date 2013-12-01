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
    public partial class MainPage : UserControl
    {
        string CustomerID;
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
                        this.sSHMC_MapControl1.Initial(CustomerID);
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
    }
}
