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
using AutoMapper;
using Newtonsoft.Json;
using GoogleManagerModels;
using Google.Apis.Calendar.v3.Data;

namespace Supp.Site.Controllers
{
    public class MediaController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly MediaRepository mediaRepo;
        private readonly SuppUtility suppUtility;
        private readonly GoogleAccountsRepository googleAccountRepository;
        

        public MediaController()
        {
            mediaRepo = new MediaRepository();
            googleAccountRepository = new GoogleAccountsRepository();
            suppUtility = new SuppUtility();
        }

        // GET: Media
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                MediaResult result = new MediaResult() { Data = new List<MediaDto>() { }, Successful = false };
                IEnumerable<MediaDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "UserName" : "";

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

                    result = await mediaRepo.GetAllMedia(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetAllMedia)}] - Message: [{result.Message}]");

                    data = from s in result.Data
                           select s;

                    var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(access_token_cookie);

                    if (!googleAccountResult.Successful)
                        throw new Exception(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    foreach (var row in data)
                    {
                        row.GoogleAccounts = googleAccountResult.Data;
                    }

                    long val = 0;
                    Int64.TryParse(searchString, out val);

                    if (val != 0)
                    {
                        data = data.Where(_ => _.Id == val);
                    }
                    else if (!String.IsNullOrEmpty(searchString))
                    {
                        var _googleAccountIds = googleAccountResult.Data.Where(_ => _.Account.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())).Select(_ => _.Id).ToList();
                        data = data.Where(_ => _.UserName.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Name.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.FileId.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _googleAccountIds.Contains(_.GoogleAccountId)
                        );
                    }

                    switch (sortOrder)
                    {
                        case "Id":
                            data = data.OrderBy(_ => _.Id);
                            break;
                        case "GoogleAccount":
                            data = data.OrderBy(_ => _.GoogleAccounts.Where(x => x.Id== _.GoogleAccountId).Select(_ => _.Account).FirstOrDefault());
                            break;
                        case "FileId":
                            data = data.OrderBy(_ => _.FileId);
                            break;
                        case "Name":
                            data = data.OrderBy(_ => _.Name);
                            break;
                        case "Path":
                            data = data.OrderBy(_ => _.Path);
                            break;
                        case "ModifiedTime":
                            data = data.OrderBy(_ => _.ModifiedTime);
                            break;
                        case "CreatedTime":
                            data = data.OrderBy(_ => _.CreatedTime);
                            break;
                        case "Size":
                            data = data.OrderBy(_ => _.Size);
                            break;
                        case "FileExtension":
                            data = data.OrderBy(_ => _.FileExtension);
                            break;
                        case "MimeType":
                            data = data.OrderBy(_ => _.MimeType);
                            break;
                        case "VideoDurationMillis":
                            data = data.OrderBy(_ => _.VideoDurationMillis);
                            break;
                        case "VideoHeight":
                            data = data.OrderBy(_ => _.VideoHeight);
                            break;
                        case "VideoWidth":
                            data = data.OrderBy(_ => _.VideoWidth);
                            break;
                        case "ImageTime":
                            data = data.OrderBy(_ => _.ImageTime);
                            break;
                        case "ImageWidth":
                            data = data.OrderBy(_ => _.ImageWidth);
                            break;
                        case "ImageHeight":
                            data = data.OrderBy(_ => _.ImageHeight);
                            break;
                        case "ImageLocationAltitude":
                            data = data.OrderBy(_ => _.ImageLocationAltitude);
                            break;
                        case "ImageLocationLatitude":
                            data = data.OrderBy(_ => _.ImageLocationLatitude);
                            break;
                        case "ImageLocationLongitude":
                            data = data.OrderBy(_ => _.ImageLocationLongitude);
                            break;
                        case "UserName":
                            data = data.OrderBy(_ => _.UserName);
                            break;
                        case "Type":
                            data = data.OrderBy(_ => _.Type);
                            break;
                        case "File":
                            data = data.OrderBy(_ => _.File);
                            break;
                        case "Thumbnail":
                            data = data.OrderBy(_ => _.Thumbnail);
                            break;
                        case "ThumbnailWidth":
                            data = data.OrderBy(_ => _.ThumbnailWidth);
                            break;
                        case "ThumbnailHeight":
                            data = data.OrderBy(_ => _.ThumbnailHeight);
                            break;
                        default:
                            data = data.OrderBy(_ => _.UserName);
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
                    ModelState.AddModelError("ModelStateErrors", "Media not found!");
                    return View();
                }
            }
        }

        // GET: Media/Details/5
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
                    var result = await mediaRepo.GetMediaById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(access_token_cookie);

                    if (!googleAccountResult.Successful)
                        throw new Exception(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    foreach (var row in data)
                    {
                        row.GoogleAccounts = googleAccountResult.Data;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetMediaById)}] - Message: [{result.Message}]");

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

        // GET: Media/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var data = new List<MediaDto>() { };
                    var access_token_cookie = suppUtility.GetAccessToken(Request);

                    data.Add(new MediaDto() { });

                    var googleAccountResult = googleAccountRepository.GetAllGoogleAccounts(access_token_cookie).GetAwaiter().GetResult();

                    if (!googleAccountResult.Successful)
                        throw new Exception(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    foreach (var row in data)
                    {
                        row.GoogleAccounts = googleAccountResult.Data;
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

        // POST: Media/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GoogleAccountId,FileId,Name,Path,ModifiedTime,CreatedTime,Size,FileExtension,MimeType,VideoDurationMillis,VideoHeight,VideoWidth,ImageTime,ImageWidth,ImageHeight,ImageLocationAltitude,ImageLocationLatitude,ImageLocationLongitude,UserName,Type,File,Thumbnail,ThumbnailWidth,ThumbnailHeight,InsDateTime")] MediaDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<MediaDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.GetAccessToken(Request);
                        var result = await mediaRepo.AddMedia(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful) 
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.AddMedia)}] - Message: [{result.Message}]");

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

        // GET: Media/Edit/5
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
                    var result = await mediaRepo.GetMediaById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(access_token_cookie);

                    if (!googleAccountResult.Successful)
                        throw new Exception(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    foreach (var row in data)
                    {
                        row.GoogleAccounts = googleAccountResult.Data;
                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetMediaById)}] - Message: [{result.Message}]");

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

        // POST: Media/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,GoogleAccountId,FileId,Name,Path,ModifiedTime,CreatedTime,Size,FileExtension,MimeType,VideoDurationMillis,VideoHeight,VideoWidth,ImageTime,ImageWidth,ImageHeight,ImageLocationAltitude,ImageLocationLatitude,ImageLocationLongitude,UserName,Type,File,Thumbnail,ThumbnailWidth,ThumbnailHeight,InsDateTime")] MediaDto dto)
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

                        if (!MediaExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.GetAccessToken(Request);
                        var result = await mediaRepo.UpdateMedia(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.UpdateMedia)}] - Message: [{result.Message}]");
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

        // GET: Media/Delete/5
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
                    var result = await mediaRepo.GetMediaById((long)id, access_token_cookie);
                    var data = result.Data.ToList();

                    var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(access_token_cookie);

                    if (!googleAccountResult.Successful)
                        throw new Exception(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    foreach (var row in data)
                    {
                        row.GoogleAccounts = googleAccountResult.Data;
                    }
                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetMediaById)}] - Message: [{result.Message}]");

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

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<MediaDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.GetAccessToken(Request);
                        var result = await mediaRepo.DeleteMediaById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.DeleteMediaById)}] - Message: [{result.Message}]");

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

        // GET: Media/ClearStructureMedia
        public async Task<IActionResult> ClearStructureMedia()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = suppUtility.GetAccessToken(Request);
                    var result = await mediaRepo.GetAllMedia(access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetMediaById)}] - Message: [{result.Message}]");

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

        // POST: Media/ClearStructureMedia
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearStructureMediaConfirmed([Bind("Path")] MediaDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<MediaDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.GetAccessToken(Request);
                        var result = await mediaRepo.ClearStructureMedia(access_token_cookie, dto.Path);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Clear Structure Media failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.ClearStructureMedia)}] - Message: [{result.Message}]");

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

        private bool MediaExists(long id)
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
                    var result = mediaRepo.GetMediaById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetMediaById)}] - Message: [{result.Message}]");

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

        public async Task<IActionResult> StructureMedia(bool mediaRequested, bool clear)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = new RequestResult() { Data = new List<Request>(), ResultState = GoogleManagerModels.ResultType.None, Message = "No message!" };
                var resultMedia = new MediaResult() { Data = new List<MediaDto>(), ResultState = Supp.Models.ResultType.Created };

                var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                var method = currentMethod.Name;
                var className = currentMethod.DeclaringType.Name;

                var access_token_cookie = suppUtility.GetAccessToken(Request);

                if (mediaRequested && clear == true)
                {
                    resultMedia = await mediaRepo.ClearStructureMedia(access_token_cookie, "");

                    if (resultMedia.Message == null || resultMedia.Message == String.Empty) resultMedia.Message = "No message!";

                    result.Message = resultMedia.Message;
                }

                if (mediaRequested && clear == false)
                {
                    try
                    {
                        var userName = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserNameCookieName);
                        var userId = long.Parse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAuthenticatedUserIdCookieName).ToString());

                        result = await mediaRepo.StructureMedia(access_token_cookie, userName, userId);

                        if (result.Message == null || result.Message == String.Empty) result.Message = "No message!";

                        if (result.Successful && result.Data.Count > 0)
                        {
                            try
                            {
                                foreach (var request in result.Data)
                                {
                                    var recordsToProcessed = 1;
                                    var fromRecord = 0;
                                    for (int i = 0; i < request.Files.Count; i += recordsToProcessed)
                                    {
                                        var files = request.Files.Skip(fromRecord).Take(recordsToProcessed);

                                        var json = JsonConvert.SerializeObject(files);
                                        resultMedia = await mediaRepo.AddRangeMedia(json, access_token_cookie);
                                        fromRecord += recordsToProcessed;
                                        if (result.Successful == false)
                                            throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.AddRangeMedia)}] - Message: [{resultMedia.Message}]");

                                    }
                                }
                                result.ResultState = GoogleManagerModels.ResultType.Executed;
                            }
                            catch (Exception ex)
                            {
                                result.Message = resultMedia.Message;
                                result.Successful = false;
                                result.ResultState = GoogleManagerModels.ResultType.Error;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        ModelState.AddModelError("ModelStateErrors", ex.Message);
                        suppUtility.AddErrorToCookie(Request, Response, ex.Message);

                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(result);
            }
        }

        // Show Media
        public async Task<IActionResult> MediaBox()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = suppUtility.GetAccessToken(Request);
                    var result = await mediaRepo.GetAllMedia(access_token_cookie);
                    var data = result.Data.ToList();

                    //var googleAccountResult = await googleAccountRepository.GetAllGoogleAccounts(access_token_cookie);

                    //if (!googleAccountResult.Successful)
                    //    throw new Exception(nameof(googleAccountRepository.GetAllGoogleAccounts) + " " + googleAccountResult.Message + "!");

                    //foreach (var row in data)
                    //{
                    //    row.GoogleAccounts = googleAccountResult.Data;
                    //}

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(mediaRepo.GetAllMedia)}] - Message: [{result.Message}]");

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
    }
}
