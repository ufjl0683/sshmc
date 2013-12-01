using System;
using System.Collections.Generic;
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

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace sshmc
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class News : sshmc.Common.LayoutAwarePage
    {
        public News()
        {
            this.InitializeComponent();

            this.webView1.Source = new Uri("ms-appx-web:///News.html");
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

        private async void webView1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //   InsertNews("cute", "2013/4/1 12:00:00", "蘋果迷照過來，iPhone5S和5C第一批水貨在台灣落地，東森新聞獨家取得實測，首次開賣的5S金色版，從機身到包裝盒都燙金，難怪價格昂貴，水貨喊價飆到7萬元，而5C的塑膠外殼，表面質感相當光滑，但要價也不便宜，水貨一隻2萬6起跳。香檳金的邊框與背部機身，連包裝盒也是貴氣十足的燙金邊，想搶先擁有一隻64G版本價格，已經喊到7萬天價。就算價格再貴，台灣的蘋果迷們也迫不急待一睹5S真面目。不只5S，首波iPhone5c也在東森新聞獨家曝光，長的跟iphone5差不多，只是穿上多彩的塑膠外。但5C水貨也不便宜，16G 2萬6，32G則要3萬元，看來不管5S還是5C，果迷們想搶先擁有新機，口袋得夠深才行。"
            // , "main");
             
            //InsertNews("cute", "2013/4/1 12:00:00", "蘋果迷照過來，iPhone5S和5C第一批水貨在台灣落地，東森新聞獨家取得實測，首次開賣的5S金色版，從機身到包裝盒都燙金，難怪價格昂貴，水貨喊價飆到7萬元，而5C的塑膠外殼，表面質感相當光滑，但要價也不便宜，水貨一隻2萬6起跳。香檳金的邊框與背部機身，連包裝盒也是貴氣十足的燙金邊，想搶先擁有一隻64G版本價格，已經喊到7萬天價。就算價格再貴，台灣的蘋果迷們也迫不急待一睹5S真面目。不只5S，首波iPhone5c也在東森新聞獨家曝光，長的跟iphone5差不多，只是穿上多彩的塑膠外。但5C水貨也不便宜，16G 2萬6，32G則要3萬元，看來不管5S還是5C，果迷們想搶先擁有新機，口袋得夠深才行。"
            //  , "main2");

            //InsertNews("cute", "2013/4/1 12:00:00", "蘋果迷照過來，iPhone5S和5C第一批水貨在台灣落地，東森新聞獨家取得實測，首次開賣的5S金色版，從機身到包裝盒都燙金，難怪價格昂貴，水貨喊價飆到7萬元，而5C的塑膠外殼，表面質感相當光滑，但要價也不便宜，水貨一隻2萬6起跳。香檳金的邊框與背部機身，連包裝盒也是貴氣十足的燙金邊，想搶先擁有一隻64G版本價格，已經喊到7萬天價。就算價格再貴，台灣的蘋果迷們也迫不急待一睹5S真面目。不只5S，首波iPhone5c也在東森新聞獨家曝光，長的跟iphone5差不多，只是穿上多彩的塑膠外。但5C水貨也不便宜，16G 2萬6，32G則要3萬元，看來不管5S還是5C，果迷們想搶先擁有新機，口袋得夠深才行。"
            // , "main3");
            //InsertNews("cute", "2013/4/1 12:00:00", "蘋果迷照過來，iPhone5S和5C第一批水貨在台灣落地，東森新聞獨家取得實測，首次開賣的5S金色版，從機身到包裝盒都燙金，難怪價格昂貴，水貨喊價飆到7萬元，而5C的塑膠外殼，表面質感相當光滑，但要價也不便宜，水貨一隻2萬6起跳。香檳金的邊框與背部機身，連包裝盒也是貴氣十足的燙金邊，想搶先擁有一隻64G版本價格，已經喊到7萬天價。就算價格再貴，台灣的蘋果迷們也迫不急待一睹5S真面目。不只5S，首波iPhone5c也在東森新聞獨家曝光，長的跟iphone5差不多，只是穿上多彩的塑膠外。但5C水貨也不便宜，16G 2萬6，32G則要3萬元，看來不管5S還是5C，果迷們想搶先擁有新機，口袋得夠深才行。"
            //, "main1");

            Service.SSHMCDataServiceClient client = new Service.SSHMCDataServiceClient();
            Exception ex = null;
            try
            {
                System.Collections.ObjectModel.ObservableCollection<Service.tblPre_disasterNotified> preDisasterInfos = await client.GetDisasterInfoAsync();
                foreach (Service.tblPre_disasterNotified info in preDisasterInfos)
                {
                    string classtype = "side";

                    if (info.PRE_ADMONISH_CLASS.Contains("雨"))
                        classtype = "main2";
                    else if (info.PRE_ADMONISH_CLASS.Contains("地震") || info.PRE_ADMONISH_CLASS.Contains("颱風"))
                        classtype = "side";
                    
                    webView1.InvokeScript("InsertNews",new string[]{info.TITLE,((DateTime)info.TIMESTAMP).ToString("yyyy/MM/dd hh:mm:ss"),info.CONTENT,classtype});
                }
            
            
            }
            catch (Exception ex1)
            {
               ex=
                   ex1; 
                
            }

            if (ex != null)
            {
              await  new MessageDialog(ex.Message, "SSHMC").ShowAsync();
            }

//webView1.InvokeScript("InsertNews",new string[]{"cute", "2013/4/1 12:00:00", "蘋果迷照過來，iPhone5S和5C第一批水貨在台灣落地，東森新聞獨家取得實測，首次開賣的5S金色版，從機身到包裝盒都燙金，難怪價格昂貴，水貨喊價飆到7萬元，而5C的塑膠外殼，表面質感相當光滑，但要價也不便宜，水貨一隻2萬6起跳。香檳金的邊框與背部機身，連包裝盒也是貴氣十足的燙金邊，想搶先擁有一隻64G版本價格，已經喊到7萬天價。就算價格再貴，台灣的蘋果迷們也迫不急待一睹5S真面目。不只5S，首波iPhone5c也在東森新聞獨家曝光，長的跟iphone5差不多，只是穿上多彩的塑膠外。但5C水貨也不便宜，16G 2萬6，32G則要3萬元，看來不管5S還是5C，果迷們想搶先擁有新機，口袋得夠深才行。"
//            , "main"});
        }
    }
}
