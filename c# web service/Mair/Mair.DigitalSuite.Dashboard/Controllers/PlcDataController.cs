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
using AutoMapper;
using Mair.DigitalSuite.Dashboard.Services.SignalR;
using Mair.DigitalSuite.Dashboard.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Mair.DigitalSuite.Dashboard.Models.Dto.Automation;
using Mair.DigitalSuite.Dashboard.Models.Result.Automation;
using Mair.DigitalSuite.Dashboard.Models.Result;
using Mair.DigitalSuite.Dashboard.Models.Dto;

namespace Mair.DigitalSuite.Dashboard.Controllers
{
    public class PlcDataController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly PlcDataRepository plcDataRepo;
        private readonly Utility utility;

        public PlcDataController()
        {
            plcDataRepo = new PlcDataRepository();
            utility = new Utility();
        }

        // GET: PlcData
        public async Task<IActionResult> PlcDataList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new PlcDataResult() { Data = new List<PlcDataDto>() { }, Successful = false };
                IEnumerable<PlcDataDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Driver" : "";

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

                    result = await plcDataRepo.GetPlcData(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(plcDataRepo.GetPlcData)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val

                        );
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        data = data.Where(_ => _.Driver.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.ConnectionString.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.TagValue.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.TagAddress.ToUpper().Contains(searchString.ToUpper().Trim())
                        );
                    }

                    switch (sortOrder)
                    {
                        case "ConnectionString":
                            data = data.OrderBy(_ => _.ConnectionString);
                            break;
                        case "Driver":
                            data = data.OrderBy(_ => _.Driver);
                            break;
                        case "TagAddress":
                            data = data.OrderBy(_ => _.TagAddress);
                            break;
                        case "TagValue":
                            data = data.OrderBy(_ => _.TagValue);
                            break;
                        default:
                            data = data.OrderBy(_ => _.Driver);
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
                    ModelState.AddModelError("ModelStateErrors", "PlcData not found!");
                    return View();
                }
            }
        }

        // GET: PlcData/Edit/5
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
                    var result = await plcDataRepo.GetPlcData(access_token_cookie);
                    var data = result.Data.Where(_ => _.Id == id).FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(plcDataRepo.GetPlcData)}] - Message: [{result.Message}]");

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

        // POST: PlcData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Driver,ConnectionString,TagAddress,TagValue")] PlcDataDto dto)
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

                        if (!PlcDataExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                        var result = await plcDataRepo.UpdatePlcData(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(plcDataRepo.UpdatePlcData)}] - Message: [{result.Message}]");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        return View(dto);
                    }
                    return RedirectToAction("PlcDataList", "PlcData");
                }
                return View(dto);
            }
        }

        private bool PlcDataExists(long id)
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
                    var result = plcDataRepo.GetPlcData(access_token_cookie).Result;
                    var data = result.Data.Where(_ => _.Id == id).FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(plcDataRepo.GetPlcData)}] - Message: [{result.Message}]");

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

        // GET: DashBoard
        public async Task<IActionResult> DashBoard()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new DashBoardDataResult() { Data = new List<DashBoardDataDto>() { }, Successful = false };
                List<DashBoardDataDto> data = new List<DashBoardDataDto>() { };

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                    ViewBag.AccessTokenCookie = access_token_cookie;

                    return View();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    utility.AddErrorToCookie(Request, Response, ex.Message);

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: DashBoard
        public async Task<IActionResult> DashBoardBlazorCharts()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                DashBoardDataResult result = new DashBoardDataResult() { Data = new List<DashBoardDataDto>() { }, Successful = false };
                List<DashBoardDataDto> data = new List<DashBoardDataDto>() { };

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                    ViewBag.AccessTokenCookie = access_token_cookie;

                    return View();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    utility.AddErrorToCookie(Request, Response, ex.Message);

                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}
