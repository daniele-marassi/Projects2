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

namespace Supp.Site.Recognition
{
    public class Dialogue
    {
        private readonly SuppUtility suppUtility;
        private readonly DialogueAddToNote dialogueAddToNote;
        private readonly DialogueClearNote dialogueClearNote;
        private readonly WebSpeechesRepository webSpeecheRepo;
        private readonly ExecutionQueuesRepository executionQueueRepo;


        public Dialogue()
        {
            suppUtility = new SuppUtility();
            dialogueAddToNote = new DialogueAddToNote();
            dialogueClearNote = new DialogueClearNote();
            webSpeecheRepo = new WebSpeechesRepository();
            executionQueueRepo = new ExecutionQueuesRepository(); 
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

                if (_phrase == null)
                {
                    _phrase = String.Empty;
                    name = "NewImplemented" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                }
                else
                {
                    name = suppUtility.FirstLetterToUpper(_phrase.Trim().Replace(' ', '_'));
                }

                newWebSpeech = new WebSpeechDto() { Name = name, Phrase = @"[[""" + _phrase + @"""]]", Host = "All", Type = WebSpeechTypes.Request.ToString(), FinalStep = true, OperationEnable = data.OperationEnable, PrivateInstruction = true, Ico = "/Images/Shortcuts/generic.png", Operation = data.Operation, Parameters = data.Parameters};

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
                        data.Operation = newWebSpeech.Operation;
                        data.Parameters = newWebSpeech.Parameters;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            var setCookie = false;

            // Manage RequestNotImplemented
            var manageRequestNotImplementedResult = ManageRequestNotImplemented(setCookie, newWebSpeech, _stepType, expiresInSeconds, _subType, _phrase, data, _step, userName, userId, response, request, _claims, access_token_cookie);
            setCookie = manageRequestNotImplementedResult.SetCookie;
            newWebSpeech = manageRequestNotImplementedResult.NewWebSpeech;
            data = manageRequestNotImplementedResult.Data;

            // Manage Note
            var manageNoteResult = ManageNote(setCookie, newWebSpeech, _stepType, expiresInSeconds, _subType, _phrase, data, _step, userName, userId, response, request, _claims, access_token_cookie);
            setCookie = manageNoteResult.SetCookie;
            newWebSpeech = manageNoteResult.NewWebSpeech;
            data = manageNoteResult.Data;

            // Manage WebSearch
            var manageWebSearchResult = ManageWebSearch(setCookie, newWebSpeech, _stepType, expiresInSeconds, _subType, _phrase, data, _step, userName, userId, response, request, _claims);
            setCookie = manageWebSearchResult.SetCookie;
            newWebSpeech = manageWebSearchResult.NewWebSpeech;
            data = manageWebSearchResult.Data;

            // Manage RunExe
            var manageRunExeResult = ManageRunExe(setCookie, newWebSpeech, _stepType, expiresInSeconds, _subType, _phrase, data, _step, userName, userId, response, request, _claims, _hostSelected, access_token_cookie);
            setCookie = manageRunExeResult.SetCookie;
            newWebSpeech = manageRunExeResult.NewWebSpeech;
            data = manageRunExeResult.Data;

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

            if (_stepType == StepTypes.GetAnswer.ToString()
                && (
                    _subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString()
                   )
               )
            {
                newWebSpeech.Answer = @"[""" + _phrase.Trim() + @"""]";
                setCookie = true;
            }

            if (_stepType == StepTypes.AddManually.ToString() && data?.FinalStep == true && (_subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString()))
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                try
                {
                    data.NewWebSpeechRequestName = newWebSpeech.Name;
                    suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechDtoInJsonCookieName + "_" + newWebSpeech.Name, JsonConvert.SerializeObject(newWebSpeech), expiresInSeconds);
                }
                catch (Exception)
                {
                }
            }

            if (_stepType == StepTypes.AddNow.ToString() && data?.FinalStep == true && (_subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString()))
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

            if (_stepType == StepTypes.GetElementName.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNote.ToString()
                   )
               )
            {
                newWebSpeech.ElementName = _phrase.Trim();
                setCookie = true;
            }

            if (_stepType == StepTypes.GetElementValue.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()

                        || _subType == WebSpeechTypes.SystemDialogueClearNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()
                   )
               )
            {
                if (
                    (
                        _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()
                    )
                        && data.Parameters != null && data.Parameters != String.Empty
                   )
                {
                    newWebSpeech.ElementName = data.Parameters.Trim();
                }

                newWebSpeech.ElementValue = _phrase.Trim();
                setCookie = true;
            }

            if (
                _step > 0 && _stepType == StepTypes.GetElementValue.ToString()
                && (
                        _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()
                    )
                )
            {
                var eventResult = dialogueAddToNote.AddElementInNote(newWebSpeech, access_token_cookie, userName, userId).GetAwaiter().GetResult();

                if (!eventResult.Successful)
                {
                    newWebSpeech.FinalStep = true;

                    if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                        newWebSpeech.Answer = "Errore: aggiungi elemento nella nota";

                    if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                        newWebSpeech.Answer = "Error: Add Element In Note";
                }
            }

            if (
                (
                    (_step > 0 && _stepType == StepTypes.GetElementValue.ToString())

                    || (_step == 0 && data?.FinalStep == true)

                )
                && (
                        _subType == WebSpeechTypes.SystemDialogueClearNote.ToString()
                        || _subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()
                   )
                )
            {
                var eventResult = dialogueClearNote.ClearNote(newWebSpeech, access_token_cookie, userName, userId).GetAwaiter().GetResult();

                if (!eventResult.Successful)
                {
                    newWebSpeech.FinalStep = true;

                    if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                        newWebSpeech.Answer = "Errore: nel pulire la nota";

                    if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                        newWebSpeech.Answer = "Error: in cleaning the note";
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
                && (
                        _subType == WebSpeechTypes.SystemDialogueWebSearch.ToString()
                   )
               )
            {
                newWebSpeech.ElementValue = _phrase.Trim();
                setCookie = true;
            }

            if (
                (
                    (_step > 0 && _stepType == StepTypes.GetElementValue.ToString())
                )
                && (
                        _subType == WebSpeechTypes.SystemDialogueWebSearch.ToString()
                   )
                )
            {
                var dialogueWebSearch = new DialogueWebSearch();

                data.Type = WebSpeechTypes.SystemWebSearch.ToString();

                var webSearchResult = dialogueWebSearch.WebSearch(data, _phrase).GetAwaiter().GetResult();

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
                && (
                        _subType == WebSpeechTypes.SystemDialogueRunExe.ToString()
                   )
               )
            {
                newWebSpeech.ElementValue = _phrase.Trim();
                setCookie = true;
            }

            if (
                (
                    (_step > 0 && _stepType == StepTypes.GetElementValue.ToString())
                )
                && (
                        _subType == WebSpeechTypes.SystemDialogueRunExe.ToString()
                   )
                )
            {
                var dialogueRunExe = new DialogueRunExe();
                data.Type = WebSpeechTypes.SystemRunExe.ToString();

                var runExeResult = dialogueRunExe.RunExe(data, _phrase, _hostSelected, access_token_cookie, executionQueueRepo).GetAwaiter().GetResult();

                data.Parameters = runExeResult.Parameters;
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
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueAddToNote(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueAddToNote();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
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
            var dialogue = new DialogueClearNote();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Create Note
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueCreateNote(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueCreateNote();
            return dialogue.Get(culture, lastWebSpeechId, _subType);
        }

        /// <summary>
        /// Get Dialogue Delete Note
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueDeleteNote(string culture, long lastWebSpeechId, string _subType)
        {
            var dialogue = new DialogueDeleteNote();
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
        public async Task<WebSpeechDto> RunExe(WebSpeechDto data, string _phrase, string _hostSelected, string access_token_cookie, ExecutionQueuesRepository executionQueueRepo)
        {
            var dialogue = new DialogueRunExe();
            return await dialogue.RunExe(data, _phrase, _hostSelected, access_token_cookie, executionQueueRepo);
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
    }
}