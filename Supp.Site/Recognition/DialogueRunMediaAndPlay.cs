using Additional;
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
    public class DialogueRunMediaAndPlay
    {
        private PhraseInDateTimeManager phraseInDateTimeManager;
        public DialogueRunMediaAndPlay()
        {
            phraseInDateTimeManager = new PhraseInDateTimeManager();
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
                        StepType = StepTypes.Default.ToString(),
                        ElementIndex = 0
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
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.GetElementValue.ToString(),
                        ElementIndex = 1
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
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.Execute.ToString(),
                        ElementIndex = 0
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
                        StepType = StepTypes.Default.ToString(),
                        ElementIndex = 0
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
                        StepType = StepTypes.Default.ToString(),
                        ElementIndex = 0
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
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.GetElementValue.ToString(),
                        ElementIndex = 1
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
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.Execute.ToString(),
                        ElementIndex = 0
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
                        StepType = StepTypes.Default.ToString(),
                        ElementIndex = 0
                    }
                );
            }

            return result;
        }

        /// <summary>
        /// Run Media And Play
        /// </summary>
        /// <param name="data"></param>
        /// <param name="_phrase"></param>
        /// <param name="_hostSelected"></param>
        /// <param name="access_token_cookie"></param>
        /// <param name="executionQueueRepo"></param>
        /// <returns></returns>
        public async Task<WebSpeechDto> RunMediaAndPlay(WebSpeechDto data, string _phrase, string _hostSelected, string access_token_cookie, ExecutionQueuesRepository executionQueueRepo, TokenDto identification)
        {
            if (data.Type == WebSpeechTypes.RunMediaAndPlayWithNumericParameter.ToString() || data.Type == WebSpeechTypes.SystemRunMediaAndPlayWithNumericParameter.ToString())
            {
                var value = 0;
                if (_phrase == null || _phrase == "") _phrase = data.Phrase;
                if (_phrase == null) _phrase = "";
                var words = _phrase.Trim().ToLower().Split(' ');
                foreach (var word in words)
                {
                    try
                    {
                        value = int.Parse(word);
                        if (data.Parameters == null) data.Parameters = String.Empty;
                        if (data.Parameters != "") data.Parameters += " ";
                        data.Parameters += value.ToString();
                    }
                    catch (Exception)
                    { }
                }
            }

            if (data.Type == WebSpeechTypes.RunMediaAndPlayWithNotNumericParameter.ToString() || data.Type == WebSpeechTypes.SystemRunMediaAndPlayWithNotNumericParameter.ToString())
            {
                if (_phrase == null || _phrase == "") _phrase = data.Phrase;

                try
                {
                    if (data.Parameters == null) data.Parameters = String.Empty;
                    data.Parameters += _phrase.ToString();
                }
                catch (Exception)
                { }
            }

            var scheduledDateTime = DateTime.Now;

            if (_phrase != null && _phrase != "")
            {
                var value = phraseInDateTimeManager.Convert(_phrase, JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).General.Culture);
                if (value != null) scheduledDateTime = (DateTime)value;
            }

            var hostSelected = _hostSelected;

            if (data.Host.Trim().ToLower() != "all") hostSelected = data.Host;

            var executionQueue = new ExecutionQueueDto() { FullPath = data.Operation, Arguments = data.Parameters, Host = hostSelected, Type = WebSpeechTypes.RunMediaAndPlay.ToString(), WebSpeechId = data.Id, ScheduledDateTime = scheduledDateTime, StateQueue = ExecutionQueueStateQueue.NONE.ToString() };
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
