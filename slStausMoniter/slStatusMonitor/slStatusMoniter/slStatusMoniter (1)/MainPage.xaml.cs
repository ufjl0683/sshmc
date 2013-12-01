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
using System.Windows.Media.Imaging;
using System.ServiceModel.DomainServices.Client;
namespace slStatusMoniter
{
    public partial class MainPage : UserControl
    {
        string pno = "S10100001";

        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        System.Collections.Generic.Dictionary<string, Sensor> hsSensors = new Dictionary<string, Sensor>();
        public MainPage()
        {
            InitializeComponent();

           
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("PNo"))
                this.pno = System.Windows.Browser.HtmlPage.Document.QueryString["PNo"];
            else
            {
                MessageBox.Show("Parameter PNo Not Found!");
                return;
            }

            this.img.Source = new BitmapImage(new Uri(string.Format("http://192.192.161.3/DEVICEPIC/{0}.jpg", pno), UriKind.Absolute));
            slStatusMoniter.Web.DomainService1 client = new Web.DomainService1();

            var qry = from n in client.GetVwSensorStatusQuery() where n.PNO == pno select n;

            LoadOperation<slStatusMoniter.Web.vwSensorStatus> lo = client.Load(qry);

            lo.Completed += (s, a) =>
            {
                if (lo.Error != null)
                {
                    MessageBox.Show(lo.Error.Message);
                    return; ;
                }

                IEnumerator<slStatusMoniter.Web.vwSensorStatus> ie = lo.Entities.GetEnumerator();


                while (ie.MoveNext())
                {
                    try
                    {
                        double x, y;
                        x = System.Convert.ToInt32(ie.Current.CX) * 770 / 13125;
                        y = System.Convert.ToInt32(ie.Current.CY) * 435 / 5175;
                        Sensor snr = new Sensor();
                        snr.SetDataContext(ie.Current);
                        this.LayoutRoot.Children.Add(snr);

                        snr.Width = 20;
                        snr.Height = 20;
                        snr.SetValue(Canvas.LeftProperty, x);
                        snr.SetValue(Canvas.TopProperty, y);
                        hsSensors.Add(ie.Current.CX + "_" + ie.Current.CY, snr);
                         
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                tmr.Interval = new TimeSpan(0, 0, 5);
                tmr.Tick += new EventHandler(tmr_Tick);
                tmr.Start();




            };



        }


        bool IsInTmr = false;

        void tmr_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            try
            {
                slStatusMoniter.Web.DomainService1 client = new Web.DomainService1();

                var qry = from n in client.GetVwSensorStatusQuery() where n.PNO == pno select n;

                LoadOperation<slStatusMoniter.Web.vwSensorStatus> lo = client.Load(qry);

                lo.Completed += (s, a) =>
                {


                    if (lo.Error != null)
                    {
                        MessageBox.Show(lo.Error.Message);
                        return; ;
                    }

                    IEnumerator<slStatusMoniter.Web.vwSensorStatus> ie = lo.Entities.GetEnumerator();


                    while (ie.MoveNext())
                    {
                        try
                        {



                            if (!hsSensors.ContainsKey(ie.Current.CX + "_" + ie.Current.CY))
                                return;

                            hsSensors[ie.Current.CX + "_" + ie.Current.CY].SetDataContext(ie.Current);



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }



                };

            }
            finally
            {
                IsInTmr = false;
            }


        }
    }
}
