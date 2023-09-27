using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supp.Models;
using Supp.Site.Repositories;
using Supp.Site.Common;
using System.Reflection;
using NLog;
using X.PagedList;
using static Supp.Site.Common.Config;
using Additional.NLog;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Diagnostics;
using System.Management;
using System.Globalization;

using System.Threading;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Web;
using Additional;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.IO;
using NLog.Time;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using NLog.Fluent;
using static System.Net.Mime.MediaTypeNames;
using Google.Apis.Calendar.v3.Data;
using NuGet.Frameworks;

namespace Supp.Site.Controllers
{
    public class TokensController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly TokensRepository tokensRepo;

        public TokensController()
        {
            tokensRepo = new TokensRepository();
        }

        // GET: Tokens/TokenIsValid
        public async Task<bool> TokenIsValid()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                bool result = false;

                try
                {
                    var headers = HttpContext.Request.Headers;
                    var token = headers.Where(_ => _.Key.Trim().ToLower() == "Authorization".Trim().ToLower()).FirstOrDefault().Value.ToString();

                    if (token == null) token = "";

                    token = token.Replace("Bearer", "", System.StringComparison.InvariantCultureIgnoreCase).Trim();

                    if (token == null) token = "";

                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var tokenIsValidResult = await tokensRepo.TokenIsValid(token);

                    result = (bool)tokenIsValidResult.Value;

                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    result = false;
                }

                return result;
            }
        }
    }
}