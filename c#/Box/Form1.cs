using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Threading;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Globalization;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Box
{
    public partial class Form1 : Form
    {

        SpeechSynthesizer voice = null;
        int _processId = 0;

        // Define the FindWindow API function.
        [DllImport("user32.dll", EntryPoint = "FindWindow",
            SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly,
            string lpWindowName);

        // Define the SetWindowPos API function.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            SetWindowPosFlags uFlags);

        // Define the SetWindowPosFlags enumeration.
        [Flags()]
        private enum SetWindowPosFlags : uint
        {
            SynchronousWindowPosition = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = 0x0020,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            DoNotActivate = 0x0010,
            DoNotCopyBits = 0x0100,
            IgnoreMove = 0x0002,
            DoNotChangeOwnerZOrder = 0x0200,
            DoNotRedraw = 0x0008,
            DoNotReposition = 0x0200,
            DoNotSendChangingEvent = 0x0400,
            IgnoreResize = 0x0001,
            IgnoreZOrder = 0x0004,
            ShowWindow = 0x0040,
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Speak_Click(object sender, EventArgs e)
        {
            voice.SelectVoiceByHints(VoiceGender.Female);
            voice.SpeakAsync(txtvoice.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            voice = new SpeechSynthesizer();
        }

        public (int? ProcessId, string Error) RunUS(string path, string filename, string arguments, string domain, string userName, string password, bool hidden = false, bool loadUserProfile = false, bool waitForExit = false) 
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            (int? ProcessId, string Error) output;
            output.ProcessId = null;
            output.Error = null;

            System.Security.SecureString ssPwd = new System.Security.SecureString();
            startInfo.UseShellExecute = false;
            startInfo.FileName = Path.Combine(path, filename);
            
            startInfo.WorkingDirectory = path;
            startInfo.Arguments = arguments;
            startInfo.Domain = domain;
            startInfo.UserName = userName;

            for (int x = 0; x < password.Length; x++)
            {
                ssPwd.AppendChar(password[x]);
            }
            startInfo.Password = ssPwd;
            startInfo.RedirectStandardError = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.LoadUserProfile = loadUserProfile;
            startInfo.CreateNoWindow = true;

            if (hidden)
            {
                Process processTemp = new Process();
                processTemp.StartInfo = startInfo;
                processTemp.EnableRaisingEvents = true;
                processTemp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                try
                {
                    processTemp.Start();
                    output.ProcessId = processTemp.Id;
                    if(waitForExit) processTemp.WaitForExit();
                }
                catch (Exception ex)
                {
                    output.Error = ex.Message;
                }
            }
            else
            {
                Process processTemp = new Process();
                processTemp.StartInfo = startInfo;
                processTemp.Start();
                output.ProcessId = processTemp.Id;
                if (waitForExit) processTemp.WaitForExit();
            }

            return output;
        }

        public bool KillProcessById(int processId)
        {
            var result = false;
            try
            {
                Process proc =  Process.GetProcessById(processId);
                proc.Kill();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public bool KillProcessByWindowCaption(string windowCaption)
        {
            var result = false;
            try
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process proc in processes)
                {
                    if (proc.MainWindowTitle.ToLower().Trim().Contains(windowCaption.ToLower().Trim()))
                        proc.Kill();
                }

                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public void MoveExtWindow(string windowCaption, int x, int y, int width, int height)
        {
            IntPtr mainWindowHandle =
              FindWindowByCaption(IntPtr.Zero, windowCaption);

            SetWindowPos(mainWindowHandle, IntPtr.Zero,
                x, y, width, height, 0);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            KillProcessById(_processId);

            

            var result = RunUS(@"C:\Program Files (x86)\Google\Chrome\Application", @"chrome.exe", "--chrome-frame  --window-size=100,100 --window-position=1000,1000 --app=https://192.168.1.105:83/WebSpeeches/Recognition", "ev-pc", "Admin", "Enilno.00", true, true, false);
            _processId = (int)result.ProcessId;

            KillProcessByWindowCaption("Web Speech Recognition");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MoveExtWindow("Web Speech Recognition", 300,300,600,600);
        }
    }
}
