using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
           

        }
        private Todos.ViewModels.TodoItemViewModel viewmodel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootframe = Window.Current.Content as Frame;
            if (rootframe.CanGoBack) {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            viewmodel = ((Todos.ViewModels.TodoItemViewModel)e.Parameter);
            if (viewmodel.SelectedItem == null)
            {
                textbox_detail.Text = "";
                textbox_title.Text = "";
                datepicker.Date = DateTime.Now.Date;
                button_create.Content = "Create";
            }
            else {
                 textbox_detail.Text = viewmodel.SelectedItem.description;
                textbox_title.Text = viewmodel.SelectedItem.title;
                datepicker.Date = viewmodel.SelectedItem.datetime;
             
                button_create.Content = "Update";
            }
        }


        private  void button_create_Click(object sender, RoutedEventArgs e)
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

                    viewmodel.AddTodoItem(textbox_title.Text, textbox_detail.Text, datepicker.Date);
                    var message = new MessageDialog("Create successfully! ").ShowAsync();
                    Frame.Navigate(typeof(MainPage), viewmodel);
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

                    viewmodel.AddTodoItem(textbox_title.Text, textbox_detail.Text, datepicker.Date);
                    var message = new MessageDialog("Update successfully! ").ShowAsync();
                    Frame.Navigate(typeof(MainPage), viewmodel);
                }
            }


        }

    

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            if (viewmodel.SelectedItem != null)
            {
                textbox_detail.Text = "";
                textbox_title.Text = "";
                datepicker.Date = DateTime.Now.Date;
            }
            else {
                Frame.Navigate(typeof(MainPage), viewmodel);

            }

        }

        private void deleteappbarbutton_Click(object sender, RoutedEventArgs e)
        {
            textbox_detail.Text = "";
            textbox_title.Text = "";
            datepicker.Date = DateTime.Now.Date;
            button_create.Content = "Create";
            viewmodel.RemoveTodoItem(textbox_title.Text);
            Frame.Navigate(typeof(MainPage), viewmodel);

        }
    }
 }
