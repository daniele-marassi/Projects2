using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Drawing;
using Additional;
using System;
using System.Configuration;

namespace Tools.Common
{
    public class Utility
    {
        /// <summary>
        /// ShowMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="timeToClosePopUpInMilliseconds"></param>
        /// <param name="rootPath"></param>
        public static void ShowMessage(string message, MessagesPopUp.MessageType type, int timeToClosePopUpInMilliseconds, string rootPath)
        {
            
            var par = new MessagesPopUp.Argument()
            {
                Title = "Tools - " + type.ToString(),
                Message = message,
                Type = type.ToString(),
                TimeToClosePopUpInMilliseconds = timeToClosePopUpInMilliseconds
            };

            var _json = new JavaScriptSerializer().Serialize(par);

            _json = _json.Replace(@"""", @"@DOUBLEQUOTES@");
            _json = _json.Replace(@" ", @"@SPACE@");
            var utility = new Additional.Utility();

            var result = utility.RunExe(Path.Combine(rootPath, "External_tools", "MessagesPopUp.exe"), _json, false).GetAwaiter().GetResult();
        }

        public void ClickOnTaskbar()
        {
            var workingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;

            Cursor.Position = new Point(workingAreaWidth - 520, workingAreaHeight + 20);

            Cursor.Position = new Point(workingAreaWidth - 520, workingAreaHeight + 20);

            VirtualMouse.LeftClick();

            System.Threading.Thread.Sleep(1000);

            VirtualMouse.LeftClick();
        }

        public void SetVolume(double percent)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var volumeAppPath = appSettings["VolumeAppPath"];

            var utility = new Additional.Utility();

            utility.RunExe(volumeAppPath, percent.ToString(), false);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
