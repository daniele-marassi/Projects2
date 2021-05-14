
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MVVMDemo.Views;
using System;
using MVVMDemo.Commands;
using System.Windows;

namespace MVVMDemo.ViewModels
{

    public class MainViewModel : BaseViewModel
    {

        public static MyICommand ChangeInStudentViewCommand { get; set; }
        public static MyICommand ChangeInSchoolClassViewCommand { get; set; }

        private object selectedView;

        public object SelectedView

        {
            get { return selectedView; }
            set
            {
                selectedView = value;
                OnPropertyChanged("SelectedView");
            }
        }

        private double heightMainVierw = 0;
        public double HeightMainView
        {
            get { return heightMainVierw; }
            set
            {
                heightMainVierw = value;
                OnPropertyChanged("HeightMainView");
            }
        }

        public static ObservableCollection<ToolBarHandler> items = new ObservableCollection<ToolBarHandler>() { };

        public ObservableCollection<ToolBarHandler> Items
        {
            get { return items; }
            private set { items = value; }
        }

        public static void AddToolbarButton(ToolBarHandler button)
        {
            items.Add(button);
        }

        public MainViewModel()
        {
            HeightMainView = 150;
            ChangeInStudentViewCommand = new MyICommand(OnChangeInStudentView);
            ChangeInSchoolClassViewCommand = new MyICommand(OnChangeInSchoolClassView);
            ChangeInStudentViewCommand.RaiseCanExecuteChanged();
            ChangeInSchoolClassViewCommand.RaiseCanExecuteChanged();
            OnChangeInStudentView();
        }

        public void OnChangeInStudentView()
        {
            //Application.Current.MainWindow.Height = 300;
            HeightMainView = 300;
            Items.Clear();
            SelectedView = new StudentView();
        }

        public void OnChangeInSchoolClassView()
        {

            //Application.Current.MainWindow.Height = 500;
            HeightMainView = 500;
            Items.Clear();
            SelectedView = new SchoolClassView();
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
            Action,
            Input,
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
