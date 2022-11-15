using Additional;
using Tools.ExecutionQueue;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Additional.NLog;
using System.Configuration;

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

			Task.Run(() => MainService());

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

		private static async Task MainService()
		{
			var nLogUtility = new NLogUtility();
			var limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

			while (true)
			{
				System.Threading.Thread.Sleep(3600000);

				nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
			}
		}

		private static async Task RestartTheApplicationIfUsedMemoryIsHigh(Process currentProcess) 
		{
			while (true)
			{
				System.Threading.Thread.Sleep(30000);

				long usedMemory = 0; // memsize in Byte
				PerformanceCounter PC = new PerformanceCounter();
				PC.CategoryName = "Process";
				PC.CounterName = "Working Set - Private";
				PC.InstanceName = "Tools";
				usedMemory = Convert.ToInt64(PC.NextValue());
				PC.Close();
				PC.Dispose();

				//Console.WriteLine(usedMemory);
								 
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