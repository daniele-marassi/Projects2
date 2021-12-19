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
    public class WebSpeechRequest
    {
        private readonly SuppUtility suppUtility;

        public WebSpeechRequest()
        {
            suppUtility = new SuppUtility();
        }

        /// <summary>
        /// Manage
        /// </summary>
        /// <param name="_subType"></param>
        /// <param name="_step"></param>
        /// <param name="expiresInSeconds"></param>
        /// <param name="_phrase"></param>
        /// <param name="_claims"></param>
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
                if (_stepType == StepTypes.GetAnswer.ToString() && _subType == WebSpeechTypes.SystemRequestNotImplemented.ToString()) newWebSpeech.Answer = @"[""" + _phrase.Trim() + @"""]";

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            if (_stepType == StepTypes.AddManually.ToString() && data?.FinalStep == true && _subType == WebSpeechTypes.SystemRequestNotImplemented.ToString())
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

            if (_stepType == StepTypes.AddNow.ToString() && data?.FinalStep == true && _subType == WebSpeechTypes.SystemRequestNotImplemented.ToString())
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                string access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                var addWebSpeechResult = webSpeecheRepo.AddWebSpeech(newWebSpeech, access_token_cookie).GetAwaiter().GetResult();

                if (addWebSpeechResult.Successful == false) data.Error = addWebSpeechResult.Message;

                data.NewWebSpeechRequestName = null;
            }

            return data;
        }

        public List<WebSpeechDto> GetRequestNotImplemented(string culture, long lastWebSpeechId)
        {
            var requestNotImplemented = new RequestNotImplemented();
            return requestNotImplemented.Get(culture, lastWebSpeechId);
        }

        public List<WebSpeechDto> GetRequestAddToNote(string culture, long lastWebSpeechId)
        {
            var requestAddToNote = new RequestAddToNote();
            return requestAddToNote.Get(culture, lastWebSpeechId);
        }
    }
}