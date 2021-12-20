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

        public Dialogue()
        {
            suppUtility = new SuppUtility();
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
        /// <returns></returns>
        public WebSpeechDto Manage(WebSpeechDto data, string _subType, int _step, string _stepType, int expiresInSeconds, string _phrase, HttpResponse response, HttpRequest request)
        {
            WebSpeechDto newWebSpeech = null;
            string newWebSpeechString = "";
            var suppUtility = new SuppUtility();
            var recognitionCommon = new Common();
            var webSpeecheRepo = new WebSpeechesRepository();

            if (_step == 0)
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                newWebSpeech = new WebSpeechDto() { Name = "NewImplemented" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"), Phrase = @"[[""" + _phrase + @"""]]", Host = "All", Type = WebSpeechTypes.Request.ToString(), FinalStep = true, OperationEnable = true, PrivateInstruction = true, Ico = "/Images/Shortcuts/generic.png" };

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
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            if (_step > 2 && data?.FinalStep != true)
            {
                if (_stepType == StepTypes.GetAnswer.ToString() && (_subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() || _subType == WebSpeechTypes.SystemDialogueGeneric.ToString())) newWebSpeech.Answer = @"[""" + _phrase.Trim() + @"""]";

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            if (_stepType == StepTypes.AddManually.ToString() && data?.FinalStep == true && (_subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() || _subType == WebSpeechTypes.SystemDialogueGeneric.ToString()))
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

            if (_stepType == StepTypes.AddNow.ToString() && data?.FinalStep == true && (_subType == WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() || _subType == WebSpeechTypes.SystemDialogueGeneric.ToString()))
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                string access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                var addWebSpeechResult = webSpeecheRepo.AddWebSpeech(newWebSpeech, access_token_cookie).GetAwaiter().GetResult();

                if (addWebSpeechResult.Successful == false) data.Error = addWebSpeechResult.Message;

                data.NewWebSpeechRequestName = null;
            }

            return data;
        }

        /// <summary>
        /// Get Dialogue RequestNotImplemented
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueRequestNotImplemented(string culture, long lastWebSpeechId)
        {
            var dialogueRequestNotImplemented = new DialogueRequestNotImplemented();
            return dialogueRequestNotImplemented.Get(culture, lastWebSpeechId);
        }

        /// <summary>
        /// Get Dialogue AddToNote
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> GetDialogueAddToNote(string culture, long lastWebSpeechId)
        {
            var dialogueAddToNote = new DialogueAddToNote();
            return dialogueAddToNote.Get(culture, lastWebSpeechId);
        }
    }
}