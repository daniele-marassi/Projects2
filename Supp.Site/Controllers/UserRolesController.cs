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
using Additional.NLog;

namespace Supp.Site.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly UsersRepository userRepo;
        private readonly SuppUtility suppUtility;
        private readonly UserRolesRepository userRoleRepo;
        private readonly UserRoleTypesRepository userRoleTypeRepo;

        public UserRolesController()
        {
            userRoleRepo = new UserRolesRepository();
            userRepo = new UsersRepository();
            userRoleTypeRepo = new UserRoleTypesRepository();
            suppUtility = new SuppUtility();
        }

        // GET: UserRoles
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new UserRoleResult() { Data = new List<UserRoleDto>() { }, Successful = false };
                IEnumerable<UserRoleDto> data;

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

                    result = await userRoleRepo.GetAllUserRoles(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetAllUserRoles)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var userRoleTypes = userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;

                        foreach (var userRoleType in userRoleTypes)
                        {
                            userRoleType.TypeName = userRoleType.Type;
                        }
                        row.UserRoleTypes = userRoleTypes;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val
                            || _.UserId == val
                            || _.UserRoleTypeId == val
                        );
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _userIds = users.Where(_ => _.UserFullName.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        var _userRoleTypeIds = userRoleTypes.Where(_ => _.TypeName.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();

                        data = data.Where(_ =>
                            _userIds.Contains(_.UserId)
                            || _userRoleTypeIds.Contains(_.UserRoleTypeId)
                        );
                    }

                    switch (sortOrder)
                    {
                        case "UserFullName":
                            data = data.OrderBy(x => x.Users.Where(_ => _.Id == x.UserId).Select(_ => _.UserFullName).FirstOrDefault());
                            break;
                        case "TypeName":
                            data = data.OrderBy(x => x.UserRoleTypes.Where(_ => _.Id == x.UserRoleTypeId).Select(_ => _.TypeName).FirstOrDefault());
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
                    ModelState.AddModelError("ModelStateErrors", "UserRoles not found!");
                    return View();
                }
            }
        }

        // GET: UserRoles/Details/5
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
                    var result = await userRoleRepo.GetUserRolesById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var userRoleTypes = userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;

                        foreach (var userRoleType in userRoleTypes)
                        {
                            userRoleType.TypeName = userRoleType.Type;
                        }
                        row.UserRoleTypes = userRoleTypes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetUserRolesById)}] - Message: [{result.Message}]");

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

        // GET: UserRoles/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<UserRoleDto>() { };
                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    data.Add(new UserRoleDto() { });

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var userRoleTypes = userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;

                        foreach (var userRoleType in userRoleTypes)
                        {
                            userRoleType.TypeName = userRoleType.Type;
                        }
                        row.UserRoleTypes = userRoleTypes;
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

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,UserRoleTypeId,InsDateTime")] UserRoleDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<UserRoleDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await userRoleRepo.AddUserRole(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.AddUserRole)}] - Message: [{result.Message}]");

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

        // GET: UserRoles/Edit/5
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
                    var result = await userRoleRepo.GetUserRolesById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var userRoleTypes = userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie).Result.Data.ToList();


                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;

                        foreach (var userRoleType in userRoleTypes)
                        {
                            userRoleType.TypeName = userRoleType.Type;
                        }
                        row.UserRoleTypes = userRoleTypes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetUserRolesById)}] - Message: [{result.Message}]");

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

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,UserRoleTypeId,InsDateTime")] UserRoleDto dto)
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

                        if (!UserRoleExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await userRoleRepo.UpdateUserRole(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.UpdateUserRole)}] - Message: [{result.Message}]");
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

        // GET: UserRoles/Delete/5
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
                    var result = await userRoleRepo.GetUserRolesById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var userRoleTypes = userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;

                        foreach (var userRoleType in userRoleTypes)
                        {
                            userRoleType.TypeName = userRoleType.Type;
                        }
                        row.UserRoleTypes = userRoleTypes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetUserRolesById)}] - Message: [{result.Message}]");

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

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<UserRoleDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await userRoleRepo.DeleteUserRoleById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.DeleteUserRoleById)}] - Message: [{result.Message}]");

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

        private bool UserRoleExists(long id)
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
                    var result = userRoleRepo.GetUserRolesById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetUserRolesById)}] - Message: [{result.Message}]");

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
