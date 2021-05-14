
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace GetAttestati
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkFlow workFlow = new WorkFlow();
            workFlow.Execute();
        }
    }
}
