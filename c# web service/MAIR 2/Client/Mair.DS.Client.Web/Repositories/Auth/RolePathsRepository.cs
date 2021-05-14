using Mair.DS.Client.Web.Common;
using Mair.DS.Client.Web.Models.Dto.Auth;
using Mair.DS.Client.Web.Models.Results;
using Mair.DS.Client.Web.Models.Results.Auth;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;


namespace Mair.DS.Client.Web.Repositories.Auth
{
    public class RolePathsRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public RolePathsRepository()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get All RolePaths
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RolePathResult> GetAllRolePaths(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RolePathResult() { Data = new List<RolePathDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/rolePaths", null, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<RolePathResult>(content);
                        response.Data.AddRange(JsonConvert.DeserializeObject<List<RolePathDto>>(content));
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
        /// Get RolePaths By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RolePathResult> GetRolePathsById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RolePathResult() { Data = new List<RolePathDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/rolePaths", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<RolePathResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<RolePathDto>(content));
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
        /// Update RolePath
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RolePathResult> UpdateRolePath(RolePathDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RolePathResult() { Data = new List<RolePathDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Put, Defaults.BaseUrl, "api/rolePaths", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<RolePathResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<RolePathDto>(content));
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
        /// Add RolePath
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RolePathResult> AddRolePath(RolePathDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RolePathResult() { Data = new List<RolePathDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Post, Defaults.BaseUrl, "api/rolePaths", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<RolePathResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<RolePathDto>(content));
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
        /// Delete RolePath By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RolePathResult> DeleteRolePathById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RolePathResult() { Data = new List<RolePathDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Delete, Defaults.BaseUrl, "api/rolePaths", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<RolePathResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<RolePathDto>(content));
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