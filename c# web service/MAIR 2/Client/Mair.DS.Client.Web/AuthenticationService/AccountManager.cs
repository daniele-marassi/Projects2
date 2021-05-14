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
using Mair.DS.Client.Web.Repositories.Auth;
using System.Security.Cryptography;

namespace Mair.DS.Client.Web.AuthenticationService
{
    public class AccountManager
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly Utility utility;
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly RolesRepository roleRepo ;
        private readonly RolePathsRepository rolePathRepo;
        private readonly UserRolesRepository userRoleRepo;
        private readonly UsersRepository userRepo;

        public AccountManager()
        {
            authenticationRepo = new AuthenticationsRepository();
            roleRepo = new  RolesRepository();
            rolePathRepo = new RolePathsRepository();
            userRoleRepo = new UserRolesRepository();
            userRepo = new UsersRepository();
            utility = new Utility();
        }

        /// <summary>
        /// Get Authentications By UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAuthenticationsByUserName(string userName, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var usersResult = await userRepo.GetAllUsers(token);
                    var authenticationsResult = await authenticationRepo.GetAllAuthentications(token);

                    if (usersResult.Successful == false || authenticationsResult.Successful == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += usersResult.Message;
                        response.Message += authenticationsResult.Message;
                    }
                    else
                    {
                        var user = usersResult.Data.Where(_ => _.UserName == userName).FirstOrDefault();

                        if (user == null) throw new Exception("User not found!");

                        var authentications = authenticationsResult.Data.Where(_ => _.UserId == user.Id).ToList();

                        if (authentications == null || authentications.Count == 0) throw new Exception("Authentication not found!");

                        response.Data.AddRange(authentications);
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
        /// Disable Authentications By UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> DisableAuthenticationsByUserName(string userName, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AuthenticationResult() { Data = new List<AuthenticationDto>(), ResultState = new ResultType() };

                try
                {
                    var usersResult = await userRepo.GetAllUsers(token);
                    var authenticationsResult = await authenticationRepo.GetAllAuthentications(token);

                    if (usersResult.Successful == false || authenticationsResult.Successful == false)
                    {
                        response.Successful = false;
                        response.ResultState = ResultType.Error;
                        response.Message += usersResult.Message;
                        response.Message += authenticationsResult.Message;
                    }
                    else
                    {
                        var user = usersResult.Data.Where(_ => _.UserName == userName).FirstOrDefault();

                        if (user == null) throw new Exception("User not found!");

                        var authentications = authenticationsResult.Data.Where(_ => _.UserId == user.Id && _.Enable == true).ToList();

                        if (authentications == null || authentications.Count == 0) throw new Exception("Authentication not found!");

                        try
                        {
                            foreach (var authentication in authentications)
                            {
                                authentication.Enable = false;
                                var result = await authenticationRepo.UpdateAuthentication(authentication, token);
                                response.Data.AddRange(result.Data);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

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

        /// <summary>
        /// Get Default Account
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountResult> GetDefaultAccount(string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AccountResult() { Data = new List<AccountDto>(), ResultState = new ResultType() };
                try
                {
                    var data = new List<AccountDto>() { };
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    data.Add(new AccountDto() { });

                    var getRolesResult = roleRepo.GetAllRoles(token).Result;
                    if (!getRolesResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(roleRepo.GetAllRoles)}] - Message: [{getRolesResult.Message}]");

                    var roles = getRolesResult.Data.ToList();

                    foreach (var row in data)
                    {
                        row.Name = String.Empty;
                        row.Surname = String.Empty;
                        row.Password = Defaults.DefaultPassword;
                        row.ConfirmPassword = Defaults.DefaultPassword;
                        row.Roles = roles;
                        row.CreatedAt = DateTime.Now.Date;
                        row.PasswordExpiration = true;
                        row.PasswordExpirationDays = 90;
                        row.Enable = true;
                        row.RoleIds = new long[] { roles.Where(_ => _.Type == Defaults.Roles.UserRole).FirstOrDefault().Id };
                    }

                    response.Data.AddRange(data);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountResult> CreateAccount(AccountDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new AccountResult() { Data = new List<AccountDto>(), ResultState = new ResultType() };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var getAuthenticationResult = await GetAuthenticationsByUserName(dto.UserName, token);
                    if (!getAuthenticationResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                    var passwordMd5 = "";

                    using (MD5 md5Hash = MD5.Create())
                    {
                        passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, dto.Password);
                    }

                    //Check
                    var checkError = String.Empty;
                    if (getAuthenticationResult.Data.Count > 0) checkError = $"UserName aleady exists!";
                    if (dto.Password == String.Empty) checkError = $"Password is not valid!";
                    if (dto.Password != dto.ConfirmPassword) checkError = $"Passwords do not match";

                    if (checkError != String.Empty)
                    {
                        var getRolesResult = await roleRepo.GetAllRoles(token);
                        if (!getRolesResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(roleRepo.GetAllRoles)}] - Message: [{getRolesResult.Message}]");

                        var roles = getRolesResult.Data.ToList();

                        dto.Roles = roles;

                        throw new Exception(checkError);
                    }

                    //Create
                    var userDto = new UserDto() { UserName = dto.UserName, Name = dto.Name, Surname = dto.Surname };
                    var userResult = await userRepo.AddUser(userDto, token);
                    if (!userResult.Successful)
                        throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.AddUser)}] - Message: [{userResult.Message}]");

                    var authenticationDto = new AuthenticationDto() { UserId = userResult.Data.FirstOrDefault().Id, Password = passwordMd5, PasswordExpiration = dto.PasswordExpiration, PasswordExpirationDays = dto.PasswordExpirationDays, Enable = dto.Enable, Created = DateTime.Now.Date };
                    var authenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, token);
                    if (!authenticationResult.Successful)
                        throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{authenticationResult.Message}]");

                    foreach (var roleId in dto.RoleIds)
                    {
                        var userRoleDto = new UserRoleDto() { UserId = userResult.Data.FirstOrDefault().Id, RoleId = roleId };
                        var userRoleResult = await userRoleRepo.AddUserRole(userRoleDto, token);
                        if (!userRoleResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.AddUserRole)}] - Message: [{userRoleResult.Message}]");
                    }

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ResetPasswordResult> ResetPassword(ResetPasswordDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ResetPasswordResult() { Data = new List<ResetPasswordDto>(), ResultState = new ResultType() };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var getAuthenticationResult = await GetAuthenticationsByUserName(dto.UserName, token);
                    if (!getAuthenticationResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                    var disableAuthenticationResult = await DisableAuthenticationsByUserName(dto.UserName, token);
                    if (!disableAuthenticationResult.Successful)
                        throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                    var passwordMd5 = "";

                    using (MD5 md5Hash = MD5.Create())
                    {
                        passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, Defaults.DefaultPassword);
                    }

                    var authenticationDto = getAuthenticationResult.Data.Where(_ => _.Enable == true).OrderBy(_ => _.Id).LastOrDefault();
                    authenticationDto.Id = 0;
                    authenticationDto.Password = passwordMd5;
                    authenticationDto.Created = DateTime.Now.Date;
                    authenticationDto.Enable = true;
                    var addAuthenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, token);
                    if (!addAuthenticationResult.Successful)
                        throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{addAuthenticationResult.Message}]");

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ChangePasswordResult> ChangePassword(ChangePasswordDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new ChangePasswordResult() { Data = new List<ChangePasswordDto>(), ResultState = new ResultType() };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var getAuthenticationResult = await GetAuthenticationsByUserName(dto.UserName, token);
                    if (!getAuthenticationResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                    var oldPasswords = getAuthenticationResult.Data.Where(_ => _.Enable == false).OrderByDescending(_ => _.Id).Take(10).Select(_ => _.Password).ToList();
                    var passwordMd5 = "";

                    using (MD5 md5Hash = MD5.Create())
                    {
                        passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, dto.NewPassword);
                    }

                    //Check
                    if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");
                    if (dto.NewPassword == String.Empty) throw new Exception($"Password is not valid!");
                    if (dto.ConfirmNewPassword != dto.ConfirmNewPassword) throw new Exception($"New Passwords do not match!");
                    if (oldPasswords.Contains(passwordMd5)) throw new Exception($"Password already used!");
                    if (dto.OldPassword != oldPasswords.LastOrDefault()) throw new Exception($"Old Password do not match!");

                    //Change
                    var disableAuthenticationResult = await DisableAuthenticationsByUserName(dto.UserName, token);
                    if (!disableAuthenticationResult.Successful)
                        throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                    var authenticationDto = getAuthenticationResult.Data.Where(_ => _.Enable == true).OrderBy(_ => _.Id).LastOrDefault();
                    authenticationDto.Id = 0;
                    authenticationDto.Password = passwordMd5;
                    authenticationDto.Created = DateTime.Now.Date;
                    authenticationDto.Enable = true;
                    var addAuthenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, token);
                    if (!addAuthenticationResult.Successful)
                        throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{addAuthenticationResult.Message}]");

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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

        /// <summary>
        /// Disable Account
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<DisableAccountResult> DisableAccount(DisableAccountDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new DisableAccountResult() { Data = new List<DisableAccountDto>(), ResultState = new ResultType() };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var getAuthenticationResult = await GetAuthenticationsByUserName(dto.UserName, token);
                    if (!getAuthenticationResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                    //Check
                    if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");

                    var disableAuthenticationResult = await DisableAuthenticationsByUserName(dto.UserName, token);
                    if (!disableAuthenticationResult.Successful)
                        throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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

        /// <summary>
        /// Remove Account
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RemoveAccountResult> RemoveAccount(RemoveAccountDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new RemoveAccountResult() { Data = new List<RemoveAccountDto>(), ResultState = new ResultType() };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var getAuthenticationResult = await GetAuthenticationsByUserName(dto.UserName, token);
                    if (!getAuthenticationResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                    //Check
                    if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");

                    var getUserRoleResult = await userRoleRepo.GetAllUserRoles(token);
                    if (!getUserRoleResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetAllUserRoles)}] - Message: [{getUserRoleResult.Message}]");

                    var authentications = getAuthenticationResult.Data.ToList();
                    var userId = authentications.FirstOrDefault().UserId;
                    var userRoles = getUserRoleResult.Data.Where(_ => _.UserId == userId).ToList();

                    //Delete
                    foreach (var userRole in userRoles)
                    {
                        var deleteUserRoleResult = await userRoleRepo.DeleteUserRoleById(userRole.Id, token);
                        if (!deleteUserRoleResult.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.DeleteUserRoleById)}] - Message: [{deleteUserRoleResult.Message}]");
                    }

                    foreach (var authentication in authentications)
                    {
                        var deleteAuthenticationResult = await authenticationRepo.DeleteAuthenticationById(authentication.Id, token);
                        if (!deleteAuthenticationResult.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DeleteAuthenticationById)}] - Message: [{deleteAuthenticationResult.Message}]");
                    }

                    var deleteUserResult = await userRepo.DeleteUserById(userId, token);
                    if (!deleteUserResult.Successful)
                        throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.DeleteUserById)}] - Message: [{deleteUserResult.Message}]");

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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

        /// <summary>
        /// Enable Account
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<EnableAccountResult> EnableAccount(EnableAccountDto dto, string token)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var response = new EnableAccountResult() { Data = new List<EnableAccountDto>(), ResultState = new ResultType() };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var getAuthenticationResult = await GetAuthenticationsByUserName(dto.UserName, token);
                    if (!getAuthenticationResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                    //Check
                    if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");

                    var disableAuthenticationResult = await DisableAuthenticationsByUserName(dto.UserName, token);
                    if (!disableAuthenticationResult.Successful)
                        throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                    var passwordMd5 = "";

                    using (MD5 md5Hash = MD5.Create())
                    {
                        passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, Defaults.DefaultPassword);
                    }

                    var authenticationDto = getAuthenticationResult.Data.OrderBy(_ => _.Id).LastOrDefault();
                    authenticationDto.Id = 0;
                    authenticationDto.Password = passwordMd5;
                    authenticationDto.Created = DateTime.Now.Date;
                    authenticationDto.Enable = true;
                    var addAuthenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, token);
                    if (!addAuthenticationResult.Successful)
                        throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{addAuthenticationResult.Message}]");

                    response.Data.Add(dto);
                    response.Successful = true;
                    response.ResultState = ResultType.Found;
                    response.Message = "";
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
