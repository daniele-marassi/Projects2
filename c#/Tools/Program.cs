using Additional;
using Tools.ExecutionQueue;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tools
{
	public class Program
	{
		[STAThread]
		static void Main()
		{
			Process[] process = Process.GetProcessesByName("Tools");
			var currentProcess = Process.GetCurrentProcess();
			
			Task.Run(() => RestartTheApplicationIfUsedMemoryIsHigh(currentProcess));

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

		public static async Task RestartTheApplicationIfUsedMemoryIsHigh(Process currentProcess) 
		{
			while(true)
			{
				System.Threading.Thread.Sleep(60000);

				var usedMemory = currentProcess.PrivateMemorySize64;
				                 
				if (usedMemory > 300000000) // > 300MB
				{
					string applicationFullPath = System.Reflection.Assembly.GetEntryAssembly().Location;

                    Process proc = new Process();
                    proc.StartInfo.FileName = applicationFullPath;
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.Verb = "runas";
                    proc.Start();

					currentProcess.Kill();
				}
			}
		}
	}
}