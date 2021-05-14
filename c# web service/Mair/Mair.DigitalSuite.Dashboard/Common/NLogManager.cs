using Microsoft.Win32.SafeHandles;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mair.DigitalSuite.Dashboard.Common
{
    public class NLogUtility
    {
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
    }

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
