using Bing.Maps;
using sshmc.Service;
//using Bing.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 項目詳細資料頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234232

namespace sshmc
{
    /// <summary>
    /// 顯示群組內單一項目之詳細資料的頁面，同時允許筆勢
    /// 經由屬於相同群組的其他項目翻轉。
    /// </summary>
    /// 

  
    public sealed partial class SiteView : sshmc.Common.LayoutAwarePage
    {
       
        Service.SSHMCDataServiceClient client = new Service.SSHMCDataServiceClient();
        ObservableCollection<Service.vwSiteDegree> sites;
        DispatcherTimer tmr = new DispatcherTimer();
        public SiteView()
        {
            this.InitializeComponent();
          //  this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
             
        }

        int cnt = 0;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode== NavigationMode.Back)
            {
                App.UserID = e.Parameter.ToString();
                LoadData(App.UserID);
                tmr.Interval = TimeSpan.FromSeconds(60);
                tmr.Tick += tmr_Tick;
                tmr.Start();
            }
        }

         void tmr_Tick(object sender, object e)
        {
            LoadData(App.UserID);
      //      this.pageTitle.Text = (cnt++).ToString();
            //throw new NotImplementedException();
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
            // 允許儲存的頁面狀態覆寫要顯示的初始項目
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

         //   this.map1.InitialExtent = new ESRI.ArcGIS.Runtime.Envelope(-169840.699521129, 2403270.13928, 599177.795721129, 2818099.11252);
          
          
       
          //  map.SetView(new Location(23, 120), 12);
            
            // TODO: 將可繫結的群組指派給 this.DefaultViewModel["Group"]
            // TODO: 將可繫結項目的集合指派給 this.DefaultViewModel["Items"]
            // TODO: 將選取的項目指派給 this.flipView.SelectedItem
        }

      
        async void LoadData(string UserID)
        {
            
               
                if (sites == null)
                {
                    try
                    {
                        sites = await client.GetSiteInfoAsync(UserID);
                        listView.ItemsSource = GridViewSites.ItemsSource = sites;
                        PushPinOnMap();
                    }
                    catch (Exception ex)
                    {
                       new MessageDialog(ex.Message).ShowAsync();
                    }
                }
                else
                {
                    ObservableCollection<vwSiteDegree> temp = await client.GetSiteInfoAsync(UserID);

                    foreach (vwSiteDegree info in temp)
                    {
                        sites.Where(n => n.SITE_ID == info.SITE_ID).FirstOrDefault().CURRENT_DEGREE = info.CURRENT_DEGREE;
                    }
                }
           
              

        }

        void PushPinOnMap()
        {
            try
            {
                this.mapControls.ItemsSource = sites;
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }

        }

        /// <summary>
        /// 在應用程式暫停或從巡覽快取中捨棄頁面時，
        /// 保留與這個頁面關聯的狀態。值必須符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化需求。
        /// </summary>
        /// <param name="pageState">即將以可序列化狀態填入的空白字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
         //   var selectedItem = this.flipView.SelectedItem;
            // TODO: 衍生可序列化的巡覽參數，並將它指派給 pageState["SelectedItem"]
        }

        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            try
            {
               
                this.Frame.GoBack();
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message + "," + ex.StackTrace);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
             *  <Maps:Location Longitude="120.942114"  Latitude="23.785345"    />
             *  */
            this.map.SetView(new Location(23.785345, 120.942114), 8);
           // this.map1.ZoomTo(new ESRI.ArcGIS.Runtime.Envelope(-169840.699521129, 2403270.13928, 599177.795721129, 2818099.11252));
           
        }

        private void StackPanel_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private void StackPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
           
        }

        private void Pushpin_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void GridViewSites_ItemClick(object sender, ItemClickEventArgs e)
        {
            Service.vwSiteDegree data = e.ClickedItem as Service.vwSiteDegree;
            this.map.SetView(new Location(data.Y, data.X), 16);

        }

        private void map_ViewChanged(object sender, ViewChangedEventArgs e)
        {
            double level = (sender as Bing.Maps.Map).ZoomLevel;

            foreach (TextBlock tb in list)
                tb.Opacity = GetOpacityByZoomLevel(level);



        }

        public  double GetOpacityByZoomLevel(double ZoomLevel)
        {
            if (ZoomLevel > 12)
                return 1;
            else
                return 0;
        }

        System.Collections.Generic.List<TextBlock> list = new List<TextBlock>();
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

            list.Add(sender as TextBlock);
            (sender as TextBlock).Opacity = GetOpacityByZoomLevel(map.ZoomLevel);
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            tmr.Stop();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof( News));
        }

      

        

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Report));
        }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SensorView), (sender as Button).DataContext);
        }

        private void Grid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SensorView), (sender as Grid).DataContext);
        }

       

        
    }
}
