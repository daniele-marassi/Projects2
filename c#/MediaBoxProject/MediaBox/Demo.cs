using Additional;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static Additional.ScrollBar;
using static MediaBox.Models;

namespace MediaBox
{
    public class Demo
    {
        public class SendKeysParam
        {
            public string KeyCode { get; set; }
            public int Interval { get; set; }
        }

        Emulation.MousePointer emulationMousePointer = null;
        private static System.Timers.Timer SendKeysTmr = null;
        private static List<SendKeysParam> sendKeysList = new List<SendKeysParam>() { };

        public Demo()
        {
            Color color = Color.FromArgb(255, 248, 245, 32);
            emulationMousePointer = new Emulation.MousePointer(30,color,false);
            emulationMousePointer.Show();
            SendKeysTmr_Set();
        }

        private void SendKeysTmr_Set()
        {
            SendKeysTmr = new System.Timers.Timer(10000);
            SendKeysTmr.Elapsed += new ElapsedEventHandler(SendKeysTmr_Tick);
            SendKeysTmr.Interval = 1;
            SendKeysTmr.Enabled = false;
        }

        private void SendKeysTmr_Tick(object source, ElapsedEventArgs e)
        {
            SenderKey(true);
        }

        private void SendKeysScheduled(List<SendKeysParam> _sendKeysList)
        {
            SendKeysTmr.Interval = _sendKeysList.FirstOrDefault().Interval;
            SendKeysTmr.Enabled = true;
            sendKeysList = _sendKeysList;
        }

        private void SenderKey(bool timer = false)
        {
            SoundPlayer simpleSound = new SoundPlayer($@"{Statics.Constant.MainPath}\\Resources\\SoundEffects\\Press_KeyBoard.wav");
            simpleSound.PlaySync();
            if (sendKeysList.Count > 0)
            {
                if (timer == false)
                {
                    System.Threading.Thread.Sleep(sendKeysList.FirstOrDefault().Interval);
                }    
                SendKeys.SendWait(sendKeysList.FirstOrDefault().KeyCode);
                sendKeysList.RemoveAt(0);
            }
            if (sendKeysList.Count == 0)
            {
                if (timer == true)
                {
                    SendKeysTmr.Interval = 1;
                    SendKeysTmr.Enabled = false;
                }
            }
            else
            {
                if (timer == true)
                    SendKeysTmr.Interval = sendKeysList.FirstOrDefault().Interval;
            }
        }

