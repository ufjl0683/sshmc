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
    public sealed partial class ShowPDF : sshmc.Common.LayoutAwarePage
    {
        public ShowPDF()
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
        /// 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            //<iframe src="http://docs.google.com/viewer?url=http%3A%2F%2F192.192.161.4%2Fpic%2Freport%2FA11491-101-001-R2.pdf&embedded=true" width="600" height="780" style="border: none;"></iframe>
            base.OnNavigatedTo(e);
           //string htmlstr =
             //   "<!doctype html><html><head>	<title>CSS3 Multiple Column Layout</title> </head>" +
             //   "<body> <iframe src=\"http://docs.google.com/viewer?" + e.Parameter.ToString() + "&embedded=true\" width=\"100%\" style=\"border: none;\"></iframe>";
            Service.vwReportNotified vwNotifier = e.Parameter as Service.vwReportNotified;
            this.pageTitle.Text = vwNotifier.SITE_NAME;
            string htmlstr = "https://docs.google.com/viewer?url="+ vwNotifier.URL+"&embedded=true";
            this.webview1.Navigate(new Uri(htmlstr));
            //string url = e.Parameter.ToString();
            //this.webview1.Source =;
           // this.webview1.Navigate( new Uri(url,UriKind.Absolute));
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
