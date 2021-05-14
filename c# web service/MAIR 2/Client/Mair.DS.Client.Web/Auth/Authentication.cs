using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Mair.DS.Client.Web.Common;
using System.Reflection;
using Mair.DS.Client.Web.Models.Dto.Auth;
using System.Net.Http;
using Mair.DS.Client.Web.Models.Results.Auth;
using Mair.DS.Client.Web.Models.Results;
using Newtonsoft.Json;

namespace Mair.DS.Client.Web.Auth
{
    public class Authentication
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();

        public async Task<TokenResult> Login(string userName, string password)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = new ResultType() };
                try
                {
                    var utility = new Utility();

                    //create parameters
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["UserName"] = userName;
                    keyValuePairs["Password"] = password;
                    var parameters = JsonConvert.SerializeObject(keyValuePairs);

                    //calla api
                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/tagdispatcher/GetToken", keyValuePairs, null);

                    var content = await result.Content.ReadAsStringAsync();

                    if (content == null || content == String.Empty)
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
                }
                return response;
            }
        }
    }
}
