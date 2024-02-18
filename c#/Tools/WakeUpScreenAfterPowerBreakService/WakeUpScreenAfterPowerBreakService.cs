using Additional;
using Additional.NLog;
using Common;
using Google.Apis.Keep.v1.Data;
using GoogleCalendar;
using GoogleManagerModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RenewNotes.Repositories;
using Supp.Interfaces;
using Supp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using WakeUpScreenAfterPowerBreak.Repositories;
using static Google.Apis.Requests.BatchRequest;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Tools.Common.ContextMenus;
using GoogleAccountsRepository = WakeUpScreenAfterPowerBreak.Repositories.GoogleAccountsRepository;
using GoogleAuthsRepository = WakeUpScreenAfterPowerBreak.Repositories.GoogleAuthsRepository;

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
        private static bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        int volumeOfNotify;
        bool notifyMute;
        bool notifyPopupShow;
        System.Collections.Specialized.NameValueCollection appSettings;
        string service1Username;
        string service1Password;
        string suppServiceHostBaseUrl;
        string culture;
        private static DateTime dateLastMessage;
        string suppSiteBaseUrl;
        long suppSiteUserId = 0;
        string meteoParameterToTheSalutation;
        double volumePercent;
        double informationMorningVolumePercent;

        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        private static GoogleAccountResult googleAccountResult = null;
        private static GoogleAuthResult googleAuthResult = null;
        private readonly Supp.Common.Info commonInfo;
        private static GoogleAccountsRepository googleAccountsRepository;
        private static GoogleAuthsRepository googleAuthsRepository;
        private static bool infoReaded = false;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        public WakeUpScreenAfterPowerBreakService()
        {
            InitializeComponent();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            appSettings = ConfigurationManager.AppSettings;

            sleepOfTheWakeUpScreenAfterPowerBreakServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheWakeUpScreenAfterPowerBreakServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

            service1Username = appSettings["Service1Username"];
            service1Password = appSettings["Service1Password"];
            suppServiceHostBaseUrl = appSettings["SuppServiceHostBaseUrl"];
            culture = appSettings["Culture"];
            suppSiteBaseUrl = appSettings["SuppSiteBaseUrl"];
            long.TryParse(appSettings["SuppSiteUserId"], out suppSiteUserId);

            this.ServiceName = "WakeUpScreenAfterPowerBreakService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            commonUtility = new Common.Utility();

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);

            volumePercent = double.Parse(appSettings["VolumePercent"]);

            informationMorningVolumePercent = double.Parse(appSettings["InformationMorningVolumePercent"]);

            notifyMute = bool.Parse(appSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

            meteoParameterToTheSalutation = appSettings["MeteoParameterToTheSalutation"];

            dateLastMessage = DateTime.Now;

            commonInfo = new Supp.Common.Info();

            googleAccountsRepository = new GoogleAccountsRepository(suppServiceHostBaseUrl) { };
            googleAuthsRepository = new GoogleAuthsRepository(suppServiceHostBaseUrl) { };
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

                        var now = DateTime.Now;

                        if ((now - dateLastMessage).TotalMinutes > 60)
                        {
                            if (Variables.ServiceLoginResult == null || Variables.ServiceLoginResult?.Successful == false || Variables.ServiceLoginResult?.Data?.Count == 0)
                            {
                                Variables.ServiceLoginResult = await Login(service1Username, service1Password, false);
                                googleAccountResult = null;
                                googleAuthResult = null;
                            }

                            if (Variables.ServiceLoginResult.Successful && Variables.ServiceLoginResult.Data.Count > 0)
                            {
                                var data = Variables.ServiceLoginResult.Data.FirstOrDefault();

                                var userId = data.UserId;
                                var token = data.TokenCode;

                                var answer = GetInfo(token, service1Username, userId, suppServiceHostBaseUrl, culture, now);

                                if (answer != null && answer != "" && answer != " ")
                                {
                                    dateLastMessage = DateTime.Now;

                                    bool response = false;
                                    var keyValuePairs = new Dictionary<string, string>() { };
                                    keyValuePairs.Add("message", answer);
                                    keyValuePairs.Add("userId", suppSiteUserId.ToString());

                                    var result = await utility.CallApi(HttpMethod.Post, suppSiteBaseUrl, "WebSpeeches/AddMessage", keyValuePairs, Variables.ServiceLoginResult.Data.FirstOrDefault().TokenCode, true);
                                    var content = await result.Content.ReadAsStringAsync();

                                    if (result.IsSuccessStatusCode == false)
                                    {

                                    }
                                    else
                                    {
                                        response = JsonConvert.DeserializeObject<bool>(content);
                                        if (response)
                                        {
                                            MediaPlayOrPause();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (showError == null || ex.Message != showError)
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("WakeUpScreenAfterPowerBreakServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("WakeUpScreenAfterPowerBreakService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = ex.Message;
                            logger.Error(ex.Message);
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                System.Threading.Thread.Sleep(sleepInMilliseconds);
            }
        }

        private string GetInfo(string token, string userName, long userId, string suppServiceHostBaseUrl, string culture, DateTime now)
        {
            var answer = "";

            if (meteoParameterToTheSalutation != null && meteoParameterToTheSalutation != "")
            {
                if (Supp.Common.Utility.GetPartOfTheDay(now.AddHours(-1.5)) == PartsOfTheDayEng.Morning && !infoReaded)
                {
                    commonUtility.SetVolume(informationMorningVolumePercent);

                    infoReaded = true;

                    var salutation = "NAME";

                    answer = salutation + " " + Supp.Common.Utility.GetSalutation(new CultureInfo(culture, false)) + " ";

                    answer += commonInfo.GetMeteoPhrase(String.Empty, meteoParameterToTheSalutation, culture.ToLower(), true, classLogger);

                    var timeMin = now;
                    var timeMax = DateTime.Parse(now.ToString("yyyy-MM-dd") + " 23:59:59");
                    GoogleAccountResult googleAccountResult = null;
                    GoogleAuthResult googleAuthResult = null;

                    var getRemindersResult = commonInfo.GetReminders(token, userName, userId, timeMin, timeMax, WebSpeechTypes.ReadRemindersToday, suppServiceHostBaseUrl, ref googleAccountResult, ref googleAuthResult, classLogger, googleAccountsRepository, googleAuthsRepository);

                    if (getRemindersResult.Successful && getRemindersResult.Data.Count > 0)
                    {
                        var reminders = commonInfo.ReadReminders(culture.ToLower(), getRemindersResult.Data, WebSpeechTypes.ReadRemindersToday.ToString());
                        if (answer != "") answer += " ";
                        answer += reminders;
                    }

                    if (getRemindersResult.Successful)
                    {
                        if (answer != "") answer += " ";
                        answer += commonInfo.GetHolidaysPrhase(culture.ToLower(), token, userName, userId, WebSpeechTypes.ReadRemindersToday.ToString(), suppServiceHostBaseUrl, classLogger, googleAccountsRepository, googleAuthsRepository).GetAwaiter().GetResult();

                        if (answer != "") answer += " ";
                        answer += commonInfo.GetHolidaysPrhase(culture.ToLower(), token, userName, userId, WebSpeechTypes.ReadRemindersTomorrow.ToString(), suppServiceHostBaseUrl, classLogger, googleAccountsRepository, googleAuthsRepository).GetAwaiter().GetResult();
                    }

                    if (!getRemindersResult.Successful)
                    {
                        if (culture.ToLower() == "it-it") answer += " Attenzione! probabilmente il token google è scaduto.";
                        if (culture.ToLower() == "en-us") answer += " Attention! probably the google token has expired.";
                    }

                    Task.Run(() =>
                        {
                            System.Threading.Thread.Sleep(120000);
                            commonUtility.SetVolume(volumePercent);
                        }
                    );
                }
                else 
                {
                    var timeMin = now;
                    var timeMax = DateTime.Parse(now.AddMinutes(60).ToString());
                    var getRemindersResult = commonInfo.GetReminders(token, userName, userId, timeMin, timeMax, WebSpeechTypes.ReadRemindersToday, suppServiceHostBaseUrl, ref googleAccountResult, ref googleAuthResult, classLogger, googleAccountsRepository, googleAuthsRepository);

                    if (getRemindersResult.Successful && getRemindersResult.Data.Count > 0)
                    {
                        var reminders = commonInfo.ReadReminders(culture, getRemindersResult.Data, WebSpeechTypes.ReadRemindersBetweenShortTime.ToString());
                        if (answer != "") answer += " ";
                        answer += reminders;
                    }
                }
            }

            return answer;
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

        public async Task<TokenResult> Login(string userName, string password, bool passwordAlreadyEncrypted)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["UserName"] = userName;
                    keyValuePairs["Password"] = password;
                    keyValuePairs["PasswordAlreadyEncrypted"] = passwordAlreadyEncrypted.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, suppServiceHostBaseUrl, "api/Tokens/GetToken", keyValuePairs, null);

                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TokenResult>(content);
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.IsAuthenticated = false;
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = "";
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        private void MediaPlayOrPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }
    }
}
