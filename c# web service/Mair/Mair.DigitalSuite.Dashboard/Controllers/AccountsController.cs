using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mair.DigitalSuite.Dashboard.Models;
using Mair.DigitalSuite.Dashboard.Repositories;
using Mair.DigitalSuite.Dashboard.Common;
using System.Reflection;
using NLog;
using X.PagedList;
using static Mair.DigitalSuite.Dashboard.Common.Config;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using Mair.DigitalSuite.Dashboard.Models.Dto;
using Mair.DigitalSuite.Dashboard.Models.Dto.Auth;

namespace Mair.DigitalSuite.Dashboard.Controllers
{
    public class AccountsController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly UserRolesRepository userRoleRepo;
        private readonly UsersRepository userRepo;
        private readonly UserRoleTypesRepository userRoleTypeRepo;
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly Utility utility;

        public AccountsController()
        {
            authenticationRepo = new AuthenticationsRepository();
            userRoleRepo = new UserRolesRepository();
            userRepo = new UsersRepository();
            userRoleTypeRepo = new UserRoleTypesRepository();
            utility = new Utility();
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new List<AccountDto>() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                    data.Add(new AccountDto() { });

                    var getUserRoleTypesResult = userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie).Result;
                    if (!getUserRoleTypesResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetAllUserRoleTypes)}] - Message: [{getUserRoleTypesResult.Message}]");

                    var userRoleTypes = getUserRoleTypesResult.Data.ToList();

                    foreach (var row in data)
                    {
                        row.Name = String.Empty;
                        row.Surname = String.Empty;
                        row.Password = Config.GeneralSettings.Constants.DefaultPassword;
                        row.ConfirmPassword = Config.GeneralSettings.Constants.DefaultPassword;
                        row.UserRoleTypes = userRoleTypes;
                        row.CreatedAt = DateTime.Now.Date;
                        row.PasswordExpiration = true;
                        row.PasswordExpirationDays = 90;
                        row.Enable = true;
                        row.UserRoleTypeIds = new long[] { userRoleTypes.Where(_ => _.Type == Config.Roles.Constants.RoleUser).FirstOrDefault().Id };
                    }

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,UserName,Password,ConfirmPassword,PasswordExpiration,PasswordExpirationDays,Enable,CreatedAt,UserRoleTypeIds")] AccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getAuthenticationResult = await authenticationRepo.GetAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!getAuthenticationResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                        var passwordMd5 = "";

                        using (MD5 md5Hash = MD5.Create())
                        {
                            passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, dto.Password);
                        }

                        //Check
                        var checkError = String.Empty;
                        if (getAuthenticationResult.Data.Count > 0) checkError=$"UserName aleady exists!";
                        if (dto.Password == String.Empty) checkError = $"Password is not valid!";
                        if (dto.Password != dto.ConfirmPassword) checkError = $"Passwords do not match";

                        if (checkError != String.Empty)
                        {
                            var getUserRoleTypesResult = await userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie);
                            if (!getUserRoleTypesResult.Successful)
                                throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetAllUserRoleTypes)}] - Message: [{getUserRoleTypesResult.Message}]");

                            var userRoleTypes = getUserRoleTypesResult.Data.ToList();

                            dto.UserRoleTypes = userRoleTypes;

                            throw new Exception(checkError);
                        }

                        //Create
                        var userDto = new UserDto() { UserName = dto.UserName, Name = dto.Name, Surname= dto.Surname };
                        var userResult = await userRepo.AddUser(userDto, access_token_cookie);
                        if (!userResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.AddUser)}] - Message: [{userResult.Message}]");

                        var authenticationDto = new AuthenticationDto() { UserId = userResult.Data.FirstOrDefault().Id, Password = passwordMd5, PasswordExpiration = dto.PasswordExpiration, PasswordExpirationDays = dto.PasswordExpirationDays, Enable = dto.Enable, CreatedAt = DateTime.Now.Date};
                        var authenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, access_token_cookie);
                        if (!authenticationResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{authenticationResult.Message}]");

                        foreach (var userRoleTypeId in dto.UserRoleTypeIds)
                        {
                            var userRoleDto = new UserRoleDto() { UserId = userResult.Data.FirstOrDefault().Id, UserRoleTypeId = userRoleTypeId };
                            var userRoleResult = await userRoleRepo.AddUserRole(userRoleDto, access_token_cookie);
                            if (!userRoleResult.Successful)
                                throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.AddUserRole)}] - Message: [{userRoleResult.Message}]");
                        }

                        return RedirectToAction("Create", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                }
                return View(dto);
            }
        }

        public IActionResult ResetPassword()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new ResetPasswordDto() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([Bind("UserName")] ResetPasswordDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getAuthenticationResult = await authenticationRepo.GetAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!getAuthenticationResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                        var disableAuthenticationResult = await authenticationRepo.DisableAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!disableAuthenticationResult.Successful)
                            throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                        var passwordMd5 = "";

                        using (MD5 md5Hash = MD5.Create())
                        {
                            passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, GeneralSettings.Constants.DefaultPassword);
                        }

                        var authenticationDto = getAuthenticationResult.Data.Where(_ => _.Enable == true).OrderBy(_ => _.Id).LastOrDefault();
                        authenticationDto.Id = 0;
                        authenticationDto.Password = passwordMd5;
                        authenticationDto.CreatedAt = DateTime.Now.Date;
                        authenticationDto.Enable = true;
                        var addAuthenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, access_token_cookie);
                        if (!addAuthenticationResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{addAuthenticationResult.Message}]");

                        return RedirectToAction("ResetPassword", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                }
                return View(dto);
            }
        }

        public IActionResult ChangePassword()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new ChangePasswordDto() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    data.UserName = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAuthenticatedUserCookieName);

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([Bind("UserName,OldPassword,NewPassword,ConfirmNewPassword")] ChangePasswordDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getAuthenticationResult = await authenticationRepo.GetAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!getAuthenticationResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

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
                        var disableAuthenticationResult = await authenticationRepo.DisableAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!disableAuthenticationResult.Successful)
                            throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                        var authenticationDto = getAuthenticationResult.Data.Where(_ => _.Enable == true).OrderBy(_ => _.Id).LastOrDefault();
                        authenticationDto.Id = 0;
                        authenticationDto.Password = passwordMd5;
                        authenticationDto.CreatedAt = DateTime.Now.Date;
                        authenticationDto.Enable = true;
                        var addAuthenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, access_token_cookie);
                        if (!addAuthenticationResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{addAuthenticationResult.Message}]");

                        return RedirectToAction("ChangePassword", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                }
                return View(dto);
            }
        }

        public IActionResult DisableAccount()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new DisableAccountDto() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableAccount([Bind("UserName")] DisableAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getAuthenticationResult = await authenticationRepo.GetAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!getAuthenticationResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                        //Check
                        if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");

                        var disableAuthenticationResult = await authenticationRepo.DisableAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!disableAuthenticationResult.Successful)
                            throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                        return RedirectToAction("DisableAccount", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                }
                return View(dto);
            }
        }

        public IActionResult RemoveAccount()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new RemoveAccountDto() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAccount([Bind("UserName")] RemoveAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getAuthenticationResult = await authenticationRepo.GetAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!getAuthenticationResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                        //Check
                        if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");

                        var getUserRoleResult = await userRoleRepo.GetAllUserRoles (access_token_cookie);
                        if (!getUserRoleResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetAllUserRoles)}] - Message: [{getUserRoleResult.Message}]");

                        var authentications = getAuthenticationResult.Data.ToList();
                        var userId = authentications.FirstOrDefault().UserId;
                        var userRoles = getUserRoleResult.Data.Where(_ => _.UserId == userId).ToList();

                        //Delete
                        foreach (var userRole in userRoles)
                        {
                            var deleteUserRoleResult = await userRoleRepo.DeleteUserRoleById(userRole.Id, access_token_cookie);
                            if (!deleteUserRoleResult.Successful)
                                throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.DeleteUserRoleById)}] - Message: [{deleteUserRoleResult.Message}]");
                        }

                        foreach (var authentication in authentications)
                        {
                            var deleteAuthenticationResult = await authenticationRepo.DeleteAuthenticationById(authentication.Id, access_token_cookie);
                            if (!deleteAuthenticationResult.Successful)
                                throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DeleteAuthenticationById)}] - Message: [{deleteAuthenticationResult.Message}]");
                        }

                        var deleteUserResult = await userRepo.DeleteUserById(userId, access_token_cookie);
                        if (!deleteUserResult.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.DeleteUserById)}] - Message: [{deleteUserResult.Message}]");

                        return RedirectToAction("RemoveAccount", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                }
                return View(dto);
            }
        }

        public IActionResult EnableAccount()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new EnableAccountDto() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAccount([Bind("UserName")] EnableAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getAuthenticationResult = await authenticationRepo.GetAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!getAuthenticationResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsByUserName)}] - Message: [{getAuthenticationResult.Message}]");

                        //Check
                        if (getAuthenticationResult.Data.Count == 0) throw new Exception($"UserName not found!");

                        var disableAuthenticationResult = await authenticationRepo.DisableAuthenticationsByUserName(dto.UserName, access_token_cookie);
                        if (!disableAuthenticationResult.Successful)
                            throw new Exception($"Error [Disable failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DisableAuthenticationsByUserName)}] - Message: [{disableAuthenticationResult.Message}]");

                        var passwordMd5 = "";

                        using (MD5 md5Hash = MD5.Create())
                        {
                            passwordMd5 = Common.Utility.GetMd5Hash(md5Hash, GeneralSettings.Constants.DefaultPassword);
                        }

                        var authenticationDto = getAuthenticationResult.Data.OrderBy(_ => _.Id).LastOrDefault();
                        authenticationDto.Id = 0;
                        authenticationDto.Password = passwordMd5;
                        authenticationDto.CreatedAt = DateTime.Now.Date;
                        authenticationDto.Enable = true;
                        var addAuthenticationResult = await authenticationRepo.AddAuthentication(authenticationDto, access_token_cookie);
                        if (!addAuthenticationResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{addAuthenticationResult.Message}]");

                        return RedirectToAction("EnableAccount", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                }
                return View(dto);
            }
        }
    }
}
