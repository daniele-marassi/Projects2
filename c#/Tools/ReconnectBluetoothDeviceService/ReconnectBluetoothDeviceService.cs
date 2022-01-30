using Additional;
using Additional.NLog;
using NLog;
using System;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Tools.ReconnectBluetoothDevice
{
    public partial class ReconnectBluetoothDeviceService : ServiceBase
    {
        Utility utility;
        int sleepOfTheReconnectBluetoothDeviceServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        string rootPath;
        string showError = null;
        bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private int limitLogFileInMB = 0;
        private string[] bluetoothDeviceList;
        private string[] bluetoothDevicePasswordList;
        private string[] bluetoothDeviceTypeList;
        private double volumePercent;

        public ReconnectBluetoothDeviceService()
        {
            InitializeComponent();

            sleepOfTheReconnectBluetoothDeviceServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheReconnectBluetoothDeviceServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

            bluetoothDeviceList = new string[0];
            bluetoothDevicePasswordList = new string[0];
            bluetoothDeviceTypeList = new string[0];

            var value = "";

            value = ConfigurationManager.AppSettings["BluetoothDeviceList"];
            if(value != null) bluetoothDeviceList = value.Split(',');

            value = ConfigurationManager.AppSettings["BluetoothDevicePasswordList"];
            if (value != null) bluetoothDevicePasswordList = value.Split(',');

            value = ConfigurationManager.AppSettings["BluetoothDeviceTypeList"];
            if (value != null) bluetoothDeviceTypeList = value.Split(',');

            this.ServiceName = "ReconnectBluetoothDeviceService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            volumePercent = double.Parse(ConfigurationManager.AppSettings["VolumePercent"]);
        }

        public void Stop()
        {
            serviceActive = false;
        }

        public async Task Start()
        {
            while (serviceActive)
            {
                for (int i = 0; i < bluetoothDeviceList.Length; i++)
                {
                    try
                    {
                        (string Message, bool Successful, bool PairAlreadyExists) reconnectBluetoothDeviceResult;
                        reconnectBluetoothDeviceResult.Message = "";
                        reconnectBluetoothDeviceResult.Successful = false;
                        reconnectBluetoothDeviceResult.PairAlreadyExists = false;

                        if (bluetoothDeviceTypeList[i].ToString().ToLower() == "audio")
                            reconnectBluetoothDeviceResult = utility.ReconnectBluetoothDevice(bluetoothDeviceList[i].ToString(), bluetoothDevicePasswordList[i].ToString());

                        if (!reconnectBluetoothDeviceResult.Successful && (showError == null || reconnectBluetoothDeviceResult.Message != showError))
                        {
                            nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                Common.ContextMenus.SetMenuItemWithError("ReconnectBluetoothDeviceServiceMenuItem");
                                Common.Utility.ShowMessage("ReconnectBluetoothDeviceService Message:" + reconnectBluetoothDeviceResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = reconnectBluetoothDeviceResult.Message;
                                logger.Error(reconnectBluetoothDeviceResult.Message);
                            }
                        }
                        else if (reconnectBluetoothDeviceResult.Successful && showError != null)
                        {
                            nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                Common.ContextMenus.SetMenuItemRecover("ReconnectBluetoothDeviceServiceMenuItem");
                                Common.Utility.ShowMessage("ReconnectBluetoothDeviceService Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                showError = null;
                                logger.Info(reconnectBluetoothDeviceResult.Message);
                            }
                        }

                        if (reconnectBluetoothDeviceResult.Successful && bluetoothDeviceTypeList[i].ToString().ToLower() == "audio" && reconnectBluetoothDeviceResult.PairAlreadyExists == false)
                        {
                            //System.Threading.Thread.Sleep(500);
                            SetVolume(volumePercent);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (showError == null || ex.Message != showError)
                        {
                            nLogUtility.ClearNLogFile("mainLog", limitLogFileInMB);
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                Common.ContextMenus.SetMenuItemWithError("ReconnectBluetoothDeviceServiceMenuItem");
                                Common.Utility.ShowMessage("ReconnectBluetoothDeviceService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = ex.Message;
                                logger.Error(ex.Message);
                            }
                        }
                    }
                }

                System.Threading.Thread.Sleep(sleepOfTheReconnectBluetoothDeviceServiceInMilliseconds);
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }

        public void SetVolume(double percent)
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = percent;
        }
    }
}
