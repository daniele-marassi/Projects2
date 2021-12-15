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
        public static WebSpeechDto Manage(WebSpeechDto data, int _step, int expiresInSeconds, string _phrase, HttpResponse response, HttpRequest request)
        {
            WebSpeechDto newWebSpeech = null;
            string newWebSpeechString = "";
            var suppUtility = new SuppUtility();
            var recognitionCommon = new Common();
            var webSpeecheRepo = new WebSpeechesRepository();

            if (_step == 0)
            {
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                newWebSpeech = new WebSpeechDto() { Name = "NewImplemented" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"), Phrase = @"[""" + _phrase + @"""]", Host = "All", Type = WebSpeechTypes.Request.ToString(), FinalStep = true, OperationEnable = true, PrivateInstruction = true, Ico = "/Images/Shortcuts/generic.png" };

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);

                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            //if (data?.StepType != StepTypes.GetAnswer.ToString())
            //     data.Phrase = "";

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
                if (data?.StepType == StepTypes.GetAnswer.ToString()) newWebSpeech.Answer = @"[""" + _phrase.Trim() + @"""]";

                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);
                suppUtility.RemoveCookie(response, request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                suppUtility.SetCookie(response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
            }

            if (data?.StepType == StepTypes.AddManually.ToString() && data?.FinalStep == true)
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

            if (data?.StepType == StepTypes.AddNow.ToString() && data?.FinalStep == true)
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
        /// GetRequestNotImplemented
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static List<WebSpeechDto> GetRequestNotImplemented(string culture, long lastWebSpeechId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = lastWebSpeechId + 1;
            long id = 0;
            var step = 0;

            if (culture.ToLower() == "it-it")
            {
                id = startId;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""Mi dispiace, non conosco! Mi insegni?""]]",
                        Answer = @"Mi dispiace, non conosco! Mi insegni?",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
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
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""no"", ""non ora"", ""no grazie""]]",
                        Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                        Host = "All",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "["+ (startId).ToString() + "]"
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]]",
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]"
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""Cosa devo fare?""]]",
                        Answer = @"[""Cosa devo fare?, Rispondere?, Altro?"",""Dimmi, cosa faccio?, Rispondere?, Altro? ""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]"
                    }
                );

                var parentId = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(GetRequestNotImplementedAnswerIta(id, step, parentId));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(GetRequestNotImplementedInsertIta(id, step, id));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(GetRequestNotImplementedOtherIta(id, step, parentId));
            }

            if (culture.ToLower() == "en-us")
            {
                id = startId;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""I'm sorry, I don't know! Can you teach me?""]]",
                        Answer = @"I'm sorry, I don't know! Can you teach me?",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
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
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""no"",""not now"",""no thanks""]]",
                        Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                        Host = "All",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]"
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""yes"",""ok"",""yes alright"",""yes"",""ok"",""okay"",""ok go well""]]",
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]"
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""What should I do?""]]",
                        Answer = @"[""What should I do ?, Answer ?, Other?"",""Tell me, what should I do ?, Answer ?, Other?""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]"
                    }
                );

                var parentId = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(GetRequestNotImplementedAnswerIta(id, step, parentId));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(GetRequestNotImplementedInsertIta(id, step, id));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(GetRequestNotImplementedOtherIta(id, step, parentId));
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
        public static List<WebSpeechDto> GetRequestNotImplementedInsertIta(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Inserisco""]]",
                    Answer = @"[""Inserisco subito?"", ""Inserisco ora?"", ""Aggiungo subito?"", ""Aggiungo ora?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.GetAnswer.ToString()
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""no"", ""non ora"", ""no grazie""]]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + startId.ToString() + "]",
                    StepType = StepTypes.AddManually.ToString()
                }
            );

            id++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + startId.ToString() + "]"
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Inserito""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + (id-1).ToString() + "]",
                    StepType = StepTypes.AddNow.ToString()
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
        public static List<WebSpeechDto> GetRequestNotImplementedAnswerIta(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Rispondere""]]",
                    Answer = @"[""Cosa devo rispondere?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
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
        public static List<WebSpeechDto> GetRequestNotImplementedOtherIta(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++; result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Altro""]]",
                    Answer = @"[""Ora preparo""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
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
        /// <param name = "id"> </param>
        /// <param name = "step"> </param>
        /// <param name = "parentId"> </param>
        /// <returns> </returns>
        public static List<WebSpeechDto> GetRequestNotImplementedInsertEng(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""I enter""]]",
                    Answer = @"[""Do I post now?"",""Do I post now?"",""Do I add now?"",""Do I add now?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.GetAnswer.ToString()
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""no"",""not now"",""no thanks""]]",
                    Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + startId.ToString() + "]",
                    StepType = StepTypes.AddManually.ToString()
                }
            );

            id++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""yes"",""ok"",""yes alright"",""yes"",""ok"",""okay"",""ok go well""]]",
                    Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + startId.ToString() + "]"
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Posted""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + (id - 1).ToString() + "]",
                    StepType = StepTypes.AddNow.ToString()
                }
            );

            return result;
        }

        /// <summary>
        /// GetRequestNotImplementedAnswerEng
        /// </summary>
        /// <param name = "id"> </param>
        /// <param name = "step"> </param>
        /// <param name = "parentId"> </param>
        /// <returns> </returns>
        public static List<WebSpeechDto> GetRequestNotImplementedAnswerEng(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Reply""]]",
                    Answer = @"[""What should I answer?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
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
        /// <param name = "id"> </param>
        /// <param name = "step"> </param>
        /// <param name = "parentId"> </param>
        /// <returns> </returns>
        public static List<WebSpeechDto> GetRequestNotImplementedOtherEng(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++; result.Add(
               new WebSpeechDto()
               {
                   Id = id,
                   Name = WebSpeechTypes.SystemRequestNotImplemented.ToString() + "_" + id.ToString(),
                   Phrase = @"[[""Other""]]",
                   Answer = @"[""Now I prepare""]",
                   Host = "All",
                   FinalStep = true,
                   UserId = 0,
                   Order = 0,
                   Type = WebSpeechTypes.SystemRequest.ToString(),
                   SubType = WebSpeechTypes.SystemRequestNotImplemented.ToString(),
                   Step = step,
                   OperationEnable = true,
                   ParentIds = "[" + parentId.ToString() + "]"
               }
           );

            return result;
        }
    }
}