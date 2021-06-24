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
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Diagnostics;
using System.Management;
using System.Globalization;

using System.Threading;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Web;
using Additional;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.IO;
using NLog.Time;

namespace Supp.Site.Controllers
{
    public class WebSpeechesController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly WebSpeechesRepository webSpeecheRepo;
        private readonly ExecutionQueuesRepository executionQueueRepo;
        private readonly SuppUtility suppUtility;
        private readonly AuthenticationsRepository authenticationRepo;

        public WebSpeechesController()
        {
            webSpeecheRepo = new WebSpeechesRepository();
            executionQueueRepo = new ExecutionQueuesRepository();
            suppUtility = new SuppUtility();
            authenticationRepo = new AuthenticationsRepository();
        }

        public (List<WebSpeechDto> WebSpeeches, List<ShortcutDto> Shortcuts) GetData(List<WebSpeechDto> data)
        {
            (List<WebSpeechDto> WebSpeeches, List<ShortcutDto> Shortcuts) result;
            result.WebSpeeches = new List<WebSpeechDto>() { };
            result.Shortcuts = new List<ShortcutDto>() { };

            foreach (var item in data)
            {
                if (item.UserId == 0) item.PrivateInstruction = false;
                else item.PrivateInstruction = true;

                if (item.ParentIds != null && item.ParentIds != String.Empty)
                {
                    try
                    {
                        if (item.ParentIds != null && item.ParentIds != String.Empty)
                        {
                            var webSpeechIds = JsonConvert.DeserializeObject<long[]>(item.ParentIds);
                            item.WebSpeechIds = webSpeechIds;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                if (item.WebSpeechIds != null && item.WebSpeechIds.Count() > 0)
                {
                    foreach (var id in item.WebSpeechIds)
                    {
                        var _phrase = data.Where(_ => _.Id == id).Select(_ => _.Phrase).FirstOrDefault();
                        if (item.PreviousPhrase == null) item.PreviousPhrase = String.Empty;
                        if (item.PreviousPhrase != "") item.PreviousPhrase += " ";
                        item.PreviousPhrase += _phrase;
                    }
                }

                if(!item.Type.ToLower().Contains("system"))
                    result.Shortcuts.Add( new ShortcutDto() { Id = item.Id, Type = item.Type, Order = item.Order, Title = item.Name.Replace("_"," "), Action = item.Operation.ToStringExtended().Replace("\\", "/") + " " + item.Parameters.ToStringExtended().Replace("\\", "/"), Ico = item.Ico});
                
                result.WebSpeeches.Add(item);
            }

            return result;
        }

        // GET: WebSpeeches
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                WebSpeechResult result = new WebSpeechResult() { Data = new List<WebSpeechDto>() { }, Successful = false };
                IEnumerable<WebSpeechDto> data;

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

                    string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    result = await webSpeecheRepo.GetAllWebSpeeches(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{result.Message}]");

                    result.Data = GetData(result.Data).WebSpeeches;

                    data = from s in result.Data
                           select s;

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val);
                    }
                    else if (searchString != null && (searchString.Trim().ToLower() == "true" || searchString.Trim().ToLower() == "false"))
                    {
                        data = data.Where(_ => _.FinalStep == bool.Parse(searchString.Trim().ToLower()));
                        data = data.Where(_ => _.OperationEnable == bool.Parse(searchString.Trim().ToLower()));    
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        data = data.Where(_ => _.Name.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Operation.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Parameters.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Host.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Answer.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Ico.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Type.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Phrase.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                        );
                    }

                    switch (sortOrder)
                    {
                        case "Name":
                            data = data.OrderBy(_ => _.Name);
                            break;
                        case "Operation":
                            data = data.OrderBy(_ => _.Operation);
                            break;
                        case "OperationEnable":
                            data = data.OrderBy(_ => _.OperationEnable);
                            break;
                        case "Parameters":
                            data = data.OrderBy(_ => _.Parameters);
                            break;
                        case "Host":
                            data = data.OrderBy(_ => _.Host);
                            break;
                        case "Answer":
                            data = data.OrderBy(_ => _.Answer);
                            break;
                        case "Ico":
                            data = data.OrderBy(_ => _.Ico);
                            break;
                        case "Order":
                            data = data.OrderBy(_ => _.Order);
                            break;
                        case "FinalStep":
                            data = data.OrderBy(_ => _.FinalStep);
                            break;
                        case "Type":
                            data = data.OrderBy(_ => _.Type);
                            break;
                        case "Phrase":
                            data = data.OrderBy(_ => _.Phrase);
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
                    ModelState.AddModelError("ModelStateErrors", "WebSpeeches not found!");
                    return View();
                }
            }
        }

        // GET: WebSpeeches/Details/5
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
                    var result = await webSpeecheRepo.GetWebSpeechesById((long)id, access_token_cookie);

                    result.Data = GetData(result.Data).WebSpeeches;

                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetWebSpeechesById)}] - Message: [{result.Message}]");

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

        public IEnumerable<ShortcutImage> GetShortcutImages()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var shortcutImages = new List<ShortcutImage>() { };
                try
                {
                    var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

                    path = path.Replace(@"file:\", "");
                    path = path.Replace(@"bin\Debug\netcoreapp3.1", "");

                    var d = new DirectoryInfo(Path.Combine(path, @"wwwroot\Images\Shortcuts"));
                    FileInfo[] Files = d.GetFiles("*.png");

                    foreach (FileInfo file in Files)
                    {
                        var ico = file.FullName.Replace(Path.Combine(path, "wwwroot"), "").Replace(@"\", "/");
                        shortcutImages.Add(new ShortcutImage() { Id = ico, Name = file.Name });
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                return shortcutImages;
            }
        }

        // GET: WebSpeeches/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new List<WebSpeechDto>() { };
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    data.Add(new WebSpeechDto() { });

                    var getAllWebSpeechesResult = webSpeecheRepo.GetAllWebSpeeches(access_token_cookie).Result;
                    if (!getAllWebSpeechesResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{getAllWebSpeechesResult.Message}]");

                    var webSpeeches = getAllWebSpeechesResult.Data.ToList();

                    var shortcutImages = GetShortcutImages();

                    foreach (var row in data)
                    {
                        row.Id = 0;
                        row.Name = String.Empty;
                        row.Phrase = String.Empty;
                        row.Operation = String.Empty;
                        row.OperationEnable = true;
                        row.Parameters = String.Empty;
                        row.Host = String.Empty;
                        row.Answer = String.Empty;
                        row.FinalStep = true;     
                        row.UserId = 0;
                        row.ParentIds = String.Empty;
                        row.PrivateInstruction = true;
                        row.WebSpeeches = webSpeeches;
                        row.WebSpeechIds = new long[] { };
                        row.ShortcutImages = shortcutImages;
                    }

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    return View(data);
                }
            }
        }

        // POST: WebSpeeches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phrase,Operation,OperationEnable,Parameters,Host,Answer,WebSpeechIds,FinalStep,PrivateInstruction,Ico,Order,Type,InsDateTime")] WebSpeechDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<WebSpeechDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var claims = SuppUtility.GetClaims(User);
                        if (dto.PrivateInstruction == true) dto.UserId = claims.UserId;
                        else dto.UserId = 0;

                        var result = await webSpeecheRepo.AddWebSpeech(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.AddWebSpeech)}] - Message: [{result.Message}]");

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

        // GET: WebSpeeches/Edit/5
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
                    var result = await webSpeecheRepo.GetWebSpeechesById((long)id, access_token_cookie);

                    result.Data = GetData(result.Data).WebSpeeches;

                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetWebSpeechesById)}] - Message: [{result.Message}]");

                    var getAllWebSpeechesResult = webSpeecheRepo.GetAllWebSpeeches(access_token_cookie).Result;
                    if (!getAllWebSpeechesResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{getAllWebSpeechesResult.Message}]");

                    getAllWebSpeechesResult.Data = GetData(getAllWebSpeechesResult.Data).WebSpeeches;

                    var webSpeeches = getAllWebSpeechesResult.Data.ToList();

                    var shortcutImages = GetShortcutImages();

                    data.WebSpeeches = webSpeeches;
                    data.ShortcutImages = shortcutImages;

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

        // POST: WebSpeeches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Phrase,Operation,OperationEnable,Parameters,Host,Answer,WebSpeechIds,FinalStep,PrivateInstruction,Ico,Order,Type,InsDateTime")] WebSpeechDto dto)
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

                        if (!WebSpeecheExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var parentIds = JsonConvert.SerializeObject(dto.WebSpeechIds);
                        dto.ParentIds = parentIds;

                        var claims = SuppUtility.GetClaims(User);
                        if (dto.PrivateInstruction == true) dto.UserId = claims.UserId;
                        else dto.UserId = 0;

                        var result = await webSpeecheRepo.UpdateWebSpeech(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.UpdateWebSpeech)}] - Message: [{result.Message}]");
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

        // GET: WebSpeeches/Delete/5
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
                    var result = await webSpeecheRepo.GetWebSpeechesById((long)id, access_token_cookie);

                    result.Data = GetData(result.Data).WebSpeeches;

                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetWebSpeechesById)}] - Message: [{result.Message}]");

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

        // POST: WebSpeeches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<WebSpeechDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await webSpeecheRepo.DeleteWebSpeechById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.DeleteWebSpeechById)}] - Message: [{result.Message}]");

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

        private bool WebSpeecheExists(long id)
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
                    var result = webSpeecheRepo.GetWebSpeechesById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetWebSpeechesById)}] - Message: [{result.Message}]");

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

        // GET: WebSpeeches/Recognition
        public async Task<IActionResult> Recognition(string _phrase, string _hostSelected, bool? _reset, string _userName, string _password, bool? _application, long? _executionQueueId, bool? _alwaysShow, long? _id, bool? _onlyRefresh, string _subType, int? _step, bool? _login)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                WebSpeechDto data = null;
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;
                    var rnd = new Random();
                    var application = false;
                    var reset = false;
                    var login = false;
                    var resetAfterLoad = false;
                    var alwaysShow = false;
                    var onlyRefresh = false;
                    var hostSelected = "";
                    long executionQueueId = 0;
                    long.TryParse(_executionQueueId?.ToString(), out executionQueueId);
                    long id = 0;
                    long.TryParse(_id?.ToString(), out id);
                    bool.TryParse(_reset?.ToString(), out reset);
                    bool.TryParse(_login?.ToString(), out login);
                    bool.TryParse(_onlyRefresh?.ToString(), out onlyRefresh);
                    int step = 0;
                    int.TryParse(_step?.ToString(), out step);

                    if (_userName == "null") _userName = null;
                    if (_password == "null") _password = null;
                    if (_phrase == "null") _phrase = null;
                    if (_hostSelected == "null") _hostSelected = null;
                    if (_subType == "null") _subType = null;

                    var expiresInSeconds = 0;
                    var claims = new ClaimsDto() { IsAuthenticated = false };

                    if (_userName != null && _userName != "" && _password != null && _password != "" && login == true)
                    {
                        var dto = new LoginDto() { UserName = _userName, Password = _password };
                        var authenticationResult = HomeController.Authentication(dto, nLogUtility, authenticationRepo, HttpContext, User, Response, Request);
                        resetAfterLoad = true;
                    }

                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName), out expiresInSeconds);

                    var loadDateString = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);

                    if (_onlyRefresh == null) resetAfterLoad = true;

                    if (loadDateString != null && loadDateString != "" && resetAfterLoad== false && onlyRefresh == false)
                    {
                        DateTime loadDate;
                        DateTime.TryParse(loadDateString, out loadDate);

                        DateTime now = DateTime.Now;
                        TimeSpan difference = now.Subtract(loadDate); 
                        if (difference.TotalSeconds >= 2 ) 
                        {
                            resetAfterLoad = true;
                        }
                    }

                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteApplicationCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName);

                    if(_onlyRefresh != null) suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteLoadDateCookieName, DateTime.Now.ToString(), expiresInSeconds);

                    if (_hostSelected != null && _hostSelected != "")
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteHostSelectedCookieName, _hostSelected, expiresInSeconds);
                        hostSelected = _hostSelected;
                    }
                    else
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteHostSelectedCookieName, GeneralSettings.Static.HostDefault, expiresInSeconds);
                        hostSelected = GeneralSettings.Static.HostDefault;
                    }

                    if (_application != null)
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteApplicationCookieName, _application.ToString(), expiresInSeconds);
                        bool.TryParse(_application?.ToString(), out application);
                    }

                    if (_alwaysShow != null)
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName, _alwaysShow.ToString(), expiresInSeconds);
                        bool.TryParse(_alwaysShow?.ToString(), out application);
                    }

                    claims = SuppUtility.GetClaims(User);

                    if (resetAfterLoad == false) data = GetWebSpeechDto(_phrase, hostSelected, reset, application, executionQueueId, alwaysShow, id, claims, onlyRefresh, _subType, step, expiresInSeconds).GetAwaiter().GetResult();
                    else
                    {
                        data = new WebSpeechDto() { };

                        data.Id = id;
                        data.HostSelected = hostSelected;
                        data.Application = application;
                        data.AlwaysShow = alwaysShow;
                        data.ExecutionQueueId = executionQueueId;
                    }

                    data.ResetAfterLoad = resetAfterLoad;

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    suppUtility.AddErrorToCookie(Request, Response, ex.Message);

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: WebSpeeches/RecognitionInJson
        public async Task<WebSpeechDto> GetWebSpeechDto(string _phrase, string _hostSelected, bool _reset, bool _application, long _executionQueueId, bool _alwaysShow, long _id, ClaimsDto _claims, bool _onlyRefresh, string _subType, int _step, int expiresInSeconds)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                WebSpeechDto data = null;
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;
                    WebSpeechResult result = null;
                    List<WebSpeechDto> _data = null;
                    List<ShortcutDto> shortcuts = new List<ShortcutDto>() { };
                    var rnd = new Random();
                    var _phraseMatch = "";
                    var startAnswer = "";
                    var _wordsCount = 0;
                    string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    result = await webSpeecheRepo.GetAllWebSpeeches(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{result.Message}]");

                    var getDataResult = GetData(result.Data);

                    result.Data = getDataResult.WebSpeeches;
                    shortcuts = getDataResult.Shortcuts.OrderBy(_ => _.Order).ToList();

                    if (_phrase != "" && _phrase != null && _step <=3)
                    {
                        var _words = _phrase.Split(" ");
                        _wordsCount = _words.Count();
                        var countMatch = 0;
                        var match = 0;
                        var perferctMatch = false;
                        var skipMatch = false;

                        foreach (var item in result.Data.ToList())
                        {
                            var phrases = new List<string>() { };

                            try
                            {
                                phrases = JsonConvert.DeserializeObject<List<string>>(item.Phrase);
                            }
                            catch (Exception)
                            {
                                phrases.Add(item.Phrase);
                            }

                            for (int i = 0; i < phrases.Count; i++)
                            {
                                if (item.PreviousPhrase != null && item.PreviousPhrase != String.Empty)
                                    phrases[i] = item.PreviousPhrase + " " + phrases[i];
                            }

                            foreach (var phrase in phrases)
                            {
                                var words = phrase.Split(" ");
                                var wordsCount = words.Count();
                                var minMatch = 0;
                                var maxWords = 0;

                                minMatch = (int)(_wordsCount - Math.Ceiling(_wordsCount * decimal.Parse(_claims.Configuration.Speech.MinSpeechWordsCoefficient)));
                                maxWords = (int)(_wordsCount + Math.Ceiling(_wordsCount * decimal.Parse(_claims.Configuration.Speech.MaxSpeechWordsCoefficient)));

                                if (_wordsCount == 2) minMatch = 2;
                                if (minMatch == 0) minMatch = 1;

                                if (item.Type == "SystemWebSearch" && _phrase.Trim().StartsWith(phrase) && skipMatch == false)
                                {
                                    skipMatch = true;
                                    _data = new List<WebSpeechDto>();
                                    _data.Add(item);
                                }

                                if (skipMatch == false)
                                {
                                    match = 0;

                                    for (int x = 0; x < words.Length; x++)
                                    {
                                        for (int y = 0; y < _words.Length; y++)
                                        {
                                            if (_words[y].Trim().ToLower() == words[x].Trim().ToLower() && words[x] != "")
                                            {
                                                match++;
                                                words[x] = "";
                                            }
                                        }
                                    }

                                    if ((match > countMatch || match == _wordsCount) && perferctMatch == false && wordsCount <= maxWords)
                                    {
                                        countMatch = match;
                                        _data = null;
                                    }

                                    if (match >= minMatch && wordsCount <= maxWords && (perferctMatch == false || (perferctMatch == true && match == _wordsCount && match == wordsCount)))
                                    {
                                        if (match == _wordsCount && match == wordsCount) perferctMatch = true;
                                        if (_data == null) _data = new List<WebSpeechDto>();
                                        _data.Add(item);
                                        _phraseMatch = phrase;
                                    }
                                }
                            }
                        }

                        if (_data != null)
                        {
                            int x = rnd.Next(0, _data.Count());

                            data = _data[x];

                            var answers = new List<string>() { };

                            try
                            {
                                answers = JsonConvert.DeserializeObject<List<string>>(data.Answer);
                            }
                            catch (Exception)
                            {
                                answers.Add(data.Answer);
                            }

                            x = rnd.Next(0, answers.Count());

                            data.Answer = answers[x];

                            if (data.Answer == null) data.Answer = "";
                        }
                    }
                    else if (_id != 0)
                    {
                        data = result.Data.Where(_ => _.Id == _id).FirstOrDefault();
                    }

                    if (data != null && (data.Type == "SystemRunExe" || data.Type == "RunExe") && data.OperationEnable == true)
                    {
                        var executionQueue = new ExecutionQueueDto() { FullPath = data.Operation, Arguments = data.Parameters, Host = _hostSelected, Type = data.Type };
                        var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);

                        if (addExecutionQueueResult.Successful)
                        {
                            _executionQueueId = addExecutionQueueResult.Data.FirstOrDefault().Id;
                        }
                    }

                    if (data != null && data.Type == "Meteo")
                    {
                        var phrase = _phrase;
                        if (phrase == null) phrase = _phraseMatch;
                        data.Answer = GetMeteoPhrase(phrase, data.Parameters, _claims.Configuration.General.Culture.ToLower(), true);
                    }

                    if (data != null && data.Type == "Time")
                    {
                        var now = DateTime.Now;

                        var dayofweek = now.ToString("dddd", new CultureInfo(_claims.Configuration.General.Culture));
                        var month = now.ToString("MMMM", new CultureInfo(_claims.Configuration.General.Culture));

                        if (_claims.Configuration.General.Culture.ToLower() == "it-it")
                            data.Answer = now.Hour.ToString() + " e " + now.Minute.ToString() + " minuti" + ", " + dayofweek + " " + now.Day.ToString() + " " + month;

                        if (_claims.Configuration.General.Culture.ToLower() == "en-us")
                            data.Answer = now.Hour.ToString() + " and " + now.Minute.ToString() + " minutes" + ", " + dayofweek + " " + now.Day.ToString() + " " + month;
                    }

                    if (data != null && data.Type == "SystemWebSearch")
                    {
                        var phrases = new List<string>() { };
                        var phrase = _phrase;

                        try
                        {
                            phrases = JsonConvert.DeserializeObject<List<string>>(data.Phrase);
                        }
                        catch (Exception)
                        {
                            phrases.Add(data.Phrase);
                        }

                        foreach (var item in phrases)
                        {
                            phrase = phrase.Replace(item, "");
                        }
                        //HttpUtility.UrlEncode(phrase.Replace(" ", "+"));
                        string url = "http://www.google.com/search?q=" + phrase.Trim().Replace(" ", "+");
                        data.Parameters = url;
                    }

                    var salutation = _claims.Configuration.Speech.Salutation;
                    if (_claims.Name == null && _claims.Configuration.General.Culture.ToLower() == "it-it") _claims.Name = "tu";
                    if (_claims.Name == null && _claims.Configuration.General.Culture.ToLower() == "en-us") _claims.Name = "you";
                    if (_claims.Surname == null) _claims.Surname = String.Empty;
                    salutation = salutation.Replace("NAME", _claims.Name);
                    salutation = salutation.Replace("SURNAME", _claims.Surname);

                    startAnswer = salutation + " " + SuppUtility.GetSalutation(new CultureInfo(_claims.Configuration.General.Culture, false));

                    if ((_phrase == null || _phrase == "") && data == null && _reset == false && _onlyRefresh == false && (_subType == null || _subType == ""))
                    {
                        data = new WebSpeechDto() { Answer = startAnswer, Ehi = 0, FinalStep = true };

                        var now = DateTime.Now;

                        if (_claims.Configuration.Speech.MeteoParameterToTheSalutation != null && _claims.Configuration.Speech.MeteoParameterToTheSalutation != "" && _application == true && SuppUtility.GetPartOfTheDay(now) == PartsOfTheDayEng.Morning)
                        {
                            data.Answer += GetMeteoPhrase(String.Empty, _claims.Configuration.Speech.MeteoParameterToTheSalutation, _claims.Configuration.General.Culture.ToLower(), _claims.Configuration.Speech.DescriptionMeteoToTheSalutationActive);
                        }
                    }
                    
                    if (
                        (_phrase != null && _phrase != "" && data == null && result != null && _wordsCount > 1)
                        || ((data == null || (data != null && data.Answer == "")) && result != null && _subType == "RequestNotImplemented")       
                    )
                    {
                        data = new WebSpeechDto() { };
                        if (_step < 1)
                        { 
                            _step = 1;
                            suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                            var newWebSpeech = new WebSpeechDto() {Name = "NewImplemented_"+DateTime.Now.ToString("yyyyMMddhhmmss"), Phrase = _phrase };

                            var newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);

                            suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);
                        }
                        if (_subType == null || _subType == "") _subType = "RequestNotImplemented";

                        if (data != null && (data.Answer == "" || data.Answer == null) && _step > 1) _step++;

                        data = result.Data.Where(_ => _.Name == _subType +"_"+_step.ToString()).FirstOrDefault();
                    }

                    if (data != null && data.Name != null && data.Name.Contains("RequestNotImplemented"))
                    {
                        data.SubType = data.Name.Split("_")[0].ToString();
                        data.Step = int.Parse(data.Name.Split("_")[1].ToString());

                        var newWebSpeech = new WebSpeechDto() { };

                        if (_step >= 4 && data?.FinalStep != true)
                        {
                            var newWebSpeechString = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);

                            if (newWebSpeechString != "")
                            {
                                try
                                {
                                    newWebSpeech = JsonConvert.DeserializeObject<WebSpeechDto>(newWebSpeechString);
                                }
                                catch (Exception)
                                {
                                }

                                if (_step == 4) newWebSpeech.Answer = _phrase;

                                newWebSpeechString = JsonConvert.SerializeObject(newWebSpeech);
                                suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                                suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName, newWebSpeechString, expiresInSeconds);

                            }
                            else if (_step >= 4 && data?.FinalStep == true)
                            {
                                suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);



                            }
                        }
                    }

                    if (data == null) data = new WebSpeechDto() { Answer = "", Ehi = 0 };

                    data.HostsArray = _claims.Configuration.Speech.HostsArray;
                    data.HostSelected = _claims.Configuration.Speech.HostDefault;
                    data.ListeningWord1 = _claims.Configuration.Speech.ListeningWord1;
                    data.ListeningWord2 = _claims.Configuration.Speech.ListeningWord2;
                    data.ListeningAnswer = _claims.Configuration.Speech.ListeningAnswer;
                    data.Culture = _claims.Configuration.General.Culture;
                    data.StartAnswer = startAnswer;
                    data.Application = _application;
                    data.AlwaysShow = _alwaysShow;
                    data.ExecutionQueueId = _executionQueueId;
                    data.TimesToReset = _claims.Configuration.Speech.TimesToReset;

                    if ((_phrase != null && _phrase != "") && (data.FinalStep == false || _phrase == (data.ListeningWord1 + " " + data.ListeningWord2))) data.Ehi = 1;

                    if (_reset == true && _alwaysShow == false)
                    {
                        if (_hostSelected == null || _hostSelected == String.Empty) _hostSelected = _claims.Configuration.Speech.HostDefault;
                        await ExecutionFinished(_executionQueueId, _hostSelected, _application);
                    }

                    var shortcutsInJson = JsonConvert.SerializeObject(shortcuts);

                    data.ShortcutsInJson = shortcutsInJson;

                    data.Error = null;

                    return data;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // GET: WebSpeeches/RecognitionInJson
        public async Task<string> GetWebSpeechDtoInJson(string _phrase, string _hostSelected, bool? _reset, bool? _application, long? _executionQueueId, bool? _alwaysShow, long? _id, bool? _onlyRefresh, string _subType, int? _step)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = "";

                try
                {
                    var reset = false;
                    var application = false;
                    var alwaysShow = false;
                    var onlyRefresh = false;
                    var hostSelected = "";
                    long executionQueueId = 0;
                    long.TryParse(_executionQueueId?.ToString(), out executionQueueId);
                    long id = 0;
                    long.TryParse(_id?.ToString(), out id);
                    bool.TryParse(_reset?.ToString(), out reset);
                    bool.TryParse(_onlyRefresh?.ToString(), out onlyRefresh);
                    int step = 0;
                    int.TryParse(_step?.ToString(), out step);

                    if (_phrase == "null") _phrase = null;
                    if (_hostSelected == "null") _hostSelected = null;
                    if (_subType == "null") _subType = null; 

                    var expiresInSeconds = 0;
                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName), out expiresInSeconds);

                    var claims = new ClaimsDto() { IsAuthenticated = false };

                    if (_hostSelected == null) hostSelected = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);
                    else hostSelected = _hostSelected;

                    if (_application == null) bool.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteApplicationCookieName), out application);
                    else bool.TryParse(_application?.ToString(), out application);

                    if (_alwaysShow == null) bool.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName), out alwaysShow);
                    else bool.TryParse(_alwaysShow?.ToString(), out alwaysShow);

                    try
                    {
                        var claimsString = suppUtility.ReadCookie(Request, Config.GeneralSettings.Constants.SuppSiteClaimsCookieName);
                        claims = JsonConvert.DeserializeObject<ClaimsDto>(claimsString);
                    }
                    catch (Exception)
                    {
                        claims = SuppUtility.GetClaims(User);
                    }

                    var data = GetWebSpeechDto(_phrase, hostSelected, reset, application, executionQueueId, alwaysShow, id, claims, onlyRefresh, _subType, step, expiresInSeconds).GetAwaiter().GetResult();

                    if (data != null)
                    {
                        result = System.Text.Json.JsonSerializer.Serialize(data);
                        result = result.Replace(@"\","/");
                    }
                }
                catch (Exception ex)
                {
                    var data = new WebSpeechDto() { };
                    data.Error = nameof(WebSpeechesController.Recognition) + " - " + ex.Message.ToString();
                    logger.Error(ex.ToString());
                    result = JsonConvert.SerializeObject(data);
                }

                return result;
            }
        }

        public string GetMeteoPhrase(string request, string param, string culture, bool descriptionActive)
        {
            var result = "";

            var getMeteoResult = GetMeteo(param).GetAwaiter().GetResult();

            if (getMeteoResult.Error == null)
            {
                dynamic partOfTheDay = PartsOfTheDayIta.NotSet;
                var day = Days.Oggi;

                if (request.Contains(PartsOfTheDayIta.Mattina.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Mattina;
                if (request.Contains(PartsOfTheDayIta2.Mattino.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta2.Mattino;
                if (request.Contains(PartsOfTheDayIta.Pomerriggio.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Pomerriggio;
                if (request.Contains(PartsOfTheDayIta.Sera.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Sera;
                if (request.Contains(PartsOfTheDayIta.Notte.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayIta.Notte;

                if (request.Contains(PartsOfTheDayEng.Morning.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Morning;
                if (request.Contains(PartsOfTheDayEng.Afternoon.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Afternoon;
                if (request.Contains(PartsOfTheDayEng.Evening.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Evening;
                if (request.Contains(PartsOfTheDayEng.Night.ToString(), StringComparison.InvariantCultureIgnoreCase)) partOfTheDay = PartsOfTheDayEng.Night;

                if (param.Contains(Days.Oggi.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Today;
                if (param.Contains(Days.Domani.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Tomorrow;

                if (param.Contains(Days.Today.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Today;
                if (param.Contains(Days.Tomorrow.ToString(), StringComparison.InvariantCultureIgnoreCase)) day = Days.Tomorrow;

                var meteo = MeteoManage(getMeteoResult.Data, culture, partOfTheDay, day, descriptionActive).ToString();

                result = meteo;
            }
            else
            {
                if(culture == "it-it")
                    result = " Non riesco a leggere il meteo.";
                if (culture == "en-us")
                    result = " I can't read the weather.";
            }

            return result;
        }

        public string MeteoManage(JObject src, string culture, dynamic partOfTheDay, Days day, bool descriptionActive)
        {
            var result = "";
            var description = "";
            var now = DateTime.Now;
            var hour = now.Hour;
            JToken details = null;

            if ( descriptionActive && day == Days.Tomorrow) description = src["data"]["weatherReportTomorrow"]["description"].ToString();
            if ( descriptionActive && (day == Days.Today || description == "" || description == " ")) description = src["data"]["weatherReportToday"]["description"].ToString();

            description = description.Replace("-"," ");
            description = description.Replace(System.Environment.NewLine, " ");

            if (partOfTheDay.ToString() != PartsOfTheDayIta.NotSet.ToString()) hour = (int)partOfTheDay;

            details = src["data"]["hours"][hour];

            if (culture.Trim().ToLower() == "it-it")
            {
                result = " Ecco le previsioni: ";

                result += description;

                result += " Temperatura " + details["temperature"].ToString().Replace(",", " e ") + " gradi";

                result += ", umidità " + details["umidity"].ToString().Replace(",", " e ") + " percento";

                result += " e vento " + details["windIntensity"].ToString().Replace(",", " e ") + " chilometri orari.";
            }

            if (culture.Trim().ToLower() == "en-us")
            {
                result = " Here are the forecasts: ";

                result += description;

                result += " Temperature " + details["temperature"].ToString().Replace(",", " and ") + " degrees";

                result += ", umidity " + details["umidity"].ToString().Replace(",", " and ") + " percent";

                result += " and wind " + details["windIntensity"].ToString().Replace(",", " and ") + " kilometers per hour.";
            }

            result = result.Replace("&amp;", "&");

            result = System.Net.WebUtility.HtmlDecode(result);

            result = result.Replace("a'", "à");
            result = result.Replace("e'", "è");
            result = result.Replace("o'", "ò");
            result = result.Replace("u'", "ù");
            result = result.Replace("i'", "ì");

            return result;
        }

        // GET: WebSpeeches/ExecutionFinished
        public async Task ExecutionFinished(long _id, string _hostSelected, bool _application)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (_id == 0 && _application)
                {
                    try
                    {
                        string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var executionQueue = new ExecutionQueueDto() { FullPath = "*", Arguments = "*", Host = _hostSelected, Type = "ForceHideApplication", StateQueue = ExecutionQueueStateQueue.RunningStep2.ToString() };
                        var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }

                if (_id != 0)
                {
                    try
                    {
                        string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                        var getExecutionQueuesByIdResult = await executionQueueRepo.GetExecutionQueuesById(_id, access_token_cookie);
                        var executionQueue = getExecutionQueuesByIdResult.Data.FirstOrDefault();
                        executionQueue.StateQueue = ExecutionQueueStateQueue.RunningStep2.ToString();
                        var addExecutionQueueResult = await executionQueueRepo.UpdateExecutionQueue(executionQueue, access_token_cookie);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }
            }
        }

        // GET: WebSpeeches/GetMeteo
        public async Task<(JObject Data, string Error)> GetMeteo(string _value)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                (JObject Data, string Error) response;
                response.Data = null;
                response.Error = null;
                try
                {
                    var keyValuePairs = new Dictionary<string, string>() { };

                    keyValuePairs["value"] = _value;
                    var utility = new Utility();

                    var result = await utility.CallApi(HttpMethod.Get, "http://supp.altervista.org/", "GetMeteo.php", keyValuePairs, null);
                    var content = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode == false)
                    {
                        response.Data = null;
                        response.Error = result.ReasonPhrase;
                    }
                    else
                    {
                        content = content.Replace("<meta http-equiv=\"Access-Control-Allow-Origin\" content=\"*\">\n", "");
                       
                        var data = (JObject)JsonConvert.DeserializeObject(content);

                        response.Data = data;
                        response.Error = null;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    response.Data = null;
                    response.Error = ex.Message;
                }

                return response;
            }
        }
    }
}
