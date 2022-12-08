using Additional;
using Tools.ExecutionQueue;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Additional.NLog;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
			var limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

			var logsDirectory = ConfigurationManager.AppSettings["LogsDirectory"];

			while (true)
			{
				System.Threading.Thread.Sleep(3600000); //3600000

				var files = Directory.GetFiles(logsDirectory, "*.log", SearchOption.AllDirectories);

                foreach (var file in files)
                {
					ClearNLogFile(file, limitLogFileInMB);
					System.Threading.Thread.Sleep(1000);
				}
			}
		}
		private static void ClearNLogFile(string filePath, int maxSizeInMB)
		{
			if (File.Exists(filePath))
			{
				long length = new System.IO.FileInfo(filePath).Length;

				var utility = new Utility();

				if (utility.ConvertBytesToMegabytes(length) > maxSizeInMB)
				{
					List<string> lines = new List<string>() { };

					var countLine = File.ReadLines(filePath).LongCount();

					lines.AddRange(File.ReadLines(filePath).Skip((int)(countLine / 2)).ToList());

					File.WriteAllLines(filePath, lines.ToArray());
				}
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