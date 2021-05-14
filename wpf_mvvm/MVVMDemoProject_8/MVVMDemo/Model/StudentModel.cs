using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using MVVMDemo.Notification;

namespace MVVMDemo.Model
{

    public class StudentModel { }

    public class Student : PropertyChangedNotification
    {
        private string firstName;
        private string lastName;

        [Required(ErrorMessage = "You must enter FirstName.")]
        public string FirstName
        {
            get { return firstName; }

            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    RaisePropertyChanged("FirstName");
                    RaisePropertyChanged("FullName");
                    ValidationContext validationContext = new ValidationContext(this, null, null);
                    validationContext.MemberName = "FirstName";
                    Validator.ValidateProperty(value, validationContext);
                    //ValidateName(value);


                }
            }
        }

        private void ValidateName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            Regex regex = new Regex(@"\d+");
            if (regex.Match(value) != Match.Empty)
                throw new ArgumentException("Invalid value");
        }

        public string LastName
        {
            get { return lastName; }

            set
            {
                if (lastName != value)
                {
                    ValidationContext validationContext = new ValidationContext(this, null, null);
                    validationContext.MemberName = "FirstName";
                    Validator.ValidateProperty(value, validationContext);

                    //ValidateName(value);
                    lastName = value;
                    RaisePropertyChanged("LastName");
                    RaisePropertyChanged("FullName");
                }
            }
        }

        public string FullName
        {
            get
            {
                return firstName + " " + lastName;
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
