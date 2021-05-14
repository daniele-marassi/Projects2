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
    public class TagsController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly TagsRepository tagRepo;
        private readonly EventsRepository eventRepo;
        private readonly NodesRepository nodeRepo;
        private readonly Utility utility;

        public TagsController()
        {
            tagRepo = new TagsRepository();
            eventRepo = new EventsRepository();
            nodeRepo = new NodesRepository();
            utility = new Utility();
        }

        // GET: Tags
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new TagResult() { Data = new List<TagDto>() { }, Successful = false };
                IEnumerable<TagDto> data;

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

                    result = await tagRepo.GetAllTags(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetAllTags)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var nodes = nodeRepo.GetAllNodes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var node in nodes)
                        {
                            node.NodeName = node.Name;
                        }
                        row.Nodes = nodes;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);
                    var _bool = default(bool);

                    Boolean.TryParse(searchString, out _bool);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val
                            || _.NodeId == val
                        );
                    }
                    else if (_bool != default(bool))
                    {
                        data = data.Where(_ => _.Enable == _bool);
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _timerIds = nodes.Where(_ => _.NodeName.ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();

                        data = data.Where(_ => _.Name.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Description.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Address.ToUpper().Contains(searchString.ToUpper().Trim())
                            || _timerIds.Contains(_.NodeId)
                                               );
                    }

                    switch (sortOrder)
                    {
                        case "Name":
                            data = data.OrderBy(_ => _.Name);
                            break;
                        case "Description":
                            data = data.OrderBy(_ => _.Description);
                            break;
                        case "NodeName":
                            data = data.OrderBy(x => x.Nodes.Where(_ => _.Id == x.NodeId).Select(_ => _.NodeName).FirstOrDefault());
                            break;
                        case "Address":
                            data = data.OrderBy(_ => _.Address);
                            break;
                        case "Enable":
                            data = data.OrderBy(_ => _.Enable);
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
                    ModelState.AddModelError("ModelStateErrors", "Tags not found!");
                    return View();
                }
            }
        }

        // GET: Tags/Details/5
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
                    var result = await tagRepo.GetTagsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var nodes = nodeRepo.GetAllNodes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var node in nodes)
                        {
                            node.NodeName = node.Name;
                        }
                        row.Nodes = nodes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetTagsById)}] - Message: [{result.Message}]");

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

        // GET: Tags/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<TagDto>() { };
                    var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                    data.Add(new TagDto() { });

                    var nodes = nodeRepo.GetAllNodes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var node in nodes)
                        {
                            node.NodeName = node.Name;
                        }
                        row.Nodes = nodes;
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

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,NodeId,Address,Enable")] TagDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<TagDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getTagsResult = await tagRepo.GetAllTags(access_token_cookie);
                        if (!getTagsResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetAllTags)}] - Message: [{getTagsResult.Message}]");

                        //Check
                        if (getTagsResult.Data.Where(_ => _.Name.ToLower() == dto.Name.ToLower() && _.Id != dto.Id).ToList().Count > 0) throw new Exception($"Name already exists!");

                        var result = await tagRepo.AddTag(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.AddTag)}] - Message: [{result.Message}]");

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

        // GET: Tags/Edit/5
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
                    var result = await tagRepo.GetTagsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var nodes = nodeRepo.GetAllNodes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var node in nodes)
                        {
                            node.NodeName = node.Name;
                        }
                        row.Nodes = nodes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetTagsById)}] - Message: [{result.Message}]");

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

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,NodeId,Address,Enable")] TagDto dto)
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

                        if (!TagExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = utility.ReadCookie(Request, GeneralSettings.Constants.MairDigitalSuiteAccessTokenCookieName);

                        var getTagsResult = await tagRepo.GetAllTags(access_token_cookie);
                        if (!getTagsResult.Successful)
                            throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetAllTags)}] - Message: [{getTagsResult.Message}]");

                        //Check
                        if (getTagsResult.Data.Where(_ => _.Name.ToLower() == dto.Name.ToLower() && _.Id != id).ToList().Count > 0) throw new Exception($"Name already exists!");

                        var result = await tagRepo.UpdateTag(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.UpdateTag)}] - Message: [{result.Message}]");
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

        // GET: Tags/Delete/5
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
                    var result = await tagRepo.GetTagsById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var nodes = nodeRepo.GetAllNodes(access_token_cookie).Result.Data.ToList();

                    foreach (var row in data)
                    {
                        foreach (var node in nodes)
                        {
                            node.NodeName = node.Name;
                        }
                        row.Nodes = nodes;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetTagsById)}] - Message: [{result.Message}]");

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

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<TagDto>() { };
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
                        if (getEventsResult.Data.Where(_ => _.PlcStartId == id || _.PlcEndId == id || _.PlcAckId == id).ToList().Count > 0) throw new Exception($"Tag used!");

                        var result = await tagRepo.DeleteTagById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.DeleteTagById)}] - Message: [{result.Message}]");

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

        private bool TagExists(long id)
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
                    var result = tagRepo.GetTagsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(tagRepo.GetTagsById)}] - Message: [{result.Message}]");

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
