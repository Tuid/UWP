using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using SQLitePCL;
using System.Collections.Generic;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();
            this.viewmodel =  Todos.ViewModels.TodoItemViewModel.GetInstance();
        }

        Todos.ViewModels.TodoItemViewModel viewmodel { get; set; }
        public string share_title;
        public string share_description;
        public ImageSource share_image;
        public ImageSource image_tem;
        public string imageuri;
        public static int tem_image_count;



        //跳转到当前页面
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {   //更新磁贴
            updateTile();
            //share request++;
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            //在挂起后，重启时进行的操作
            if (e.NavigationMode == NavigationMode.New)
            {
                //如果先前没有挂起并关闭，则将新建的移除
                ApplicationData.Current.LocalSettings.Values.Remove("mainpage_store");
            }
            else {
                //如果正常挂起并关闭，就读取在DataCompate中的数据
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mainpage_store")) {
                    var composite = ApplicationData.Current.LocalSettings.Values["mainpage_store"] as ApplicationDataCompositeValue;
                  
                    textbox_title.Text = (string)composite["title"];
                    textbox_detail.Text = (string)composite["description"];
                    DateTimeOffset tem = DateTimeOffset.Parse((string)composite["date"]);
                    datepicker.Date = tem;

                    StorageFile thefile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync((string)ApplicationData.Current.LocalSettings.Values["mainpage_image"]);
                    var stream = await thefile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    imagepicker.Source = bitmapImage;
                    image_tem = bitmapImage;

                    for (int i = 0; i < Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem.Count(); i++) {
                      Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].completed = (bool)composite["ischeck" + i];
                        Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].title = (string)composite["viewmodeltitle" + i];
                        Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].description = (string)composite["viewmodeldescription" + i];
                        DateTimeOffset dateTime = DateTimeOffset.Parse((string)composite["viewmodeldatetime" + i]);
                        Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].datetime = dateTime;

                    }
                //We're done with it ,so remove it。在使用完后清除储存的数据项
                    ApplicationData.Current.LocalSettings.Values.Remove("mainpage_store");
                
                }
            }
        }

        //在挂起是进行过的操作
        protected  override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //share request--;
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = textbox_title.Text.ToString();
                composite["description"] = textbox_detail.Text.ToString();
                composite["date"] =datepicker.Date.ToString();


                for (int i = 0; i < Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem.Count(); i++)
                 {
                     composite["ischeck" + i] = Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].completed;
                     composite["viewmodeltitle" + i] = Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].title;
                     composite["viewmodeldescription" + i] = Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].description;
                     composite["viewmodeldatetime" + i] = Todos.ViewModels.TodoItemViewModel.GetInstance().AllItem[i].datetime.ToString();
                }

                  
                //储存数据项
                ApplicationData.Current.LocalSettings.Values["mainpage_store"] = composite;

            }
        }


        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            viewmodel.SelectedItem = (Todos.Models.TodoItem)(e.ClickedItem);

            viewmodel.SelectedItem.completed = true;
            if (Window.Current.Bounds.Width <= 800)
            {
                Frame.Navigate(typeof(NewPage1),e.ClickedItem);
            }
            else {
                if (viewmodel.SelectedItem == null)
                {
                    button_create.Content = "Create";
                }
                else {
                    imagepicker.Source = viewmodel.SelectedItem.image;
                    textbox_title.Text = viewmodel.SelectedItem.title;
                    textbox_detail.Text = viewmodel.SelectedItem.description;
                    datepicker.Date = viewmodel.SelectedItem.datetime;
                    button_create.Content = "Update";
                }
            }

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewmodel.SelectedItem == null) //如果没有创建
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
                    if (image_tem == null) {
                        ImageSource bitmapimage = new BitmapImage(new Uri("ms-appx:///Assets/bar.jpg"));
                        image_tem = bitmapimage;
                        imageuri = "ms-appx:///Assets/bar.jpg";
                    }
                    viewmodel.AddTodoItem(imageuri, textbox_title.Text, textbox_detail.Text, datepicker.Date);
                    updateTile();//更新磁贴
                    var message = new MessageDialog("Create successfully! ").ShowAsync();
                }
            }
            else {
                if (imageuri == null) {
                    imageuri = "ms-appx:///Assets/bar.jpg";
                }
                viewmodel.UpdateTodoItem(imageuri, textbox_title.Text, textbox_detail.Text, datepicker.Date);
                updateTile();//更新磁贴
                var message = new MessageDialog("Update successfully! ").ShowAsync();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            textbox_detail.Text = "";
            textbox_title.Text = "";
            datepicker.Date = DateTime.Now.Date;
            button_create.Content = "Create";
            viewmodel.SelectedItem = null;


        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void AppBarButtonAddClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage1));
        }

        private void appbarbutton_edit_Click(object sender, RoutedEventArgs e)
        {   
            Frame.Navigate(typeof(NewPage1));
        }

        private void appbarbutton_delete_Click(object sender, RoutedEventArgs e)
        {
            
            viewmodel.RemoveTodoItem(this.textbox_title.Text);
            textbox_detail.Text = "";
            textbox_title.Text = "";
            datepicker.Date = DateTime.Now.Date;
            button_create.Content = "Create";
        }

        //选取图片
        public async void barbutton_select_click(object sender, RoutedEventArgs e)
        {
           
            Windows.Storage.Pickers.FileOpenPicker filepicker = new Windows.Storage.Pickers.FileOpenPicker();
            filepicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            filepicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            filepicker.FileTypeFilter.Add(".jpg");
            filepicker.FileTypeFilter.Add(".jpeg");
            filepicker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await filepicker.PickSingleFileAsync();
            if (file != null)
            {   
                //将图片暂时复制保存到应用的文件中
                await file.CopyAsync(ApplicationData.Current.LocalFolder, tem_image_count + file.Name, NameCollisionOption.ReplaceExisting);
                imageuri = "ms-appdata:///local/" + tem_image_count + file.Name;
                BitmapImage bitmapImage = new BitmapImage(new Uri(imageuri));
                imagepicker.Source = bitmapImage;
                image_tem = bitmapImage;
                tem_image_count++;
                ApplicationData.Current.LocalSettings.Values["mainpage_image"] = StorageApplicationPermissions.FutureAccessList.Add(file);
            }
            else
            {
                ImageSource bitmapimage = new BitmapImage(new Uri("ms-appx:///Assets/bar.jpg"));
                imagepicker.Source = bitmapimage;
                image_tem = bitmapimage;
                imageuri = "ms-appx:///Assets/bar.jpg";


            }
        }

        private  void appbarbutton_share_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            var todoitem = (Todos.Models.TodoItem)s.DataContext;
            share_title = todoitem.title;
            share_description = todoitem.description;
            share_image = todoitem.image;
            DataTransferManager.ShowShareUI();
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
                    //只能加载本地的图片
                    if (todo.image != null)
                        ((XmlElement)element).SetAttribute("src", bitmapimage.UriSource.ToString());
                    else {
                        ((XmlElement)element).SetAttribute("src", "Assets/bar.jpg");
                    }
                }


                var notification = new TileNotification(xml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

            }
        }

        //share request;
        void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            request.Data.Properties.Title =share_title ;
            request.Data.Properties.Description = share_description;
            request.Data.SetText(share_description);
            BitmapImage bitmap = (BitmapImage)share_image;
            //share 本地中的图片
            request.Data.SetBitmap(Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(bitmap.UriSource));
            DataRequestDeferral deferral = request.GetDeferral();
            deferral.Complete(); 
        }

        //search
        public void button_search_Click(object sender, RoutedEventArgs e)
        {
             List < Todos.Models.TodoItem > todolist = new List<Todos.Models.TodoItem>();
            var db = new SQLiteConnection("demo.db");
            string sql = @"SELECT Title,Description,Date,Image From Todoitem  WHERE Title Like ? OR Description LIKE ? OR Date LIKE ?;";
            //Query 使用SELECE 查询
            using (var todoitem = db.Prepare(sql))
            {
                todoitem.Bind(1,"%"+textbox_search.Text.ToString()+"%");
                todoitem.Bind(2,"%" +textbox_search.Text.ToString()+"%");
                todoitem.Bind(3,"%" +textbox_search.Text.ToString()+"%");
                while (SQLiteResult.ROW == todoitem.Step()) {
                    string Title = (string)todoitem[0];
                    string Description = (string)todoitem[1];
                    string Date = (string)todoitem[2];
                    string Image = (string)todoitem[3];
                    BitmapImage bitmapImage_1 = new BitmapImage();
                    bitmapImage_1.UriSource = new Uri(Image);
                    todolist.Add(new Todos.Models.TodoItem(bitmapImage_1, Title, Description, DateTimeOffset.Parse( Date)));
                   
                }

            }
            if (todolist.Count == 0)
            {
                var message = new MessageDialog("Can not find\n").ShowAsync();
            }
            else
            {
                string messages = "";
                for (int i = 0; i < todolist.Count(); i++)
                {
                    messages += i+"  Title:" + todolist[i].title + "   Description:" + todolist[i].description + "   Date:" + todolist[i].datetime.ToString()+"\n"; 
                }
              
                var message = new MessageDialog(messages).ShowAsync();
              
                
            }
        }

        private void AppBarButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            viewmodel.RemoveTodoItem(this.textbox_title.Text);
            textbox_detail.Text = "";
            textbox_title.Text = "";
            ImageSource imageuri = new BitmapImage(new Uri("ms-appx:///Assets/bar.jpg"));
            imagepicker.Source = imageuri;
            datepicker.Date = DateTime.Now.Date;
            button_create.Content = "Create";
        }
    }


   
}
       
        











