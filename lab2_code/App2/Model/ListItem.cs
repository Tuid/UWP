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
using System.ComponentModel;
namespace Todos.Models
{
    public class TodoItem : INotifyPropertyChanged{

        public event PropertyChangedEventHandler PropertyChanged;
        private string id;

        private string _title;
        public string title {
            get {
                return _title;
            }
            set {
                _title = value;
                NotifyPropertyChanged("title");
            }
        }
        private string _description;
        public string description {
            get {
                return _description;
            }
            set {
                _description = value;
                NotifyPropertyChanged("description");
            }
                }
        private bool _complete;
        public bool completed { get {
                return _complete;
            } set {
                _complete = value;
                NotifyPropertyChanged("complete");
            } }

        private DateTimeOffset _datetime;
        public System.DateTimeOffset datetime { get {
                return _datetime;
            }
            set {
                _datetime = value;
                NotifyPropertyChanged("datetime");
            } }
        private BitmapImage _image;
        public BitmapImage image { get {
                return _image;
            } set {
                _image = value;
                NotifyPropertyChanged("image");
            } }


        //构造函数
        public TodoItem(BitmapImage image,string title ,string description,DateTimeOffset datetime)
        {
            this.id = Guid.NewGuid().ToString();
            this.image = image;
            this.title = title;
            this.description = description;
            this.completed = false;
            this.datetime = datetime;
         }
        private void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    //转化，将complate(bool)型，转化为visibility的属性值
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
