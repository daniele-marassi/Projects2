using GoogleManagerModels;
using Supp.Models;
using Supp.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Recognition
{
    public class DialogueAddToNote
    {
        private readonly WebSpeechesRepository webSpeecheRepo;

        public DialogueAddToNote()
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
                if (_subType == WebSpeechTypes.SystemDialogueAddToNote.ToString())
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
                            StepType = StepTypes.GetElementName.ToString()
                        }
                    );
                }

                if (_subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()) id++;
                if (_subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()) id = startId;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = _subType + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""Cosa devo aggiungere?"",""Dimmi cosa devo aggiungere""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString() ? "[" + (id - 1).ToString() + "]" : "",
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
                        Answer = @"[""Inserito""]",
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
                        StepType = StepTypes.GetElementName.ToString()
                    }
                );

                if (_subType == WebSpeechTypes.SystemDialogueAddToNote.ToString()) id++;
                if (_subType == WebSpeechTypes.SystemDialogueAddToNoteWithName.ToString()) id = startId;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = _subType + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""What should I add?"",""Tell me what should I add""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = _subType,
                        Step = step,
                        OperationEnable = true,
                        ParentIds = _subType == WebSpeechTypes.SystemDialogueAddToNote.ToString() ? "[" + (id - 1).ToString() + "]" : "",
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
                        Answer = @"[""Posted""]",
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

        public async Task<EventResult> AddElementInNote(WebSpeechDto dto, string token, string userName, long userId)
        {
            var timeMin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var timeMax = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            var editCalendarEventRequest = new EditCalendarEventRequest() { SummaryToSearch = dto.ElementName, Description = "\n"+dto.ElementValue, TimeMax = timeMax, TimeMin = timeMin, DescriptionAppended = true };

            var getRemindersResult = await webSpeecheRepo.EditLastReminder(token, userName, userId, WebSpeechTypes.EditNote, editCalendarEventRequest);

            return getRemindersResult;
        }
    }
}
