using Additional;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Supp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Supp.ServiceHost.Common
{
    public class SuppUtility
    {
        private Utility utility;

        public SuppUtility()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Restart Service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <returns></returns>
        public bool RestartService(string serviceName, int timeoutMilliseconds)
        {
            var result = true;
            var service = new ServiceController(serviceName);
            try
            {
                var millisec1 = Environment.TickCount;
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                try
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                catch (Exception)
                {
                }

                // count the rest of the timeout
                var millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch(Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="additionalKeys"></param>
        /// <returns></returns>
        public string CreateToken(long userId, List<string> additionalKeys)
        {
            var result = "";

            var guid = DateTime.Now.ToString("yyyyMMddHHmmssffftt");
            if (additionalKeys == null) additionalKeys = new List<string>() { };

            using (MD5 md5Hash = MD5.Create())
            {
                for (int i = 0; i < additionalKeys.Count(); i++)
                {
                    additionalKeys[i] = utility.GetMd5Hash(md5Hash, additionalKeys[i]);
                }

                additionalKeys.Add(utility.GetMd5Hash(md5Hash, guid));
            }

            var code = "";

            if (additionalKeys.Count > 1)
            {
                for (int i = 0; i < additionalKeys.Count(); i++)
                {
                    if (i < additionalKeys.Count() - 1)
                    {
                        code += additionalKeys[i].Substring(0, 16);
                        code += additionalKeys[i + 1].Substring(0, 16);
                        code += additionalKeys[i].Substring(16, 16);
                        code += additionalKeys[i + 1].Substring(16, 16);
                    }
                }
            }
            else
                code = additionalKeys[0];

            result = userId.ToString() + "|" + code;

            return result;
        }

        /// <summary>
        /// Check Authorizations
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="requiredRolesSeparatedByComma"></param>
        /// <returns></returns>
        public static (bool IsAuthorized, string Message, TokenDto Identification) CheckAuthorizations(IHeaderDictionary headers, string requiredRolesSeparatedByComma)
        {
            (bool IsAuthorized, string Message, TokenDto Identification) result;
            result.IsAuthorized = true;
            result.Message = null;
            result.Identification = null;

            try
            {
                var token = headers.Where(_ => _.Key.Trim().ToLower() == "Authorization".Trim().ToLower()).FirstOrDefault().Value.ToString();

                if (token == null) token = "";

                token = token.Replace("Bearer", "", System.StringComparison.InvariantCultureIgnoreCase).Trim();

                if (token == null) token = "";

                long userId = 0;
                
                long.TryParse(token.Split("|")[0], out userId);
                TokenDto tokenDto = null;

                if (Program.TokensArchive.ContainsKey(userId))
                    tokenDto = Program.TokensArchive[userId];

                if (tokenDto == null || token != tokenDto.TokenCode)
                {
                    result.IsAuthorized = false;
                    result.Message = "Not Authorizated! - Invalid Token!";
                }
                else
                {
                    if (tokenDto.ExpiryDate != null && tokenDto.ExpiryDate <= DateTime.Now)
                    {
                        result.IsAuthorized = false;
                        result.Message = "Not Authorizated! - Token Expired!";
                    }

                    if (requiredRolesSeparatedByComma != null && requiredRolesSeparatedByComma != "" && result.IsAuthorized)
                    {
                        result.IsAuthorized = false;

                        if (tokenDto.Roles != null)
                        {
                            var userRoles = tokenDto.Roles;

                            if (userRoles != null && userRoles.Count > 0)
                            {
                                var requiredRoles = requiredRolesSeparatedByComma.Split(",");

                                foreach (var role in requiredRoles)
                                {
                                    if (userRoles.Contains(role.Trim())) result.IsAuthorized = true;
                                }
                            }
                        }

                        if (!result.IsAuthorized) result.Message = "Not Authorizated! - Insufficient Permissions!";
                    }
                }

                result.Identification = tokenDto;
            }
            catch (System.Exception ex)
            {
                result.IsAuthorized = false;
                result.Message = "Not Authorizated! - Error:" + ex.Message;
            }

            return result;
        }

        public static MethodBase GetMethod(MethodBase method)
        {
            MethodBase _method = null;
            try
            {
                var memberType = method.DeclaringType.MemberType;
                var methodName = String.Empty;
                var fullClassName = String.Empty;
                var start = 0;
                var end = 0;

                if (memberType == MemberTypes.NestedType)
                {
                    var fullName = method.DeclaringType.FullName;

                    start = 0;
                    end = fullName.IndexOf("+");
                    fullClassName = fullName.Substring(start, end - start);

                    start = fullName.IndexOf("<") + 1;
                    end = fullName.IndexOf(">");
                    methodName = fullName.Substring(start, end - start);

                    _method = new StackTrace()
                            .GetFrames()
                            .Select(frame => frame.GetMethod())
                            .Where(_ => _.Name == methodName && _.DeclaringType.FullName == fullClassName)
                            .FirstOrDefault();
                }

                if (memberType != MemberTypes.NestedType || _method == null)
                {
                    _method = method;
                }
            }
            catch (Exception)
            {
                _method = method;
                //throw;
            }

            return _method;
        }

        public static string GetRoles(MethodBase method) 
        {
            var result = "";
            var methodInfo = SuppUtility.GetMethod(method);

            var customAttributeRoles = methodInfo.CustomAttributes.Where(_ =>
                _.AttributeType.Name == nameof(CustomAttribute)
            );

            try
            {
                result = customAttributeRoles.FirstOrDefault().ConstructorArguments[1].Value.ToString();
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
