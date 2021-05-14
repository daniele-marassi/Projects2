using Mair.DigitalSuite.Dashboard.Common;
using Mair.DigitalSuite.Dashboard.Models;
using Mair.DigitalSuite.Dashboard.Models.Dto.Auth;
using Mair.DigitalSuite.Dashboard.Models.Result;
using Mair.DigitalSuite.Dashboard.Models.Result.Auth;
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
    public class UserRoleTypesRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public UserRoleTypesRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get All UserRoleTypes
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> GetAllUserRoleTypes(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/UserRoleTypes/GetAllUserRoleTypes", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserRoleTypeResult>(content);

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
        /// Get UserRoleTypes By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> GetUserRoleTypesById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("Id", id.ToString()));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/UserRoleTypes/GetUserRoleType", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserRoleTypeResult>(content);

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
        /// Update UserRoleType
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> UpdateUserRoleType(UserRoleTypeDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if(prop.GetValue(dto, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(dto, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Put, GeneralSettings.Static.BaseUrl, "api/UserRoleTypes/UpdateUserRoleType", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserRoleTypeResult>(content);

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
        /// Add UserRoleType
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> AddUserRoleType(UserRoleTypeDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if(prop.GetValue(dto, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(dto, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/UserRoleTypes/AddUserRoleType", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserRoleTypeResult>(content);

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
        /// Delete UserRoleType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserRoleTypeResult> DeleteUserRoleTypeById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("Id", id.ToString()));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/UserRoleTypes/DeleteUserRoleType", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserRoleTypeResult>(content);

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