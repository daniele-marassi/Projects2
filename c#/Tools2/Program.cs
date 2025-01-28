using Additional;
using Tools.ExecutionQueue;
using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using Supp.Models;

namespace Tools
{
	public class Program
	{
        [STAThread]
		static void Main()
		{
			Task.Run(() => MainService.Start());

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