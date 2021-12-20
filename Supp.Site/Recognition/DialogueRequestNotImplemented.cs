using Supp.Models;
using System.Collections.Generic;
using System.Linq;

namespace Supp.Site.Recognition
{
    public class DialogueRequestNotImplemented
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="lastWebSpeechId"></param>
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
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""Mi dispiace, non conosco! Mi insegni?""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "",
                        StepType = StepTypes.Choice.ToString()
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""no"", ""non ora"", ""no grazie""]]",
                        Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                        Host = "All",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]",
                        StepType = StepTypes.Default.ToString()
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]]",
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]",
                        StepType = StepTypes.Default.ToString()
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""Cosa devo fare ?, Rispondere?, Altro?"",""Dimmi, cosa faccio ?, Rispondere?, Altro? ""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.Choice.ToString()
                    }
                );

                var parentId = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;

                var stepOfChoice = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = stepOfChoice;
                result.AddRange(AnswerIta(id, step, parentId));

                stepOfChoice = step;

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(InsertIta(id, step, id));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = stepOfChoice;
                result.AddRange(OtherIta(id, step, parentId));
            }

            if (culture.ToLower() == "en-us")
            {
                id = startId;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""I'm sorry, I don't know! Can you teach me?""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "",
                        StepType = StepTypes.Choice.ToString()
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""no"",""not now"",""no thanks""]]",
                        Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                        Host = "All",
                        FinalStep = true,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]",
                        StepType = StepTypes.Default.ToString()
                    }
                );

                id++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"[[""yes"",""ok"",""yes alright"",""yes"",""ok"",""okay"",""ok go well""]]",
                        Answer = null,
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (startId).ToString() + "]",
                        StepType = StepTypes.Default.ToString()
                    }
                );

                id++;
                step++;
                result.Add(
                    new WebSpeechDto()
                    {
                        Id = id,
                        Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                        Phrase = @"EMPTY",
                        Answer = @"[""What should I do ?, Answer ?, Other?"",""Tell me, what should I do ?, Answer ?, Other?""]",
                        Host = "All",
                        FinalStep = false,
                        UserId = 0,
                        Order = 0,
                        Type = WebSpeechTypes.SystemRequest.ToString(),
                        SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                        Step = step,
                        OperationEnable = true,
                        ParentIds = "[" + (id - 1).ToString() + "]",
                        StepType = StepTypes.Choice.ToString()
                    }
                );

                var parentId = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(AnswerEng(id, step, parentId));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(InsertEng(id, step, id));

                id = result.OrderByDescending(_ => _.Id).FirstOrDefault().Id;
                step = result.OrderByDescending(_ => _.Step).FirstOrDefault().Step;
                result.AddRange(OtherEng(id, step, parentId));
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Inserisco subito?"", ""Inserisco ora?"", ""Aggiungo subito?"", ""Aggiungo ora?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""no"", ""non ora"", ""no grazie""]]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""sì"",""ok"",""sì va bene"",""si"",""ok"",""si va bene"", ""ok va bene""]]",
                    Answer = @"[""ok"",""va bene"",""certo"",""bene""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Inserito""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Rispondere""]]",
                    Answer = @"[""Cosa devo rispondere?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.GetAnswer.ToString()
                }
            );

            return result;
        }

        /// <summary>
        /// OtherIta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WebSpeechDto> OtherIta(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++; result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Altro""]]",
                    Answer = @"[""Ora preparo""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.Default.ToString()
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Do I post now?"",""Do I post now?"",""Do I add now?"",""Do I add now?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""no"",""not now"",""no thanks""]]",
                    Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""yes"",""ok"",""yes alright"",""yes"",""ok"",""okay"",""ok go well""]]",
                    Answer = @"[""ok"",""ok"",""sure"",""ok""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
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
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"EMPTY",
                    Answer = @"[""Posted""]",
                    Host = "All",
                    FinalStep = true,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + (id - 1).ToString() + "]",
                    StepType = StepTypes.Default.ToString()
                }
            );

            return result;
        }

        /// <summary>
        /// AnswerEng
        /// </summary>
        /// <param name = "id"> </param>
        /// <param name = "step"> </param>
        /// <param name = "parentId"> </param>
        /// <returns> </returns>
        public List<WebSpeechDto> AnswerEng(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++;
            result.Add(
                new WebSpeechDto()
                {
                    Id = id,
                    Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                    Phrase = @"[[""Answer""]]",
                    Answer = @"[""What should I answer?""]",
                    Host = "All",
                    FinalStep = false,
                    UserId = 0,
                    Order = 0,
                    Type = WebSpeechTypes.SystemRequest.ToString(),
                    SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                    Step = step,
                    OperationEnable = true,
                    ParentIds = "[" + parentId.ToString() + "]",
                    StepType = StepTypes.GetAnswer.ToString()
                }
            );

            return result;
        }

        /// <summary>
        /// OtherEng
        /// </summary>
        /// <param name = "id"> </param>
        /// <param name = "step"> </param>
        /// <param name = "parentId"> </param>
        /// <returns> </returns>
        public List<WebSpeechDto> OtherEng(long id, int step, long parentId)
        {
            var result = new List<WebSpeechDto>() { };
            var startId = id + 1;

            id = startId;
            step++; result.Add(
               new WebSpeechDto()
               {
                   Id = id,
                   Name = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString() + "_" + id.ToString(),
                   Phrase = @"[[""Other""]]",
                   Answer = @"[""Now I prepare""]",
                   Host = "All",
                   FinalStep = true,
                   UserId = 0,
                   Order = 0,
                   Type = WebSpeechTypes.SystemRequest.ToString(),
                   SubType = WebSpeechTypes.SystemDialogueRequestNotImplemented.ToString(),
                   Step = step,
                   OperationEnable = true,
                   ParentIds = "[" + parentId.ToString() + "]",
                   StepType = StepTypes.Default.ToString()
               }
           );

            return result;
        }
    }
}
