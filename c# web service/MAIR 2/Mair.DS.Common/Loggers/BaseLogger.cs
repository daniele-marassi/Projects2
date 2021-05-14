using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Mair.DS.Common.Loggers
{
    public abstract class BaseLogger : ILogger
    {
        public string Name { get; set; }
        public LoggerType Type { get; set; }
        public ILogger Logger { get; set; }
        public virtual ILogger Init()
        {
            Logger = Instances.GetInstance<ILogger>();
            return this;
        }

        public virtual string LogErr(object body)
        {
            return string.Concat(Header(LogHeaderType.Error), Body(body));
        }

        public virtual string LogWarn(object body)
        {
            return string.Concat(Header(LogHeaderType.Warning), Body(body));
        }

        public virtual string LogInfo(object body)
        {
            return string.Concat(Header(LogHeaderType.Information), Body(body));
        }

        //public virtual string Log(object body, LogHeaderType logType, List<LogHeaderType> headerArgs);

        public virtual string Header(LogHeaderType type, List<LogHeaderType> args = null)
        {
            List<LogHeaderType> logHeaderTypes = new List<LogHeaderType>();
            logHeaderTypes.Add(type);

            if (args != null)
                logHeaderTypes.AddRange(args);

            return BaseHeader() + ArgsHeader(logHeaderTypes);
        }
        public virtual string BaseHeader()
        {
            MethodBase methodBase = new StackFrame(4, false).GetMethod();
            string caller = string.Format("{0}.{1}", methodBase.DeclaringType.Name, methodBase.Name);

            string baseHeader = string.Format("[{0}][{1}]", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fffffff"), caller);

            return baseHeader;
        }

        public virtual string ArgsHeader(List<LogHeaderType> args)
        {
            string argsHeader = string.Empty;
            if(args != null)
                foreach (var arg in args)
                {
                    argsHeader += "[" + arg.ToString() + "]";
                }

            return argsHeader;
        }


        public virtual string Body(object obj)
        {
            return obj.ToString();
        }

    }
}
