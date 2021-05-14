using System.Collections.Generic;

namespace Mair.DS.Common.Loggers
{
    public interface ILogger
    {
        string Name { get; set; }
        LoggerType Type{ get; set; }
        ILogger Init();
        string Header(LogHeaderType logType, List<LogHeaderType> args = null);
        string Body(object obj);
        string LogInfo(object body);
        string LogErr(object body);
        string LogWarn(object body);

        //string Log(object body, LogHeaderType logType, List<LogHeaderType> headerArgs = null);
    }
}
