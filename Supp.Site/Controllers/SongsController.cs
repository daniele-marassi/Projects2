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
using Newtonsoft.Json;
using System.IO;

namespace Supp.Site.Controllers
{
    public class SongsController : Controller
    {
        private readonly static Logger classLogger  = LogManager.GetCurrentClassLogger();
        private readonly  NLogUtility nLogUtility = new NLogUtility();
        private readonly SongsRepository songRepo;
        private readonly SuppUtility suppUtility;
        private readonly ExecutionQueuesRepository executionQueueRepo;
        private readonly WebSpeechesRepository webSpeecheRepo;

        public SongsController()
        {
            songRepo = new SongsRepository();
            suppUtility = new SuppUtility();
            executionQueueRepo = new ExecutionQueuesRepository();
            webSpeecheRepo = new WebSpeechesRepository();
        }

        // GET: Songs
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                SongResult result = new SongResult() { Data = new List<SongDto>() { }, Successful = false };
                IEnumerable<SongDto> data;

                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "FullPath" : "";

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

                    result = await songRepo.GetAllSongs(access_token_cookie);

                    if (result.Successful == false)
                        throw new Exception($"Error - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.GetAllSongs)}] - Message: [{result.Message}]");

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
                        data = data.Where(_ => _.FullPath.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                            || _.Position.ToStringExtended().ToUpper().Contains(searchString.ToUpper().Trim())
                        );
                    }

