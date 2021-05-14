using Mair.DigitalSuite.Dashboard.Common;
using Mair.DigitalSuite.Dashboard.Models;
using Mair.DigitalSuite.Dashboard.Models.Dto.Automation;
using Mair.DigitalSuite.Dashboard.Models.Result;
using Mair.DigitalSuite.Dashboard.Models.Result.Automation;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static Mair.DigitalSuite.Dashboard.Common.Config;

namespace Mair.DigitalSuite.Dashboard.Repositories
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
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Timers/GetAllTimers", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TimerResult>(content);

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
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("Id", id.ToString()));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Timers/GetTimer", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TimerResult>(content);

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
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if(prop.GetValue(dto, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(dto, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Put, GeneralSettings.Static.BaseUrl, "api/Timers/UpdateTimer", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TimerResult>(content);

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
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if(prop.GetValue(dto, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(dto, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/Timers/AddTimer", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TimerResult>(content);

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
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("Id", id.ToString()));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/Timers/DeleteTimer", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TimerResult>(content);

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