        public void OpenMenu(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            Button MediaBoxMenuBtn = (Button)utility.GetControlByChildName(form, "MediaBoxMenuBtn");
            MediaBoxMenuBtn.PerformClick();

            emulationMousePointer.MoveToControl(form, "MediaBoxMenuBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(MediaBoxMenuBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            MediaBoxMenuBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            common.SetDemo(DemoStatus.HowToClose);
        }

        public void HowToClose(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "CloseMediaBoxBtn");
            Button CloseMediaBoxBtn = (Button)utility.GetControlByChildName(form, "CloseMediaBoxBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(CloseMediaBoxBtn.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(3000);
            emulationMousePointer.CloseTooltip();

            common.SetDemo(DemoStatus.SetDirectories);
        }

        public void SetDirectories(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            List<SendKeysParam> _sendKeysList = null;
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "OpenCloseManageSourceBtn");
            Button OpenCloseManageSourceBtn = (Button)utility.GetControlByChildName(form, "OpenCloseManageSourceBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(OpenCloseManageSourceBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseManageSourceBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "DirectoryOpenBtn");
            Button DirectoryOpenBtn = (Button)utility.GetControlByChildName(form, "DirectoryOpenBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(DirectoryOpenBtn.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;

            _sendKeysList = new List<SendKeysParam>() { new SendKeysParam { KeyCode = "{ESC}", Interval = 2000 }};
            SendKeysScheduled(_sendKeysList);

            emulationMousePointer.ClickUp(true);
            DirectoryOpenBtn.PerformClick();
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "DirectoryTxt");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickUp(true);
            TextBox DirectoryTxt = (TextBox)utility.GetControlByChildName(form, "DirectoryTxt");

            emulationMousePointer.ShowTooltip(common.GetTraslation(DirectoryTxt.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            DirectoryTxt.Focus();

            _sendKeysList = new List<SendKeysParam>()
            {
                new SendKeysParam { KeyCode = "{BACKSPACE}", Interval = 1000 }
            };

            text = $"{Statics.Constant.MainPath}\\Resources\\DemoPhotos\\";

            for (int i = 0; i < text.Substring(0,6).Length; i++)
            {
                intervalRnd = rnd.Next(1, 200);
                _sendKeysList.Add(new SendKeysParam { KeyCode = "{"+text[i].ToString()+"}", Interval = intervalRnd });
            }

            sendKeysList = _sendKeysList;
            while (sendKeysList.Count > 0 )
            {
                SenderKey();
            }

            DirectoryTxt.Text = text;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SourceAddBtn");
            Button SourceAddBtn = (Button)utility.GetControlByChildName(form, "SourceAddBtn");
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickDown(true);
            SourceAddBtn.PerformClick();
            emulationMousePointer.ClickUp(true);
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "DirectoryTxt");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickUp(true);
            DirectoryTxt.Focus();
            DirectoryTxt.Text = String.Empty;
            _sendKeysList = new List<SendKeysParam>()
            {
                new SendKeysParam { KeyCode = "{BACKSPACE}", Interval = 1000 }
            };

            text = $"{Statics.Constant.MainPath}\\Resources\\DemoPhotos_2\\";

            for (int i = 0; i < text.Substring(0, 6).Length; i++)
            {
                intervalRnd = rnd.Next(1, 200);
                _sendKeysList.Add(new SendKeysParam { KeyCode = "{" + text[i].ToString() + "}", Interval = intervalRnd });
            }

            sendKeysList = _sendKeysList;
            while (sendKeysList.Count > 0)
            {
                SenderKey();
            }

            DirectoryTxt.Text = text;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SourceAddBtn");
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickDown(true);
            SourceAddBtn.PerformClick();
            emulationMousePointer.ClickUp(true);
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "DirectoryTxt");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickUp(true);
            DirectoryTxt.Focus();
            DirectoryTxt.Text = String.Empty;
            _sendKeysList = new List<SendKeysParam>()
            {
                new SendKeysParam { KeyCode = "{BACKSPACE}", Interval = 1000 }
            };

            text = $"£%$??_TTTT";

            for (int i = 0; i < text.Length; i++)
            {
                intervalRnd = rnd.Next(1, 200);
                _sendKeysList.Add(new SendKeysParam { KeyCode = "{" + text[i].ToString() + "}", Interval = intervalRnd });
            }

            sendKeysList = _sendKeysList;
            while (sendKeysList.Count > 0)
            {
                SenderKey();
            }

            DirectoryTxt.Text = text;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SourceAddBtn");
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickDown(true);
            SourceAddBtn.PerformClick();
            emulationMousePointer.ClickUp(true);
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);

            emulationMousePointer.MoveToControl(form, "SourceList");
            ListBox SourceList = (ListBox)utility.GetControlByChildName(form, "SourceList");
            
            emulationMousePointer.ShowTooltip(common.GetTraslation(SourceList.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();
            emulationMousePointer.MoveToTop(-10);
            System.Threading.Thread.Sleep(500);
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickDown(true);
            SourceList.SelectedIndex = 2;
            emulationMousePointer.ClickUp(true);
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SourceDelBtn");
            Button SourceDelBtn = (Button)utility.GetControlByChildName(form, "SourceDelBtn");
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickDown(true);
            SourceDelBtn.PerformClick();
            emulationMousePointer.ClickUp(true);
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "OpenCloseManageSourceBtn");
            LeftPnl.Enabled = true;
            emulationMousePointer.ClickDown(true);
            OpenCloseManageSourceBtn.PerformClick();
            emulationMousePointer.ClickUp(true);
            LeftPnl.Enabled = false;

            common.SetDemo(DemoStatus.UpdateCatalog);
        }

        public void UpdateCatalog(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            List<SendKeysParam> _sendKeysList = null;
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            common.SetDemo(DemoStatus.PersonalizeColor);

            emulationMousePointer.MoveToControl(form, "UpdateCatalogBtn");
            Button UpdateCatalogBtn = (Button)utility.GetControlByChildName(form, "UpdateCatalogBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(UpdateCatalogBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;

            _sendKeysList = new List<SendKeysParam>()
            {
                new SendKeysParam { KeyCode = "{ENTER}", Interval = 1000 }
            };

            SendKeysScheduled(_sendKeysList);

            UpdateCatalogBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);
        }

        public void PersonalizeColor(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "OpenClosePersonalizeColorBtn");
            Button OpenClosePersonalizeColorBtn = (Button)utility.GetControlByChildName(form, "OpenClosePersonalizeColorBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(OpenClosePersonalizeColorBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenClosePersonalizeColorBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ChoiceColorCmb");
            ComboBox ChoiceColorCmb = (ComboBox)utility.GetControlByChildName(form, "ChoiceColorCmb");

            emulationMousePointer.ShowTooltip(common.GetTraslation(ChoiceColorCmb.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            ChoiceColorCmb.DroppedDown = true;   
            emulationMousePointer.ClickUp(true);

            ChoiceColorCmb.Focus();
            SoundPlayer simpleSound = new SoundPlayer($@"{Statics.Constant.MainPath}\\Resources\\SoundEffects\\Press_KeyBoard.wav");
            ChoiceColorCmb.SelectedIndex=0;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ChoiceColorCmb");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            ChoiceColorCmb.DroppedDown = false;
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ScrollBarVerticalCursorBtn_PersonalizeColorRed");
            emulationMousePointer.MoveToTop(10);
            emulationMousePointer.MoveToLeft(10);
            emulationMousePointer.Focus();
            Control ScrollBarVerticalCursorBtn_PersonalizeColorRed = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_PersonalizeColorRed");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            for (int i = 0; i < 210; i++)
            {
                ScrollBarVerticalCursorBtn_PersonalizeColorRed.Location = new Point(ScrollBarVerticalCursorBtn_PersonalizeColorRed.Location.X, ScrollBarVerticalCursorBtn_PersonalizeColorRed.Location.Y+1);
                emulationMousePointer.MoveToTop(1);
                Services.SetPersonalizeColor(form);
            }
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ScrollBarVerticalCursorBtn_PersonalizeColorGreen");
            emulationMousePointer.MoveToTop(10);
            emulationMousePointer.MoveToLeft(10);
            emulationMousePointer.Focus();
            Control ScrollBarVerticalCursorBtn_PersonalizeColorGreen = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_PersonalizeColorGreen");
            emulationMousePointer.ClickDown(true);
            for (int i = 0; i < 210; i++)
            {
                ScrollBarVerticalCursorBtn_PersonalizeColorGreen.Location = new Point(ScrollBarVerticalCursorBtn_PersonalizeColorGreen.Location.X, ScrollBarVerticalCursorBtn_PersonalizeColorGreen.Location.Y + 1);
                emulationMousePointer.MoveToTop(1);
                Services.SetPersonalizeColor(form);
            }
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ScrollBarVerticalCursorBtn_PersonalizeColorBlue");
            emulationMousePointer.MoveToTop(10);
            emulationMousePointer.MoveToLeft(10);
            emulationMousePointer.Focus();
            Control ScrollBarVerticalCursorBtn_PersonalizeColorBlue = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_PersonalizeColorBlue");
            emulationMousePointer.ClickDown(true);
            for (int i = 0; i < 210; i++)
            {
                ScrollBarVerticalCursorBtn_PersonalizeColorBlue.Location = new Point(ScrollBarVerticalCursorBtn_PersonalizeColorBlue.Location.X, ScrollBarVerticalCursorBtn_PersonalizeColorBlue.Location.Y + 1);
                emulationMousePointer.MoveToTop(1);
                Services.SetPersonalizeColor(form);
            }
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SetColorBtn");
            Button SetColorBtn = (Button)utility.GetControlByChildName(form, "SetColorBtn");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            SetColorBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ChoiceColorCmb");
            ChoiceColorCmb = (ComboBox)utility.GetControlByChildName(form, "ChoiceColorCmb");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            ChoiceColorCmb.DroppedDown = true;
            emulationMousePointer.ClickUp(true);

            ChoiceColorCmb.Focus();
            simpleSound = new SoundPlayer($@"{Statics.Constant.MainPath}\\Resources\\SoundEffects\\Press_KeyBoard.wav");
            ChoiceColorCmb.SelectedIndex = 1;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ChoiceColorCmb");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            ChoiceColorCmb.DroppedDown = false;
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ScrollBarVerticalCursorBtn_PersonalizeColorRed");
            emulationMousePointer.MoveToTop(10);
            emulationMousePointer.MoveToLeft(10);
            emulationMousePointer.Focus();
            ScrollBarVerticalCursorBtn_PersonalizeColorRed = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_PersonalizeColorRed");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            for (int i = 0; i < 200; i++)
            {
                ScrollBarVerticalCursorBtn_PersonalizeColorRed.Location = new Point(ScrollBarVerticalCursorBtn_PersonalizeColorRed.Location.X, ScrollBarVerticalCursorBtn_PersonalizeColorRed.Location.Y - 1);
                emulationMousePointer.MoveToTop(-1);
                Services.SetPersonalizeColor(form);
            }
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "ScrollBarVerticalCursorBtn_PersonalizeColorGreen");
            emulationMousePointer.MoveToTop(10);
            emulationMousePointer.MoveToLeft(10);
            emulationMousePointer.Focus();
            ScrollBarVerticalCursorBtn_PersonalizeColorGreen = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_PersonalizeColorGreen");
            emulationMousePointer.ClickDown(true);
            for (int i = 0; i < 200; i++)
            {
                ScrollBarVerticalCursorBtn_PersonalizeColorGreen.Location = new Point(ScrollBarVerticalCursorBtn_PersonalizeColorGreen.Location.X, ScrollBarVerticalCursorBtn_PersonalizeColorGreen.Location.Y - 1);
                emulationMousePointer.MoveToTop(-1);
                Services.SetPersonalizeColor(form);
            }
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SetColorBtn");
            SetColorBtn = (Button)utility.GetControlByChildName(form, "SetColorBtn");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            SetColorBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "OpenClosePersonalizeColorBtn");
            OpenClosePersonalizeColorBtn = (Button)utility.GetControlByChildName(form, "OpenClosePersonalizeColorBtn");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenClosePersonalizeColorBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            common.SetDemo(DemoStatus.GeneralSettings);
        }

