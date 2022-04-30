using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Supp.Site.Common;
using Supp.Models;
using Supp.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Supp.Site.Common.Config;
using GoogleManagerModels;
using Additional;
using System.Globalization;

namespace Supp.Site.Recognition
{
    public class Dialogue
    {
        private readonly Utility utility;
        private readonly SuppUtility suppUtility;
        private readonly DialogueAddToNote dialogueAddToNote;
        private readonly DialogueCreateNote dialogueCreateNote;
        private readonly DialogueClearNote dialogueClearNote;
        private readonly DialogueDeleteNote dialogueDeleteNote;
        private readonly DialogueCreateExtendedReminder dialogueCreateExtendedReminder;
        private readonly DialogueCreateReminder dialogueCreateReminder;
        private readonly DialogueDeleteReminder dialogueDeleteReminder;
        private readonly WebSpeechesRepository webSpeecheRepo;
        private readonly ExecutionQueuesRepository executionQueueRepo;
        private readonly DialogueSetTimer dialogueSetTimer;
        private readonly PhraseInDateTimeManager phraseInDateTimeManager;


        public Dialogue()
        {
            suppUtility = new SuppUtility();
            dialogueAddToNote = new DialogueAddToNote();
            dialogueCreateNote = new DialogueCreateNote();
            dialogueDeleteNote = new DialogueDeleteNote();
            dialogueClearNote = new DialogueClearNote();
            dialogueCreateExtendedReminder = new DialogueCreateExtendedReminder();
            dialogueCreateReminder = new DialogueCreateReminder();
            dialogueDeleteReminder = new DialogueDeleteReminder();
            webSpeecheRepo = new WebSpeechesRepository();
            executionQueueRepo = new ExecutionQueuesRepository();
            utility = new Utility();
            dialogueSetTimer = new DialogueSetTimer();
            phraseInDateTimeManager = new PhraseInDateTimeManager();
        }

        private Element[] InitElements(Element[] elements, int index)
        {
            Element[] result;

            if (elements == null || elements.Length == 0)
            {
                result = new Element[2];
                elements = result;
            }
            else
            {
                result = new Element[elements.Length + 2];
                for (int i = 0; i < elements.Length; i++)
                {
                    result[i] = elements[i];
                }
            }

            if(result[index] == null) 
                result[index] = new Element() { Value = String.Empty };

            return result;
        }

