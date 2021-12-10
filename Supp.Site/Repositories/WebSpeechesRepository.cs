using Supp.Site.Common;
using SuppModels;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static Supp.Site.Common.Config;
using Additional.NLog;
using Additional;
using System.Security.Cryptography;
using System.Linq;
using GoogleManagerModels;
using GoogleCalendar;

namespace Supp.Site.Repositories
{
    public class WebSpeechesRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public WebSpeechesRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get All WebSpeeches
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> GetAllWebSpeeches(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new SuppModels.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/GetAllWebSpeeches", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = SuppModels.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<WebSpeechResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = SuppModels.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Get WebSpeeches By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> GetWebSpeechesById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new SuppModels.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();


                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/GetWebSpeech", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = SuppModels.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<WebSpeechResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = SuppModels.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Update WebSpeech
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> UpdateWebSpeech(WebSpeechDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new SuppModels.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null)
                            keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Put, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/UpdateWebSpeech", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = SuppModels.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<WebSpeechResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = SuppModels.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Add WebSpeech
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> AddWebSpeech(WebSpeechDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new SuppModels.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null)
                            keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }



                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/AddWebSpeech", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = SuppModels.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<WebSpeechResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = SuppModels.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete WebSpeech By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebSpeechResult> DeleteWebSpeechById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new SuppModels.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();


                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/DeleteWebSpeech", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = SuppModels.ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<WebSpeechResult>(content);

                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = SuppModels.ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
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
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var googleAccountRepository = new GoogleAccountsRepository() { };
                    var googleAuthsRepository = new GoogleAuthsRepository() { };

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
                        var events = googleCalendarUtility.GetCalendarEvents(getCalendarEventsRequest);

                        if(webSpeechTypes == WebSpeechTypes.ReadRemindersToday || webSpeechTypes == WebSpeechTypes.ReadRemindersTomorrow)
                            events.Data = events.Data.Where(_ => _.Summary.Contains("#Note", StringComparison.InvariantCultureIgnoreCase) == false).ToList();

                        if (webSpeechTypes == WebSpeechTypes.ReadNotes)
                            events.Data = events.Data.Where(_ => _.Summary.Contains("#Note", StringComparison.InvariantCultureIgnoreCase) == true).ToList();

                        if(summaryToSearch != null && summaryToSearch != String.Empty)
                            events.Data = events.Data.Where(_ => _.Summary.Contains(summaryToSearch, StringComparison.InvariantCultureIgnoreCase) == true).ToList();

                        response.Data.AddRange(events.Data);

                        if(events.Successful == false && response.Message != null && response.Message != String.Empty) errors.Add(events.Message);
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
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        public async Task<HolidaysResult> GetHolidays(string token, string userName, long userId, DateTime timeMin, DateTime timeMax, string culture)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new HolidaysResult() { Data = new List<Holiday>(), ResultState = new GoogleManagerModels.ResultType() };

                try
                {
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var googleAccountRepository = new GoogleAccountsRepository() { };
                    var googleAuthsRepository = new GoogleAuthsRepository() { };
                    var mediaConfigurationsRepository = new MediaConfigurationsRepository() { };

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

                        var countryAndLanguageType = CountryAndLanguageType.it_italian;

                        if (culture.Trim().ToLower() == "it-it") countryAndLanguageType = CountryAndLanguageType.it_italian;
                        if (culture.Trim().ToLower() == "en-us") countryAndLanguageType = CountryAndLanguageType.en_uk;

                        var holidays = await googleCalendarUtility.GetHolidays(countryAndLanguageType, auth.GooglePublicKey, timeMin, timeMax);

                        response.Data.AddRange(holidays.Data);

                        if (holidays.Successful == false && response.Message != null && response.Message != String.Empty) errors.Add(holidays.Message);
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
                    response.Message = ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }
    }
}