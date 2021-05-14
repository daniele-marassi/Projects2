
using MVVMDemo.Models;
using System.Collections.ObjectModel;
using MVVMDemo.Commands;
using MVVMDemo.Views;
using System.Windows.Controls;
using static MVVMDemo.ViewModels.ToolBarHandler;

namespace MVVMDemo.ViewModels
{

    public class StudentViewModel
    {
        public static StudentICommand DeleteCommand { get; set; }

        public StudentViewModel()
        {
            LoadStudents();
            DeleteCommand = new StudentICommand(OnDelete, CanDelete);

            ToolBarHandler deleteButton = new ToolBarHandler() { Image = ImageType.Delete, MyCommand = DeleteCommand , Type = TypeItem.Button};
            ToolBarHandler separator = new ToolBarHandler() { Type = TypeItem.Separator };
            ToolBarHandler helpButton = new ToolBarHandler() { Image = ImageType.Help, Text = "Help", Type = TypeItem.Button };

            MainViewModel.AddToolbarButton(deleteButton);
            MainViewModel.AddToolbarButton(separator);
            MainViewModel.AddToolbarButton(helpButton);

        }

        public static ObservableCollection<Student> Students
        {
            get;
            set;
        }

        public void LoadStudents()
        {
            ObservableCollection<Student> students = new ObservableCollection<Student>() { };

            students.Add(new Student { FirstName = "Mark", LastName = "Allain" });
            students.Add(new Student { FirstName = "Allen", LastName = "Brown" });
            students.Add(new Student { FirstName = "Linda", LastName = "Hamerski" });

            Students = students;
        }

        private static Student _selectedStudent;

        public static Student SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }

            set
            {
                _selectedStudent = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }

        }

        public void OnDelete()
        {
            Students.Remove(SelectedStudent);
        }

        public bool CanDelete()
        {
            return SelectedStudent != null;
        }

        public int GetStudentCount()
        {
            return Students.Count;
        }
    }
}