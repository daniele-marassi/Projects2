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
using Additional;

namespace Tools.ExecutionQueue
{
    public partial class QueueService : ServiceBase
    {
        bool serviceActive = true;
        private IExecutionQueuesRepository _repo;
        string oldServiceError = null;
        string oldRunExeError = null;
        string oldUpdateError = null;
        string host = "";
        int sleepOfTheQueueServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        string suppDatabaseConnection = "";
        string rootPath;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        Utility utilty;
        System.Collections.Specialized.NameValueCollection appSettings;
        int windowWidth;
        int windowHeight;
        int windowX;
        int windowY;
        string windowCaption;
        int workingAreaHeight;
        bool alwaysShow;

        public QueueService()
        {
            InitializeComponent();
            utilty = new Utility();
            appSettings = ConfigurationManager.AppSettings;
            windowWidth = int.Parse(appSettings["WindowWidth"]);
            windowHeight = int.Parse(appSettings["WindowHeight"]);
            windowCaption = appSettings["WindowCaption"];
            alwaysShow = bool.Parse(appSettings["AlwaysShow"]);
            workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
            windowX = 0;
            windowY = 0;

            host = appSettings["Host"];
            sleepOfTheQueueServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheQueueServiceInMilliseconds"]);
            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            suppDatabaseConnection = ConfigurationManager.AppSettings["SuppDatabaseConnection"];
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);
            _repo = new ExecutionQueuesRepository(suppDatabaseConnection);
            Database.SetInitializer<SuppDatabaseContext>(null);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
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
                utilty.MoveExtWindow(windowCaption, windowX, windowY, windowWidth, windowHeight);
            }
            catch (Exception)
            {
            }
        }

        public async Task Start()
        {
            while (serviceActive)
            {
                try
                {
                    var getAllExecutionQueuesResult = await _repo.GetAllExecutionQueues();

                    if (getAllExecutionQueuesResult.Successful)
                    {
                        var newQueue = getAllExecutionQueuesResult.Data.Where(_ => _.Host?.Trim().ToLower() == host.Trim().ToLower() && _.StateQueue == null).OrderBy(_ => _.Id).ToList();

                        var runningQueue = getAllExecutionQueuesResult.Data.Where(_ => _.Host?.Trim().ToLower() == host.Trim().ToLower() && _.StateQueue == ExecutionQueueStateQueue.RunningStep2.ToString()).OrderBy(_ => _.Id).ToList();

                        foreach (var item in runningQueue)
                        {
                            try
                            {
                                item.StateQueue = ExecutionQueueStateQueue.Executed.ToString();
                                var updateExecutionQueueResult = await _repo.UpdateExecutionQueue(item);

                                var activeWindowTitle = utilty.GetActiveWindowTitle();
                                if ((activeWindowTitle != windowCaption || item.Type == "ForceHideApplication") && alwaysShow == false) Hide();
                            }
                            catch (Exception)
                            {
                            }

                        }

                        foreach (var item in newQueue)
                        {
                            if (item.Type == ExecutionQueueType.RunExe.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.AttemptToStart.ToString();
                            }

                            if (item.Type == ExecutionQueueType.Other.ToString())
                            {
                                item.StateQueue = ExecutionQueueStateQueue.AttemptToStart.ToString();
                            }

                            var updateExecutionQueueResult = await _repo.UpdateExecutionQueue(item);

                            if (!updateExecutionQueueResult.Successful)
                            {
                                if (oldUpdateError == null || oldUpdateError != updateExecutionQueueResult.Message)
                                {
                                    nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                                    {
                                        Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem");
                                        Common.Utility.ShowMessage("QueueService Message:" + updateExecutionQueueResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                        oldUpdateError = updateExecutionQueueResult.Message;
                                        logger.Error(oldUpdateError);
                                    }
                                }
                            }
                            else
                            {
                                if (item.Type == ExecutionQueueType.RunExe.ToString())
                                {
                                    //Task.Run(() => RunExeAndUpdateDbAsync(item));
                                    await RunExeAndUpdateDbAsync(item);
                                }

                                if (item.Type == ExecutionQueueType.Other.ToString())
                                {
                                    //TODO
                                }

                                if (oldUpdateError != null)
                                {
                                    nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                                    {
                                        oldUpdateError = null;
                                        Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem");
                                        Common.Utility.ShowMessage("QueueService Message:" + " Update db now work!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                        logger.Info("Update db now work!");
                                    }
                                }
                            }
                        }

                        if (oldServiceError != null)
                        {
                            nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem");
                                Common.Utility.ShowMessage("QueueService Message:" + "Queue Service Recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                logger.Info("Queue Service Recovered!");
                            }
                         }

                        oldServiceError = null;
                    }
                    else if (oldServiceError == null || oldServiceError != getAllExecutionQueuesResult.Message)
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem");
                            Common.Utility.ShowMessage("QueueService Message:" + getAllExecutionQueuesResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            oldServiceError = getAllExecutionQueuesResult.Message;
                            logger.Error(oldServiceError);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (oldServiceError == null || oldServiceError != ex.Message)
                    {
                        nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                        using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                        {
                            Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem");
                            Common.Utility.ShowMessage("QueueService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                            oldServiceError = ex.Message;
                            logger.Error(oldServiceError);
                        }
                    }
                }

                System.Threading.Thread.Sleep(sleepOfTheQueueServiceInMilliseconds);
            }
        }

        /// <summary>
        /// RunExeAsync
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task RunExeAndUpdateDbAsync(ExecutionQueueDto item)
        {
            var result = Common.Utility.RunExeAsync(item.FullPath, item.Arguments).GetAwaiter().GetResult();

            if (result.Error != null && result.Error != String.Empty)
            {
                Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem");
                var error = result.Error + " [" + item.FullPath + "]";
                if (oldRunExeError == null || oldRunExeError != error)
                {
                    nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                    {
                        Common.Utility.ShowMessage("QueueService Message:" + error, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                        oldRunExeError = error;
                        logger.Error(oldRunExeError);
                    }
                }
            }
            else 
            {
                if (oldRunExeError != null)
                {
                    oldRunExeError = null;
                    Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem");
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
                if (oldUpdateError == null || oldUpdateError != updateExecutionQueueResult.Message)
                {
                    nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                    {
                        Common.ContextMenus.SetMenuItemWithError("QueueServiceMenuItem");
                        Common.Utility.ShowMessage("QueueService Message:" + updateExecutionQueueResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                        oldUpdateError = updateExecutionQueueResult.Message;
                        logger.Error(oldUpdateError);
                    }
                }
            }
            else
            {
                if (oldUpdateError != null)
                {
                    nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                    using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                    {
                        oldUpdateError = null;
                        Common.ContextMenus.SetMenuItemRecover("QueueServiceMenuItem");
                        Common.Utility.ShowMessage("QueueService Message:" + " Update db now work!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
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
