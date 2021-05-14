
using Mair.DS.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Mair.DS.Services.Token
{
    public class CheckCredentials
    {
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public CheckCredentials()
        {
             
        }

        /// <summary>
        /// Validate Token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                ClaimsPrincipal principal = new ClaimsPrincipal() { };
                try
                {
                    if (jwtToken != null)
                    {
                        IdentityModelEventSource.ShowPII = true;

                        SecurityToken validatedToken;
                        TokenValidationParameters validationParameters = new TokenValidationParameters();

                        validationParameters.ValidateLifetime = true;
                        validationParameters.ValidIssuer = Defaults.JwtAppSettingOptions["JwtIssuer"];
                        validationParameters.ValidAudience = Defaults.JwtAppSettingOptions["JwtIssuer"];
                        validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Defaults.JwtAppSettingOptions["JwtKey"]));

                        principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return principal;
            }
        }

        /// <summary>
        /// Check Hub Authorization
        /// </summary>
        /// <param name="context"></param>
        /// <param name="accessToken"></param>
        /// <param name="hubPath"></param>
        /// <param name="roles"></param>
        public void CheckHubAuthorization(MessageReceivedContext context, string accessToken, string hubPath, List<string> roles)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var claimsPrincipal = ValidateToken(accessToken);
                var claims = claimsPrincipal.Claims.FirstOrDefault();
                var claimValues = new Dictionary<string, string>() { };
                var isAuthenticated = false;

                if (claims != null && claims.Subject != null)
                {
                    foreach (var claim in claims.Subject.Claims)
                    {
                        var key = claim.Type.ToString().Split('/').Last(); ;

                        if (claimValues.ContainsKey(key))
                            claimValues[key] += ", " + claim.Value.ToString();
                        else
                            claimValues.Add(key, claim.Value.ToString());
                    }

                    isAuthenticated = claims.Subject.IsAuthenticated;
                }

                var validRoles = false;
                if (claimValues.ContainsKey("role"))
                { 
                    foreach (var role in roles)
                    {
                        if (claimValues["role"].Contains(role)) validRoles = true;
                    }
                }

                var path = context.HttpContext.Request.Path;
                if (isAuthenticated
                    && validRoles
                    && (path.StartsWithSegments(hubPath)))
                {
                    context.Token = accessToken;
                }
                else if (isAuthenticated && path.StartsWithSegments(hubPath))
                {
                    throw new Exception("Not Authorized!");
                }
                else if (!isAuthenticated && path.StartsWithSegments(hubPath))
                {
                    throw new Exception("Not Authenticated!");
                }
            }
        }
    }
}
