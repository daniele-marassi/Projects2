using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartTools
{
    internal class Program
    {
        static void Main(string[] args)
        {
			Process[] process = Process.GetProcessesByName("Tools");
			var currentProcess = Process.GetCurrentProcess();

			if (process.Length == 0)
			{
				Task.Run(() => StartTools());
			}

			RestartTheApplicationIfUsedMemoryIsHigh(currentProcess);
		}

		public static void RestartTheApplicationIfUsedMemoryIsHigh(Process currentProcess)
		{
			while (true)
			{
				System.Threading.Thread.Sleep(60);

                //currentProcess.Refresh();
                //var usedMemory = currentProcess.PrivateMemorySize;

                long usedMemory = 0; // memsize in KB
                PerformanceCounter PC = new PerformanceCounter();
                PC.CategoryName = "Process";
                PC.CounterName = "Working Set - Private";
                PC.InstanceName = "Tools";
                usedMemory = Convert.ToInt64(PC.NextValue());
                PC.Close();
                PC.Dispose();

                Console.WriteLine(usedMemory);

				if (usedMemory > 300000000) // > 300MB
				{
					//currentProcess.Kill();

					//Task.Run(() => StartTools());
				}
			}
		}

		public static async Task StartTools()
		{
			var applicationFullPath = "";

			applicationFullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

			applicationFullPath = applicationFullPath.Replace(@"\StartTools", "");

			applicationFullPath = Path.Combine(applicationFullPath, "Tools.exe");

			Process proc = new Process();
			proc.StartInfo.FileName = applicationFullPath;
			proc.StartInfo.UseShellExecute = true;
			proc.StartInfo.Verb = "runas";
			proc.Start();
		}
	}
}
