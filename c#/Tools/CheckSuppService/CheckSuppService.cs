using Additional;
using Additional.NLog;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static Tools.Common.ContextMenus;

namespace Tools.CheckSupp
{
    public partial class CheckSuppService : ServiceBase
    {
        Utility utility;
        int sleepOfTheCheckSuppServiceInMilliseconds = 1000;
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
        string accessTokenAdminForService = null;
        string suppSiteBaseUrl = null;
        string message = null;

        public CheckSuppService()
        {
            InitializeComponent();

            appSettings = ConfigurationManager.AppSettings;

            sleepOfTheCheckSuppServiceInMilliseconds = int.Parse(appSettings["SleepOfTheCheckSuppServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(appSettings["LimitLogFileInMB"]);

            this.ServiceName = "CheckSuppService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(appSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);

            notifyMute = bool.Parse(appSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

            accessTokenAdminForService = appSettings["AccessTokenAdminForService"];
            suppSiteBaseUrl = appSettings["SuppSiteBaseUrl"];
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
                    bool response = false;
                    var keyValuePairs = new Dictionary<string, string>() { };
                    message = null;

                    var result = await CallApi(HttpMethod.Get, suppSiteBaseUrl, "Tokens/TokenIsValid", keyValuePairs, accessTokenAdminForService);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response = false;
                        message = "Access Token Admin For Service, Is Not Valid!" + " - ReasonPhrase:" + result.ReasonPhrase + " - RequestMessage:" + result.RequestMessage + " - StatusCode:" + result.StatusCode.ToString();
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<bool>(content);
                    }

                    if (response == false && message == null) message = "Access Token Admin For Service, Is Not Valid!";
                    else if (response == true) message = "Access Token Admin For Service, Is Valid!";

                    if (response == false && (showError == null || message != showError))
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("CheckSuppServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("CheckSuppService Message:" + message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = message;
                            logger.Error(message);
                        }
                    }
                    else if (response == true && showError != null)
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemRecover("CheckSuppServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                            if (notifyPopupShow) Common.Utility.ShowMessage("CheckSuppService Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                            showError = null;
                            logger.Info(message);
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

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("CheckSuppServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("CheckSuppService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            showError = ex.Message;
                            logger.Error(ex.Message);
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                System.Threading.Thread.Sleep(sleepOfTheCheckSuppServiceInMilliseconds);
            }
        }


        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="keyValuePairs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(HttpMethod methodType, string baseUrl, string apiUrl, Dictionary<string, string> keyValuePairs = null, string token = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var handler = new HttpClientHandler()
            {
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
            };

            using (HttpClient client = new HttpClient(handler))
            {
                //init
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMinutes(10);

                var par = "";

                //add parameters in Uri
                if (((keyValuePairs != null && keyValuePairs.Count > 0) && (methodType == HttpMethod.Get || methodType == HttpMethod.Delete)))
                {
                    par = JsonToQuery(JsonConvert.SerializeObject(keyValuePairs));
                }

                //create request
                var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl + par);

                if (token != null && token != String.Empty)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                //add parameters in headers
                if ((keyValuePairs != null && keyValuePairs.Count > 0) && (methodType == HttpMethod.Post || methodType == HttpMethod.Put))
                {
                    var content = new FormUrlEncodedContent(keyValuePairs);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    request.Content = content;
                }
                //Console.WriteLine(request);
                //call
                response = await client.SendAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Json To Query
        /// </summary>
        /// <param name="jsonQuery"></param>
        /// <returns></returns>
        public string JsonToQuery(string jsonQuery)
        {
            string str = "?";
            str += jsonQuery.Replace(":", "=").Replace("{", "").
                        Replace("}", "").Replace(",", "&").
                            Replace("\"", "");
            return str;
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }
    }
}
