using Microsoft.Win32.SafeHandles;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SearchAttestati
{
    public class NLogScope : IDisposable
    {
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
