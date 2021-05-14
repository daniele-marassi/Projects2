


using Mair.DS.Common;
using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Mair.DS.Models.Dto.Auth;
using Mair.DS.Models.Results;
using Mair.DS.Models.Results.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
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

namespace Mair.DS.Services.Token
{
    public class GrantCredentials
    {
        private AuthenticationRepository authenticationRepository;
        private UserRepository userRepository;
        private RoleRepository roleRepository;
        private UserRoleRepository userRoleRepository;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();

        public GrantCredentials()
        {
            var optionBuilder = new DbContextOptionsBuilder<AutomationContext>();
            optionBuilder.UseSqlServer(Models.Defaults.ConnectionString);
            var automationContext = new AutomationContext(optionBuilder.Options);

            authenticationRepository = new AuthenticationRepository(automationContext);
            userRepository = new UserRepository(automationContext);
            roleRepository = new RoleRepository(automationContext);
            userRoleRepository = new UserRoleRepository(automationContext);
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
                var response = new TokenResult() { Data = new List<TokenDto>() { }, ResultState = new ResultType() };

                try
                {
                    var getUserResult = await userRepository.Read();

                    if (getUserResult.ToList().Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getUserResult)}!");
                    }

                    var user = getUserResult.Where(_ => _.UserName.ToLower() == userName.Trim().ToLower()).FirstOrDefault();

                    if (user == null)
                    {
                        throw new Exception($"User not found!");
                    }

                    var getAuthenticationsResult = await authenticationRepository.Read();    

                    if (getAuthenticationsResult.ToList().Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getAuthenticationsResult)}!");
                    }

                    var authentication = getAuthenticationsResult.Where(_ => _.UserId == user.Id).OrderBy(_ => _.Id).LastOrDefault();

                    var getUserRoleResult = await userRoleRepository.Read();

                    if (getUserRoleResult.ToList().Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getUserRoleResult)}!");
                    }

                    var roleIds = getUserRoleResult.Where(_ => _.UserId == user.Id).Select(_ => _.RoleId).ToList();

                    var getRoleResult = await roleRepository.Read();

                    if (getRoleResult.ToList().Count == 0)
                    {
                        throw new Exception($"Not data found in {nameof(getRoleResult)}!");
                    }

                    var roles = getRoleResult.Where(_ => roleIds.Contains(_.Id)).Select(_ => _.Type).ToList();

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

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Defaults.JwtAppSettingOptions["JwtKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expires = DateTime.Now.AddDays(Convert.ToDouble(Defaults.JwtAppSettingOptions["JwtExpireDays"]));

                        var token = new JwtSecurityToken(
                            Defaults.JwtAppSettingOptions["JwtIssuer"],
                            Defaults.JwtAppSettingOptions["JwtIssuer"],
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

                        if ((aboutToExpire <= daysToAlert) && authentication.PasswordExpiration)
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
                    response.Message = ex.Message;
                    response.OriginalException = null;
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
    }
}
