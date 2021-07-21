using Additional;
using Additional.NLog;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Supp.Site.Common;
using Supp.Site.Models;
using Supp.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static Supp.Site.Common.Config;

namespace Supp.Site.Recognition
{
    public class Common
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly SuppUtility suppUtility;
        private readonly WebSpeechesRepository webSpeecheRepo;
        private readonly ExecutionQueuesRepository executionQueueRepo;

        public Common()
        {
            suppUtility = new SuppUtility();
            webSpeecheRepo = new WebSpeechesRepository();
            executionQueueRepo = new ExecutionQueuesRepository();
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public (List<WebSpeechDto> Data, List<ShortcutDto> Shortcuts) GetData(List<WebSpeechDto> data)
        {
            (List<WebSpeechDto> Data, List<ShortcutDto> Shortcuts) result;
            result.Data = new List<WebSpeechDto>() { };
            result.Shortcuts = new List<ShortcutDto>() { };

            foreach (var item in data)
            {
                if (item.UserId == 0) item.PrivateInstruction = false;
                else item.PrivateInstruction = true;

                if (item.ParentIds != null && item.ParentIds != String.Empty)
                {
                    try
                    {
                        if (item.ParentIds != null && item.ParentIds != String.Empty)
                        {
                            var webSpeechIds = JsonConvert.DeserializeObject<long[]>(item.ParentIds);
                            item.WebSpeechIds = webSpeechIds;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                if (item.WebSpeechIds != null && item.WebSpeechIds.Count() > 0)
                {
                    foreach (var id in item.WebSpeechIds)
                    {
                        var _phrase = data.Where(_ => _.Id == id).Select(_ => _.Phrase).FirstOrDefault();
                        if (item.PreviousPhrase == null) item.PreviousPhrase = String.Empty;
                        if (item.PreviousPhrase != "") item.PreviousPhrase += " ";
                        item.PreviousPhrase += _phrase;
                    }
                }

                if (!item.Type.ToLower().Contains("system"))
                    result.Shortcuts.Add(new ShortcutDto() { Id = item.Id, Type = item.Type, Order = item.Order, Title = item.Name.Replace("_", " "), Action = item.Operation.ToStringExtended().Replace("\\", "/") + " " + item.Parameters.ToStringExtended().Replace("\\", "/"), Ico = item.Ico });

                result.Data.Add(item);
            }

            return result;
        }

        /// <summary>
        /// GetWebSpeechDto
        /// </summary>
        /// <param name="_phrase"></param>
        /// <param name="_hostSelected"></param>
        /// <param name="_reset"></param>
        /// <param name="_application"></param>
        /// <param name="_executionQueueId"></param>
        /// <param name="_alwaysShow"></param>
        /// <param name="_id"></param>
        /// <param name="_claims"></param>
        /// <param name="_onlyRefresh"></param>
        /// <param name="_subType"></param>
        /// <param name="_step"></param>
        /// <param name="expiresInSeconds"></param>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<WebSpeechDto> GetWebSpeechDto(string _phrase, string _hostSelected, bool _reset, bool _application, long _executionQueueId, bool _alwaysShow, long _id, ClaimsDto _claims, bool _onlyRefresh, string _subType, int _step, int expiresInSeconds, HttpResponse response, HttpRequest request)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                WebSpeechDto data = null;
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;
                    WebSpeechResult result = null;
                    var _phraseMatched = "";
                    List<ShortcutDto> shortcuts = new List<ShortcutDto>() { };
                    var startAnswer = "";

                    logger.Info("response:" + response?.ToString());
                    logger.Info("request:" + request?.ToString());
                    logger.Info("SuppSiteAccessTokenCookieName:" + GeneralSettings.Constants.SuppSiteAccessTokenCookieName?.ToString());

                    string access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    result = await webSpeecheRepo.GetAllWebSpeeches(access_token_cookie);

                    if (result.Successful == false)
                    {
                        var error = $"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{result.Message}]";
                        //throw new Exception(error);

                        data = new WebSpeechDto() { Answer = "", Ehi = 0, Error = error/*, ResetAfterLoad = true*/ };
                        return data;
                    }

                    var getDataResult = GetData(result.Data);

                    result.Data = getDataResult.Data;
                    shortcuts = getDataResult.Shortcuts.OrderBy(_ => _.Order).ToList();

                    if (_phrase != "" && _phrase != null && (_subType == "" || _subType == null))
                    {
                        var matchPhraseResult = MatchPhrase(_phrase, result.Data, _claims);
                        data = matchPhraseResult.Data;
                        _phraseMatched = matchPhraseResult.PhraseMatched;
                    }
                    else if (_id != 0)
                    {
                        data = result.Data.Where(_ => _.Id == _id).FirstOrDefault();
                        data = GetAnswer(data);
                    }

                    if (data != null && (data.Type == "SystemRunExe" || data.Type == "RunExe") && data.OperationEnable == true)
                    {
                        var executionQueue = new ExecutionQueueDto() { FullPath = data.Operation, Arguments = data.Parameters, Host = _hostSelected, Type = data.Type };
                        var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);

                        if (addExecutionQueueResult.Successful)
                        {
                            _executionQueueId = addExecutionQueueResult.Data.FirstOrDefault().Id;
                        }
                    }

                    if (data != null && data.Type == "Meteo")
                    {
                        var phrase = _phrase;
                        if (phrase == null) phrase = _phraseMatched;
                        data.Answer = GetMeteoPhrase(phrase, data.Parameters, _claims.Configuration.General.Culture.ToLower(), true);
                    }

                    if (data != null && data.Type == "Time")
                    {
                        var now = DateTime.Now;

                        var dayofweek = now.ToString("dddd", new CultureInfo(_claims.Configuration.General.Culture));
                        var month = now.ToString("MMMM", new CultureInfo(_claims.Configuration.General.Culture));

                        if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                            data.Answer = now.Hour.ToString() + " e " + now.Minute.ToString() + " minuti" + ", " + dayofweek + " " + now.Day.ToString() + " " + month;

                        if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                            data.Answer = now.Hour.ToString() + " and " + now.Minute.ToString() + " minutes" + ", " + dayofweek + " " + now.Day.ToString() + " " + month;
                    }

                    if (data != null && data.Type == "SystemWebSearch")
                    {
                        var phrases = new List<string>() { };
                        var phrase = _phrase;

                        try
                        {
                            phrases = JsonConvert.DeserializeObject<List<string>>(data.Phrase);
                        }
                        catch (Exception)
                        {
                            phrases.Add(data.Phrase);
                        }

                        foreach (var item in phrases)
                        {
                            phrase = phrase.Replace(item, "");
                        }
                        //HttpUtility.UrlEncode(phrase.Replace(" ", "+"));
                        string url = "http://www.google.com/search?q=" + phrase.Trim().Replace(" ", "+");
                        data.Parameters = url;
                    }

                    var salutation = _claims.Configuration.Speech.Salutation;
                    if (_claims.Name == null && _claims.Configuration.General.Culture.ToLower() == "it-it") _claims.Name = "tu";
                    if (_claims.Name == null && _claims.Configuration.General.Culture.ToLower() == "en-us") _claims.Name = "you";
                    if (_claims.Surname == null) _claims.Surname = String.Empty;
                    salutation = salutation.Replace("NAME", _claims.Name);
                    salutation = salutation.Replace("SURNAME", _claims.Surname);

                    startAnswer = salutation + " " + SuppUtility.GetSalutation(new CultureInfo(_claims.Configuration.General.Culture, false));

                    if ((_phrase == null || _phrase == "") && data == null && _reset == false && _onlyRefresh == false && (_subType == null || _subType == ""))
                    {
                        data = new WebSpeechDto() { Answer = startAnswer, Ehi = 0, FinalStep = true };

                        var now = DateTime.Now;

                        if (_claims.Configuration.Speech.MeteoParameterToTheSalutation != null && _claims.Configuration.Speech.MeteoParameterToTheSalutation != "" && _application == true && SuppUtility.GetPartOfTheDay(now) == PartsOfTheDayEng.Morning)
                        {
                            data.Answer += GetMeteoPhrase(String.Empty, _claims.Configuration.Speech.MeteoParameterToTheSalutation, _claims.Configuration.General.Culture.ToLower(), _claims.Configuration.Speech.DescriptionMeteoToTheSalutationActive);
                        }
                    }

                    if (
                        (_phrase != null && _phrase != "" && data == null && result != null && _phrase.Split(" ").Count() > 1)
                        || (data == null && _subType == "RequestNotImplemented")
                    )
                    {
                        data = Recognition.RequestNotImplemented.ManageRequestNotImplemented(_subType, _step, expiresInSeconds, _phrase, _claims, response, request);
                    }

                    if (data == null) data = new WebSpeechDto() { Answer = "", Ehi = 0 };

                    data.HostsArray = _claims.Configuration.Speech.HostsArray;
                    data.HostSelected = _claims.Configuration.Speech.HostDefault;
                    data.ListeningWord1 = _claims.Configuration.Speech.ListeningWord1;
                    data.ListeningWord2 = _claims.Configuration.Speech.ListeningWord2;
                    data.ListeningAnswer = _claims.Configuration.Speech.ListeningAnswer;
                    data.Culture = _claims.Configuration.General.Culture;
                    data.StartAnswer = startAnswer;
                    data.Application = _application;
                    data.AlwaysShow = _alwaysShow;
                    data.ExecutionQueueId = _executionQueueId;
                    data.TimesToReset = _claims.Configuration.Speech.TimesToReset;
                    data.OnlyRefresh = _onlyRefresh;

                    if ((_phrase != null && _phrase != "") && (data.FinalStep == false || _phrase == (data.ListeningWord1 + " " + data.ListeningWord2))) data.Ehi = 1;

                    if (_reset == true && _alwaysShow == false)
                    {
                        if (_hostSelected == null || _hostSelected == String.Empty) _hostSelected = _claims.Configuration.Speech.HostDefault;
                        await ExecutionFinished(_executionQueueId, _hostSelected, _application, request);
                    }

                    var shortcutsInJson = JsonConvert.SerializeObject(shortcuts);

                    data.ShortcutsInJson = shortcutsInJson;

                    data.Error = null;

                    return data;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    throw ex;
                }
            }
        }

