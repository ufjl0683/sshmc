using sshmc.ChartControls;
using sshmc.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace sshmc
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class SensorChart : sshmc.Common.LayoutAwarePage
    {
      //  DispatcherTimer tmr = new DispatcherTimer();
        public SensorChart()
        {
            this.InitializeComponent();
        //    BindContent();

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
        /// 
        private async void BindContent()
        {

        }


        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
            SensorChartControl sensorChartCrl = new SensorChartControl();
//DataService.SSHMCDataServiceClient client = new DataService.SSHMCDataServiceClient();
         
          //  sensorChartCrl.DataContext = q.FirstOrDefault();
            grid1.Children.Add(sensorChartCrl);
            vwSensorDegree snrdeg = e.Parameter as vwSensorDegree;
             sensorChartCrl.SetSensorId((int)snrdeg.SENSOR_ID);
            this.pageTitle.Text = snrdeg.SENSOR_NAME;

            

        }

        
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }
        /// <summary>
        /// 在應用程式暫停或從巡覽快取中捨棄頁面時，
        /// 保留與這個頁面關聯的狀態。值必須符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化需求。
        /// </summary>
        /// <param name="pageState">即將以可序列化狀態填入的空白字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
