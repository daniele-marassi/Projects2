using Additional.NLog;
using Tools.Songs.Contexts;
using Tools.Songs.Contracts;
using Tools.Songs.Models;
using Tools.Songs.Repositories;
using NLog;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using Additional;
using System.Collections.Generic;
using System.IO;
using MediaToolkit;

namespace Tools.Songs
{
    public partial class SongsManager : ServiceBase
    {
        bool serviceActive = true;
        private ISongsRepository _repo;
        string oldServiceError = null;
        string oldAddError = null;
        string suppDatabaseConnection = "";
        string rootPath;
        int timeToClosePopUpInMilliseconds = 1000;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        Utility utilty;
        System.Collections.Specialized.NameValueCollection appSettings;
        string songsPath;
        int volumeOfNotify;
        bool notifyMute;
        bool notifyPopupShow;

        public SongsManager()
        {
            InitializeComponent();
            utilty = new Utility();
            appSettings = ConfigurationManager.AppSettings;
            songsPath = ConfigurationManager.AppSettings["SongsPath"];
            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);

            suppDatabaseConnection = ConfigurationManager.AppSettings["SuppDatabaseConnection"];
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);
            _repo = new SongsRepository(suppDatabaseConnection);
            Database.SetInitializer<SuppDatabaseContext>(null);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);

            notifyMute = bool.Parse(appSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);
        }

        protected override void OnStart(string[] args)
        {

        }

        public void Stop()
        {
            serviceActive = false;
        }

        public async Task Start()
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var songsFounded = await FindAndAddSongs(songsPath);

                    if (songsFounded)
                    {
                        logger.Info("Songs Founded!");
                    }
                    else
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                        if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SongsManagerMenuItem", volumeOfNotify, notifyMute);
                        if (notifyPopupShow) Common.Utility.ShowMessage("SongsManager Message:" + "FindSongs failed!", MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                        oldServiceError = "FindSongs failed!";
                        logger.Error(oldServiceError);
                    }
                }
                catch (Exception ex)
                {
                    if (oldServiceError == null || oldServiceError != ex.Message)
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);


                        if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SongsManagerMenuItem", volumeOfNotify, notifyMute);
                        if (notifyPopupShow) Common.Utility.ShowMessage("SongsManager Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                        oldServiceError = ex.Message;
                        logger.Error(oldServiceError);
                    }
                }
            }
        }

        public async Task<bool> FindAndAddSongs(string path)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var founded = true;
                List<string> files = new List<string>();
                var extensions = new List<string> { ".mp3", ".mp4", ".wav", ".flac", ".m3u" };
                try
                {
                    files.AddRange(Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories));

                    files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                        .Where(_ => extensions.IndexOf(Path.GetExtension(_.ToLower())) >= 0).OrderBy(_=>_).ToList();

                    var order = -1;
                    var oldPosition = "";

                    await _repo.ClearSongs();

                    foreach (var item in files)
                    {
                        var position = Path.GetDirectoryName(item);
                        position = position.ToLower().Replace(path.ToLower(), "");
                        position = position.Substring(1, position.Length-1);

                        order++;
                        //if (position != oldPosition && oldPosition != "") order = 0;

                        oldPosition = position;

                        var inputFile = new MediaToolkit.Model.MediaFile { Filename = item };
                        using (var engine = new Engine())
                        {
                            engine.GetMetadata(inputFile);
                        }

                        var totalMilliseconds = inputFile?.Metadata?.Duration != null ? (long)inputFile?.Metadata?.Duration.TotalMilliseconds : 0;

                        if (totalMilliseconds > 0)
                        {
                            var song = new SongDto()
                            {
                                FullPath = item,
                                Listened = false,
                                Position = position,
                                Order = order,
                                DurationInMilliseconds = totalMilliseconds
                            };

                            await AddSongsDbAsync(song);
                        }
                    }
                }
                catch (Exception ex)
                {
                    founded = false;
                    logger.Error(ex.ToString());
                }
                return founded;
            }
        }

        /// <summary>
        /// AddSongsDbAsync
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddSongsDbAsync(SongDto item)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var addSongResult = await _repo.AddSongs(item);

                if (!addSongResult.Successful)
                {
                    if (oldAddError == null || oldAddError != addSongResult.Message)
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                        if (serviceActive) Common.ContextMenus.SetMenuItemWithError("SongsManagerMenuItem", volumeOfNotify, notifyMute);
                        if (notifyPopupShow) Common.Utility.ShowMessage("SongsManager Message:" + addSongResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                        oldAddError = addSongResult.Message;
                        logger.Error(oldAddError);
                    }
                }
                else
                {
                    if (oldAddError != null)
                    {
                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        oldAddError = null;
                        if (serviceActive) Common.ContextMenus.SetMenuItemRecover("SongsManagerMenuItem", volumeOfNotify, notifyMute);
                        if (notifyPopupShow) Common.Utility.ShowMessage("SongsManager Message:" + " Update db now work!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                        logger.Info("Update db now work!");
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
