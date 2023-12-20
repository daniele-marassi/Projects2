using Additional;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Tools
{
    public class SpeechService
    {
        public static bool serviceActive = true;
        Speech speech;
        string windowCaption;
        Utility utilty;
        System.Collections.Specialized.NameValueCollection appSettings;
        int showTimeout;

        public SpeechService()
        {
            speech = new Speech();
            utilty = new Utility();
            appSettings = ConfigurationManager.AppSettings;
            windowCaption = appSettings["WindowCaption"];
            showTimeout = int.Parse(appSettings["ShowTimeout"]);
        }

        public async Task Start(bool noLoop = false)
        {
            while (serviceActive)
            {
                if (serviceActive == false) return;
                System.Threading.Thread.Sleep(showTimeout);

                var activeWindowTitle = utilty.GetActiveWindowTitle();
                //Console.WriteLine(activeWindowTitle);
                if (activeWindowTitle != windowCaption)
                {
                    speech.Hide();
                }
                
                if(noLoop) serviceActive = false;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void Stop()
        {
            serviceActive = false;
        }
    }
}
