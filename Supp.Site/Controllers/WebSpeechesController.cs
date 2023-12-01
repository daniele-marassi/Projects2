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
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Diagnostics;
using System.Management;
using System.Globalization;

using System.Threading;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Web;
using Additional;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.IO;
using NLog.Time;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using NLog.Fluent;
using static System.Net.Mime.MediaTypeNames;
using Google.Apis.Calendar.v3.Data;
using NuGet.Frameworks;
using Microsoft.AspNetCore.Hosting.Server;
using GoogleManagerModels;
using static Google.Apis.Requests.BatchRequest;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.Extensions;
using System.Runtime.ConstrainedExecution;

namespace Supp.Site.Controllers
{
    public class WebSpeechesController : Controller
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();
        private readonly WebSpeechesRepository webSpeecheRepo;
        private readonly TokensRepository tokensRepo;
        private readonly ExecutionQueuesRepository executionQueueRepo;
        private readonly SuppUtility suppUtility;
        private readonly AuthenticationsRepository authenticationRepo;
        private readonly Supp.Site.Recognition.Common recognitionCommon;
        private readonly Utility utility;

        public WebSpeechesController()
        {
            webSpeecheRepo = new WebSpeechesRepository();
            executionQueueRepo = new ExecutionQueuesRepository();
            suppUtility = new SuppUtility();
            authenticationRepo = new AuthenticationsRepository();
            tokensRepo = new TokensRepository();
            recognitionCommon = new Recognition.Common();
            utility = new Utility();
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

                    var access_token_cookie = suppUtility.GetAccessToken(Request);

                    result = await webSpeecheRepo.GetAllWebSpeeches(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{result.Message}]");

                    result.Data = recognitionCommon.GetData(result.Data, false, SuppUtility.GetUserIdFromToken(access_token_cookie)).Data;

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
                            || _.SubType.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Phrase.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.StepType.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.GroupName.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
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
                        case "SubType":
                            data = data.OrderBy(_ => _.SubType);
                            break;
                        case "Phrase":
                            data = data.OrderBy(_ => _.Phrase);
                            break;
                        case "StepType":
                            data = data.OrderBy(_ => _.StepType);
                            break;
                        case "ElementIndex":
                            data = data.OrderBy(_ => _.ElementIndex);
                            break;
                        case "Groupable":
                            data = data.OrderBy(_ => _.Groupable);
                            break;
                        case "GroupName":
                            data = data.OrderBy(_ => _.GroupName);
                            break;
                        case "GroupOrder":
                            data = data.OrderBy(_ => _.GroupOrder);
                            break;
                        case "HotShortcut":
                            data = data.OrderBy(_ => _.HotShortcut);
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

                    var access_token_cookie = suppUtility.GetAccessToken(Request);
                    var result = await webSpeecheRepo.GetWebSpeechesById((long)id, access_token_cookie);

                    result.Data = recognitionCommon.GetData(result.Data, false, SuppUtility.GetUserIdFromToken(access_token_cookie)).Data;

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
                    var files = d.GetFiles("*.png").ToList();
                    files.AddRange(d.GetFiles("*.gif").ToList());

                    foreach (FileInfo file in files)
                    {
                        var ico = file.FullName.Replace(Path.Combine(path, "wwwroot"), "").Replace(@"\", "/");
                        shortcutImages.Add(new ShortcutImage() { Id = ico.Trim(), Name = file.Name.Trim() });
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
        public IActionResult Create(string _newWebSpeechRequestName)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var data = new List<WebSpeechDto>() { };
                try
                {
                    WebSpeechDto newWebSpeech = null;
                    if (_newWebSpeechRequestName != null && _newWebSpeechRequestName != String.Empty)
                    {
                        var newWebSpeechDtoInJson = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteNewWebSpeechDtoInJsonCookieName + "_" + _newWebSpeechRequestName);
                        newWebSpeech = JsonConvert.DeserializeObject<WebSpeechDto>(newWebSpeechDtoInJson);
                    }
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = suppUtility.GetAccessToken(Request);

                    data.Add(new WebSpeechDto() { });

                    var getAllWebSpeechesResult = webSpeecheRepo.GetAllWebSpeeches(access_token_cookie).Result;
                    if (!getAllWebSpeechesResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{getAllWebSpeechesResult.Message}]");

                    var webSpeeches = getAllWebSpeechesResult.Data.ToList();

                    var shortcutImages = GetShortcutImages();
                    var identification = SuppUtility.GetIdentification(Request, -1);

                    var hosts = new List<Host>() { };
                    var hostsArray = JsonConvert.DeserializeObject<List<string>>(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.HostsArray);

                    if (hostsArray.Where(_ => _.ToLower() == "all").Count() == 0) hostsArray.Add("All");

                    foreach (var item in hostsArray)
                    {
                        hosts.Add(new Host() { Id = item, Name = item });
                    }

                    foreach (var row in data)
                    {
                        if (newWebSpeech == null)
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
                            row.Hosts = hosts;
                        }
                        else
                        {
                            row.Id = 0;
                            row.Name = newWebSpeech.Name;
                            row.Phrase = newWebSpeech.Phrase;
                            row.Operation = newWebSpeech.Operation;
                            row.OperationEnable = newWebSpeech.OperationEnable;
                            row.Parameters = newWebSpeech.Parameters;
                            row.Host = newWebSpeech.Host;
                            row.Answer = newWebSpeech.Answer;
                            row.FinalStep = newWebSpeech.FinalStep;
                            row.ParentIds = String.Empty;
                            row.PrivateInstruction = newWebSpeech.PrivateInstruction;
                            row.WebSpeeches = webSpeeches;
                            row.WebSpeechIds = new long[] { };
                            row.ShortcutImages = shortcutImages;
                            row.Ico = newWebSpeech.Ico;
                            row.Type = newWebSpeech.Type;
                            row.Order = newWebSpeech.Order;
                            row.Hosts = hosts;

                            if (newWebSpeech.PrivateInstruction == true) row.UserId = identification.UserId;
                            else row.UserId = 0;
                        }
                    }

                    return View(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);

                    return View(data.FirstOrDefault());
                }
            }
        }

        // POST: WebSpeeches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phrase,Operation,OperationEnable,Parameters,Host,Answer,WebSpeechIds,FinalStep,PrivateInstruction,Ico,Order,Type,SubType,Step,StepType,ElementIndex,InsDateTime,Groupable,GroupName,GroupOrder,HotShortcut")] WebSpeechDto dto)
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
                        var access_token_cookie = suppUtility.GetAccessToken(Request);

                        var identification = SuppUtility.GetIdentification(Request, -1);
                        if (dto.PrivateInstruction == true) dto.UserId = identification.UserId;
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

                    var access_token_cookie = suppUtility.GetAccessToken(Request);
                    var result = await webSpeecheRepo.GetWebSpeechesById((long)id, access_token_cookie);

                    result.Data = recognitionCommon.GetData(result.Data, false, SuppUtility.GetUserIdFromToken(access_token_cookie)).Data;

                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetWebSpeechesById)}] - Message: [{result.Message}]");

                    var getAllWebSpeechesResult = webSpeecheRepo.GetAllWebSpeeches(access_token_cookie).Result;
                    if (!getAllWebSpeechesResult.Successful)
                        throw new Exception($"Error [Get failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{getAllWebSpeechesResult.Message}]");

                    getAllWebSpeechesResult.Data = recognitionCommon.GetData(getAllWebSpeechesResult.Data, false, SuppUtility.GetUserIdFromToken(access_token_cookie)).Data;

                    var webSpeeches = getAllWebSpeechesResult.Data.ToList();

                    var shortcutImages = GetShortcutImages();

                    data.WebSpeeches = webSpeeches;
                    data.ShortcutImages = shortcutImages;

                    var identification = SuppUtility.GetIdentification(Request, -1);
                    var hosts = new List<Host>() { };
                    var hostsArray = JsonConvert.DeserializeObject<List<string>>(JsonConvert.DeserializeObject<Configuration>(identification.ConfigInJson).Speech.HostsArray);

                    if (hostsArray.Where(_ => _.ToLower() == "all").Count() == 0) hostsArray.Add("All");

                    foreach (var item in hostsArray)
                    {
                        hosts.Add( new Host() { Id = item, Name = item });
                    }

                    data.Hosts = hosts;

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
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Phrase,Operation,OperationEnable,Parameters,Host,Answer,WebSpeechIds,FinalStep,PrivateInstruction,Ico,Order,Type,SubType,Step,StepType,ElementIndex,InsDateTime,Groupable,GroupName,GroupOrder,HotShortcut")] WebSpeechDto dto)
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

                        var access_token_cookie = suppUtility.GetAccessToken(Request);

                        var parentIds = JsonConvert.SerializeObject(dto.WebSpeechIds);
                        dto.ParentIds = parentIds;

                        var identification = SuppUtility.GetIdentification(Request, -1);
                        if (dto.PrivateInstruction == true) dto.UserId = identification.UserId;
                        else dto.UserId = 0;

                        var result = await webSpeecheRepo.UpdateWebSpeech(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(webSpeecheRepo.UpdateWebSpeech)}] - Message: [{result.Message}]");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);

                        return View();
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

                    var access_token_cookie = suppUtility.GetAccessToken(Request);
                    var result = await webSpeecheRepo.GetWebSpeechesById((long)id, access_token_cookie);

                    result.Data = recognitionCommon.GetData(result.Data, false, SuppUtility.GetUserIdFromToken(access_token_cookie)).Data;

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
                        var access_token_cookie = suppUtility.GetAccessToken(Request);
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

                    var access_token_cookie = suppUtility.GetAccessToken(Request);
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

        private void ManageAuthentication(ref TokenDto identification, ref bool login, ref bool onlyRefresh, ref bool passwordAlreadyEncrypted, ref string _userName, ref string _password, ref bool resetAfterLoad, ref bool application, ref bool reset, long userId, bool checkIfTokenIsValid = false)
        {
            var loginIsNecessary = false;
            if ((identification == null || identification.IsAuthenticated == false))
            {
                identification = SuppUtility.GetIdentification(Request, userId);
                checkIfTokenIsValid = true;
            }
            else if ((identification == null || identification.IsAuthenticated == false) && application == false)
            {
                throw new Exception("Authentication expired! login again");
            }
            else
            {
                try
                {
                    var tokenDtoString = suppUtility.ReadCookie(Request, Config.GeneralSettings.Constants.SuppSiteTokenDtoCookieName);
                    identification = JsonConvert.DeserializeObject<TokenDto>(tokenDtoString);
                    checkIfTokenIsValid = true;
                }
                catch (Exception)
                {

                }
            }

            if (identification == null || identification.IsAuthenticated == false)
            {
                loginIsNecessary = true;
                //login = true;
                //reset = false;
                //onlyRefresh = true;
                resetAfterLoad = true;
            }

            if (checkIfTokenIsValid && identification != null && identification.IsAuthenticated == true)
            {
                var access_token_cookie = suppUtility.GetAccessToken(Request);

                bool? tokenIsValid = tokensRepo.TokenIsValid(access_token_cookie).GetAwaiter().GetResult();
                if (tokenIsValid != true)
                {
                    loginIsNecessary = true;
                    //login = true;
                    //onlyRefresh = true;
                    //reset = false;
                    resetAfterLoad = true;
                }
                else
                    login = false;
            }

            if (loginIsNecessary == true)
            {
                passwordAlreadyEncrypted = false;

                if (_userName == null || _userName == "" || _password == null || _password == "")
                {
                    passwordAlreadyEncrypted = true;
                    _userName = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                    _password = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAuthenticatedPasswordCookieName);
                }

                if (_userName != null && _userName != "" && _password != null && _password != "")
                {
                    var dto = new LoginDto() { UserName = _userName, Password = _password, PasswordAlreadyEncrypted = passwordAlreadyEncrypted };
                    var authenticationResult = HomeController.Authentication(dto, nLogUtility, authenticationRepo, HttpContext, Response, Request);

                    if (!authenticationResult.IsValidUser)
                    {
                        if (passwordAlreadyEncrypted) passwordAlreadyEncrypted = false;
                        else passwordAlreadyEncrypted = true;
                        dto = new LoginDto() { UserName = _userName, Password = _password, PasswordAlreadyEncrypted = passwordAlreadyEncrypted };
                        authenticationResult = HomeController.Authentication(dto, nLogUtility, authenticationRepo, HttpContext, Response, Request);
                    }

                    if (authenticationResult.IsValidUser)
                    {
                        userId = (long)authenticationResult.Data?.UserId;
                        Program.TokensArchive[userId] = authenticationResult.Data;
                        identification = authenticationResult.Data;
                    }
                    else
                        throw new Exception("Authentication invalid! Login again!");
                }
                else
                    throw new Exception("Password and/or username passed or saved in cookies are empty! Login again!");
            }

            if (identification == null || identification.IsAuthenticated == false)
            {
                throw new Exception("Authentication expired! Login again!");
            }
        }

        // GET: WebSpeeches/Recognition
        public async Task<IActionResult> Recognition(string _phrase, string _hostSelected, bool? _reset, string _userName, string _password, bool? _application, long? _executionQueueId, bool? _alwaysShow, long? _id, bool? _onlyRefresh, string _subType, int? _step, bool? _login, string _param, long? _userId, string _message)
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
                    long userId = 0;
                    long.TryParse(_userId?.ToString(), out userId);
                    bool.TryParse(_reset?.ToString(), out reset);
                    bool.TryParse(_login?.ToString(), out login);
                    bool.TryParse(_onlyRefresh?.ToString(), out onlyRefresh);
                    int step = 0;
                    int.TryParse(_step?.ToString(), out step);
                    var passwordAlreadyEncrypted = false;

                    if (_userName == "null") _userName = null;
                    if (_password == "null") _password = null;
                    if (_phrase == "null") _phrase = null;
                    if (_message == "null") _message = null;
                    if (_hostSelected == "null") _hostSelected = null;
                    if (_subType == "null") _subType = null;
                    if (_param == "null") _param = null;
                    else _param = HttpUtility.UrlDecode(_param);

                    var expiresInSeconds = 0;
                    var identification = new TokenDto() { IsAuthenticated = false };

                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName), out expiresInSeconds);

                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteApplicationCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteErrorsCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);
                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteNewWebSpeechDtoInJsonCookieName);

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

                    //application = true; //to debug

                    if (_alwaysShow != null)
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName, _alwaysShow.ToString(), expiresInSeconds);
                        bool.TryParse(_alwaysShow?.ToString(), out application);
                    }

                    if (_password != null && _password != "" && login == true)
                    {
                        suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteAuthenticatedPasswordCookieName);

                        var passwordMd5 = "";

                        using (MD5 md5Hash = MD5.Create())
                        {
                            passwordMd5 = utility.GetMd5Hash(md5Hash, _password);
                        }

                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteAuthenticatedPasswordCookieName, passwordMd5, expiresInSeconds);
                    }

                    if ((_onlyRefresh == null && _onlyRefresh == true) || login == true) resetAfterLoad = true;

                    ManageAuthentication(ref identification, ref login, ref onlyRefresh, ref passwordAlreadyEncrypted, ref _userName, ref _password, ref resetAfterLoad, ref application, ref reset, userId:userId);

                    //var loadDateString = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);

                    //if (loadDateString != null && loadDateString != "" && resetAfterLoad == false && onlyRefresh == false)
                    //{
                    //    DateTime loadDate;
                    //    DateTime.TryParse(loadDateString, out loadDate);

                    //    DateTime now = DateTime.Now;
                    //    TimeSpan difference = now.Subtract(loadDate);
                    //    if (difference.TotalSeconds >= 2)
                    //    {
                    //        resetAfterLoad = true;
                    //    }
                    //}

                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteLoadDateCookieName);

                    if (_onlyRefresh != null) suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteLoadDateCookieName, DateTime.Now.ToString(), expiresInSeconds);

                    if (resetAfterLoad == false)
                    {
                        WebSpeechResult _webSpeechResult = null;

                        if (Program._webSpeechResultList.ContainsKey(identification.UserId))
                            _webSpeechResult = SuppUtility.Clone(Program._webSpeechResultList[identification.UserId]);
                        else
                            _webSpeechResult = await _GetAllWebSpeeches(className, method, identification);

                        if (_webSpeechResult.Successful == false)
                        {
                            data = _webSpeechResult.Data.FirstOrDefault();
                        }
                        else
                        {
                            data = recognitionCommon.GetWebSpeechDto(_phrase, hostSelected, reset, application, executionQueueId, alwaysShow, id, identification, onlyRefresh, _subType, step, expiresInSeconds, Response, Request, _param, _webSpeechResult, true, null).GetAwaiter().GetResult();
                        }
                    }

                    if (resetAfterLoad == true || data == null)
                    {
                        data = new WebSpeechDto() { };
                        data.OnlyRefresh = false;
                        data.Id = id;
                        data.HostSelected = hostSelected;
                        data.Application = application;
                        data.AlwaysShow = alwaysShow;
                        data.ExecutionQueueId = executionQueueId;
                        data.LogJSActive = GeneralSettings.Static.LogJSActive;
                        data.UserId = identification.UserId;
                    }

                    //string url = HttpContext.Request.GetDisplayUrl();

                    //var cert = await GetServerCertificateAsync(url);

                    //data.SslCertificateExpirationDate = cert.GetExpirationDateString();

                    if (_message != null && _message != "")
                        data.Message = _message.Replace("NAME", identification.Name);

                    data.ResetAfterLoad = resetAfterLoad;

                    if (data != null && application && data?.Answer != null && data?.Answer != String.Empty)
                    {
                        await MediaPlayOrPause(hostSelected);
                    }

                    return View(data);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    ModelState.AddModelError("ModelStateErrors", ex.Message);
                    suppUtility.AddErrorToCookie(Request, Response, ex.Message);

                    if(ex.Message.Contains("login", StringComparison.InvariantCultureIgnoreCase))
                        return RedirectToAction("Login", "Home");
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
        }

        static async Task<X509Certificate2> GetServerCertificateAsync(string url)
        {
            X509Certificate2 certificate = null;
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, cert, __, ___) =>
                {
                    certificate = new X509Certificate2(cert.GetRawCertData());
                    return true;
                }
            };

            var httpClient = new HttpClient(httpClientHandler);
            await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            return certificate ?? throw new NullReferenceException();
        }

        private async Task<WebSpeechResult> _GetAllWebSpeeches(string className, string method, TokenDto identification)
        {
            var result = new WebSpeechResult() { };

            var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
            var _method = currentMethod.Name;
            var _className = currentMethod.DeclaringType.Name;

            var access_token_cookie = suppUtility.GetAccessToken(Request);

            var webSpeechResult = await webSpeecheRepo.GetAllWebSpeeches(access_token_cookie);

            if (webSpeechResult.Successful == false)
            {
                var error = $"Error - Class: [{className}, Method: [{method}->{_method}], Operation: [{nameof(webSpeecheRepo.GetAllWebSpeeches)}] - Message: [{webSpeechResult.Message}]";
                result.Successful = false;
                result.Message = error;
                result.Data = new List<WebSpeechDto>() { new WebSpeechDto() { Answer = "", Ehi = 0, Error = error } };
                result.ResultState = Models.ResultType.Error;
            }
            else
            {
                result = webSpeechResult;

                Program._webSpeechResultList[identification.UserId] = SuppUtility.Clone(webSpeechResult);
            }

            return result;
        }

        // GET: WebSpeeches/GetWebSpeechDtoInJson
        public async Task<string> GetWebSpeechDtoInJson(string _phrase, string _hostSelected, bool? _reset, bool? _application, long? _executionQueueId, bool? _alwaysShow, long? _id, bool? _onlyRefresh, string _subType, int? _step, bool? _recognitionDisable, string _param, string _keysMatched, long? _userId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = "";

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;
                    var reset = false;
                    var application = false;
                    var alwaysShow = false;
                    var onlyRefresh = false;
                    var recognitionDisable = false;
                    var hostSelected = "";
                    long executionQueueId = 0;
                    long.TryParse(_executionQueueId?.ToString(), out executionQueueId);
                    long id = 0;
                    long.TryParse(_id?.ToString(), out id);

                    long userId = 0;
                    long.TryParse(_userId?.ToString(), out userId);

                    bool.TryParse(_reset?.ToString(), out reset);
                    bool.TryParse(_onlyRefresh?.ToString(), out onlyRefresh);
                    bool.TryParse(_recognitionDisable?.ToString(), out recognitionDisable);
                    
                    int step = 0;
                    int.TryParse(_step?.ToString(), out step);

                    if (_phrase == "null") _phrase = null;
                    if (_hostSelected == "null") _hostSelected = null;
                    if (_subType == "null") _subType = null;
                    if (_param == "null") _param = null;
                    else _param = HttpUtility.UrlDecode(_param);

                    var expiresInSeconds = 0;
                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName), out expiresInSeconds);

                    var identification = new TokenDto() { IsAuthenticated = false };

                    if (_hostSelected == null) hostSelected = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);
                    else hostSelected = _hostSelected;

                    if (_application == null) bool.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteApplicationCookieName), out application);
                    else bool.TryParse(_application?.ToString(), out application);

                    if (_alwaysShow == null) bool.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAlwaysShowCookieName), out alwaysShow);
                    else bool.TryParse(_alwaysShow?.ToString(), out alwaysShow);

                    var login = false;
                    var passwordAlreadyEncrypted = true;
                    var _userName = "";
                    var _password = "";
                    var resetAfterLoad = false;
                    ManageAuthentication(ref identification, ref login, ref onlyRefresh, ref passwordAlreadyEncrypted, ref _userName, ref _password, ref resetAfterLoad, ref application, ref reset, userId: userId);

                    WebSpeechDto data = null;

                    var webSpeechResult = new WebSpeechResult() { };

                    if (Program._webSpeechResultList.ContainsKey(identification.UserId))
                        webSpeechResult = SuppUtility.Clone(Program._webSpeechResultList[identification.UserId]);
                    else
                    {
                        var _webSpeechResult = await _GetAllWebSpeeches(className, method, identification);

                        if (_webSpeechResult.Successful == false)
                            data = _webSpeechResult.Data.FirstOrDefault();
                        else
                            if (Program._webSpeechResultList.ContainsKey(identification.UserId))
                                webSpeechResult = SuppUtility.Clone(Program._webSpeechResultList[identification.UserId]);
                    }

                    if(webSpeechResult.Successful)
                        data = recognitionCommon.GetWebSpeechDto(_phrase, hostSelected, reset, application, executionQueueId, alwaysShow, id, identification, onlyRefresh, _subType, step, expiresInSeconds, Response, Request, _param, webSpeechResult, false, _keysMatched).GetAwaiter().GetResult();

                    if (data != null)
                    {
                        data.RecognitionDisable = recognitionDisable;
                        result = System.Text.Json.JsonSerializer.Serialize(data);
                        //result = result.Replace(@"\", "/");

                        if (application && data?.Answer != null && data?.Answer != String.Empty) 
                        {
                            await MediaPlayOrPause(hostSelected);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var data = new WebSpeechDto() { };
                    data.LogJSActive = data.LogJSActive = GeneralSettings.Static.LogJSActive;
                    data.Error = nameof(WebSpeechesController.Recognition) + " - " + ex.Message.ToString().Replace("'","");
                    logger.Error(ex.ToString());
                    result = JsonConvert.SerializeObject(data);
                }

                return result;
            }
        }

        // GET: WebSpeeches/ExecutionFinished
        public async Task ExecutionFinished(long _id, string _hostSelected, bool _application)
        {
            await recognitionCommon.ExecutionFinished(_id, _hostSelected, _application, Request);
        }

        // GET: WebSpeeches/MediaPlayOrPause
        public async Task MediaPlayOrPause(string _hostSelected)
        {
            await recognitionCommon.AddExecutionQueueQuickly(_hostSelected, Request, ExecutionQueueType.MediaPlayOrPause);
        }

        // GET: WebSpeeches/MediaNextTrack
        public async Task MediaNextTrack(string _hostSelected)
        {
            await recognitionCommon.AddExecutionQueueQuickly(_hostSelected, Request, ExecutionQueueType.MediaNextTrack);
        }

        // GET: WebSpeeches/MediaPreviousTrack
        public async Task MediaPreviousTrack(string _hostSelected)
        {
            await recognitionCommon.AddExecutionQueueQuickly(_hostSelected, Request, ExecutionQueueType.MediaPreviousTrack);
        }

        // GET: WebSpeeches/CleanMessage
        public async Task<bool> CleanMessage(long userId)
        {
            var result = true;
            var path = System.IO.Directory.GetCurrentDirectory();

            var filePath = Path.Combine(path, "wwwroot\\Files\\Message_" + userId.ToString() + ".txt");

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception)
                {

                    result = false;
                }
                    
            }

            return result;
        }

        // GET: WebSpeeches/AddMessage
        public async Task<bool> AddMessage(string message, long userId)
        {
            var result = true;

            var filePath = "";
            try
            {
                var path = System.IO.Directory.GetCurrentDirectory();

                filePath = Path.Combine(path, "wwwroot\\Files\\Message_" + userId.ToString() + ".txt");

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(message);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}