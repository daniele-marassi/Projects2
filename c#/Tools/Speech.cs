using Additional;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools
{
    public class Speech
    {
        Utility utility;
        Common.Utility commonUtility;
        System.Collections.Specialized.NameValueCollection appSettings;
        int windowWidth;
        int windowHeight;
        int windowX;
        int windowY;
        string windowCaption;
        string browserPath;
        string browserExeName;
        string host;
        string windowsUsername;
        string windowsPassword;
        string suppUsername;
        string suppPassword;
        string suppSiteSpeechAppUrl;
        string suppSiteBaseUrl;
        string[] suppSiteSpeechAppUrlParamsArray;
        int workingAreaWidth;
        int workingAreaHeight;
        bool fullScreen;
        int showTimeout;
        bool alwaysShow;
        double volumePercent;
        int volumeOfNotify;

        public Speech()
        {
            utility = new Utility();
            commonUtility = new Common.Utility();
            appSettings = ConfigurationManager.AppSettings;
            windowWidth = int.Parse(appSettings["WindowWidth"]);
            windowHeight = int.Parse(appSettings["WindowHeight"]);
            windowX = 0;
            windowY = 0;
            windowCaption = appSettings["WindowCaption"];
            browserPath = appSettings["BrowserPath"];
            browserExeName = appSettings["BrowserExeName"];
            host = appSettings["Host"];
            windowsUsername = appSettings["WindowsUsername"];
            windowsPassword = appSettings["WindowsPassword"];
            suppUsername = appSettings["SuppUsername"];
            suppPassword = appSettings["SuppPassword"];
            suppSiteBaseUrl = appSettings["SuppSiteBaseUrl"];
            suppSiteSpeechAppUrl = appSettings["SuppSiteSpeechAppUrl"];
            var suppSiteSpeechAppUrlParamsSeparatedWithCommaString = appSettings["SuppSiteSpeechAppUrlParamsSeparatedWithComma"];
            suppSiteSpeechAppUrlParamsSeparatedWithCommaString = suppSiteSpeechAppUrlParamsSeparatedWithCommaString.Replace("#USERNAME#", suppUsername);
            suppSiteSpeechAppUrlParamsSeparatedWithCommaString = suppSiteSpeechAppUrlParamsSeparatedWithCommaString.Replace("#PASSWORD#", suppPassword);

            suppSiteSpeechAppUrlParamsArray = suppSiteSpeechAppUrlParamsSeparatedWithCommaString.Split(',');

            fullScreen = bool.Parse(appSettings["FullScreen"]);
            alwaysShow = bool.Parse(appSettings["AlwaysShow"]);

            workingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;
            workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;

            showTimeout = int.Parse(appSettings["ShowTimeout"]);
            volumePercent = double.Parse(appSettings["VolumePercent"]);

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);
        }

        private enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT }

        private (TaskBarLocation Position, int Amount) GetTaskBarInfo()
        {
            (TaskBarLocation Position, int Amount) result;
            result.Position = TaskBarLocation.BOTTOM;
            result.Amount = 0;

            TaskBarLocation taskBarLocation = TaskBarLocation.BOTTOM;
            bool taskBarOnTopOrBottom = (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width);
            if (taskBarOnTopOrBottom)
            {
                if (Screen.PrimaryScreen.WorkingArea.Top > 0) taskBarLocation = TaskBarLocation.TOP;
                result.Amount = Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;
            }
            else
            {
                if (Screen.PrimaryScreen.WorkingArea.Left > 0)
                {
                    taskBarLocation = TaskBarLocation.LEFT;
                }
                else
                {
                    taskBarLocation = TaskBarLocation.RIGHT;
                }
                result.Amount = Screen.PrimaryScreen.Bounds.Width - Screen.PrimaryScreen.WorkingArea.Width;
            }

            result.Position = taskBarLocation;
            
            return result;
        }

        public void Start(bool removeFocus = true, bool windowNormalFormat = false, bool hide= false, bool firstCall = false)
        {        
            var suppSiteSpeechAppUrlParamsString = "";
            (int? ProcessId, string Error) result;
            result.ProcessId = 0;
            result.Error = String.Empty;

            if (firstCall) Task.Run(() => commonUtility.SetVolume(volumePercent));

            if (suppSiteSpeechAppUrlParamsArray.Count() > 0) suppSiteSpeechAppUrl += "?";

            foreach (var item in suppSiteSpeechAppUrlParamsArray)
            {
                if (firstCall && item.Contains("_onlyRefresh"))
                {
                }
                else
                {
                    if (suppSiteSpeechAppUrlParamsString != "") suppSiteSpeechAppUrlParamsString += "&";
                    suppSiteSpeechAppUrlParamsString += item;
                }
            }

            suppSiteSpeechAppUrl += suppSiteSpeechAppUrlParamsString;

            utility.KillProcessByWindowCaption(windowCaption);

            windowX = 0;
            windowY = workingAreaHeight;

            windowY = 0;
            //windowWidth = 10;
            //windowHeight = 10;

            var taskBarInfo = GetTaskBarInfo();
            var taskBarHeight = 0;
            var taskBarWidth = 0;

            if (taskBarInfo.Position == TaskBarLocation.TOP || taskBarInfo.Position == TaskBarLocation.BOTTOM) taskBarHeight = taskBarInfo.Amount;
            if (taskBarInfo.Position == TaskBarLocation.LEFT || taskBarInfo.Position == TaskBarLocation.RIGHT) taskBarWidth = taskBarInfo.Amount;

            if (hide == true)
            {
                windowX = 0;
                windowY = workingAreaHeight;
            }

            if (fullScreen && alwaysShow && windowNormalFormat == false && hide == false) result = utility.RunAS(browserPath, browserExeName, $"-–ignore-certificate-errors --chrome-frame --enable-speech-dispatcher --window-size={workingAreaWidth + 20},{workingAreaHeight + taskBarHeight+10} --window-position={-10},{-(taskBarHeight)} --app={suppSiteBaseUrl + suppSiteSpeechAppUrl}", host, windowsUsername, windowsPassword, true, true, false);
            else if (!fullScreen && alwaysShow && windowNormalFormat == false && hide == false) result = utility.RunAS(browserPath, browserExeName, $"-–ignore-certificate-errors --chrome-frame --enable-speech-dispatcher --window-size={windowWidth},{windowHeight} --window-position={(workingAreaWidth - windowWidth) + 10},{(workingAreaHeight - windowHeight) + 10} --app={suppSiteBaseUrl + suppSiteSpeechAppUrl}", host, windowsUsername, windowsPassword, true, true, false);
            else if (windowNormalFormat == false && hide == true) result = utility.RunAS(browserPath, browserExeName, $"-–ignore-certificate-errors --chrome-frame --enable-speech-dispatcher --window-size={windowWidth},{windowHeight} --window-position={windowX},{windowY} --app={suppSiteBaseUrl + suppSiteSpeechAppUrl}", host, windowsUsername, windowsPassword, true, true, false);
            else if (windowNormalFormat && hide == false) 
                result = utility.RunAS(browserPath, browserExeName, $"-–ignore-certificate-errors --new-window --enable-speech-dispatcher --window-size={workingAreaWidth},{workingAreaHeight} --window-position={0},{0} {suppSiteBaseUrl + suppSiteSpeechAppUrl}", host, windowsUsername, windowsPassword, true, true, false);

            //var _processId = (int)result.ProcessId;

            if (removeFocus) commonUtility.ClickOnTaskbar();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void Stop()
        {
            var windowCaption = appSettings["WindowCaption"];
            utility.KillProcessByWindowCaption(windowCaption);
        }

        public void Show(bool windowNormalFormat = false)
        {
            //windowWidth = 530;
            //windowHeight = 600;
            IntPtr result = default(IntPtr);
            try
            {     
                if(fullScreen && windowNormalFormat == false) result = utility.MoveExtWindow(windowCaption, -10, -40, workingAreaWidth + 20, workingAreaHeight + 50);
                else if(windowNormalFormat == false) result = utility.MoveExtWindow(windowCaption, (workingAreaWidth - windowWidth) + 10, (workingAreaHeight - windowHeight) + 10, windowWidth, windowHeight);

                if (result == default(IntPtr) || windowNormalFormat) Start(false, windowNormalFormat);

                if (!alwaysShow)
                {
                    var speechService = new SpeechService();
                    Task.Run(() => speechService.Start(true));
                }
            }
            catch (Exception)
            {
            }
        }

        public void HideAfterShow()
        {
            Start(false, false, true);
        }

        public void Restart()
        {
            //Stop();
            Start(false, false, false,false);
        }

        public void Hide()
        {
            //windowWidth = 530;
            //windowHeight = 600;

            windowX = 0;
            windowY = workingAreaHeight;
            IntPtr result = default(IntPtr);

            try
            {
                utility.MoveExtWindow(windowCaption, windowX, windowY, windowWidth, windowHeight);
            }
            catch (Exception)
            {
            }            
        }

        public void ShowByProcessId(int? processId)
        {
            //windowWidth = 530;
            //windowHeight = 600;

            try
            {
                if (fullScreen) utility.MoveExtWindowByProcessId(processId, -10, -40, workingAreaWidth + 20, workingAreaHeight + 50);
                else utility.MoveExtWindowByProcessId(processId, (workingAreaWidth - windowWidth) + 10, (workingAreaHeight - windowHeight) + 10, windowWidth, windowHeight);

                var speechService = new SpeechService();
                Task.Run(() => speechService.Start(true));
            }
            catch (Exception)
            {
            }
        }

        public void HideByProcessId(int? processId)
        {
            //windowWidth = 530;
            //windowHeight = 600;

            windowX = 0;
            windowY = workingAreaHeight;

            try
            {
                utility.MoveExtWindowByProcessId(processId, windowX, windowY, windowWidth, windowHeight);
            }
            catch (Exception)
            {
            }
        }
    }
}
