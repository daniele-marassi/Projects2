using Supp.Models;
using System.Collections.Generic;
using System.Linq;

namespace Supp.Site.Recognition
{
    public class RequestAddToNote
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public List<WebSpeechDto> Get(string culture, long lastWebSpeechId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = lastWebSpeechId + 1;
            long id = 0;
            var step = 0;

            if (culture.ToLower() == "it-it")
            {
                id = startId;
                step++;

                var parentId = id;

                var stepOfChoice = step;

                id = startId;
                step = stepOfChoice;
                result.AddRange(AnswerIta(id, step, parentId));

                stepOfChoice = step;

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(InsertIta(id, step, id));

            }

            if (culture.ToLower() == "en-us")
            {

            }

            return result;
        }


        /// <summary>
        /// InsertIta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> InsertIta(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Inserisco""]]",
                    Answer = @"[""Inserisco subito?"", ""Inserisco ora?"", ""Aggiungo subito?"", ""Aggiungo ora?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.Choice.ToString()
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""no"", ""non ora"", ""no grazie""]]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
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
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + startId.ToString() + "]",
                    StepType = StepTypes.AddNow.ToString()
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Inserito""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + (id - 1).ToString() + "]",
                    StepType = StepTypes.Default.ToString()
                }
            );

            return result;
        }

        /// <summary>
        /// AnswerIta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> AnswerIta(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Dimmi il nome della nota"",""Qual'è il nome della nota?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
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
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Cosa devo aggiungere?"",""Dimmi cosa devo aggiungere""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.GetAnswer.ToString()
                }
            );

            return result;
        }

        /// <summary>
        /// InsertEng
        /// </summary>
        /// <param name = "id"> </param>
        /// <param name = "step"> </param>
        /// <param name = "parentId"> </param>
        /// <returns> </returns>
        public List<WebSpeechDto> InsertEng(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""I enter""]]",
                    Answer = @"[""Do I post now?"",""Do I post now?"",""Do I add now?"",""Do I add now?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.Choice.ToString()
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""no"",""not now"",""no thanks""]]",
                    Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
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
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""yes"",""ok"",""yes alright"",""yes"",""ok"",""okay"",""ok go well""]]",
                    Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + startId.ToString() + "]",
                    StepType = StepTypes.AddNow.ToString()
                }
            );

            id++;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemRequestAddToNote.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Posted""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemRequestAddToNote.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + (id - 1).ToString() + "]",
                    StepType = StepTypes.Default.ToString()
                }
            );

            return result;
        }
    }
}
