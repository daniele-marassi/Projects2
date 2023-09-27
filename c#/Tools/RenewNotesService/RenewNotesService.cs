using Additional;
using Additional.NLog;
using Google.Apis.Calendar.v3.Data;
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
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tools.Common.ContextMenus;

namespace Tools.RenewNotes
{
    public partial class RenewNotesService : ServiceBase
    {
        Utility utility;
        int sleepOfTheRenewNotesServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        string rootPath;
        string showError = null;
        bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        string suppUsername;
        string suppPassword;
        string suppServiceHostBaseUrl;
        int volumeOfNotify;
        bool notifyMute;
        bool notifyPopupShow;
        System.Collections.Specialized.NameValueCollection appSettings;

        public RenewNotesService()
        {
            InitializeComponent();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            appSettings = ConfigurationManager.AppSettings;

            sleepOfTheRenewNotesServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheRenewNotesServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

            this.ServiceName = "RenewNotesService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            suppUsername = appSettings["SuppUsername"];
            suppPassword = appSettings["SuppPassword"];
            suppServiceHostBaseUrl = appSettings["SuppServiceHostBaseUrl"];

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
                    var loginResult = await Login(suppUsername, suppPassword, false);

                    if ((loginResult.Successful == false || loginResult.Data.Count == 0) && (showError == null || loginResult.Message != showError))
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("RenewNotesServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("RenewNotesService Login Message:" + loginResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = loginResult.Message;
                            logger.Error(loginResult.Message);
                        }
                    }
                    else if (loginResult.Successful && loginResult.Data.Count > 0 && showError != null)
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemRecover("RenewNotesServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                            if (notifyPopupShow) Common.Utility.ShowMessage("RenewNotesService Login Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                            showError = null;
                            logger.Info(loginResult.Message);
                        }
                    }

                    if (loginResult.Successful && loginResult.Data.Count > 0)
                    {
                        var data = loginResult.Data.FirstOrDefault();

                        var userId = data.UserId;
                        var token = data.TokenCode;

                        var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(-7);
                        var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                        var renewNotesResult = await RenewNotes(token, suppUsername, userId, timeMin, timeMax);

                        if ((renewNotesResult.Successful == false || renewNotesResult.Data.Count == 0) && (showError == null || renewNotesResult.Message != showError))
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemWithError("RenewNotesServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                                if (notifyPopupShow) Common.Utility.ShowMessage("RenewNotesService RenewNotes Message:" + renewNotesResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = renewNotesResult.Message;
                                logger.Error(renewNotesResult.Message);
                            }
                        }
                        else if (renewNotesResult.Successful && renewNotesResult.Data.Count > 0 && showError != null)
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemRecover("RenewNotesServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                                if (notifyPopupShow) Common.Utility.ShowMessage("RenewNotesService RenewNotes Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                showError = null;
                                logger.Info(renewNotesResult.Message);
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

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("RenewNotesServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("RenewNotesService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = ex.Message;
                            logger.Error(ex.Message);
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                System.Threading.Thread.Sleep(sleepOfTheRenewNotesServiceInMilliseconds);
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

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

        public async Task<EventResult> RenewNotes(string token, string userName, long userId, DateTime timeMin, DateTime timeMax)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<Event>(), ResultState = new GoogleManagerModels.ResultType() };

                try
                {
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var googleAccountRepository = new GoogleAccountsRepository(suppServiceHostBaseUrl) { };
                    var googleAuthsRepository = new GoogleAuthsRepository(suppServiceHostBaseUrl) { };

                    var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(token);
                    var googleAuthResult = await googleAuthsRepository.GetAllGoogleAuths(token);

                    var googleAccounts = googleAccountResult.Data.Where(_ => _.UserId == userId && _.AccountType == AccountType.Calendar.ToString()).ToList();
                    var googleAuthIds = googleAccounts.Select(_ => _.GoogleAuthId).ToList();

                    var errors = new List<string>() { };

                    foreach (var account in googleAccounts)
                    {
                        var auth = googleAuthResult.Data.Where(_ => _.Id == account.GoogleAuthId).FirstOrDefault();
                        var tokenFile = JsonConvert.DeserializeObject<TokenFile>(auth.TokenFileInJson);

                        var googleCalendarUtility = new GoogleCalendarUtility();

                        var calendarEventsRequest = new CalendarEventsRequest() 
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

                        var getCalendarEventsResult = googleCalendarUtility.GetCalendarEvents(calendarEventsRequest);

                        if (getCalendarEventsResult.Successful)
                        {
                            foreach (var item in getCalendarEventsResult.Data.Where(_=>_.Summary.Contains("#Note")).ToList())
                            {
                                var editCalendarEventRequest = new EditCalendarEventRequest()
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
                                    IdToSearch = item.Id,
                                    EventDateStart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"),
                                    EventDateEnd = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"),
                                    TimeMin = DateTime.Parse(item.EventDateStart.Value.ToString("yyyy-MM-dd") + " 00:00:00"),
                                    TimeMax = DateTime.Parse(item.EventDateStart.Value.ToString("yyyy-MM-dd") + " 23:59:59")
                                };

                                var calendarEventUpdated = googleCalendarUtility.EditCalendarEventById(editCalendarEventRequest);

                                if (calendarEventUpdated.Successful && calendarEventUpdated.Data.Count > 0) response.Data.AddRange(calendarEventUpdated.Data);

                                if (calendarEventUpdated.Successful == false && response.Message != null && response.Message != String.Empty) errors.Add(calendarEventUpdated.Message);
                            }
                        }

                        if (getCalendarEventsResult.Successful == false) errors.Add(getCalendarEventsResult.Message);
                    }

                    if (googleAccounts.Count == 0)
                    {
                        response.ResultState = GoogleManagerModels.ResultType.NotFound;
                        errors.Add("No google accounts found!");
                    }

                    if (response.Data.Count == 0 && errors.Count == 0)
                        response.ResultState = GoogleManagerModels.ResultType.NotFound;
                    if (response.Data.Count > 0 && errors.Count == 0)
                        response.ResultState = GoogleManagerModels.ResultType.Found;
                    if (response.Data.Count > 0 && errors.Count > 0)
                        response.ResultState = GoogleManagerModels.ResultType.FoundWithError;

                    if (response.Data.Count == 0 && errors.Count > 0 && googleAccounts.Count > 0)
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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }
    }
}
