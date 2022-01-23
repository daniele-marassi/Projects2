using GoogleManagerModels;
using Supp.Models;
using Supp.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Recognition
{
    public class DialogueClearNote
    {
        private readonly WebSpeechesRepository webSpeecheRepo;

        public DialogueClearNote()
        {
            webSpeecheRepo = new WebSpeechesRepository();
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
                if (_subType == WebSpeechTypes.SystemDialogueClearNote.ToString())
                {
                    id = startId;
                    step++;
                    result.Add(
                        new WebSpeechDto()
                        {
                            Id = id,
                            Name = _subType + "_" + id.ToString(),
                            Phrase = @"EMPTY",
                            Answer = @"[""Dimmi il nome della nota"",""Qual'è il nome della nota?""]",
                            Host = "All",
                            FinalStep = false,
                            UserId = 0,
                            Order = 0,
                            Type = WebSpeechTypes.SystemRequest.ToString(),
                            SubType = _subType,
                            Step = step,
                            OperationEnable = true,
                            ParentIds = "",
                            StepType = StepTypes.Ask.ToString(),
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
                }

                if (_subType == WebSpeechTypes.SystemDialogueClearNote.ToString()) id++;
                if (_subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()) id = startId;
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
                        ParentIds = _subType == WebSpeechTypes.SystemDialogueClearNote.ToString() ? "[" + (id - 1).ToString() + "]" : "",
                        StepType = StepTypes.ApplyNow.ToString(),
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
                        Answer = @"[""Pulito"",""svuotato""]",
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
                if (_subType == WebSpeechTypes.SystemDialogueClearNote.ToString())
                {
                    id = startId;
                    step++;
                    result.Add(
                        new WebSpeechDto()
                        {
                            Id = id,
                            Name = _subType + "_" + id.ToString(),
                            Phrase = @"EMPTY",
                            Answer = @"[""Tell me the name of the note"",""What is the name of the note?""]",
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
                }

                if (_subType == WebSpeechTypes.SystemDialogueClearNote.ToString()) id++;
                if (_subType == WebSpeechTypes.SystemDialogueClearNoteWithName.ToString()) id = startId;
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
                        ParentIds = _subType == WebSpeechTypes.SystemDialogueClearNote.ToString() ? "[" + (id - 1).ToString() + "]" : "",
                        StepType = StepTypes.ApplyNow.ToString(),
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
                        Answer = @"[""Clean"",""emptied""]",
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

        public async Task<EventResult> ClearNote(WebSpeechDto dto, string token, string userName, long userId)
        {
            var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            var editCalendarEventRequest = new EditCalendarEventRequest() { SummaryToSearch = dto.Elements[1].Value, Description = String.Empty, TimeMax = timeMax, TimeMin = timeMin, DescriptionAppended = false };

            var getRemindersResult = await webSpeecheRepo.EditLastReminder(token, userName, userId, WebSpeechTypes.EditNote, editCalendarEventRequest);

            return getRemindersResult;
        }
    }
}