        /// <summary>
        /// Manage
        /// </summary>
        /// <param name="data"></param>
        /// <param name="_subType"></param>
        /// <param name="_step"></param>
        /// <param name="_stepType"></param>
        /// <param name="expiresInSeconds"></param>
        /// <param name="_phrase"></param>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <param name="_claims"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WebSpeechDto Manage(WebSpeechDto data, string _subType, int _step, string _stepType, int expiresInSeconds, string _phrase, HttpResponse response, HttpRequest request, ClaimsDto _claims, string userName, long userId, string _hostSelected)
        {
            WebSpeechDto newWebSpeech = null;
            string newWebSpeechString = "";

            var access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

            if (_step == 0)
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                var name = String.Empty;

                if (_phrase == null || _phrase == "")
                {
                    _phrase = String.Empty;
                    name = "NewImplemented" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                }
                else
                {
                    name = utility.FirstLetterToUpper(_phrase.Trim().Replace(' ', '_'));
                }

                newWebSpeech = new WebSpeechDto() { Name = name, Phrase = @"[[""" + _phrase + @"""]]", Host = "All", Type = data.Type, FinalStep = true, OperationEnable = data.OperationEnable, PrivateInstruction = true, Ico = "/Images/Shortcuts/generic.png", Operation = data.Operation, Parameters = data.Parameters, ElementIndex = data.ElementIndex, SubType = data.SubType, StepType = data.StepType};

                newWebSpeech.Elements = InitElements(newWebSpeech.Elements, newWebSpeech.ElementIndex);

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);

                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            if (_step > 0)
            {
                newWebSpeechString = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                if (newWebSpeechString != "")
                {
                    try
                    {
                        newWebSpeech = JsonConvert.DeserializeObject<WebSpeechDto>(newWebSpeechString);
                        newWebSpeech.ElementIndex = data.ElementIndex;
                        newWebSpeech.SubType = data.SubType;
                        newWebSpeech.StepType = data.StepType;
                        newWebSpeech.Elements = InitElements(newWebSpeech.Elements, newWebSpeech.ElementIndex);
                        if (data.Elements != null && data.Elements.Length > 0)
                            newWebSpeech.Elements[0] = data.Elements[0];
                        data.Operation = newWebSpeech.Operation;
                        data.Parameters = newWebSpeech.Parameters;
                        data.Type = newWebSpeech.Type;
                        data.Elements = newWebSpeech.Elements;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            var setCookie = false;

            // Manage RequestNotImplemented
            var manageRequestNotImplementedResult = ManageRequestNotImplemented(setCookie, newWebSpeech, data.StepType, expiresInSeconds, data.SubType, _phrase, data, data.Step, userName, userId, response, request, _claims, access_token_cookie);
            setCookie = manageRequestNotImplementedResult.SetCookie;
            newWebSpeech = manageRequestNotImplementedResult.NewWebSpeech;
            data = manageRequestNotImplementedResult.Data;

            // Manage Note
            var manageNoteResult = ManageNote(setCookie, newWebSpeech, data.StepType, expiresInSeconds, data.SubType, _phrase, data, data.Step, userName, userId, response, request, _claims, access_token_cookie);
            setCookie = manageNoteResult.SetCookie;
            newWebSpeech = manageNoteResult.NewWebSpeech;
            data = manageNoteResult.Data;

            // Manage WebSearch
            var manageWebSearchResult = ManageWebSearch(setCookie, newWebSpeech, data.StepType, expiresInSeconds, data.SubType, _phrase, data, data.Step, userName, userId, response, request, _claims);
            setCookie = manageWebSearchResult.SetCookie;
            newWebSpeech = manageWebSearchResult.NewWebSpeech;
            data = manageWebSearchResult.Data;

            // Manage RunExe
            var manageRunExeResult = ManageRunExe(setCookie, newWebSpeech, data.StepType, expiresInSeconds, data.SubType, _phrase, data, data.Step, userName, userId, response, request, _claims, _hostSelected, access_token_cookie);
            setCookie = manageRunExeResult.SetCookie;
            newWebSpeech = manageRunExeResult.NewWebSpeech;
            data = manageRunExeResult.Data;

            // Manage Reminder
            var manageReminderResult = ManageReminder(setCookie, newWebSpeech, data.StepType, expiresInSeconds, data.SubType, _phrase, data, data.Step, userName, userId, response, request, _claims, access_token_cookie);
            setCookie = manageReminderResult.SetCookie;
            newWebSpeech = manageReminderResult.NewWebSpeech;
            data = manageReminderResult.Data;

            // Manage Timer
            var manageTimerResult = ManageTimer(setCookie, newWebSpeech, data.StepType, expiresInSeconds, data.SubType, _phrase, data, data.Step, userName, userId, response, request, _claims, access_token_cookie);
            setCookie = manageTimerResult.SetCookie;
            newWebSpeech = manageTimerResult.NewWebSpeech;
            data = manageTimerResult.Data;

            if (setCookie)
            {
                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            return data;
        }

        private (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) ManageRequestNotImplemented(bool setCookie, WebSpeechDto newWebSpeech, string _stepType, int expiresInSeconds, string _subType, string _phrase, WebSpeechDto data, int _step, string userName, long userId, HttpResponse response, HttpRequest request, ClaimsDto _claims, string access_token_cookie)
        {
            (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) result;

            result.SetCookie = false;
            result.NewWebSpeech = null;
            result.Data = null;

            if (_stepType == StepTypes.GetElementValue.ToString() 
                && newWebSpeech.ElementIndex == 1
                && (
                    _subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString()
                   )
               )
            {
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = _phrase.Trim();
                setCookie = true;
            }

            if (_stepType == StepTypes.ApplyManually.ToString() && _subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString())
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                newWebSpeech.Ico = "/Images/Shortcuts/answer.png";

                try
                {
                    data.NewWebSpeechRequestName = newWebSpeech.Name;
                    newWebSpeech.Answer = @"[""" + newWebSpeech.Elements[1].Value + @"""]";
                    suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechDtoInJsonCookieName + "_" + newWebSpeech.Name, JsonConvert.SerializeObject(newWebSpeech), expiresInSeconds);
                }
                catch (Exception)
                {
                }
            }

            if (_stepType == StepTypes.ApplyNow.ToString() && _subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString())
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                newWebSpeech.Ico = "/Images/Shortcuts/answer.png";

                var addWebSpeechResult = webSpeecheRepo.AddWebSpeech(newWebSpeech, access_token_cookie).GetAwaiter().GetResult();

                if (addWebSpeechResult.Successful == false) data.Error = addWebSpeechResult.Message;

                data.NewWebSpeechRequestName = null;
            }

            result.SetCookie = setCookie;
            result.NewWebSpeech = newWebSpeech;
            result.Data = data;

            return result;
        }

        private (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) ManageNote(bool setCookie, WebSpeechDto newWebSpeech, string _stepType, int expiresInSeconds, string _subType, string _phrase, WebSpeechDto data, int _step, string userName, long userId, HttpResponse response, HttpRequest request, ClaimsDto _claims, string access_token_cookie)
        {
            (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) result;

            result.SetCookie = false;
            result.NewWebSpeech = null;
            result.Data = null;

            if (_stepType == StepTypes.GetElementValue.ToString() 
                && newWebSpeech.ElementIndex == 1
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueClearNote.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteNote.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateNote.ToString()
                   )
               )
            {
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = _phrase.Trim();
                setCookie = true;
            }

            if (_stepType == StepTypes.ApplyNow.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteTimer.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteAlarmClock.ToString()
                   )
               )
            {
                var index = newWebSpeech.ElementIndex;
                if (newWebSpeech.Elements[index] == null)
                    newWebSpeech.Elements[index] = new Element() { Value = String.Empty };
                newWebSpeech.Elements[index].Value = data.Parameters.Trim();
                setCookie = true;
            }

            if (_stepType == StepTypes.GetElementValue.ToString() 
                && newWebSpeech.ElementIndex == 2
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueClearNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString()
                   )
               )
            {
                if (
                    (
                        _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString()
                    )
                        && data.Parameters != null && data.Parameters != String.Empty
                   )
                {
                    var index = newWebSpeech.ElementIndex - 1;
                    if (newWebSpeech.Elements[index] == null)
                        newWebSpeech.Elements[index] = new Element() { Value = String.Empty };
                    newWebSpeech.Elements[index].Value = data.Parameters.Trim();
                }

                if (newWebSpeech.Elements[newWebSpeech.ElementIndex].Value != null && newWebSpeech.Elements[newWebSpeech.ElementIndex].Value != "")
                    newWebSpeech.Elements[newWebSpeech.ElementIndex].Value += "\n";
                else
                    newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = "";

                if (_phrase == null) _phrase = "";

                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value += _phrase.Trim();
                setCookie = true;
            }

            if (
                _stepType == StepTypes.ApplyNow.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueClearNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteTimer.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueDeleteAlarmClock.ToString()
                    )
                )
            {
                var eventResult = new EventResult();

                if (_subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString())
                eventResult = dialogueAddToNote.AddElementInNote(newWebSpeech, access_token_cookie, userName, userId).GetAwaiter().GetResult();

                if (_subType == WebSpeechTypes.SystemDialogueClearNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString())
                    eventResult = dialogueClearNote.ClearNote(newWebSpeech, access_token_cookie, userName, userId).GetAwaiter().GetResult();

                if (_subType == WebSpeechTypes.SystemDialogueCreateNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueCreateNoteWithName.ToString())
                    eventResult = dialogueCreateNote.CreateNote(newWebSpeech, access_token_cookie, userName, userId, _claims).GetAwaiter().GetResult();

                if (_subType == WebSpeechTypes.SystemDialogueDeleteNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueDeleteNoteWithName.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueDeleteTimer.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueDeleteAlarmClock.ToString()
                        )
                    eventResult = dialogueDeleteNote.DeleteNote(newWebSpeech, access_token_cookie, userName, userId).GetAwaiter().GetResult();

                if (!eventResult.Successful)
                {
                    newWebSpeech.FinalStep = true;

                    if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                        newWebSpeech.Answer = "Errore: " + utility.SplitCamelCase(_subType.Replace("System", "").Replace("Dialogue", ""));

                    if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                        newWebSpeech.Answer = "Error: " + utility.SplitCamelCase(_subType.Replace("System", "").Replace("Dialogue", ""));
                }
            }

            result.SetCookie = setCookie;
            result.NewWebSpeech = newWebSpeech;
            result.Data = data;

            return result;
        }

        private string GetPhrase(string phrase)
        {
            //ITA
            if (phrase.Trim().ToLower() == "niente") phrase = "";
            if (phrase.Trim().ToLower() == "nulla") phrase = "";

            if (phrase.Trim().ToLower() == "non inserire") phrase = "";
            if (phrase.Trim().ToLower() == "non inserire niente") phrase = "";
            if (phrase.Trim().ToLower() == "non inserire nulla") phrase = "";

            if (phrase.Trim().ToLower() == "non aggiungere") phrase = "";
            if (phrase.Trim().ToLower() == "non aggiungere niente") phrase = "";
            if (phrase.Trim().ToLower() == "non aggiungere nulla") phrase = "";

            if (phrase.Trim().ToLower() == "non metterci niente") phrase = "";
            if (phrase.Trim().ToLower() == "non metterci nulla") phrase = "";

            if (phrase.Trim().ToLower() == "lascia vuoto") phrase = "";
            if (phrase.Trim().ToLower() == "vuoto") phrase = "";
            if (phrase.Trim().ToLower() == "lascia stare") phrase = "";

            //ENG
            if (phrase.Trim().ToLower() == "nothing") phrase = "";
            if (phrase.Trim().ToLower() == "nothing") phrase = "";

            if (phrase.Trim().ToLower() == "do not insert") phrase = "";
            if (phrase.Trim().ToLower() == "do not insert anything") phrase = "";
            if (phrase.Trim().ToLower() == "do not insert anything") phrase = "";

            if (phrase.Trim().ToLower() == "do not add") phrase = "";
            if (phrase.Trim().ToLower() == "do not add anything") phrase = "";
            if (phrase.Trim().ToLower() == "do not add anything") phrase = "";

            if (phrase.Trim().ToLower() == "do not put anything in it") phrase = "";
            if (phrase.Trim().ToLower() == "do not put anything in it") phrase = "";

            if (phrase.Trim().ToLower() == "don't insert") phrase = "";
            if (phrase.Trim().ToLower() == "don't insert anything") phrase = "";
            if (phrase.Trim().ToLower() == "don't insert anything") phrase = "";

            if (phrase.Trim().ToLower() == "don't add") phrase = "";
            if (phrase.Trim().ToLower() == "don't add anything") phrase = "";
            if (phrase.Trim().ToLower() == "don't add anything") phrase = "";

            if (phrase.Trim().ToLower() == "don't put anything in it") phrase = "";
            if (phrase.Trim().ToLower() == "don't put anything in it") phrase = "";

            if (phrase.Trim().ToLower() == "leave blank") phrase = "";
            if (phrase.Trim().ToLower() == "empty") phrase = "";
            if (phrase.Trim().ToLower() == "leave it alone") phrase = "";

            return phrase;
        }

        private (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) ManageReminder(bool setCookie, WebSpeechDto newWebSpeech, string _stepType, int expiresInSeconds, string _subType, string _phrase, WebSpeechDto data, int _step, string userName, long userId, HttpResponse response, HttpRequest request, ClaimsDto _claims, string access_token_cookie)
        {
            (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) result;

            result.SetCookie = false;
            result.NewWebSpeech = null;
            result.Data = null;

            if (_stepType == StepTypes.GetElementValue.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueDeleteReminder.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateExtendedReminder.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateReminder.ToString()
                   )
               )
            {
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = GetPhrase(_phrase.Trim());
                setCookie = true;
            }

            if ((_stepType == StepTypes.GetElementDateTime.ToString() || _stepType == StepTypes.GetElementDate.ToString() || _stepType == StepTypes.GetElementTime.ToString())
                && (
                        _subType == WebSpeechTypes.SystemDialogueCreateExtendedReminder.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateReminder.ToString()
                   )
               )
            {
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = _phrase.Trim();
                setCookie = true;
            }

            if (
                _stepType == StepTypes.ApplyNow.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueCreateExtendedReminder.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueCreateReminder.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueDeleteReminder.ToString()

                    )
                )
            {
                var eventResult = new EventResult();

                if (_subType == WebSpeechTypes.SystemDialogueCreateExtendedReminder.ToString())
                    eventResult = dialogueCreateExtendedReminder.CreateExtendedReminder(newWebSpeech, access_token_cookie, userName, userId, _claims).GetAwaiter().GetResult();

                if (_subType == WebSpeechTypes.SystemDialogueCreateReminder.ToString())
                    eventResult = dialogueCreateReminder.CreateReminder(newWebSpeech, access_token_cookie, userName, userId, _claims).GetAwaiter().GetResult();

                if (_subType == WebSpeechTypes.SystemDialogueDeleteReminder.ToString())
                    eventResult = dialogueDeleteReminder.DeleteReminder(newWebSpeech, access_token_cookie, userName, userId).GetAwaiter().GetResult();

                if (!eventResult.Successful)
                {
                    newWebSpeech.FinalStep = true;

                    if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                        newWebSpeech.Answer = "Errore: " + utility.SplitCamelCase(_subType.Replace("System", "").Replace("Dialogue", ""));

                    if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                        newWebSpeech.Answer = "Error: " + utility.SplitCamelCase(_subType.Replace("System", "").Replace("Dialogue", ""));
                }
            }

            result.SetCookie = setCookie;
            result.NewWebSpeech = newWebSpeech;
            result.Data = data;

            return result;
        }

        private (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) ManageWebSearch(bool setCookie, WebSpeechDto newWebSpeech, string _stepType, int expiresInSeconds, string _subType, string _phrase, WebSpeechDto data, int _step, string userName, long userId, HttpResponse response, HttpRequest request, ClaimsDto _claims)
        {
            (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) result;

            result.SetCookie = false;
            result.NewWebSpeech = null;
            result.Data = null;

            if (_stepType == StepTypes.GetElementValue.ToString() 
                && newWebSpeech.ElementIndex == 1
                && (
                        _subType == WebSpeechTypes.SystemDialogueWebSearch.ToString()
                   )
               )
            {
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = _phrase.Trim();
                setCookie = true;
            }

            if (
                _stepType == StepTypes.Execute.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueWebSearch.ToString()
                   )
                )
            {
                var dialogueWebSearch = new DialogueWebSearch();

                //data.Type = WebSpeechTypes.SystemWebSearch.ToString();

                var webSearchResult = dialogueWebSearch.WebSearch(data, newWebSpeech.Elements[1].Value).GetAwaiter().GetResult();

                data.Parameters = webSearchResult.Parameters;
            }

            result.SetCookie = setCookie;
            result.NewWebSpeech = newWebSpeech;
            result.Data = data;

            return result;
        }

        private (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) ManageRunExe(bool setCookie, WebSpeechDto newWebSpeech, string _stepType, int expiresInSeconds, string _subType, string _phrase, WebSpeechDto data, int _step, string userName, long userId, HttpResponse response, HttpRequest request, ClaimsDto _claims, string _hostSelected, string access_token_cookie)
        {
            (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) result;

            result.SetCookie = false;
            result.NewWebSpeech = null;
            result.Data = null;

            if (_stepType == StepTypes.GetElementValue.ToString() 
                && newWebSpeech.ElementIndex == 1
                && (
                        _subType == WebSpeechTypes.SystemDialogueRunExe.ToString()
                   )
               )
            {
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[newWebSpeech.ElementIndex].Value = _phrase.Trim();
                setCookie = true;
            }

            if (
                _stepType == StepTypes.Execute.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueRunExe.ToString()
                   )
                )
            {
                var dialogueRunExe = new DialogueRunExe();
                //data.Type = WebSpeechTypes.SystemRunExe.ToString();

                var runExeResult = dialogueRunExe.RunExe(data, newWebSpeech.Elements[1].Value, _hostSelected, access_token_cookie, executionQueueRepo, _claims).GetAwaiter().GetResult();

                data.Parameters = runExeResult.Parameters;
            }

            result.SetCookie = setCookie;
            result.NewWebSpeech = newWebSpeech;
            result.Data = data;

            return result;
        }

        private (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) ManageTimer(bool setCookie, WebSpeechDto newWebSpeech, string _stepType, int expiresInSeconds, string _subType, string _phrase, WebSpeechDto data, int _step, string userName, long userId, HttpResponse response, HttpRequest request, ClaimsDto _claims, string access_token_cookie)
        {
            (bool SetCookie, WebSpeechDto NewWebSpeech, WebSpeechDto Data) result;

            result.SetCookie = false;
            result.NewWebSpeech = null;
            result.Data = null;

            if (_stepType == StepTypes.GetElementDateTime.ToString()
                && newWebSpeech.ElementIndex == 1
                && (
                        _subType == WebSpeechTypes.SystemDialogueSetTimer.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueSetAlarmClock.ToString()
                   )
               )
            {
                var index = newWebSpeech.ElementIndex;
                if (newWebSpeech.Elements[index] == null)
                    newWebSpeech.Elements[index] = new Element() { Value = String.Empty };
                if (_phrase == null) _phrase = "";
                newWebSpeech.Elements[index].Value = _phrase.Trim();
                setCookie = true;
            }

            if (
                _stepType == StepTypes.ApplyNow.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueSetTimer.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueSetAlarmClock.ToString()
                    )
                )
            {
                var eventResult = new EventResult();

                if (_subType == WebSpeechTypes.SystemDialogueSetTimer.ToString() || _subType == WebSpeechTypes.SystemDialogueSetAlarmClock.ToString())
                {
                    var cultureInfo = default(CultureInfo);
                    if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                        cultureInfo = new CultureInfo("it-IT");
                    if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                        cultureInfo = new CultureInfo("en-US");

                    var date = DateTime.Parse(newWebSpeech.Elements[1].Value, cultureInfo);
                    eventResult = dialogueSetTimer.SetTimer(newWebSpeech, access_token_cookie, userName, userId, _claims, request, response, expiresInSeconds, date).GetAwaiter().GetResult();
                }

                if (!eventResult.Successful)
                {
                    newWebSpeech.FinalStep = true;

                    if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                        newWebSpeech.Answer = "Errore: " + utility.SplitCamelCase(_subType.Replace("System", "").Replace("Dialogue", ""));

                    if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                        newWebSpeech.Answer = "Error: " + utility.SplitCamelCase(_subType.Replace("System", "").Replace("Dialogue", ""));
                }
            }

            result.SetCookie = setCookie;
            result.NewWebSpeech = newWebSpeech;
            result.Data = data;

            return result;
        }

        /// <summary>
        /// Get Dialogue Request Not Implemented
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueRequestNotImplemented(string culture, long lastWebSpeechId)
        {
            var dialogue = new DialogueRequestNotImplemented();
            return dialogue.Get(culture, lastWebSpeechId);
        }

        /// <summary>
        /// Get Dialogue Add To Note
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueAddToNote(string culture, long lastWebSpeechId, string _subType)
        {
            return dialogueAddToNote.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Clear Note
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueClearNote(string culture, long lastWebSpeechId, string _subType)
        {
            return dialogueClearNote.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Create Note
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueCreateNote(string culture, long lastWebSpeechId, string _subType)
        {
            return dialogueCreateNote.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Delete Note
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueDeleteNote(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueDeleteNote();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Delete Note
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<EventResult> DeleteNote(WebSpeechDto dto, string token, string userName, long userId)
        {
            var dialogue = new DialogueDeleteNote();
            return await dialogue.DeleteNote(dto, token, userName, userId);
        }

        /// <summary>
        /// Get Dialogue Set Timer
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueSetTimer(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueSetTimer();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Web Search
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueWebSearch(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueWebSearch();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Run Exe
        /// </summary>
        /// <param name="data"></param>
        /// <param name="_phrase"></param>
        /// <param name="_hostSelected"></param>
        /// <param name="access_token_cookie"></param>
        /// <param name="executionQueueRepo"></param>
        /// <returns></returns>
        public async Task<WebSpeechDto> RunExe(WebSpeechDto data, string _phrase, string _hostSelected, string access_token_cookie, ExecutionQueuesRepository executionQueueRepo, ClaimsDto _claims)
        {
            var dialogue = new DialogueRunExe();
            return await dialogue.RunExe(data, _phrase, _hostSelected, access_token_cookie, executionQueueRepo, _claims);
        }

        public async Task<WebSpeechDto> SetTimer(WebSpeechDto dto, string token, string userName, long userId, ClaimsDto _claims, HttpRequest request, HttpResponse response, int expiresInSeconds, DateTime timerDate)
        {
            var dialogue = new DialogueSetTimer();

            var setTimerResult = await dialogue.SetTimer(dto, token, userName, userId, _claims, request, response, expiresInSeconds, timerDate);

            return dto;
        }

        /// <summary>
        /// Get Dialogue Run Exe
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueRunExe(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueRunExe();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Create Extended Remider
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueCreateExtendedReminder(string culture, long lastWebSpeechId, string _subType, HttpRequest request)
        {
            return dialogueCreateExtendedReminder.Get(culture, lastWebSpeechId, _subType, request);
        }

        /// <summary>
        /// Get Dialogue Create Remider
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueCreateReminder(string culture, long lastWebSpeechId, string _subType, HttpRequest request)
        {
            return dialogueCreateReminder.Get(culture, lastWebSpeechId, _subType, request);
        }

        /// <summary>
        /// Get Dialogue Delete Remider
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueDeleteReminder(string culture, long lastWebSpeechId, string _subType)
        {
            return dialogueDeleteReminder.Get(culture, lastWebSpeechId, _subType);
        }
    }
}