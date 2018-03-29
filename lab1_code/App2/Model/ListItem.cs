using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
namespace Todos.Models
{
    public class TodoItem {

        private string id;
        public string title { get; set; }
        public string description { get; set; }
        public bool completed { get; set; }
        public System.DateTimeOffset datetime;




        public TodoItem(string title ,string description,DateTimeOffset datetime)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.completed = false;
            this.datetime = datetime;
         }
    }
    class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool myValue = (bool)value;
            if (myValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
