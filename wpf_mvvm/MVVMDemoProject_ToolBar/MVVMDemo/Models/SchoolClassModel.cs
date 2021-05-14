using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace MVVMDemo.Models
{

    public class SchoolClassModel { }

    public class SchoolClass : INotifyPropertyChanged
    {
        private string grade;
        private string address;

        public string Grade
        {
            get { return grade; }

            set
            {
                if (grade != value)
                {
                    grade = value;
                    RaisePropertyChanged("Grade");
                }
            }
        }

        public string Address
        {
            get { return address; }

            set
            {
                if (address != value)
                {
                    address = value;
                    RaisePropertyChanged("Address");
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
             if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
