using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.IO;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage1 : Page
    {
        public NewPage1()
        {
            this.InitializeComponent();
            viewmodel = Todos.ViewModels.TodoItemViewModel.GetInstance();

        }
        private Todos.ViewModels.TodoItemViewModel viewmodel;
        public BitmapImage image_tem ;
        public string imageuri ;

        //打卡页面为此页面时进行的操作
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            if (rootframe != null)
            {
                //跳转按钮
                if (rootframe.CanGoBack)
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }

            //是否为从mianpage继承页面内容 
            viewmodel.SelectedItem = (Todos.Models.TodoItem)(e.Parameter);
         
            //是否为从mianpage继承页面内容
            if (viewmodel.SelectedItem == null)
            {
                BitmapImage bitmapimage = new BitmapImage(new Uri("ms-appx:///Assets/bar.jpg"));
                imageuri = "ms-appx:///Assets/bar.jpg";
                image.Source = bitmapimage;
                textbox_detail.Text = "";
                textbox_title.Text = "";
                datepicker.Date = DateTime.Now.Date;
                button_create.Content = "Create";
            }
            else {
                image_tem = viewmodel.SelectedItem.image;
                imageuri = image_tem.UriSource.ToString();
                image.Source = viewmodel.SelectedItem.image;
                textbox_detail.Text = viewmodel.SelectedItem.description;
                textbox_title.Text = viewmodel.SelectedItem.title;
                datepicker.Date = viewmodel.SelectedItem.datetime;
                button_create.Content = "Update";
            }
            
            //从挂起关闭，到重新启动是需要加载的数据
            if (e.NavigationMode == NavigationMode.New)
            {
                //如果先前没有挂起并关闭，则将新建的移除
                ApplicationData.Current.LocalSettings.Values.Remove("newpage1_store");
            }
            else
            {
                //如果正常挂起并关闭，就读取在DataCompate中的数据
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newpage1_store"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["newpage1_store"] as ApplicationDataCompositeValue;
                    textbox_title.Text = (string)composite["title"];
                    textbox_detail.Text = (string)composite["description"];
                    DateTimeOffset tem = DateTimeOffset.Parse((string)composite["date"]);
                    datepicker.Date = tem;

                    StorageFile thefile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync((string)ApplicationData.Current.LocalSettings.Values["newpage1_image"]);
                    var stream = await thefile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    image.Source = bitmapImage;
                    image_tem = bitmapImage;

                    //We're done with it ,so remove it。在使用完后清除储存的数据项
                    ApplicationData.Current.LocalSettings.Values.Remove("newpage1_store");
                   
                }
            }
            
        }


        //在挂起是进行过的操作
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = textbox_title.Text.ToString();
                composite["description"] = textbox_detail.Text.ToString();
                composite["date"] = datepicker.Date.ToString();
                //储存数据项
                ApplicationData.Current.LocalSettings.Values["newpage1_store"] = composite;
            }
        }

        //点击create button时的函数，判断是否合法，并添加或更新TodoItem
        private  void button_create_Click(object sender, RoutedEventArgs e)
        {
            //如果没有创建
            if (viewmodel.SelectedItem == null) 
            {
                if (textbox_title.Text == "")
                {
                    var messages = new MessageDialog("Title is empty").ShowAsync();
                }
                if (textbox_detail.Text == "")
                {
                    var message = new MessageDialog("Detail is empty").ShowAsync();
                }
                if (datepicker.Date < DateTime.Now.Date)
                {
                    var message = new MessageDialog("Date is before today ").ShowAsync();

                }
                if (image.Source !=null && textbox_title.Text != "" && textbox_detail.Text != "" && datepicker.Date >= DateTime.Now.Date)
                {
                    
                    viewmodel.AddTodoItem(imageuri,textbox_title.Text, textbox_detail.Text, datepicker.Date);
                    updateTile();
                    var message = new MessageDialog("Create successfully! ").ShowAsync();
                    
                }
                
            }
            else
            {
                if (textbox_title.Text == "")
                {
                    var messages = new MessageDialog("Title is empty").ShowAsync();
                }
                if (textbox_detail.Text == "")
                {
                    var message = new MessageDialog("Detail is empty").ShowAsync();
                }
                if (datepicker.Date < DateTime.Now.Date)
                {
                    var message = new MessageDialog("Date is before today ").ShowAsync();

                }
                if (textbox_title.Text != "" && textbox_detail.Text != "" && datepicker.Date >= DateTime.Now.Date)
                {

                    viewmodel.UpdateTodoItem (imageuri,textbox_title.Text, textbox_detail.Text, datepicker.Date);
                    updateTile();
                    var message = new MessageDialog("Update successfully! ").ShowAsync();
         
                }
            }


        }

    
        //cancel button的操作，清空title，detail。重置date，image
        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            if (viewmodel.SelectedItem != null)
            {
                textbox_detail.Text = "";
                textbox_title.Text = "";
                datepicker.Date = DateTime.Now.Date;
            }
            else {
                Frame.Navigate(typeof(MainPage));

            }

        }

        //appbar上的删除按钮，删除Todoitem
        private void deleteappbarbutton_Click(object sender, RoutedEventArgs e)
        {
            textbox_detail.Text = "";
            textbox_title.Text = "";
            datepicker.Date = DateTime.Now.Date;
            button_create.Content = "Create";
            viewmodel.RemoveTodoItem(textbox_title.Text);
            Frame.Navigate(typeof(MainPage));

        }

        //图片选择
        private async void barbutton_select_click(object sender, RoutedEventArgs e)
        {
            var filepicker = new Windows.Storage.Pickers.FileOpenPicker();
            filepicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            filepicker.SuggestedStartLocation =
            Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            filepicker.FileTypeFilter.Add(".jpg");
            filepicker.FileTypeFilter.Add(".jpeg");
            filepicker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await filepicker.PickSingleFileAsync();  
            if (file != null)
            {
                await file.CopyAsync(ApplicationData.Current.LocalFolder, file.Name, NameCollisionOption.ReplaceExisting);
                imageuri = "ms-appdata:///local/" + file.Name;
                BitmapImage bitmapImage = new BitmapImage(new Uri(imageuri));
                image.Source = bitmapImage;
                image_tem = bitmapImage;;
                ApplicationData.Current.LocalSettings.Values["newpage1_image"] = StorageApplicationPermissions.FutureAccessList.Add(file);

            }
            else
            {
                BitmapImage bitmapimage = new BitmapImage(new Uri("ms-appx:///Assets/bar.jpg"));
                image.Source = bitmapimage;
                image_tem = bitmapimage;
                imageuri = "ms-appx:///Assets/bar.jpg";
            }
        }

        //更新磁贴
        private void updateTile()
        {
            //使用TileUpdateManager来创建磁贴更新队列
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            foreach (var todo in viewmodel.AllItem)
            {
                XmlDocument xml = new XmlDocument();
                //读取Tile.xml中的内容
                xml.LoadXml(File.ReadAllText("Tile.xml"));
                //通过标签来定位，text 标签有title 与discrption两种
                XmlNodeList text = xml.GetElementsByTagName("text");
                XmlNodeList image = xml.GetElementsByTagName("image");

                //修改所有的text标签内容
                for (int i = 0; i < text.Count; i++)
                {
                    ((XmlElement)text[i]).InnerText = todo.title;
                    i++;
                    ((XmlElement)text[i]).InnerText = todo.description;
                }
                foreach (var element in image)
                {
                    BitmapImage bitmapimage = (BitmapImage)todo.image;
                    //只能加载Assets中的图片
                    ((XmlElement)element).SetAttribute("src", bitmapimage.UriSource.ToString());
                }


                var notification = new TileNotification(xml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

            }
        }

    }
}
