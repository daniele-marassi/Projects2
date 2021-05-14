using Additional;
using System.Configuration;

namespace StartSpeech
{
    class Program
    {
        static void Main(string[] args)
        {
            var utilty = new Utility();

            var appSettings = ConfigurationManager.AppSettings;
            var windowWidth = int.Parse(appSettings["WindowWidth"]);
            var windowHeight = int.Parse(appSettings["WindowHeight"]);
            var windowX = int.Parse(appSettings["WindowX"]);
            var windowY = int.Parse(appSettings["WindowY"]);
            var windowCaption = appSettings["WindowCaption"];
            var browserPath = appSettings["BrowserPath"];
            var browserExeName = appSettings["BrowserExeName"];
            var host = appSettings["Host"];
            var username = appSettings["Username"];
            var password = appSettings["Password"];
            var webAddress = appSettings["WebAddress"];

            utilty.KillProcessByWindowCaption(windowCaption);

            var result = utilty.RunAS(browserPath, browserExeName, $"--chrome-frame  --window-size={windowWidth},{windowHeight} --window-position={windowX},{windowY} --app={webAddress}", host, username, password, true, true, false);
            var _processId = (int)result.ProcessId;
        }
    }
}
