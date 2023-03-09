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
			Task.Run(() => MainService());

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Show the system tray icon.					
			using (ProcessIcon pi = new ProcessIcon())
			{
				pi.Display();
				Application.Run();
			}
		}

		private static async Task MainService()
		{
			var limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

			var logsDirectory = ConfigurationManager.AppSettings["LogsDirectory"];

			var sleepOfTheMainServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheMainServiceInMilliseconds"]);

			while (true)
			{
				System.Threading.Thread.Sleep(sleepOfTheMainServiceInMilliseconds);

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
	}
}