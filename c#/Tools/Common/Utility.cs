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

            var result = RunExe(Path.Combine(rootPath, "External_tools", "MessagesPopUp.exe"), _json, true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// RunExeAsync
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static async Task<(string Output, string Error)> RunExe(string fullPath, string arguments, bool async)
        {
            (string Output, string Error) result;
            result.Output = null;
            result.Error = null;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = fullPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = async;
                process.StartInfo.RedirectStandardError = async;
                process.Start();

                if (async)
                {
                    result.Output = process.StandardOutput.ReadToEnd();

                    result.Error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            finally
            {
                if (result.Output != null && result.Output != String.Empty) Console.WriteLine("Output: " + result.Output);
                if (result.Error != null && result.Error != String.Empty) Console.WriteLine("Error: " + result.Error);
            }

            return result;
        }

        public void ClickOnTaskbar()
        {
            var workingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;

            Cursor.Position = new Point(workingAreaWidth - 520, workingAreaHeight + 20);

            System.Threading.Thread.Sleep(100);

            VirtualMouse.LeftClick();
        }
    }
}
