using Additional;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Tools
{
    public class Speech
    {
        Utility utilty;
        System.Collections.Specialized.NameValueCollection appSettings;
        int windowWidth;
        int windowHeight;
        int windowX;
        int windowY;
        string windowCaption;
        string browserPath;
        string browserExeName;
        string host;
        string username;
        string password;
        string webAddress;
        string[] webAddressParamsArray;
        int workingAreaWidth;
        int workingAreaHeight;
        int screenWidth;
        int screenHeight;
        bool fullScreen;
        int showTimeout;
        bool alwaysShow;

        public Speech()
        {
            utilty = new Utility();
            appSettings = ConfigurationManager.AppSettings;
            windowWidth = int.Parse(appSettings["WindowWidth"]);
            windowHeight = int.Parse(appSettings["WindowHeight"]);
            windowX = 0;
            windowY = 0;
            windowCaption = appSettings["WindowCaption"];
            browserPath = appSettings["BrowserPath"];
            browserExeName = appSettings["BrowserExeName"];
            host = appSettings["Host"];
            username = appSettings["Username"];
            password = appSettings["Password"];
            webAddress = appSettings["WebAddress"];
            webAddressParamsArray = appSettings["WebAddressParamsSeparatedWithComma"].Split(',');

            fullScreen = bool.Parse(appSettings["FullScreen"]);
            alwaysShow = bool.Parse(appSettings["AlwaysShow"]);

            workingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;
            workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            showTimeout = int.Parse(appSettings["ShowTimeout"]);
        }

        public void Start()
        {
            var webAddressParamsString = "";
            (int? ProcessId, string Error) result;
            if (webAddressParamsArray.Count() > 0) webAddress += "?";

            foreach (var item in webAddressParamsArray)
            {
                if (webAddressParamsString != "") webAddressParamsString += "&";
                webAddressParamsString += item;
            }

            webAddress += webAddressParamsString;

            utilty.KillProcessByWindowCaption(windowCaption);

            windowX = 0;
            windowY = workingAreaHeight;

            windowY = 0;
            //windowWidth = 10;
            //windowHeight = 10;

            if (fullScreen && alwaysShow) result = utilty.RunAS(browserPath, browserExeName, $"--chrome-frame --enable-speech-dispatcher --window-size={workingAreaWidth + 20},{workingAreaHeight + 50} --window-position={-10},{-40} --app={webAddress}", host, username, password, true, true, false);
            else if (!fullScreen && alwaysShow) result = utilty.RunAS(browserPath, browserExeName, $"--chrome-frame --enable-speech-dispatcher --window-size={windowWidth},{windowHeight} --window-position={(workingAreaWidth - windowWidth) + 10},{(workingAreaHeight - windowHeight) + 10} --app={webAddress}", host, username, password, true, true, false);
            else result = utilty.RunAS(browserPath, browserExeName, $"--chrome-frame --enable-speech-dispatcher --window-size={windowWidth},{windowHeight} --window-position={windowX},{windowY} --app={webAddress}", host, username, password, true, true, false);

            var _processId = (int)result.ProcessId;

            RemoveFocus();
        }

        public void RemoveFocus() 
        {
            Cursor.Position = new Point(workingAreaWidth - 320, workingAreaHeight + 20);

            System.Threading.Thread.Sleep(1000);

            VirtualMouse.LeftClick();
        }

        public void Stop()
        {
            var windowCaption = appSettings["WindowCaption"];
            utilty.KillProcessByWindowCaption(windowCaption);
        }

        public void Show()
        {
            //windowWidth = 530;
            //windowHeight = 600;

            try
            {     
                if(fullScreen) utilty.MoveExtWindow(windowCaption, -10, -40, workingAreaWidth + 20, workingAreaHeight + 50);
                else utilty.MoveExtWindow(windowCaption, (workingAreaWidth - windowWidth) + 10, (workingAreaHeight - windowHeight) + 10, windowWidth, windowHeight);

                var speechService = new SpeechService();
                Task.Run(() => speechService.Start(true));
            }
            catch (Exception)
            {
            }
        }

        public void Hide()
        {
            //windowWidth = 530;
            //windowHeight = 600;

            windowX = 0;
            windowY = workingAreaHeight;

            try
            {
                utilty.MoveExtWindow(windowCaption, windowX, windowY, windowWidth, windowHeight);
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
                if (fullScreen) utilty.MoveExtWindowByProcessId(processId, -10, -40, workingAreaWidth + 20, workingAreaHeight + 50);
                else utilty.MoveExtWindowByProcessId(processId, (workingAreaWidth - windowWidth) + 10, (workingAreaHeight - windowHeight) + 10, windowWidth, windowHeight);

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
                utilty.MoveExtWindowByProcessId(processId, windowX, windowY, windowWidth, windowHeight);
            }
            catch (Exception)
            {
            }
        }
    }
}
