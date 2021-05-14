using Additional;
using Tools.ExecutionQueue;
using System;
using System.Windows.Forms;

namespace Tools
{
	public class Program
	{
        [STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
			{
				pi.Display();
                Application.Run(); 
            }
        }
    }
}