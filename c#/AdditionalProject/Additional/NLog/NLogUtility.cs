using Microsoft.Win32.SafeHandles;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Additional.NLog
{
    public class NLogUtility
    {
        private Utility utility;

        public NLogUtility()
        {
            utility = new Utility();
        }

        public MethodBase GetMethodToNLog(MethodBase method)
        {
            MethodBase _method = null;
            try
            {
                var memberType = method.DeclaringType.MemberType;
                var methodName = String.Empty;
                var fullClassName = String.Empty;
                var start = 0;
                var end = 0;

                if (memberType == MemberTypes.NestedType)
                {
                    var fullName = method.DeclaringType.FullName;

                    start = 0;
                    end = fullName.IndexOf("+");
                    fullClassName = fullName.Substring(start, end - start);

                    start = fullName.IndexOf("<") + 1;
                    end = fullName.IndexOf(">");
                    methodName = fullName.Substring(start, end - start);

                    _method = new StackTrace()
                            .GetFrames()
                            .Select(frame => frame.GetMethod())
                            .Where(_ => _.Name == methodName && _.DeclaringType.FullName == fullClassName)
                            .FirstOrDefault();
                }

                if (memberType != MemberTypes.NestedType || _method == null)
                {
                    _method = method;
                }
            }
            catch (Exception)
            {
                _method = method;
                //throw;
            }

            return _method;
        }

        /// <summary>
        /// Clear NLog log file
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="maxSizeInMB"></param>
        public void ClearNLogFile(string targetName, int maxSizeInMB)
        {
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName(targetName);
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            string filePath = fileTarget.FileName.Render(logEventInfo).Replace(@"\\", @"\");
            if (File.Exists(filePath))
            {
                long length = new System.IO.FileInfo(filePath).Length;

                if (utility.ConvertBytesToMegabytes(length) > maxSizeInMB)
                {
                    int counter = 0;
                    List<string> lines = new List<string>() { };
                    List<string> _lines = new List<string>() { };
                    string line = null;

                    System.IO.StreamReader file = new System.IO.StreamReader(filePath);

                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    file.Close();

                    counter = lines.Count;

                    for (int i = (int)(counter / 2); i < counter; i++)
                    {
                        _lines.Add(lines.ToArray()[i]);
                    }

                    File.WriteAllLines(filePath, _lines.ToArray());
                }
            }
        }
    }

    public class NLogScope : IDisposable
    {

        private Utility utility;

        public NLogScope()
        {
            utility = new Utility();
        }

        Logger _logger = null;
        string currentMethodName = null;
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        DateTime start = DateTime.MinValue;
        DateTime end = DateTime.MinValue;

        /// <summary>
        /// NLogScope
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="currentMethod"></param>
        public NLogScope(Logger logger, MethodBase currentMethod)
        {
            var archivingLogEnabled = false;
            var archivingLogMegabyteLimit = 0;
            var archivingLogSelfCleaningEnabled = false;
            var archivingLogSelfCleaningAfterDays = 0;
            var mainDir = String.Empty;
            var mainLogFileName = String.Empty;
            var mainLogFullPath = String.Empty;
            try
            {
                archivingLogEnabled = bool.Parse(logger.Factory.Configuration.Variables["ArchivingLog.Enabled"].OriginalText.ToString());
                archivingLogMegabyteLimit = int.Parse(logger.Factory.Configuration.Variables["ArchivingLog.MegabyteLimit"].OriginalText.ToString());
                archivingLogSelfCleaningEnabled = bool.Parse(logger.Factory.Configuration.Variables["ArchivingLog.SelfCleaning.Enabled"].OriginalText.ToString());
                archivingLogSelfCleaningAfterDays = int.Parse(logger.Factory.Configuration.Variables["ArchivingLog.SelfCleaning.AfterDays"].OriginalText.ToString());
                mainDir = logger.Factory.Configuration.Variables["mainDir"].OriginalText.ToString();
                mainLogFileName = logger.Factory.Configuration.Variables["mainLogFileName"].OriginalText.ToString();
                mainDir = mainDir.Replace(@"\\", @"\");
                mainLogFileName = mainLogFileName.Replace(@"\\", @"\");
                mainLogFullPath = Path.Combine(mainDir, mainLogFileName);

            }
            catch (Exception ex)
            {
                logger.Error("NLogScope: " + ex.Message);
            }

            if (archivingLogEnabled)
            {
                if (File.Exists(mainLogFullPath))
                {
                    FileInfo info = new FileInfo(mainLogFullPath);
                    double size = info.Length / 1000000d;

                    if (size > archivingLogMegabyteLimit)
                    {
                        var zipName = "backup_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                        try
                        {
                            using (ZipArchive zip = ZipFile.Open(Path.Combine(mainDir, zipName + ".zip"), ZipArchiveMode.Create))
                            {
                                zip.CreateEntryFromFile(mainLogFullPath, Path.Combine(zipName, mainLogFileName));
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                        }

                        try
                        {
                            File.WriteAllText(mainLogFullPath, String.Empty);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                        }
                    }
                }
            }

            if (archivingLogSelfCleaningEnabled)
            {
                if (Directory.Exists(mainDir))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(mainDir);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        var date1 = file.CreationTime;
                        var date2 = DateTime.Now;
                        var daysDiff = ((TimeSpan)(date2 - date1)).Days;

                        if (file.Extension == ".zip" && daysDiff >= archivingLogSelfCleaningAfterDays)
                        {
                            try
                            {
                                file.Delete();
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.Message);
                            }
                        }
                    }
                }
            }

            _logger = logger;
            currentMethodName = currentMethod.Name;
            start = DateTime.Now;
            _logger.Info($"{currentMethodName} -> STARTED");
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            end = DateTime.Now;
            TimeSpan span = (end - start);

            var spanString = String.Format("{0} days and {1}:{2}:{3}.{4}",
                span.Days, span.Hours.ToString("00"), span.Minutes.ToString("00"), span.Seconds.ToString("00"), span.Milliseconds.ToString("0000"));

            _logger.Info($"{currentMethodName} -> ENDED after {spanString}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }

        /// <summary>
        /// Write Log how Error
        /// </summary>
        /// <param name="value"></param>
        public void Error(string value)
        {
            _logger.Error(value);
        }

        /// <summary>
        /// Write Log how Warn
        /// </summary>
        /// <param name="value"></param>
        public void Warn(string value)
        {
            _logger.Warn(value);
        }

        /// <summary>
        /// Write Log how Debug
        /// </summary>
        /// <param name="value"></param>
        public void Debug(string value)
        {
            _logger.Debug(value);
        }

        /// <summary>
        /// Write Log how Info
        /// </summary>
        /// <param name="value"></param>
        public void Info(string value)
        {
            _logger.Info(value);
        }

        /// <summary>
        /// Write Log how Trace
        /// </summary>
        /// <param name="value"></param>
        public void Trace(string value)
        {
            _logger.Trace(value);
        }

        /// <summary>
        /// Write Log how Fatal
        /// </summary>
        /// <param name="value"></param>
        public void Fatal(string value)
        {
            _logger.Fatal(value);
        }
    }
}
