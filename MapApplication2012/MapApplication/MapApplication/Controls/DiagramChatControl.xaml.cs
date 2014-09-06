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
        string status = "Normal";/////////// Normal=> 正常顯示 Range=>全距顯示
        string Isvalid = "N";  // 判斷下載時的char Values是否有效
        byte sensorvaluescount = 0; 
        string[] titlename_arry;
        double[] scale_arry;



        public DiagramChatControl(int SensorID) //Sensorid => send sensor_value id
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
            if (status != "Range")
            stackpanel.Children.Clear();
            db = new DbContext();

            Isvalid = "N";
            FunctionLoadSensorTypeGroupID(stardt, enddt,SensorID); // 抓SensorTypeGroup 的 ID 


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


                    FunctionCreateDiagramChart(stardt,enddt, sensorvaluescount); // 傳入 開始時間、結束時間、軸數
                }
            };
        }

        private void FunctionCreateDiagramChart(DateTime stardt, DateTime enddt, byte sensorvaluescount)
        {
            if (CheckBoxIsValue.IsChecked ?? true)
            {
                #region 有效筆數
                //DbContext context = new DbContext();
                //var query = (from n in context.vwSensorValuesAndTC10MinDataLogs
                //             where n.TIMESTAMP >= stardt && n.TIMESTAMP < enddt.AddDays(1) && n.SENSOR_ID == sensorID && n.ISVALID == "Y"
                //             orderby n.TIMESTAMP
                //             select n).ToList();
                //foreach (var i in query)
                //{ 
                //    //
                //}



                EntityQuery<vwSensorValuesAndTC10MinDataLog> qry = from n in db.GetVwSensorValuesAndTC10MinDataLogQuery()                                       // ISVALID指的是 通常=0為無效，主要為有效 !=0
                                                                   where n.TIMESTAMP >= stardt && n.TIMESTAMP < enddt.AddDays(1) && n.SENSOR_ID == sensorID && n.ISVALID == "Y" // 老師寫的程式會先判斷是否有效  Y/N
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
                    FunctionTypeGroupData(stardt, enddt,sensorTypeGroupID);   // 取出SenSorTypeGroup的資料，包涵 Title 和 scale
                };



        }
        //Visifire.Charts.Chart chart = new Visifire.Charts.Chart();
        public void CreateChart(int valueid) // 傳入第N個軸數 valueid <x,y,z,...>
        {
            // 先宣告一條線Series
            // 程式用的是quickLine，不會顯示點點
            // 網頁用的是SpLine，會顯示點點
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
            datanum.Text = "開始:" + BeginPicker.SelectedDates.FirstOrDefault().ToString("yyyy/MM/dd") + "\n結束:" + BeginPicker.SelectedDates.LastOrDefault().ToString("yyyy/MM/dd") + "\n共 " + db.vwSensorValuesAndTC10MinDataLogs.Count.ToString() + " 筆資料！";
            datanum.Tag = BeginPicker.SelectedDates.FirstOrDefault()+","+BeginPicker.SelectedDates.LastOrDefault();
            if (db.vwSensorValuesAndTC10MinDataLogs.Count != 0)
            {
                foreach (vwSensorValuesAndTC10MinDataLog data in db.vwSensorValuesAndTC10MinDataLogs)
                {   // 純脆程式檢查用
                    if (datavalchk != 1)
                        datavalchk = 1;
                    DataPoint dataPoint = new DataPoint();
         


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





                    double linethickness = 0.5, avg = 0, sigma1 = 0, sigma2 = 0, sigma3 = 0;/////////////
                    /****************************/
                    datacnt++; // 計算這個foreach迴圈做了總共幾次，當他==總比數時才做，只做一次!
                    if (datacnt == db.vwSensorValuesAndTC10MinDataLogs.Count)
                    {
                        if (valueid == 1)
                        {
                            avg = data.initmean0??0;
                            sigma1 = data.T0_MEANTHRESHOLD1??0;

                        }
                        else if (valueid == 2)
                        {
                            avg = data.initmean1??0;
                            sigma2 = data.T1_MEANTHRESHOLD1??0;

                        }
                        else if (valueid == 3)
                        {
                            avg = data.initmean2??0;
                            sigma3 = data.T2_MEANTHRESHOLD1??0;

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
                        //if (valueid == 1)
                        //{
                        //    avg = data.initmean0;
                        //    //sigma = scale_arry[0];
                        //    sigma1 = data.T0_MEANTHRESHOLD1 ?? 0;

                        //}
                        //else if (valueid == 2)
                        //{
                        //    avg = data.initmean1;
                        //    //sigma = scale_arry[1];
                        //    sigma1 = data.T1_MEANTHRESHOLD1 ?? 0;

                        //}
                        //else if (valueid == 3)
                        //{
                        //    avg = data.initmean2;
                        //    //sigma = scale_arry[2];       
                        //    sigma3 = data.T2_MEANTHRESHOLD1 ?? 0;

                        //}


                        #endregion


                        if (status == "Normal")
                        {
                            //max = avg + (sigma * 4);
                            //min = avg - (sigma * 4);
                            //datacnt++;
                            //double func1 = Math.Abs(data.T0_MEANTHRESHOLD1 ?? 0 + data.T0_MEANTHRESHOLD2 ?? 0 + data.T0_MEANTHRESHOLD3 ?? 0 / 3) *4;
                            //double func2 = Math.Abs(data.T1_MEANTHRESHOLD1 ?? 0 + data.T1_MEANTHRESHOLD2 ?? 0 + data.T1_MEANTHRESHOLD3 ?? 0 / 3) *4;
                            //double func3 = Math.Abs(data.T2_MEANTHRESHOLD1 ?? 0 + data.T2_MEANTHRESHOLD2 ?? 0 + data.T2_MEANTHRESHOLD3 ?? 0 / 3) *4;
                            double func1 =  data.T0_MEANTHRESHOLD3 ?? 0 ;
                            double func2 =  data.T1_MEANTHRESHOLD3 ?? 0 ;
                            double func3 =  data.T2_MEANTHRESHOLD3 ?? 0 ;

                            if (valueid==1)
                            {
                                max = avg + func1;
                                min = avg - func1;
                            }
                            else if (valueid==2)
                            {
                                max = avg + func2;
                                min = avg - func2;
                            }
                            else if (valueid==3)
                            {
                                max = avg + func3;
                                min = avg - func3;
                            }

                            // ************************** 判斷例外狀況 Start************************** //
                            //if (max == min)
                            //{
                            //    max++;
                            //    min--;
                            //}

                            //if (max ==0 || min ==0)
                            //{
                            //    max++;
                            //    min--;
                            //}

                            //if (max == 0 || min != 0)
                            //{
                            //    max++;
                            //    min--;
                            //}

                            //if (max != 0 || min == 0)
                            //{
                            //    max++;
                            //    min--;
                            //}
                            // ************************** 判斷例外狀況 End************************** //
                            

                        }
                        else if (status == "Range")
                        {
                            #region 2014/07/21 改用speedFunRange()
                            
                            
                            //if (valueid == 1)
                            //{
                            //    max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE0);
                            //    min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE0);
                            //    avg = ((max + min)) / 2;
                            //    sigma1 = (max - min) / 6;

                            //}
                            //else if (valueid == 2)
                            //{
                            //    max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE1);
                            //    min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE1);
                            //    avg = ((max + min)) / 2;
                            //    sigma2 = (max - min) / 6;
                            //}
                            //else if (valueid == 3)
                            //{
                            //    max = (double)db.vwSensorValuesAndTC10MinDataLogs.Max(n => n.VALUE2);
                            //    min = (double)db.vwSensorValuesAndTC10MinDataLogs.Min(n => n.VALUE2);
                            //    avg = ((max + min)) / 2;
                            //    sigma3 = (max - min) / 6;
                            //}

                            //// ************************** 判斷例外狀況 Start************************** //
                            ////if (max == min)
                            ////{
                            ////    max++;
                            ////    min--;
                            ////}

                            ////if (max == 0 || min == 0)
                            ////{
                            ////    max++;
                            ////    min--;
                            ////}

                            ////if (max != 0 || min == 0)
                            ////{
                            ////    max++;
                            ////    min--;
                            ////}

                            ////if (max == 0 || min != 0)
                            ////{
                            ////    max++;
                            ////    min--;
                            ////}
                            //// ************************** 判斷例外狀況 End************************** //

                            //if (valueid == 1)
                            //{
                            //    sigma1 = Math.Abs(max) / 6;
                            //    max = avg + sigma1 * 3;
                            //    min = avg - sigma1 * 3;
                            //}
                            //else if (valueid == 2)
                            //{
                            //    sigma2 = Math.Abs(max) / 6;
                            //    max = avg + sigma2 * 3;
                            //    min = avg - sigma2 * 3;
                            //}
                            //else if (valueid == 3)
                            //{
                            //    sigma3 = Math.Abs(max) / 6;
                            //    max = avg + sigma3 * 3;
                            //    min = avg - sigma3 * 3;
                            //}
                                
                            
#endregion
                            sppendFunRange(valueid );
                            break;
                        }
                    }
                    /****************************/

                    //datacnt++;

                    // 因為用的是valueid
                    // 所以在畫線的function裏頭
                    // 不能用sigma(n)表示 
                    // sigma1 對應 t0 --> x軸
                    // 若再valueid == 1的時候用到 sigma2 也就是y軸的T1數值，會有問題
                    // 因此單獨帶 MEANTHRESHOLD進去
                    switch (valueid)
                    {
                        case 1:
                            dataPoint.YValue = (double)data.VALUE0;
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                            if (db.vwSensorValuesAndTC10MinDataLogs.Count > 144)
                                dataPoint.AxisXLabel = string.Format("{0:00}/{1:00} {2:00}:{3:00}", data.TIMESTAMP.Month, data.TIMESTAMP.Day, data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                            else
                                dataPoint.AxisXLabel = string.Format("{0:00}:{1:00}", data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);


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
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T0_MEANTHRESHOLD1 ?? 0), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg + data.T0_MEANTHRESHOLD1 ?? 0).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T0_MEANTHRESHOLD1 ?? 0), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg - data.T0_MEANTHRESHOLD1 ?? 0).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T0_MEANTHRESHOLD2 ?? 0), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg + data.T0_MEANTHRESHOLD2 ?? 0).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T0_MEANTHRESHOLD2 ?? 0), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg - data.T0_MEANTHRESHOLD2 ?? 0).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T0_MEANTHRESHOLD3 ?? 0), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg + Math.Abs(data.T0_MEANTHRESHOLD3 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T0_MEANTHRESHOLD3 ?? 0), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg - Math.Abs(data.T0_MEANTHRESHOLD3 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            }
                            break;
                        case 2:
                            dataPoint.YValue = (double)data.VALUE1;
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                            if (db.vwSensorValuesAndTC10MinDataLogs.Count > 144)
                                dataPoint.AxisXLabel = string.Format("{0:00}/{1:00} {2:00}:{3:00}", data.TIMESTAMP.Month, data.TIMESTAMP.Day, data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                            else
                                dataPoint.AxisXLabel = string.Format("{0:00}:{1:00}", data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);


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
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T1_MEANTHRESHOLD1 ?? 0), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg + Math.Abs(data.T1_MEANTHRESHOLD1 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T1_MEANTHRESHOLD1 ?? 0), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg - Math.Abs(data.T1_MEANTHRESHOLD1 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T1_MEANTHRESHOLD2 ?? 0), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg + Math.Abs(data.T1_MEANTHRESHOLD2 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T1_MEANTHRESHOLD2 ?? 0), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg - Math.Abs(data.T1_MEANTHRESHOLD2 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T1_MEANTHRESHOLD3 ?? 0), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg + Math.Abs(data.T1_MEANTHRESHOLD3 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T1_MEANTHRESHOLD3 ?? 0), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg - Math.Abs(data.T1_MEANTHRESHOLD3 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                            }
                            break;
                        case 3:
                            dataPoint.YValue = (double)data.VALUE2;
                            //                        dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh:mm:ss tt");
                            //dataPoint.AxisXLabel = data.TIMESTAMP.ToString("hh");
                            if (db.vwSensorValuesAndTC10MinDataLogs.Count > 144)
                                dataPoint.AxisXLabel = string.Format("{0:00}/{1:00} {2:00}:{3:00}", data.TIMESTAMP.Month, data.TIMESTAMP.Day, data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);
                            else
                                dataPoint.AxisXLabel = string.Format("{0:00}:{1:00}", data.TIMESTAMP.Hour, data.TIMESTAMP.Minute);


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
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T2_MEANTHRESHOLD1 ?? 0), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg + Math.Abs(data.T2_MEANTHRESHOLD1 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T2_MEANTHRESHOLD1 ?? 0), LineColor = new SolidColorBrush(Colors.Yellow), LabelText = (avg - Math.Abs(data.T2_MEANTHRESHOLD1 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T2_MEANTHRESHOLD2 ?? 0), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg + Math.Abs(data.T2_MEANTHRESHOLD2 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T2_MEANTHRESHOLD2 ?? 0), LineColor = new SolidColorBrush(Colors.Orange), LabelText = (avg - Math.Abs(data.T2_MEANTHRESHOLD2 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg + Math.Abs(data.T2_MEANTHRESHOLD3 ?? 0), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg + Math.Abs(data.T2_MEANTHRESHOLD3 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
                                chart.TrendLines.Add(new TrendLine() { Value = avg - Math.Abs(data.T2_MEANTHRESHOLD3 ?? 0), LineColor = new SolidColorBrush(Colors.Red), LabelText = (avg - Math.Abs(data.T2_MEANTHRESHOLD3 ?? 0)).ToString("#,0.0000"), LineStyle = LineStyles.Solid, LineThickness = linethickness, LabelFontColor = new SolidColorBrush(Colors.White) });
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







            if (status != "Range")
            {

                Axis yaxis = new Axis();
                yaxis.Opacity = 0;



                yaxis.AxisMaximum = max;
                yaxis.AxisMinimum = min;
                //yaxis.Interval = (max - min) / 5;
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


            }
            else
            {



                Border label = new Border()
                {
                   // Content = "此感知器尚無資料。",
                    Width = LayoutRoot.ActualWidth,
                    Height = LayoutRoot.ActualHeight / vcnt,
                    Background = new SolidColorBrush(Colors.Black),
                  //  Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(5),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };



                //MessageBox.Show("此感知器尚無資料。");
                stackpanel.Children.Add(label);
            }


        }
        double max, min;
        private void sppendFunRange(int vaild)
        {
            #region  //2014/7/21 修改


            if (vaild == 3)
            {
                foreach (Chart chartchil in (LayoutRoot.Children.FirstOrDefault() as StackPanel).Children.OfType<Chart>())
                {
                    DataSeries dataseries = chartchil.Series.FirstOrDefault();


                    max = min = dataseries.DataPoints.FirstOrDefault().YValue;
                    foreach (DataPoint dp in dataseries.DataPoints)
                    {
                        if (dp.YValue != 0)
                        {
                            if (max < dp.YValue)
                                max = dp.YValue;

                            if (min > dp.YValue)
                                min = dp.YValue;
                        }
                        else
                        {

                        }
                    }
                    Axis yaxis = chartchil.AxesY[0];
                    yaxis.AxisMaximum = max;
                    yaxis.AxisMinimum = min;
                    yaxis.Opacity = 1;
                   //dataseries.AxisYType = AxisTypes.Secondary;

                    yaxis.Grids.Add(new ChartGrid() { Enabled = true });//********0721**********


                    foreach (TrendLine trendline in chartchil.TrendLines)
                    {
                        trendline.ToolTipText = trendline.LabelText;
                        trendline.LineThickness = 5;
                        trendline.LabelText = "";
                       
                    }

                }
            }
           


           
            #endregion
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
        string removeStringchecksp;
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

              
                if (root.Children.OfType<StackPanel>().Last().Children.Count() != 0)
                {
                    //MessageBox.Show(root.Children.OfType<StackPanel>().Last().Children.ToString());
                    removeStringchecksp = (root.Children.OfType<StackPanel>().Last().Children.OfType<TextBlock>().FirstOrDefault()).Text;
                    (root.Children.OfType<StackPanel>().Last().Children.OfType<TextBlock>().FirstOrDefault()).Text = "";
                }
                
               
                if (removeStringchecksp == "Visifire Trial Edition")
                {
                    // root.Children.OfType<StackPanel>().Cast<StackPanel>().Last().Children.RemoveAt(0);
                  //  root.Children.Remove(root.Children.OfType<StackPanel>().Cast<StackPanel>().Last());
                    //
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
            //DateTime startime = BeginPicker.SelectedDates.FirstOrDefault().Date.AddDays(0);
            //DateTime endtime = BeginPicker.SelectedDates.LastOrDefault().Date.AddDays(0);
            //if (BeginPicker.SelectedDate == null)
            //    LoadData(DateTime.Today, DateTime.Today, sensorID);
            //else
            //    LoadData(startime, endtime, sensorID);
        }



        private void RangeDisable_Click(object sender, RoutedEventArgs e)
        {
            ChangeRangeDisable("Range");
            //DateTime startime = BeginPicker.SelectedDates.FirstOrDefault().Date.AddDays(0);
            //DateTime endtime = BeginPicker.SelectedDates.LastOrDefault().Date.AddDays(0);
            //if (BeginPicker.SelectedDate == null)
            //    LoadData(DateTime.Today, DateTime.Today, sensorID);
            //else
            //    LoadData(startime, endtime, sensorID);
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

            string[] strspi = datanum.Tag.ToString().Split(',');
            //MessageBox.Show(datanum.Tag.ToString());
            DateTime oldstartime =  Convert.ToDateTime(strspi[0]).Date.AddDays(0);
            DateTime oldendtime = Convert.ToDateTime(strspi[1]).Date.AddDays(0);

            if (startime != oldstartime || endtime != oldendtime )
            {
                if (NormalDisable.IsChecked == true)
                {
                   
                    status = "Normal";
                    LoadData(startime, endtime, sensorID);
                }
                else if (RangeDisable.IsChecked == true)
                { 
                  //range error
                    NormalDisable.IsChecked = true;
                    status = "Normal";
                    LoadData(startime, endtime, sensorID);
                }
              
            }
            else 
            {
                if (NormalDisable.IsChecked == true)
                {
                   
                    status = "Normal";
                    LoadData(startime, endtime, sensorID);
                }
                 else if (RangeDisable.IsChecked == true)
                {
                   
                    status = "Range";
                    LoadData(startime, endtime, sensorID);
                }
               
            }
                
                
                
                
            //    if(status !="Range")
            //{
            //    if (status == "Normal")
            //    {
                    
            //    }
            //    else if (status == "Range")
            //    { 
                
                
            //    RangeDisable.IsChecked = true;
            //    status = "Range";
            //    LoadData(startime, endtime, sensorID);
            //}
            //else if(status !="Normal")
            //{
            //    NormalDisable.IsChecked = true;
            //    status = "Normal";
            //    LoadData(startime, endtime, sensorID);
            //}
            //else
            //{
            //    MessageBox.Show("ex");
            //}
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
