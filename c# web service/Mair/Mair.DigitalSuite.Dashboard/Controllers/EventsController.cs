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
using Mair.DigitalSuite.Dashboard.Models.Dto.Automation;
using Mair.DigitalSuite.Dashboard.Models.Result.Automation;

namespace Mair.DigitalSuite.Dashboard.Controllers
{
    public class EventsController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly EventsRepository eventRepo;
        private readonly TimersRepository timerRepo;
        private readonly TagsRepository tagRepo;
        private readonly Utility utility;

        public EventsController()
        {
            eventRepo = new EventsRepository();
            timerRepo = new TimersRepository();
            tagRepo = new TagsRepository();
            utility = new Utility();
        }

        // GET: Events
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new EventResult() { Data = new List<EventDto>() { }, Successful = false };
                IEnumerable<EventDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";

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

                    result = await eventRepo.GetAllEvents(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetAllEvents)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var timers = timerRepo.GetAllTimers(access_token_cookie).Result.Data.ToList();
                    var tags = tagRepo.GetAllTags(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var timer in timers)
                        {
                            timer.TimerName = timer.Name;
                        }
                        row.Timers = timers;

                        foreach (var tag in tags)
                        {
                            tag.TagName = tag.Name;
                        }
                        row.Tags = tags;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val
                            || _.Type == (byte) val
                            || _.TimerId == val
                            || _.PlcStartId == val
                            || _.PlcEndId == val
                            || _.PlcAckId == val
                        );
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _timerIds = timers.Where(_ => _.TimerName.ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        var _tagIds = tags.Where(_ => _.TagName.ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();

                        data = data.Where(_ => _.Name.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Description.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _timerIds.Contains(_.TimerId)
                            || _tagIds.Contains(_.PlcStartId)
                            || _tagIds.Contains(_.PlcEndId)
                            || _tagIds.Contains(_.PlcAckId)
                                               );
                    }

                    switch (sortOrder)
                    {
                        case "Id":
                            data = data.OrderBy(_ => _.Id);
                            break;
                        case "Name":
                            data = data.OrderBy(_ => _.Name);
                            break;
                        case "Description":
                            data = data.OrderBy(_ => _.Description);
                            break;
                        case "Type":
                            data = data.OrderBy(_ => _.Type);
                            break;
                        case "TimerName":
                            data = data.OrderBy(x => x.Timers.Where(_ => _.Id == x.TimerId).Select(_ => _.TimerName).FirstOrDefault());
                            break;
                        case "PlcStart":
                            data = data.OrderBy(x => x.Tags.Where(_ => _.Id == x.PlcStartId).Select(_ => _.TagName).FirstOrDefault());
                            break;
                        case "PlcEnd":
                            data = data.OrderBy(x => x.Tags.Where(_ => _.Id == x.PlcEndId).Select(_ => _.TagName).FirstOrDefault());
                            break;
                        case "PlcAck":
                            data = data.OrderBy(x => x.Tags.Where(_ => _.Id == x.PlcAckId).Select(_ => _.TagName).FirstOrDefault());
                            break;
                        default:
                            data = data.OrderBy(_ => _.Name);
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
                    ModelState.AddModelError("ModelStateErrors", "Events not found!");
                    return View();
                }
            }
        }

        // GET: Events/Details/5
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
                    var result = await eventRepo.GetEventsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var timers = timerRepo.GetAllTimers(access_token_cookie).Result.Data.ToList();
                    var tags = tagRepo.GetAllTags(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var timer in timers)
                        {
                            timer.TimerName = timer.Name;
                        }
                        row.Timers = timers;

                        foreach (var tag in tags)
                        {
                            tag.TagName = tag.Name;
                        }
                        row.Tags = tags;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetEventsById)}] - Message: [{result.Message}]");

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

        // GET: Events/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<EventDto>() { };
                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                    data.Add(new EventDto() { });

                    var timers = timerRepo.GetAllTimers(access_token_cookie).Result.Data.ToList();
                    var tags = tagRepo.GetAllTags(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var timer in timers)
                        {
                            timer.TimerName = timer.Name;
                        }
                        row.Timers = timers;

                        foreach (var tag in tags)
                        {
                            tag.TagName = tag.Name;
                        }
                        row.Tags = tags;
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

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Type,TimerId,PlcStartId,PlcEndId,PlcAckId")] EventDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<EventDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getEventsResult = await eventRepo.GetAllEvents(access_token_cookie);
                        if (!getEventsResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetAllEvents)}] - Message: [{getEventsResult.Message}]");

                        //Check
                        if (getEventsResult.Data.Where(_ => _.Name.ToLower() == dto.Name.ToLower() && _.Id != dto.Id).ToList().Count > 0) throw new Exception($"Name already exists!");

                        var result = eventRepo.AddEvent(dto, access_token_cookie).Result;

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.AddEvent)}] - Message: [{result.Message}]");

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

        // GET: Events/Edit/5
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
                    var result = await eventRepo.GetEventsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var timers = timerRepo.GetAllTimers(access_token_cookie).Result.Data.ToList();
                    var tags = tagRepo.GetAllTags(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var timer in timers)
                        {
                            timer.TimerName = timer.Name;
                        }
                        row.Timers = timers;

                        foreach (var tag in tags)
                        {
                            tag.TagName = tag.Name;
                        }
                        row.Tags = tags;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetEventsById)}] - Message: [{result.Message}]");

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

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Type,TimerId,PlcStartId,PlcEndId,PlcAckId")] EventDto dto)
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

                        if (!EventExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getEventsResult = await eventRepo.GetAllEvents(access_token_cookie);
                        if (!getEventsResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetAllEvents)}] - Message: [{getEventsResult.Message}]");

                        //Check
                        if (getEventsResult.Data.Where(_ => _.Name.ToLower() == dto.Name.ToLower() && _.Id != id).ToList().Count > 0) throw new Exception($"Name already exists!");

                        var result = await eventRepo.UpdateEvent(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.UpdateEvent)}] - Message: [{result.Message}]");
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

        // GET: Events/Delete/5
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
                    var result = await eventRepo.GetEventsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var timers = timerRepo.GetAllTimers(access_token_cookie).Result.Data.ToList();
                    var tags = tagRepo.GetAllTags(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var timer in timers)
                        {
                            timer.TimerName = timer.Name;
                        }
                        row.Timers = timers;

                        foreach (var tag in tags)
                        {
                            tag.TagName = tag.Name;
                        }
                        row.Tags = tags;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetEventsById)}] - Message: [{result.Message}]");

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

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<EventDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);
                        var result = await eventRepo.DeleteEventById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.DeleteEventById)}] - Message: [{result.Message}]");

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

        private bool EventExists(long id)
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
                    var result = eventRepo.GetEventsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(eventRepo.GetEventsById)}] - Message: [{result.Message}]");

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