                    switch (sortOrder)
                    {
                        case "FullPath":
                            data = data.OrderBy(_ => _.FullPath);
                            break;
                        case "Position":
                            data = data.OrderBy(_ => _.Position);
                            break;
                        case "Order":
                            data = data.OrderBy(_ => _.Order);
                            break;
                        case "Listened":
                            data = data.OrderBy(_ => _.Listened);
                            break;
                        default:
                            data = data.OrderBy(_ => _.FullPath);
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
                    ModelState.AddModelError("ModelStateErrors", "Songs not found!");
                    return View();
                }
            }
        }

        // GET: Songs/Details/5
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
                    var result = await songRepo.GetSongsById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.GetSongsById)}] - Message: [{result.Message}]");

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

        // GET: Songs/Create
        public IActionResult Create()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var claims = new ClaimsDto() { IsAuthenticated = false };
                var suppUtility = new SuppUtility();

                try
                {
                    var claimsString = suppUtility.ReadCookie(Request, Config.GeneralSettings.Constants.SuppSiteClaimsCookieName);
                    claims = JsonConvert.DeserializeObject<ClaimsDto>(claimsString);
                }
                catch (Exception)
                {

                }

                var dto = new SongDto() { };
                return View(dto);
            }
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SongName,Name,Surname,InsDateTime,CustomizeParams")] SongDto dto)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<SongDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await songRepo.AddSong(dto, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful) 
                            throw new Exception($"Error [Add failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.AddSong)}] - Message: [{result.Message}]");

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

        // GET: Songs/Edit/5
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
                    var result = await songRepo.GetSongsById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    var claims = new ClaimsDto() { IsAuthenticated = false };

                    try
                    {
                        var claimsString = suppUtility.ReadCookie(Request, Config.GeneralSettings.Constants.SuppSiteClaimsCookieName);
                        claims = JsonConvert.DeserializeObject<ClaimsDto>(claimsString);
                    }
                    catch (Exception)
                    {

                    }

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.GetSongsById)}] - Message: [{result.Message}]");

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

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,SongName,Name,Surname,InsDateTime,CustomizeParams")] SongDto dto)
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

                        if (!SongExists(dto.Id))
                            throw new Exception($"Error [Id not exists!] - Class: [{className}, Method: [{method}], Operation: [] - Message: []");

                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await songRepo.UpdateSong(dto, access_token_cookie);

                        if (!result.Successful)
                            throw new Exception($"Error [Update failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.UpdateSong)}] - Message: [{result.Message}]");
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

        // GET: Songs/Delete/5
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
                    var result = await songRepo.GetSongsById((long)id, access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.GetSongsById)}] - Message: [{result.Message}]");

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

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<SongDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await songRepo.DeleteSongById(id, access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Delete failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.DeleteSongById)}] - Message: [{result.Message}]");

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

        // GET: Songs/ClearSongs
        public async Task<IActionResult> ClearSongs()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                    var method = currentMethod.Name;
                    var className = currentMethod.DeclaringType.Name;

                    var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                    var result = await songRepo.GetAllSongs(access_token_cookie);
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.GetSongsById)}] - Message: [{result.Message}]");

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

        // POST: Songs/ClearSongs
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearSongsConfirmed()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                if (ModelState.IsValid)
                {
                    var data = new List<SongDto>() { };
                    try
                    {
                        var currentMethod = nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod());
                        var method = currentMethod.Name;
                        var className = currentMethod.DeclaringType.Name;
                        var access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);
                        var result = await songRepo.ClearSongs(access_token_cookie);

                        data.AddRange(result.Data);

                        if (!result.Successful)
                            throw new Exception($"Error [Clear Songs failed!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.ClearSongs)}] - Message: [{result.Message}]");

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

        private bool SongExists(long id)
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
                    var result = songRepo.GetSongsById(id, access_token_cookie).Result;
                    var data = result.Data.FirstOrDefault();

                    if (result.Successful == false || data == null)
                        throw new Exception($"Error [Data not found!] - Class: [{className}, Method: [{method}], Operation: [{nameof(songRepo.GetSongsById)}] - Message: [{result.Message}]");

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

        // GET: Songs/SongsPlayer
        public async Task<IActionResult> SongsPlayer(string _playListSelected, int? _volume, string _command, bool? _shuffle, bool? _repeat)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                SongDto data = null;

                try
                {
                    var volume = 0;
                    bool shuffle = false;
                    bool repeat = false;
                    int.TryParse(_volume?.ToString(), out volume);
                    bool.TryParse(_shuffle?.ToString(), out shuffle);
                    bool.TryParse(_repeat?.ToString(), out repeat);

                    data = new SongDto() { FullPath = "" };

                    var expiresInSeconds = 0;
                    var claims = new ClaimsDto() { IsAuthenticated = false };

                    claims = SuppUtility.GetClaims(User);

                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName), out expiresInSeconds);

                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);

                    suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteHostSelectedCookieName, GeneralSettings.Static.HostDefault, expiresInSeconds);
                    var hostSelected = GeneralSettings.Static.HostDefault;

                    var playListSelected = suppUtility.ReadCookie(Request, Config.GeneralSettings.Constants.SuppSitePlayListSelectedCookieName);

                    if (_playListSelected != null && _playListSelected != "")
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSitePlayListSelectedCookieName, _playListSelected, expiresInSeconds);
                        playListSelected = _playListSelected;
                    }

                    data.HostSelected = hostSelected;
                    data.HostsArray = claims.Configuration.Speech.HostsArray;
                    data.HostSelected = claims.Configuration.Speech.HostDefault;
                    data.PlayListSelected = playListSelected;
                    data.Command = _command;
                    data.Shuffle = shuffle;
                    data.Repeat = repeat;
                    data.Volume = volume;

                    string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    var getAllSongsResult = songRepo.GetAllSongs(access_token_cookie).GetAwaiter().GetResult();

                    var playlist = new List<string>() { };

                    //foreach (var item in getAllSongsResult.Data)
                    //{
                    //    var splitedPath = Path.GetDirectoryName(item.FullPath).Substring(3).Split(@"\");

                    //    var path = "";

                    //    foreach (var _item in splitedPath)
                    //    {
                    //        if (path != "") path += "/";
                    //        path += _item;
                    //        playlist.Add(path);
                    //    }    
                    //}

                    foreach (var item in getAllSongsResult.Data)
                    {
                        playlist.Add(item.Position.Replace(@"\",@"/"));
                    }

                    playlist = playlist.Distinct().OrderBy(_=>_).ToList();

                    data.PlayListArray = System.Text.Json.JsonSerializer.Serialize(playlist.ToArray());

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

        // GET: Songs/ManageSongsPlayer
        public async Task<string> ManageSongsPlayer(string _command, string _hostSelected, bool? _shuffle, bool? _repeat, string _playListSelected)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var result = "";

                try
                {
                    var hostSelected = "";
                    var rnd = new Random();
                    var expiresInSeconds = 0;
                    var claims = new ClaimsDto() { IsAuthenticated = false };
                    var shuffle = false;
                    var repeat = false;
                    var reset = false;
                    var songsPosition = 0;
                    long songId = 0;
                    var playListSelected = "";

                    bool.TryParse(_shuffle?.ToString(), out shuffle);

                    bool.TryParse(_repeat?.ToString(), out repeat);

                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteSongsPositionCookieName), out songsPosition);

                    long.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteSongIdCookieName), out songId);

                    int.TryParse(suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteExpiresInSecondsCookieName), out expiresInSeconds);

                    suppUtility.RemoveCookie(Response, Request, GeneralSettings.Constants.SuppSiteHostSelectedCookieName);

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

                    if (_playListSelected != null && _playListSelected != "")
                    {
                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSitePlayListSelectedCookieName, _playListSelected, expiresInSeconds);
                        playListSelected = _playListSelected;
                    }

                    try
                    {
                        var claimsString = suppUtility.ReadCookie(Request, Config.GeneralSettings.Constants.SuppSiteClaimsCookieName);
                        claims = JsonConvert.DeserializeObject<ClaimsDto>(claimsString);
                    }
                    catch (Exception)
                    {
                        claims = SuppUtility.GetClaims(User);
                    }

                    string access_token_cookie = suppUtility.ReadCookie(Request, GeneralSettings.Constants.SuppSiteAccessTokenCookieName);

                    var getAllSongsResult = songRepo.GetAllSongs(access_token_cookie).GetAwaiter().GetResult();

                    var songs = new List<SongDto>() { };

                    SongDto data =null;

                    var _songs = getAllSongsResult.Data.OrderBy(_ => _.Order).ToList();
                    if (playListSelected != null && playListSelected != "") _songs = _songs.Where(_ => _.FullPath.Contains(playListSelected.Replace("/", @"\"), StringComparison.InvariantCultureIgnoreCase)).ToList();

                    if (_command.Contains("reset"))
                    {
                        reset = true;
                        foreach (var item in _songs)
                        {
                            item.Listened = false;
                            await songRepo.UpdateSong(item, access_token_cookie);
                        }

                        _songs = getAllSongsResult.Data.OrderBy(_ => _.Order).ToList();
                        if (playListSelected != null && playListSelected != "") _songs = _songs.Where(_ => _.FullPath.Contains(playListSelected.Replace("/", @"\"), StringComparison.InvariantCultureIgnoreCase)).ToList();
                    }

                    if (_command.Contains("volume"))
                    {
                        var commands = _command.Split(",");
                        var volume = "";

                        _command = "";

                        foreach (var item in commands)
                        {
                            if (item.Contains("volume"))
                            {
                                volume = item.Replace("volume:", "");
                            }
                            else
                            {
                                if (_command != "") _command += ",";
                                _command += item;
                            }
                        }

                        var getAllWebSpeechesResult = await webSpeecheRepo.GetAllWebSpeeches(access_token_cookie);
                        var webSpeech = getAllWebSpeechesResult.Data.Where(_ => _.Name == "volume_with_percentage").FirstOrDefault();

                        if (webSpeech != null)
                        {
                            var executionQueue = new ExecutionQueueDto() { FullPath = webSpeech.Operation, Arguments = volume, Host = _hostSelected, Type = webSpeech.Type };
                            var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);
                        }
                    }

                    if (getAllSongsResult != null) songs = _songs.Where(_ => _.Listened == false).OrderBy(_ => _.Order).ToList();
                    if(songs.Count() == 0 && getAllSongsResult != null && repeat) songs = _songs.OrderBy(_ => _.Order).ToList();

                    if (songs.Count > 0) 
                    {
                        int x = rnd.Next(0, songs.Count());

                        if (shuffle == false && _command.Contains("forward")) x = songsPosition + 1;
                        if (shuffle == false && _command.Contains("previous")) x = songsPosition - 1;
                        if (_command.Contains("stop") || _command.Contains("play")) x = songsPosition;

                        if (x >= songs.Count()) x = 0;

                        suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteSongsPositionCookieName, x.ToString(), expiresInSeconds);

                        if ((_command.Contains("stop") || _command.Contains("play")) && songId != 0) data = _songs.Where(_ => _.Id == songId).FirstOrDefault(); 
                        else data = songs[x];

                        if(data == null) data = songs[x];
                        if (data == null) 
                        {
                            x = 0;
                            data = songs[0];
                        }

                        try
                        {
                            suppUtility.SetCookie(Response, GeneralSettings.Constants.SuppSiteSongIdCookieName, data.Id.ToString(), expiresInSeconds);

                            data.Shuffle = shuffle;
                            data.Repeat = repeat;
                            data.Command = _command;
                            data.PlayListSelected = playListSelected;
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (data != null)
                    {
                        (string Command, long Id, string FullPath) arguments;
                        arguments.Command = _command;
                        arguments.Id = data.Id;
                        arguments.FullPath = data.FullPath;

                        var executionQueue = new ExecutionQueueDto() { Arguments = JsonConvert.SerializeObject(arguments), Host = _hostSelected, Type = "SongsPlayer" };
                        var addExecutionQueueResult = await executionQueueRepo.AddExecutionQueue(executionQueue, access_token_cookie);

                        if (addExecutionQueueResult.Successful)
                        {
                            data.Successful = true;
                            data.Title = Path.GetFileNameWithoutExtension(data.FullPath)?.Replace("_", " ");
                        }

                        result = System.Text.Json.JsonSerializer.Serialize(data);
                        result = result.Replace(@"\", "/");
                    }

                    if (data == null)
                    {
                        data = new SongDto() { };
                        data.Id = 0;
                        data.FullPath = "";
                        data.Successful = false;
                        data.Title = "No aviable data found!";
                        data.Shuffle = shuffle;
                        data.Repeat = repeat;
                        if (_command == "play") data.Command = "stop";
                        else data.Command = _command;

                        (string Command, long Id, string FullPath) arguments;
                        arguments.Command = data.Command;
                        arguments.Id = data.Id;
                        arguments.FullPath = data.FullPath;

                        result = System.Text.Json.JsonSerializer.Serialize(data);
                        result = result.Replace(@"\", "/");
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
    }
}
