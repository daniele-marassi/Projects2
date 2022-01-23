using Supp.Site.Common;
using Supp.Models;
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
using Google.Apis.Calendar.v3.Data;

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
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/GetAllWebSpeeches", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();


                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/GetWebSpeech", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new Supp.Models.ResultType() };

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
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new Supp.Models.ResultType() };

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
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                var response = new WebSpeechResult() { Data = new List<WebSpeechDto>(), ResultState = new Supp.Models.ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();


                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/WebSpeeches/DeleteWebSpeech", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = Supp.Models.ResultType.Error;
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
                    response.ResultState = Supp.Models.ResultType.Error;
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
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
                        var getCalendarEventsResult = googleCalendarUtility.GetCalendarEvents(getCalendarEventsRequest);

                        if(webSpeechTypes == WebSpeechTypes.ReadRemindersToday || webSpeechTypes == WebSpeechTypes.ReadRemindersTomorrow)
                            getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.Contains("#Note", StringComparison.InvariantCultureIgnoreCase) == false).ToList();

                        if (webSpeechTypes == WebSpeechTypes.ReadNotes)
                            getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.Contains("#Note", StringComparison.InvariantCultureIgnoreCase) == true).ToList();

                        if(summaryToSearch != null && summaryToSearch != String.Empty)
                            getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.Contains(summaryToSearch, StringComparison.InvariantCultureIgnoreCase) == true).ToList();

                        response.Data.AddRange(getCalendarEventsResult.Data);

                        if(getCalendarEventsResult.Successful == false && getCalendarEventsResult.Message != null && getCalendarEventsResult.Message != String.Empty) errors.Add(getCalendarEventsResult.Message);
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

        /// <summary>
        /// Get Holidays
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="timeMin"></param>
        /// <param name="timeMax"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

                        var getHolidaysResult = await googleCalendarUtility.GetHolidays(countryAndLanguageType, auth.GooglePublicKey, timeMin, timeMax);

                        response.Data.AddRange(getHolidaysResult.Data);

                        if (getHolidaysResult.Successful == false && getHolidaysResult.Message != null && getHolidaysResult.Message != String.Empty) errors.Add(getHolidaysResult.Message);
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

        /// <summary>
        /// Edit Last Reminder
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="webSpeechTypes"></param>
        /// <param name="editCalendarEventRequest"></param>
        /// <returns></returns>
        public async Task<EventResult> EditLastReminder(string token, string userName, long userId, WebSpeechTypes webSpeechTypes, EditCalendarEventRequest editCalendarEventRequest)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<Event>(), ResultState = new GoogleManagerModels.ResultType() };

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

                    if (editCalendarEventRequest.SummaryToSearch == null) editCalendarEventRequest.SummaryToSearch = String.Empty;

                    if (webSpeechTypes == WebSpeechTypes.EditNote && editCalendarEventRequest.SummaryToSearch.StartsWith("#Note") == false)
                        editCalendarEventRequest.SummaryToSearch = "#Note" + " " + editCalendarEventRequest.SummaryToSearch.Trim();

                    foreach (var account in googleAccounts)
                    {
                        var auth = googleAuthResult.Data.Where(_ => _.Id == account.GoogleAuthId).FirstOrDefault();
                        var tokenFile = JsonConvert.DeserializeObject<TokenFile>(auth.TokenFileInJson);

                        var googleCalendarUtility = new GoogleCalendarUtility();
                        var _editCalendarEventRequest = new EditCalendarEventRequest()
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
                            TimeMin = editCalendarEventRequest.TimeMin,
                            TimeMax = editCalendarEventRequest.TimeMax,
                            SummaryToSearch = editCalendarEventRequest.SummaryToSearch,
                            Description = editCalendarEventRequest.Description,
                            Summary = editCalendarEventRequest.Summary,
                            Color = editCalendarEventRequest.Color,
                            EventDateEnd = editCalendarEventRequest.EventDateEnd,
                            EventDateStart = editCalendarEventRequest.EventDateStart,
                            IdToSearch = editCalendarEventRequest.IdToSearch,
                            Location = editCalendarEventRequest.Location,
                            NotificationMinutes = editCalendarEventRequest.NotificationMinutes,
                            DescriptionAppended = editCalendarEventRequest.DescriptionAppended
                        };
                        var editLastCalendarEventBySummaryResult = googleCalendarUtility.EditLastCalendarEventBySummary(_editCalendarEventRequest);

                        response.Data.AddRange(editLastCalendarEventBySummaryResult.Data);

                        if (editLastCalendarEventBySummaryResult.Successful == false && editLastCalendarEventBySummaryResult.Message != null && editLastCalendarEventBySummaryResult.Message != String.Empty) errors.Add(editLastCalendarEventBySummaryResult.Message);
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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Delete Last Reminder
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="webSpeechTypes"></param>
        /// <param name="deleteCalendarEventsRequest"></param>
        /// <returns></returns>
        public async Task<EventResult> DeleteLastReminder(string token, string userName, long userId, WebSpeechTypes webSpeechTypes, DeleteCalendarEventsRequest deleteCalendarEventsRequest)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<Event>(), ResultState = new GoogleManagerModels.ResultType() };

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

                    if (deleteCalendarEventsRequest.Summary == null) deleteCalendarEventsRequest.Summary = String.Empty;

                    if (webSpeechTypes == WebSpeechTypes.EditNote && deleteCalendarEventsRequest.Summary.StartsWith("#Note") == false)
                        deleteCalendarEventsRequest.Summary = "#Note" + " " + deleteCalendarEventsRequest.Summary.Trim();

                    foreach (var account in googleAccounts)
                    {
                        var auth = googleAuthResult.Data.Where(_ => _.Id == account.GoogleAuthId).FirstOrDefault();
                        var tokenFile = JsonConvert.DeserializeObject<TokenFile>(auth.TokenFileInJson);

                        var googleCalendarUtility = new GoogleCalendarUtility();
                        var _deleteCalendarEventsRequest = new DeleteCalendarEventsRequest()
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
                            TimeMin = deleteCalendarEventsRequest.TimeMin,
                            TimeMax = deleteCalendarEventsRequest.TimeMax,
                            Summary = deleteCalendarEventsRequest.Summary
                        };
                        var deleteLastCalendarEventBySummaryResult = googleCalendarUtility.DeleteLastCalendarEventBySummary(_deleteCalendarEventsRequest);

                        response.Data.AddRange(deleteLastCalendarEventBySummaryResult.Data);

                        if (deleteLastCalendarEventBySummaryResult.Successful == false && deleteLastCalendarEventBySummaryResult.Message != null && deleteLastCalendarEventBySummaryResult.Message != String.Empty) errors.Add(deleteLastCalendarEventBySummaryResult.Message);
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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

        /// <summary>
        /// Create Reminder
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="webSpeechTypes"></param>
        /// <param name="createCalendarEventsRequest"></param>
        /// <returns></returns>
        public async Task<EventResult> CreateReminder(string token, string userName, long userId, WebSpeechTypes webSpeechTypes, CreateCalendarEventRequest createCalendarEventsRequest, string accountName)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EventResult() { Data = new List<Event>(), ResultState = new GoogleManagerModels.ResultType() };

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

                    if (createCalendarEventsRequest.Summary == null) createCalendarEventsRequest.Summary = String.Empty;

                    if (webSpeechTypes == WebSpeechTypes.EditNote && createCalendarEventsRequest.Summary.StartsWith("#Note") == false)
                        createCalendarEventsRequest.Summary = "#Note" + " " + createCalendarEventsRequest.Summary.Trim();

                    var account = googleAccounts.Where(_=>_.Account.Trim().ToLower() == accountName.Trim().ToLower()).FirstOrDefault();

                    var auth = googleAuthResult.Data.Where(_ => _.Id == account.GoogleAuthId).FirstOrDefault();
                    var tokenFile = JsonConvert.DeserializeObject<TokenFile>(auth.TokenFileInJson);

                    var googleCalendarUtility = new GoogleCalendarUtility();
                    var _createCalendarEventRequest = new CreateCalendarEventRequest()
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
                        Summary = createCalendarEventsRequest.Summary,
                        Color = createCalendarEventsRequest.Color,
                        Description = createCalendarEventsRequest.Description,
                        EventDateStart = createCalendarEventsRequest.EventDateStart,
                        EventDateEnd = createCalendarEventsRequest.EventDateEnd,
                        Location = createCalendarEventsRequest.Location,
                        NotificationMinutes = createCalendarEventsRequest.NotificationMinutes
                    };
                    var createCalendarEventResult = googleCalendarUtility.CreateCalendarEvent(_createCalendarEventRequest);

                    response.Data.AddRange(createCalendarEventResult.Data);

                    if (createCalendarEventResult.Successful == false && createCalendarEventResult.Message != null && createCalendarEventResult.Message != String.Empty) errors.Add(createCalendarEventResult.Message);

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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }
    }
}