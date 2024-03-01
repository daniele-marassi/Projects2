using Additional;
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
                //args = new string[6];//for test
                //
                //args[0] = "101";//for test
                //args[1] = "102";//for test
                //args[2] = "103";//for test
                //
                //args[3] = "201";//for test
                //args[4] = "202";//for test
                //args[5] = "203";//for test

                for (int i = 0; i < args.Length; i++)
                {
                    _params.Add(new Parameters() { X = int.Parse(args[i]), Y = int.Parse(args[i + 1]), Sleep = int.Parse(args[i + 2]) });
                    i = i + 2;
                }

                if (_params == null) throw new Exception();

                foreach (var param in _params)
                {
                    MoveAndClick(param);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Pass this structure in the parameters: " + "X Y SLEEP X Y SLEEP es. 101 102 103 201 202 203", "MoveMouseAndClick");
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