        /// <summary>
        /// MatchPhrase
        /// </summary>
        /// <param name="_phrase"></param>
        /// <param name="WebSpeechlist"></param>
        /// <param name="_claims"></param>
        /// <returns></returns>
        public (WebSpeechDto Data, string PhraseMatched) MatchPhrase(string _phrase, List<WebSpeechDto> WebSpeechlist, ClaimsDto _claims)
        {
            WebSpeechDto data = null;
            var _words = _phrase.Split(" ");
            var _wordsCount = _words.Count();
            var countMatch = 0;
            var match = 0;
            var perferctMatch = false;
            var skipMatch = false;
            List<WebSpeechDto> _data = null;
            var _phraseMatched = "";
            var rnd = new Random();
            (WebSpeechDto Data, string PhraseMatched) result;
            result.Data = null;
            result.PhraseMatched = null;

            foreach (var item in WebSpeechlist)
            {
                var phrases = new List<string>() { };

                try
                {
                    phrases = JsonConvert.DeserializeObject<List<string>>(item.Phrase);
                }
                catch (Exception)
                {
                    phrases.Add(item.Phrase);
                }

                for (int i = 0; i < phrases.Count; i++)
                {
                    if (item.PreviousPhrase != null && item.PreviousPhrase != String.Empty)
                        phrases[i] = item.PreviousPhrase + " " + phrases[i];
                }

                foreach (var phrase in phrases)
                {
                    var words = phrase.Split(" ");
                    var wordsCount = words.Count();
                    var minMatch = 0;
                    var maxWords = 0;

                    minMatch = (int)(_wordsCount - Math.Ceiling(_wordsCount * decimal.Parse(_claims.Configuration.Speech.MinSpeechWordsCoefficient)));
                    maxWords = (int)(_wordsCount + Math.Ceiling(_wordsCount * decimal.Parse(_claims.Configuration.Speech.MaxSpeechWordsCoefficient)));

                    if (_wordsCount == 2) minMatch = 2;
                    if (minMatch == 0) minMatch = 1;

                    if (item.Type == "SystemWebSearch" && _phrase.Trim().StartsWith(phrase) && skipMatch == false)
                    {
                        skipMatch = true;
                        _data = new List<WebSpeechDto>();
                        _data.Add(item);
                    }

                    if (skipMatch == false)
                    {
                        match = 0;

                        for (int x = 0; x < words.Length; x++)
                        {
                            for (int y = 0; y < _words.Length; y++)
                            {
                                if (_words[y].Trim().ToLower() == words[x].Trim().ToLower() && words[x] != "")
                                {
                                    match++;
                                    words[x] = "";
                                }
                            }
                        }

                        if ((match > countMatch || match == _wordsCount) && perferctMatch == false && wordsCount <= maxWords)
                        {
                            countMatch = match;
                            _data = null;
                        }

                        if (match >= minMatch && wordsCount <= maxWords && (perferctMatch == false || (perferctMatch == true && match == _wordsCount && match == wordsCount)))
                        {
                            if (match == _wordsCount && match == wordsCount) perferctMatch = true;
                            if (_data == null) _data = new List<WebSpeechDto>();
                            _data.Add(item);
                            _phraseMatched = phrase;
                        }
                    }
                }
            }

            if (_data != null)
            {
                int x = rnd.Next(0, _data.Count());

                data = _data[x];

                data = GetAnswer(data);
            }

            result.Data = data;
            result.PhraseMatched = _phraseMatched;

            return result;
        }

