using Tools.AboutBox;
using Additional;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools.AboutBox;

namespace Shut
{
    public partial class Shut : Form
    {
        public static PrivateFontCollection Fonts { get; set; } = null;
        public static int stepOpenClose = 0;

        public static float amountBrightnessShutDown = 0f;
        public static float amountBrightnessExit = 0;
        public static float amountBrightnessLock = 0;
        public static float amountBrightnessRestart = 0;
        public static float amountBrightnessInfo = 0;
        public static float amountBrightnessHibernate = 0;
        public static float amountBrightnessLogOff = 0;

        public static bool increaseBrightnessShutDown = true;
        public static bool increaseBrightnessExit = true;
        public static bool increaseBrightnessLock = true;
        public static bool increaseBrightnessRestart = true;
        public static bool increaseBrightnessInfo = true;
        public static bool increaseBrightnessHibernate = true;
        public static bool increaseBrightnessLogOff = true;

        public static float amountFontSizeShutDown = 0;
        public static float amountFontSizeExit = 0;
        public static float amountFontSizeLock = 0;
        public static float amountFontSizeRestart = 0;
        public static float amountFontSizeInfo = 0;
        public static float amountFontSizeHibernate = 0;
        public static float amountFontSizeLogOff = 0;

        public static Dictionary<string, float> fontsSizes = new Dictionary<string, float>();

        public static Dictionary<string, Point> elementsLocacations = new Dictionary<string, Point>();

        public static int colorAndZoomTmrInterval = 20;

        public static string style = String.Empty;

        public static Dictionary<string, bool> elementsColorAndZoom = new Dictionary<string, bool>();

        public static Dictionary<string, bool> elementsInMouseHover = new Dictionary<string, bool>();

        public static Dictionary<string, bool> elementsInMouseDown = new Dictionary<string, bool>();

        private static Cursor cursorCurrent = null;

        private static Color color = default(Color);

        private static Color colorOnSelection = default(Color);

        private static Color colorOnClickDown = default(Color);



        private static RequestType request;

        private static Utility utility = new Utility();

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("user32")]
        public static extern void LockWorkStation();

        private ShutBack shutBack = null;

        private static bool exit = false;

        public Shut(string param)
        {
            if (utility.ProgramRunningCount(nameof(Shut)) > 1)
                exit = true;

            InitializeComponent();

            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;
            this.Hide();
           
            Inizialize();
            LoadFonts();
            SetFonts();

            this.Icon = new Icon("Resources/Shut.ico");
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); 
            style = config.AppSettings.Settings["Style"].Value;
            StartHook();

