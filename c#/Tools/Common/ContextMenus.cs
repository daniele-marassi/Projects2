using Common.Properties;
using System.Windows.Forms;

namespace Tools.Common
{
	/// <summary>
	/// 
	/// </summary>
	public class ContextMenus
	{
		public static ContextMenuStrip Menu;

		public static void SetMenuItemWithError(string itemName, int volumeOfNotify, bool notifyMute)
		{
			var item = Menu.Items[itemName];
			item.Image = Resources.ServicesError;

			string applicationPath =
				System.IO.Path.GetDirectoryName(
				   System.Reflection.Assembly.GetEntryAssembly().Location);

			if (!notifyMute) PlayFile(System.IO.Path.Combine(applicationPath, "Resources","Error.mp3").ToString(), volumeOfNotify);
		}

		public static void SetMenuItemRecover(string itemName, int volumeOfNotify, bool notifyMute)
		{
			var item = Menu.Items[itemName];
			item.Image = Resources.ServiceActive;

			string applicationPath =
				System.IO.Path.GetDirectoryName(
				   System.Reflection.Assembly.GetEntryAssembly().Location);

			if(!notifyMute) PlayFile(System.IO.Path.Combine(applicationPath, "Resources", "Recover.mp3").ToString(), volumeOfNotify);
		}

		private static void PlayFile(string url, int volumeOfNotify)
		{
			var player = new WMPLib.WindowsMediaPlayer();
			player.settings.volume = volumeOfNotify;
			player.PlayStateChange += Player_PlayStateChange;
			player.URL = url;
			
			player.controls.play();
		}

		private static void Player_PlayStateChange(int NewState)
		{
			if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
			{
				//Actions on stop
			}
		}
	}
}
