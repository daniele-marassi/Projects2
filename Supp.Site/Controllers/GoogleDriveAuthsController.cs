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
    public class GoogleDriveAuthsController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly GoogleDriveAuthsRepository googleDriveAuthRepo;
        private readonly SuppUtility suppUtility;

        public GoogleDriveAuthsController()
        {
            googleDriveAuthRepo = new GoogleDriveAuthsRepository();
            suppUtility = new SuppUtility();
        }

        // GET: GoogleDriveAuths
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                GoogleDriveAuthResult result = new GoogleDriveAuthResult() { Data = new List<GoogleDriveAuthDto>() { }, Successful = false };
                IEnumerable<GoogleDriveAuthDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Client_id" : "";

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

                    result = await googleDriveAuthRepo.GetAllGoogleDriveAuths(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.GetAllGoogleDriveAuths)}] - Message: [{result.Message}]");

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
                        data = data.Where(_ => _.Client_id.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Project_id.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Client_secret.ToUpper().Contains(searchString.ToUpper().Trim())
                        );
                    }

                    switch (sortOrder)
                    {
                        case "Id":
                            data = data.OrderBy(_ => _.Id);
                            break;
                        case "Client_id":
                            data = data.OrderBy(_ => _.Client_id);
                            break;
                        case "Project_id":
                            data = data.OrderBy(_ => _.Project_id);
                            break;
                        case "Client_secret":
                            data = data.OrderBy(_ => _.Client_secret);
                            break;
                        default:
                            data = data.OrderBy(_ => _.Client_id);
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
                    ModelState.AddModelError("ModelStateErrors", "GoogleDriveAuths not found!");
                    return View();
                }
            }
        }

        // GET: GoogleDriveAuths/Details/5
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
                    var result = await googleDriveAuthRepo.GetGoogleDriveAuthsById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.GetGoogleDriveAuthsById)}] - Message: [{result.Message}]");

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

        // GET: GoogleDriveAuths/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                return View();
            }
        }

        // POST: GoogleDriveAuths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Client_id,Project_id,Client_secret,InsDateTime")] GoogleDriveAuthDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<GoogleDriveAuthDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleDriveAuthRepo.AddGoogleDriveAuth(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.AddGoogleDriveAuth)}] - Message: [{result.Message}]");

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

        // GET: GoogleDriveAuths/Edit/5
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
                    var result = await googleDriveAuthRepo.GetGoogleDriveAuthsById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.GetGoogleDriveAuthsById)}] - Message: [{result.Message}]");

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

        // POST: GoogleDriveAuths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Client_id,Project_id,Client_secret,InsDateTime")] GoogleDriveAuthDto dto)
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

                        if (!GoogleDriveAuthExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleDriveAuthRepo.UpdateGoogleDriveAuth(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.UpdateGoogleDriveAuth)}] - Message: [{result.Message}]");
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

        // GET: GoogleDriveAuths/Delete/5
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
                    var result = await googleDriveAuthRepo.GetGoogleDriveAuthsById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.GetGoogleDriveAuthsById)}] - Message: [{result.Message}]");

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

        // POST: GoogleDriveAuths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<GoogleDriveAuthDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await googleDriveAuthRepo.DeleteGoogleDriveAuthById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.DeleteGoogleDriveAuthById)}] - Message: [{result.Message}]");

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

        private bool GoogleDriveAuthExists(long id)
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
                    var result = googleDriveAuthRepo.GetGoogleDriveAuthsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(googleDriveAuthRepo.GetGoogleDriveAuthsById)}] - Message: [{result.Message}]");

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