            if (param.ToLower() == "ShutDown".ToLower()) ShutDown_Click();
            if (param.ToLower() == "Exit".ToLower()) Exit_Click();
            if (param.ToLower() == "Lock".ToLower()) Lock_Click();
            if (param.ToLower() == "Restart".ToLower()) Restart_Click();
            if (param.ToLower() == "Info".ToLower()) Info_Click();
            if (param.ToLower() == "Hibernate".ToLower()) Hibernate_Click();
            if (param.ToLower() == "LogOff".ToLower()) LogOff_Click();
        }

        private void ForceExit()
        {
            var proc = Process.GetProcessesByName(nameof(Shut)).OrderBy(_ => _.StartTime).FirstOrDefault();
            proc.Kill();
            //Environment.Exit(0);
            //return;
            Exit_Click();
        }

        private void StartHook()
        {
            //HookManager.KeyDown += HookManager_KeyDown;
            //HookManager.KeyUp += HookManager_KeyUp;
            HookManager.MouseDown += HookManager_MouseDown;
            HookManager.MouseUp += HookManager_MouseUp;
        }

        private void StopHook()
        {
            //HookManager.KeyDown -= HookManager_KeyDown;
            //HookManager.KeyUp -= HookManager_KeyUp;
            HookManager.MouseDown -= HookManager_MouseDown;
            HookManager.MouseUp -= HookManager_MouseUp;
        }

        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            StopHook();
            
            //Console.WriteLine(string.Format("MouseUp - {0}\n", keyCode));

            //utility.CallMethodByName("NAMESPACE.CLASS", "METHOD", keyCode);

            if (elementsInMouseHover["ShutDown"] == true) ShutDown_Click();
            if (elementsInMouseHover["Exit"] == true) Exit_Click();
            if (elementsInMouseHover["Lock"] == true) Lock_Click();
            if (elementsInMouseHover["Restart"] == true) Restart_Click();
            if (elementsInMouseHover["Info"] == true) Info_Click();
            if (elementsInMouseHover["Hibernate"] == true) Hibernate_Click();
            if (elementsInMouseHover["LogOff"] == true) LogOff_Click();

            if (elementsInMouseHover["ShutDown"] == true)
            {
                this.ShutDown.Font = new Font(this.ShutDown.Font.FontFamily, fontsSizes["ShutDown"] - 0, FontStyle.Bold);
                this.ShutDown.ForeColor = colorOnSelection;
                this.ShutDown.Location = elementsLocacations["ShutDown"];
            }
            if (elementsInMouseHover["Exit"] == true)
            {
                this.Exit.Font = new Font(this.Exit.Font.FontFamily, fontsSizes["Exit"] - 0, FontStyle.Bold);
                this.Exit.ForeColor = colorOnSelection;
                this.Exit.Location = elementsLocacations["Exit"];
            }
            if (elementsInMouseHover["Lock"] == true)
            {
                this.Lock.Font = new Font(this.Lock.Font.FontFamily, fontsSizes["Lock"] - 0, FontStyle.Bold);
                this.Lock.ForeColor = colorOnSelection;
                this.Lock.Location = elementsLocacations["Lock"];
            }
            if (elementsInMouseHover["Restart"] == true)
            {
                this.Restart.Font = new Font(this.Restart.Font.FontFamily, fontsSizes["Restart"] - 0, FontStyle.Bold);
                this.Restart.ForeColor = colorOnSelection;
                this.Restart.Location = elementsLocacations["Restart"];
            }
            if (elementsInMouseHover["Info"] == true)
            {
                this.Info.Font = new Font(this.Info.Font.FontFamily, fontsSizes["Info"] - 0, FontStyle.Bold);
                this.Info.ForeColor = colorOnSelection;
                this.Info.Location = elementsLocacations["Info"];
            }
            if (elementsInMouseHover["Hibernate"] == true)
            {
                this.Hibernate.Font = new Font(this.Hibernate.Font.FontFamily, fontsSizes["Hibernate"] - 0, FontStyle.Bold);
                this.Hibernate.ForeColor = colorOnSelection;
                this.Hibernate.Location = elementsLocacations["Hibernate"];
            }
            if (elementsInMouseHover["LogOff"] == true)
            {
                this.LogOff.Font = new Font(this.LogOff.Font.FontFamily, fontsSizes["LogOff"] - 0, FontStyle.Bold);
                this.LogOff.ForeColor = colorOnSelection;
                this.LogOff.Location = elementsLocacations["LogOff"];
            }

            StartHook();
        }

        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            StopHook();

            //Console.WriteLine(string.Format("MouseDown - {0}\n", keyCode));

            //utility.CallMethodByName("NAMESPACE.CLASS", "METHOD", keyCode);

            if (elementsInMouseHover["ShutDown"] == true)
            {
                this.ShutDown.Font = new Font(this.ShutDown.Font.FontFamily, fontsSizes["ShutDown"] - 10, FontStyle.Bold);
                this.ShutDown.ForeColor = colorOnClickDown;
                this.ShutDown.Location = new Point(elementsLocacations["ShutDown"].X + 10, elementsLocacations["ShutDown"].Y + 7);
            }
            if (elementsInMouseHover["Exit"] == true)
            {
                this.Exit.Font = new Font(this.Exit.Font.FontFamily, fontsSizes["Exit"] - 10, FontStyle.Bold);
                this.Exit.ForeColor = colorOnClickDown;
                this.Exit.Location = new Point(elementsLocacations["Exit"].X + 10, elementsLocacations["Exit"].Y + 7);
            }
            if (elementsInMouseHover["Lock"] == true)
            {
                this.Lock.Font = new Font(this.Lock.Font.FontFamily, fontsSizes["Lock"] - 10, FontStyle.Bold);
                this.Lock.ForeColor = colorOnClickDown;
                this.Lock.Location = new Point(elementsLocacations["Lock"].X + 10, elementsLocacations["Lock"].Y + 7);
            }
            if (elementsInMouseHover["Restart"] == true)
            {
                this.Restart.Font = new Font(this.Restart.Font.FontFamily, fontsSizes["Restart"] - 10, FontStyle.Bold);
                this.Restart.ForeColor = colorOnClickDown;
                this.Restart.Location = new Point(elementsLocacations["Restart"].X + 10, elementsLocacations["Restart"].Y + 7);
            }
            if (elementsInMouseHover["Info"] == true)
            {
                this.Info.Font = new Font(this.Info.Font.FontFamily, fontsSizes["Info"] - 10, FontStyle.Bold);
                this.Info.ForeColor = colorOnClickDown;
                this.Info.Location = new Point(elementsLocacations["Info"].X + 10, elementsLocacations["Info"].Y + 7);
            }
            if (elementsInMouseHover["Hibernate"] == true)
            {
                this.Hibernate.Font = new Font(this.Hibernate.Font.FontFamily, fontsSizes["Hibernate"] - 10, FontStyle.Bold);
                this.Hibernate.ForeColor = colorOnClickDown;
                this.Hibernate.Location = new Point(elementsLocacations["Hibernate"].X + 10, elementsLocacations["Hibernate"].Y + 7);
            }
            if (elementsInMouseHover["LogOff"] == true)
            {
                this.LogOff.Font = new Font(this.LogOff.Font.FontFamily, fontsSizes["LogOff"] - 10, FontStyle.Bold);
                this.LogOff.ForeColor = colorOnClickDown;
                this.LogOff.Location = new Point(elementsLocacations["LogOff"].X + 10, elementsLocacations["LogOff"].Y + 7);
            }

            StartHook();
        }

        private void HookManager_KeyUp(object sender, EventArgs e)
        {
            StopHook();

            Type type = e.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            string keyCode = String.Empty;
            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                if (propName == "KeyCode")
                {
                    object propValue = prop.GetValue(e, null);
                    keyCode = propValue.ToString();
                    break;
                }
            }

            //Console.WriteLine(string.Format("KeyDown - {0}\n", keyCode));

            if (keyCode == "KEY")
            {
                //utility.CallMethodByName("NAMESPACE.CLASS", "METHOD", keyCode);
            }

            StartHook();
        }


        private void HookManager_KeyDown(object sender, EventArgs e)
        {
            StopHook();

            Type type = e.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            string keyCode = String.Empty;
            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                if (propName == "KeyCode")
                {
                    object propValue = prop.GetValue(e, null);
                    keyCode = propValue.ToString();
                    break;
                }
            }

            //Console.WriteLine(string.Format("KeyDown - {0}\n", keyCode));

            if (keyCode == "KEY")
            {
                //utility.CallMethodByName("NAMESPACE.CLASS", "METHOD", keyCode);
            }

            StartHook();
        }

        private enum RequestType
        {
            ShutDown = 0,
            Exit = 1,
            Lock = 2,
            Restart = 3,
            Info = 4,
            Hibernate = 5,
            LogOff = 6
        }

        private void Inizialize()
        {
            StopColorTimer();

            elementsLocacations["ShutDown"] = this.ShutDown.Location;
            elementsLocacations["Exit"] = this.Exit.Location;
            elementsLocacations["Lock"] = this.Lock.Location;
            elementsLocacations["Restart"] = this.Restart.Location;
            elementsLocacations["Info"] = this.Info.Location;
            elementsLocacations["Hibernate"] = this.Hibernate.Location;
            elementsLocacations["LogOff"] = this.LogOff.Location;

            this.ShutDown.Location = new Point(-446, 538);
            this.Exit.Location = new Point(-446, 538);
            this.Lock.Location = new Point(-446, 538);
            this.Restart.Location = new Point(-446, 538);
            this.Info.Location = new Point(-446, 538);
            this.Hibernate.Location = new Point(-446, 538);
            this.LogOff.Location = new Point(-446, 538);

            elementsColorAndZoom["ShutDown"] = false;
            elementsColorAndZoom["Exit"] = false;
            elementsColorAndZoom["Lock"] = false;
            elementsColorAndZoom["Restart"] = false;
            elementsColorAndZoom["Info"] = false;
            elementsColorAndZoom["Hibernate"] = false;
            elementsColorAndZoom["LogOff"] = false;

            elementsInMouseHover["ShutDown"] = false;
            elementsInMouseHover["Exit"] = false;
            elementsInMouseHover["Lock"] = false;
            elementsInMouseHover["Restart"] = false;
            elementsInMouseHover["Info"] = false;
            elementsInMouseHover["Hibernate"] = false;
            elementsInMouseHover["LogOff"] = false;

            elementsInMouseDown["ShutDown"] = false;
            elementsInMouseDown["Exit"] = false;
            elementsInMouseDown["Lock"] = false;
            elementsInMouseDown["Restart"] = false;
            elementsInMouseDown["Info"] = false;
            elementsInMouseDown["Hibernate"] = false;
            elementsInMouseDown["LogOff"] = false;

            SetColor();
        }

        private void SetColor()
        {
            color = Color.DarkGray;

            try
            {
                
                int autoColorization = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoColorization", null);

                int colorizationIntReg = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", "StartColorMenu", null);
                //int colorizationIntReg = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "AccentColor", null);
                string colorizationValueReg = String.Empty;

                colorizationValueReg += colorizationIntReg.ToString("X").Substring(6, 2);
                colorizationValueReg += colorizationIntReg.ToString("X").Substring(4, 2);
                colorizationValueReg += colorizationIntReg.ToString("X").Substring(2, 2);

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string colorizationValueConf = config.AppSettings.Settings["colorizationValueConf"].Value;

                config.AppSettings.Settings["colorizationValueConf"].Value = colorizationValueReg;
                config.Save(ConfigurationSaveMode.Modified);

                if (!exit)
                    color = utility.GenerateRgba(colorizationValueReg);
                else
                    color = utility.GenerateRgba(colorizationValueConf);

                if (autoColorization == 1) color = ControlPaint.Light(color, 0.7f);

            }
            catch (Exception ex)
            {}

            //color = Color.FromArgb(255, 225, 0, 0);/////////////////////////////////////

            if ((color.R + color.G + color.B) <= (255 * 3) - 100)
            {
                colorOnSelection = ControlPaint.Light(color, 1.0f);
                colorOnClickDown = ControlPaint.Light(color, 0.5f);
            }
            else
            {
                colorOnSelection = ControlPaint.Dark(color, 1.0f);
                colorOnClickDown = ControlPaint.Dark(color, 0.5f);
            }

            if (shutBack != null)
                shutBack.SetColor(color);;

            this.ShutDown.ForeColor = color;
            this.Exit.ForeColor = color;
            this.Lock.ForeColor = color;
            this.Restart.ForeColor = color;
            this.Info.ForeColor = color;
            this.Hibernate.ForeColor = color;
            this.LogOff.ForeColor = color;
        }

        private void LoadFonts()
        {
            try
            {
                Fonts = new PrivateFontCollection();

                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\breezi_font-webfont.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\WEBDINGS.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Byom-Icons-Trial.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\WINGDING.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\multimedia icons.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\WINGDNG2.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\WINGDNG3.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Icons South St.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\CELLPIC.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Cafed_v2.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Mad Bubbles.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\YouMurderer BB.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\SPLAT.ttf");
                Fonts.AddFontFile($"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Dog-Rough.ttf");
            }
            catch (Exception ex)
            {
            }
        }

        private void SetFonts()
        {
            try
            {
                this.ShutDown.Font = utility.GetFonts("multimedia icons", 85F, Fonts);
                this.ShutDown.Font = new Font(this.ShutDown.Font, FontStyle.Bold);

                this.Exit.Font = utility.GetFonts("Webdings", 200F, Fonts);
                this.Exit.Font = new Font(this.Exit.Font, FontStyle.Regular);

                this.Lock.Font = utility.GetFonts("multimedia icons", 72F, Fonts);
                this.Lock.Font = new Font(this.Lock.Font, FontStyle.Bold);

                this.Restart.Font = utility.GetFonts("Wingdings 3", 80F, Fonts);
                this.Restart.Font = new Font(this.Restart.Font, FontStyle.Bold);

                this.Info.Font = utility.GetFonts("Byom Icons", 28F, Fonts);

                this.Hibernate.Font = utility.GetFonts("cellpic", 100F, Fonts);
                this.Hibernate.Font = new Font(this.Hibernate.Font, FontStyle.Regular);

                this.LogOff.Font = utility.GetFonts("Wingdings 3", 80F, Fonts);
                this.LogOff.Font = new Font(this.LogOff.Font, FontStyle.Bold);
            }
            catch (Exception ex)
            {
            }

            fontsSizes["ShutDown"] = this.ShutDown.Font.Size;
            fontsSizes["Exit"] = this.Exit.Font.Size;
            fontsSizes["Lock"] = this.Lock.Font.Size;
            fontsSizes["Restart"] = this.Restart.Font.Size;
            fontsSizes["Info"] = this.Info.Font.Size;
            fontsSizes["Hibernate"] = this.Hibernate.Font.Size;
            fontsSizes["LogOff"] = this.LogOff.Font.Size;
        }

        private void Shut_Load(object sender, EventArgs e)
        {

            shutBack = new ShutBack();
            
            shutBack.Show();
            shutBack.SetToolTip(String.Empty);
            shutBack.Left = 0;
            shutBack.Top = Screen.PrimaryScreen.WorkingArea.Height - (shutBack.Height + 0);

            this.Left = 0;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            OpenTmr.Enabled = true;
        }

        private void StartColorAndZoomTimer()
        {
            amountBrightnessShutDown = 0;
            amountBrightnessExit = 0;
            amountBrightnessLock = 0;
            amountBrightnessRestart = 0;
            amountBrightnessInfo = 0;
            amountBrightnessHibernate = 0;
            amountBrightnessLogOff = 0;

            amountFontSizeShutDown = 0;
            amountFontSizeExit = 0;
            amountFontSizeLock = 0;
            amountFontSizeRestart = 0;
            amountFontSizeInfo = 0;
            amountFontSizeHibernate = 0;
            amountFontSizeLogOff = 0;

            increaseBrightnessShutDown = true;
            increaseBrightnessExit = true;
            increaseBrightnessLock = true;
            increaseBrightnessRestart = true;
            increaseBrightnessInfo = true;
            increaseBrightnessHibernate = true;
            increaseBrightnessLogOff = true;

            StopColorTimer();

            ColorAndZoomShutDownTmr.Enabled = true;
            ColorAndZoomRestartTmr.Enabled = true;
            ColorAndZoomHibernateTmr.Enabled = true;
            ColorAndZoomLockTmr.Enabled = true;
            ColorAndZoomLogOffTmr.Enabled = true;
            ColorAndZoomInfoTmr.Enabled = true;
            ColorAndZoomExitTmr.Enabled = true;

            int intv = 0;

            intv = 400;
            ColorAndZoomShutDownTmr.Interval = intv;
            intv = 600;
            ColorAndZoomRestartTmr.Interval = intv;
            intv = 800;
            ColorAndZoomHibernateTmr.Interval = intv;
            intv = 1000;
            ColorAndZoomLockTmr.Interval = intv;
            intv = 1200;
            ColorAndZoomLogOffTmr.Interval = intv;
            intv = 1400;
            ColorAndZoomInfoTmr.Interval = intv;
            intv = 1600;
            ColorAndZoomExitTmr.Interval = intv;
        }

        private void StopColorTimer()
        {
            ColorAndZoomShutDownTmr.Enabled = false;
            ColorAndZoomRestartTmr.Enabled = false;
            ColorAndZoomHibernateTmr.Enabled = false;
            ColorAndZoomLockTmr.Enabled = false;
            ColorAndZoomLogOffTmr.Enabled = false;
            ColorAndZoomInfoTmr.Enabled = false;
            ColorAndZoomExitTmr.Enabled = false;
        }

        private void ShutDown_Click()
        {
            request = RequestType.ShutDown;
            CloseTmr.Enabled = true;
        }

        private void Restart_Click()
        {
            request = RequestType.Restart;
            CloseTmr.Enabled = true;
        }

        private void Exit_Click()
        {
            request = RequestType.Exit;
            CloseTmr.Enabled = true;
        }

        private void Info_Click()
        {
            if (!utility.IsOpenForm(Application.OpenForms, nameof(AboutBoxFrm)))
            {
                request = RequestType.Info;

                AboutBoxFrm aboutBoxFrm = new AboutBoxFrm(utility.AssemblyTitle, utility.AssemblyProduct, utility.AssemblyVersion, utility.AssemblyCopyright, utility.AssemblyCompany, utility.AssemblyDescription, Color.FromArgb(255, 60, 60, 60), Color.FromArgb(255, 245, 40, 40));
                aboutBoxFrm.TopMost = true;
                aboutBoxFrm.Icon = new Icon("Resources/About.ico");
            }
        }

        private void Hibernate_Click()
        {
            request = RequestType.Hibernate;
            CloseTmr.Enabled = true;
        }

        private void Lock_Click()
        {
            request = RequestType.Lock;
            CloseTmr.Enabled = true;
        }

        private void LogOff_Click()
        {
            request = RequestType.LogOff;
            CloseTmr.Enabled = true;
        }

        private void ExecuteRequest()
        {
            if (request == RequestType.LogOff) ExitWindowsEx(0, 0);
            if (request == RequestType.Lock) LockWorkStation();
            if (request == RequestType.ShutDown) Process.Start("shutdown", "/s /t 0");
            if (request == RequestType.Restart) Process.Start("shutdown", "/r /t 0");
            if (request == RequestType.Hibernate) Application.SetSuspendState(PowerState.Hibernate, true, true);
        }

        private void OpenTmr_Tick(object sender, EventArgs e)
        {
            SetColor();
            stepOpenClose++;

            if (exit) stepOpenClose = 100;

            if (stepOpenClose == 1) shutBack.OpenClose(1, 1); 
            if (stepOpenClose == 2) shutBack.OpenClose(2,2);
            if (stepOpenClose == 3) shutBack.OpenClose(3,3);
            if (stepOpenClose == 4) shutBack.OpenClose(6,4);
            if (stepOpenClose == 5) shutBack.OpenClose(9,5);
            if (stepOpenClose == 6) shutBack.OpenClose(12,6);
            if (stepOpenClose == 7) shutBack.OpenClose(15, 7);
            if (stepOpenClose == 8) shutBack.OpenClose(18, 8);
            if (stepOpenClose == 9) shutBack.OpenClose(21, 9);
            if (stepOpenClose == 10) shutBack.OpenClose(24, 10);
            if (stepOpenClose == 11) shutBack.OpenClose(35, 11);
            if (stepOpenClose == 12) shutBack.OpenClose(45, 12);

            if (stepOpenClose == 24)
                this.Exit.Location = new Point(-266, 483);
            if (stepOpenClose == 25)
                this.Exit.Location = new Point(-256, 461);
            if (stepOpenClose == 26)
                this.Exit.Location = new Point(-241, 451);
            if (stepOpenClose == 27)
                this.Exit.Location = new Point(-226, 439);
            if (stepOpenClose == 28)
                this.Exit.Location = new Point(-206, 422);
            if (stepOpenClose == 29)
                this.Exit.Location = new Point(-187, 403);
            if (stepOpenClose == 30)
                this.Exit.Location = new Point(-162, 386);

            if (stepOpenClose == 31)
                this.Info.Location = new Point(233, 506);
            if (stepOpenClose == 32)
                this.Info.Location = new Point(225, 471);
            if (stepOpenClose == 33)
                this.Info.Location = new Point(210, 440);
            if (stepOpenClose == 34)
                this.Info.Location = new Point(195, 414);
            if (stepOpenClose == 35)
                this.Info.Location = new Point(177, 386);
            if (stepOpenClose == 36)
                this.Info.Location = new Point(155, 353);
            if (stepOpenClose == 37)
                this.Info.Location = new Point(130, 326);

            if (stepOpenClose == 31)
                this.LogOff.Location = new Point(446, 469);
            if (stepOpenClose == 32)
                this.LogOff.Location = new Point(446, 403);
            if (stepOpenClose == 33)
                this.LogOff.Location = new Point(422, 335);
            if (stepOpenClose == 34)
                this.LogOff.Location = new Point(400, 266);
            if (stepOpenClose == 35)
                this.LogOff.Location = new Point(356, 200);
            if (stepOpenClose == 36)
                this.LogOff.Location = new Point(316, 142);
            if (stepOpenClose == 37)
                this.LogOff.Location = new Point(240, 89);
            if (stepOpenClose == 38)
                this.LogOff.Location = new Point(173, 40);
            if (stepOpenClose == 39)
                this.LogOff.Location = new Point(83, 23);
            if (stepOpenClose == 40)
                this.LogOff.Location = new Point(-10, 9);

            if (stepOpenClose == 41)
                this.Lock.Location = new Point(453, 481);
            if (stepOpenClose == 42)
                this.Lock.Location = new Point(453, 416);
            if (stepOpenClose == 43)
                this.Lock.Location = new Point(426, 342);
            if (stepOpenClose == 44)
                this.Lock.Location = new Point(388, 266);
            if (stepOpenClose == 45)
                this.Lock.Location = new Point(366, 205);
            if (stepOpenClose == 46)
                this.Lock.Location = new Point(313, 146);
            if (stepOpenClose == 47)
                this.Lock.Location = new Point(233, 89);
            if (stepOpenClose == 48)
                this.Lock.Location = new Point(163, 72);

            if (stepOpenClose == 49)
                this.Hibernate.Location = new Point(441, 467);
            if (stepOpenClose == 50)
                this.Hibernate.Location = new Point(441, 400);
            if (stepOpenClose == 51)
                this.Hibernate.Location = new Point(428, 326);
            if (stepOpenClose == 52)
                this.Hibernate.Location = new Point(395, 259);
            if (stepOpenClose == 53)
                this.Hibernate.Location = new Point(357, 196);
            if (stepOpenClose == 54)
                this.Hibernate.Location = new Point(287, 144);

            if (stepOpenClose == 55)
                this.Restart.Location = new Point(439, 474);
            if (stepOpenClose == 56)
                this.Restart.Location = new Point(439, 394);
            if (stepOpenClose == 57)
                this.Restart.Location = new Point(413, 326);
            if (stepOpenClose == 58)
                this.Restart.Location = new Point(388, 266);
            if (stepOpenClose == 59)
                this.ShutDown.Location = new Point(432, 474);
            if (stepOpenClose == 60)
                this.ShutDown.Location = new Point(432, 403);

            if (stepOpenClose == 61  || stepOpenClose == 100)
            {
                stepOpenClose = 0;
                OpenTmr.Enabled = false;
                StartColorAndZoomTimer();
                ServiceTmr.Enabled = true;
                this.TopMost = true;
                if (exit)
                {
                    shutBack.OpenClose(35, 12);
                    this.Exit.Location = new Point(-162, 386);
                    this.Info.Location = new Point(130, 326);
                    this.LogOff.Location = new Point(-10, 9);
                    this.Lock.Location = new Point(163, 72);
                    this.Hibernate.Location = new Point(287, 144);
                    this.Restart.Location = new Point(388, 266);
                    this.ShutDown.Location = new Point(432, 403);
                    ForceExit();
                }
            }
        }

        private void CloseTmr_Tick(object sender, EventArgs e)
        {
            stepOpenClose++;

            if (stepOpenClose == 1)
            {
                StopColorTimer();
                ServiceTmr.Enabled = false;
            } 

            if (stepOpenClose == 1)
                this.ShutDown.Location = new Point(432, 474);
            if (stepOpenClose == 2)
                this.ShutDown.Location = new Point(432, 538);

            if (stepOpenClose == 3)
                this.Restart.Location = new Point(413, 326);
            if (stepOpenClose == 4)
                this.Restart.Location = new Point(439, 394);
            if (stepOpenClose == 5)
                this.Restart.Location = new Point(439, 474);
            if (stepOpenClose == 6)
                this.Restart.Location = new Point(439, 538);

            if (stepOpenClose == 7)
                this.Hibernate.Location = new Point(357, 196);
            if (stepOpenClose == 8)
                this.Hibernate.Location = new Point(395, 259);
            if (stepOpenClose == 9)
                this.Hibernate.Location = new Point(428, 326);
            if (stepOpenClose == 10)
                this.Hibernate.Location = new Point(441, 400);
            if (stepOpenClose == 11)
                this.Hibernate.Location = new Point(441, 467);
            if (stepOpenClose == 12)
                this.Hibernate.Location = new Point(441, 538);

            if (stepOpenClose == 13)
                this.Lock.Location = new Point(233, 89);
            if (stepOpenClose == 14)
                this.Lock.Location = new Point(313, 146);
            if (stepOpenClose == 15)
                this.Lock.Location = new Point(366, 205);
            if (stepOpenClose == 16)
                this.Lock.Location = new Point(388, 266);
            if (stepOpenClose == 17)
                this.Lock.Location = new Point(426, 342);
            if (stepOpenClose == 18)
                this.Lock.Location = new Point(453, 416);
            if (stepOpenClose == 19)
                this.Lock.Location = new Point(453, 481);
            if (stepOpenClose == 20)
                this.Lock.Location = new Point(453, 538);

            if (stepOpenClose == 21)
                this.LogOff.Location = new Point(83, 23);
            if (stepOpenClose == 22)
                this.LogOff.Location = new Point(173, 40);
            if (stepOpenClose == 23)
                this.LogOff.Location = new Point(240, 89);
            if (stepOpenClose == 24)
                this.LogOff.Location = new Point(316, 142);
            if (stepOpenClose == 25)
                this.LogOff.Location = new Point(356, 200);
            if (stepOpenClose == 26)
                this.LogOff.Location = new Point(400, 266);
            if (stepOpenClose == 27)
                this.LogOff.Location = new Point(422, 335);
            if (stepOpenClose == 28)
                this.LogOff.Location = new Point(446, 403);
            if (stepOpenClose == 29)
                this.LogOff.Location = new Point(446, 469);
            if (stepOpenClose == 30)
                this.LogOff.Location = new Point(446, 538);

            if (stepOpenClose == 24)
                this.Info.Location = new Point(155, 353);
            if (stepOpenClose == 25)
                this.Info.Location = new Point(177, 386);
            if (stepOpenClose == 26)
                this.Info.Location = new Point(195, 414);
            if (stepOpenClose == 27)
                this.Info.Location = new Point(210, 440);
            if (stepOpenClose == 28)
                this.Info.Location = new Point(225, 471);
            if (stepOpenClose == 29)
                this.Info.Location = new Point(233, 506);
            if (stepOpenClose == 30)
                this.Info.Location = new Point(233, 538);

            if (stepOpenClose == 31)
                this.Exit.Location = new Point(-187, 403);
            if (stepOpenClose == 32)
                this.Exit.Location = new Point(-206, 422);
            if (stepOpenClose == 33)
                this.Exit.Location = new Point(-226, 439);
            if (stepOpenClose == 34)
                this.Exit.Location = new Point(-241, 451);
            if (stepOpenClose == 35)
                this.Exit.Location = new Point(-256, 461);
            if (stepOpenClose == 36)
                this.Exit.Location = new Point(-266, 483);
            if (stepOpenClose == 37)
                this.Exit.Location = new Point(-300, 542);

            if (stepOpenClose == 38) shutBack.OpenClose(35, 11);
            if (stepOpenClose == 39) shutBack.OpenClose(24, 10);
            if (stepOpenClose == 40) shutBack.OpenClose(21, 9);
            if (stepOpenClose == 41) shutBack.OpenClose(18, 8);
            if (stepOpenClose == 42) shutBack.OpenClose(15, 7);
            if (stepOpenClose == 43) shutBack.OpenClose(12, 6);
            if (stepOpenClose == 44) shutBack.OpenClose(9, 5);
            if (stepOpenClose == 45) shutBack.OpenClose(6, 4);
            if (stepOpenClose == 46) shutBack.OpenClose(3, 3);
            if (stepOpenClose == 47) shutBack.OpenClose(2, 2);
            if (stepOpenClose == 48) shutBack.OpenClose(1, 1);
            if (stepOpenClose == 49) shutBack.OpenClose(0, 0);

            if (stepOpenClose == 50)
            {
                stepOpenClose = 0;
                CloseTmr.Enabled = false;
                ExecuteRequest();
                //Shut.ActiveForm.Close();
                //Application.Exit();
                Environment.Exit(0);
            }
        }

        private void ColorAndZoomShutDownTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "ShutDown";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomShutDownTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessShutDown == true && amountBrightnessShutDown <= 1.0f) { 
                amountBrightnessShutDown += 0.10f;
                amountFontSizeShutDown += 1;
                if(amountBrightnessShutDown >= 1.0f) increaseBrightnessShutDown = false;
            }
            else 
            {
                amountBrightnessShutDown-= 0.10f;
                if (amountBrightnessShutDown <= 0) { increaseBrightnessShutDown = true; amountFontSizeShutDown = 0; ColorAndZoomShutDownTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeShutDown -= 1;
            }
            if (amountBrightnessShutDown < 0 || amountBrightnessShutDown > 1.0f) amountBrightnessShutDown = 0;
            if ((elementsInMouseHover[elementName] == false && increaseBrightnessShutDown == true))
            {
                this.ShutDown.Font = new Font(this.ShutDown.Font.FontFamily, fontsSizes[elementName] + amountFontSizeShutDown, FontStyle.Bold);
                
                this.ShutDown.ForeColor = AnimationColor(amountBrightnessShutDown);

                this.ShutDown.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeShutDown, elementsLocacations[elementName].Y - (int)amountFontSizeShutDown);
            }
        }

        private Color AnimationColor(float amount)
        {
            Color _color = default(Color);
            if ((color.R + color.G + color.B) <= (255 * 3) - 100)
            {
                _color = ControlPaint.Light(color, amount);
            }
            else
            {
                _color = ControlPaint.Dark(color, amount);
            }
            return _color;
        }

        private void ColorAndZoomExitTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "Exit";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomExitTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessExit == true && amountBrightnessExit <= 1.0f)
            {
                amountBrightnessExit += 0.10f;
                amountFontSizeExit += 1;
                if (amountBrightnessExit >= 1.0f) increaseBrightnessExit = false;
            }
            else 
            {
                amountBrightnessExit -= 0.10f;
                if (amountBrightnessExit <= 0) { increaseBrightnessExit = true; amountFontSizeExit = 0; ColorAndZoomExitTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeExit -= 1;
            }
            if (amountBrightnessExit < 0 || amountBrightnessExit > 1.0f) amountBrightnessExit = 0;
            if ((elementsInMouseHover[elementName] == false && increaseBrightnessExit == true))
            {
                this.Exit.Font = new Font(this.Exit.Font.FontFamily, fontsSizes[elementName] + amountFontSizeExit, FontStyle.Bold);
                
                this.Exit.ForeColor = AnimationColor(amountBrightnessExit);

                this.Exit.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeExit, elementsLocacations[elementName].Y - (int)amountFontSizeExit);
            }
        }

        private void ColorAndZoomLockTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "Lock";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomLockTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessLock == true && amountBrightnessLock <= 1.0f)
            {
                amountBrightnessLock += 0.10f;
                amountFontSizeLock += 1;
                if (amountBrightnessLock >= 1.0f) increaseBrightnessLock = false;
            }
            else 
            {
                amountBrightnessLock -= 0.10f;
                if (amountBrightnessLock <= 0) { increaseBrightnessLock = true; amountFontSizeLock = 0; ColorAndZoomLockTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeLock -= 1;
            }
            if (amountBrightnessLock < 0 || amountBrightnessLock > 1.0f) amountBrightnessLock = 0;
            if ((elementsInMouseHover[elementName] == false && increaseBrightnessLock == true))
            {
                this.Lock.Font = new Font(this.Lock.Font.FontFamily, fontsSizes[elementName] + amountFontSizeLock, FontStyle.Bold);
                
                this.Lock.ForeColor = AnimationColor(amountBrightnessLock);

                this.Lock.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeLock, elementsLocacations[elementName].Y - (int)amountFontSizeLock);
            }
        }

        private void ColorAndZoomRestartTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "Restart";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomRestartTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessRestart == true && amountBrightnessRestart <= 1.0f)
            {
                amountBrightnessRestart += 0.10f;
                amountFontSizeRestart += 1;
                if (amountBrightnessRestart >= 1.0f) increaseBrightnessRestart = false;
            }
            else
            {
                amountBrightnessRestart -= 0.10f;
                if (amountBrightnessRestart <= 0) { increaseBrightnessRestart = true; amountFontSizeRestart = 0; ColorAndZoomRestartTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeRestart -= 1;
            }
            if (amountBrightnessRestart < 0 || amountBrightnessRestart > 1.0f) amountBrightnessRestart = 0;
            if ((elementsInMouseHover[elementName] == false && increaseBrightnessRestart == true))
            {
                this.Restart.Font = new Font(this.Restart.Font.FontFamily, fontsSizes[elementName] + amountFontSizeRestart, FontStyle.Bold);
                
                this.Restart.ForeColor = AnimationColor(amountBrightnessRestart);

                this.Restart.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeRestart, elementsLocacations[elementName].Y - (int)amountFontSizeRestart);
            }
        }

        private void ColorAndZoomInfoTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "Info";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomInfoTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessInfo == true && amountBrightnessInfo <= 1.0f)
            {
                amountBrightnessInfo += 0.10f;
                amountFontSizeInfo += 1;
                if (amountBrightnessInfo >= 1.0f) increaseBrightnessInfo = false;
            }
            else 
            {
                amountBrightnessInfo -= 0.10f;
                if (amountBrightnessInfo <= 0) { increaseBrightnessInfo = true; amountFontSizeInfo = 0;  ColorAndZoomInfoTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeInfo -= 1;
            }
            if (amountBrightnessInfo < 0 || amountBrightnessInfo > 1.0f) amountBrightnessInfo = 0;
            if ((elementsInMouseHover[elementName] == false && increaseBrightnessInfo == true))
            {
                this.Info.Font = new Font(this.Info.Font.FontFamily, fontsSizes[elementName] + amountFontSizeInfo, FontStyle.Bold);
                
                this.Info.ForeColor = AnimationColor(amountBrightnessInfo);

                this.Info.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeInfo, elementsLocacations[elementName].Y - (int)amountFontSizeInfo);
            }
        }

        private void ColorAndZoomHibernateTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "Hibernate";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomHibernateTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessHibernate == true && amountBrightnessHibernate <= 1.0f)
            {
                amountBrightnessHibernate += 0.10f;
                amountFontSizeHibernate += 1;
                if (amountBrightnessHibernate >= 1.0f) increaseBrightnessHibernate = false;
            }
            else 
            {
                amountBrightnessHibernate -= 0.10f;
                if (amountBrightnessHibernate <= 0) { increaseBrightnessHibernate = true; amountFontSizeHibernate = 0; ColorAndZoomHibernateTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeHibernate -= 1;
            }
            if (amountBrightnessHibernate < 0 || amountBrightnessHibernate > 1.0f) amountBrightnessHibernate = 0;
            if ((elementsInMouseHover[elementName] == false && increaseBrightnessHibernate == true))
            {
                this.Hibernate.Font = new Font(this.Hibernate.Font.FontFamily, fontsSizes[elementName] + amountFontSizeHibernate, FontStyle.Bold);
                
                this.Hibernate.ForeColor = AnimationColor(amountBrightnessHibernate);

                this.Hibernate.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeHibernate, elementsLocacations[elementName].Y - (int)amountFontSizeHibernate);
            }
        }

        private void ColorAndZoomLogOffTmr_Tick(object sender, EventArgs e)
        {
            string elementName = "LogOff";
            if (!elementsInMouseHover.ContainsKey(elementName)) { return; }
            if (!elementsColorAndZoom.ContainsKey(elementName)) { return; }
            if (!fontsSizes.ContainsKey(elementName)) { return; }
            elementsColorAndZoom[elementName] = true;
            ColorAndZoomLogOffTmr.Interval = colorAndZoomTmrInterval;
            if (increaseBrightnessLogOff == true && amountBrightnessLogOff <= 1.0f)
            {
                amountBrightnessLogOff += 0.10f;
                amountFontSizeLogOff += 1;
                if (amountBrightnessLogOff >= 1.0f) increaseBrightnessLogOff = false;
            }
            else 
            {
                amountBrightnessLogOff -= 0.10f;
                if (amountBrightnessLogOff <= 0) { increaseBrightnessLogOff = true; amountFontSizeLogOff =0; ColorAndZoomLogOffTmr.Interval = 5000; elementsColorAndZoom[elementName] = false; }
                amountFontSizeLogOff -= 1;
            }
            
            if (amountBrightnessLogOff < 0 || amountBrightnessLogOff > 1.0f) amountBrightnessLogOff = 0;

            if ((elementsInMouseHover[elementName] == false && increaseBrightnessLogOff == true))
            {
                this.LogOff.Font = new Font(this.LogOff.Font.FontFamily, fontsSizes[elementName] + amountFontSizeLogOff, FontStyle.Bold);
                
                this.LogOff.ForeColor = AnimationColor(amountBrightnessLogOff);

                this.LogOff.Location = new Point(elementsLocacations[elementName].X - (int)amountFontSizeLogOff, elementsLocacations[elementName].Y - (int)amountFontSizeLogOff);
            }
        }

        private void MouseHover(Control control, float fontSize)
        {
            control.Font = new Font(control.Font.FontFamily, fontSize + 10, FontStyle.Bold);
            
            control.ForeColor = colorOnSelection;

            control.Top -= 7;
            control.Left -= 8;
        }

        private void MouseLeave(Control control, float fontSize)
        {
            control.Font = new Font(control.Font.FontFamily, fontSize, FontStyle.Bold);
            
            control.ForeColor = color;

            control.Location = elementsLocacations[control.Name];
        }

        private void ServiceTmr_Tick(object sender, EventArgs e)
        {
            try
            {
                Mouse(ShutDown);
                Mouse(Restart);
                Mouse(Hibernate);
                Mouse(Lock);
                Mouse(LogOff);
                Mouse(Info);
                Mouse(Exit);
            }
            catch (Exception ex)
            {
            }
        }

        private void Mouse(Control control)
        {
            Point pos = new Point(0,0);
            pos = utility.GetPositionAbsolute(this, control);

            if (!elementsInMouseHover.ContainsKey(control.Name) || !elementsColorAndZoom.ContainsKey(control.Name))
            {
                elementsInMouseHover["ShutDown"] = false;
                elementsInMouseHover["Exit"] = false;
                elementsInMouseHover["Lock"] = false;
                elementsInMouseHover["Restart"] = false;
                elementsInMouseHover["Info"] = false;
                elementsInMouseHover["Hibernate"] = false;
                elementsInMouseHover["LogOff"] = false;
                return;
            }

            if ((Cursor.Position.X >= pos.X && Cursor.Position.X <= pos.X + control.Width
                && Cursor.Position.Y >= pos.Y && Cursor.Position.Y <= pos.Y + control.Height)
                && (Cursor.Position.Y >= this.Top && Cursor.Position.Y <= this.Top + this.Height)
                && (Cursor.Position.X >= this.Left && Cursor.Position.X <= this.Top + this.Width)
                )
            {
                shutBack.SetToolTip(control.Name);
                if (elementsInMouseHover[control.Name] == false)
                {
                    elementsInMouseHover[control.Name] = true;
                    MouseHover(control, fontsSizes[control.Name]);
                }
            }
            else if (elementsColorAndZoom[control.Name] == false)
            {
                
                if (elementsInMouseHover[control.Name] == true)
                {
                    MouseLeave(control, fontsSizes[control.Name]);
                    elementsInMouseHover[control.Name] = false;
                    shutBack.SetToolTip(String.Empty);
                }
            }

            if ((elementsInMouseHover["ShutDown"] == true ||
            elementsInMouseHover["Exit"] == true ||
            elementsInMouseHover["Lock"] == true ||
            elementsInMouseHover["Restart"] == true ||
            elementsInMouseHover["Info"] == true ||
            elementsInMouseHover["Hibernate"] == true ||
            elementsInMouseHover["LogOff"] == true)) {
                cursorCurrent = Cursor.Current;
                Cursor.Current = Cursors.Hand;
            }
            else 
            {
                Cursor.Current = cursorCurrent;
                cursorCurrent = null;
            }
        }
    }
}