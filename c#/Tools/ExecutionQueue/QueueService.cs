using Additional.NLog;
using Tools.ExecutionQueue.Contexts;
using Tools.ExecutionQueue.Contracts;
using Tools.ExecutionQueue.Models;
using Tools.ExecutionQueue.Repositories;
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
using Tools.Songs.Contracts;
using Tools.Songs.Repositories;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using static Tools.Common.ContextMenus;
using ExecutionQueue;
using System.Reflection.Emit;
using System.ServiceModel.Channels;
using System.Timers;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace Tools.ExecutionQueue
{
    public partial class QueueService : ServiceBase
    {
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        bool serviceActive = true;
        private IExecutionQueuesRepository _repo;
        private ISongsRepository _songsRepo;
        string oldServiceError = null;
        string oldRunExeError = null;
        string oldUpdateError = null;
        string host = "";
        int sleepOfTheQueueServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        int sleepOfTheRunMediaAndPlayInMilliseconds = 1000;
        string suppDatabaseConnection = "";
        string rootPath;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        Additional.Utility utility;
        Common.Utility commonUtility;
        System.Collections.Specialized.NameValueCollection appSettings;
        int windowWidth;
        int windowHeight;
        int windowX;
        int windowY;
        string windowCaption;
        int workingAreaHeight;
        bool alwaysShow;
        string songsPlayer;
        int volumeOfNotify;
        bool notifyMute;
        bool notifyPopupShow;

        System.Timers.Timer timer;

        private void TimerEventProcessor(object source, ElapsedEventArgs e)
        {
            StopTimer();
            Init();
            serviceActive = true;
            Start();
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 50000;
            timer.Elapsed += new ElapsedEventHandler(TimerEventProcessor);
            //timer.Enabled = true;
            timer.Start();
        }

        private void StopTimer()
        {
            //timer.Stop();
            timer.Enabled = false;
            timer = null;
        }

        private void Init()
        {
            nLogUtility = new NLogUtility();
            utility = new Additional.Utility();
            commonUtility = new Common.Utility();
            appSettings = ConfigurationManager.AppSettings;
            windowWidth = int.Parse(appSettings["WindowWidth"]);
            windowHeight = int.Parse(appSettings["WindowHeight"]);
            windowCaption = appSettings["WindowCaption"];
            alwaysShow = bool.Parse(appSettings["AlwaysShow"]);
            songsPlayer = appSettings["SongsPlayer"];
            workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
            windowX = 0;
            windowY = 0;

            host = appSettings["Host"];
            sleepOfTheQueueServiceInMilliseconds = int.Parse(appSettings["SleepOfTheQueueServiceInMilliseconds"]);
            timeToClosePopUpInMilliseconds = int.Parse(appSettings["TimeToClosePopUpInMilliseconds"]);
            sleepOfTheRunMediaAndPlayInMilliseconds = int.Parse(appSettings["SleepOfTheRunMediaAndPlayInMilliseconds"]);
            
            suppDatabaseConnection = appSettings["SuppDatabaseConnection"];
            limitLogFileInMB = int.Parse(appSettings["LimitLogFileInMB"]);
            _repo = new ExecutionQueuesRepository(suppDatabaseConnection);
            _songsRepo = new SongsRepository(suppDatabaseConnection);
            Database.SetInitializer<SuppDatabaseContext>(null);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            volumeOfNotify = int.Parse(appSettings["VolumeOfNotify"]);

            notifyMute = bool.Parse(appSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

            timer = null;
        }

        public QueueService()
        {
            InitializeComponent();
            Init();
        }

        protected override void OnStart(string[] args)
        {

        }

        public void Stop()
        {
            serviceActive = false;
        }

        public void Hide()
        {
            windowX = 0;
            windowY = workingAreaHeight;

            try
            {
                utility.MoveExtWindow(windowCaption, windowX, windowY, windowWidth, windowHeight);
            }
            catch (Exception)
            {
            }
        }

        public async Task Start()
        {
            while (serviceActive)
            {
                if (serviceActive == false) return;

                try
                {
                    var now = DateTime.Now;
                    var newQueueResult = await _repo.GetQueues(host.Trim(), ExecutionQueueStateQueue.NONE.ToString(), now);

                    var runningQueueResult = await _repo.GetQueues(host.Trim(), ExecutionQueueStateQueue.RunningStep2.ToString(), now);

                    if (newQueueResult.Successful)
                    {
                        var newQueue = newQueueResult.Data.OrderBy(_ => _.Id).ToList();

                        var runningQueue = runningQueueResult.Data.OrderBy(_ => _.Id).ToList();

                        foreach (var item in runningQueue)
                        {
                            try
                            {
                                item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                                var updateExecutionQueueResult = await _repo.UpdateExecutionQueue(item);

                                var activeWindowTitle = utility.GetActiveWindowTitle();
                                if ((activeWindowTitle != windowCaption || item.Type == ExecutionQueueType.ForceHideApplication.ToString()) && alwaysShow == false) Hide();
                            }
                            catch (Exception)
                            {
                            }
                        }

                        foreach (var item in newQueue)
                        {
                            if (item.Type == ExecutionQueueType.RunExe.ToString() || item.Type == ExecutionQueueType.SystemRunExe.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.AttemptToStart.ToString();
                            }

                            if (item.Type == ExecutionQueueType.Other.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.AttemptToStart.ToString();
                            }

                            if (item.Type == ExecutionQueueType.SongsPlayer.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.AttemptToStart.ToString();
                            }

                            if (item.Type == ExecutionQueueType.WakeUpScreenAfterEhi.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                            }

                            if (item.Type == ExecutionQueueType.MediaPlayOrPause.ToString())
                            {
                                MediaPlayOrPause();

                                item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                            }

                            if (item.Type == ExecutionQueueType.MediaNextTrack.ToString())
                            {
                                MediaNextTrack();

                                item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                            }

                            if (item.Type == ExecutionQueueType.MediaPreviousTrack.ToString())
                            {
                                MediaPreviousTrack();

                                item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                            }

                            if (item.Type == ExecutionQueueType.RunMediaAndPlay.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.AttemptToStart.ToString();
                            }

                            var updateExecutionQueueResult = await _repo.UpdateExecutionQueue(item);

                            if (!updateExecutionQueueResult.Successful)
                            {
                                if (oldUpdateError == null || (oldUpdateError != null && oldUpdateError?.RemoveIntegers()?.Contains(updateExecutionQueueResult.Message.RemoveIntegers()) == false))
                                {
                                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                                    {
                                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                        if (serviceActive) Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                                        if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + updateExecutionQueueResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                        
                                        if (oldUpdateError != null) oldUpdateError += " - ";
                                        else oldUpdateError = String.Empty;

                                        oldUpdateError += updateExecutionQueueResult.Message;

                                        logger.Error(updateExecutionQueueResult.Message);
                                    }
                                }
                            }
                            else
                            {
                                if (item.Type == ExecutionQueueType.SongsPlayer.ToString())
                                {
                                    (string Command, long Id, string FullPath) arguments;

                                    arguments.Command = "";
                                    arguments.Id = 0;
                                    arguments.FullPath = "";
                                    var failed = false;

                                    try
                                    {
                                        arguments = JsonConvert.DeserializeObject<(string Command, long Id, string FullPath)>(item.Arguments);
                                    }
                                    catch (Exception)
                                    {
                                        failed = true;
                                    }

                                    if (arguments.Command == "play" || arguments.Command == "forward" || arguments.Command == "previous")
                                    {
                                        var getSongsByIdResult = await _songsRepo.GetSongsById(arguments.Id);

                                        if (getSongsByIdResult.Successful)
                                        {
                                            var song = getSongsByIdResult.Data.FirstOrDefault();

                                            item.FullPath = songsPlayer;
                                            item.Arguments = @"""" + arguments.FullPath + @"""";

                                            await RunExeAndUpdateDbAsync(item, false);

                                            song.Listened = true;

                                            var updateSongsResult = await _songsRepo.UpdateSongs(song);
                                        }
                                    }

                                    if (arguments.Command == "stop")
                                    {
                                        EndTask(Path.GetFileName(songsPlayer));
                                    }

                                    if(failed)
                                    {
                                        item.StateQueue = ExecutionQueueStateQueue.Failed.ToString();
                                        updateExecutionQueueResult = await _repo.UpdateExecutionQueue(item);
                                    }
                                }

                                if (item.Type == ExecutionQueueType.RunExe.ToString() || item.Type == ExecutionQueueType.SystemRunExe.ToString())
                                {
                                    //await Task.Run(() => RunExeAndUpdateDbAsync(item, false));
                                    await RunExeAndUpdateDbAsync(item, false);
                                }

                                if (item.Type == ExecutionQueueType.RunMediaAndPlay.ToString())
                                {
                                    await RunExeAndUpdateDbAsync(item, false, true);
                                }

                                if (item.Type == ExecutionQueueType.Other.ToString())
                                {
                                    //TODO
                                }

                                if (item.Type == ExecutionQueueType.WakeUpScreenAfterEhi.ToString())
                                {
                                    commonUtility.ClickOnTaskbar();
                                }

                                if (oldUpdateError != null)
                                {
                                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                                    {
                                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                        oldUpdateError = null;
                                        if (serviceActive) Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                                        if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + " Update db now work!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                        logger.Info("Update db now work!");
                                    }
                                }
                            }
                        }

                        if (oldServiceError != null)
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                oldServiceError = null;

                                if (serviceActive) Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                                if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + "Queue Service Recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                logger.Info("Queue Service Recovered!");
                            }
                         }
                    }
                    else if (oldServiceError == null || (oldServiceError != null && oldServiceError?.RemoveIntegers()?.Contains(newQueueResult.Message.RemoveIntegers()) == false))
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + newQueueResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);

                            if (oldServiceError != null) oldServiceError += " - ";
                            else oldServiceError = String.Empty;

                            oldServiceError += newQueueResult.Message;
                            logger.Error(newQueueResult.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (oldServiceError == null || (oldServiceError != null && oldServiceError?.RemoveIntegers()?.Contains(ex.Message.RemoveIntegers()) == false))
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);

                            if (oldServiceError != null) oldServiceError += " - ";
                            else oldServiceError = String.Empty;

                            oldServiceError += ex.Message;
                            logger.Error(ex.ToString());
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                if ((oldServiceError != null || oldUpdateError != null) && timer == null)
                {
                    Stop();
                    StartTimer();
                }
                else
                    System.Threading.Thread.Sleep(sleepOfTheQueueServiceInMilliseconds);
            }
        }

        public void EndTask(string taskname)
        {
            string processName = taskname.Replace(".exe", "");

            foreach (Process process in Process.GetProcessesByName(processName))
            {
                process.Kill();
            }
        }

        /// <summary>
        /// RunExeAsync
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RunExeAndUpdateDbAsync(ExecutionQueueDto item, bool async, bool playMedia = false)
        {
            (string Output, string Error) result;
            result.Output = null;
            result.Error = null;

            try
            {
                result = utility.RunExe(item.FullPath, item.Arguments, async).GetAwaiter().GetResult();

                if (result.Error != null && result.Error != String.Empty)
                {
                    appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                    notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                    if (serviceActive) Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                    var error = result.Error + " [" + item.FullPath + "]";
                    if (oldRunExeError == null || (oldRunExeError != null && oldRunExeError?.RemoveIntegers()?.Contains(error.RemoveIntegers()) == false))
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + error, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);

                            if (oldRunExeError != null) oldRunExeError += " - ";
                            else oldRunExeError = String.Empty;

                            oldRunExeError += error;
                            logger.Error(error);
                        }
                    }
                }
                else
                {
                    if (oldRunExeError != null)
                    {
                        oldRunExeError = null;
                        appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                        notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                        if (serviceActive) Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                    }
                }

                if (result.Error == null || result.Error == String.Empty)
                {
                    if (!alwaysShow) item.StateQueue = ExecutionQueueStateQueue.RunningStep1.ToString();
                    if (alwaysShow) item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                    item.Output = result.Output;
                }
                else
                {
                    item.StateQueue = ExecutionQueueStateQueue.Failed.ToString();
                    item.Output = result.Error;
                }

                var updateExecutionQueueResult = await _repo.UpdateExecutionQueue(item);

                if (!updateExecutionQueueResult.Successful)
                {
                    if (oldUpdateError == null || (oldUpdateError != null && oldUpdateError?.RemoveIntegers()?.Contains(updateExecutionQueueResult.Message.RemoveIntegers()) == false))
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            if (serviceActive) Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                            if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + updateExecutionQueueResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);

                            if (oldUpdateError != null) oldUpdateError += " - ";
                            else oldUpdateError = String.Empty;

                            oldUpdateError += updateExecutionQueueResult.Message;

                            logger.Error(updateExecutionQueueResult.Message);
                        }
                    }
                }
                else
                {
                    if (oldUpdateError != null)
                    {
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                            notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                            oldUpdateError = null;
                            if (serviceActive) Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                            if (notifyPopupShow) Common.Utility.ShowMessage("QueueService Message:" + " Update db now work!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                            logger.Info("Update db now work!");
                        }
                    }

                    if (playMedia)
                    {
                        await Task.Run(() => RunMediaAndPlayWithDelay());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void RunMediaAndPlayWithDelay() 
        {
            System.Threading.Thread.Sleep(sleepOfTheRunMediaAndPlayInMilliseconds);
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void MediaPlayOrPause()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void MediaNextTrack()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private void MediaPreviousTrack()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        protected override void OnStop()
        {
        }
    }
}
