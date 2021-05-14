using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ApplicationStart
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[]{ @"ccccC:\Program Files (x86)\Shut\Shut.exe"};
            foreach (string arg in args)
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(arg);
                    p.StartInfo.FileName = Path.GetFileName(arg);
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("***START: [" + arg + "]***" + " " + ex.ToString());
                }
            }
        }
    }
}
