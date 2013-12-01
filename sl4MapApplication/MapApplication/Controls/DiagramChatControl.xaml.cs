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
using System.Windows.Browser;
using Visifire.Charts;
using MapApplication.Web;
using System.ServiceModel.DomainServices.Client;

namespace MapApplication.Controls
{
  
    public partial class DiagramChatControl : UserControl
    {
        MapApplication.Web.DbContext db = new MapApplication.Web.DbContext();
        int sensorID = 0;
        System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();
        //Boolean _oddState = false;
        int vcnt = 0,IsTilt=0;
        string status = "Normal";///////////
        string Isvalid = "";

        public DiagramChatControl(int SensorID)
        {
            InitializeComponent();
            sensorID = SensorID;
            LoadData(System.DateTime.Now.Date.AddDays(0), sensorID);
            //_timer.Tick += new EventHandler(_timer_Tick);
            //_timer.Interval = TimeSpan.FromMinutes(10);


            LayoutRoot.Loaded += new RoutedEventHandler(LayoutRoot_Loaded);
          

        }

        void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }







        void _timer_Tick(object sender, EventArgs e)
        {
            LoadData(DateTime.Now, sensorID);
        }


        private void LoadData(DateTime dt, int SensorID)
        {

            stackpanel.Children.Clear();
            db = new DbContext();
            EntityQuery<vwSensorValuesAndTC10MinDataLog> qry = from n in db.GetVwSensorValuesAndTC10MinDataLogQuery()
                                                               where n.TIMESTAMP >= dt && n.TIMESTAMP < dt.AddDays(1) && n.SENSOR_ID == SensorID && n.ISVALID == "Y"
                                                               orderby n.TIMESTAMP
                                                               select n;
            //EntityQuery  qry = from n in db.GetTblTC10MinDataLogQuery()   
            //                                     join m in db.GetTblSensor_ValuesQuery() on n.SensorID equals m.SensorID
            //                   where n.TIMESTAMP >= dt && n.TIMESTAMP < dt.AddDays(1) && n.SENSOR_ID == SensorID && n.ISVALID == "Y"
            //                   orderby n.TIMESTAMP
            //                   select new { n., m };



            LoadOperation<vwSensorValuesAndTC10MinDataLog> lo = db.Load<vwSensorValuesAndTC10MinDataLog>(qry);

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
                    
                    if (db.tblSensors.FirstOrDefault().SENSOR_TYPE == "TILT")
                    {
                        IsTilt = 1;
                        vcnt = 3;
                    }
                    else
                        vcnt = 3;
                    for (int i = 1; i <= vcnt; i++)
                    {
                        CreateChart(i);
                    }
                    //db.Load<tblSensor_Values>(db.GetTblSensor_ValuesQuery().Where(dv => dv.SENSOR_ID == SensorID)).Completed += (sss, aaa)
                    //    =>
                    //    {
                    //        initmean = db.tblSensor_Values.FirstOrDefault().INITMEAN;
                    //        sigma = db.tblSensor_Values.FirstOrDefault().SIGMA;
                    //    };

                };



            };












        }
        //Visifire.Charts.Chart chart = new Visifire.Charts.Chart();


        private void chart_Rendered(object sender, EventArgs e)
        {
            var c = sender as Chart;
            var legend = c.Legends[0];
            var root = legend.Parent as Grid;


            //root.Children.Clear();
            root.Children.RemoveAt(8);
            //root.Children.RemoveAt(8);
            //MessageBox.Show(root.Children.Count().ToString());
        }

        public void CreateChart(int valueid)
        {

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Spline;
            dataSeries.Color = new SolidColorBrush(Colors.White);
            dataSeries.LineThickness = 1;
            dataSeries.LightingEnabled = true;
            dataSeries.SelectionEnabled = false;
            dataSeries.MovingMarkerEnabled = true;

            //DataSeries dataSeries1 = new DataSeries();
            //dataSeries1.RenderAs = RenderAs.Spline;
            //dataSeries1.Color = new SolidColorBrush(Colors.Blue);
            //dataSeries1.LightingEnabled = true;
            //dataSeries1.SelectionEnabled = false;
            //dataSeries1.MovingMarkerEnabled = true;


            double max = 0;
            double min = 0;


            Title title = new Visifire.Charts.Title();
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();
            chart.Rendered += new EventHandler(chart_Rendered); 
            int datacnt = 0;
            int datavalchk = 0;
            foreach (vwSensorValuesAndTC10MinDataLog data in db.vwSensorValuesAndTC10MinDataLogs)
            {
                if (datavalchk != 1)
                datavalchk = 1;
                DataPoint dataPoint = new DataPoint();
                tblSensor sensor = new tblSensor();
                dataPoint.MarkerSize = 5;
                //foreach (tblSensor_Values rule in db.tblSensor_Values)
                //{

                //    MessageBox.Show(initmean.ToString());
                //}

                double linethickness = 0.5, avg = 0, sigma = 0;/////////////
                /****************************/


                if (valueid == 1)
                {
                    avg = data.initmean0;
                    sigma = data.signama0;

                }
                else if (valueid == 2)
                {
                    avg = data.initmean1;
                    sigma = data.sigma1;

                }
                else if (valueid == 3)
                {
                    avg = data.initmean2;
                    sigma = data.sigma2;

                }


                if (status == "Normal")
                {
                    max = avg + (sigma * 4);
                    min = avg - (sigma * 4);
                    //datacnt++;
                }
                else if (status == "Range")
                {

                    if (valueid == 1)
                    {
                        max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE0);
                        min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE0);
                        avg = ((max + min)) / 2;
                        sigma = (max - min) / 6;

                    }
                    else if (valueid == 2)
                    {
                        max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE1);
                        min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE1);
                        avg = ((max + min)) / 2;
                        sigma = (max - min) / 6;
                    }
                    else if (valueid == 3)
                    {
                        max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE2);
                        min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE2);
                        avg = ((max + min)) / 2;
                        sigma = (max - min) / 6;
                    }
                    if (max == min)
                    {
                        sigma = max / 6;
                        max = avg + sigma * 3;
                        min = avg - sigma * 3;
                    }


                }
                /****************************/

                datacnt++;

                switch (valueid)
                {
                    case 1:
                        dataPoint.YValue = (double)data.VALUE0;
                        //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                        //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                        dataPoint.AxisXLabel = string.Format("{0:00}:{1:00}", data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                        if (data.DEGREE == 0)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Green);
                            //dataSeries.Color = new SolidColorBrush(Colors.Green);
                        }
                        if (data.DEGREE == 1)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow);
                            //dataSeries.Color = new SolidColorBrush(Colors.Yellow);
                        }
                        if (data.DEGREE == 2)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Orange);
                            //dataSeries.Color = new SolidColorBrush(Colors.Orange);
                        }
                        if (data.DEGREE == 3)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);
                            //dataSeries.Color = new SolidColorBrush(Colors.Red);
                        }
                        //data.EXECUTION_MODE
                        //max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE0);
                        //min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE0);
                        //max = data.initmean0 + (data.signama0) * 4;
                        //min = data.initmean0 - (data.signama0) * 4;
                        //Hour_MA = (double)db.GetTblRulesQuery();
                        title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-X軸";
                        if (datacnt == db.vwSensorValuesAndTC10MinDataLogs.Count)
                        {
                            
                            //Isvalid = "Y";
                            chart.TrendLines.Add(new TrendLine() { Value = avg, LineColor = new SolidColorBrush(Colors.Green), LabelText = avg.ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 1), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg + (sigma * 1)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 1), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg - (sigma * 1)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 2), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg + (sigma * 2)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 2), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg - (sigma * 2)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 3), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg + (sigma * 3)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 3), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg - (sigma * 3)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                        }
                        break;
                    case 2:
                        dataPoint.YValue = (double)data.VALUE1;
                        //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                        //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                        dataPoint.AxisXLabel = string.Format("{0:00}:{1:00}", data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                        if (data.DEGREE == 0)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Green);
                            //dataSeries.Color = new SolidColorBrush(Colors.Green);
                        }
                        if (data.DEGREE == 1)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow);
                            //dataSeries.Color = new SolidColorBrush(Colors.Yellow);
                        }
                        if (data.DEGREE == 2)
                        {
                            dataPoint.Color = new SolidColorBrush(Colors.Orange);
                            //dataSeries.Color = new SolidColorBrush(Colors.Orange);
                        }
                        if (data.DEGREE == 3)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);
                            //dataSeries.Color = new SolidColorBrush(Colors.Red);
                        }
                        //max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE1);
                        //min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE1);

                        title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-Y軸";
                        if (datacnt == db.vwSensorValuesAndTC10MinDataLogs.Count)
                        {
                            //max = data.initmean1 + (data.sigma1) * 4;
                            //min = data.initmean1 - (data.sigma1) * 4;


                            chart.TrendLines.Add(new TrendLine() { Value = avg, LineColor = new SolidColorBrush(Colors.Green), LabelText = avg.ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 1), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg + (sigma * 1)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 1), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg - (sigma * 1)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 2), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg + (sigma * 2)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 2), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg - (sigma * 2)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 3), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg + (sigma * 3)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 3), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg - (sigma * 3)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                        }
                        break;
                    case 3:
                        dataPoint.YValue = (double)data.VALUE2;
                        //                        dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                        //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                        dataPoint.AxisXLabel = string.Format("{0:00}:{1:00}", data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                        if (data.DEGREE == 0)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Green);
                            //dataSeries.Color = new SolidColorBrush(Colors.Green);
                        }
                        if (data.DEGREE == 1)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow);
                            //dataSeries.Color = new SolidColorBrush(Colors.Yellow);
                        }
                        if (data.DEGREE == 2)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Orange);
                            //dataSeries.Color = new SolidColorBrush(Colors.Orange);
                        }
                        if (data.DEGREE == 3)
                        {
                            dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);
                            //dataSeries.Color = new SolidColorBrush(Colors.Red);
                        }
                        //max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE2);
                        //min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE2);
                        //max = data.initmean2 + (data.sigma2) * 4;
                        //min = data.initmean2 - (data.sigma2) * 4;



                        if (db.tblSensors.FirstOrDefault().SENSOR_TYPE == "TILT")
                            title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-溫度";
                        else
                            title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-Z軸";
                        if (datacnt == db.vwSensorValuesAndTC10MinDataLogs.Count)
                        {
                            chart.TrendLines.Add(new TrendLine() { Value = avg, LineColor = new SolidColorBrush(Colors.Green), LabelText = avg.ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 1), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg + (sigma * 1)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 1), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg - (sigma * 1)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 2), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg + (sigma * 2)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 2), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg - (sigma * 2)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg + (sigma * 3), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg + (sigma * 3)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            chart.TrendLines.Add(new TrendLine() { Value = avg - (sigma * 3), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg - (sigma * 3)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                        }
                        break;
                }

                //dataPoint.YValue = (double)data.VALUE0;
                dataSeries.DataPoints.Add(dataPoint);
                dataSeries.ToolTipText = "時間:#AxisXLabel時\n資料:#YValue";

            }

            if (datavalchk != 1)
                Isvalid = "N";
            else
                Isvalid = "Y";
            //double LastpointYValue0, LastpointYValue1, LastpointYValue2;
            //if (db.vwSensorValuesAndTC10MinDataLogs.Count != 0)
            //{
            //    LastpointYValue0 = (double)db.vwSensorValuesAndTC10MinDataLogs.Last().VALUE0;
            //    //LastpointYValue1 = (double)db.vwSensorValuesAndTC10MinDataLogs.Last().VALUE1;
            //    //LastpointYValue2 = (double)db.vwSensorValuesAndTC10MinDataLogs.Last().VALUE2;
            //}

            //DataPoint datapoint1 ;
            //datapoint1 = new DataPoint() ;
            //datapoint1.AxisXLabel = string.Format("{0:00}:{1:00}", db.vwSensorValuesAndTC10MinDataLogs.First().TIMESTAMP.Hour, db.vwSensorValuesAndTC10MinDataLogs.Last().TIMESTAMP.Minute);
            //datapoint1.XValue = 1;
            //datapoint1.YValue = (double)db.vwSensorValuesAndTC10MinDataLogs.Last().VALUE0;
            //dataSeries1.DataPoints.Add(datapoint1);
            //datapoint1 = new DataPoint();
            //datapoint1.AxisXLabel = string.Format("{0:00}:{1:00}", db.vwSensorValuesAndTC10MinDataLogs.Last().TIMESTAMP.Hour, db.vwSensorValuesAndTC10MinDataLogs.Last().TIMESTAMP.Minute);
            //datapoint1.XValue = db.vwSensorValuesAndTC10MinDataLogs.Count() ;
            //datapoint1.YValue = (double)db.vwSensorValuesAndTC10MinDataLogs.Last().VALUE0;
            ////{ AxisXLabel = string.Format("{0:00}:{1:00}", db.vwSensorValuesAndTC10MinDataLogs.Last().TIMESTAMP.Hour, db.vwSensorValuesAndTC10MinDataLogs.Last().TIMESTAMP.Minute), XValue = db.vwSensorValuesAndTC10MinDataLogs.Count - 1, YValue = (double)db.vwSensorValuesAndTC10MinDataLogs.Last().VALUE0 };
            //dataSeries1.DataPoints.Add(datapoint1);

            //chart.Series.Add(dataSeries1);








            //switch (valueid)
            //{
            //    case 0:
            //        chart = chart1;
            //        break;
            //}






            Axis yaxis = new Axis();
            yaxis.Opacity = 0;

            //yaxis.LineColor =new SolidColorBrush(Colors.Black);

            yaxis.AxisMaximum = max;
            yaxis.AxisMinimum = min;
            yaxis.Interval = (max - min) / 5;
            yaxis.ValueFormatString = "#,0.0000";
            yaxis.Grids.Add(new ChartGrid() { Enabled = false });//********1126**********
            chart.AxesY.Add(yaxis);

            Axis axisX = new Axis();

            //axisX.ValueFormatString = "hh:mm:ss tt";
            axisX.AxisLabels = new AxisLabels();
            //axisX.AxisLabels.Angle = -45;
            axisX.AxisLabels.Interval = 12;
            chart.AxesX.Add(axisX);

            chart.Titles.Add(title);

            chart.ScrollingEnabled = false;

            chart.IndicatorEnabled = true;
            chart.AnimationEnabled = true;
            chart.AnimatedUpdate = true;
            chart.ZoomingEnabled = true;
            chart.ShadowEnabled = true;
            chart.Series.Add(dataSeries);
            chart.Theme = "Theme3";
            chart.Height = LayoutRoot.ActualHeight / vcnt;
            Thickness tk = new Thickness()
            {
                Bottom = 1,
                Top = 1
            };
            chart.Margin = tk;
            //stackpanel.Orientation = Orientation.Horizontal;
            stackpanel.Children.Add(chart);





        }

        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            string date = BeginPicker.SelectedDate.ToString();
            
            if (BeginPicker.SelectedDate == null)
                date = DateTime.Today.ToString();
            else
                date = BeginPicker.SelectedDate.ToString();

            //if(BeginPicker.SelectedDate.ToString()!="")

            if (Isvalid == "Y")
            {
                if(date == DateTime.Today.ToString())
                    html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID + "&date= &Istilt=" + IsTilt, UriKind.Relative));
                else
                    html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID + "&date=" + date + "&Istilt=" + IsTilt, UriKind.Relative));
            
            }
            else
                MessageBox.Show("目前未收到資料");

            //html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID , UriKind.Relative));
        }





        //private void BeginPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{

        //    LoadData(BeginPicker.SelectedDate.Value.AddDays(0), sensorID);
        //}

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int cnt = stackpanel.Children.Count;
            foreach (Chart u in stackpanel.Children)
                u.Height = LayoutRoot.ActualHeight / cnt;



        }

        private void BeginPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData(BeginPicker.SelectedDate.Value.AddDays(0), sensorID);
        }

        private void NormalDisable_Click(object sender, RoutedEventArgs e)
        {
            ChangeRangeDisable("Normal");

            if (BeginPicker.SelectedDate == null)
                LoadData(DateTime.Today, sensorID);
            else
                LoadData(BeginPicker.SelectedDate.Value.AddDays(0), sensorID);
        }



        private void RangeDisable_Click(object sender, RoutedEventArgs e)
        {
            ChangeRangeDisable("Range");

            if (BeginPicker.SelectedDate == null)
                LoadData(DateTime.Today, sensorID);
            else
                LoadData(BeginPicker.SelectedDate.Value.AddDays(0), sensorID);
        }


        private void ChangeRangeDisable(string s)
        {
            switch (s)
            {
                case "Normal":
                    status = s;
                    break;
                case "Range":
                    status = s;
                    break;
            }

        }

        
        


    }
}
