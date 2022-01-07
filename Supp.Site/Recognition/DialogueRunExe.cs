using GoogleManagerModels;
using Newtonsoft.Json;
using Supp.Models;
using Supp.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Recognition
{
    public class DialogueRunExe
    {
        public DialogueRunExe()
        {

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
        /// <param name="_subType"></param>
        /// <returns></returns>
        public List<WebSpeechDto> Get(string culture, long lastWebSpeechId, string _subType)
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
                        Name = _subType + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""Dimmi il valore"",""Qual'è il valore?""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "",
                        StepType = StepTypes.GetElementValue.ToString()
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = _subType + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""ok"",""va bene""]",
                        Host = "All",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.Default.ToString()
                    }
                );
            }

            if (culture.ToLower() == "en-us")
            {
                id = startId;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = _subType + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""Tell me the value"",""What is the value?""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "",
                        StepType = StepTypes.GetElementValue.ToString()
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = _subType + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""ok"",""okay""]",
                        Host = "All",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.Default.ToString()
                    }
                );
            }

            return result;
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
            if (data.Type == WebSpeechTypes.RunExeWithNumericParameter.ToString() || data.Type == WebSpeechTypes.SystemRunExeWithNumericParameter.ToString())
            {
                var value = 0;
                if (_phrase == null || _phrase == "") _phrase = data.Phrase;

                var words = _phrase.Trim().ToLower().Split(' ');
                foreach (var word in words)
                {
                    try
                    {
                        value = int.Parse(word);
                        if (data.Parameters == null) data.Parameters = String.Empty;
                        data.Parameters += value.ToString();
                    }
                    catch (Exception)
                    { }
                }
            }

            if (data.Type == WebSpeechTypes.RunExeWithNotNumericParameter.ToString() || data.Type == WebSpeechTypes.SystemRunExeWithNotNumericParameter.ToString())
            {
                var value = 0;
                if (_phrase == null || _phrase == "") _phrase = data.Phrase;

                try
                {
                    if (data.Parameters == null) data.Parameters = String.Empty;
                    data.Parameters += _phrase.ToString();
                }
                catch (Exception)
                { }
            }

            var hostSelected = _hostSelected;

            if (data.Host.Trim().ToLower() != "all") hostSelected = data.Host;

            var executionQueue = new ExecutionQueueDto() { FullPath = data.Operation, Arguments = data.Parameters, Host = hostSelected, Type = WebSpeechTypes.RunExe.ToString() };
            var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);

            if (addExecutionQueueResult.Successful)
            {
                data.ExecutionQueueId = addExecutionQueueResult.Data.FirstOrDefault().Id;
            }
            else
                data.ExecutionQueueId = 0;

            return data;
        }
    }
}
