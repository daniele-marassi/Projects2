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
using GoogleManagerModels;

namespace Supp.Site.Controllers
{
    public class GoogleAccountsController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly GoogleAccountsRepository googleAccountRepo;
        private readonly SuppUtility suppUtility;
        private readonly GoogleAuthsRepository googleAuthRepo;
        private readonly UsersRepository userRepo;

        public GoogleAccountsController()
        {
            googleAccountRepo = new GoogleAccountsRepository();
            suppUtility = new SuppUtility();
            googleAuthRepo = new GoogleAuthsRepository();
            userRepo = new UsersRepository();
        }

        // GET: GoogleAccounts
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                GoogleAccountResult result = new GoogleAccountResult() { Data = new List<GoogleAccountDto>() { }, Successful = false };
                IEnumerable<GoogleAccountDto> data;

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

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    result = await googleAccountRepo.GetAllGoogleAccounts(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.GetAllGoogleAccounts)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleAuths = googleAuthRepo.GetAllGoogleAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleAuths = googleAuths;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val);
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _userIds = users.Where(_ => _.UserFullName.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        var _googleAuthIds = googleAuths.Where(_ => _.Client_id.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        data = data.Where(_ => _.Account.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.FolderToFilter.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.AccountType.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _userIds.Contains(_.UserId)
                            || _googleAuthIds.Contains(_.GoogleAuthId)
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
                        case "GoogleAuthId":
                            data = data.OrderBy(_ => _.GoogleAuthId);
                            break;
                        case "GoogleAuthClient_id":
                            data = data.OrderBy(x => x.GoogleAuths.Where(_ => _.Id == x.GoogleAuthId).Select(_ => _.Client_id).FirstOrDefault());
                            break;
                        case "UserFullName":
                            data = data.OrderBy(x => x.Users.Where(_ => _.Id == x.UserId).Select(_ => _.UserFullName).FirstOrDefault());
                            break;
                        case "AccountType":
                            data = data.OrderBy(_ => _.AccountType);
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
                    ModelState.AddModelError("ModelStateErrors", "GoogleAccounts not found!");
                    return View();
                }
            }
        }

        // GET: GoogleAccounts/Details/5
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
                    var result = await googleAccountRepo.GetGoogleAccountsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleAuths = googleAuthRepo.GetAllGoogleAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleAuths = googleAuths;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.GetGoogleAccountsById)}] - Message: [{result.Message}]");

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

        // GET: GoogleAccounts/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<GoogleAccountDto>() { };
                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    data.Add(new GoogleAccountDto() { });

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleAuths = googleAuthRepo.GetAllGoogleAuths(access_token_cookie).Result.Data.ToList();

                    var accountTypes = Enum.GetValues(typeof(AccountType));

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleAuths = googleAuths;

                        var _accountTypes = new List<(string Id, string AccountType)>() { };

                        foreach (var item in accountTypes)
                        {
                            (string Id, string AccountType) _accountType;
                            _accountType.Id = item.ToString();
                            _accountType.AccountType = item.ToString();

                            _accountTypes.Add(_accountType);
                        }
                        row.AccountTypes = _accountTypes;
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

        // POST: GoogleAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Account,FolderToFilter,GoogleAuthId,UserId,InsDateTime,AccountType")] GoogleAccountDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<GoogleAccountDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleAccountRepo.AddGoogleAccount(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.AddGoogleAccount)}] - Message: [{result.Message}]");

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

        // GET: GoogleAccounts/Edit/5
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
                    var result = await googleAccountRepo.GetGoogleAccountsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleAuths = googleAuthRepo.GetAllGoogleAuths(access_token_cookie).Result.Data.ToList();

                    var accountTypes = Enum.GetValues(typeof(AccountType));

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleAuths = googleAuths;

                        var _accountTypes = new List<(string Id, string AccountType)>() { };

                        foreach (var item in accountTypes)
                        {
                            (string Id, string AccountType) _accountType;
                            _accountType.Id = item.ToString();
                            _accountType.AccountType = item.ToString();

                            _accountTypes.Add(_accountType);
                        }
                        row.AccountTypes = _accountTypes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.GetGoogleAccountsById)}] - Message: [{result.Message}]");

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

        // POST: GoogleAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Account,FolderToFilter,GoogleAuthId,UserId,InsDateTime,AccountType")] GoogleAccountDto dto)
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

                        if (!GoogleAccountExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleAccountRepo.UpdateGoogleAccount(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.UpdateGoogleAccount)}] - Message: [{result.Message}]");
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

        // GET: GoogleAccounts/Delete/5
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
                    var result = await googleAccountRepo.GetGoogleAccountsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var users = userRepo.GetAllUsers(access_token_cookie).Result.Data.ToList();
                    var googleAuths = googleAuthRepo.GetAllGoogleAuths(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var user in users)
                        {
                            user.UserFullName = user.Surname + " " + user.Name;
                        }
                        row.Users = users;
                        row.GoogleAuths = googleAuths;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.GetGoogleAccountsById)}] - Message: [{result.Message}]");

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

        // POST: GoogleAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<GoogleAccountDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleAccountRepo.DeleteGoogleAccountById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.DeleteGoogleAccountById)}] - Message: [{result.Message}]");

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

        private bool GoogleAccountExists(long id)
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
                    var result = googleAccountRepo.GetGoogleAccountsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleAccountRepo.GetGoogleAccountsById)}] - Message: [{result.Message}]");

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

        public IActionResult AddGoogleAccounts()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new GoogleAccountDto() { };
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
    }
}
