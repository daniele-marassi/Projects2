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
		
		public static bool? HookKeyActive = null;
		public static bool? QueueServiceActive = null;
		public static bool? SyncIpServiceActive = null;
		public static bool? ReconnectBluetoothDeviceServiceActive = null;	
		public static bool? RenewNotesServiceActive = null;
		public static bool? WakeUpScreenAfterPowerBreakServiceActive = null;
		public static bool? SpeechServiceActive = null;		
		public static bool? SongsManagerActive = null;
		public static bool? NotifyMuteActive = null;
		public static bool? NotifyPopupShowActive = null;

		public static int SpeechShowHideActive = -1;
		bool notifyMute;
		bool notifyPopupShow;


		System.Collections.Specialized.NameValueCollection appSettings;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();
			appSettings = ConfigurationManager.AppSettings;
			Hook.controlKey = appSettings["controlKey"];
			Hook.keyUp = appSettings["keyUp"];
			Hook.keyDown = appSettings["keyDown"];

			Hook.methodOfUse = Environment.NewLine + "HookKey - method of use: ";

            if (Hook.controlKey != String.Empty) Hook.methodOfUse += Environment.NewLine + "ControlKey " + "[" + Hook.controlKey + "]";
			Hook.methodOfUse += Environment.NewLine + "keyUp " + "[" + Hook.keyUp + "]";
			Hook.methodOfUse += Environment.NewLine + "keyDown " + "[" + Hook.keyDown + "]";

			notifyMute = bool.Parse(appSettings["NotifyMute"]);
			notifyPopupShow = bool.Parse(appSettings["NotifyPopupShow"]);
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

			if (bool.Parse(appSettings["HookKey"]))
			{
				HookKeyActive = false;
				ContextMenus.SetMenuItem("HookKeyMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("HookKeyMenuItem");
			}

			if (bool.Parse(appSettings["NotifyMute"]))
			{
				NotifyMuteActive = false;
				ContextMenus.SetMenuItem("NotifyMuteMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("NotifyMuteMenuItem");
			}

			if (bool.Parse(appSettings["NotifyPopupShow"]))
			{
				NotifyPopupShowActive = false;
				ContextMenus.SetMenuItem("NotifyPopupShowMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("NotifyPopupShowMenuItem");
			}

			if (bool.Parse(appSettings["QueueService"]))
			{
				QueueServiceActive = false;
				ContextMenus.SetMenuItem("QueueServiceMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("QueueServiceMenuItem");
			}

			if (bool.Parse(appSettings["SyncIpService"]))
			{
				SyncIpServiceActive = false;
				ContextMenus.SetMenuItem("SyncIpServiceMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("SyncIpServiceMenuItem");
			}

			if (bool.Parse(appSettings["ReconnectBluetoothDeviceService"]))
			{
				ReconnectBluetoothDeviceServiceActive = false;
				ContextMenus.SetMenuItem("ReconnectBluetoothDeviceServiceMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("ReconnectBluetoothDeviceServiceMenuItem");
			}

			if (bool.Parse(appSettings["RenewNotesService"]))
			{
				RenewNotesServiceActive = false;
				ContextMenus.SetMenuItem("RenewNotesServiceMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("RenewNotesServiceMenuItem");
			}

			if (bool.Parse(appSettings["WakeUpScreenAfterPowerBreakService"]))
			{
				WakeUpScreenAfterPowerBreakServiceActive = false;
				ContextMenus.SetMenuItem("WakeUpScreenAfterPowerBreakServiceMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("WakeUpScreenAfterPowerBreakServiceMenuItem");
			}

			if (bool.Parse(appSettings["Speech"]))
			{
				SpeechServiceActive = false;
				ContextMenus.SetMenuItem("SpeechServiceMenuItem");

				System.Threading.Thread.Sleep(50);
			}
			else
			{
				ContextMenus.SetMenuItem("SpeechServiceMenuItem");
			}

			if (bool.Parse(appSettings["SongsManager"]))
			{
				//SongsManager strat only with click
				ContextMenus.SetMenuItem("SongsManagerMenuItem");

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