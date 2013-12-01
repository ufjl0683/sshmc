using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// 使用者控制項項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234236

namespace sshmc.ChartControls
{
    public   partial class SensorChartControl : UserControl
    {
        int siteId;
        Service.SSHMCDataServiceClient client;
        public SensorChartControl( )
        {
            this.InitializeComponent();
            
       
                  //DateTime d1 = DateTime.Now;
            
                  
        }
        public    void SetSensorId(int snrid)
        {
            clendar_Crl.SelectedDate = System.DateTime.Now.Date.AddDays(-1);
            clendar_Crl.DisplayDateEnd = System.DateTime.Now.AddDays(0);


            UpdateCharts(snrid,(DateTime) clendar_Crl.SelectedDate, System.DateTime.Now.AddDays(0));

                

                  siteId = snrid;
        }

        public async Task UpdateCharts(int SensorId,DateTime StarTime,DateTime EndTime)
        {

            try
            {
            client = new Service.SSHMCDataServiceClient();

            var q = await (client.GetvwSensorValuesAndTC10MinDataLogAsync(SensorId,StarTime,EndTime));

            PlotSeriesData[] seriesdata = new PlotSeriesData[]{
                new PlotSeriesData(){ Items=new System.Collections.ObjectModel.ObservableCollection<NameValueItem>()},
                  new PlotSeriesData(){ Items=new System.Collections.ObjectModel.ObservableCollection<NameValueItem>()},
                    new PlotSeriesData(){ Items=new System.Collections.ObjectModel.ObservableCollection<NameValueItem>()}
            };

            foreach (var i in q)
            {
                if (i.TIMESTAMP.Minute != 0)
                    continue;
                seriesdata[0].Items.Add(new NameValueItem() { X_Label = i.TIMESTAMP.Hour.ToString(), Value = i.VALUE0 ?? 0 });
                seriesdata[1].Items.Add(new NameValueItem() { X_Label = i.TIMESTAMP.Hour.ToString(), Value = i.VALUE1 ?? 0 });
                seriesdata[2].Items.Add(new NameValueItem() { X_Label = i.TIMESTAMP.Hour.ToString(), Value = i.VALUE2 ?? 0 });

            }

            if (q.Count != 0)
            {
                if (LineChart0.LegendItems.Count != 0)
                    LineChart0.LegendItems.RemoveAt(0);
                if (LineChart1.LegendItems.Count != 0)
                    LineChart1.LegendItems.RemoveAt(0);
                if (LineChart2.LegendItems.Count != 0)
                    LineChart2.LegendItems.RemoveAt(0);



                //LinearAxis liAxis = LineChart0.Series[0] as LinearAxis;

                //liAxis.Background = new SolidColorBrush() { Color = Windows.UI.Colors.Green };
                ((LineSeries)LineChart0.Series[0]).Background = new SolidColorBrush() { Color = Windows.UI.Colors.Green };
                ((LineSeries)LineChart1.Series[0]).Background = new SolidColorBrush() { Color = Windows.UI.Colors.Green };
                ((LineSeries)LineChart2.Series[0]).Background = new SolidColorBrush() { Color = Windows.UI.Colors.Green };

                //Style style = new Style();
                //Setter setterWidth = new Setter(){ Property = LineDataPoint.WidthProperty,Value = 0};
                //Setter setterHeight = new Setter() { Property = LineDataPoint.HeightProperty, Value = 0 };
                
                
                //Type type = typeof(LineDataPoint);
                //style.TargetType = type;
                //style.Setters.Add(setterHeight);
                //style.Setters.Add(setterWidth);

                //((LineSeries)LineChart0.Series[0]).DataPointStyle = style;
                //((LineSeries)LineChart1.Series[0]).DataPointStyle = style;
                //((LineSeries)LineChart2.Series[0]).DataPointStyle = style;


       



                //LineChart0.Axes.;
                if (seriesdata[0].Max != seriesdata[0].Min)
                {
                    ((LineSeries)this.LineChart0.Series[0]).DependentRangeAxis = new LinearAxis() { Minimum = seriesdata[0].Min, Maximum = seriesdata[0].Max, Orientation = AxisOrientation.Y, ShowGridLines = true };
                }
                if (seriesdata[1].Max != seriesdata[1].Min)
                {
                    ((LineSeries)this.LineChart1.Series[0]).DependentRangeAxis = new LinearAxis() { Minimum = seriesdata[1].Min, Maximum = seriesdata[1].Max, Orientation = AxisOrientation.Y, ShowGridLines = true };
                }
                if (seriesdata[2].Max != seriesdata[2].Min)
                {
                    ((LineSeries)this.LineChart2.Series[0]).DependentRangeAxis = new LinearAxis() { Minimum = seriesdata[2].Min, Maximum = seriesdata[2].Max, Orientation = AxisOrientation.Y, ShowGridLines = true };
                }



                ((LineSeries)this.LineChart0.Series[0]).ItemsSource = seriesdata[0].Items;
                ((LineSeries)this.LineChart1.Series[0]).ItemsSource = seriesdata[1].Items;
                ((LineSeries)this.LineChart2.Series[0]).ItemsSource = seriesdata[2].Items;

                

                LineChart0.Title = "X";
                LineChart1.Title = "Y";
                LineChart2.Title = "Z";



                CategoryAxis cate = LineChart0.ActualAxes[0] as CategoryAxis;
                cate.Opacity = 1;
                cate.Title = "hour";

                CategoryAxis cate1 = LineChart1.ActualAxes[0] as CategoryAxis;
                cate1.Opacity = 1;
                cate1.Title = "hour";

                CategoryAxis cate2 = LineChart2.ActualAxes[0] as CategoryAxis;
                cate2.Opacity = 1;
                cate2.Title = "hour";
                //cate.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                //WinRTXamlToolkit.Controls.DataVisualization.Charting.CategoryAxis ca = new CategoryAxis();
          
            }
            else
            {
                //MessageBoxFun("無資料!!!");
                

            }
            }
            catch (Exception ex)
            {
            }

            DateTimeBlock.Text = StarTime.ToString("yyyy/MM/dd");
            
        }

