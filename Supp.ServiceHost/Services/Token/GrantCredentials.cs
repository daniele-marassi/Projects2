using AutoMapper;
using Supp.ServiceHost.Common;
using Supp.ServiceHost.Repositories;
using Supp.Models;

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Supp.ServiceHost.Common.Config;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Contexts;
using System.Configuration;

using Additional.NLog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Supp.ServiceHost.Services.Token
{
    public class GrantCredentials
    {
        private IAuthenticationsRepository iAuthenticationsRepo;
        private IUsersRepository iUsersRepo;
        private IUserRoleTypesRepository iUserRoleTypesRepo;
        private IUserRolesRepository iUserRolesRepo;
        private ITokensRepository iTokensRepo;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private Additional.Utility utility;
        private SuppUtility suppUtility;

        public GrantCredentials(SuppDatabaseContext context)
        {
            iAuthenticationsRepo = new AuthenticationsRepository(context);
            iUsersRepo = new UsersRepository(context);
            iUserRoleTypesRepo = new UserRoleTypesRepository(context);
            iUserRolesRepo = new UserRolesRepository(context);
            iTokensRepo = new TokensRepository(context);
            utility = new Additional.Utility();
            suppUtility = new SuppUtility();
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="passwordAlreadyEncrypted"></param>
        /// <returns></returns>
        public async Task<TokenResult> GetToken(string userName, string password, bool passwordAlreadyEncrypted)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new TokenResult() { Data = new List<TokenDto>() {}, ResultState = ResultType.NotFound, Successful = true };

                try
                {
                    var getAuthenticationsResult = await iAuthenticationsRepo.GetAuthenticationsByUserName(userName);

                    if (!getAuthenticationsResult.Successful)
                        throw new Exception(getAuthenticationsResult.Message);

                    if (getAuthenticationsResult.Data.Count == 0)
                        throw new Exception($"Invalid grant - Provided username not found! {getAuthenticationsResult.OriginalException!.Message}");

                    var authentication = getAuthenticationsResult.Data.OrderBy(_ => _.Id).LastOrDefault();
                    
                    var getUserResult = await iUsersRepo.GetUsersById(authentication!.UserId);

                    if (getUserResult.Data.Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getUserResult)}! {getUserResult.OriginalException.Message}");
                    }

                    var user = getUserResult.Data.FirstOrDefault();

                    var getUserRoleResult = await iUserRolesRepo.GetAllUserRoles();

                    if (getUserResult.Data.Count == 0)
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

                    var rolesInJson = JsonConvert.SerializeObject(roles);

                    var passwordMd5 = "";

                    var now = DateTime.Now.Date;

                    if (passwordAlreadyEncrypted == false)
                    {
                        using (MD5 md5Hash = MD5.Create())
                        {
                            passwordMd5 = utility.GetMd5Hash(md5Hash, password);
                        }
                    }
                    else passwordMd5 = password;

                    var createdAt = authentication.CreatedAt;
                    createdAt = new DateTime(createdAt.Year, createdAt.Month, createdAt.Day, 0, 0, 0);

                    var passwordExpirationDate = createdAt.AddDays(authentication.PasswordExpirationDays);

                    var daysToAlert = 3;

                    var alertPasswordExpirationDate = createdAt.AddDays(authentication.PasswordExpirationDays - daysToAlert);

                    //check user exists
                    if (userName.Trim().ToLower() == user.UserName.ToLower() && passwordMd5 == authentication.Password && authentication.Enable && ((authentication.PasswordExpiration && passwordExpirationDate >= now) || !authentication.PasswordExpiration))
                    {
                        var expires = DateTime.Now.AddDays(GeneralSettings.Static.ExpireDays);

                        var additionalKeys = new List<string>();
                        additionalKeys.Add("Supp.Site");
                        additionalKeys.Add(user.Id.ToString());
                        additionalKeys.Add(userName);
                        additionalKeys.Add(passwordMd5);
                        additionalKeys.Add(user.Name);
                        additionalKeys.Add(user.Surname);
                        additionalKeys.Add(rolesInJson);

                        var tokenString = suppUtility.CreateToken(user.Id, additionalKeys);

                        TimeSpan span = expires - now;
                        var expiresIn = (int)span.TotalSeconds;

                        TimeSpan passwordExpirationSpan = alertPasswordExpirationDate - now;
                        var aboutToExpire = daysToAlert + (int)passwordExpirationSpan.TotalDays;

                        var message = String.Empty;

                        if ((aboutToExpire <= daysToAlert) && authentication.PasswordExpiration)
                            message = $"Password is about to expire! Expire in [{aboutToExpire}] days";
                        else
                            message = "";

                        var tokenDto = new TokenDto() { IsAuthenticated = true, ExpiryDate = expires, RolesInJson = rolesInJson, TokenCode = tokenString, ExpiresInSeconds = expiresIn, Roles = roles, UserId = user.Id, UserName = user.UserName, Message = message, Surname = user.Surname, Name = user.Name, ConfigInJson = user.CustomizeParams };

                        if (roles.Contains(Config.Roles.Constants.RoleAdmin) || roles.Contains(Config.Roles.Constants.RoleSuperUser)) tokenDto.ExpiryDate = null;

                        TokenStorage(tokenDto, nLogUtility);

                        await iTokensRepo.CleanAndAddToken(tokenDto);

                        response.Data.Add(tokenDto);
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
                    response.Message = ex.InnerException != null && ex.InnerException.Message != null? ex.InnerException.Message: ex.Message;
                    response.OriginalException = null;
                    logger.Error(ex.ToString());
                    //throw ex;
                }
                return response;
            }
        }

        private void TokenStorage(TokenDto dto, NLogUtility nLogUtility)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    if (Program.TokensArchive.ContainsKey(dto.UserId))
                    {
                        TokenDto tokenDtoDeleted = null;
                        Program.TokensArchive.TryRemove(dto.UserId, out tokenDtoDeleted);
                    }

                    Program.TokensArchive.TryAdd(dto.UserId, dto);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
