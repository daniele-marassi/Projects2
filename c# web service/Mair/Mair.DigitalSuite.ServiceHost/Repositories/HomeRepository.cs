using AutoMapper;
using Mair.DigitalSuite.ServiceHost.Common;
using Mair.DigitalSuite.ServiceHost.Repositories;
using Mair.DigitalSuite.ServiceHost.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Mair.DigitalSuite.ServiceHost.Models.Result;
using Mair.DigitalSuite.ServiceHost.Models.Dto;

namespace Mair.DigitalSuite.ServiceHost.Repositories
{
    public class HomeRepository : IHomeRepository, IDisposable
    {

        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public HomeRepository()
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// Get All Api
        /// </summary>
        /// <param name="request"></param>
        /// <param name="nameSpaceControllers"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetAllApi(HttpRequest request, string nameSpaceControllers)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ApiResult() { Data = new List<ApiDto>(), ResultState = new ResultType() };

                try
                {
                    var data = new List<ApiDto>() { };
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    Assembly asm = Assembly.GetExecutingAssembly();

                    var controllers = asm.GetTypes()
                            .Where(type => typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(type))
                            .Select(x => new { Controller = x.Name })
                            .OrderBy(x => x.Controller)
                            .ToList();

                    foreach (var controller in controllers)
                    {
                        Type type = Type.GetType($"{nameSpaceControllers}.{controller.Controller}");

                        string pathApi = "api";

                        try
                        {
                            pathApi = type.CustomAttributes.Where(_ => _.AttributeType.Name == "RouteAttribute").FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString().Replace("/[controller]", "");
                        }
                        catch (Exception)
                        {
                        }

                        MethodInfo[] methodInfoList = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                        foreach (var methodInfo in methodInfoList)
                        {
                            var customAttributeHttp = methodInfo.CustomAttributes.Where(_ =>
                                _.AttributeType.Name == typeof(HttpPutAttribute).Name
                                || _.AttributeType.Name == typeof(HttpGetAttribute).Name
                                || _.AttributeType.Name == typeof(HttpPostAttribute).Name
                                || _.AttributeType.Name == typeof(HttpDeleteAttribute).Name
                            );

                            var customAttributeRoles = methodInfo.CustomAttributes.Where(_ =>
                                _.AttributeType.Name == typeof(AuthorizeAttribute).Name
                            );
                            string roles = String.Empty;
                            string actionType = String.Empty;
                            string parameters = String.Empty;
                            string controllerName = String.Empty;
                            string apiName = String.Empty;

                            try
                            {
                                roles = customAttributeRoles.FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue.Value.ToString();
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                actionType = customAttributeHttp.FirstOrDefault().AttributeType.Name.Replace("Http", "").Replace("Attribute", "");
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                foreach (ParameterInfo pParameter in methodInfo.GetParameters().OrderBy(_ => _.Position).ToList())
                                {
                                    if (parameters != String.Empty) parameters += ", ";
                                    parameters += pParameter.ParameterType.Name + " " + pParameter.Name;
                                }
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                controllerName = methodInfo.DeclaringType.Name.Replace("Controller", "");
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                apiName = customAttributeHttp.FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString();
                            }
                            catch (Exception)
                            {
                            }

                            string url = $"{request.Scheme}://{request.Host}{request.PathBase}";

                            data.Add(new ApiDto() { Url = $"{url}/{pathApi}/{controllerName}/{apiName}", Parameters = parameters, ActionType = actionType, Roles = roles });
                        }
                    }

                    if (data != null)
                    {
                        response.Data.AddRange(data);
                        response.Successful = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.ResultState = ResultType.Error;
                    response.Message = "";
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }
    }
}
