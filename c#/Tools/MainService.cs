using Additional;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tools
{
    public class MainService
    {
		/// <summary>
		/// Start MainService
		/// </summary>
		/// <returns></returns>
		public static async Task Start()
		{
			var limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

			var logsDirectory = ConfigurationManager.AppSettings["LogsDirectory"];

			var sleepOfTheMainServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheMainServiceInMilliseconds"]);

			while (true)
			{
				System.Threading.Thread.Sleep(sleepOfTheMainServiceInMilliseconds);

				await ClearNLogFiles(limitLogFileInMB, logsDirectory, sleepOfTheMainServiceInMilliseconds);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
		}

		/// <summary>
		/// ClearNLogFiles
		/// </summary>
		/// <param name="limitLogFileInMB"></param>
		/// <param name="logsDirectory"></param>
		/// <param name="sleepOfTheMainServiceInMilliseconds"></param>
		/// <returns></returns>
		public static async Task ClearNLogFiles(int limitLogFileInMB, string logsDirectory, int sleepOfTheMainServiceInMilliseconds)
		{
			var files = Directory.GetFiles(logsDirectory, "*.log", SearchOption.AllDirectories);

			foreach (var file in files)
			{
				ClearNLogFile(file, limitLogFileInMB);
				System.Threading.Thread.Sleep(1000);
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
