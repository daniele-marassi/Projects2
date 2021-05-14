
using MVVMDemo.Models;
using System.Collections.ObjectModel;
using MVVMDemo.Commands;
using MVVMDemo.Views;
using System.Windows.Controls;
using static MVVMDemo.ViewModels.ToolBarHandler;

namespace MVVMDemo.ViewModels
{

    public class SchoolClassViewModel
    {
        public static MyICommand DeleteCommand { get; set; }

        public SchoolClassViewModel()
        {
            LoadStudents();
            DeleteCommand = new MyICommand(OnDelete, CanDelete);

            ToolBarHandler deleteButton = new ToolBarHandler() { Image = ImageType.Delete, MyCommand = DeleteCommand , Type = TypeItem.Action};
            ToolBarHandler separator = new ToolBarHandler() { Type = TypeItem.Separator };
            ToolBarHandler helpButton = new ToolBarHandler() { Image = ImageType.Help, Text = "Help", Type = TypeItem.Action };

            MainViewModel.AddToolbarButton(deleteButton);
            MainViewModel.AddToolbarButton(separator);

        }

        public static ObservableCollection<SchoolClass> SchoolClasses
        {
            get;
            set;
        }

        public void LoadStudents()
        {
            ObservableCollection<SchoolClass> schoolClasses = new ObservableCollection<SchoolClass>() { };

            schoolClasses.Add(new SchoolClass { Address = "Liceo classico",  Grade = "1C" });
            schoolClasses.Add(new SchoolClass { Address = "Liceo classico", Grade = "2A" });
            schoolClasses.Add(new SchoolClass { Address = "Liceo Scientifico", Grade = "1A" });
            schoolClasses.Add(new SchoolClass { Address = "Liceo Scientifico", Grade = "2B" });

            SchoolClasses = schoolClasses;
        }

        private static SchoolClass _selectedSchoolClass;

        public static SchoolClass SelectedSchoolClass
        {
            get
            {
                return _selectedSchoolClass;
            }

            set
            {
                _selectedSchoolClass = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public void OnDelete()
        {
            SchoolClasses.Remove(SelectedSchoolClass);
        }

        public bool CanDelete()
        {
            return SelectedSchoolClass != null;
        }

        public int GetSchoolClassCount()
        {
            return SchoolClasses.Count;
        }
    }
}