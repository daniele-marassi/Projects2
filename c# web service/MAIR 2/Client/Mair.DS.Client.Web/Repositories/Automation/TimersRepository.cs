using Mair.DS.Client.Web.Common;
using Mair.DS.Client.Web.Models.Dto.Auth;
using Mair.DS.Client.Web.Models.Dto.Automation;
using Mair.DS.Client.Web.Models.Results;
using Mair.DS.Client.Web.Models.Results.Auth;
using Mair.DS.Client.Web.Models.Results.Automation;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
namespace Mair.DS.Client.Web.Repositories.Automation
{
    public class TimersRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public TimersRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get All Timers
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimerResult> GetAllTimers(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/timers", null, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TimerResult>(content);
                        response.Data.AddRange(JsonConvert.DeserializeObject<List<TimerDto>>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Get Timers By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimerResult> GetTimersById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/timers", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TimerResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TimerDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Update Timer
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimerResult> UpdateTimer(TimerDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Put, Defaults.BaseUrl, "api/timers", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TimerResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TimerDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Updated;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Add Timer
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimerResult> AddTimer(TimerDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Post, Defaults.BaseUrl, "api/timers", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TimerResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TimerDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Created;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        /// <summary>
        /// Delete Timer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimerResult> DeleteTimerById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TimerResult() { Data = new List<TimerDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Delete, Defaults.BaseUrl, "api/timers", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<TimerResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<TimerDto>(content));
                        response.Successful = true;
                        response.ResultState = ResultType.Deleted;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = ex.Message;
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }
    }
}