using Additional;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Properties;

namespace Tools
{
    public class MainService
    {
		// Define the FindWindow API function.
		[DllImport("user32.dll", EntryPoint = "FindWindow",
			SetLastError = true)]
		static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly,
			string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// Start MainService
        /// </summary>
        /// <returns></returns>
        public static async Task Start()
		{
            var utilty = new Utility();
            var appSettings = ConfigurationManager.AppSettings;

            var limitLogFileInMB = int.Parse(appSettings["LimitLogFileInMB"]);
			var logsDirectory = appSettings["LogsDirectory"];
			var sleepOfTheMainServiceInMilliseconds = int.Parse(appSettings["SleepOfTheMainServiceInMilliseconds"]);
            var windowCaption = appSettings["WindowCaption"];
            var exceptions = appSettings["BoxRepositioningExceptions"];
			string[] exceptionList = null;

            while (true)
			{
				System.Threading.Thread.Sleep(sleepOfTheMainServiceInMilliseconds);

				//await ClearNLogFiles(limitLogFileInMB, logsDirectory, sleepOfTheMainServiceInMilliseconds);

				if (utilty.ProcessIsActiveByWindowCaption(windowCaption))
				{
					var skip = false;				
					var hWnd = FindWindowByCaption(IntPtr.Zero, windowCaption);
					var rct = new RECT();
					GetWindowRect(hWnd, ref rct);

					if (rct.Top > -10)
					{
                        ConfigurationManager.RefreshSection("appSettings");
                        appSettings = ConfigurationManager.AppSettings;
                        exceptions = appSettings["BoxRepositioningExceptions"];

                        if (exceptions != null)
                            exceptionList = exceptions.Split(',');

                        if (exceptionList != null && exceptionList.Length > 0)
                        {
                            foreach (var exception in exceptionList)
                            {
                                var result = Process.GetProcessesByName(exception).FirstOrDefault();

                                if (result != null) { skip = true; break; }
                            }
                        }

                        if (!skip)
						{
							try
							{
								if (ProcessIcon.SpeechShowHideActive == 1)
								{
									ProcessIcon._Speech = new Speech();

									Task.Run(() => ProcessIcon._Speech.Restart());
								}
							}
							catch (Exception)
							{
							}
						}
					}
				}

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
