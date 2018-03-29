using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Todos.Models;
using Todos.ViewModels;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.DataTransfer;
using System.Reflection;
using Windows.Storage.Streams;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
       

        public MainPage()
        {
            this.InitializeComponent();
            this.viewmodel = new Todos.ViewModels.TodoItemViewModel();
        }

        Todos.ViewModels.TodoItemViewModel viewmodel { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter.GetType() == typeof(Todos.ViewModels.TodoItemViewModel))
            {
                this.viewmodel = (Todos.ViewModels.TodoItemViewModel)(e.Parameter);

            }
        }

        private void deleteappbarbutton_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.RemoveTodoItem(this.textbox_title.Text);
            textbox_detail.Text = "";
            textbox_title.Text = "";
            datepicker.Date = DateTime.Now.Date;
            button_create.Content = "Create";
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            viewmodel.SelectedItem = (Todos.Models.TodoItem)(e.ClickedItem);
            if (Window.Current.Bounds.Width <= 800)
            {
                Frame.Navigate(typeof(NewPage1), viewmodel);
            }
            else {
                if (viewmodel.SelectedItem == null)
                {
                    button_create.Content = "Create";
                }
                else {
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
                if(textbox_title.Text != ""&& textbox_detail.Text != ""&& datepicker.Date >= DateTime.Now.Date)
                {
                    viewmodel.AddTodoItem(textbox_title.Text, textbox_detail.Text, datepicker.Date);
                    var message = new MessageDialog("Create successfully! ").ShowAsync();
                }
            }
            else {
                viewmodel.updateTodoItem(textbox_title.Text, textbox_detail.Text, datepicker.Date);
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

        private void addappbarbutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage1), viewmodel);
        }

       
    }


}
       
        











