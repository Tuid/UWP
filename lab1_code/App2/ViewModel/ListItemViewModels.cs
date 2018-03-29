using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Models;
namespace Todos.ViewModels {

    public class TodoItemViewModel {
        private ObservableCollection<Models.TodoItem> allItem = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItem { get { return this.allItem; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem{get{return selectedItem;} set{this.selectedItem = value;} }
        
       
        public TodoItemViewModel()//例子
        {
            this.allItem.Add(new Models.TodoItem("asfatitle", "description", DateTime.Today));
            this.allItem.Add(new Models.TodoItem("afaTitle", "Description", DateTime.Now.Date));
        }


        public void AddTodoItem(string title, string description,DateTimeOffset datetime) {//添加
            this.allItem.Add(new Models.TodoItem(title,description,datetime));
        }

        public void RemoveTodoItem(string title) {//移出，使用title来获取整个对象
            this.allItem.Remove(this.SelectedItem);

            this.selectedItem = null;
        }

        public void updateTodoItem( string title, string description,DateTimeOffset datetime) {//更新

            var index = this.allItem.IndexOf(this.selectedItem);
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.datetime = datetime;
            this.allItem.Remove(this.SelectedItem);
            this.allItem.Insert(index, this.selectedItem);
            this.selectedItem = null;
        }
        }
}