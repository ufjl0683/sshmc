using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
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

// 群組項目頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234231

namespace sshmc
{
    /// <summary>
    /// 顯示項目的群組集合之頁面。
    /// </summary>
    public sealed partial class Report : sshmc.Common.LayoutAwarePage
    {
        public Report()
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
       async protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: 將可繫結群組的集合指派給 this.DefaultViewModel["Groups"]
            Service.SSHMCDataServiceClient client = new Service.SSHMCDataServiceClient();
            
            try
            {
             ObservableCollection<Service.vwReportNotified>   q = await client.GettblReportNotifiedAsync(App.UserID);
             var res = from n in q orderby n.TYPE,n.DATATIME descending
                       group n by new { n.SITE_ID ,n.SITE_NAME} into g
                       select new BindingData { Title = g.Key.SITE_NAME, TopItems = g.ToList() };
             this.DefaultViewModel["Groups"] = res.ToList();
            }
            catch (FaultException ex)
            {

                //string msg = "FaultException: " + ex.Message;
                //MessageFault fault = ex.CreateMessageFault();
                //if (fault.HasDetail == true)
                //{
                //    System.Xml.XmlReader reader = fault.GetReaderAtDetailContents();
                //    if (reader.Name == "ExceptionDetail")
                //    {
                //        ExceptionDetail detail = fault.GetDetail<ExceptionDetail>();
                //        msg += "\n\nStack Trace: " + detail.StackTrace;
                //    }
                //}


                   new MessageDialog(ex.Message).ShowAsync();
            }
           
         
        }

       private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
       {
           this.Frame.Navigate(typeof(ShowPDF), e.ClickedItem as Service.vwReportNotified);
       }
    }


        class BindingData
    {
        public string Title { get; set; }
        List<Service.vwReportNotified> _TopItems;
        public List<Service.vwReportNotified> TopItems
        {
            get { return this._TopItems; }
            set
            {
                _TopItems = value;
            }
        }
       
    }
}
