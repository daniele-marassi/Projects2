using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Supp.Site.Common;
using SuppModels;
using Supp.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Supp.Site.Common.Config;

namespace Supp.Site.Recognition
{
    public class RequestNotImplemented
    {
        private readonly SuppUtility suppUtility;

        public RequestNotImplemented()
        {
            suppUtility = new SuppUtility();
        }

        /// <summary>
        /// ManageRequestNotImplemented
        /// </summary>
        /// <param name="_subType"></param>
        /// <param name="_step"></param>
        /// <param name="expiresInSeconds"></param>
        /// <param name="_phrase"></param>
        /// <param name="_claims"></param>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static WebSpeechDto ManageRequestNotImplemented(string _subType, int _step, int expiresInSeconds, string _phrase, ClaimsDto _claims, HttpResponse response, HttpRequest request)
        {
            WebSpeechDto newWebSpeech = null;
            var data = new WebSpeechDto() { };
            string newWebSpeechString = "";
            var suppUtility = new SuppUtility();
            var recognitionCommon = new Common();
            var webSpeecheRepo = new WebSpeechesRepository();

            if (_step < 1)
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                newWebSpeech = new WebSpeechDto() { Name = "NewImplemented" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"), Phrase = _phrase, Host = "all", Type = WebSpeechTypes.Request.ToString(), FinalStep = true, OperationEnable = true, PrivateInstruction = true, Ico = " /Images/Shortcuts/generic.png" };

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);

                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            if (_subType == null || _subType == "") _subType = WebSpeechTypes.RequestNotImplemented.ToString();

            _step++;

            var requestsNotImplemented = GetRequestNotImplemented(_claims.Configuration.General.Culture);
            var dataResult = recognitionCommon.GetData(requestsNotImplemented);

            if (_step == 2 || _step == 4 || _step == 6)
            {
                var matchPhraseResult = recognitionCommon.MatchPhrase(_phrase, dataResult.Data, _claims);
                data = matchPhraseResult.Data;

                data.Phrase = "";

                if (data != null) _step = data.Step;
                if (_step == 2 && data?.FinalStep != true) _step++;
                if (_step == 6 && data?.FinalStep != true) _step++;
            }

            if ((data?.FinalStep != true) && _step != 2 && _step != 4 && _step != 6)
            {
                data = dataResult.Data.Where(_ => _.SubType == _subType && _.Step == _step).FirstOrDefault();
                data = recognitionCommon.GetAnswer(data);
                if (data?.FinalStep != true) _step++;
            }

            if (_step > 2)
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
                if (_step == 6) newWebSpeech.Answer = _phrase.Trim();

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            if ((_step == 6 || _step == 8) && data?.FinalStep == true)
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                data.NewWebSpeechDtoInJson = JsonConvert.SerializeObject(newWebSpeech);
            }

            if ((_step == 7) && data?.FinalStep == true)
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                string access_token_cookie = suppUtility.ReadCookie(request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                var addWebSpeechResult = webSpeecheRepo.AddWebSpeech(newWebSpeech, access_token_cookie).GetAwaiter().GetResult();

                if (addWebSpeechResult.Successful == false) data.Error = addWebSpeechResult.Message;

                data.NewWebSpeechDtoInJson = null;
            }

            return data;
        }

