using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 搜尋合約項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234240

namespace sshmc
{
    /// <summary>
    /// 此頁面會在全域搜尋導向此應用程式時顯示搜尋結果。
    /// </summary>
    public sealed partial class SearchResultsPage1 : sshmc.Common.LayoutAwarePage
    {

        public SearchResultsPage1()
        {
            this.InitializeComponent();
            
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
            var queryText = navigationParameter as String;

            // TODO: 應用程式專屬的搜尋邏輯。搜尋處理序會負責
            //       建立使用者可選取之結果分類的清單:
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       只有第一個篩選 (通常是 "All") 應該傳遞 true 做為第三個引數，才能
            //       在作用狀態中啟動。作用中篩選的結果會提供
            //       在以下的 Filter_SelectionChanged。

            var filterList = new List<Filter>();
            filterList.Add(new Filter("All", 0, true));

            // 經由檢視模型通訊結果
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        /// <summary>
        /// 在使用 ComboBox 於快照檢視狀態中選取篩選時叫用。
        /// </summary>
        /// <param name="sender">ComboBox 執行個體。</param>
        /// <param name="e">描述選取的篩選如何變更之事件資料。</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 判斷已選取的篩選
            var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            if (selectedFilter != null)
            {
                // 將結果鏡像至對應的 Filter 物件以允許
                // 在非快照檢視中反映變更時使用 RadioButton 表示
                selectedFilter.Active = true;

                // TODO: 藉由設定 this.DefaultViewModel["Results"] 回應作用中篩選的變更
                //       針對含可繫結之 Image、Title、Subtitle 和 Description 屬性之項目的集合

                // 確定找到結果

                if (!App.IsLogin)
                {

                    VisualStateManager.GoToState(this, "NotLogin", true);
                    return;
                }
                object results;
                ICollection resultsCollection;
                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as ICollection) != null &&
                    resultsCollection.Count != 0)
                {
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // 沒有搜尋結果時顯示資訊文字。
           
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// 在於非快照時使用 RadioButton 選取篩選時叫用。
        /// </summary>
        /// <param name="sender">選取的 RadioButton 執行個體。</param>
        /// <param name="e">描述如何選取 RadioButton 之事件資料。</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // 將變更鏡像至對應之 ComboBox 所使用的 CollectionViewSource
            // 以確保在快照時反映變更
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        /// <summary>
        /// 檢視描述可用於檢視搜尋結果之其中一個篩選的模型。
        /// </summary>
        private sealed class Filter : sshmc.Common.BindableBase
        {
            private String _name;
            private int _count;
            private bool _active;

            public Filter(String name, int count, bool active = false)
            {
                this.Name = name;
                this.Count = count;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
            }
        }
    }
}
