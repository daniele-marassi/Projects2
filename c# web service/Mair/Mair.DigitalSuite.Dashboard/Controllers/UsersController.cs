﻿using System;
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
using Mair.DigitalSuite.Dashboard.Models.Dto.Auth;
using Mair.DigitalSuite.Dashboard.Models.Result.Auth;

namespace Mair.DigitalSuite.Dashboard.Controllers
{
    public class UsersController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly UsersRepository userRepo;
        private readonly Utility utility;

        public UsersController()
        {
            userRepo = new UsersRepository();
            utility = new Utility();
        }

        // GET: Users
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new UserResult() { Data = new List<UserDto>() { }, Successful = false };
                IEnumerable<UserDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Surname" : "";

                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchString;

                    string access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                    result = await userRepo.GetAllUsers(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.GetAllUsers)}] - Message: [{result.Message}]");

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
                        data = data.Where(_ => _.Surname.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Name.ToUpper().Contains(searchString.ToUpper().Trim())
                        );
                    }

                    switch (sortOrder)
                    {
                        case "Name":
                            data = data.OrderBy(_ => _.Name);
                            break;
                        case "Surname":
                            data = data.OrderBy(_ => _.Surname);
                            break;
                        default:
                            data = data.OrderBy(_ => _.Surname);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    utility.AddErrorToCookie(Request, Response, ex.Message);

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
                    ModelState.AddModelError("ModelStateErrors", "Users not found!");
                    return View();
                }
            }
        }

        // GET: Users/Details/5
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

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                    var result = await userRepo.GetUsersById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.GetUsersById)}] - Message: [{result.Message}]");

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

        // GET: Users/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                return View();
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Name,Surname")] UserDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<UserDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                        var result = await userRepo.AddUser(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful) 
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.AddUser)}] - Message: [{result.Message}]");

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

        // GET: Users/Edit/5
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

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                    var result = await userRepo.GetUsersById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.GetUsersById)}] - Message: [{result.Message}]");

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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserName,Name,Surname")] UserDto dto)
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

                        if (!UserExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                        var result = await userRepo.UpdateUser(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.UpdateUser)}] - Message: [{result.Message}]");
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

        // GET: Users/Delete/5
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

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                    var result = await userRepo.GetUsersById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.GetUsersById)}] - Message: [{result.Message}]");

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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<UserDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                        var result = await userRepo.DeleteUserById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.DeleteUserById)}] - Message: [{result.Message}]");

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

        private bool UserExists(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var exists = false;
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                    var result = userRepo.GetUsersById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(userRepo.GetUsersById)}] - Message: [{result.Message}]");

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
