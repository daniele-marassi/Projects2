using AutoMapper;
using Mair.DigitalSuite.Dashboard.Common;
using Mair.DigitalSuite.Dashboard.Models;
using Mair.DigitalSuite.Dashboard.Models.Dto.Automation;
using Mair.DigitalSuite.Dashboard.Models.Param.Automation;
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
    public class PlcDataRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public PlcDataRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get PlcData
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PlcDataResult> GetPlcData(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/PlcData/GetPlcData", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<PlcDataResult>(content);

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
        /// Update PlcData
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PlcDataResult> UpdatePlcData(PlcDataDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new PlcDataResult() { Data = new List<PlcDataDto>(), ResultState = new ResultType() };

                try
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<PlcDataDto, PlcDataParam>());
                    var mapper = config.CreateMapper();
                    var param = mapper.Map<PlcDataParam>(dto);

                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in param.GetType().GetProperties())
                    {
                        if(prop.GetValue(param, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(param, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/PlcData/UpdatePlcData", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<PlcDataResult>(content);

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