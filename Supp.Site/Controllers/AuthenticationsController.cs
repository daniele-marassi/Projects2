using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supp.Site.Models;
using Supp.Site.Repositories;
using Supp.Site.Common;
using System.Reflection;
using NLog;
using X.PagedList;
using static Supp.Site.Common.Config;
using System.Security.Cryptography;
using Additional.NLog;

namespace Supp.Site.Controllers
{
    public class AuthenticationsController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly SuppUtility suppUtility;
        private readonly UsersRepository userRepo;

        public AuthenticationsController()
        {
            authenticationRepo = new AuthenticationsRepository();
            suppUtility = new SuppUtility();
            userRepo = new UsersRepository();
        }

        // GET: Authentications
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                AuthenticationResult result = new AuthenticationResult() { Data = new List<AuthenticationDto>() { }, Successful = false };
                IEnumerable<AuthenticationDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "UserFullName" : "";

                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;

                    string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    result = await authenticationRepo.GetAllAuthentications(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAllAuthentications)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ =>  _.Id == val
                            || _.UserId == val
                        );
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _userIds = users.Where(_ => _.UserFullName.ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        data = data.Where(_ => _userIds.Contains(_.UserId)
                        );
                    }

                    switch (sortOrder)
                    {
                        case "Password":
                            data = data.OrderBy(_ => _.Password);
                            break;
                        case "PasswordExpiration":
                            data = data.OrderBy(_ => _.PasswordExpiration);
                            break;
                        case "PasswordExpirationDays":
                            data = data.OrderBy(_ => _.PasswordExpirationDays);
                            break;
                        case "Enable":
                            data = data.OrderBy(_ => _.Enable);
                            break;
                        case "UserFullName":
                            data = data.OrderBy(x => x.Users.Where(_ => _.Id == x.UserId).Select(_ => _.UserFullName).FirstOrDefault());
                            break;
                        default:
                            data = data.OrderBy(x => x.Users.Where(_ => _.Id == x.UserId).Select(_ => _.UserFullName).FirstOrDefault());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    suppUtility.AddErrorToCookie(Request, Response, ex.Message);

                    return RedirectToAction("Index", "Home");
                }

                if (result.Successful && result.Data.Count > 0)
                {
                    int pageSize = GeneralSettings.Static.PageSize;
                    int pageNumber = (page ?? 1);
                    return View(data.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ModelState.AddModelError("ModelStateErrors", "Authentications not found!");
                    return View();
                }
            }
        }

        // GET: Authentications/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    if (id == null)
                        throw new Exception($"Error [Id is null!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    var result = await authenticationRepo.GetAuthenticationsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsById)}] - Message: [{result.Message}]");

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // GET: Authentications/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<AuthenticationDto>() { };
                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    data.Add(new AuthenticationDto() { });

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.CreatedAt = DateTime.Now.Date;
                        row.Users = users;
                        row.PasswordExpiration = true;
                        row.PasswordExpirationDays = 90;
                        row.Enable = true;
                    }

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // POST: Authentications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password,PasswordExpiration,PasswordExpirationDays,Enable,UserId,InsDateTime")] AuthenticationDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<AuthenticationDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var passwordMd5 = "";

                        using (MD5 md5Hash = MD5.Create())
                        {
                            passwordMd5 = Common.SuppUtility.GetMd5Hash(md5Hash, dto.Password);
                        }

                        dto.Password = passwordMd5;

                        var result = await authenticationRepo.AddAuthentication(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.AddAuthentication)}] - Message: [{result.Message}]");

                        return RedirectToAction(nameof(Index));
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

        // GET: Authentications/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    if (id == null)
                        throw new Exception($"Error [Id is null!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    var result = await authenticationRepo.GetAuthenticationsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsById)}] - Message: [{result.Message}]");

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // POST: Authentications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Password,PasswordExpiration,PasswordExpirationDays,Enable,UserId,InsDateTime")] AuthenticationDto dto)
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

                        if (id != dto.Id)
                            throw new Exception($"Error [Id not match!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        if (!AuthenticationExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await authenticationRepo.UpdateAuthentication(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.UpdateAuthentication)}] - Message: [{result.Message}]");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(dto);
            }
        }

        // GET: Authentications/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    if (id == null)
                        throw new Exception($"Error [Id is null!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    var result = await authenticationRepo.GetAuthenticationsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsById)}] - Message: [{result.Message}]");

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // POST: Authentications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<AuthenticationDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await authenticationRepo.DeleteAuthenticationById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.DeleteAuthenticationById)}] - Message: [{result.Message}]");

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View();
                    }
                }
                return View();
            }
        }

        private bool AuthenticationExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var exists = false;
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    var result = authenticationRepo.GetAuthenticationsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(authenticationRepo.GetAuthenticationsById)}] - Message: [{result.Message}]");

                    exists = true;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                }

                return exists;
            }
        }
    }
}
