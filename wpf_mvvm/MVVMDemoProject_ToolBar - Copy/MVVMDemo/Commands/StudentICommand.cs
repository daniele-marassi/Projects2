
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System; 
using System.Windows.Input;
using System.Windows;
using Telerik.Windows.Controls;

namespace MVVMDemo.Commands
{

    public class StudentICommand : ICommand
    {
        Action _TargetExecuteMethod;
        Func<bool> _TargetCanExecuteMethod;
        public ICommand CustomCommand { get; set; }

        public StudentICommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
            this.CustomCommand = new DelegateCommand(OnCustomCommandExecuted);
        }

        private void OnCustomCommandExecuted(object obj)
        {
            MessageBox.Show(obj.ToString());
        }

        public StudentICommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {

            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }

            if (_TargetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }
		
      // Beware - should use weak references if command instance lifetime  is longer than lifetime of UI objects that get hooked up to command
 
      // Prism commands solve this in their implementation 
        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod();
            }
        }
    }
}