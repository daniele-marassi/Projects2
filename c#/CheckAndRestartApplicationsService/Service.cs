using CheckAndRestartApplicationsService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CheckAndRestartApplicationsService
{
    public partial class Service : ServiceBase
    {
		private static bool active = false;
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
			active = true;
			var sleepOfTheCheckAndRestartApplicationsServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheCheckAndRestartApplicationsServiceInMilliseconds"]);
			var sleepOfTheCheckAndRestartApplicationsServiceFirstStepInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheCheckAndRestartApplicationsServiceFirstStepInMilliseconds"]);

			var applicationList = JsonConvert.DeserializeObject<List<Application>>(ConfigurationManager.AppSettings["applicationListInJson"]);

			RestartTheApplicationIfUsedMemoryIsHighOrIsClosed(sleepOfTheCheckAndRestartApplicationsServiceInMilliseconds, sleepOfTheCheckAndRestartApplicationsServiceFirstStepInMilliseconds, applicationList);
		}

        protected override void OnStop()
        {
			active = false;
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

					Process[] processArray = Process.GetProcessesByName(application.Name);
					long usedMemoryInByte = 0;

					if (processArray.Count() > 1)
					{
						var _processArray = processArray.OrderBy(_ => _.Id).ToArray();

						for (int i = 0; i < _processArray.Count() - 1; i++)
						{
							try
							{
								_processArray[i].Kill();
							}
							catch (Exception)
							{
							}
						}
					}

					if (processArray.Count() > 0)
					{
						try
						{
							var pc = new PerformanceCounter();
							pc.CategoryName = "Process";
							pc.CounterName = "Working Set - Private";
							pc.InstanceName = application.Name;
							usedMemoryInByte = Convert.ToInt64(pc.NextValue());
							pc.Close();
							pc.Dispose();
						}
						catch (Exception)
						{
						}

						//Console.WriteLine(usedMemoryInByte);
					}

					if (usedMemoryInByte > application.MaxMemoryInByte || processArray.Count() == 0)
					{
						if (usedMemoryInByte > application.MaxMemoryInByte)
						{
							try
							{
								processArray[processArray.Count() - 1].Kill();
							}
							catch (Exception)
							{
							}
						}

						try
						{
							var proc = new Process();
							proc.StartInfo.FileName = application.ApplicationFullPath;
							proc.StartInfo.UseShellExecute = true;
							proc.StartInfo.Verb = "runas";
							proc.Start();
						}
						catch (Exception)
						{
						}
					}
				}
			}
		}
	}
}
