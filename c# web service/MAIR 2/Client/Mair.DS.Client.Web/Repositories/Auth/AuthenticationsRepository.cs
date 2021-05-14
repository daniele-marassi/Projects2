using Mair.DS.Client.Web.AuthenticationService;
using Mair.DS.Client.Web.Common;
using Mair.DS.Client.Web.Models.Dto.Auth;
using Mair.DS.Client.Web.Models.Results;
using Mair.DS.Client.Web.Models.Results.Auth;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;


namespace Mair.DS.Client.Web.Repositories.Auth
{
    public class AuthenticationsRepository
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public AuthenticationsRepository()
        {
            utility = new Utility();
        }

        public async Task<TokenResult> Login(string userName, string password)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["UserName"] = userName;
                    keyValuePairs["Password"] = password;

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/authentications/GetToken", keyValuePairs, null);

                    var content = await result.Content.ReadAsStringAsync();

                    //if (content == null || content == String.Empty)
                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<TokenResult>(content);
                        if (response.IsAuthenticated)
                        {
                            var data = response.Data.FirstOrDefault();
                            AuthStateProvider authStateProvider = new AuthStateProvider();
                            authStateProvider.NotifyAuthenticationStateChanged(data.UserName, data.UserId, data.Roles);
                        }
                    }  
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.IsAuthenticated = false;
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
        /// Get All Authentications
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAllAuthentications(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/authentications", null, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<AuthenticationResult>(content);
                        response.Data.AddRange(JsonConvert.DeserializeObject<List<AuthenticationDto>>(content));
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
        /// Get Authentications By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAuthenticationsById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/authentications", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<AuthenticationResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<AuthenticationDto>(content));
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
        /// Update Authentication
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> UpdateAuthentication(AuthenticationDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Put, Defaults.BaseUrl, "api/authentications", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<AuthenticationResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<AuthenticationDto>(content));
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
        /// Add Authentication
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> AddAuthentication(AuthenticationDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    foreach (var prop in dto.GetType().GetProperties())
                    {
                        if (prop.GetValue(dto, null) != null) keyValuePairs[prop.Name] = prop.GetValue(dto, null).ToString();
                    }

                    var result = await utility.CallApi(HttpMethod.Post, Defaults.BaseUrl, "api/authentications", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<AuthenticationResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<AuthenticationDto>(content));
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
        /// Delete Authentication By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> DeleteAuthenticationById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["Id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Delete, Defaults.BaseUrl, "api/authentications", keyValuePairs, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += result.ReasonPhrase;
                    }
                    else
                    {
                        //response = JsonConvert.DeserializeObject<AuthenticationResult>(content);
                        response.Data.Add(JsonConvert.DeserializeObject<AuthenticationDto>(content));
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