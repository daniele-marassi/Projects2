using Additional;
using Additional.NLog;
using NLog;
using System;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools.WakeUpScreenAfterPowerBreak
{
    public partial class WakeUpScreenAfterPowerBreakService : ServiceBase
    {
        Utility utility;
        Common.Utility commonUtility;
        int sleepOfTheWakeUpScreenAfterPowerBreakServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        string rootPath;
        string showError = null;
        bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;

        public WakeUpScreenAfterPowerBreakService()
        {
            InitializeComponent();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var appSettings = ConfigurationManager.AppSettings;

            sleepOfTheWakeUpScreenAfterPowerBreakServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheWakeUpScreenAfterPowerBreakServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

            this.ServiceName = "WakeUpScreenAfterPowerBreakService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            commonUtility = new Common.Utility();
        }

        public void Stop()
        {
            serviceActive = false;
        }

        public async Task Start()
        {
            var sleepInMilliseconds = 0;
            while (serviceActive)
            {
                if (serviceActive == false) return;
                sleepInMilliseconds = sleepOfTheWakeUpScreenAfterPowerBreakServiceInMilliseconds;

                try
                {
                    var powerStatusResult = PowerStatus();

                    if (powerStatusResult.PluggedIn == false)
                    {
                        commonUtility.ClickOnTaskbar();
                        sleepInMilliseconds = 30000;
                    }
                }
                catch (Exception ex)
                {
                    if (showError == null || ex.Message != showError)
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("WakeUpScreenAfterPowerBreakServiceMenuItem");
                            Common.Utility.ShowMessage("WakeUpScreenAfterPowerBreakService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = ex.Message;
                            logger.Error(ex.Message);
                        }
                    }
                }

                System.Threading.Thread.Sleep(sleepInMilliseconds);
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }

        /// <summary>
        /// Power Status
        /// </summary>
        /// <returns></returns>
        public (bool? PluggedIn, string Status, string BatteryInfo) PowerStatus()
        {
            (bool? PluggedIn, string Status, string BatteryInfo) result;
            result.PluggedIn = null;
            result.Status = null;
            result.BatteryInfo = null;

            try
            {
                var pwr = SystemInformation.PowerStatus;
                result.Status = pwr.BatteryChargeStatus.ToString();

                switch (pwr.PowerLineStatus)
                {
                    case (PowerLineStatus.Offline):
                        result.PluggedIn = false;
                        break;

                    case (PowerLineStatus.Online):
                        result.PluggedIn = true;
                        break;
                }

                result.BatteryInfo += pwr.BatteryLifePercent.ToString("P0");

                if (pwr.BatteryLifeRemaining > 0)
                    result.BatteryInfo += " " +
                        String.Format("{0} hr {1} min remaining",
                        pwr.BatteryLifeRemaining / 3600, (pwr.BatteryLifeRemaining % 3600) / 60);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
