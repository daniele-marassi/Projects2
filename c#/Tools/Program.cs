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
				System.Threading.Thread.Sleep(10);

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