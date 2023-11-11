using Additional;
using Additional.NLog;
using GoogleCalendar;
using GoogleManagerModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Supp.Site.Common;
using Supp.Models;
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
        private readonly Dialogue dialogue;
        private readonly Utility utility;
        private readonly PhraseInDateTimeManager phraseInDateTimeManager;

        public Common()
        {
            suppUtility = new SuppUtility();
            webSpeecheRepo = new WebSpeechesRepository();
            executionQueueRepo = new ExecutionQueuesRepository();
            dialogue = new Dialogue();
            utility = new Utility();
            phraseInDateTimeManager = new PhraseInDateTimeManager();
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <param name="data"></param>
        /// <param name="loadPage"></param>
        /// <returns></returns>
        public (List<WebSpeechDto> Data, List<ShortcutDto> Shortcuts) GetData(List<WebSpeechDto> data, bool loadPage, long userId)
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

                //if (item.WebSpeechIds != null && item.WebSpeechIds.Count() > 0)
                //{
                //    foreach (var id in item.WebSpeechIds.OrderBy(_ => _).ToList())
                //    {
                //        var _phrase = data.Where(_ => _.Id == id).Select(_ => _.Phrase).FirstOrDefault();
                //        if (item.PreviousPhrase == null) item.PreviousPhrase = String.Empty;
                //        if (item.PreviousPhrase != "") item.PreviousPhrase += " -> ";
                //        item.PreviousPhrase += "Id:" + id.ToString() + " - Phrase:" + _phrase;
                //    }
                //}

                if (loadPage && (item.UserId == 0 || item.UserId == userId) )
                {
                    //if (!item.Type.ToLower().Contains("system"))
                    result.Shortcuts.Add(new ShortcutDto() { Id = item.Id, Type = item.Type, Order = item.Order, Title = item.Name.Replace("_", " "), Action = item.Operation.ToStringExtended().Replace("\\", "/") + " " + item.Parameters.ToStringExtended().Replace("\\", "/"), Ico = item.Ico, Groupable = item.Groupable, GroupName = item.GroupName, GroupOrder = item.GroupOrder, HotShortcut = item.HotShortcut });
                }

                result.Data.Add(item);
            }

            return result;
        }

        private List<long> GetParentIds(string parentIds)
        {
            var _parentIds = new List<long>() { };

            try
            {
                if (parentIds != null && parentIds != "" && parentIds != "null")
                    _parentIds = JsonConvert.DeserializeObject<List<long>>(parentIds);
            }
            catch (Exception)
            {
                try
                {
                    if (parentIds != null && parentIds != "" && parentIds != "null")
                        _parentIds.Add(long.Parse(parentIds));
                }
                catch (Exception)
                {
                }
            }

            return _parentIds;
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
        /// <param name="identification"></param>
        /// <param name="_onlyRefresh"></param>
        /// <param name="_subType"></param>
        /// <param name="_step"></param>
        /// <param name="expiresInSeconds"></param>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <param name="_param"></param>
        /// <param name="webSpeechResult"></param>
        /// <param name="loadPage"></param>
        /// <param name="_keysMatched"></param>
        /// <returns></returns>
        public async Task<WebSpeechDto> GetWebSpeechDto(string _phrase, string _hostSelected, bool _reset, bool _application, long _executionQueueId, bool _alwaysShow, long _id, TokenDto identification, bool _onlyRefresh, string _subType, int _step, int expiresInSeconds, HttpResponse response, HttpRequest request, string _param, WebSpeechResult webSpeechResult, bool loadPage, string _keysMatched)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                WebSpeechDto data = null;
                
                try
                {
                    if (_phrase != null && _phrase != String.Empty)
                    {
                        _phrase = _phrase.Replace("'", " ");

                        var percentage = "";
                        if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") percentage = " percento";
                        if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") percentage = " percent";
                        _phrase = _phrase.Replace("%", percentage);
                    }

                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    List<ShortcutDto> shortcuts = new List<ShortcutDto>() { };
                    List<ShortcutDto> shortcutGroups= new List<ShortcutDto>() { };
                    var startAnswer = "";

                    var access_token_cookie = suppUtility.GetAccessToken(request);
                    var userName = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                    var userId = long.Parse(suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName).ToString());

                    if(webSpeechResult != null && webSpeechResult.Data != null)
                    {
                        var lastWebSpeechId = webSpeechResult.Data.Select(_ => _.Id).OrderByDescending(_ => _).FirstOrDefault();

                        var getDataResult = GetData(webSpeechResult.Data, loadPage, userId);

                        webSpeechResult.Data = getDataResult.Data;
                        if (loadPage)
                        {
                            shortcuts = getDataResult.Shortcuts.OrderByDescending(_ => _.Order).ThenBy(_ => _.Title).ToList();

                            var _shortcutsGrouped = getDataResult.Shortcuts.Where(_ => _.Groupable == true && _.GroupName != null && _.GroupName?.Trim() != "").OrderByDescending(_ => _.GroupOrder).ThenBy(_ => _.GroupName).GroupBy(_ => _.GroupName).ToList();

                            foreach (var _shortcut in _shortcutsGrouped)
                            {
                                shortcutGroups.Add(_shortcut.FirstOrDefault());
                            }
                        }

                        if (_id == 0 && _phrase != "" && _phrase != null && (_subType == "" || _subType == null || _subType == "null") && _step == 0)
                        {
                            var wakeUpScreenAfterEhiResult = WakeUpScreenAfterEhi(_application, identification, access_token_cookie);

                            var matchPhraseResult = MatchPhrase(_phrase, webSpeechResult.Data, identification);
                            data = matchPhraseResult.Data;
                            _keysMatched = matchPhraseResult.WebSpeechKeysMatched;
                        }
                        else if (_id != 0 && (_subType == "" || _subType == null || _subType == "null") && _step == 0)
                        {
                            data = webSpeechResult.Data.Where(_ => _.Id == _id).FirstOrDefault();
                            if (data != null) data = GetAnswer(data, identification);
                        }
                        else if (_id != 0 && _subType != "" && _subType != null && _subType != "null" && _step > 0)
                        {
                            var wakeUpScreenAfterEhiResult = WakeUpScreenAfterEhi(_application, identification, access_token_cookie);

                            if (_subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString())
                            {
                                var requests = dialogue.GetDialogueRequestNotImplemented(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueAddToNote.ToString() || _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString() || _subType == WebSpeechTypes.DialogueAddToNoteWithName.ToString())
                            {
                                var requests = dialogue.GetDialogueAddToNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueCreateNote.ToString() || _subType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString())
                            {
                                var requests = dialogue.GetDialogueCreateNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueDeleteNote.ToString() || _subType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString())
                            {
                                var requests = dialogue.GetDialogueDeleteNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueSetTimer.ToString() || _subType == WebSpeechTypes.SystemDialogueSetAlarmClock.ToString())
                            {
                                var requests = dialogue.GetDialogueSetTimer(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueClearNote.ToString() || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString() || _subType == WebSpeechTypes.DialogueClearNoteWithName.ToString())
                            {
                                var requests = dialogue.GetDialogueClearNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueCreateExtendedReminder.ToString())
                            {
                                var requests = dialogue.GetDialogueCreateExtendedReminder(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType, request);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueCreateReminder.ToString())
                            {
                                var requests = dialogue.GetDialogueCreateReminder(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType, request);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueDeleteReminder.ToString())
                            {
                                var requests = dialogue.GetDialogueDeleteReminder(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueWebSearch.ToString())
                            {
                                var requests = dialogue.GetDialogueWebSearch(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueRunExe.ToString())
                            {
                                var requests = dialogue.GetDialogueRunExe(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            if (_subType == WebSpeechTypes.SystemDialogueRunMediaAndPlay.ToString())
                            {
                                var requests = dialogue.GetDialogueRunMediaAndPlay(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, _subType);
                                if (requests != null && requests.Count > 0)
                                {
                                    var dataResult = GetData(requests, loadPage, userId);
                                    webSpeechResult.Data.AddRange(dataResult.Data);
                                }
                            }

                            var stepType = "";

                            var previousWebSpeech = webSpeechResult.Data.Where(_ => _.Id == _id).FirstOrDefault();
                            if (previousWebSpeech != null)
                                stepType = previousWebSpeech.StepType;

                            var items = webSpeechResult.Data.Where(_ => GetParentIds(_.ParentIds).Contains(_id) && _.Step == (_step + 1)).ToList();
                            (WebSpeechDto Data, string WebSpeechKeysMatched) matchPhraseResult;
                            matchPhraseResult.Data = null;
                            matchPhraseResult.WebSpeechKeysMatched = null;

                            if (items != null)
                            {
                                matchPhraseResult = MatchPhrase(_phrase, items, identification);
                                if (matchPhraseResult.Data != null)
                                {
                                    if (matchPhraseResult.Data.StepType == StepTypes.Default.ToString() && matchPhraseResult.Data.FinalStep == false) data = webSpeechResult.Data.Where(_ => GetParentIds(_.ParentIds).Contains(matchPhraseResult.Data.Id)).FirstOrDefault();
                                    else data = matchPhraseResult.Data;

                                    if (data != null)
                                    {
                                        data = GetAnswer(data, identification);
                                        stepType = data.StepType;
                                    }
                                }
                            }

                            if (data == null && items != null && items.Count == 1 && stepType != StepTypes.Choice.ToString())
                            {
                                data = items.FirstOrDefault();
                                data = GetAnswer(data, identification);
                                stepType = data.StepType;
                            }

                            if (data != null && (stepType == StepTypes.GetElementDateTime.ToString() || stepType == StepTypes.GetElementDate.ToString() || stepType == StepTypes.GetElementTime.ToString()))
                            {
                                if (data.Elements == null)
                                {
                                    data.Elements = new Element[2];
                                    data.Elements[0] = new Element() { Value = String.Empty };
                                }
                                if (_phrase == null) _phrase = "";
                                data.Elements[0].Value = _phrase.Trim();
                                var value = phraseInDateTimeManager.Convert(_phrase, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture);

                                if (value == null) data = null;
                                else _phrase = value.ToString();
                            }

                            if (data != null && data.StepType == StepTypes.GoToFirstStep.ToString())
                            {
                                data = webSpeechResult.Data.Where(_ => _.Step == 1 && _.SubType == data.SubType).FirstOrDefault();
                                data = GetAnswer(data, identification);
                                stepType = data.StepType;
                            }

                            if (data != null && _subType != "" && _subType != null && _subType != "null") data = dialogue.Manage(data, _subType, _step, stepType, expiresInSeconds, _phrase, response, request, identification, userName, userId, _hostSelected);

                            if (data == null && _subType != "" && _subType != null && _subType != "null")
                            {
                                data = previousWebSpeech;
                                if (data != null)
                                {
                                    data = GetAnswer(data, identification);

                                    if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it")
                                        data.Answer = "Non ho capito!" + " " + data.Answer;

                                    if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us")
                                        data.Answer = "I did not understand!" + " " + data.Answer;
                                }
                            }
                        }

                        if (data != null && (data.Type == WebSpeechTypes.ReadRemindersToday.ToString() || data.Type == WebSpeechTypes.ReadRemindersTomorrow.ToString()))
                        {
                            var timeMin = DateTime.Now;
                            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

                            var webSpeechTypes = WebSpeechTypes.ReadRemindersToday;
                            if (data.Type == WebSpeechTypes.ReadRemindersToday.ToString()) webSpeechTypes = WebSpeechTypes.ReadRemindersToday;
                            if (data.Type == WebSpeechTypes.ReadRemindersTomorrow.ToString()) webSpeechTypes = WebSpeechTypes.ReadRemindersTomorrow;

                            if (webSpeechTypes == WebSpeechTypes.ReadRemindersTomorrow)
                            {
                                timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(1);
                                timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59").AddDays(1);
                            }

                            var getRemindersResult = await webSpeecheRepo.GetReminders(access_token_cookie, userName, userId, timeMin, timeMax, webSpeechTypes);

                            var answer = "";

                            if (getRemindersResult.Successful && getRemindersResult.Data.Count > 0)
                            {
                                var reminders = ReadReminders(identification, getRemindersResult.Data, data.Type);
                                if (answer != "") answer += " ";
                                answer += reminders;
                            }

                            if (getRemindersResult.Successful)
                            {
                                if (answer != "") answer += " ";
                                answer += await GetHolidays(identification, access_token_cookie, userName, userId, data.Type);
                            }

                            if (getRemindersResult.Successful && answer == "")
                            {
                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") answer += " Vuoto.";
                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") answer += " Empty.";
                            }

                            if (!getRemindersResult.Successful)
                            {
                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") answer += " Attenzione! probabilmente il token google è scaduto.";
                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") answer += " Attention! probably the google token has expired.";
                            }

                            data.Answer = answer;
                        }

                        if (data != null && data.Type == WebSpeechTypes.ReadNotes.ToString())
                        {
                            var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

                            var webSpeechTypes = WebSpeechTypes.ReadNotes;

                            var getRemindersResult = await webSpeecheRepo.GetReminders(access_token_cookie, userName, userId, timeMin, timeMax, webSpeechTypes, data.Parameters);

                            var answer = "";

                            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") answer = "";
                            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") answer = "";

                            if (getRemindersResult.Successful && getRemindersResult.Data.Count > 0)
                            {
                                foreach (var item in getRemindersResult.Data)
                                {
                                    if (answer != "") answer += " ";
                                    answer += item.Summary.Replace("#Note ", "") + ",";
                                    if (answer != "") answer += " ";

                                    if (item.Description != String.Empty && item.Description != null)
                                    {
                                        answer += item.Description.Replace("\n", ", ") + ".";
                                    }
                                    else
                                    {
                                        if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") answer += "vuota!.";
                                        if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") answer += "empty!.";
                                    }
                                }
                            }

                            if (!getRemindersResult.Successful)
                            {
                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") answer += " Attenzione! probabilmente il token google è scaduto.";
                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") answer += " Attention! probably the google token has expired.";
                            }

                            data.Answer = answer;
                        }

                        if (
                                data != null
                                && (
                                    data.Type == WebSpeechTypes.SystemRunExe.ToString()
                                    || data.Type == WebSpeechTypes.RunExe.ToString()
                                    || data.Type == WebSpeechTypes.SystemRunExeWithNotNumericParameter.ToString()
                                    || data.Type == WebSpeechTypes.RunExeWithNotNumericParameter.ToString()
                                    || data.Type == WebSpeechTypes.SystemRunExeWithNumericParameter.ToString()
                                    || data.Type == WebSpeechTypes.RunExeWithNumericParameter.ToString()
                                    )
                                && data.OperationEnable == true && data.Step == 0
                            )
                        {
                            var phrase = GetValueFromPronouncedPhrase(_phrase, _keysMatched).Trim();

                            if (data.SubType == WebSpeechTypes.SystemDialogueRunExe.ToString() && (phrase == null || phrase == String.Empty))
                            {
                                List<WebSpeechDto> dialogue = null;

                                if (data.SubType == WebSpeechTypes.SystemDialogueRunExe.ToString())
                                    dialogue = this.dialogue.GetDialogueRunExe(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                                if (dialogue != null && dialogue.Count > 0)
                                {
                                    var dataResult = GetData(dialogue, loadPage, userId);

                                    var _data = dataResult.Data.OrderBy(_ => _.Id).FirstOrDefault();

                                    _data = GetAnswer(_data, identification);

                                    _data.Parameters = data.Parameters;
                                    _data.Operation = data.Operation;
                                    _data.Type = data.Type;

                                    data = this.dialogue.Manage(_data, _data.SubType, 0, _data.StepType, expiresInSeconds, phrase, response, request, identification, userName, userId, _hostSelected);
                                }
                            }
                            else
                            {
                                data = await dialogue.RunExe(data, phrase, _hostSelected, access_token_cookie, executionQueueRepo, identification);

                                if (data.ExecutionQueueId != 0)
                                {
                                    _executionQueueId = data.ExecutionQueueId;
                                }
                            }
                        }

                        if (
                                data != null
                                && (
                                    data.Type == WebSpeechTypes.SystemRunMediaAndPlay.ToString()
                                    || data.Type == WebSpeechTypes.RunMediaAndPlay.ToString()
                                    || data.Type == WebSpeechTypes.SystemRunMediaAndPlayWithNotNumericParameter.ToString()
                                    || data.Type == WebSpeechTypes.RunMediaAndPlayWithNotNumericParameter.ToString()
                                    || data.Type == WebSpeechTypes.SystemRunMediaAndPlayWithNumericParameter.ToString()
                                    || data.Type == WebSpeechTypes.RunMediaAndPlayWithNumericParameter.ToString()
                                    )
                                && data.OperationEnable == true && data.Step == 0
                            )
                        {
                            var phrase = GetValueFromPronouncedPhrase(_phrase, _keysMatched).Trim();

                            if (data.SubType == WebSpeechTypes.SystemDialogueRunMediaAndPlay.ToString() && (phrase == null || phrase == String.Empty))
                            {
                                List<WebSpeechDto> dialogue = null;

                                if (data.SubType == WebSpeechTypes.SystemDialogueRunMediaAndPlay.ToString())
                                    dialogue = this.dialogue.GetDialogueRunMediaAndPlay(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                                if (dialogue != null && dialogue.Count > 0)
                                {
                                    var dataResult = GetData(dialogue, loadPage, userId);

                                    var _data = dataResult.Data.OrderBy(_ => _.Id).FirstOrDefault();

                                    _data = GetAnswer(_data, identification);

                                    _data.Parameters = data.Parameters;
                                    _data.Operation = data.Operation;
                                    _data.Type = data.Type;

                                    data = this.dialogue.Manage(_data, _data.SubType, 0, _data.StepType, expiresInSeconds, phrase, response, request, identification, userName, userId, _hostSelected);
                                }
                            }
                            else
                            {
                                data = await dialogue.RunMediaAndPlay(data, phrase, _hostSelected, access_token_cookie, executionQueueRepo, identification);

                                if (data.ExecutionQueueId != 0)
                                {
                                    _executionQueueId = data.ExecutionQueueId;
                                }
                            }
                        }

                        if (
                                data != null
                                && (
                                    data.Type == WebSpeechTypes.SystemSetTimer.ToString()
                                    || data.Type == WebSpeechTypes.SetTimer.ToString()
                                    || data.Type == WebSpeechTypes.SystemSetAlarmClock.ToString()
                                    || data.Type == WebSpeechTypes.SetAlarmClock.ToString()
                                    )
                                && data.OperationEnable == true && data.Step == 0
                            )
                        {
                            if (_phrase == null) _phrase = "";
                            var date = phraseInDateTimeManager.Convert(_phrase, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture);

                            data.Parameters = data.Parameters.Replace("NAME", identification.Name);

                            if ((data.SubType == WebSpeechTypes.SystemDialogueSetTimer.ToString() || data.SubType == WebSpeechTypes.SystemDialogueSetAlarmClock.ToString()) && (date == null))
                            {
                                List<WebSpeechDto> dialogue = null;

                                if ((data.SubType == WebSpeechTypes.SystemDialogueSetTimer.ToString() || data.SubType == WebSpeechTypes.SystemDialogueSetAlarmClock.ToString()))
                                    dialogue = this.dialogue.GetDialogueSetTimer(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                                if (dialogue != null && dialogue.Count > 0)
                                {
                                    var dataResult = GetData(dialogue, loadPage, userId);

                                    var _data = dataResult.Data.OrderBy(_ => _.Id).FirstOrDefault();

                                    _data = GetAnswer(_data, identification);

                                    _data.Parameters = data.Parameters;
                                    _data.Operation = data.Operation;
                                    _data.Type = data.Type;

                                    data = this.dialogue.Manage(_data, _data.SubType, 0, _data.StepType, expiresInSeconds, date.ToString(), response, request, identification, userName, userId, _hostSelected);
                                }
                            }
                            else
                            {
                                data = await dialogue.SetTimer(data, access_token_cookie, userName, userId, identification, request, response, expiresInSeconds, (DateTime)date);
                            }
                        }

                        if (data != null && data.Type == WebSpeechTypes.Meteo.ToString())
                        {
                            data.Answer = GetMeteoPhrase(data.Phrase, data.Parameters, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower(), true);
                        }

                        if (data != null && data.Type == WebSpeechTypes.Time.ToString())
                        {
                            var now = DateTime.Now;

                            var dayofweek = now.ToString("dddd", new CultureInfo(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture));
                            var month = now.ToString("MMMM", new CultureInfo(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture));

                            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it")
                            {
                                data.Answer = now.Hour.ToString();

                                if (now.Minute == 1) data.Answer += " e " + now.Minute.ToString() + " minuto, ";
                                else if (now.Minute > 1) data.Answer += " e " + now.Minute.ToString() + " minuti, ";
                                else data.Answer += ", ";

                                data.Answer += dayofweek + " " + now.Day.ToString() + " " + month;
                            }

                            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us")
                            {
                                data.Answer = now.Hour.ToString();

                                if (now.Minute == 1) data.Answer += " and " + now.Minute.ToString() + " minute, ";
                                else if (now.Minute > 1) data.Answer += " and " + now.Minute.ToString() + " minutes, ";
                                else data.Answer += ", ";

                                data.Answer += dayofweek + " " + now.Day.ToString() + " " + month;
                            }
                        }

                        if (data != null && (data.Type == WebSpeechTypes.SystemWebSearch.ToString() || data.Type == WebSpeechTypes.WebSearch.ToString()) && _step == 0)
                        {
                            var dialogueWebSearch = new DialogueWebSearch();

                            var phrase = GetValueFromPronouncedPhrase(_phrase, _keysMatched).Trim();

                            if (phrase == null || phrase == String.Empty || (data.SubType == WebSpeechTypes.SystemDialogueWebSearch.ToString() && data.FinalStep == false))
                            {
                                List<WebSpeechDto> dialogue = null;

                                if (data.SubType == WebSpeechTypes.SystemDialogueWebSearch.ToString())
                                    dialogue = this.dialogue.GetDialogueWebSearch(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                                if (dialogue != null && dialogue.Count > 0)
                                {
                                    var dataResult = GetData(dialogue, loadPage, userId);

                                    var _data = dataResult.Data.OrderBy(_ => _.Id).FirstOrDefault();

                                    _data = GetAnswer(_data, identification);

                                    _data.Parameters = data.Parameters;
                                    _data.Type = data.Type;

                                    data = this.dialogue.Manage(_data, _data.SubType, _step, _data.StepType, expiresInSeconds, _phrase, response, request, identification, userName, userId, _hostSelected);
                                }
                            }
                            else
                            {
                                var webSearchResult = await dialogueWebSearch.WebSearch(data, phrase);

                                data.Parameters = webSearchResult.Parameters;
                            }
                        }

                        if (data != null &&
                            (
                                data.Type == WebSpeechTypes.EditNote.ToString()
                                || data.Type == WebSpeechTypes.SystemEditNote.ToString()

                                || data.Type == WebSpeechTypes.CreateNote.ToString()
                                || data.Type == WebSpeechTypes.SystemCreateNote.ToString()

                                || data.Type == WebSpeechTypes.DeleteNote.ToString()
                                || data.Type == WebSpeechTypes.SystemDeleteNote.ToString()

                                || data.Type == WebSpeechTypes.DeleteTimer.ToString()
                                || data.Type == WebSpeechTypes.SystemDeleteTimer.ToString()

                                || data.Type == WebSpeechTypes.DeleteAlarmClock.ToString()
                                || data.Type == WebSpeechTypes.SystemDeleteAlarmClock.ToString()
                            )
                             && _step == 0
                        )
                        {
                            List<WebSpeechDto> dialogue = null;

                            if (data.SubType == WebSpeechTypes.SystemDialogueAddToNote.ToString() || data.SubType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString() || data.SubType == WebSpeechTypes.DialogueAddToNoteWithName.ToString())
                                dialogue = this.dialogue.GetDialogueAddToNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                            if (data.SubType == WebSpeechTypes.SystemDialogueClearNote.ToString() || data.SubType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString() || data.SubType == WebSpeechTypes.DialogueClearNoteWithName.ToString())
                                dialogue = this.dialogue.GetDialogueClearNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                            if (data.SubType == WebSpeechTypes.SystemDialogueCreateNote.ToString() || data.SubType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString())
                                dialogue = this.dialogue.GetDialogueCreateNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                            if (
                                    data.SubType == WebSpeechTypes.SystemDialogueDeleteNote.ToString()
                                    || data.SubType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString()

                                    || data.SubType == WebSpeechTypes.SystemDialogueDeleteTimer.ToString()
                                    || data.SubType == WebSpeechTypes.SystemDialogueDeleteAlarmClock.ToString()
                                )
                                dialogue = this.dialogue.GetDialogueDeleteNote(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                            if (dialogue != null && dialogue.Count > 0)
                            {
                                var dataResult = GetData(dialogue, loadPage, userId);

                                var _data = dataResult.Data.OrderBy(_ => _.Id).FirstOrDefault();

                                _data = GetAnswer(_data, identification);

                                _data.Parameters = data.Parameters;
                                if (_param != null && _param != "null" && _param != "") _data.Parameters = _param;
                                if (_data.Answer == null || _data.Answer == "") _data.Answer = data.Answer;
                                //_data.Type = data.Type;

                                data = this.dialogue.Manage(_data, _data.SubType, 0, _data.StepType, expiresInSeconds, _phrase, response, request, identification, userName, userId, _hostSelected);
                            }
                        }

                        if (data != null &&
                            (
                                data.Type == WebSpeechTypes.CreateExtendedReminder.ToString()
                                || data.Type == WebSpeechTypes.SystemCreateExtendedReminder.ToString()

                                || data.Type == WebSpeechTypes.CreateReminder.ToString()
                                || data.Type == WebSpeechTypes.SystemCreateReminder.ToString()

                                || data.Type == WebSpeechTypes.DeleteReminder.ToString()
                                || data.Type == WebSpeechTypes.SystemDeleteReminder.ToString()
                            )
                             && _step == 0
                        )
                        {
                            List<WebSpeechDto> dialogue = null;

                            if (data.SubType == WebSpeechTypes.SystemDialogueCreateExtendedReminder.ToString())
                                dialogue = this.dialogue.GetDialogueCreateExtendedReminder(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType, request);

                            if (data.SubType == WebSpeechTypes.SystemDialogueCreateReminder.ToString())
                                dialogue = this.dialogue.GetDialogueCreateReminder(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType, request);

                            if (data.SubType == WebSpeechTypes.SystemDialogueDeleteReminder.ToString())
                                dialogue = this.dialogue.GetDialogueDeleteReminder(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId, data.SubType);

                            if (dialogue != null && dialogue.Count > 0)
                            {
                                var dataResult = GetData(dialogue, loadPage, userId);

                                var _data = dataResult.Data.OrderBy(_ => _.Id).FirstOrDefault();

                                _data = GetAnswer(_data, identification);

                                _data.Parameters = data.Parameters;
                                //_data.Type = data.Type;

                                data = this.dialogue.Manage(_data, _data.SubType, 0, _data.StepType, expiresInSeconds, _phrase, response, request, identification, userName, userId, _hostSelected);
                            }
                        }

                        if (data != null && data.Type == WebSpeechTypes.MediaPlayOrPause.ToString())
                        {
                            await AddExecutionQueueQuickly(_hostSelected, request, ExecutionQueueType.MediaPlayOrPause);
                        }

                        if (data != null && data.Type == WebSpeechTypes.MediaNextTrack.ToString())
                        {
                            await AddExecutionQueueQuickly(_hostSelected, request, ExecutionQueueType.MediaNextTrack);
                        }

                        if (data != null && data.Type == WebSpeechTypes.MediaPreviousTrack.ToString())
                        {
                            await AddExecutionQueueQuickly(_hostSelected, request, ExecutionQueueType.MediaPreviousTrack);
                        }

                        var salutation = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.Salutation;
                        if (identification.Name == null && JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") identification.Name = "tu";
                        if (identification.Name == null && JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") identification.Name = "you";
                        if (identification.Surname == null) identification.Surname = String.Empty;
                        salutation = salutation.Replace("NAME", identification.Name);
                        salutation = salutation.Replace("SURNAME", identification.Surname);

                        startAnswer = salutation + " " + SuppUtility.GetSalutation(new CultureInfo(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, false));

                        if (DateTime.Now.Hour == 3) startAnswer = "";

                        if ((_phrase == null || _phrase == "") && data == null && _reset == false && _onlyRefresh == false && (_subType == null || _subType == ""))
                        {
                            data = new WebSpeechDto() { Answer = startAnswer, Ehi = 0, FinalStep = true };

                            var now = DateTime.Now;

                            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.MeteoParameterToTheSalutation != null && JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.MeteoParameterToTheSalutation != "" && _application == true && SuppUtility.GetPartOfTheDay(now) == PartsOfTheDayEng.Morning)
                            {
                                data.Answer += GetMeteoPhrase(String.Empty, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.MeteoParameterToTheSalutation, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower(), JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.DescriptionMeteoToTheSalutationActive);

                                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.RemindersActive)
                                {
                                    var timeMin = DateTime.Now;
                                    var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

                                    var getRemindersResult = await webSpeecheRepo.GetReminders(access_token_cookie, userName, userId, timeMin, timeMax, WebSpeechTypes.ReadRemindersToday);

                                    if (getRemindersResult.Successful && getRemindersResult.Data.Count > 0)
                                    {
                                        var reminders = ReadReminders(identification, getRemindersResult.Data, WebSpeechTypes.ReadRemindersToday.ToString());

                                        data.Answer += reminders;
                                    }

                                    if (getRemindersResult.Successful)
                                    {
                                        if (data.Answer != "") data.Answer += " ";
                                        data.Answer += await GetHolidays(identification, access_token_cookie, userName, userId, WebSpeechTypes.ReadRemindersToday.ToString());

                                        if (data.Answer != "") data.Answer += " ";
                                        data.Answer += await GetHolidays(identification, access_token_cookie, userName, userId, WebSpeechTypes.ReadRemindersTomorrow.ToString());
                                    }

                                    if (!getRemindersResult.Successful)
                                    {
                                        if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") data.Answer += " Attenzione! probabilmente il token google è scaduto.";
                                        if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") data.Answer += " Attention! probably the google token has expired.";
                                    }
                                }
                            }
                        }

                        if (
                            (_phrase != null && _phrase != "" && data == null && webSpeechResult != null)
                            || (data == null && _subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString())
                        )
                        {
                            if (_subType == null || _subType == "") _subType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString();

                            var dialogueRequestNotImplemented = dialogue.GetDialogueRequestNotImplemented(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture, lastWebSpeechId);
                            if (dialogueRequestNotImplemented != null && dialogueRequestNotImplemented.Count > 0)
                            {
                                var dataResult = GetData(dialogueRequestNotImplemented, loadPage, userId);
                                data = dataResult.Data.Where(_ => _.Step == 1).FirstOrDefault();
                            }

                            data = GetAnswer(data, identification);

                            data = dialogue.Manage(data, WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(), _step, StepTypes.Default.ToString(), expiresInSeconds, _phrase, response, request, identification, userName, userId, _hostSelected);
                        }
                    }

                    if (data == null) data = new WebSpeechDto() { Answer = "", Ehi = 0 };

                    data.HostsArray = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.HostsArray;
                    data.HostSelected = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.HostDefault;
                    data.ListeningWord1 = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.ListeningWord1;
                    data.ListeningWord2 = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.ListeningWord2;
                    data.ListeningAnswer = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.ListeningAnswer;
                    data.Culture = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture;
                    data.StartAnswer = startAnswer;
                    data.Application = _application;
                    data.AlwaysShow = _alwaysShow;
                    data.ExecutionQueueId = _executionQueueId;
                    data.TimeToResetInSeconds = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.TimeToResetInSeconds;
                    data.TimeToEhiTimeoutInSeconds = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.TimeToEhiTimeoutInSeconds;
                    data.OnlyRefresh = _onlyRefresh;
                    data.LogJSActive = data.LogJSActive = GeneralSettings.Static.LogJSActive;
                    data.UserId = identification.UserId;

                    if ((_phrase != null && _phrase != "") && (data.FinalStep == false || _phrase == (data.ListeningWord1 + " " + data.ListeningWord2).Trim().ToLower())) data.Ehi = 1;

                    if (_reset == true && _alwaysShow == false)
                    {
                        if (_hostSelected == null || _hostSelected == String.Empty) _hostSelected = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.HostDefault;
                        await ExecutionFinished(_executionQueueId, _hostSelected, _application, request);
                    }

                    if (identification != null && loadPage)
                    {
                        var _identification = SuppUtility.Clone(identification);

                        _identification.RolesInJson = _identification.RolesInJson.Replace(((char)34).ToString(), @"\" + ((char)34));

                        _identification.ConfigInJson = _identification.ConfigInJson.Replace(((char)34).ToString(), @"\" + ((char)34));

                        _identification.ConfigInJson = _identification.ConfigInJson.Replace(@"\\", @"\");

                        data.IdentificationInJson = JsonConvert.SerializeObject(_identification);
                    }

                    if (webSpeechResult != null && webSpeechResult.Data != null && loadPage)
                    {
                        var webSpeeches = SuppUtility.Clone(webSpeechResult.Data);

                        foreach (var item in webSpeeches)
                        {
                            if (item.Phrase != null && item.Phrase.Contains(@"\\")) item.Phrase = item.Phrase.Replace(@"\\", @"\");
                            if (item.Answer != null && item.Answer.Contains(@"\\")) item.Phrase = item.Phrase.Replace(@"\\", @"\");
                            if (item.HostsArray != null && item.Answer.Contains(@"\\")) item.HostsArray = item.HostsArray.Replace(@"\\", @"\");

                            if (item.Phrase != null && !item.Phrase.Contains(@"\")) item.Phrase = item.Phrase.Replace(((char)34).ToString(), @"\" + ((char)34).ToString());
                            if (item.Operation != null && !item.Operation.Contains(@"\\")) item.Operation = item.Operation.Replace(@"\", @"\\");
                            if (item.Answer != null && !item.Answer.Contains(@"\")) item.Answer = item.Answer.Replace(((char)34).ToString(), @"\" + ((char)34).ToString());
                            if (item.Parameters != null && !item.Parameters.Contains(@"\\")) item.Parameters = item.Parameters.Replace(@"\", @"\\");
                            if (item.HostsArray != null && !item.HostsArray.Contains(@"\")) item.HostsArray = item.HostsArray.Replace(((char)34).ToString(), @"\" + ((char)34).ToString());
                        }
                        
                        data.WebSpeechesInJson = JsonConvert.SerializeObject(webSpeeches);
                    }

                    if (loadPage)
                    {
                        var shortcutsInJson = JsonConvert.SerializeObject(shortcuts);

                        data.ShortcutsInJson = shortcutsInJson;

                        var shortcutGroupsInJson = JsonConvert.SerializeObject(shortcutGroups);

                        data.ShortcutGroupsInJson = shortcutGroupsInJson;
                    }

                    data.Error = null;

                    //logger.Info("data:" + JsonConvert.SerializeObject(data));

                    if (data.Phrase != null && data.Phrase.Contains(@"\\")) data.Phrase = data.Phrase.Replace(@"\\", @"\");
                    if (data.Answer != null && data.Answer.Contains(@"\\")) data.Phrase = data.Phrase.Replace(@"\\", @"\");
                    if (data.HostsArray != null && data.Answer.Contains(@"\\")) data.HostsArray = data.HostsArray.Replace(@"\\", @"\");

                    if (data.Phrase != null && !data.Phrase.Contains(@"\")) data.Phrase = data.Phrase.Replace(((char)34).ToString(), @"\" + ((char)34).ToString());
                    if (data.Operation != null && !data.Operation.Contains(@"\\")) data.Operation = data.Operation.Replace(@"\", @"\\");
                    if (data.Answer != null && !data.Answer.Contains(@"\")) data.Answer = data.Answer.Replace(((char)34).ToString(), @"\" + ((char)34).ToString());
                    if (data.Parameters != null && !data.Parameters.Contains(@"\\")) data.Parameters = data.Parameters.Replace(@"\", @"\\");
                    if (data.HostsArray != null && !data.HostsArray.Contains(@"\")) data.HostsArray = data.HostsArray.Replace(((char)34).ToString(), @"\" + ((char)34).ToString());

                    return data;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    throw ex;
                }
            }
        }

        private async Task<string> GetHolidays(TokenDto identification, string access_token_cookie, string userName, long userId, string type)
        {
            var result = "";
            var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            if (type == WebSpeechTypes.ReadRemindersToday.ToString())
            {
                var getHolidaysTodayResult = await webSpeecheRepo.GetHolidays(access_token_cookie, userName, userId, timeMin, timeMax, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture);

                if (getHolidaysTodayResult.Successful && getHolidaysTodayResult.Data.Count > 0)
                {
                    var holidays = "";
                    if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") holidays = " Le festività di oggi: ";
                    if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") holidays = " Today's holidays: ";

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

                var getHolidaysTomorrowResult = await webSpeecheRepo.GetHolidays(access_token_cookie, userName, userId, timeMin, timeMax, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture);

                if (getHolidaysTomorrowResult.Successful && getHolidaysTomorrowResult.Data.Count > 0)
                {
                    var holidays = "";
                    if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it") holidays = " Le festività di domani: ";
                    if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us") holidays = " Tomorrow's holidays: ";

                    foreach (var item in getHolidaysTomorrowResult.Data)
                    {
                        holidays += item.Summary + ", ";
                    }

                    result += holidays;
                }
            }

            return result;
        }

        private string ReadReminders(TokenDto identification, List<CalendarEvent> data, string type)
        {
            var reminders = "";

            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it" && type == WebSpeechTypes.ReadRemindersToday.ToString()) reminders = "I promemoria di oggi:";
            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us" && type == WebSpeechTypes.ReadRemindersToday.ToString()) reminders = "Today's reminders:";
                                                                              
            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it" && type == WebSpeechTypes.ReadRemindersTomorrow.ToString()) reminders = "I promemoria di domani:";
            if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us" && type == WebSpeechTypes.ReadRemindersTomorrow.ToString()) reminders = "Tomorrow's reminders:";

            foreach (var item in data)
            {
                reminders += item.Summary;

                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "it-it")
                {
                    reminders += " alle " + item.EventDateStart.Value.Hour.ToString();

                    if (item.EventDateStart.Value.Minute == 1) reminders += " e " + item.EventDateStart.Value.Minute.ToString() + " minuto.";
                    else if (item.EventDateStart.Value.Minute > 1) reminders += " e " + item.EventDateStart.Value.Minute.ToString() + " minuti.";
                    else reminders += ".";
                }

                if (JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture.ToLower() == "en-us")
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
        /// Get Value From Pronounced Phrase
        /// </summary>
        /// <param name="pronouncedPhrase"></param>
        /// <param name="phraseFromData"></param>
        /// <returns></returns>
        public string GetValueFromPronouncedPhrase(string pronouncedPhrase, string phraseFromData)
        {
            var phrases = new List<string>() { };

            if (pronouncedPhrase == null) pronouncedPhrase = String.Empty;
            if (phraseFromData == null) phraseFromData = String.Empty;

            var phrase = pronouncedPhrase;

            try
            {
                phrases = JsonConvert.DeserializeObject<List<string>>(phraseFromData);
            }
            catch (Exception)
            {
                try
                {
                    phrases.AddRange(phraseFromData.Split(' '));
                }
                catch (Exception)
                {
                    phrases.Add(phraseFromData);
                }
            }

            if(phrases == null) phrases = new List<string>() { };

            foreach (var item in phrases)
            {
                phrase = phrase.Replace(item, "");
            }

            return phrase;
        }

        /// <summary>
        /// Wake Up Screen After Ehi
        /// </summary>
        /// <param name="_application"></param>
        /// <param name="identification"></param>
        /// <param name="access_token_cookie"></param>
        /// <returns></returns>
        public async Task<ExecutionQueueResult> WakeUpScreenAfterEhi(bool _application, TokenDto identification, string access_token_cookie)
        {
            var response = new ExecutionQueueResult() { Data = new List<ExecutionQueueDto>(), ResultState = Models.ResultType.None, Successful = true };

            if (_application == true && JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.WakeUpScreenAfterEhiActive == true)
            {
                var executionQueue = new ExecutionQueueDto() { Host = JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.HostDefault, Type = ExecutionQueueType.WakeUpScreenAfterEhi.ToString(), WebSpeechId = 0, ScheduledDateTime = DateTime.Now, StateQueue = ExecutionQueueStateQueue.NONE.ToString() };
                response = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);
            }

            return response;
        }

        /// <summary>
        /// Match Phrase
        /// </summary>
        /// <param name="_phrase"></param>
        /// <param name="webSpeechlist"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public (WebSpeechDto Data, string WebSpeechKeysMatched) MatchPhrase(string _phrase, List<WebSpeechDto> webSpeechlist, TokenDto identification)
        {
            WebSpeechDto _data = null;
            (WebSpeechDto Data, string WebSpeechKeysMatched) result;
            result.Data = null;
            result.WebSpeechKeysMatched = null;
            var minMatch = 0;
            var countMatch = 0;
            var phraseMatch = "";
            var previousCountMatch = 0;

            foreach (var item in webSpeechlist)
            {
                try
                {
                    var keysInObjectList = JsonConvert.DeserializeObject<List<object>>(item.Phrase);

                    minMatch = keysInObjectList.Count;
                    countMatch = 0;
                    phraseMatch = "";

                    foreach (var keysInObject in keysInObjectList)
                    {
                        var keysArray = new List<object>();

                        var type = keysInObject.GetType();
                        if (type.Name.Trim().ToLower().Contains("array"))
                        {
                            keysArray = JsonConvert.DeserializeObject<List<object>>(keysInObject.ToString());
                        }
                        else
                        {
                            keysArray.Add(keysInObject.ToString());
                        }

                        var founded = false;
                        foreach (var key in keysArray)
                        {
                            var keys = key.ToString().Trim().ToLower().Split(' ');
                            if (_phrase == null) _phrase = "";
                            if (keys.Count() == 1)
                            {
                                var words = _phrase.Trim().ToLower().Split(' ');

                                if (!founded && words.Contains(key.ToString().Trim().ToLower()))
                                {
                                    founded = true;
                                    countMatch++;
                                    if (phraseMatch != "") phraseMatch += " ";
                                    phraseMatch += key.ToString();
                                }
                            }
                            else
                            {
                                if (!founded)
                                {
                                    var matchPrhareNoKeysResult = MatchPrhareNoKeys(key.ToString().Trim().ToLower(), _phrase, minMatch, countMatch, phraseMatch, previousCountMatch);
                                    minMatch = matchPrhareNoKeysResult.MinMatch;
                                    countMatch = matchPrhareNoKeysResult.CountMatch;
                                    phraseMatch = matchPrhareNoKeysResult.PhraseMatch;
                                    founded = matchPrhareNoKeysResult.Founded;
                                    previousCountMatch = matchPrhareNoKeysResult.PreviousCountMatch;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    var matchPrhareNoKeysResult = MatchPrhareNoKeys(item.Phrase, _phrase, minMatch, countMatch, phraseMatch, previousCountMatch);
                    minMatch = matchPrhareNoKeysResult.MinMatch;
                    countMatch = matchPrhareNoKeysResult.CountMatch;
                    phraseMatch = matchPrhareNoKeysResult.PhraseMatch;
                    previousCountMatch = matchPrhareNoKeysResult.PreviousCountMatch;
                }

                if (countMatch >= minMatch && countMatch > previousCountMatch)
                {
                    previousCountMatch = countMatch;
                    _data = item;
                    result.WebSpeechKeysMatched = phraseMatch;
                    countMatch = 0;
                }
            }

            if (_data != null)
            {
                result.Data = GetAnswer(_data, identification);            
            }

            return result;
        }

        private (int MinMatch, int CountMatch, string PhraseMatch, bool Founded, int PreviousCountMatch) MatchPrhareNoKeys(string itemPhrase, string _phrase, int minMatch, int countMatch, string phraseMatch, int previousCountMatch)
        {
            (int MinMatch, int CountMatch, string PhraseMatch, bool Founded, int PreviousCountMatch) result;
            result.MinMatch = minMatch;
            result.CountMatch = countMatch;
            result.PhraseMatch = phraseMatch;
            result.Founded = false;
            result.PreviousCountMatch = previousCountMatch;

            var _countMatch = 0;
            var _phraseMatch = "";
            var _minMatch = 0;
            var founded = false;

            try
            {
                var keysArray = itemPhrase.Split(' ').ToList();
                _minMatch = keysArray.Count;
                if (_phrase == null) _phrase = "";

                foreach (var key in keysArray)
                {
                    if (_phrase.Trim().ToLower().Contains(key.ToString().Trim().ToLower()))
                    {
                        founded = true;
                        _countMatch++;
                        if (_phraseMatch != "") _phraseMatch += " ";
                        _phraseMatch += key.ToString();
                    }
                }
            }
            catch (Exception)
            {}

            if (_countMatch >= result.MinMatch && _countMatch > result.PreviousCountMatch)
            {
                previousCountMatch = countMatch;
                result.MinMatch = _minMatch;
                result.CountMatch += _countMatch;
                result.PhraseMatch += _phraseMatch;
                result.Founded = founded;
                result.PreviousCountMatch = previousCountMatch;
            }

            return result;
        }

        /// <summary>
        /// GetAnswer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="identification"></param>
        /// <returns></returns>
        public WebSpeechDto GetAnswer(WebSpeechDto data, TokenDto identification)
        {
            data.Answer = suppUtility.GetAnswer(data.Answer, identification);

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
                {}
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

                result += " Temperatura " + temperatureSplit[0];

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

                result += " Temperature " + temperatureSplit[0];

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
                        var access_token_cookie = suppUtility.GetAccessToken(request);

                        var executionQueue = new ExecutionQueueDto() { FullPath = "*", Arguments = "*", Host = _hostSelected, Type = ExecutionQueueType.ForceHideApplication.ToString(), StateQueue = ExecutionQueueStateQueue.RunningStep2.ToString(), WebSpeechId = 0, ScheduledDateTime = DateTime.Now };
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
                        var access_token_cookie = suppUtility.GetAccessToken(request);

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

        /// <summary>
        /// Add Execution Queue Quickly
        /// </summary>
        /// <param name="_hostSelected"></param>
        /// <param name="request"></param>
        /// <param name="executionQueueType"></param>
        /// <returns></returns>
        public async Task AddExecutionQueueQuickly(string _hostSelected, HttpRequest request, ExecutionQueueType executionQueueType)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var access_token_cookie = suppUtility.GetAccessToken(request);

                    var executionQueue = new ExecutionQueueDto() { FullPath = "", Arguments = "", Host = _hostSelected, Type = executionQueueType.ToString(), StateQueue = ExecutionQueueStateQueue.NONE.ToString(), WebSpeechId = 0, ScheduledDateTime = DateTime.Now };
                    var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
            }
        }
    }
}
