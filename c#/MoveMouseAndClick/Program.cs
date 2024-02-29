using Additional;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoveMouseAndClick
{
    internal class Program
    {
        private class Parameters
        { 
            public int X { get; set; }
            public int Y { get; set; }
            public int Sleep { get; set; }
        }

        static void Main(string[] args)
        {
            var _params = new List<Parameters>() { };

            try
            {
                var _arg = "";

                for (int i = 0; i < args.Length; i++)
                {
                    if (_arg != "") _arg += " ";
                    _arg += args[i];
                }

                //_arg = "[{\"X\":101, \"Y\":102, \"Sleep\":103},{\"X\":201, \"Y\":202, \"Sleep\":203}]"; //for test
                _params = JsonConvert.DeserializeObject<List<Parameters>>(_arg);

                if (_params == null) throw new Exception();

                foreach (var param in _params)
                {
                    MoveAndClick(param);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Pass this JSON structure in the parameters: " + "[{\"X\":101, \"Y\":102, \"Sleep\":103},{\"X\":201, \"Y\":202, \"Sleep\":203}]", "MoveMouseAndClick");
            }
        }

        private static void MoveAndClick(Parameters param)
        {
            System.Threading.Thread.Sleep(param.Sleep);

            Cursor.Position = new Point(param.X, param.Y);

            System.Threading.Thread.Sleep(100);

            VirtualMouse.LeftClick();
        }
    }
}
