using Additional;
using Tools.ExecutionQueue;
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tools
{
	public class Program
	{
        [STAThread]
		static void Main()
		{
			Process[] process = Process.GetProcessesByName("Tools");

			if (process.Length == 1)
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
}