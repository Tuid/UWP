using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using SQLitePCL;

namespace Todos.ViewModels
{


    public class TodoItemViewModel {

        private static ViewModels.TodoItemViewModel viewmodel;//单例模式
        private TodoItemViewModel(){
            ;
        }

        //单例模式
        public static TodoItemViewModel GetInstance() {
         /*   if (viewmodel == null)
            {
                //创建两个例子
                viewmodel = new TodoItemViewModel();
                BitmapImage bitmapImage_1 = new BitmapImage();
                bitmapImage_1.UriSource = new Uri("ms-appx:///Assets/background.jpg");
                viewmodel.allItem.Add(new Models.TodoItem(bitmapImage_1, "Todolist_1", "Hello world !", DateTime.Today));
                var db = new SQLiteConnection("demo.db");
                string sql = @"INSERT INTO Todoitem(Image,Title,Description,Date) VALUES (?,?,?,?);";
                using (var todoitem = db.Prepare(sql))
                {
                    todoitem.Bind(1, bitmapImage_1.ToString());
                    todoitem.Bind(2, "Todolist_1");
                    todoitem.Bind(3, "Hello world !");
                    todoitem.Bind(4, DateTime.Today.ToString());
                    todoitem.Step();
                }

            }*/
           // else
          //  {
                using (var db = new SQLiteConnection("demo.db"))
                {
                viewmodel = new TodoItemViewModel();
                string sql = @"SELECT Image,Title,Description,Date FROM Todoitem;";
                    using (var statement = db.Prepare(sql))
                    {
                        while (SQLiteResult.ROW == statement.Step())
                        {
                            BitmapImage bitmapImage_1 = new BitmapImage();
                        if ((string)statement[0] == null)
                        {
                            bitmapImage_1.UriSource = new Uri("ms-appx:///Assets/bar.jpg");
                        }
                        else {
                            bitmapImage_1.UriSource = new Uri((string)statement[0]);
                        }
                            viewmodel.allItem.Add(new Models.TodoItem(bitmapImage_1, (string)statement[1], (string)statement[2], (DateTimeOffset.Parse((string)statement[3]))));
                        }
                    }
                }
           // }
                return viewmodel;
        }

        private ObservableCollection<Models.TodoItem> allItem = new ObservableCollection<Models.TodoItem>();//ObservableCollection表示一个动态数据集合，在添加项、移除项或刷新整个列表时，此集合将提供通知。
        public ObservableCollection<Models.TodoItem> AllItem { get { return this.allItem; } }//得到所有的Item,保存Item的model的属性

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        
        //表示现在选中的哪一ITem
        public Models.TodoItem SelectedItem{get{return selectedItem;} set{this.selectedItem = value;} }
        
        //添加TodoItem
        public void AddTodoItem(string  imageuri, string title, string description, DateTimeOffset datetime)
        {   //添加
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageuri));
            this.allItem.Add(new Models.TodoItem(bitmapImage, title, description, datetime));
            var db = new SQLiteConnection("demo.db");
            string sql = @"INSERT INTO Todoitem(Image,Title,Description,Date) VALUES (?,?,?,?);";
            using (var todoitem = db.Prepare(sql)) {
                
                todoitem.Bind(1,imageuri);
                todoitem.Bind(2, title);
                todoitem.Bind(3, description);
                todoitem.Bind(4, datetime.ToString());
                todoitem.Step();
            }
           
          
        }

        //删除TodoItem
        public void RemoveTodoItem(string title)
        {   
            //移出，使用title来获取整个对象
            this.allItem.Remove(this.SelectedItem);
            this.selectedItem = null;
            var db = new SQLiteConnection("demo.db");
            string sql = @"DELETE FROM Todoitem WHERE Title = ? ";
            using (var todoitem = db.Prepare(sql)) {
                todoitem.Bind(1, title);
                todoitem.Step();
            }
        }

        //更新TodoItem
        public void UpdateTodoItem(string imageuri, string title, string description, DateTimeOffset datetime)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageuri));
         //   var index = this.allItem.IndexOf(this.selectedItem);
            this.selectedItem.image =bitmapImage;
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.datetime = datetime;
         //  this.allItem.Remove(this.SelectedItem);
         //   this.allItem.Insert(index, this.selectedItem);
            this.selectedItem = null;
            var db = new SQLiteConnection("demo.db");
            string sql = @"UPDATE Todoitem SET Image=?, Title = ?,Description= ?,Date = ? ";
            using (var todoitem = db.Prepare(sql)) {
                todoitem.Bind(1, imageuri);
                todoitem.Bind(2, title);
                todoitem.Bind(3, description);
                todoitem.Bind(4, datetime.ToString());
                todoitem.Step();
            }
        }
  
    }



}


        
