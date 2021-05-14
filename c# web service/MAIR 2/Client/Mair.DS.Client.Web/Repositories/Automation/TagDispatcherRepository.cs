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
    public class TagDispatcherRepository
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;

        public TagDispatcherRepository()
        {
            utility = new Utility();
        }


        /// <summary>
        /// Get Tags By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetTagDispatcherById(long id, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = String.Empty;

                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };
                    keyValuePairs["id"] = id.ToString();

                    var result = await utility.CallApi(HttpMethod.Get, Defaults.BaseUrl, "api/tagDispatcher/" + id.ToString(), null, token);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {

                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<string>(content);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }
    }
}