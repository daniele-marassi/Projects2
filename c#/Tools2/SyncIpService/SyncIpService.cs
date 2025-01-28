using Additional;
using Additional.NLog;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tools.Common.ContextMenus;

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
        private static bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        int volumeOfNotify;
        bool notifyMute;
        bool notifyPopupShow;
        System.Collections.Specialized.NameValueCollection appSettings;

        public SyncIpService()
        {
            InitializeComponent();

            appSettings = ConfigurationManager.AppSettings;

            sleepOfTheSyncIpServiceInMilliseconds = int.Parse(appSettings["SleepOfTheSyncIpServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(appSettings["LimitLogFileInMB"]);

            this.ServiceName = "SyncIpService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(appSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);

            notifyMute = bool.Parse(appSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);
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
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SyncIpServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                                if (notifyPopupShow) Common.Utility.ShowMessage("SyncIpService Message:" + message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = message;
                                logger.Error(message);
                            }
                        }
                        else if (state == "Successful" && showError != null)
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemRecover("SyncIpServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                                if (notifyPopupShow) Common.Utility.ShowMessage("SyncIpService Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
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
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SyncIpServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("SyncIpService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = ex.Message;
                            logger.Error(ex.Message);
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

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
