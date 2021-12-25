using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supp.Models;
using Supp.Site.Repositories;
using Supp.Site.Common;
using System.Reflection;
using NLog;
using X.PagedList;
using static Supp.Site.Common.Config;
using Additional.NLog;

namespace Supp.Site.Controllers
{
    public class UserRoleTypesController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly UserRoleTypesRepository userRoleTypeRepo;
        private readonly UserRolesRepository userRoleRepo;
        private readonly SuppUtility suppUtility;

        public UserRoleTypesController()
        {
            userRoleTypeRepo = new UserRoleTypesRepository();
            userRoleRepo = new UserRolesRepository();
            suppUtility = new SuppUtility();
        }

        // GET: UserRoleTypes
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                UserRoleTypeResult result = new UserRoleTypeResult() { Data = new List<UserRoleTypeDto>() { }, Successful = false };
                IEnumerable<UserRoleTypeDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Type" : "";

                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    result = await userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetAllUserRoleTypes)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val);
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        data = data.Where(_ => _.Type.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim()));
                    }

                    switch (sortOrder)
                    {
                        case "Id":
                            data = data.OrderBy(_ => _.Id);
                            break;
                        case "Type":
                            data = data.OrderBy(_ => _.Type);
                            break;
                        default:
                            data = data.OrderBy(_ => _.Type);
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
                    ModelState.AddModelError("ModelStateErrors", "UserRoleTypes not found!");
                    return View();
                }
            }
        }

        // GET: UserRoleTypes/Details/5
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
                    var result = await userRoleTypeRepo.GetUserRoleTypesById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetUserRoleTypesById)}] - Message: [{result.Message}]");

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // GET: UserRoleTypes/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                return View();
            }
        }

        // POST: UserRoleTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,InsDateTime")] UserRoleTypeDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<UserRoleTypeDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var getUserRoleTypesResult = await userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie);
                        if (!getUserRoleTypesResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetAllUserRoleTypes)}] - Message: [{getUserRoleTypesResult.Message}]");

                        //Check
                        if (getUserRoleTypesResult.Data.Where(_ => _.Type.ToLower() == dto.Type.ToLower() && _.Id != dto.Id).ToList().Count > 0) throw new Exception($"Type already exists!");

                        var addUserRoleTypesResult = await userRoleTypeRepo.AddUserRoleType(dto, access_token_cookie);
                        if (!addUserRoleTypesResult.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.AddUserRoleType)}] - Message: [{addUserRoleTypesResult.Message}]");

                        data.AddRange(addUserRoleTypesResult.Data);


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

        // GET: UserRoleTypes/Edit/5
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

                    var result = await userRoleTypeRepo.GetUserRoleTypesById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetUserRoleTypesById)}] - Message: [{result.Message}]");

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // POST: UserRoleTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Type,InsDateTime")] UserRoleTypeDto dto)
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

                        if (!UserRoleTypeExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var getUserRoleTypesResult = await userRoleTypeRepo.GetAllUserRoleTypes(access_token_cookie);
                        if (!getUserRoleTypesResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetAllUserRoleTypes)}] - Message: [{getUserRoleTypesResult.Message}]");

                        //Check
                        if (getUserRoleTypesResult.Data.Where(_ => _.Type.ToLower() == dto.Type.ToLower() && _.Id != id).ToList().Count > 0) throw new Exception($"Type already exists!");

                        var updateUserRoleTypesResult = await userRoleTypeRepo.UpdateUserRoleType(dto, access_token_cookie);
                        if (!updateUserRoleTypesResult.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.UpdateUserRoleType)}] - Message: [{updateUserRoleTypesResult.Message}]");
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

        // GET: UserRoleTypes/Delete/5
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
                    var result = await userRoleTypeRepo.GetUserRoleTypesById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetUserRoleTypesById)}] - Message: [{result.Message}]");

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View();
                }
            }
        }

        // POST: UserRoleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<UserRoleTypeDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var getUserRolesResult = await userRoleRepo.GetAllUserRoles(access_token_cookie);
                        if (!getUserRolesResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleRepo.GetAllUserRoles)}] - Message: [{getUserRolesResult.Message}]");

                        //Check
                        if (getUserRolesResult.Data.Where(_ => _.UserRoleTypeId == id).ToList().Count > 0) throw new Exception($"Type used!");


                        var result = await userRoleTypeRepo.DeleteUserRoleTypeById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.DeleteUserRoleTypeById)}] - Message: [{result.Message}]");

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

        private bool UserRoleTypeExists(long id)
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
                    var result = userRoleTypeRepo.GetUserRoleTypesById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRoleTypeRepo.GetUserRoleTypesById)}] - Message: [{result.Message}]");

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
