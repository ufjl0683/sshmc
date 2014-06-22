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
        int sensorID = 0, sensorTypeGroupID = 0;
        //System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();
        //Boolean _oddState = false;
        int vcnt = 0, IsTilt = 0;
        string status = "Normal";///////////
        string Isvalid = "N";
        byte sensorvaluescount = 0;
        string[] titlename_arry;
        double[] scale_arry;



        public DiagramChatControl(int SensorID)
        {
            InitializeComponent();
            sensorID = SensorID;
            //CheckBoxIsValue.IsChecked = false;
            BeginPicker.SelectedDate = DateTime.Now.Date.AddDays(0);
            //BeginPicker.SelectedDate = DateTime.Today; /////////////102.08.19新增   
            LoadData(System.DateTime.Now.Date.AddDays(0), System.DateTime.Now.Date.AddDays(0), sensorID);
            //_timer.Tick += new EventHandler(_timer_Tick);
            //_timer.Interval = TimeSpan.FromMinutes(10);





        }








        //void _timer_Tick(object sender, EventArgs e)
        //{
        //    if (BeginPicker.SelectedDate == DateTime.Today)/////////////102.08.19新增
        //    LoadData(DateTime.Today, sensorID);///////////////102.08.19新增           目的讓每十分鐘自動更新 如果selectdate不是今天則不更新

        //}


        private void LoadData(DateTime stardt, DateTime enddt, int SensorID)
        {
            //qry = null;
            stackpanel.Children.Clear();
            db = new DbContext();

            Isvalid = "N";
            FunctionLoadSensorTypeGroupID(stardt, enddt,SensorID);


        }

        private void FunctionTypeGroupData(DateTime stardt, DateTime enddt, int sensorTypeGroupID)
        {
            var query2 = from n in db.GetTblSensorTypeGroupQuery() where n.TYPEGROUP_ID == sensorTypeGroupID select n;
            LoadOperation<tblSensorTypeGroup> lo2 = db.Load<tblSensorTypeGroup>(query2);
            lo2.Completed += (s, a) =>
            {
                if (lo2.Error != null)
                {
                    MessageBox.Show(lo2.Error.Message);
                    return;
                }
                sensorvaluescount = lo2.Entities.FirstOrDefault().VALUE_COUNT ?? 0;
                if (sensorvaluescount != 0)
                {
                    titlename_arry = new string[3];
                    titlename_arry[0] = lo2.Entities.FirstOrDefault().VALUE0_TITLE;
                    titlename_arry[1] = lo2.Entities.FirstOrDefault().VALUE1_TITLE;
                    titlename_arry[2] = lo2.Entities.FirstOrDefault().VALUE2_TITLE;
                    scale_arry = new double[3];
                    scale_arry[0] = lo2.Entities.FirstOrDefault().SCALE0??0;
                    scale_arry[1] = lo2.Entities.FirstOrDefault().SCALE1??0;
                    scale_arry[2] = lo2.Entities.FirstOrDefault().SCALE2??0;


                    FunctionCreateDiagramChart(stardt,enddt, sensorvaluescount);
                }
            };
        }

        private void FunctionCreateDiagramChart(DateTime stardt, DateTime enddt, byte sensorvaluescount)
        {
            if (CheckBoxIsValue.IsChecked ?? true)
            {
                #region 有效筆數

                EntityQuery<vwSensorValuesAndTC10MinDataLog> qry = from n in db.GetVwSensorValuesAndTC10MinDataLogQuery()
                                                                   where n.TIMESTAMP >= stardt && n.TIMESTAMP < enddt.AddDays(1) && n.SENSOR_ID == sensorID && n.ISVALID == "Y"
                                                                   orderby n.TIMESTAMP
                                                                   select n;


                LoadOperation<vwSensorValuesAndTC10MinDataLog> lo3 = db.Load<vwSensorValuesAndTC10MinDataLog>(qry);

                lo3.Completed += (s, a) =>
                {

                    if (lo3.Error != null)
                    {
                        MessageBox.Show(lo3.Error.Message);
                        return;
                    }


                    db.Load<tblSensor>(db.GetTblSensorQuery().Where(dd => dd.SENSOR_ID == sensorID)).Completed += (ss, aa)
                        =>
                    {
                        if ((ss as LoadOperation).Error != null)
                        {

                            MessageBox.Show((ss as LoadOperation).Error.Message);
                            return;
                        }



                        //if (db.tblSensors.FirstOrDefault().SENSOR_TYPE == "TILT")
                        //{
                        //    IsTilt = 1;
                        //    vcnt = sensorvaluescount;
                        //}
                        //else
                        vcnt = sensorvaluescount;

                        if (vcnt != 0)
                        {
                            for (int i = 1; i <= vcnt; i++)
                            {
                                CreateChart(i);
                            }
                        }
                        else
                        {
                            MessageBox.Show("此感知器無軸數、出圖數線數量，如有疑問請詢問中心維護人員。");
                        }

                    };
                };
                #endregion
            }
            else
            {
                #region 含無效筆數


                EntityQuery<vwSensorValuesAndTC10MinDataLog> qry = from n in db.GetVwSensorValuesAndTC10MinDataLogQuery()
                                                                   where n.TIMESTAMP >= stardt && n.TIMESTAMP < enddt.AddDays(1) && n.SENSOR_ID == sensorID
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



                    db.Load<tblSensor>(db.GetTblSensorQuery().Where(dd => dd.SENSOR_ID == sensorID)).Completed += (ss, aa)
                        =>
                    {
                        if ((ss as LoadOperation).Error != null)
                        {

                            MessageBox.Show((ss as LoadOperation).Error.Message);
                            return;
                        }

                        //tblTC10MinDataLog data = new tblTC10MinDataLog();
                        //in db.tblTC10MinDataLogs


                        vcnt = sensorvaluescount;
                        for (int i = 1; i <= vcnt; i++)
                        {
                            CreateChart(i);
                        }


                    };
                };
            }
                #endregion
            
        }

        private void FunctionLoadSensorTypeGroupID(DateTime stardt, DateTime enddt, int SensorID)
        {
            var query = from n in db.GetTblSensorQuery() where n.SENSOR_ID == SensorID select n;
            LoadOperation<tblSensor> lo1 = db.Load<tblSensor>(query);
            lo1.Completed += (s, a) =>
                {
                    if (lo1.Error != null)
                    {
                        MessageBox.Show(lo1.Error.Message);
                        return;
                    }
                    sensorTypeGroupID = lo1.Entities.FirstOrDefault().TYPEGROUP_ID ?? 0;
                    FunctionTypeGroupData(stardt, enddt,sensorTypeGroupID);
                };



        }
        //Visifire.Charts.Chart chart = new Visifire.Charts.Chart();
        public void CreateChart(int valueid)
        {

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Spline;
            dataSeries.Color = new SolidColorBrush(Colors.White);
            dataSeries.LineThickness = 1;
            dataSeries.LightingEnabled = true;
            dataSeries.SelectionEnabled = false;
            dataSeries.MovingMarkerEnabled = true;




            double max = 0;
            double min = 0;


            Title title = new Visifire.Charts.Title();
            Visifire.Charts.Chart chart = new Visifire.Charts.Chart();
            chart.Rendered += new EventHandler(chart_Rendered);
            int datacnt = 0;
            int datavalchk = 0;
            datanum.Text = "共 " + db.vwSensorValuesAndTC10MinDataLogs.Count.ToString() + " 筆資料！";
            if (db.vwSensorValuesAndTC10MinDataLogs.Count != 0)
            {
                foreach (vwSensorValuesAndTC10MinDataLog data in db.vwSensorValuesAndTC10MinDataLogs)
                {
                    if (datavalchk != 1)
                        datavalchk = 1;
                    DataPoint dataPoint = new DataPoint();
                    tblSensor sensor = new tblSensor();


                    #region 設定point大小，隨資料多寡，縮放。


                    if (db.vwSensorValuesAndTC10MinDataLogs.Count < 500)
                        dataPoint.MarkerSize = 5;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 1000)
                        dataPoint.MarkerSize = 4.5;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 1500)
                        dataPoint.MarkerSize = 4;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 2000)
                        dataPoint.MarkerSize = 3.5;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 2500)
                        dataPoint.MarkerSize = 3;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 3000)
                        dataPoint.MarkerSize = 2.5;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 3500)
                        dataPoint.MarkerSize = 2;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 4000)
                        dataPoint.MarkerSize = 1.5;
                    else if (db.vwSensorValuesAndTC10MinDataLogs.Count < 4500)
                        dataPoint.MarkerSize = 1;
                    else
                        dataPoint.MarkerSize = 0.5;
                    #endregion





                    double linethickness = 0.5, avg = 0, sigma = 0;/////////////
                    /****************************/
                    datacnt++;
                    if (datacnt == db.vwSensorValuesAndTC10MinDataLogs.Count)
                    {
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

                        #region 103.05.08 by corn   讓sigma變異數固定 ， 隨感知器類型 ， 給不同的固定值 。
                        //switch (db.tblSensors.FirstOrDefault().SENSOR_TYPE)
                        //{
                        //    case "EGPS":
                        //        sigma = 0.03;
                        //        break;
                        //    case "TILT":
                        //        sigma = 0.0167;
                        //        break;
                        //    case "GPS":
                        //        sigma = 0.03;
                        //        break;



                        //}
                        if (valueid == 1)
                        {
                            avg = data.initmean0;
                            sigma = scale_arry[0];

                        }
                        else if (valueid == 2)
                        {
                            avg = data.initmean1;
                            sigma = scale_arry[1];

                        }
                        else if (valueid == 3)
                        {
                            avg = data.initmean2;
                            sigma = scale_arry[2];

                        }


                        #endregion


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
                                sigma = Math.Abs(max) / 6;
                                max = avg + sigma * 3;
                                min = avg - sigma * 3;
                            }


                        }
                    }
                    /****************************/

                    //datacnt++;

                    switch (valueid)
                    {
                        case 1:
                            dataPoint.YValue = (double)data.VALUE0;
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                            dataPoint.AxisXLabel = string.Format("{0:00}/{1:00} {2:00}:{3:00}", data.TIMESTAMP.Month, data.TIMESTAMP.Day, data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                            if (data.DEGREE == 0)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Green);
                            }
                            if (data.DEGREE == 1)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow);
                            }
                            if (data.DEGREE == 2)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Orange);
                            }
                            if (data.DEGREE == 3)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);
                            }
                            //data.EXECUTION_MODE
                            //max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE0);
                            //min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE0);
                            //max = data.initmean0 + (data.signama0) * 4;
                            //min = data.initmean0 - (data.signama0) * 4;
                            //Hour_MA = (double)db.GetTblRulesQuery();
                            title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + titlename_arry[0];
                            //title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-X軸";
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
                            dataPoint.AxisXLabel = string.Format("{0:00}/{1:00} {2:00}:{3:00}", data.TIMESTAMP.Month, data.TIMESTAMP.Day, data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                            if (data.DEGREE == 0)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Green);
                            }
                            if (data.DEGREE == 1)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow);
                            }
                            if (data.DEGREE == 2)
                            {
                                dataPoint.Color = new SolidColorBrush(Colors.Orange);
                            }
                            if (data.DEGREE == 3)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);
                            }
                            //max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE1);
                            //min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE1);
                            title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + titlename_arry[1];
                            //title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-Y軸";
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
                            dataPoint.AxisXLabel = string.Format("{0:00}/{1:00} {2:00}:{3:00}", data.TIMESTAMP.Month, data.TIMESTAMP.Day, data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                            if (data.DEGREE == 0)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Green);
                            }
                            if (data.DEGREE == 1)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Yellow);
                            }
                            if (data.DEGREE == 2)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Orange);
                            }
                            if (data.DEGREE == 3)
                            {
                                dataPoint.MarkerColor = new SolidColorBrush(Colors.Red);
                            }



                            title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + titlename_arry[2];
                            //if (db.tblSensors.FirstOrDefault().SENSOR_TYPE == "TILT")
                            //{
                            //    title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-溫度";
                            //    sigma = 5;
                            //}
                            //else
                            //    title.Text = db.tblSensors.FirstOrDefault().SENSOR_NAME + "-Z軸";
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









            Axis yaxis = new Axis();
            yaxis.Opacity = 0;



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
            axisX.AxisLabels.Interval = db.vwSensorValuesAndTC10MinDataLogs.Count / 6;
            chart.AxesX.Add(axisX);

            

            chart.Titles.Add(title);

            chart.ScrollingEnabled = false;
            chart.LightingEnabled = true;
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

            // MessageBox.Show((dataSeries.Color as SolidColorBrush).Color.ToString());
            }
            else
            {
                Border label = new Border() 
                {
                  //  Content = "此感知器尚無資料。",
                     Width = LayoutRoot.ActualWidth,
                     Height = LayoutRoot.ActualHeight / vcnt,
                     Background = new SolidColorBrush(Colors.Black),
                  //   Foreground = new SolidColorBrush(Colors.White),
                     Margin = new Thickness(5),
                     HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                     VerticalAlignment = System.Windows.VerticalAlignment.Center
                };

                

                //MessageBox.Show("此感知器尚無資料。");
                stackpanel.Children.Add(label);
            }


        }

        //private void chart_Rendered(object sender, EventArgs e)
        //{
        //    var c = sender as Chart;
        //    var legend = c.Legends[0];
        //    var root = legend.Parent as Grid;

        //    //root.Children.Clear();
        //    root.Children.RemoveAt(8);
        //    //root.Children.RemoveAt(8);
        //    //MessageBox.Show(root.Children.Count().ToString());
        //}
        private void chart_Rendered(object sender, EventArgs e)
        {

            try
            {



                var c = sender as Chart;
                var legend = c.Legends[0];
                var root = legend.Parent as Grid;

                //string removeStringcheck = (root.Children.OfType<Border>().Last().Child as TextBlock).Text;



                //if (removeStringcheck == "You are using the trial version")
                //{
                //    root.Children.Remove(root.Children.OfType<Border>().Last());
                //}




                string removeStringchecksp = (root.Children.OfType<StackPanel>().Cast<StackPanel>().Last().Children[0] as TextBlock).Text;
                if (removeStringchecksp == "Visifire Trial Edition")
                {
                    // root.Children.OfType<StackPanel>().Cast<StackPanel>().Last().Children.RemoveAt(0);
                    root.Children.Remove(root.Children.OfType<StackPanel>().Cast<StackPanel>().Last());
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;


            DateTime startime = BeginPicker.SelectedDates.FirstOrDefault().Date.AddDays(0);
            DateTime endtime = BeginPicker.SelectedDates.LastOrDefault().Date.AddDays(0);
            string title0 = titlename_arry[0],title1 = titlename_arry[1] ,title2 = titlename_arry[2];
            string date = BeginPicker.SelectedDate.ToString();

            if (BeginPicker.SelectedDate == null)
                date = DateTime.Today.ToString();
            else
                date = startime + "," + endtime;
            //date = BeginPicker.SelectedDate.ToString();

            //if(BeginPicker.SelectedDate.ToString()!="")

            if (Isvalid == "Y")
            {
                
                if (CheckBoxIsValue.IsChecked ?? true)
                {
                    if (date == DateTime.Today.ToString())
                    {
                        html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID + "&date= &title0=" + title0 + "&title1=" + title1 + "&title2=" + title2+ "&IsValue=Y" , UriKind.Relative));
                    }
                    else
                    {
                        html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID + "&date=" + date + "&title0=" + title0 + "&title1=" + title1 + "&title2=" + title2 + "&IsValue=Y", UriKind.Relative));
                    }
                }
                else
                {
                    if (date == DateTime.Today.ToString())
                    {
                        html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID + "&date=" + "&IsValue=N" + "&title0=" + title0 + "&title1=" + title1 + "&title2=" + title2, UriKind.Relative));
                    }
                    else
                    {
                        html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID + "&date=" + date + "&IsValue=N" + "&title0=" + title0 + "&title1=" + title1 + "&title2=" + title2, UriKind.Relative));
                    }
                }
            }
            else
                MessageBox.Show("目前未收到資料");

            //html.Navigate(new Uri("DownLoadForm.aspx?id=" + sensorID , UriKind.Relative));
        }





        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int cnt = stackpanel.Children.Count;
            foreach (Chart u in stackpanel.Children)
                u.Height = LayoutRoot.ActualHeight / cnt;



        }

        private void BeginPicker_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar c = sender as Calendar;
            //MessageBox.Show(c.SelectedDates.LastOrDefault().Date.AddDays(0).Subtract(c.SelectedDates.FirstOrDefault().Date.AddDays(0)).ToString() + "\n" + c.SelectedDates.FirstOrDefault().Date.AddDays(0).ToString());
            if (c.SelectedDates.LastOrDefault().Date.AddDays(0).Subtract(c.SelectedDates.FirstOrDefault().Date.AddDays(0)) < new TimeSpan(31, 0, 0, 0, 0))
            {

            }
            else
            {
                MessageBox.Show("必須小於等於一個月!");
                c.SelectedDates.Clear();
                c.SelectedDate = DateTime.Now;
            }

            //DateTime startime = BeginPicker.SelectedDates.FirstOrDefault();
            //DateTime endtime = BeginPicker.SelectedDates.LastOrDefault();
            //LoadData(startime,endtime, sensorID);
        }



        private void NormalDisable_Click(object sender, RoutedEventArgs e)
        {
            ChangeRangeDisable("Normal");
            DateTime startime = BeginPicker.SelectedDates.FirstOrDefault().Date.AddDays(0);
            DateTime endtime = BeginPicker.SelectedDates.LastOrDefault().Date.AddDays(0);
            if (BeginPicker.SelectedDate == null)
                LoadData(DateTime.Today, DateTime.Today, sensorID);
            else
                LoadData(startime, endtime, sensorID);
        }



        private void RangeDisable_Click(object sender, RoutedEventArgs e)
        {
            ChangeRangeDisable("Range");
            DateTime startime = BeginPicker.SelectedDates.FirstOrDefault().Date.AddDays(0);
            DateTime endtime = BeginPicker.SelectedDates.LastOrDefault().Date.AddDays(0);
            if (BeginPicker.SelectedDate == null)
                LoadData(DateTime.Today, DateTime.Today, sensorID);
            else
                LoadData(startime, endtime, sensorID);
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

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
           
            DateTime startime = BeginPicker.SelectedDates.FirstOrDefault().Date.AddDays(0);
            DateTime endtime = BeginPicker.SelectedDates.LastOrDefault().Date.AddDays(0);
            LoadData(startime, endtime, sensorID);
        }



        // bool checkboxisvalue = true;
        private void CheckBoxIsValue_Click(object sender, RoutedEventArgs e)
        {
            //if (CheckBoxIsValue.IsChecked == false)
            //    checkboxisvalue = false;
            //else if(CheckBoxIsValue.IsChecked == true)
            //    checkboxisvalue = true;
        }







    }
}
