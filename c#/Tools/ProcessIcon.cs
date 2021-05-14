using Tools.ExecutionQueue;
using Tools.SyncIp;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.Properties;

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
		public static Speech _Speech;
		public static SpeechService _SpeechService;
		public static bool HookKeyActive = true;
		public static bool QueueServiceActive = true;
		public static bool SyncIpServiceActive = true;
		public static bool SpeechServiceActive = true;
		public static bool SpeechShowHideActive = false;

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

				Task.Run(() => _SyncIpService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("SyncIpServiceMenuItem");
			}

			if (bool.Parse(ConfigurationManager.AppSettings["Speech"]))
			{
				SpeechServiceActive = true;

				_Speech = new Speech();
				//_SpeechService = new SpeechService();

				Task.Run(() => _Speech.Start());
				//Task.Run(() => _SpeechService.Start());
			}
			else
			{
				ContextMenus.SetMenuItem("SpeechServiceMenuItem");
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