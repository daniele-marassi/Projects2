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
        private string[] bluetoothDeviceServiceGuidOrBluetoothServiceNameList;

        public ReconnectBluetoothDeviceService()
        {
            InitializeComponent();

            sleepOfTheReconnectBluetoothDeviceServiceInMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepOfTheReconnectBluetoothDeviceServiceInMilliseconds"]);
            limitLogFileInMB = int.Parse(ConfigurationManager.AppSettings["LimitLogFileInMB"]);

            bluetoothDeviceList = new string[0];
            bluetoothDevicePasswordList = new string[0];
            bluetoothDeviceServiceGuidOrBluetoothServiceNameList = new string[0];

            var value = "";

            value = ConfigurationManager.AppSettings["BluetoothDeviceList"];
            if(value != null) bluetoothDeviceList = value.Split(',');

            value = ConfigurationManager.AppSettings["BluetoothDevicePasswordList"];
            if (value != null) bluetoothDevicePasswordList = value.Split(',');

            value = ConfigurationManager.AppSettings["BluetoothDeviceServiceGuidOrBluetoothServiceNameList"];
            if (value != null) bluetoothDeviceServiceGuidOrBluetoothServiceNameList = value.Split(',');

            this.ServiceName = "ReconnectBluetoothDeviceService";
            utility = new Utility();

            timeToClosePopUpInMilliseconds = int.Parse(ConfigurationManager.AppSettings["TimeToClosePopUpInMilliseconds"]);
            rootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
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
                        var value = bluetoothDeviceServiceGuidOrBluetoothServiceNameList[i];
                        var guid = Guid.NewGuid();

                        try
                        {
                            guid = Guid.Parse(value);
                        }
                        catch (Exception)
                        {
                            Type t = typeof(BluetoothService);
                            FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);

                            foreach (FieldInfo fi in fields)
                            {
                                if (fi.Name.ToLower() == value.Trim().ToLower())
                                {
                                    guid = (Guid)fi.GetValue(null);
                                }
                            }                           
                        }

                        (string Message, bool Successful) reconnectBluetoothDeviceResult = utility.ReconnectBluetoothDevice(bluetoothDeviceList[i].ToString(), bluetoothDevicePasswordList[i].ToString(), guid);

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
    }
}
