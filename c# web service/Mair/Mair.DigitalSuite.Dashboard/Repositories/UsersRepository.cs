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
    public class UsersRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public UsersRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserResult> GetAllUsers(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserResult() { Data = new List<UserDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Users/GetAllUsers", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserResult>(content);

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
        /// Get Users By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserResult> GetUsersById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserResult() { Data = new List<UserDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("Id", id.ToString()));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Get, GeneralSettings.Static.BaseUrl, "api/Users/GetUser", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserResult>(content);

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
        /// Update User
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserResult> UpdateUser(UserDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserResult() { Data = new List<UserDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if(prop.GetValue(dto, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(dto, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Put, GeneralSettings.Static.BaseUrl, "api/Users/UpdateUser", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserResult>(content);

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
        /// Add User
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserResult> AddUser(UserDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserResult() { Data = new List<UserDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if(prop.GetValue(dto, null) != null) keyValuePairs.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(dto, null).ToString()));
                    }

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Post, GeneralSettings.Static.BaseUrl, "api/Users/AddUser", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserResult>(content);

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
        /// Delete User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserResult> DeleteUserById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new UserResult() { Data = new List<UserDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new List<KeyValuePair<string, string>>() { };

                    keyValuePairs.Add(new KeyValuePair<string, string>("Id", id.ToString()));

                    var parameters = new FormUrlEncodedContent(keyValuePairs);

                    var result = await utility.CallApi(HttpMethod.Delete, GeneralSettings.Static.BaseUrl, "api/Users/DeleteUser", parameters, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<UserResult>(content);

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