        /// <summary>
        /// GetAnswer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public WebSpeechDto GetAnswer(WebSpeechDto data)
        {
            var rnd = new Random();
            var answers = new List<string>() { };

            try
            {
                answers = JsonConvert.DeserializeObject<List<string>>(data.Answer);
            }
            catch (Exception)
            {
                answers.Add(data.Answer);
            }

            var x = rnd.Next(0, answers.Count());

            data.Answer = answers[x];

            if (data.Answer == null) data.Answer = "";

            return data;
        }

        /// <summary>
        /// GetMeteoPhrase
        /// </summary>
        /// <param name="request"></param>
        /// <param name="param"></param>
        /// <param name="culture"></param>
        /// <param name="descriptionActive"></param>
        /// <returns></returns>
        public string GetMeteoPhrase(string request, string param, string culture, bool descriptionActive)
        {
            var result = "";
            var getMeteoResult = GetMeteo(param).GetAwaiter().GetResult();

            if (getMeteoResult.Error == null)
            {
                dynamic partOfTheDay = PartsOfTheDayIta.NotSet;
                var day = Days.Oggi;

                if (request.Contains(PartsOfTheDayIta.Mattina.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Mattina;
                if (request.Contains(PartsOfTheDayIta2.Mattino.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta2.Mattino;
                if (request.Contains(PartsOfTheDayIta.Pomerriggio.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Pomerriggio;
                if (request.Contains(PartsOfTheDayIta.Sera.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Sera;
                if (request.Contains(PartsOfTheDayIta.Notte.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Notte;

                if (request.Contains(PartsOfTheDayEng.Morning.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Morning;
                if (request.Contains(PartsOfTheDayEng.Afternoon.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Afternoon;
                if (request.Contains(PartsOfTheDayEng.Evening.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Evening;
                if (request.Contains(PartsOfTheDayEng.Night.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Night;

                if (param.Contains(Days.Oggi.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Today;
                if (param.Contains(Days.Domani.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Tomorrow;

                if (param.Contains(Days.Today.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Today;
                if (param.Contains(Days.Tomorrow.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Tomorrow;

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

            if (descriptionActive && day == Days.Tomorrow) description = src["data"]["weatherReportTomorrow"]["description"].ToString();
            if (descriptionActive && (day == Days.Today || description == "" || description == " ")) description = src["data"]["weatherReportToday"]["description"].ToString();

            description = description.Replace("-", " ");
            description = description.Replace(System.Environment.NewLine, " ");

            if (partOfTheDay.ToString() != PartsOfTheDayIta.NotSet.ToString()) hour = (int)partOfTheDay;

            details = src["data"]["hours"][hour];

            if (culture.Trim().ToLower() == "it-it")
            {
                result = " Ecco le previsioni: ";

                result += description;

                result += " Temperatura " + details["temperature"].ToString().Replace(",", " e ") + " gradi";

                result += ", umidità " + details["umidity"].ToString().Replace(",", " e ") + " percento";

                result += " e vento " + details["windIntensity"].ToString().Replace(",", " e ") + " chilometri orari.";
            }

            if (culture.Trim().ToLower() == "en-us")
            {
                result = " Here are the forecasts: ";

                result += description;

                result += " Temperature " + details["temperature"].ToString().Replace(",", " and ") + " degrees";

                result += ", umidity " + details["umidity"].ToString().Replace(",", " and ") + " percent";

                result += " and wind " + details["windIntensity"].ToString().Replace(",", " and ") + " kilometers per hour.";
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
        /// GetMeteo
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public async Task<(JObject Data, string Error)> GetMeteo(string _value)
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
                    var utility = new Utility();

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
        /// ExecutionFinished
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_hostSelected"></param>
        /// <param name="_application"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task ExecutionFinished(long _id, string _hostSelected, bool _application, HttpRequest request)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (_id == 0 && _application)
                {
                    try
                    {
                        string access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var executionQueue = new ExecutionQueueDto() { FullPath = "*", Arguments = "*", Host = _hostSelected, Type = "ForceHideApplication", StateQueue = ExecutionQueueStateQueue.RunningStep2.ToString() };
                        var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }

                if (_id != 0)
                {
                    try
                    {
                        string access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var getExecutionQueuesByIdResult = await executionQueueRepo.GetExecutionQueuesById(_id, access_token_cookie);
                        var executionQueue = getExecutionQueuesByIdResult.Data.FirstOrDefault();
                        executionQueue.StateQueue = ExecutionQueueStateQueue.RunningStep2.ToString();
                        var addExecutionQueueResult = await executionQueueRepo.UpdateExecutionQueue(executionQueue, access_token_cookie);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }
            }
        }
    }
}
