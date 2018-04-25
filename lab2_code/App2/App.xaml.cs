using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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
using Windows.UI.Core;
using Windows.Storage;
using SQLitePCL;

namespace App2
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        /// 判断是否挂起；
        public bool issuspend = false;
   
        private void LoadDatebase()
        {

            SQLiteConnection conn = new SQLiteConnection("demo.db");
            string sql = @"CREATE TABLE IF NOT EXISTS Todoitem(Image CHAR(80) ,
                                                               Title CHAR(40),
                                                               Description CHAR(160),
                                                               Date CHAR(40) );";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }
        }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            LoadDatebase();
            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态

                    //重新打开时加载的应用状态
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NavigationState")) {
                        rootFrame.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["NavigationState"]);
                    }
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
                //处理返回需求
              
            }
            rootFrame.Navigated += OnNavigated;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
          void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
            {
                throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
            }

       

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //挂起；
            issuspend = true;
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            
            //Get frame navigatation state serualized as a string and save in setting
            //程序挂起时保存应用程序状态
            Frame frame = Window.Current.Content as Frame;
           
            ApplicationData.Current.LocalSettings.Values["NavigationState"] = frame.GetNavigationState();
    
           
            deferral.Complete();
        }

        public event EventHandler<BackRequestedEventArgs > onBaackRequested;
        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (onBaackRequested != null) {
                onBaackRequested(this, e);
            }
            if (!e.Handled) {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack) {
                    rootFrame.GoBack();
                    e.Handled = true;
                }
            }
        }
        void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (((Frame)sender).CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            }
        }
        

    }
}
