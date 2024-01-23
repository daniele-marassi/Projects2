using Additional.NLog;
using GoogleCalendar;
using GoogleManagerModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Supp.Interfaces;
using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Supp.Common
{
    public class Info
    {
        private readonly NLogUtility nLogUtility = new NLogUtility();

        /// <summary>
        /// Read Reminders
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ReadReminders(string culture, List<CalendarEvent> data, string type)
        {
            var reminders = "";

            if (culture.ToLower() == "it-it" && type == WebSpeechTypes.ReadRemindersToday.ToString()) reminders = "I promemoria di oggi:";
            if (culture.ToLower() == "en-us" && type == WebSpeechTypes.ReadRemindersToday.ToString()) reminders = "Today's reminders:";

            if (culture.ToLower() == "it-it" && type == WebSpeechTypes.ReadRemindersTomorrow.ToString()) reminders = "I promemoria di domani:";
            if (culture.ToLower() == "en-us" && type == WebSpeechTypes.ReadRemindersTomorrow.ToString()) reminders = "Tomorrow's reminders:";

            if (culture.ToLower() == "it-it" && type == WebSpeechTypes.ReadRemindersBetweenShortTime.ToString()) reminders = "NAME, ricordati di questi promemoria:";
            if (culture.ToLower() == "en-us" && type == WebSpeechTypes.ReadRemindersBetweenShortTime.ToString()) reminders = "NAME, remember these reminders:";

            foreach (var item in data)
            {
                reminders += " " + item.Summary;

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
        /// <param name="suppServiceHostBaseUrl"></param>
        /// <param name="googleAccountResult"></param>
        /// <param name="googleAuthResult"></param>
        /// <param name="classLogger"></param>
        /// <param name="googleAccountsRepository"></param>
        /// <param name="googleAuthsRepository"></param>
        /// <param name="summaryToSearch"></param>
        /// <returns></returns>
        public CalendarEventsResult GetReminders(string token, string userName, long userId, DateTime timeMin, DateTime timeMax, WebSpeechTypes webSpeechTypes, string suppServiceHostBaseUrl, ref GoogleAccountResult googleAccountResult, ref GoogleAuthResult googleAuthResult, Logger classLogger, IGoogleAccountsRepository googleAccountsRepository, IGoogleAuthsRepository googleAuthsRepository, string summaryToSearch = null)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new CalendarEventsResult() { Data = new List<CalendarEvent>(), ResultState = new GoogleManagerModels.ResultType() };

                try
                {
                    var errors = new List<string>() { };
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    if (googleAccountResult == null || googleAccountResult?.Successful == false || googleAccountResult?.Data?.Count() == 0)
                        googleAccountResult = googleAccountsRepository.GetAllGoogleAccounts(token).GetAwaiter().GetResult();

                    if (!googleAccountResult.Successful)
                        errors.Add(nameof(googleAccountsRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    if (errors.Count == 0)
                    {
                        if (googleAuthResult == null || googleAuthResult?.Successful == false || googleAuthResult?.Data?.Count() == 0)
                            googleAuthResult = googleAuthsRepository.GetAllGoogleAuths(token).GetAwaiter().GetResult();

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
                            {
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#Note").ToLower()) == false).ToList();
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#Timer").ToLower()) == false).ToList();
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#AlarmClock").ToLower()) == false).ToList();
                            }

                            if (webSpeechTypes == WebSpeechTypes.ReadNotes)
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#Note").ToLower()) == true).ToList();

                            if (summaryToSearch != null && summaryToSearch != String.Empty)
                                getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(summaryToSearch.ToLower()) == true).ToList();

                            getCalendarEventsResult.Data = getCalendarEventsResult.Data.Where(_ => _.Summary.ToLower().Contains(("#Timer").ToLower()) == false).ToList();

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

        /// <summary>
        /// GetMeteoPhrase
        /// </summary>
        /// <param name="request"></param>
        /// <param name="param"></param>
        /// <param name="culture"></param>
        /// <param name="descriptionActive"></param>
        /// <param name="classLogger"></param>
        /// <returns></returns>
        public string GetMeteoPhrase(string request, string param, string culture, bool descriptionActive, Logger classLogger)
        {
            var result = "";
            var getMeteoResult = GetMeteo(param, classLogger).GetAwaiter().GetResult();

            if (getMeteoResult.Error == null)
            {
                dynamic partOfTheDay = PartsOfTheDayIta.NotSet;
                var day = Days.Oggi;

                if (request.ToLower().Contains(PartsOfTheDayIta.Mattina.ToString().ToLower())) partOfTheDay = PartsOfTheDayIta.Mattina;
                if (request.ToLower().Contains(PartsOfTheDayIta2.Mattino.ToString().ToLower())) partOfTheDay = PartsOfTheDayIta2.Mattino;
                if (request.ToLower().Contains(PartsOfTheDayIta.Pomerriggio.ToString().ToLower())) partOfTheDay = PartsOfTheDayIta.Pomerriggio;
                if (request.ToLower().Contains(PartsOfTheDayIta.Sera.ToString().ToLower())) partOfTheDay = PartsOfTheDayIta.Sera;
                if (request.ToLower().Contains(PartsOfTheDayIta.Notte.ToString().ToLower())) partOfTheDay = PartsOfTheDayIta.Notte;

                if (request.ToLower().Contains(PartsOfTheDayEng.Morning.ToString().ToLower())) partOfTheDay = PartsOfTheDayEng.Morning;
                if (request.ToLower().Contains(PartsOfTheDayEng.Afternoon.ToString().ToLower())) partOfTheDay = PartsOfTheDayEng.Afternoon;
                if (request.ToLower().Contains(PartsOfTheDayEng.Evening.ToString().ToLower())) partOfTheDay = PartsOfTheDayEng.Evening;
                if (request.ToLower().Contains(PartsOfTheDayEng.Night.ToString().ToLower())) partOfTheDay = PartsOfTheDayEng.Night;

                if (param.ToLower().Contains(Days.Oggi.ToString().ToLower())) day = Days.Today;
                if (param.ToLower().Contains(Days.Domani.ToString().ToLower())) day = Days.Tomorrow;

                if (param.ToLower().Contains(Days.Today.ToString().ToLower())) day = Days.Today;
                if (param.ToLower().Contains(Days.Tomorrow.ToString().ToLower())) day = Days.Tomorrow;

                if ((day == Days.Tomorrow || day == Days.Domani) && partOfTheDay == PartsOfTheDayIta.NotSet)
                {
                    if (culture.Trim().ToLower() == "it-it")
                        partOfTheDay = PartsOfTheDayIta.Mattina;

                    if (culture.Trim().ToLower() == "en-us")
                        partOfTheDay = PartsOfTheDayEng.Morning;
                }
                var meteo = MeteoManage(getMeteoResult.Data, culture, partOfTheDay, day, descriptionActive).ToString();

                result = meteo;
            }
            else
            {
                if (culture == "it-it")
                    result = " Non riesco a leggere il meteo.";
                if (culture == "en-us")
                    result = " I can't read the weather.";
            }

            return result;
        }

        /// <summary>
        /// GetMeteo
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="classLogger"></param>
        /// <returns></returns>
        public async Task<(JObject Data, string Error)> GetMeteo(string _value, Logger classLogger)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                (JObject Data, string Error) response;
                response.Data = null;
                response.Error = null;
                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["value"] = _value;
                    var utility = new Additional.Utility();

                    var result = await utility.CallApi(HttpMethod.Get, "http://supp.altervista.org/", "GetMeteo.php", keyValuePairs, null);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Data = null;
                        response.Error = result.ReasonPhrase;
                    }
                    else
                    {
                        content = content.Replace("<meta http-equiv=\"Access-Control-Allow-Origin\" content=\"*\">\n", "");

                        var data = (JObject)JsonConvert.DeserializeObject(content);

                        response.Data = data;
                        response.Error = null;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    response.Data = null;
                    response.Error = ex.Message;
                }

                return response;
            }
        }

        /// <summary>
        /// MeteoManage
        /// </summary>
        /// <param name="src"></param>
        /// <param name="culture"></param>
        /// <param name="partOfTheDay"></param>
        /// <param name="day"></param>
        /// <param name="descriptionActive"></param>
        /// <returns></returns>
        public string MeteoManage(JObject src, string culture, dynamic partOfTheDay, Days day, bool descriptionActive)
        {
            var result = "";
            var description = "";
            var now = DateTime.Now;
            var hour = now.Hour;
            JToken details = null;

            //if (descriptionActive && day == Days.Tomorrow) description = src["data"]["weatherReportTomorrow"]["description"].ToString();
            //if (descriptionActive && (day == Days.Today || description == "" || description == " ")) description = src["data"]["weatherReportToday"]["description"].ToString();

            if (descriptionActive)
            {
                try
                {
                    description = src["data"]["previsionAbstract"]["description"].ToString();
                }
                catch (Exception)
                { }
            }

            if (description != String.Empty) description += ".";

            description = description.Replace("-", " ");
            description = description.Replace(System.Environment.NewLine, " ");

            if (partOfTheDay.ToString() != PartsOfTheDayIta.NotSet.ToString()) hour = (int)partOfTheDay;

            details = src["data"]["hours"][hour];

            var previsions = new Dictionary<int, int>() { };

            for (int i = hour; i <= 24; i++)
            {
                var _details = src["data"]["hours"][i];

                int x = int.Parse(_details["prevision"].ToString());

                if (previsions.ContainsKey(x)) previsions[x] += 1;
                else previsions[x] = 1;
            }

            var prevailingPrevision = previsions.OrderByDescending(_ => _.Value).FirstOrDefault();

            var prevailingPrevisionIndex = prevailingPrevision.Key;

            var pervision = GetPrevisionPrhase(culture, prevailingPrevisionIndex);

            if (culture.Trim().ToLower() == "it-it")
            {
                result = " Ecco le previsioni: ";

                result += " Giornata " + pervision;

                result += description;

                var temperatureSplit = details["temperature"].ToString().Split(',');

                result += " Ora si prevede temperatura " + temperatureSplit[0];

                if (temperatureSplit.Length > 1)
                {
                    if (int.Parse(temperatureSplit[1]) == 1) result += " e " + int.Parse(temperatureSplit[1]) + " grado";
                    else if (int.Parse(temperatureSplit[1]) > 1) result += " e " + int.Parse(temperatureSplit[1]) + " gradi";
                }
                else
                {
                    if (int.Parse(temperatureSplit[0]) == 1) result += " grado";
                    else result += " gradi";
                }

                result += ", umidità " + details["umidity"].ToString().Replace(",", " e ") + " percento";

                var windIntensitySplit = details["windIntensity"].ToString().Split(',');

                result += " e vento " + windIntensitySplit[0];

                if (windIntensitySplit.Length > 1)
                {
                    if (int.Parse(windIntensitySplit[1]) == 1) result += " e " + int.Parse(windIntensitySplit[1]) + " chilometro orario.";
                    else if (int.Parse(windIntensitySplit[1]) > 1) result += " e " + int.Parse(windIntensitySplit[1]) + " chilometri orari.";
                }
                else
                {
                    if (int.Parse(windIntensitySplit[0]) == 1) result += " chilometro orario.";
                    else result += " chilometri orari.";
                }
            }

            if (culture.Trim().ToLower() == "en-us")
            {
                result = " Here are the forecasts: ";

                result += " Day " + pervision;

                result += description;

                var temperatureSplit = details["temperature"].ToString().Split(',');

                result += " Now it is expected temperature " + temperatureSplit[0];

                if (temperatureSplit.Length > 1)
                {
                    if (int.Parse(temperatureSplit[1]) == 1) result += " and " + int.Parse(temperatureSplit[1]) + " degre";
                    else if (int.Parse(temperatureSplit[1]) > 1) result += " and " + int.Parse(temperatureSplit[1]) + " degrees";
                }
                else
                {
                    if (int.Parse(temperatureSplit[0]) == 1) result += " degre";
                    else result += " degrees";
                }

                result += ", umidity " + details["umidity"].ToString().Replace(",", " and ") + " percent";

                var windIntensitySplit = details["windIntensity"].ToString().Split(',');

                result += " and wind " + windIntensitySplit[0];

                if (windIntensitySplit.Length > 1)
                {
                    if (int.Parse(windIntensitySplit[1]) == 1) result += " and " + int.Parse(windIntensitySplit[1]) + " kilometer per hour.";
                    else if (int.Parse(windIntensitySplit[1]) > 1) result += " and " + int.Parse(windIntensitySplit[1]) + " kilometers per hour.";
                }
                else
                {
                    if (int.Parse(windIntensitySplit[0]) == 1) result += " kilometer per hour.";
                    else result += " kilometers per hour.";
                }
            }

            result = result.Replace("&amp;", "&");

            result = System.Net.WebUtility.HtmlDecode(result);

            result = result.Replace("a'", "à");
            result = result.Replace("e'", "è");
            result = result.Replace("o'", "ò");
            result = result.Replace("u'", "ù");
            result = result.Replace("i'", "ì");

            return result;
        }

        /// <summary>
        /// Get Prevision Prhase
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="previsionIndex"></param>
        /// <returns></returns>
        public string GetPrevisionPrhase(string culture, int previsionIndex)
        {
            var result = "";

            if (culture.Trim().ToLower() == "it-it")
            {
                result = "PrevisionIndex: " + previsionIndex.ToString() + " non implementato.";
                //if (previsionIndex == 0) result = "";
                if (previsionIndex == 1) result = "con cielo coperto.";
                if (previsionIndex == 2) result = "serena.";
                //if (previsionIndex == 3) result = "";
                if (previsionIndex == 4) result = "con nebbia fitta.";
                if (previsionIndex == 5) result = "soleggiata.";
                if (previsionIndex == 6) result = "con banchi di nebbia.";
                if (previsionIndex == 7) result = "con neve debole.";
                if (previsionIndex == 8) result = "con neve moderata.";
                if (previsionIndex == 9) result = "con neve forte.";
                if (previsionIndex == 10) result = "con cielo in gran parte nuvoloso.";
                if (previsionIndex == 11) result = "con cielo in gran parte nuvoloso.";
                if (previsionIndex == 12) result = "nuvolosa con pioggia leggera.";
                if (previsionIndex == 13) result = "nuvolosa con pioggia media.";
                if (previsionIndex == 14) result = "nuvolosa con pioggia forte.";
                if (previsionIndex == 15) result = "nuvolosa con pioggia leggera.";
                if (previsionIndex == 16) result = "nuvolosa con pioggia media.";
                if (previsionIndex == 17) result = "nuvolosa con pioggia forte.";
                if (previsionIndex == 18) result = "con pioggia debole.";
                if (previsionIndex == 19) result = "con pioggia moderata.";
                if (previsionIndex == 20) result = "con pioggia forte.";
                if (previsionIndex == 21) result = "prevalentemente soleggiata.";
                if (previsionIndex == 22) result = "poco nuvolosa.";
                if (previsionIndex == 23) result = "con temporale.";
                if (previsionIndex == 24) result = "nuvolosa con temporale.";
                if (previsionIndex == 25) result = "nuvolosa con temporale.";
                if (previsionIndex == 26) result = "con cielo in gran parte nuvoloso.";
                if (previsionIndex == 27) result = "con cielo in gran parte nuvoloso.";
                if (previsionIndex == 28) result = "con cielo coperto.";
                //if (previsionIndex == 29) result = "";
                if (previsionIndex == 30) result = "con neve debole.";
                if (previsionIndex == 31) result = "con neve moderata.";
                if (previsionIndex == 32) result = "con neve forte.";
                if (previsionIndex == 33) result = "con pioggia debole.";
                if (previsionIndex == 34) result = "con pioggia moderata.";
                if (previsionIndex == 35) result = "con pioggia forte.";
                if (previsionIndex == 36) result = "con temporale.";
                //if (previsionIndex == 37) result = "";
                //if (previsionIndex == 38) result = "";
                //if (previsionIndex == 39) result = "";
                //if (previsionIndex == 40) result = "";
            }

            if (culture.Trim().ToLower() == "en-us")
            {
                result = "PrevisionIndex: " + previsionIndex.ToString() + " not implemented.";
                // if (previsionIndex == 0) result = "";
                if (previsionIndex == 1) result = "with cloudy sky.";
                if (previsionIndex == 2) result = "serena.";
                // if (previsionIndex == 3) result = "";
                if (previsionIndex == 4) result = "with thick fog.";
                if (previsionIndex == 5) result = "sunny.";
                if (previsionIndex == 6) result = "with fog banks.";
                if (previsionIndex == 7) result = "with light snow.";
                if (previsionIndex == 8) result = "with moderate snow.";
                if (previsionIndex == 9) result = "with heavy snow.";
                if (previsionIndex == 10) result = "with mostly cloudy skies.";
                if (previsionIndex == 11) result = "with mostly cloudy skies.";
                if (previsionIndex == 12) result = "cloudy with light rain.";
                if (previsionIndex == 13) result = "cloudy with average rain.";
                if (previsionIndex == 14) result = "cloudy with heavy rain.";
                if (previsionIndex == 15) result = "cloudy with light rain.";
                if (previsionIndex == 16) result = "cloudy with average rain.";
                if (previsionIndex == 17) result = "cloudy with heavy rain.";
                if (previsionIndex == 18) result = "with light rain.";
                if (previsionIndex == 19) result = "with moderate rain.";
                if (previsionIndex == 20) result = "with heavy rain.";
                if (previsionIndex == 21) result = "mostly sunny.";
                if (previsionIndex == 22) result = "slightly cloudy.";
                if (previsionIndex == 23) result = "with temporal.";
                if (previsionIndex == 24) result = "cloudy with thunderstorm.";
                if (previsionIndex == 25) result = "cloudy with thunderstorm.";
                if (previsionIndex == 26) result = "with mostly cloudy skies.";
                if (previsionIndex == 27) result = "with mostly cloudy skies.";
                if (previsionIndex == 28) result = "with cloudy sky.";
                // if (previsionIndex == 29) result = "";
                if (previsionIndex == 30) result = "with light snow.";
                if (previsionIndex == 31) result = "with moderate snow.";
                if (previsionIndex == 32) result = "with heavy snow.";
                if (previsionIndex == 33) result = "with light rain.";
                if (previsionIndex == 34) result = "with moderate rain.";
                if (previsionIndex == 35) result = "with heavy rain.";
                if (previsionIndex == 36) result = "with temporal.";
                // if (previsionIndex == 37) result = "";
                // if (previsionIndex == 38) result = "";
                // if (previsionIndex == 39) result = "";
                // if (previsionIndex == 40) result = "";
            }

            return result;
        }

        /// <summary>
        /// Get Holidays Prhase
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="access_token_cookie"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="suppServiceHostBaseUrl"></param>
        /// <param name="classLogger"></param>
        /// <param name="googleAccountsRepository"></param>
        /// <param name="googleAuthsRepository"></param>
        /// <returns></returns>
        public async Task<string> GetHolidaysPrhase(string culture, string access_token_cookie, string userName, long userId, string type, string suppServiceHostBaseUrl, Logger classLogger, IGoogleAccountsRepository googleAccountsRepository, IGoogleAuthsRepository googleAuthsRepository)
        {
            var result = "";
            var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            if (type == WebSpeechTypes.ReadRemindersToday.ToString())
            {
                var getHolidaysTodayResult = await GetHolidays(access_token_cookie, userName, userId, timeMin, timeMax, culture, suppServiceHostBaseUrl, classLogger, googleAccountsRepository, googleAuthsRepository);

                if (getHolidaysTodayResult.Successful && getHolidaysTodayResult.Data.Count > 0)
                {
                    var holidays = "";
                    if (culture.ToLower() == "it-it") holidays = " Le festività di oggi: ";
                    if (culture.ToLower() == "en-us") holidays = " Today's holidays: ";

                    foreach (var item in getHolidaysTodayResult.Data)
                    {
                        holidays += item.Summary + ", ";
                    }

                    result += holidays;
                }
            }

            if (type == WebSpeechTypes.ReadRemindersTomorrow.ToString())
            {
                timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(1);
                timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59").AddDays(1);

                var getHolidaysTomorrowResult = await GetHolidays(access_token_cookie, userName, userId, timeMin, timeMax, culture, suppServiceHostBaseUrl, classLogger, googleAccountsRepository, googleAuthsRepository);

                if (getHolidaysTomorrowResult.Successful && getHolidaysTomorrowResult.Data.Count > 0)
                {
                    var holidays = "";
                    if (culture.ToLower() == "it-it") holidays = " Le festività di domani: ";
                    if (culture.ToLower() == "en-us") holidays = " Tomorrow's holidays: ";

                    foreach (var item in getHolidaysTomorrowResult.Data)
                    {
                        holidays += item.Summary + ", ";
                    }

                    result += holidays;
                }
            }

            return result;
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
        /// <param name="suppServiceHostBaseUrl"></param>
        /// <param name="classLogger"></param>
        /// <param name="googleAccountsRepository"></param>
        /// <param name="googleAuthsRepository"></param>
        /// <returns></returns>
        public async Task<HolidaysResult> GetHolidays(string token, string userName, long userId, DateTime timeMin, DateTime timeMax, string culture, string suppServiceHostBaseUrl, Logger classLogger, IGoogleAccountsRepository googleAccountsRepository, IGoogleAuthsRepository googleAuthsRepository)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new HolidaysResult() { Data = new List<Holiday>(), ResultState = new GoogleManagerModels.ResultType() };

                try
                {
                    var errors = new List<string>() { };
                    var identity = userName + userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    var googleAccountResult = await googleAccountsRepository.GetAllGoogleAccounts(token);

                    if (!googleAccountResult.Successful)
                        errors.Add(nameof(googleAccountsRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    if (errors.Count == 0)
                    {
                        var googleAuthResult = await googleAuthsRepository.GetAllGoogleAuths(token);

                        var googleAccounts = googleAccountResult.Data.Where(_ => _.UserId == userId && _.AccountType == AccountType.Calendar.ToString()).ToList();
                        var googleAuthIds = googleAccounts.Select(_ => _.GoogleAuthId).ToList();

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
    }
}
