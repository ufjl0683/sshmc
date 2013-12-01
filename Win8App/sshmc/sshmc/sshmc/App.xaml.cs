using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// 空白應用程式範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234227

namespace sshmc
{
    /// <summary>
    /// 提供應用程式專屬行為以補充預設的應用程式類別。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化單一應用程式物件。這是第一行執行之撰寫程式碼，
        /// 而且其邏輯相當於 main() 或 WinMain()。
        /// </summary>
        /// 
        public static  bool IsLogin=false;
        public static string UserID;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;
        }

          async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog(e.Exception.Message).ShowAsync();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 在應用程式由使用者正常啟動時叫用。其他進入點
        /// 將在啟動應用程式以開啟特定檔案時使用，以顯示
        /// 搜尋結果等。
        /// </summary>
        /// <param name="args">關於啟動要求和處理序的詳細資料。</param>
        /// 
        private Popup BuildSettingsItem(UserControl u, int w)
        {
            Popup p = new Popup();
            p.IsLightDismissEnabled = true;
            p.ChildTransitions = new TransitionCollection();
            p.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                        EdgeTransitionLocation.Right :
                        EdgeTransitionLocation.Left
            });

            u.Width = w;
            u.Height = Window.Current.Bounds.Height;
            p.Child = u;

            p.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - w) : 0);
            p.SetValue(Canvas.TopProperty, 0);

            return p;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {

            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;     
            Frame rootFrame = Window.Current.Content as Frame;

            // 當視窗已經有內容時，不重複應用程式初始化，
            // 只確定視窗是作用中
            if (rootFrame == null)
            {
                // 建立框架做為巡覽內容，並巡覽至第一頁
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 從之前暫停的應用程式載入狀態
                }

                // 將框架放在目前視窗中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 在巡覽堆疊未還原時，巡覽至第一頁，
                // 設定新的頁面，方式是透過傳遞必要資訊做為巡覽
                // 參數
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // 確定目前視窗是作用中
            Window.Current.Activate();
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand command = new SettingsCommand("About", "關於 SSHMC", (handler) =>
            {
                //Popup popup = BuildSettingsItem(new AboutPage(), 646);
                //popup.IsOpen = true;

                
                    Popup popup = BuildSettingsItem(new About(),600);
                    popup.IsOpen = true;
               

                

            });

            args.Request.ApplicationCommands.Add(command);

           // throw new NotImplementedException();
        }

        /// <summary>
        /// 在應用程式暫停執行時叫用。應用程式狀態會儲存起來，
        /// 但不知道應用程式即將結束或繼續，而且仍將記憶體
        /// 的內容保持不變。
        /// </summary>
        /// <param name="sender">暫停之要求的來源。</param>
        /// <param name="e">有關暫停之要求的詳細資料。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 儲存應用程式狀態，並停止任何背景活動
            deferral.Complete();
        }

        /// <summary>
        /// 在啟用應用程式以顯示搜尋結果時叫用。
        /// </summary>
        /// <param name="args">有關啟用要求的詳細資料。</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: 在 OnWindowCreated 中註冊 Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // 事件，以加速已經執行之應用程式的搜尋速度

            // 如果視窗尚未使用框架巡覽，則插入我們自己的框架
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // 如果應用程式不包含最上層框架，這可能是
            // 應用程式的初始啟動。一般而言，這個方法和 App.xaml.cs 中的 OnLaunched 
            // 可以呼叫共同的方法。
            if (frame == null)
            {
                // 建立框架做為巡覽內容，並與
                // SuspensionManager 機碼產生關聯
                frame = new Frame();
                sshmc.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 只在適當時還原儲存的工作階段狀態
                    try
                    {
                        await sshmc.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (sshmc.Common.SuspensionManagerException)
                    {
                        //發生狀況，還原狀態。
                        //假定沒有狀態，並繼續
                    }
                }
            }

            frame.Navigate(typeof(SearchResultsPage1), args.QueryText);
            Window.Current.Content = frame;

            // 確定目前視窗是作用中
            Window.Current.Activate();
        }
    }
}
