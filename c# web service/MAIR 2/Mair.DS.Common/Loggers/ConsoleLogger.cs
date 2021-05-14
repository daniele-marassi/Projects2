using System;

namespace Mair.DS.Common.Loggers
{
    public class ConsoleLogger : BaseLogger
    {
        public override ILogger Init()
        {
            base.Init();
            Name = "ConsoleLogger";
            Type = LoggerType.Console;
            return this;
        }

        public override string LogInfo(object body)
        {
            string log = base.LogInfo(body);
            Console.WriteLine(log);
            return log;
        }

        public override string LogErr(object body)
        {
            string log = base.LogErr(body);
            Console.WriteLine(log);
            return log;
        }
    }
}
