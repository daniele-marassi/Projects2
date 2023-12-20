using Additional;
using Additional.NLog;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tools.Common.ContextMenus;
using System.Linq;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Tools.ReconnectBluetoothDevice
{
    public partial class ReconnectBluetoothDeviceService : ServiceBase
    {
        Common.Utility commonUtility;
        Utility utility;
        int sleepOfTheReconnectBluetoothDeviceServiceInMilliseconds = 1000;
        int timeToClosePopUpInMilliseconds = 1000;
        string rootPath;
        private static string showError = null;
        private static bool serviceActive = true;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private NLogUtility nLogUtility = new NLogUtility();
        private static int limitLogFileInMB = 0;
        private static string[] bluetoothDeviceList;
        private static string[] bluetoothDevicePasswordList;
        private static string[] bluetoothDeviceTypeList;
        private static double volumePercent;
        int volumeOfNotify;
        bool notifyMute;
        bool notifyPopupShow;
        System.Collections.Specialized.NameValueCollection appSettings;
        private static bool forceReset = true;

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

            volumeOfNotify = int.Parse(ConfigurationManager.AppSettings["VolumeOfNotify"]);

            notifyMute = bool.Parse(ConfigurationManager.AppSettings["NotifyMute"]);
            notifyPopupShow = bool.Parse(ConfigurationManager.AppSettings["NotifyPopupShow"]);

            commonUtility = new Common.Utility();
        }

        public void Stop()
        {
            serviceActive = false;
        }

        public async Task Start()
        {
            forceReset = true;

            while (serviceActive)
            {

                System.Threading.Thread.Sleep(60000);

                if (serviceActive == false) return;
                for (int i = 0; i < bluetoothDeviceList.Length; i++)
                {
                    if (serviceActive == false) return;
                    try
                    {
                        (string Message, bool Successful, bool PairAlreadyExists) reconnectBluetoothDeviceResult;
                        reconnectBluetoothDeviceResult.Message = "";
                        reconnectBluetoothDeviceResult.Successful = false;
                        reconnectBluetoothDeviceResult.PairAlreadyExists = false;

                        if (bluetoothDeviceTypeList[i].ToString().ToLower() == "audio")
                        {
                            reconnectBluetoothDeviceResult = ReconnectBluetoothDevice(bluetoothDeviceList[i].ToString(), bluetoothDevicePasswordList[i].ToString(), true, forceReset);
                            forceReset = false;
                        }
                        if (!reconnectBluetoothDeviceResult.Successful && (showError == null || reconnectBluetoothDeviceResult.Message != showError))
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemWithError("ReconnectBluetoothDeviceServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                                if (notifyPopupShow) Common.Utility.ShowMessage("ReconnectBluetoothDeviceService Message:" + reconnectBluetoothDeviceResult.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = reconnectBluetoothDeviceResult.Message;
                                logger.Error(reconnectBluetoothDeviceResult.Message);
                            }
                        }
                        else if (reconnectBluetoothDeviceResult.Successful && showError != null)
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemRecover("ReconnectBluetoothDeviceServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServiceActive);
                                if (notifyPopupShow) Common.Utility.ShowMessage("ReconnectBluetoothDeviceService Message:" + "Service recovered!", MessagesPopUp.MessageType.Info, timeToClosePopUpInMilliseconds, rootPath);
                                showError = null;
                                logger.Info(reconnectBluetoothDeviceResult.Message);
                            }
                        }

                        if (reconnectBluetoothDeviceResult.Successful && bluetoothDeviceTypeList[i].ToString().ToLower() == "audio" && reconnectBluetoothDeviceResult.PairAlreadyExists == false)
                        {
                            //System.Threading.Thread.Sleep(500);
                            await Task.Run(() => commonUtility.SetVolume(volumePercent));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (showError == null || ex.Message != showError)
                        {
                            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                            {
                                appSettings = ConfigurationManager.AppSettings; notifyMute = bool.Parse(appSettings["NotifyMute"]);
                                notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);

                                if (serviceActive) Common.ContextMenus.SetMenuItemWithError("ReconnectBluetoothDeviceServiceMenuItem", volumeOfNotify, notifyMute, ResourcesType.ServicesError);
                                if (notifyPopupShow) Common.Utility.ShowMessage("ReconnectBluetoothDeviceService Message:" + ex.Message, MessagesPopUp.MessageType.Error, timeToClosePopUpInMilliseconds, rootPath);
                                showError = ex.Message;
                                logger.Error(ex.Message);
                            }
                        }
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                System.Threading.Thread.Sleep(sleepOfTheReconnectBluetoothDeviceServiceInMilliseconds);
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }

        /// <summary>
        /// Set Registry Key
        /// </summary>
        /// <param name="root"></param>
        /// <param name="KeyPath"></param>
        /// <param name="valueName"></param>
        /// <param name="valueData"></param>
        /// <param name="registryValueKind"></param>
        /// <param name="registryType"></param>
        public void SetRegistryKey(RegistryHive root, string KeyPath, string valueName, object valueData, RegistryValueKind registryValueKind, RegistryView registryType)
        {
            var regKeySpecific = RegistryKey.OpenBaseKey(root, registryType);
            var registryKey = regKeySpecific.OpenSubKey(KeyPath, true);
            registryKey.SetValue(valueName, valueData, registryValueKind);
            registryKey.Close();
        }

        /// <summary>
        /// Reconnect Bluetooth Device
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="password"></param>
        /// <param name="removeDevice"></param>
        /// <param name="forceReset"></param>
        /// <returns></returns>
        public (string Message, bool Successful, bool PairAlreadyExists) ReconnectBluetoothDevice(string deviceName, string password, bool removeDevice, bool forceReset = false)
        {
            (string Message, bool Successful, bool PairAlreadyExists) result;
            result.Message = "";
            result.Successful = false;
            result.PairAlreadyExists = false;

            var bluetoothPairedDeviceInfo = GetBluetoothDeviceByName(GetBluetoothPairedDevices(), deviceName);
            var bluetoothDiscoverDeviceInfo = GetBluetoothDeviceByName(GetBluetoothDiscoverDevices(), deviceName);

            var defaultAudioDeviceName = GetDefaultAudioDeviceName();

            if (bluetoothPairedDeviceInfo == null || bluetoothDiscoverDeviceInfo != null || defaultAudioDeviceName.ToLower().Contains(deviceName.ToLower()) == false || forceReset)
            {
                SetRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\PolicyManager\\default\\Connectivity\\AllowBluetooth", "value", 0, RegistryValueKind.DWord, RegistryView.Registry64);

                System.Threading.Thread.Sleep(30000);

                SetRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\PolicyManager\\default\\Connectivity\\AllowBluetooth", "value", 2, RegistryValueKind.DWord, RegistryView.Registry64);

                System.Threading.Thread.Sleep(30000);

                bluetoothDiscoverDeviceInfo = GetBluetoothDeviceByName(GetBluetoothDiscoverDevices(), deviceName);

                if (bluetoothDiscoverDeviceInfo != null || bluetoothPairedDeviceInfo != null)
                {
                    BluetoothAddress deviceAddress = null;

                    if (bluetoothDiscoverDeviceInfo != null)
                        deviceAddress = bluetoothDiscoverDeviceInfo?.DeviceAddress;
                    else
                        deviceAddress = bluetoothPairedDeviceInfo?.DeviceAddress;

                    if (removeDevice)
                    {
                        RemoveBluetoothDevice(deviceAddress);

                        System.Threading.Thread.Sleep(30000);
                    }

                    var connectResult = ConnectBluetoothSpeakers(deviceAddress, password);

                    result.Message = connectResult.Message;
                    result.Successful = connectResult.Successful;
                    result.PairAlreadyExists = false;
                }
                else
                {
                    result.Message = $"Device {deviceName} not found!";
                    result.Successful = false;
                    result.PairAlreadyExists = false;
                }
            }
            else
            {
                result.Message = "The device is already paired.";
                result.Successful = true;
                result.PairAlreadyExists = true;
            }

            return result;
        }

        /// <summary>
        /// Get Bluetooth Discover Devices
        /// </summary>
        /// <returns></returns>
        public List<BluetoothDeviceInfo> GetBluetoothDiscoverDevices()
        {
            var result = new List<BluetoothDeviceInfo>() { };

            BluetoothClient bluetoothClient = null;

            try
            {
                bluetoothClient = new BluetoothClient();
                var buleRadio = BluetoothRadio.Default;//PrimaryRadio;
                buleRadio.Mode = RadioMode.Connectable; //.Connectable;
                var devices = bluetoothClient.DiscoverDevices();

                result.AddRange(devices);
            }
            catch (Exception)
            {
                try
                {
                    bluetoothClient.Close();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Get Bluetooth Paired Devices
        /// </summary>
        /// <returns></returns>
        public List<BluetoothDeviceInfo> GetBluetoothPairedDevices()
        {
            var result = new List<BluetoothDeviceInfo>() { };

            BluetoothClient bluetoothClient = null;

            try
            {
                bluetoothClient = new BluetoothClient();
                var buleRadio = BluetoothRadio.Default;//PrimaryRadio;
                buleRadio.Mode = RadioMode.Connectable; //.Connectable;
                var devices = bluetoothClient.PairedDevices.ToList();

                result.AddRange(devices);
            }
            catch (Exception)
            {
                try
                {
                    bluetoothClient.Close();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Get Bluetooth Device By Name
        /// </summary>
        /// <param name="bluetoothDevices"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public BluetoothDeviceInfo GetBluetoothDeviceByName(List<BluetoothDeviceInfo> bluetoothDevices, string deviceName)
        {
            BluetoothDeviceInfo result;

            result = bluetoothDevices.Where(_ => _.DeviceName.Trim().ToLower() == deviceName.Trim().ToLower()).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Connect Bluetooth Speakers
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public (string Message, bool Successful) ConnectBluetoothSpeakers(BluetoothAddress bluetoothAddress, string password)
        {
            (string Message, bool Successful) result;
            result.Message = "";
            result.Successful = false;

            BluetoothClient bluetoothClient = null;

            try
            {
                var device = new BluetoothDeviceInfo(bluetoothAddress);
                BluetoothSecurity.PairRequest(bluetoothAddress, password);
                bluetoothClient = new BluetoothClient();
                bluetoothClient.Connect(device.DeviceAddress, BluetoothService.Handsfree);

                if (bluetoothClient.Connected)
                {
                    result.Message = "The pairing is successful.";
                    result.Successful = true;
                }
                else
                {
                    result.Message = "The pairing is failed.";
                    result.Successful = false;
                }

                var stream = bluetoothClient.GetStream();
                stream.Close();

                bluetoothClient.Close();
                device.SetServiceState(BluetoothService.Handsfree, true);
                device.SetServiceState(BluetoothService.AudioSink, true);
                device.SetServiceState(BluetoothService.AVRemoteControl, true);
                device.SetServiceState(BluetoothService.GenericAudio, true);
                device.SetServiceState(BluetoothService.AudioVideo, true);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Successful = false;

                try
                {
                    bluetoothClient.Close();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Remove Bluetooth Device
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <returns></returns>
        public void RemoveBluetoothDevice(BluetoothAddress bluetoothAddress)
        {
            try
            {
                BluetoothSecurity.RemoveDevice(bluetoothAddress);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// GetDefaultAudioDeviceName
        /// </summary>
        /// <returns></returns>
        public string GetDefaultAudioDeviceName()
        {
            var defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            return defaultPlaybackDevice.FullName;
        }
    }
}
