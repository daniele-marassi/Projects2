using Additional;
using Additional.NLog;
using Common;
using Google.Apis.Keep.v1.Data;
using GoogleCalendar;
using GoogleManagerModels;
using Newtonsoft.Json;
using NLog;
using RenewNotes.Repositories;
using Supp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Apis.Requests.BatchRequest;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Tools.Common.ContextMenus;

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

        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        private static GoogleAccountResult googleAccountResult = null;
        private static GoogleAuthResult googleAuthResult = null;

        

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
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            commonUtility = new Common.Utility();

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);

            notifyMute = bool.Parse(appSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

            dateLastMessage = DateTime.Now;
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

                        if ((DateTime.Now - dateLastMessage).TotalMinutes > 60)
                        {
                            dateLastMessage = DateTime.Now;

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

                                var timeMin = DateTime.Now;
                                var timeMax = DateTime.Parse(DateTime.Now.AddMinutes(60).ToString());

                                var getRemindersResult = await GetReminders(token, service1Username, userId, timeMin, timeMax, WebSpeechTypes.ReadRemindersToday);

                                var answer = "";

                                if (getRemindersResult.Successful && getRemindersResult.Data.Count > 0)
                                {
                                    var reminders = ReadReminders(getRemindersResult.Data, WebSpeechTypes.ReadRemindersToday.ToString());
                                    if (answer != "") answer += " ";
                                    answer += reminders;

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
                                        if(response) 
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

        private string ReadReminders(List<CalendarEvent> data, string type)
        {
            var reminders = "";

            if (culture.ToLower() == "it-it" && type == WebSpeechTypes.ReadRemindersToday.ToString()) reminders = "NAME, ricordati di questi promemoria:";
            if (culture.ToLower() == "en-us" && type == WebSpeechTypes.ReadRemindersToday.ToString()) reminders = "NAME, remember these reminders:";

            foreach (var item in data)
            {
                reminders += item.Summary;

                if (culture.ToLower() == "it-it")
                {
                    reminders += " alle " + item.EventDateStart.Value.Hour.ToString();

                    if (item.EventDateStart.Value.Minute == 1) reminders += " e " + item.EventDateStart.Value.Minute.ToString() + " minuto.";
                    else if (item.EventDateStart.Value.Minute > 1) reminders += " e " + item.EventDateStart.Value.Minute.ToString() + " minuti.";
                    else reminders += ".";
                }

                if (culture.ToLower() == "en-us")
                {
                    reminders += " at " + item.EventDateStart.Value.Hour.ToString();

                    if (item.EventDateStart.Value.Minute == 1) reminders += " and " + item.EventDateStart.Value.Minute.ToString() + " minute.";
                    else if (item.EventDateStart.Value.Minute > 1) reminders += " and " + item.EventDateStart.Value.Minute.ToString() + " minutes.";
                    else reminders += ".";
                }
            }

            return reminders;
        }

        /// <summary>
        /// Get Reminders
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="timeMin"></param>
        /// <param name="timeMax"></param>
        /// <param name="webSpeechTypes"></param>
        /// <param name="summaryToSearch"></param>
        /// <returns></returns>
        public async Task<CalendarEventsResult> GetReminders(string token, string userName, long userId, DateTime timeMin, DateTime timeMax, WebSpeechTypes webSpeechTypes, string summaryToSearch = null)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new CalendarEventsResult() { Data = new List<CalendarEvent>(), ResultState = new GoogleManagerModels.ResultType() };

                try
                {
                    var errors = new List<string>() { };
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var googleAccountRepository = new GoogleAccountsRepository(suppServiceHostBaseUrl) { };
                    var googleAuthsRepository = new GoogleAuthsRepository(suppServiceHostBaseUrl) { };

                    if (googleAccountResult == null || googleAccountResult?.Successful == false || googleAccountResult?.Data?.Count() == 0)
                        googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(token);

                    if (!googleAccountResult.Successful)
                        errors.Add(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    if (errors.Count == 0)
                    {
                        if (googleAuthResult == null || googleAuthResult?.Successful == false || googleAuthResult?.Data?.Count() == 0)
                            googleAuthResult = await googleAuthsRepository.GetAllGoogleAuths(token);

                        var googleAccounts = googleAccountResult.Data.Where(_ => _.UserId == userId && _.AccountType == AccountType.Calendar.ToString()).ToList();
                        var googleAuthIds = googleAccounts.Select(_ => _.GoogleAuthId).ToList();

                        foreach (var account in googleAccounts)
                        {
                            var auth = googleAuthResult.Data.Where(_ => _.Id == account.GoogleAuthId).FirstOrDefault();
                            var tokenFile = JsonConvert.DeserializeObject<TokenFile>(auth.TokenFileInJson);

                            var googleCalendarUtility = new GoogleCalendarUtility();
                            var getCalendarEventsRequest = new CalendarEventsRequest()
                            {
                                Auth = new Auth()
                                {
                                    Installed = new AuthProperties()
                                    {
                                        Client_id = auth.Client_id,
                                        Client_secret = auth.Client_secret,
                                        Project_id = auth.Project_id
                                    }
                                },
                                TokenFile = tokenFile,
                                Account = account.Account,
                                TimeMin = timeMin,
                                TimeMax = timeMax
                            };

                            var getCalendarEventsResult = googleCalendarUtility.GetCalendarEvents(getCalendarEventsRequest);

                            if (webSpeechTypes == WebSpeechTypes.ReadRemindersToday || webSpeechTypes == WebSpeechTypes.ReadRemindersTomorrow)
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#Note").ToLower()) == false).ToList();

                            if (webSpeechTypes == WebSpeechTypes.ReadNotes)
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#Note").ToLower()) == true).ToList();

                            if (summaryToSearch != null && summaryToSearch != String.Empty)
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(summaryToSearch.ToLower()) == true).ToList();

                            response.Data.AddRange(getCalendarEventsResult.Data);

                            if (getCalendarEventsResult.Successful == false && getCalendarEventsResult.Message != null && getCalendarEventsResult.Message != String.Empty) errors.Add(getCalendarEventsResult.Message);
                        }

                        if (googleAccounts.Count == 0)
                        {
                            response.ResultState = GoogleManagerModels.ResultType.NotFound;
                            errors.Add("No google accounts found!");
                        }
                    }

                    if (response.Data.Count == 0 && errors.Count == 0)
                        response.ResultState = GoogleManagerModels.ResultType.NotFound;
                    if (response.Data.Count > 0 && errors.Count == 0)
                        response.ResultState = GoogleManagerModels.ResultType.Found;
                    if (response.Data.Count > 0 && errors.Count > 0)
                        response.ResultState = GoogleManagerModels.ResultType.FoundWithError;

                    if (response.Data.Count == 0 && errors.Count > 0)
                    {
                        response.Successful = false;
                        response.ResultState = GoogleManagerModels.ResultType.Error;
                    }
                    else
                        response.Successful = true;

                    if (errors.Count > 0)
                        response.Message = JsonConvert.SerializeObject(errors);
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = GoogleManagerModels.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
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
