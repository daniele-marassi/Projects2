using Additional;
using Newtonsoft.Json;
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
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\multimedia_icons.ttf");
            Statics.Fonts.AddFontFile($"{Statics.Constant.MainPath}\\Resources\\Fonts\\Icons_South_St_0.ttf");
        }

        public void SetDemo(DemoStatus status)
        {
            
            SqLite sqLite = new SqLite();
            sqLite.OpenDB(Statics.Constant.FilePathDB);
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `Demo`={Convert.ToInt32(status).ToString()} WHERE ID = 1;");
            sqLite.CloseDB();
            Statics.FormParam.Demo = status;
        }

        public string GetTraslation(string name, int language = -1)
        {
            string text = String.Empty;
            try
            {
                if(language == -1) language = (int)Statics.FormParam.LanguageSelected;
                text = Statics.Translates.Where(_ => _.Name == name && _.IDLanguage == language).FirstOrDefault().Text;   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

        public SourceValue GetSourceValue(string valueStringJson)
        {
            var sourceValue = new SourceValue() { };
            try
            {
                sourceValue = JsonConvert.DeserializeObject<SourceValue>(valueStringJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return sourceValue;
        }

        public string GetSourceToList(string valueStringJson)
        {
            SourceType sourceType = new SourceType();
            var sourceValue = GetSourceValue(valueStringJson);
            var source = String.Empty;

            if (source != String.Empty) source += ", ";
            if (sourceValue.Type != String.Empty && sourceValue.Type != null) source += sourceValue.Type;

            if (sourceValue.Path != String.Empty && sourceValue.Path != null && sourceValue.Type == sourceType.Directory)
            {
                if (source != String.Empty) source += ", ";
                source += sourceValue.Path;
            }

            if (sourceValue.AccountDrive != String.Empty && sourceValue.AccountDrive != null && sourceValue.Type == sourceType.Drive)
            {
                if (source != String.Empty) source += ", ";
                source += sourceValue.AccountDrive;
            }

            if (sourceValue.PathAuthenticateJsonToDrive != String.Empty && sourceValue.PathAuthenticateJsonToDrive != null && sourceValue.Type == sourceType.Drive)
            {
                if (source != String.Empty) source += ", ";
                source += sourceValue.PathAuthenticateJsonToDrive;
            }
            
            if (sourceValue.FolderNameToFilterDrive != String.Empty && sourceValue.FolderNameToFilterDrive != null && sourceValue.Type == sourceType.Drive)
            {
                if (source != String.Empty) source += ", ";
                source += sourceValue.FolderNameToFilterDrive;
            }
            return source;
        }
    }
}