        public void GeneralSettings(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            List<SendKeysParam> _sendKeysList = null;
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "OpenCloseGeneralSettingsBtn");
            Button OpenCloseGeneralSettingsBtn = (Button)utility.GetControlByChildName(form, "OpenCloseGeneralSettingsBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(OpenCloseGeneralSettingsBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseGeneralSettingsBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "LanguageitITBtn");
            Button LanguageitITBtn = (Button)utility.GetControlByChildName(form, "LanguageitITBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(LanguageitITBtn.Name + "Demo"),Color.FromArgb(255,255,255,255),Color.FromArgb(255,0,0,0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            LanguageitITBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "PictureIntervalTxt");

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            //LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            TextBox PictureIntervalTxt = (TextBox)utility.GetControlByChildName(form, "PictureIntervalTxt");

            emulationMousePointer.ShowTooltip(common.GetTraslation(PictureIntervalTxt.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(3000);
            emulationMousePointer.CloseTooltip();
            PictureIntervalTxt.Focus();

            _sendKeysList = new List<SendKeysParam>()
            {
                new SendKeysParam { KeyCode = "{BACKSPACE}", Interval = 1000 }
            };

            text = "10";

            for (int i = 0; i < text.Length; i++)
            {
                intervalRnd = rnd.Next(1, 200);
                _sendKeysList.Add(new SendKeysParam { KeyCode = "{" + text[i].ToString() + "}", Interval = intervalRnd });
            }

            sendKeysList = _sendKeysList;
            LeftPnl.Enabled = true;
            while (sendKeysList.Count > 0)
            {
                SenderKey();
            }
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "WidthBorderMediaTxt");

            TextBox WidthBorderMediaTxt = (TextBox)utility.GetControlByChildName(form, "WidthBorderMediaTxt");

            emulationMousePointer.ShowTooltip(common.GetTraslation(WidthBorderMediaTxt.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "AutoRunAnimationMediaBtn");
            Button AutoRunAnimationMediaBtn = (Button)utility.GetControlByChildName(form, "AutoRunAnimationMediaBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(AutoRunAnimationMediaBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            AutoRunAnimationMediaBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "SetGeneralSettingsBtn");
            Button SetGeneralSettingsBtn = (Button)utility.GetControlByChildName(form, "SetGeneralSettingsBtn");

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            SetGeneralSettingsBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "OpenCloseGeneralSettingsBtn");

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseGeneralSettingsBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            common.SetDemo(DemoStatus.Music);
        }

        public void Music(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "OpenCloseMusicBtn");
            Button OpenCloseMusicBtn = (Button)utility.GetControlByChildName(form, "OpenCloseMusicBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(OpenCloseMusicBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseMusicBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "PlayListMusicCmb");
            ComboBox PlayListMusicCmb = (ComboBox)utility.GetControlByChildName(form, "PlayListMusicCmb");

            emulationMousePointer.ShowTooltip(common.GetTraslation(PlayListMusicCmb.Name + "Demo"), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(4000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            PlayListMusicCmb.DroppedDown = true;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(500);
            LeftPnl.Enabled = false;

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.MoveToControl(form, "OpenCloseMusicBtn");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseMusicBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            common.SetDemo(DemoStatus.Info);
        }

        public void Info(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "OpenCloseInfoBtn");
            Button OpenCloseInfoBtn = (Button)utility.GetControlByChildName(form, "OpenCloseInfoBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(OpenCloseInfoBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseInfoBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            System.Threading.Thread.Sleep(2000);

            emulationMousePointer.MoveToControl(form, "OpenCloseInfoBtn");
            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            OpenCloseInfoBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

            common.SetDemo(DemoStatus.Animate);
        }

        public void Animate(Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            Panel LeftPnl = (Panel)utility.GetControlByChildName(form, "LeftPnl");
            string text = String.Empty;
            var rnd = new Random(DateTime.Now.Millisecond);

            int intervalRnd = rnd.Next(0, 2);

            emulationMousePointer.MoveToControl(form, "StartStopAnimateBtn");
            Button StartStopAnimateBtn = (Button)utility.GetControlByChildName(form, "StartStopAnimateBtn");

            emulationMousePointer.ShowTooltip(common.GetTraslation(StartStopAnimateBtn.Name), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 0, 0, 0));
            System.Threading.Thread.Sleep(2000);
            emulationMousePointer.CloseTooltip();

            emulationMousePointer.ClickDown(true);
            LeftPnl.Enabled = true;
            StartStopAnimateBtn.PerformClick();
            LeftPnl.Enabled = false;
            emulationMousePointer.ClickUp(true);

        }

    }
}
