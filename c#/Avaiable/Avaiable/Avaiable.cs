using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;

namespace Avaiable
{
    public partial class Avaiable : Form
    {
        static Random random = new Random();

        DateTime startTime = DateTime.Now;
        DateTime closeTime = DateTime.Now;
        DateTime startTime1 = DateTime.Now;
        DateTime closeTime1 = DateTime.Now;
        DateTime startTime2 = DateTime.Now;
        DateTime closeTime2 = DateTime.Now;
        DateTime startTime3 = DateTime.Now;
        DateTime closeTime3 = DateTime.Now;

        public Avaiable()
        {
            InitializeComponent();
        }

        public int RandomNumber(int min, int max)
        {
            var result = random.Next(min, max);
            return result;
        }

        private void Init()
        {
            startTime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + StartTxt.Text);
            closeTime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + CloseTxt.Text);
            startTime1 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + StartBreak1Txt.Text);
            closeTime1 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + CloseBreak1Txt.Text);
            startTime2 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + StartBreak2Txt.Text);
            closeTime2 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + CloseBreak2Txt.Text);
            startTime3 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + StartBreak3Txt.Text);
            closeTime3 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + CloseBreak3Txt.Text);

            startTime = startTime.AddMinutes((double)RandomNumber(0, 10));
            closeTime = closeTime.AddMinutes((double)RandomNumber(0, 10));
            startTime1 = startTime1.AddMinutes((double)RandomNumber(0, 10));
            closeTime1 = closeTime1.AddMinutes((double)RandomNumber(0, 10));
            startTime2 = startTime2.AddMinutes((double)RandomNumber(0, 10));
            closeTime2 = closeTime2.AddMinutes((double)RandomNumber(0, 10));
            startTime3 = startTime3.AddMinutes((double)RandomNumber(0, 10));
            closeTime3 = closeTime3.AddMinutes((double)RandomNumber(0, 10));
        }

        private void AvaiableTmr_Tick(object sender, EventArgs e)
        {
            try
            {
                var _char = (char)random.Next(97, 122);

                if (DateTime.Now >= closeTime)
                {
                    //MessageBox.Show("ok");
                    if (radioButton2.Checked)
                        System.Windows.Forms.Application.Exit();

                    if (radioButton3.Checked)
                        Process.Start("shutdown", "/s /t 0");
                    //DoExitWin(EWX_SHUTDOWN);
                    Init();
                }
                else if ((DateTime.Now >= startTime1 && DateTime.Now < closeTime1) || (DateTime.Now >= startTime2 && DateTime.Now < closeTime2) || (DateTime.Now >= startTime3 && DateTime.Now < closeTime3))
                {
                    TextBox.Text = "";
                }
                else if (DateTime.Now >= startTime) 
                {
                    TextBox.Text = "";
                    if (TextBox.Focused)
                    {
                        SendKeys.Send(_char.ToString());

                        Keys key = (Keys)Enum.Parse(typeof(Keys), _char.ToString(), true);

                        keybd_event((byte)key, (byte)0x02, (int)WM_KEYDOWN, 0);

                        var hWnd = new IntPtr();

                        sendKey(hWnd, key);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const int WM_SYSKEYDOWN = 0x0104;
        const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        public static void sendKey(IntPtr hwnd, Keys keyCode)
        {
            uint scanCode = MapVirtualKey((uint)keyCode, 0);
            uint lParam;
            // 
            //KEY DOWN
            lParam = (0x00000001 | (scanCode << 16));
            //if (extended)
            //{
            //    lParam |= 0x01000000;
            //}
            PostMessage(hwnd, (UInt32)WM_KEYDOWN, (IntPtr)keyCode, (IntPtr)lParam);
            Thread.Sleep(100);

            uint scanCode1 = MapVirtualKey((uint)Keys.D1, 0);
            uint lParam1;
            // 
            //KEY DOWN
            lParam1 = (0x00000001 | (scanCode1 << 16));

            PostMessage(hwnd, (UInt32)WM_KEYDOWN, (IntPtr)Keys.D1, (IntPtr)lParam1);

            Thread.Sleep(50);
            //KEY UP
            //lParam |= 0xC0000000;  // set previous key and transition states (bits 30 and 31)
            PostMessage(hwnd, WM_KEYUP, (IntPtr)Keys.D1, (IntPtr)(0xC0020001));
            PostMessage(hwnd, WM_KEYUP, (IntPtr)keyCode, (IntPtr)(0xC0390001));
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr
        phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name,
        ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int flg, int rea);

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

        private void DoExitWin(int flg)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(flg, 0);
        }

        private void Avaiable_Load(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            //string closingTime = appSettings["ClosingTime"] ?? "";
            //CloseTxt.Text = closingTime;
            var init = random.Next(5, 100);
            Init();
        }

        private void StartBreak2Txt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void StartTxt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void StartBreak1Txt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void CloseBreak1Txt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void CloseBreak2Txt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void StartBreak3Txt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void CloseBreak3Txt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void CloseTxt_TextChanged(object sender, EventArgs e)
        {
            Init();
        }
    }
}
