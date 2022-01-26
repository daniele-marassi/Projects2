using Tools.ExecutionQueue;
using Tools.SyncIp;
using Tools.RenewNotes;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Properties;
using Tools.Songs;
using Tools.WakeUpScreenAfterPowerBreak;
using Tools.ReconnectBluetoothDevice;

namespace Tools
{
	/// <summary>
	/// 
	/// </summary>
	public class ProcessIcon : IDisposable
	{
        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        NotifyIcon ni;
		public static QueueService _QueueService;
		public static SyncIpService _SyncIpService;
		public static ReconnectBluetoothDeviceService _ReconnectBluetoothDeviceService;
		public static RenewNotesService _RenewNotesService;
		public static WakeUpScreenAfterPowerBreakService _WakeUpScreenAfterPowerBreakService;
		public static Speech _Speech;
		public static SpeechService _SpeechService;
		public static SongsManager _SongsManager;
		
		public static bool HookKeyActive = true;
		public static bool QueueServiceActive = true;
		public static bool SyncIpServiceActive = true;
		public static bool ReconnectBluetoothDeviceServiceActive = true;	
		public static bool RenewNotesServiceActive = true;
		public static bool WakeUpScreenAfterPowerBreakServiceActive = true;
		public static bool SpeechServiceActive = true;
		public static int SpeechShowHideActive = -1;
		public static bool SongsManagerActive = true;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();

			Hook.controlKey = ConfigurationManager.AppSettings["controlKey"];
			Hook.keyUp = ConfigurationManager.AppSettings["keyUp"];
			Hook.keyDown = ConfigurationManager.AppSettings["keyDown"];

			Hook.methodOfUse = Environment.NewLine + "HookKey - method of use: ";

            if (Hook.controlKey != String.Empty) Hook.methodOfUse += Environment.NewLine + "ControlKey " + "[" + Hook.controlKey + "]";
			Hook.methodOfUse += Environment.NewLine + "keyUp " + "[" + Hook.keyUp + "]";
			Hook.methodOfUse += Environment.NewLine + "keyDown " + "[" + Hook.keyDown + "]";
		}

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = Resources.Tools;
			ni.Text = "Tools";
			ni.Visible = true;

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();

			if (bool.Parse(ConfigurationManager.AppSettings["HookKey"]))
			{
				HookKeyActive = true;

				//HookManager.EnableKeyDownHook();
				System.Threading.Thread.Sleep(50);
				Hook.Start();
			}
			else
			{
				ContextMenus.SetMenuItem("HookKeyMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["QueueService"]))
			{
				QueueServiceActive = true;

				_QueueService = new QueueService();

				System.Threading.Thread.Sleep(50);
				Task.Run(() => _QueueService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("QueueServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["SyncIpService"]))
			{
				SyncIpServiceActive = true;

				_SyncIpService = new SyncIpService();

				System.Threading.Thread.Sleep(50);
				Task.Run(() => _SyncIpService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("SyncIpServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["ReconnectBluetoothDeviceService"]))
			{
				ReconnectBluetoothDeviceServiceActive = true;

				_ReconnectBluetoothDeviceService = new ReconnectBluetoothDeviceService();

				System.Threading.Thread.Sleep(50);
				Task.Run(() => _ReconnectBluetoothDeviceService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("ReconnectBluetoothDeviceServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["RenewNotesService"]))
			{
				RenewNotesServiceActive = true;

				_RenewNotesService = new RenewNotesService();

				System.Threading.Thread.Sleep(50);
				Task.Run(() => _RenewNotesService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("RenewNotesServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["WakeUpScreenAfterPowerBreakService"]))
			{
				WakeUpScreenAfterPowerBreakServiceActive = true;

				_WakeUpScreenAfterPowerBreakService = new WakeUpScreenAfterPowerBreakService();

				System.Threading.Thread.Sleep(50);
				Task.Run(() => _WakeUpScreenAfterPowerBreakService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("WakeUpScreenAfterPowerBreakServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["Speech"]))
			{
				SpeechServiceActive = true;

				_Speech = new Speech();
				//_SpeechService = new SpeechService();

				System.Threading.Thread.Sleep(50);
				Task.Run(() => _Speech.Start());
				//Task.Run(() => _SpeechService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("SpeechServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["SongsManager"]))
			{
				SongsManagerActive = true;

				//_SongsManager = new SongsManager();

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("SongsManagerMenuItem");
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

		/// <summary>
		/// Handles the MouseClick event of the ni control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		void ni_MouseClick(object sender, MouseEventArgs e)
		{
			// Handle mouse button clicks.
			if (e.Button == MouseButtons.Left)
			{
				// Start 

			}
		}
	}
}