using sshmc.ChartControls;
using sshmc.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 群組詳細資料頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234229

namespace sshmc
{
    /// <summary>
    /// 顯示單一群組概觀的頁面，包括群組內項目的預覽
    /// 。
    /// </summary>
    public sealed partial class SensorView : sshmc.Common.LayoutAwarePage
    {

        DispatcherTimer tmr = new DispatcherTimer();
        vwSiteDegree current_vwSiteDegree;
        public SensorView()
        {
            this.InitializeComponent();
          
        }

        async void tmr_Tick(object sender, object e)
        {
            BindingSensorData data;
            if (current_vwSiteDegree == null) return;
            this.DefaultViewModel["Group"] = data = await GenerateBindingData( current_vwSiteDegree);
            

            //throw new NotImplementedException();
        }


        protected   async override void OnNavigatedTo(NavigationEventArgs e)
        {
            BindingSensorData data;
            base.OnNavigatedTo(e);
            current_vwSiteDegree = e.Parameter as vwSiteDegree;
            this.DefaultViewModel["Group"] = data = await GenerateBindingData(current_vwSiteDegree);
            this.MapControls.ItemsSource = data.Items;
            this.cctvControls.ItemsSource = data.cctvs;
            foreach (tblCCTV cctv in data.cctvs)
            {
                Controls.CCTV cctvctl = new Controls.CCTV() { DataContext = cctv };

                cctvctl.Margin = new Thickness(0);
                cctvctl.Width = 300;
                cctvctl.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                cctvctl.Tapped += cctvctl_Tapped;
                
                this.stkCCTV.Children.Add(cctvctl);
            }
           // tmp.DataContext = data.Items.FirstOrDefault(n => n.CURRENT_DEGREE == 3);
            this.map.SetView(new Bing.Maps.Location() { Longitude = data.X, Latitude = data.Y }, 19);
            tmr.Interval = TimeSpan.FromSeconds(60);
            tmr.Tick += tmr_Tick;
            tmr.Start();
        
        }

      

        Controls.CCTV largeCCTV = null;
        void cctvctl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Controls.CCTV tapedCCTV = sender as Controls.CCTV;
            if (largeCCTV!= null)
            {
                this.LayoutRoot.Children.Remove(largeCCTV);

            }
              largeCCTV=new Controls.CCTV(){Margin=new Thickness(10), DataContext=tapedCCTV.DataContext,HorizontalAlignment= Windows.UI.Xaml.HorizontalAlignment.Stretch,VerticalAlignment= Windows.UI.Xaml.VerticalAlignment.Stretch};
              Grid.SetRow(largeCCTV,2);
              Grid.SetColumn(largeCCTV,1);
              this.LayoutRoot.Children.Add(largeCCTV);
              Grid.SetRowSpan(this.grdMap, 1);
              this.map.SetView(new Bing.Maps.Location()
              {
                  Longitude = (double)(tapedCCTV.DataContext as tblCCTV).X,
                  Latitude = (double)(tapedCCTV.DataContext as tblCCTV).Y
              });

              largeCCTV.DoubleTapped += largeCCTV_DoubleTapped;
             
            //throw new NotImplementedException();
        }

        void largeCCTV_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            //throw new NotImplementedException();
            Controls.CCTV tapedCCTV = sender as Controls.CCTV;
            if (largeCCTV != null)
            {
                this.LayoutRoot.Children.Remove(largeCCTV);

            }

            largeCCTV = null;//  new Controls.CCTV() { Margin = new Thickness(10), DataContext = tapedCCTV.DataContext, HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch };

            //  this.LayoutRoot.Children.Add(largeCCTV);
            Grid.SetRowSpan(this.grdMap, 2);
            this.map.SetView(new Bing.Maps.Location()
            {
                Longitude = (double)(tapedCCTV.DataContext as tblCCTV).X,
                Latitude = (double)(tapedCCTV.DataContext as tblCCTV).Y
            });
          //  throw new NotImplementedException();
        }

        

      async  Task< BindingSensorData> GenerateBindingData(vwSiteDegree info)
        {
          Service.SSHMCDataServiceClient client=new SSHMCDataServiceClient();
           var snrinfo=  await  client.GetSensorInfoAsync(info.SITE_ID);
           var cctvs = await client.GetCCTVInfoAsync(info.SITE_ID);

            BindingSensorData result = new BindingSensorData
            {
                Title = info.CUSTOMER_NAME,
                X=info.X,
                Y=info.Y,
                Subtitle=info.SITE_NAME,
                cctvs=cctvs,
                Items= snrinfo

            };
            return result;
        }
        /// <summary>
        /// 巡覽期間以傳遞的內容填入頁面。從之前的工作階段
        /// 重新建立頁面時，也會提供儲存的狀態。
        /// </summary>
        /// <param name="navigationParameter">最初要求這個頁面時，傳遞到
        /// <see cref="Frame.Navigate(Type, Object)"/> 的參數。
        /// </param>
        /// <param name="pageState">這個頁面在先前的工作階段期間保留的
        /// 狀態字典。第一次瀏覽頁面時，這一項是 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: 將可繫結的群組指派給 this.DefaultViewModel["Group"]
            // TODO: 將可繫結項目的集合指派給 this.DefaultViewModel["Items"]
        }

        private void SensorChartControl_Loaded(object sender, RoutedEventArgs e)
        {
            SensorChartControl ctl = sender as SensorChartControl;
             
          //  ctl.SetSensorId((ctl.DataContext as vwSensorDegree).SENSOR_ID);
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            vwSensorDegree snrdeg = (sender as Grid).DataContext as vwSensorDegree;
            this.map.SetView(new Bing.Maps.Location()
            {
                Longitude = (double)snrdeg.X,
                Latitude = (double)snrdeg.Y
            });
        }

        private void Grid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            vwSensorDegree snrdeg = (sender as Grid).DataContext as vwSensorDegree;
            this.Frame.Navigate(typeof(SensorChart), snrdeg);
        }

        private void itemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }
    }


      class BindingSensorData
    {

        public string Title{get;set;}
        public double X { get; set; }
        public double Y { get; set; }
        public string Subtitle { get; set; }
        public ObservableCollection<vwSensorDegree>   Items { get; set; }
        public ObservableCollection<tblCCTV> cctvs { get; set; }

    }
}
