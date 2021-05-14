using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MVVMDemo.ViewModels;
using MVVMDemo.Commands;
using MVVMDemo.Models;

namespace MVVMDemo.ViewModels
{
    public class ButtonViewModel : BaseViewModel
    {
        private string text;
        private string imagePath;

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

        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

    }
}
