using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Additional;
using Tools.Properties;
using System.Threading.Tasks;
using System.Configuration;
using AboutBox;
using Tools.Songs;

namespace Tools
{
	/// <summary>
	/// 
	/// </summary>
	public class ContextMenus
	{
		
		/// <summary>
		/// Is the About box displayed?
		/// </summary>

		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns>ContextMenuStrip</returns>
		public ContextMenuStrip Create()
		{
			// Add the default menu options.
			Common.ContextMenus.Menu = new ContextMenuStrip();
			ToolStripMenuItem item;
			ToolStripSeparator sep;

			// About.
			item = new ToolStripMenuItem();
			item.Text = "About";
			item.Name = "AboutMenuItem";
			item.Click += new EventHandler(About_Click);
			item.Image = Resources.About;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// HookKey.
			item = new ToolStripMenuItem();
			item.Text = "Hook Key";
			item.Name = "HookKeyMenuItem";
			item.Click += new EventHandler(HookKey_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// NotifyMute.
			item = new ToolStripMenuItem();
			item.Text = "Notify Mute";
			item.Name = "NotifyMuteMenuItem";
			item.Click += new EventHandler(NotifyMute_Click);
			item.Image = Resources.SoundNotifyDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// NotifyPopupShow.
			item = new ToolStripMenuItem();
			item.Text = "Notify Popup Show";
			item.Name = "NotifyPopupShowMenuItem";
			item.Click += new EventHandler(NotifyPopupShow_Click);
			item.Image = Resources.NotifyDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// QueueService.
			item = new ToolStripMenuItem();
			item.Text = "Queue Service";
			item.Name = "QueueServiceMenuItem";
			item.Click += new EventHandler(QueueService_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// SongsManager.
			item = new ToolStripMenuItem();
			item.Text = "Songs Manager";
			item.Name = "SongsManagerMenuItem";
			item.Click += new EventHandler(SongsManager_Click);
			item.Image = Resources.SongsManagerDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// SyncIpService.
			item = new ToolStripMenuItem();
			item.Text = "Sync Ip Service";
			item.Name = "SyncIpServiceMenuItem";
			item.Click += new EventHandler(SyncIpService_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// ReconnectBluetoothDeviceService.
			item = new ToolStripMenuItem();
			item.Text = "Reconnect Bluetooth Device Service";
			item.Name = "ReconnectBluetoothDeviceServiceMenuItem";
			item.Click += new EventHandler(ReconnectBluetoothDeviceService_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			//RenewNotesService
			item = new ToolStripMenuItem();
			item.Text = "Renew Notes Service";
			item.Name = "RenewNotesServiceMenuItem";
			item.Click += new EventHandler(RenewNotesService_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			//WakeUpScreenAfterPowerBreakService
			item = new ToolStripMenuItem();
			item.Text = "Wake Up Screen After Power Break Service";
			item.Name = "WakeUpScreenAfterPowerBreakServiceMenuItem";
			item.Click += new EventHandler(WakeUpScreenAfterPowerBreakService_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// Speech Service.
			item = new ToolStripMenuItem();
			item.Text = "Speech Service";
			item.Name = "SpeechServiceMenuItem";
			item.Click += new EventHandler(SpeechService_Click);
			item.Image = Resources.ServiceDisable;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// Speech Show/Hide.
			item = new ToolStripMenuItem();
			item.Text = "Speech Show/Hide";
			item.Name = "SpeechShowHideMenuItem";
			item.Click += new EventHandler(SpeechShowHide_Click);
			item.Image = Resources.Supp;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// Exit.
			item = new ToolStripMenuItem();
			item.Text = "Exit";
			item.Name = "ExitMenuItem";
			item.Click += new System.EventHandler(Exit_Click);
			item.Image = Resources.Exit;
			Common.ContextMenus.Menu.Items.Add(item);

			return Common.ContextMenus.Menu;
		}

		/// <summary>
		/// Handles the Click event of the Explorer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Explorer_Click(object sender, EventArgs e)
		{
			Process.Start("explorer", null);
		}

		/// <summary>
		/// Handles the Click event of the About control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void About_Click(object sender, EventArgs e)
		{
            Utility utility = new Utility();
            if (!utility.IsOpenForm(Application.OpenForms, nameof(AboutBoxFrm)))
            {
                AboutBoxFrm aboutBoxFrm = new AboutBoxFrm(utility.AssemblyTitle, utility.AssemblyProduct, utility.AssemblyVersion, utility.AssemblyCopyright, utility.AssemblyCompany, utility.AssemblyDescription + " " + Hook.methodOfUse, Color.FromArgb(255,60,60,60), Color.FromArgb(255, 245, 40, 40));
                aboutBoxFrm.TopMost = true;
                aboutBoxFrm.Icon = new Icon("Resources/About.ico");
			}
		}

		/// <summary>
		/// Processes a menu item.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Exit_Click(object sender, EventArgs e)
		{
			// Quit without further ado.
			Application.Exit();
		}

		void HookKey_Click(object sender, EventArgs e)
		{
			SetMenuItem("HookKeyMenuItem");
		}

		void NotifyMute_Click(object sender, EventArgs e)
		{
			SetMenuItem("NotifyMuteMenuItem");
		}

		void NotifyPopupShow_Click(object sender, EventArgs e)
		{
			SetMenuItem("NotifyPopupShowMenuItem");
		}

		void QueueService_Click(object sender, EventArgs e)
		{
			SetMenuItem("QueueServiceMenuItem");
		}

		void SongsManager_Click(object sender, EventArgs e)
		{
			SetMenuItem("SongsManagerMenuItem");
		}

		void SyncIpService_Click(object sender, EventArgs e)
		{
			SetMenuItem("SyncIpServiceMenuItem");
		}

		void ReconnectBluetoothDeviceService_Click(object sender, EventArgs e)
		{
			SetMenuItem("ReconnectBluetoothDeviceServiceMenuItem");
		}

		void RenewNotesService_Click(object sender, EventArgs e)
		{
			SetMenuItem("RenewNotesServiceMenuItem");
		}

		void WakeUpScreenAfterPowerBreakService_Click(object sender, EventArgs e)
		{
			SetMenuItem("WakeUpScreenAfterPowerBreakServiceMenuItem");
		}

		void SpeechService_Click(object sender, EventArgs e)
		{
			SetMenuItem("SpeechServiceMenuItem");
		}

		void SpeechShowHide_Click(object sender, EventArgs e)
		{
			SetMenuItem("SpeechShowHideMenuItem");
		}

		public static void SetMenuItem(string itemName)
		{
			var utilty = new Utility();
			string windowCaption;

			var appSettings = ConfigurationManager.AppSettings;

			windowCaption = appSettings["WindowCaption"];

			if (itemName == "HookKeyMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.HookKeyActive == null || (bool)ProcessIcon.HookKeyActive)
				{
					ProcessIcon.HookKeyActive = false;
					AddOrUpdateAppSettings("HookKey", ProcessIcon.HookKeyActive.ToString());
					item.Image = Resources.ServiceDisable;
					Hook.Stop();
				}
				else
				{
					ProcessIcon.HookKeyActive = true;

					AddOrUpdateAppSettings("HookKey", ProcessIcon.HookKeyActive.ToString());
					item.Image = Resources.ServiceActive;
					//HookManager.EnableKeyDownHook();
					Hook.Start();
				}
			}

			if (itemName == "NotifyMuteMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.NotifyMuteActive == null || (bool)ProcessIcon.NotifyMuteActive)
				{
					ProcessIcon.NotifyMuteActive = false;
					AddOrUpdateAppSettings("NotifyMute", ProcessIcon.NotifyMuteActive.ToString());
					item.Image = Resources.SoundNotifyDisable;
				}
				else
				{
					ProcessIcon.NotifyMuteActive = true;

					AddOrUpdateAppSettings("NotifyMute", ProcessIcon.NotifyMuteActive.ToString());
					item.Image = Resources.SoundNotifyActive;
				}
			}

			if (itemName == "NotifyPopupShowMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.NotifyPopupShowActive == null || (bool)ProcessIcon.NotifyPopupShowActive)
				{
					ProcessIcon.NotifyPopupShowActive = false;
					AddOrUpdateAppSettings("NotifyPopupShow", ProcessIcon.NotifyPopupShowActive.ToString());
					item.Image = Resources.NotifyDisable;
				}
				else
				{
					ProcessIcon.NotifyPopupShowActive = true;

					AddOrUpdateAppSettings("NotifyPopupShow", ProcessIcon.NotifyPopupShowActive.ToString());
					item.Image = Resources.NotifyActive;
				}
			}

			if (itemName == "QueueServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.QueueServiceActive == null || (bool)ProcessIcon.QueueServiceActive)
				{
					ProcessIcon.QueueServiceActive = false;

					AddOrUpdateAppSettings("QueueService", ProcessIcon.QueueServiceActive.ToString());

					item.Image = Resources.ServiceDisable;
					if (ProcessIcon._QueueService != null)
					{
						ProcessIcon._QueueService.Stop();
						ProcessIcon._QueueService = null;
					}
				}
				else
				{
					ProcessIcon.QueueServiceActive = true;
					AddOrUpdateAppSettings("QueueService", ProcessIcon.QueueServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					ProcessIcon._QueueService = new ExecutionQueue.QueueService();
					Task.Run(() => ProcessIcon._QueueService.Start());
				}
			}

			if (itemName == "SyncIpServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.SyncIpServiceActive == null || (bool)ProcessIcon.SyncIpServiceActive)
				{
					ProcessIcon.SyncIpServiceActive = false;

					AddOrUpdateAppSettings("SyncIpService", ProcessIcon.SyncIpServiceActive.ToString());

					item.Image = Resources.ServiceDisable;
					if (ProcessIcon._SyncIpService != null)
					{
						ProcessIcon._SyncIpService.Stop();
						ProcessIcon._SyncIpService = null;
					}
				}
				else
				{
					ProcessIcon.SyncIpServiceActive = true;
					AddOrUpdateAppSettings("SyncIpService", ProcessIcon.SyncIpServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					ProcessIcon._SyncIpService = new SyncIp.SyncIpService();
					Task.Run(() => ProcessIcon._SyncIpService.Start());
				}
			}

			if (itemName == "ReconnectBluetoothDeviceServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.ReconnectBluetoothDeviceServiceActive == null || (bool)ProcessIcon.ReconnectBluetoothDeviceServiceActive)
				{
					ProcessIcon.ReconnectBluetoothDeviceServiceActive = false;

					AddOrUpdateAppSettings("ReconnectBluetoothDeviceService", ProcessIcon.ReconnectBluetoothDeviceServiceActive.ToString());

					item.Image = Resources.ServiceDisable;
					if (ProcessIcon._ReconnectBluetoothDeviceService != null)
					{
						ProcessIcon._ReconnectBluetoothDeviceService.Stop();
						ProcessIcon._ReconnectBluetoothDeviceService = null;
					}
				}
				else
				{
					ProcessIcon.ReconnectBluetoothDeviceServiceActive = true;
					AddOrUpdateAppSettings("ReconnectBluetoothDeviceService", ProcessIcon.ReconnectBluetoothDeviceServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					ProcessIcon._ReconnectBluetoothDeviceService = new ReconnectBluetoothDevice.ReconnectBluetoothDeviceService();
					Task.Run(() => ProcessIcon._ReconnectBluetoothDeviceService.Start());
				}
			}

			if (itemName == "RenewNotesServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.RenewNotesServiceActive == null || (bool)ProcessIcon.RenewNotesServiceActive)
				{
					ProcessIcon.RenewNotesServiceActive = false;

					AddOrUpdateAppSettings("RenewNotesService", ProcessIcon.RenewNotesServiceActive.ToString());

					item.Image = Resources.ServiceDisable;
					if (ProcessIcon._RenewNotesService != null)
					{
						ProcessIcon._RenewNotesService.Stop();
						ProcessIcon._RenewNotesService = null;
					}
				}
				else
				{
					ProcessIcon.RenewNotesServiceActive = true;
					AddOrUpdateAppSettings("RenewNotesService", ProcessIcon.RenewNotesServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					ProcessIcon._RenewNotesService = new RenewNotes.RenewNotesService();
					Task.Run(() => ProcessIcon._RenewNotesService.Start());
				}
			}

			if (itemName == "WakeUpScreenAfterPowerBreakServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.WakeUpScreenAfterPowerBreakServiceActive == null || (bool)ProcessIcon.WakeUpScreenAfterPowerBreakServiceActive)
				{
					ProcessIcon.WakeUpScreenAfterPowerBreakServiceActive = false;

					AddOrUpdateAppSettings("WakeUpScreenAfterPowerBreakService", ProcessIcon.WakeUpScreenAfterPowerBreakServiceActive.ToString());

					item.Image = Resources.ServiceDisable;
					if (ProcessIcon._WakeUpScreenAfterPowerBreakService != null)
					{
						ProcessIcon._WakeUpScreenAfterPowerBreakService.Stop();
						ProcessIcon._WakeUpScreenAfterPowerBreakService = null;
					}
				}
				else
				{
					ProcessIcon.WakeUpScreenAfterPowerBreakServiceActive = true;
					AddOrUpdateAppSettings("WakeUpScreenAfterPowerBreakService", ProcessIcon.WakeUpScreenAfterPowerBreakServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					ProcessIcon._WakeUpScreenAfterPowerBreakService = new WakeUpScreenAfterPowerBreak.WakeUpScreenAfterPowerBreakService();
					Task.Run(() => ProcessIcon._WakeUpScreenAfterPowerBreakService.Start());
				}
			}

			if (itemName == "SpeechServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];
				var _item = Common.ContextMenus.Menu.Items["SpeechShowHideMenuItem"];

				if (ProcessIcon.SpeechServiceActive == null || (bool)ProcessIcon.SpeechServiceActive)
				{
					ProcessIcon.SpeechServiceActive = false;
					ProcessIcon.SpeechShowHideActive = 0;

					AddOrUpdateAppSettings("Speech", ProcessIcon.SpeechServiceActive.ToString());

					item.Image = Resources.ServiceDisable;
					_item.Image = Resources.SuppHide;
					if (ProcessIcon._Speech != null)
					{
						//ProcessIcon._SpeechService.Stop();
						//ProcessIcon._SpeechService = null;
						ProcessIcon._Speech.Stop();
						ProcessIcon._Speech = null;
					}
				}
				else
				{
					ProcessIcon.SpeechServiceActive = true;
					ProcessIcon.SpeechShowHideActive = 1;
					AddOrUpdateAppSettings("Speech", ProcessIcon.SpeechServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					_item.Image = Resources.Supp;
					ProcessIcon._Speech = new Speech();
					//ProcessIcon._SpeechService = new SpeechService();
					Task.Run(() => ProcessIcon._Speech.Start());
					//Task.Run(() => ProcessIcon._SpeechService.Start());
				}
			}

			if (itemName == "SpeechShowHideMenuItem" && (bool)ProcessIcon.SpeechServiceActive)
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				var processIsActive = utilty.ProcessIsActiveByWindowCaption(windowCaption);

				if (processIsActive && ProcessIcon.SpeechShowHideActive == -1) ProcessIcon.SpeechShowHideActive = 1;

				if (processIsActive && ProcessIcon.SpeechShowHideActive == 2)
				{
					ProcessIcon.SpeechShowHideActive = 0;
					item.Image = Resources.SuppHide;
					if (ProcessIcon._Speech != null)
					{
						ProcessIcon._Speech.HideAfterShow();
						ProcessIcon._Speech = null;
					}
				}
				else if (processIsActive == false || ProcessIcon.SpeechShowHideActive == 0)
				{
					ProcessIcon.SpeechShowHideActive = 1;
					item.Image = Resources.Supp;
					ProcessIcon._Speech = new Speech();
					Task.Run(() => ProcessIcon._Speech.Show(false));
				}
				else if (processIsActive == true && ProcessIcon.SpeechShowHideActive == 1)
				{
					ProcessIcon.SpeechShowHideActive = 2;
					item.Image = Resources.Supp;
					ProcessIcon._Speech = new Speech();
					Task.Run(() => ProcessIcon._Speech.Show(true));
				}
			}

			if (itemName == "SongsManagerMenuItem")
			{
				if (ProcessIcon.SongsManagerActive != null && Common.ContextMenus.SongsManagerActive == false)
				{
					ProcessIcon.SongsManagerActive = true;
					Common.ContextMenus.SongsManagerActive = true;
					var item = Common.ContextMenus.Menu.Items[itemName];
					item.Image = Resources.SongsManagerActive;
					ProcessIcon._SongsManager = new SongsManager();
					Task.Run(() => ProcessIcon._SongsManager.Start());
				}
				else
					ProcessIcon.SongsManagerActive = false;
			}
		}

		public static void AddOrUpdateAppSettings(string key, string value)
		{
			try
			{
				var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var settings = configFile.AppSettings.Settings;
				if (settings[key] == null)
				{
					settings.Add(key, value);
				}
				else
				{
					settings[key].Value = value;
				}
				configFile.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
			}
			catch (ConfigurationErrorsException)
			{
				Console.WriteLine("Error writing app settings");
			}
		}
	}
}