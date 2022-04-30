using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using NLog.Targets;
using NLog;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Drawing;
using Additional;

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
    }
}
