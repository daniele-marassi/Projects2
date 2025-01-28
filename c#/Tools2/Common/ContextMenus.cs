using Common.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Tools.Common
{
	/// <summary>
	/// 
	/// </summary>
	public class ContextMenus
	{
		public static ContextMenuStrip Menu;

		public static bool SongsManagerActive;

		public enum ResourcesType
		{
			ServicesError,
			ServiceActive,
			SongsManagerDisable,
			SongsManagerActive,
			SongsManagerError
		}

		public static void SetMenuItemWithError(string itemName, int volumeOfNotify, bool notifyMute, ResourcesType resourcesType)
		{
			var item = Menu.Items[itemName];
			if(resourcesType == ResourcesType.ServicesError) item.Image = Resources.ServicesError;
			if (resourcesType == ResourcesType.SongsManagerError) item.Image = Resources.SongsManagerError;

			string applicationPath =
				System.IO.Path.GetDirectoryName(
				   System.Reflection.Assembly.GetEntryAssembly().Location);

			if (!notifyMute) PlayFile(System.IO.Path.Combine(applicationPath, "Resources","Error.mp3").ToString(), volumeOfNotify);
		}

		public static void SetMenuItemRecover(string itemName, int volumeOfNotify, bool notifyMute, ResourcesType resourcesType)
		{
			var item = Menu.Items[itemName];
			if (resourcesType == ResourcesType.ServiceActive)  item.Image = Resources.ServiceActive;
			if (resourcesType == ResourcesType.SongsManagerDisable) item.Image = Resources.SongsManagerDisable;

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
