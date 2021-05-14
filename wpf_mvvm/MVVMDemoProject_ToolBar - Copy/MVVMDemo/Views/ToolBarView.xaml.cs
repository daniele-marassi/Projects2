using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using MVVMDemo.ViewModels;
using System.Collections;
using MVVMDemo.Commands;
using System.Globalization;

namespace MVVMDemo.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ToolBarView : UserControl
    {
        public ToolBarView()
        {
            InitializeComponent();
        }   
    }

    public enum ImageType
    {
        Delete,
        Help
    }

    public class ImageTypeToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ImageType.Delete:
                    return "../Images/delete.png";
                case ImageType.Help:
                    return "../Images/help.png";
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "../Images/delete.png":
                    return ImageType.Delete;
                case "../Images/help.png":
                    return ImageType.Help;
            }
            return null;
        }
    }
}
