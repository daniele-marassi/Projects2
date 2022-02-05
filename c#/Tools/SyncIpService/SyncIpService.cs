using Additional;
using Additional.NLog;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools.SyncIp
{
    public partial class SyncIpService : ServiceBase
    {
        string address = "";
        string oldAddress = "";
        Utility utility;
        int sleepOfTheSyncIpServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        string rootPath;
        string showError = null;
        bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;

        public SyncIpService()
        {
            InitializeComponent();

            sleepOfTheSyncIpServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheSyncIpServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

            this.ServiceName = "SyncIpService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        }

        public void Stop()
        {
            serviceActive = false;
        }

        public async Task Start()
        {
            while (serviceActive)
            {
                if (serviceActive == false) return;
                try
                {
                    address = utility.GetPublicIPAddress();
                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + address);
                    if (address != oldAddress || showError != null)
                    {
                        var keyValuePairs = new Dictionary<string, string>() { };
                        keyValuePairs["api"] = "SetIp";
                        keyValuePairs["Token"] = "cf870b1832e928369b7872dd741906e4";
                        keyValuePairs["Ip"] = address;

                        var result = await utility.CallApi(HttpMethod.Post, "http://supp.altervista.org/", "Config.php", keyValuePairs, null);
                        var content = await result.Content.ReadAsStringAsync();
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + content);
                        var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                        var message = obj["Message"];
                        var state = obj["State"];

                        if (state != "Successful" && (showError == null || message != showError))
                        {
                            nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SyncIpServiceMenuItem");
                                Common.Utility.ShowMessage("SyncIpService Message:" + message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = message;
                                logger.Error(message);
                            }
                        }
                        else if (state == "Successful" && showError != null)
                        {
                            nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                if (serviceActive) Common.ContextMenus.SetMenuItemRecover("SyncIpServiceMenuItem");
                                Common.Utility.ShowMessage("SyncIpService Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                showError = null;
                                logger.Info(message);
                            }
                        }
                    }
                    oldAddress = address;
                }
                catch (Exception ex)
                {
                    if (showError == null || ex.Message != showError)
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SyncIpServiceMenuItem");
                            Common.Utility.ShowMessage("SyncIpService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = ex.Message;
                            logger.Error(ex.Message);
                        }
                    }
                }

                System.Threading.Thread.Sleep(sleepOfTheSyncIpServiceInMilliseconds);
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }
    }
}
