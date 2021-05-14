using System.Windows.Forms;
using Tools.Common.Properties;

namespace Tools.Common
{
	/// <summary>
	/// 
	/// </summary>
	public class ContextMenus
	{
		public static ContextMenuStrip Menu;

		public static void SetMenuItemWithError(string itemName)
		{
			var item = Menu.Items[itemName];
			item.Image = Resources.ServicesError;
		}

		public static void SetMenuItemRecover(string itemName)
		{
			var item = Menu.Items[itemName];
			item.Image = Resources.ServiceActive;
		}
	}
}
