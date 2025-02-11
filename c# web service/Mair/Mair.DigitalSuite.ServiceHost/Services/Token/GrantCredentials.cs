﻿using AutoMapper;
using Mair.DigitalSuite.ServiceHost.Common;
using Mair.DigitalSuite.ServiceHost.Repositories;
using Mair.DigitalSuite.ServiceHost.Models;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Mair.DigitalSuite.ServiceHost.Common.Config;
using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Contexts;
using System.Configuration;
using Microsoft.IdentityModel.Logging;
using Mair.DigitalSuite.ServiceHost.Models.Result.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Dto;
using Mair.DigitalSuite.ServiceHost.Models.Result;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Auth;

namespace Mair.DigitalSuite.ServiceHost.Services.Token
{
    public class GrantCredentials
    {
        private IAuthenticationsRepository iAuthenticationsRepo;
        private IUsersRepository iUsersRepo;
        private IUserRoleTypesRepository iUserRoleTypesRepo;
        private IUserRolesRepository iUserRolesRepo;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public GrantCredentials(MairDigitalSuiteDatabaseContext context)
        {
            iAuthenticationsRepo = new AuthenticationsRepository(context);
            iUsersRepo = new UsersRepository(context);
            iUserRoleTypesRepo = new UserRoleTypesRepository(context);
            iUserRolesRepo = new UserRolesRepository(context);
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<TokenResult> GetToken(string userName, string password)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>(), ResultState = new ResultType() };

                try
                {
                    var getAuthenticationsResult = await iAuthenticationsRepo.GetAuthenticationsByUserName(userName);

                    if (getAuthenticationsResult.Data.Count == 0)
                    {
                        throw new Exception($"Invalid grant - Provided username not found! {getAuthenticationsResult.OriginalException!.Message}");
                    }

                    var authentication = getAuthenticationsResult.Data.OrderBy(_ => _.Id).LastOrDefault();
                    
                    var getUserResult = await iUsersRepo.GetUsersById(authentication!.UserId);

                    if (getUserResult.Data.Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getUserResult)}! {getUserResult.OriginalException.Message}");
                    }

                    var user = getUserResult.Data.FirstOrDefault();

                    var getUserRoleResult = await iUserRolesRepo.GetAllUserRoles();

                    if (getUserRoleResult.Data.Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getUserRoleResult)}! {getUserRoleResult.OriginalException.Message}");
                    }

                    var userRoleTypeIds = getUserRoleResult.Data.Where(_ => _.UserId == user.Id).Select(_ => _.UserRoleTypeId).ToList();

                    var getUserRoleTypeResult = await iUserRoleTypesRepo.GetAllUserRoleTypes();

                    if (getUserRoleTypeResult.Data.Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getUserRoleTypeResult)}! {getUserRoleTypeResult.OriginalException.Message}");
                    }

                    var roles = getUserRoleTypeResult.Data.Where(_ => userRoleTypeIds.Contains(_.Id)).Select(_ => _.Type).ToList();

                    var passwordMd5 = "";

                    var now = DateTime.Now.Date;

                    using (MD5 md5Hash = MD5.Create())
                    {
                        passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, password);
                    }

                    var createdAt = authentication.Created;
                    createdAt = new DateTime(createdAt.Year, createdAt.Month, createdAt.Day, 0, 0, 0);

                    var passwordExpirationDate = createdAt.AddDays(authentication.PasswordExpirationDays);

                    var daysToAlert = 3;

                    var alertPasswordExpirationDate = createdAt.AddDays(authentication.PasswordExpirationDays - daysToAlert);

                    //check user exists
                    if (userName.Trim().ToLower() == user.UserName.ToLower() && passwordMd5 == authentication.Password && authentication.Enable && ((authentication.PasswordExpiration && passwordExpirationDate >= now) || !authentication.PasswordExpiration))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GeneralSettings.Static.JwtAppSettingOptions["JwtKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expires = DateTime.Now.AddDays(Convert.ToDouble(GeneralSettings.Static.JwtAppSettingOptions["JwtExpireDays"]));

                        var token = new JwtSecurityToken(
                            GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"],
                            GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"],
                            claims,
                            expires: expires,
                            signingCredentials: creds
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                        TimeSpan span = expires - now;
                        var expiresIn = (int)span.TotalSeconds;

                        TimeSpan passwordExpirationSpan = alertPasswordExpirationDate - now;
                        var aboutToExpire = daysToAlert + (int)passwordExpirationSpan.TotalDays;

                        var message = String.Empty;

                        if ((aboutToExpire <= daysToAlert))
                            message = $"Password is about to expire! Expire in [{aboutToExpire}] days";
                        else
                            message = "";

                        response.Data.Add(new TokenDto() { Token = tokenString, ExpiresInSeconds = expiresIn, Roles = roles, UserId = user.Id, UserName = user.UserName, Message = message });
                        response.Successful = true;
                        response.IsAuthenticated = true;
                        response.ResultState = ResultType.Found;
                        response.Message = "";
                    }
                    else if(userName.Trim().ToLower() == user.UserName.ToLower() && passwordMd5 == authentication.Password && authentication.Enable && (authentication.PasswordExpiration && passwordExpirationDate < now))
                    {
                        throw new Exception("Invalid grant - Password is expiried!");
                    }
                    else if(userName.Trim().ToLower() == user.UserName.ToLower() && passwordMd5 == authentication.Password && !authentication.Enable)
                    {
                        throw new Exception("Invalid grant - Account not enable!");
                    }
                    else
                    {
                        throw new Exception("Invalid grant - Provided username and password is incorrect!");
                    }
                }
                catch (Exception ex)
                {
                    response.Successful = false;
                    response.IsAuthenticated = false;
                    response.ResultState = ResultType.Error;
                    response.Message = "";
                    response.OriginalException = ex;
                    logger.Error(ex.ToString());
                    //throw ex;
                }

                return response;
            }
        }

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

                        validationParameters.ValidIssuer = GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"];
                        validationParameters.ValidAudience = GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"];

                        validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GeneralSettings.Static.JwtAppSettingOptions["JwtKey"]));


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
    }
}
