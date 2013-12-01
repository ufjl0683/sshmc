using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace sshmc
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : sshmc.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        /// <summary>
        /// 在此頁面即將顯示在框架中時叫用。
        /// </summary>
        /// <param name="e">描述如何到達此頁面的事件資料。Parameter
        /// 屬性通常用來設定頁面。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Storyboard1.Begin();
            App.IsLogin = false;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Exception ex = null;
            Service.SSHMCDataServiceClient client = new Service.SSHMCDataServiceClient();
            try
            {
                bool success = await client.CheckUserIDPasswordAsync(txtAccount.Text.Trim(), txtpwd.Password.Trim());
           
                    if (success)
                    {
                        this.Frame.Navigate(typeof(SiteView), txtAccount.Text);
                        App.IsLogin = true;
                    }
                    else
                    {
                        MessageDialog dlg = new MessageDialog("帳號或密碼錯誤!", "SSHMC");
                        await dlg.ShowAsync();
                    }

            }
            catch (Exception ex1)
            {
                ex = ex1;
            }
            if (ex != null)
                await new MessageDialog(ex.Message).ShowAsync();


        }


        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // 允許儲存的頁面狀態覆寫要顯示的初始項目
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }



            //  map.SetView(new Location(23, 120), 12);

            // TODO: 將可繫結的群組指派給 this.DefaultViewModel["Group"]
            // TODO: 將可繫結項目的集合指派給 this.DefaultViewModel["Items"]
            // TODO: 將選取的項目指派給 this.flipView.SelectedItem
        }

        /// <summary>
        /// 在使用 ComboBox 於快照檢視狀態中選取篩選時叫用。
        /// </summary>
        /// <param name="sender">ComboBox 執行個體。</param>
        /// <param name="e">描述選取的篩選如何變更之事件資料。</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 判斷已選取的篩選
            //var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            //if (selectedFilter != null)
            //{
            //    // 將結果鏡像至對應的 Filter 物件以允許
            //    // 在非快照檢視中反映變更時使用 RadioButton 表示
            //    selectedFilter.Active = true;

            //    // TODO: 藉由設定 this.DefaultViewModel["Results"] 回應作用中篩選的變更
            //    //       針對含可繫結之 Image、Title、Subtitle 和 Description 屬性之項目的集合

            //    // 確定找到結果

            //    if (!App.IsLogin)
            //    {

            //        VisualStateManager.GoToState(this, "NotLogin", true);
            //        return;
            //    }
            //    object results;
            //    ICollection resultsCollection;
            //    if (this.DefaultViewModel.TryGetValue("Results", out results) &&
            //        (resultsCollection = results as ICollection) != null &&
            //        resultsCollection.Count != 0)
            //    {
            //        VisualStateManager.GoToState(this, "ResultsFound", true);
            //        return;
            //    }
            //}

            //// 沒有搜尋結果時顯示資訊文字。

            //VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// 在於非快照時使用 RadioButton 選取篩選時叫用。
        /// </summary>
        /// <param name="sender">選取的 RadioButton 執行個體。</param>
        /// <param name="e">描述如何選取 RadioButton 之事件資料。</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // 將變更鏡像至對應之 ComboBox 所使用的 CollectionViewSource
            //// 以確保在快照時反映變更
            //if (filtersViewSource.View != null)
            //{
            //    var filter = (sender as FrameworkElement).DataContext;
            //    filtersViewSource.View.MoveCurrentTo(filter);
            //}
        }
    }
}
