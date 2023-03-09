using CheckAndRestartApplications.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace CheckAndRestartApplications
{
    internal class Service
    {
        static void Main(string[] args)
        {
            var sleepOfTheCheckAndRestartApplicationsServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheCheckAndRestartApplicationsServiceInMilliseconds"]);
			var sleepOfTheCheckAndRestartApplicationsServiceFirstStepInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheCheckAndRestartApplicationsServiceFirstStepInMilliseconds"]);

			var applicationList = JsonConvert.DeserializeObject<List<Application>>(ConfigurationManager.AppSettings["applicationListInJson"]);

			RestartTheApplicationIfUsedMemoryIsHighOrIsClosed(sleepOfTheCheckAndRestartApplicationsServiceInMilliseconds, sleepOfTheCheckAndRestartApplicationsServiceFirstStepInMilliseconds, applicationList);
		}

		private static void RestartTheApplicationIfUsedMemoryIsHighOrIsClosed(int sleep, int sleepFirstStep, List<Application> applicationList)
		{
			var firstStep = true;

			while (true)
			{
                foreach (var application in applicationList)
                {
					if (!firstStep) System.Threading.Thread.Sleep(sleep);
					else 
					{
						System.Threading.Thread.Sleep(sleepFirstStep);
						firstStep = false;
					}

					Process[] process = Process.GetProcessesByName(application.Name);
					var currentProcess = Process.GetCurrentProcess();
					long usedMemoryInByte = 0;

					if (process.Count() > 0)
					{
						var pc = new PerformanceCounter();
						pc.CategoryName = "Process";
						pc.CounterName = "Working Set - Private";
						pc.InstanceName = application.Name;
						usedMemoryInByte = Convert.ToInt64(pc.NextValue());
						pc.Close();
						pc.Dispose();

						//Console.WriteLine(usedMemoryInByte);
					}

					if (usedMemoryInByte > application.MaxMemoryInByte || process.Count() == 0)
					{
						if (usedMemoryInByte > application.MaxMemoryInByte)
							currentProcess.Kill();

						var proc = new Process();
						proc.StartInfo.FileName = application.ApplicationFullPath;
						proc.StartInfo.UseShellExecute = true;
						proc.StartInfo.Verb = "runas";
						proc.Start();
					}
				}
			}
		}
	}
}