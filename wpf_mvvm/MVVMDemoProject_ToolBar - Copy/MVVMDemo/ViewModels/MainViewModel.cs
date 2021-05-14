
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MVVMDemo.Views;
using System;

namespace MVVMDemo.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        private static ObservableCollection<ToolBarHandler> items = new ObservableCollection<ToolBarHandler>() { };

        public ObservableCollection<ToolBarHandler> Items
        {
            get { return items; }
            private set { items = value; }
        }

        public static void AddToolbarButton(ToolBarHandler button)
        {
            items.Add(button);
        }
    }

    public class ToolBarHandler : INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand myCommand;

        public ICommand MyCommand
        {
            get { return myCommand; }
            set
            {
                myCommand = value;
                OnPropertyChanged(nameof(MyCommand));
            }
        }

        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public enum TypeItem
        {
            Button,
            Textbox,
            Separator
        }

        private TypeItem type;
        public TypeItem Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        private ImageType image;
        public ImageType Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }



    }
}