        /// <summary>
        /// GetRequestNotImplemented
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplemented(string culture)
        {
            var result = new List<WebSpeechDto>() { };
            var id = 0;

            if (culture.ToLower() == "it-it")
            {
                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"Mi dispiace, non conosco! Mi insegni?",
                        Answer = @"Mi dispiace, non conosco! Mi insegni?",
                        Host = "all",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 1,
                        OperationEnable = true,
                        ParentIds = ""
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[""no"", ""non ora"", ""no grazie""]",
                        Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                        Host = "all",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 2,
                        OperationEnable = true,
                        ParentIds = "[1]"
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]",
                        Answer = null,
                        Host = "all",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 2,
                        OperationEnable = true,
                        ParentIds = "[1]"
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"Cosa devo fare?",
                        Answer = @"[""Cosa devo fare?, Rispondere?, Altro?"",""Dimmi, cosa faccio?, Rispondere?, Altro? ""]",
                        Host = "all",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 3,
                        OperationEnable = true,
                        ParentIds = ""
                    }
                );

                id++;
                result.AddRange(GetRequestNotImplementedAnswerIta(id, 4, 4));

                id++;
                result.AddRange(GetRequestNotImplementedInsertIta(id, 5, 6));

                id++;
                result.AddRange(GetRequestNotImplementedOtherIta(id, 8, 4));
            }

            if (culture.ToLower() == "en-us")
            {
                id = 0;

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"I'm sorry, I don't know! Teach me?",
                        Answer = @"I'm sorry, I don't know! Teach me?",
                        Host = "all",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 1,
                        OperationEnable = true,
                        ParentIds = ""
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[""no"", ""not now"", ""no thanks""]",
                        Answer = @"[""ok"",""all right"",""of course"",""well""]",
                        Host = "all",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 2,
                        OperationEnable = true,
                        ParentIds = "[1]"
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[""yes"",""ok"",""yes that's fine"", ""ok it's good""]",
                        Answer = null,
                        Host = "all",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 2,
                        OperationEnable = true,
                        ParentIds = "[1]"
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"What should I do?",
                        Answer = @"[""What should I do?"",""Tell me, what do I do?""]",
                        Host = "all",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                        Step = 3,
                        OperationEnable = true,
                        ParentIds = "[1]"
                    }
                );

                id++;
                result.AddRange(GetRequestNotImplementedAnswerEng(id, 4, 4));

                id++;
                result.AddRange(GetRequestNotImplementedInsertEng(id, 5, 6));

                id++;
                result.AddRange(GetRequestNotImplementedOtherEng(id, 8, 4));
            }

            return result;
        }


        /// <summary>
        /// GetRequestNotImplementedInsertIta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplementedInsertIta(int id, int step, int parentId)
        {
            var result = new List<WebSpeechDto>() { };

            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"Inserisco",
                    Answer = @"[""Inserisco subito?"", ""Inserisco ora?"", ""Aggiungo subito?"", ""Aggiungo ora?""]",
                    Host = "all",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = ""
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[""no"", ""non ora"", ""no grazie""]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "all",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            id++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "all",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Inserito""]",
                    Host = "all",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = ""
                }
            );

            return result;
        }

        /// <summary>
        /// GetRequestNotImplementedAnswerIta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplementedAnswerIta(int id, int step, int parentId)
        {
            var result = new List<WebSpeechDto>() { };

            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"Rispondere",
                    Answer = @"[""Cosa devo rispondere?""]",
                    Host = "all",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            return result;
        }

        /// <summary>
        /// GetRequestNotImplementedOtherIta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplementedOtherIta(int id, int step, int parentId)
        {
            var result = new List<WebSpeechDto>() { };

            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"Altro",
                    Answer = @"[""Ora preparo""]",
                    Host = "all",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            return result;
        }

        /// <summary>
        /// GetRequestNotImplementedInsertEng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplementedInsertEng(int id, int step, int parentId)
        {
            var result = new List<WebSpeechDto>() { };

            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"I insert",
                    Answer = @"[""Do I post immediately?"", ""Do I post now?"", ""Do I add immediately?"", ""Add now?""]",
                    Host = "all",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = ""
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[""no"", ""not now"", ""no thanks""]",
                    Answer = @"[""ok"",""all right"",""of course"",""well""]",
                    Host = "all",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            id++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[""yes"",""ok"",""yes that's fine"", ""ok it's good""]",
                    Answer = @"[""ok"",""all right"",""of course"",""well""]",
                    Host = "all",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Inserted!""]",
                    Host = "all",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = ""
                }
            );

            return result;
        }

        /// <summary>
        /// GetRequestNotImplementedAnswerEng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplementedAnswerEng(int id, int step, int parentId)
        {
            var result = new List<WebSpeechDto>() { };

            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"Answer",
                    Answer = @"[""What I have to answer?""]",
                    Host = "all",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            return result;
        }

        /// <summary>
        /// GetRequestNotImplementedOtherEng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplementedOtherEng(int id, int step, int parentId)
        {
            var result = new List<WebSpeechDto>() { };

            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.RequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"Other",
                    Answer = @"[""Now I prepare""]",
                    Host = "all",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.RequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]"
                }
            );

            return result;
        }
    }
}