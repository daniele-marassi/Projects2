using Additional;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MediaBox.Models;

namespace MediaBox
{
    public class Common
    {
        private Additional.ProgressBar progressBar = new Additional.ProgressBar();

        public void ProgressBar(int countElement, string info, Form form, string status = null)
        {
            Color color = Statics.FormParam.ForeColor;
            Color backColor = Statics.FormParam.BackForeColor;
            Additional.ProgressBar.StatusOption statusOption = new Additional.ProgressBar.StatusOption();
            if (status == statusOption.Start) { ProgressBarStart(); }
            string currentStatusOption = progressBar.Step(countElement, info, backColor, color, form.Controls["BottomPnl"], status);
            if (currentStatusOption == statusOption.End) { ProgressBarEnd(); }
        }

        public void ProgressBarStart()
        {
            MediaBox mediaBox = new MediaBox();
            mediaBox.ProgressBarStart();
        }

        public void ProgressBarEnd()
        {
            MediaBox mediaBox = new MediaBox();
            mediaBox.ProgressBarEnd();
        }

        public void LoadFonts()
        {
            Statics.Fonts = new PrivateFontCollection();
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\cour.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\courbd.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\courbi.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\couri.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\heydings_icons.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\JournalDingbats2.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\verdana.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\verdanab.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\verdanai.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\verdanaz.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\webdings.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\WebSymbols-Regular.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\wingding.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\WINGDNG2.TTF");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\WINGDNG3.TTF");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\Marlett.TTF");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\We Spray.TTF");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\MusiSync.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\Flavors-Regular_0.ttf");
        }

        public void SetDemo(DemoStatus status)
        {
            
            SqLite sqLite = new SqLite();
            sqLite.OpenDB(Statics.Constant.FilePathDB);
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `Demo`={Convert.ToInt32(status).ToString()} WHERE ID = 1;");
            sqLite.CloseDB();
            Statics.FormParam.Demo = status;
        }

        public string GetTraslation(string name)
        {
            string text = String.Empty;
            try
            {
                text = Statics.Translates.Where(_ => _.Name == name && _.IDLanguage == (int)Statics.FormParam.LanguageSelected).FirstOrDefault().Text;   
            }
            catch (Exception)
            {
            }
            return text;
        }

        public void ChangeLogo(Form form)
        {
            Application.DoEvents();
            form.Icon = null;
            Utility utility = new Utility();
            utility.CreatePngFromControl(Statics.MainForm, (Button)Statics.MainForm.Controls["MediaBoxMenuBtn"], $"{Statics.Constant.PersonalPath}\\Temp\\Logo.png");

            Bitmap myBitmap = new Bitmap($"{Statics.Constant.PersonalPath}\\Temp\\Logo.png");

            //myBitmap.Save($"{Statics.Constant.PersonalPath}\\Temp\\Logo.ico", System.Drawing.Imaging.ImageFormat.Icon);

            IntPtr Hicon = myBitmap.GetHicon();
            Icon newIcon = Icon.FromHandle(Hicon);

            form.Icon = newIcon;
            //this.Icon = Icon.ExtractAssociatedIcon($"{Statics.Constant.PersonalPath}\\Temp\\Logo.ico");
            form.ShowInTaskbar = true;
            form.Refresh();
            form.Update();
            form.Focus();
        }
    }
}
