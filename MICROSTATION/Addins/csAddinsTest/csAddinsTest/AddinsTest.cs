using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bentley.MicroStation.InteropServices;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace csAddinsTest
{

    [Bentley.MicroStation.AddInAttribute
                 (KeyinTree = "csAddinsTest.commands.xml", MdlTaskID = "CSADDINSTEST")]
    public sealed class AddinsTest : Bentley.MicroStation.AddIn
    {
        public static AddinsTest s_addin = null;

        public static Bentley.Interop.MicroStationDGN.Application app = null;
        public AddinsTest(System.IntPtr mdlDesc) : base(mdlDesc)
        {
            s_addin = this;
            app = Utilities.ComApp;
        }

        protected override int Run(string[] commandLine)
        {
            var exePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(Path.Combine(exePath, "log.txt"), true))
            {
                var i = 0;
                foreach (var comand in commandLine)
                {
                    i++;
                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "       " + i.ToString() + " - " + comand);
                }

            }

            Console.WriteLine("MDL csAddinsTest inserted!");

            if(app.Visible == true) app.CadInputQueue.SendKeyin("csAddinsTest LoadForms Toolbar");
            Console.WriteLine("Open Toolbar csAddinsTest");
            return 0;
        }
    }
}
