using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePosition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText(Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "MousePosition.txt"), "");
            while (true) 
            {
                System.Threading.Thread.Sleep(2000);
                var position = Cursor.Position;

                var positionText = "X:" + position.X.ToString() + " Y:" + position.Y.ToString();
                Console.WriteLine(positionText);

                using (StreamWriter w = File.AppendText(Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "MousePosition.txt")))
                {
                    w.WriteLine(positionText);
                }
            }
        }
    }
}
