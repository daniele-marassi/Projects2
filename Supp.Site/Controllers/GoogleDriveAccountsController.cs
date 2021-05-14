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
    public class GoogleDriveAccountsController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly GoogleDriveAccountsRepository googleDriveAccountRepo;
        private readonly SuppUtility suppUtility;
        private readonly GoogleDriveAuthsRepository googleDriveAuthRepo;
        private readonly UsersRepository userRepo;

        public GoogleDriveAccountsController()
        {
            googleDriveAccountRepo = new GoogleDriveAccountsRepository();
            suppUtility = new SuppUtility();
            googleDriveAuthRepo = new GoogleDriveAuthsRepository();
            userRepo = new UsersRepository();
        }

        // GET: GoogleDriveAccounts
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                GoogleDriveAccountResult result = new GoogleDriveAccountResult() { Data = new List<GoogleDriveAccountDto>() { }, Successful = false };
                IEnumerable<GoogleDriveAccountDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Account" : "";

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

                    result = await googleDriveAccountRepo.GetAllGoogleDriveAccounts(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.GetAllGoogleDriveAccounts)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleDriveAuths = googleDriveAuthRepo.GetAllGoogleDriveAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleDriveAuths = googleDriveAuths;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val);
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _userIds = users.Where(_ => _.UserFullName.ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        var _googleDriveAuthIds = googleDriveAuths.Where(_ => _.Client_id.ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        data = data.Where(_ => _.Account.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.FolderToFilter.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _userIds.Contains(_.UserId)
                            || _googleDriveAuthIds.Contains(_.GoogleDriveAuthId)
                        );
                    }

                    switch (sortOrder)
                    {
                        case "Id":
                            data = data.OrderBy(_ => _.Id);
                            break;
                        case "Account":
                            data = data.OrderBy(_ => _.Account);
                            break;
                        case "FolderToFilter":
                            data = data.OrderBy(_ => _.FolderToFilter);
                            break;
                        case "GoogleDriveAuthId":
                            data = data.OrderBy(_ => _.GoogleDriveAuthId);
                            break;
                        case "GoogleDriveAuthClient_id":
                            data = data.OrderBy(x => x.GoogleDriveAuths.Where(_ => _.Id == x.GoogleDriveAuthId).Select(_ => _.Client_id).FirstOrDefault());
                            break;
                        case "UserFullName":
                            data = data.OrderBy(x => x.Users.Where(_ => _.Id == x.UserId).Select(_ => _.UserFullName).FirstOrDefault());
                            break;
                        default:
                            data = data.OrderBy(_ => _.Account);
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
                    ModelState.AddModelError("ModelStateErrors", "GoogleDriveAccounts not found!");
                    return View();
                }
            }
        }

        // GET: GoogleDriveAccounts/Details/5
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
                    var result = await googleDriveAccountRepo.GetGoogleDriveAccountsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleDriveAuths = googleDriveAuthRepo.GetAllGoogleDriveAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleDriveAuths = googleDriveAuths;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.GetGoogleDriveAccountsById)}] - Message: [{result.Message}]");

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

        // GET: GoogleDriveAccounts/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<GoogleDriveAccountDto>() { };
                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    data.Add(new GoogleDriveAccountDto() { });

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleDriveAuths = googleDriveAuthRepo.GetAllGoogleDriveAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleDriveAuths = googleDriveAuths;
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

        // POST: GoogleDriveAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Account,FolderToFilter,GoogleDriveAuthId,UserId,InsDateTime")] GoogleDriveAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<GoogleDriveAccountDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleDriveAccountRepo.AddGoogleDriveAccount(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.AddGoogleDriveAccount)}] - Message: [{result.Message}]");

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

        // GET: GoogleDriveAccounts/Edit/5
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
                    var result = await googleDriveAccountRepo.GetGoogleDriveAccountsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleDriveAuths = googleDriveAuthRepo.GetAllGoogleDriveAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleDriveAuths = googleDriveAuths;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.GetGoogleDriveAccountsById)}] - Message: [{result.Message}]");

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

        // POST: GoogleDriveAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Account,FolderToFilter,GoogleDriveAuthId,UserId,InsDateTime")] GoogleDriveAccountDto dto)
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

                        if (!GoogleDriveAccountExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleDriveAccountRepo.UpdateGoogleDriveAccount(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.UpdateGoogleDriveAccount)}] - Message: [{result.Message}]");
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

        // GET: GoogleDriveAccounts/Delete/5
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
                    var result = await googleDriveAccountRepo.GetGoogleDriveAccountsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleDriveAuths = googleDriveAuthRepo.GetAllGoogleDriveAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleDriveAuths = googleDriveAuths;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.GetGoogleDriveAccountsById)}] - Message: [{result.Message}]");

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

        // POST: GoogleDriveAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<GoogleDriveAccountDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleDriveAccountRepo.DeleteGoogleDriveAccountById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.DeleteGoogleDriveAccountById)}] - Message: [{result.Message}]");

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

        private bool GoogleDriveAccountExists(long id)
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
                    var result = googleDriveAccountRepo.GetGoogleDriveAccountsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAccountRepo.GetGoogleDriveAccountsById)}] - Message: [{result.Message}]");

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
