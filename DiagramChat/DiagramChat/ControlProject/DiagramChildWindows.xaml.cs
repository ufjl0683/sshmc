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
using Visifire.Charts;
using System.ServiceModel.DomainServices.Client;
using MapApplication.Web;
using System.IO;
using System.Text;
using System.Windows.Browser;

namespace ControlProject
{
    public partial class DiagramChildWindows : ChildWindow
    {

        MapApplication.Web.DbContext db = new MapApplication.Web.DbContext();
        public DiagramChildWindows(int SensorID)
        {
            InitializeComponent();

            LoadData(System.DateTime.Now.Date.AddDays(0), SensorID);
          //  CreateChart();
        }

        private void LoadData(DateTime dt,int SensorID)
        {
            EntityQuery<tblTC10MinDataLog> qry = from n in db.GetTblTC10MinDataLogQuery()
                                                 where n.TIMESTAMP >= dt && n.TIMESTAMP < dt.AddDays(1) && n.SENSOR_ID == SensorID && n.ISVALID=="Y"
                                                  orderby n.TIMESTAMP select n;

            LoadOperation<tblTC10MinDataLog> lo = db.Load<tblTC10MinDataLog>(qry);

            lo.Completed += (s, a) =>
                {

                    if (lo.Error != null)
                    {
                        MessageBox.Show(lo.Error.Message);
                        return;
                    }

                    db.Load<tblSensor>(db.GetTblSensorQuery().Where(dd => dd.SENSOR_ID == SensorID)).Completed += (ss, aa)
                        =>
                        {
                            if ((ss as LoadOperation).Error != null)
                            {

                                MessageBox.Show((ss as LoadOperation).Error.Message);
                                return;
                            }

                            //tblTC10MinDataLog data = new tblTC10MinDataLog();
                            //in db.tblTC10MinDataLogs
                            int vcnt = 0;
                            if (db.tblSensors.FirstOrDefault().SENSOR_TYPE == "TILT")
                                vcnt = 2;
                            else
                                vcnt = 3;
                            for (int i = 1; i <= vcnt; i++)
                            {
                                CreateChart(i);
                            }
                        };
                        

                    
                };
        }

        public void CreateChart(int valueid)
        {
            
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Spline;
                dataSeries.Color = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                dataSeries.LightingEnabled = true;
                dataSeries.SelectionEnabled = false;
                dataSeries.MovingMarkerEnabled = true;

                tblSensor info = db.tblSensors.FirstOrDefault();

                if (info != null)
                {
                    //MessageBox.Show(info.SENSOR_TYPE);
                }

                int numberOfDataPoints = db.tblTC10MinDataLogs.Count; ;

                double max = 0;
                double min = 0;
                
            Title title = new Visifire.Charts.Title();

                foreach (tblTC10MinDataLog data in db.tblTC10MinDataLogs)
                {
                    DataPoint dataPoint = new DataPoint();
                    tblSensor sensor = new tblSensor();

                    
                    
                    
                    

                    switch (valueid)
                    {
                        case 1:
                            dataPoint.YValue = (double)data.VALUE0;
                            
                            if(data.DEGREE==0)
                                dataPoint.Color = new SolidColorBrush(Colors.Green);
                            if (data.DEGREE == 1)
                            dataPoint.Color = new SolidColorBrush(Colors.Yellow);
                            if (data.DEGREE == 2)
                                dataPoint.Color = new SolidColorBrush(Colors.Red);
                            //data.EXECUTION_MODE
                            max = (double)db.tblTC10MinDataLogs.Max(n => n.VALUE0);
                            min = (double)db.tblTC10MinDataLogs.Min(n => n.VALUE0);
                            title.Text = data.tblSensor.SENSOR_NAME + "-X軸";
                            break;
                        case 2:
                            dataPoint.YValue = (double)data.VALUE1;
                            if(data.DEGREE==0)
                                dataPoint.Color = new SolidColorBrush(Colors.Green);
                            if (data.DEGREE == 1)
                            dataPoint.Color = new SolidColorBrush(Colors.Yellow);
                            if (data.DEGREE == 2)
                                dataPoint.Color = new SolidColorBrush(Colors.Red);
                            max = (double)db.tblTC10MinDataLogs.Max(n => n.VALUE1);
                            min = (double)db.tblTC10MinDataLogs.Min(n => n.VALUE1);
                            title.Text = data.tblSensor.SENSOR_NAME + "-Y軸";
                            break;
                        case 3:
                            dataPoint.YValue = (double)data.VALUE2;
                            if(data.DEGREE==0)
                                dataPoint.Color = new SolidColorBrush(Colors.Green);
                            if (data.DEGREE == 1)
                            dataPoint.Color = new SolidColorBrush(Colors.Yellow);
                            if (data.DEGREE == 2)
                                dataPoint.Color = new SolidColorBrush(Colors.Red);
                            max = (double)db.tblTC10MinDataLogs.Max(n => n.VALUE2);
                            min = (double)db.tblTC10MinDataLogs.Min(n => n.VALUE2);
                            title.Text = data.tblSensor.SENSOR_NAME + "-Z軸";
                            break;
                    }
                    
                    //dataPoint.YValue = (double)data.VALUE0;
                    dataSeries.DataPoints.Add(dataPoint);

                }
                Visifire.Charts.Chart chart = new Visifire.Charts.Chart();
                //switch (valueid)
                //{
                //    case 0:
                //        chart = chart1;
                //        break;
                //}


                

                Axis yaxis = new Axis();
                yaxis.AxisMaximum = max;
                yaxis.AxisMinimum = min;
                chart.AxesY.Add(yaxis);

                chart.Titles.Add(title);

                chart.ScrollingEnabled = false;
                chart.IndicatorEnabled = true;
                chart.AnimationEnabled = true;
                chart.AnimatedUpdate = true;
                chart.ZoomingEnabled = true;
                chart.ShadowEnabled = true;
                chart.Series.Add(dataSeries);
                chart.Theme = "Theme3";
                chart.Height = LayoutRoot.ActualHeight / 3;
                Thickness tk = new Thickness() { 
                Bottom = 1,
                Top = 1
                };
                chart.Margin = tk;
                //stackpanel.Orientation = Orientation.Horizontal;
                stackpanel.Children.Add(chart);
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;


        }

        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Navigate(new Uri("DownLoadForm.aspx" , UriKind.Relative));
        }
    }
}

