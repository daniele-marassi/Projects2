using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Additional;
using Tools.Properties;
using System.Threading.Tasks;
using System.Configuration;
using Tools.AboutBox;
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
			item.Image = Resources.ServiceActive;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// QueueService.
			item = new ToolStripMenuItem();
			item.Text = "Queue Service";
			item.Name = "QueueServiceMenuItem";
			item.Click += new EventHandler(QueueService_Click);
			item.Image = Resources.ServiceActive;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// SongsManager.
			item = new ToolStripMenuItem();
			item.Text = "Songs Manager";
			item.Name = "SongsManagerMenuItem";
			item.Click += new EventHandler(SongsManager_Click);
			item.Image = Resources.SongsManagerActive;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// SyncIpService.
			item = new ToolStripMenuItem();
			item.Text = "Sync Ip Service";
			item.Name = "SyncIpServiceMenuItem";
			item.Click += new EventHandler(SyncIpService_Click);
			item.Image = Resources.ServiceActive;
			Common.ContextMenus.Menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			Common.ContextMenus.Menu.Items.Add(sep);

			// Speech Service.
			item = new ToolStripMenuItem();
			item.Text = "Speech Service";
			item.Name = "SpeechServiceMenuItem";
			item.Click += new EventHandler(SpeechService_Click);
			item.Image = Resources.ServiceActive;
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

				if (ProcessIcon.HookKeyActive)
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
					Hook.Start();
				}
			}

			if (itemName == "QueueServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];

				if (ProcessIcon.QueueServiceActive)
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

				if (ProcessIcon.SyncIpServiceActive)
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

			if (itemName == "SpeechServiceMenuItem")
			{
				var item = Common.ContextMenus.Menu.Items[itemName];
				var _item = Common.ContextMenus.Menu.Items["SpeechShowHideMenuItem"];

				if (ProcessIcon.SpeechServiceActive)
				{
					ProcessIcon.SpeechServiceActive = false;

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
					AddOrUpdateAppSettings("Speech", ProcessIcon.SpeechServiceActive.ToString());
					item.Image = Resources.ServiceActive;
					_item.Image = Resources.Supp;
					ProcessIcon._Speech = new Speech();
					//ProcessIcon._SpeechService = new SpeechService();
					Task.Run(() => ProcessIcon._Speech.Start());
					//Task.Run(() => ProcessIcon._SpeechService.Start());
				}
			}

			if (itemName == "SpeechShowHideMenuItem" && ProcessIcon.SpeechServiceActive)
			{
				var item = Common.ContextMenus.Menu.Items[itemName];
				if (utilty.ProcessIsActiveByWindowCaption(windowCaption))
				{
					ProcessIcon.SpeechShowHideActive = false;
					item.Image = Resources.SuppHide;
					if (ProcessIcon._Speech != null)
					{
						ProcessIcon._Speech.Hide();
						ProcessIcon._Speech = null;
					}
				}
				else
				{
					ProcessIcon.SpeechShowHideActive = true;
					item.Image = Resources.Supp;
					ProcessIcon._Speech = new Speech();
					Task.Run(() => ProcessIcon._Speech.Show());
				}
			}

			if (itemName == "SongsManagerMenuItem" && ProcessIcon.SongsManagerActive)
			{
				ProcessIcon._SongsManager = new SongsManager();
				Task.Run(() => ProcessIcon._SongsManager.Start());
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