        public async void MessageBoxFun(string s)
        {
            Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog(s);
            await msg.ShowAsync();
        }

        public class PlotSeriesData
        {
            public double Max
            {
                get
                {
                    return Items.Max(n => n.Value);
                }


            }
            public double Min
            {
                get
                {
                    return Items.Min(n => n.Value);
                }

            }

            public System.Collections.ObjectModel.ObservableCollection<NameValueItem> Items { get; set; }
        }
        public class NameValueItem
        {
            public string X_Label { get; set; }
            public double Value { get; set; }
        }
        
        private void DateTimeBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.clendar_Crl.Visibility = Windows.UI.Xaml.Visibility.Visible;
            clendar_Crl.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        private void clendar_Crl_LostFocus(object sender, RoutedEventArgs e)
        {
            this.clendar_Crl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void left_btn_Click(object sender, RoutedEventArgs e)
        {
           left_btn.IsEnabled = false;
        //   progress1.IsEnabled = true;
         //  progress1.Visibility = Windows.UI.Xaml.Visibility.Visible;
           progress1.IsActive = true;
           
          await  UpdateCharts(siteId, clendar_Crl.SelectedDate.Value.AddDays(-1), clendar_Crl.SelectedDate.Value.AddDays(0));
        ;
            clendar_Crl.SelectedDate = clendar_Crl.SelectedDate.Value.AddDays(-1);
            left_btn.IsEnabled = true;
            progress1.IsActive = false;
          //  progress1.IsEnabled = false;
          //  progress1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void right_btn_Click(object sender, RoutedEventArgs e)
        {
            if (clendar_Crl.SelectedDate.Value.AddDays(1).CompareTo(clendar_Crl.DisplayDateEnd.Value) <= 0)
            {
                right_btn.IsEnabled = false;
              //  progress1.IsEnabled = true;
               // progress1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                progress1.IsActive = true;
                 await UpdateCharts(siteId, clendar_Crl.SelectedDate.Value.AddDays(1), clendar_Crl.SelectedDate.Value.AddDays(2));
                clendar_Crl.SelectedDate = clendar_Crl.SelectedDate.Value.AddDays(1);
                right_btn.IsEnabled = true;
                progress1.IsActive = false;
              //  progress1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private  async  void clendar_Crl_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
           await UpdateCharts(siteId, (sender as Calendar).SelectedDate.Value, (sender as Calendar).SelectedDate.Value.AddDays(1));
            clendar_Crl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private  void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //        SetSensorId((this.DataContext as Service.vwSensorDegree).SENSOR_ID);
            //    });
        }


    }
}
