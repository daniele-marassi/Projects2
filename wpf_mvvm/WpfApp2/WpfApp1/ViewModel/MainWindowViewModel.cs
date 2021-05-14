using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand TestCommand;

        public MainWindowViewModel()
        {
            TestCommand = new RelayCommand(Test);
        }

        public void Test()
        {

        }
    }
}
