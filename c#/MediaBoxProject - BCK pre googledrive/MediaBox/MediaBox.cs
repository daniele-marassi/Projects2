using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using static MediaBox.Models;
using WMPLib;
using System.Timers;
using System.Drawing.Text;
using System.Reflection;
using Additional;
using static Additional.ScrollBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using System.Security.AccessControl;

namespace MediaBox
{
    public partial class MediaBox : Form
    {
        
        private dynamic previousStatusHideMenu = null;
        private bool PlayListMusicCmb_Enabled = false;
        private static Color CurrentPersonalizeColor = Color.FromArgb(0,0,0,0);

        public static Additional.ScrollBar scrollBarPersonalizeColorRed = new Additional.ScrollBar();
        public static Additional.ScrollBar scrollBarPersonalizeColorGreen = new Additional.ScrollBar();
        public static Additional.ScrollBar scrollBarPersonalizeColorBlue = new Additional.ScrollBar();

        public static Additional.ScrollBar scrollBarAudio = new Additional.ScrollBar();

        public static Additional.ScrollBar scrollBarMediaMovementSpeed = new Additional.ScrollBar();
        
        System.Windows.Forms.ToolTip toolTips = new System.Windows.Forms.ToolTip();

        public MediaBox()
        {
            InitializeComponent();
            Common common = new Common();
        }

        private void ReadData()
        {
            try
            {
                SqLite sqLite = new SqLite();
                Data data = new Data();
                MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
                if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
                { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
                else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
                Statics.DB = data.GetData(this);
                sqLite.CloseDB();
                multimediaCatalog.LoadThumbnail(PicturePnl, Statics.DB, this);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void UpdateCatalogBtn_Click(object sender, EventArgs e)
        {
            Common common = new Common();
            DialogResult dialogResult = MessageBox.Show(common.GetTraslation("UpdateCatalogMessage"), "Media Box", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            try
            {
                Application.DoEvents();
                MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
                SqLite sqLite = new SqLite();
                Models.MediaParam media = new Models.MediaParam();
                Models.MediaParam media_tmp = new Models.MediaParam();
                if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
                { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
                else { sqLite.OpenDB(Statics.Constant.FilePathDB); }

                try
                {
                    sqLite.ExecuteCommand("DELETE FROM Catalog;");
                }
                catch (Exception ex)
                {
                    //throw;
                }

                sqLite.CloseDB();
                int nPictures=0;
                List<Catalog> listCatalog = null;
                DirectoryType directoryType = new DirectoryType();
                foreach (DirectoryParam item in Statics.Directories.Where(_ => _.Type == directoryType.Catalog))
                {
                    if (!Directory.Exists(item.Path)) { continue; }
                    listCatalog = multimediaCatalog.GetMedia(item.Path, this);
                    nPictures += listCatalog.Count;
                }

                foreach (DirectoryParam item in Statics.Directories.Where(_ => _.Type == directoryType.Catalog))
                {
                    if (!Directory.Exists(item.Path)) { continue; }
                    media_tmp = multimediaCatalog.PopolateCatalogFromSource(item.Path, this, nPictures);
                    
                    media.CountElements += media_tmp.ListCatalog.Count();
                    media.HeightPanel = media_tmp.HeightPanel;
                    media.WidthPanel = media_tmp.WidthPanel;

                    if (media.ListCatalog == null)
                    { media.ListCatalog = media_tmp.ListCatalog; }
                    else { media.ListCatalog.AddRange(media_tmp.ListCatalog); }

                    if (media.ListThumbnail == null)
                    { media.ListThumbnail = media_tmp.ListThumbnail; }
                    else { media.ListThumbnail.AddRange(media_tmp.ListThumbnail); }

                }
                media = multimediaCatalog.PicturePosition(media, this);
                multimediaCatalog.SaveData(media, this);
                media = null;
                PicturePnl.Controls.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //ReadData();
                Restart();
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        private void PicturePnl_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            Statics.Position.Y = e.Location.Y;
            Statics.Position.X = e.Location.X;
            Statics.Mouse.Down = true;
        }

        private void PicturePnl_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            Statics.Mouse.Down = false;
        }

        private void PicturePnl_MouseMove(object sender, MouseEventArgs e)
        {
            if (Statics.Mouse.Down)
            {
                Application.DoEvents();
                int top = Cursor.Position.Y - (this.Location.Y + Statics.Position.Y);
                int left = Cursor.Position.X - (this.Location.X + Statics.Position.X);
                if (top <= 0 && (Math.Abs(top) <= Math.Abs(this.Height - PicturePnl.Height) && PicturePnl.Height > this.Height)) { PicturePnl.Top = top; }
                if (left <= 0 && (Math.Abs(left) <= Math.Abs(this.Width - PicturePnl.Width) && PicturePnl.Width > this.Width)) { PicturePnl.Left = left; }
            }
        }

        private void MediaBox_Move(object sender, EventArgs e)
        {
            Statics.FormParam.Left = this.Location.X;
            Statics.FormParam.Top = this.Location.Y;
        }

        private void MinimizeMediaBoxBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CloseMediaBoxBtn_Click(object sender, EventArgs e)
        {
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.Avviable) { StopDemo(); }
            this.Close();
        }

        private void TopPnl_MouseDown(object sender, MouseEventArgs e)
        {
            Statics.Mouse.Down = true;
            Statics.Position = e.Location;
        }

        private void TopPnl_MouseUp(object sender, MouseEventArgs e)
        {
            Statics.Mouse.Down = false;
        }

        private void TopPnl_MouseMove(object sender, MouseEventArgs e)
        {
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart && Statics.FormParam.Demo != DemoStatus.Avviable) { return; }
            if (Statics.Mouse.Down)
            {
                this.Location = new Point(
                    Cursor.Position.X - (Statics.Position.X), Cursor.Position.Y - (Statics.Position.Y ));

                this.Update();
            }
        }

        private void SetFonts()
        {
            Utility utility = new Utility();
            this.AutoRunAnimationMediaBtn.Font = utility.GetFonts("Journal Dingbats 2", 18F, Statics.Fonts);
            this.OpenCloseGeneralSettingsBtn.Font = utility.GetFonts("Web Symbols", 20.25F, Statics.Fonts);
            this.DirectoryOpenBtn.Font = utility.GetFonts("Web Symbols", 12F, Statics.Fonts);
            this.DirectoryDelBtn.Font = utility.GetFonts("Wingdings 2", 16F, Statics.Fonts);
            this.DirectoryAddBtn.Font = utility.GetFonts("Web Symbols", 14.25F, Statics.Fonts);
            this.OpenCloseManageDirectoryBtn.Font = utility.GetFonts("Web Symbols", 18F, Statics.Fonts);
            this.StartStopAnimateBtn.Font = utility.GetFonts("Web Symbols", 18F, Statics.Fonts);
            this.SetColorBtn.Font = utility.GetFonts("Web Symbols", 12F, Statics.Fonts);
            this.OpenClosePersonalizeColorBtn.Font = utility.GetFonts("We Spray", 20.25F, Statics.Fonts);
            this.UpdateCatalogBtn.Font = utility.GetFonts("Web Symbols", 18F, Statics.Fonts);
            this.MaximizeMediaBoxBtn.Font = utility.GetFonts("Webdings", 14F, Statics.Fonts);
            this.CloseMediaBoxBtn.Font = utility.GetFonts("Webdings", 12F, Statics.Fonts);
            this.ResizeMediaBoxLbl.Font = utility.GetFonts("Marlett", 10F, Statics.Fonts);
            this.OpenCloseMusicBtn.Font = utility.GetFonts("MusiSync", 18F, Statics.Fonts);
            this.AudioLbl.Font = utility.GetFonts("Heydings Icons", 18F, Statics.Fonts);
            this.SetGeneralSettingsBtn.Font = utility.GetFonts("Web Symbols", 14.25F, Statics.Fonts);
            this.MediaBoxMenuBtn.Font = utility.GetFonts("Flavors", 27.75F, Statics.Fonts);
        }

        private void SetDB()
        {
            SqLite sqLite = new SqLite();
            string sql = String.Empty;
            Statics.Constant.FilePathDB = $"{Statics.Constant.PersonalPath}\\Data\\MediaBoxDB.sqlite";
            if (!File.Exists(Statics.Constant.FilePathDB))
            {
                sqLite.CreateDB(Statics.Constant.FilePathDB);
                sqLite.OpenDB(Statics.Constant.FilePathDB);
                sql = System.IO.File.ReadAllText($"{Statics.Constant.MainPath}\\Data\\default_DB_NOT_EDIT_NOT_REMOVE.sql");
                sqLite.ExecuteCommand(sql);
                sqLite.CloseDB();
            }
        }

        private void SetDemoDB()
        {
            SqLite sqLite = new SqLite();
            string sql = String.Empty;
            Statics.Constant.FilePathDemoDB = $"{Statics.Constant.PersonalPath}\\Data\\MediaBoxDemoDB.sqlite";

            if (
                    Statics.FormParam.Demo == DemoStatus.NotAvviable
                    || Statics.FormParam.Demo == DemoStatus.Avviable
               )
            {
                File.Delete(Statics.Constant.FilePathDemoDB);
            }
            

            if (!File.Exists(Statics.Constant.FilePathDemoDB))
            {
                sqLite.CreateDB(Statics.Constant.FilePathDemoDB);
                sqLite.OpenDB(Statics.Constant.FilePathDemoDB);
                sql = System.IO.File.ReadAllText($"{Statics.Constant.MainPath}\\Data\\default_DB_NOT_EDIT_NOT_REMOVE.sql");
                sqLite.ExecuteCommand(sql);
                sqLite.CloseDB();
            }

        }

        private DemoStatus Get_DemoStatus()
        {
            DemoStatus demoStatus= DemoStatus.Undefined;
            try
            {
                SqLite sqLite = new SqLite();
                string sql = String.Empty;

                sqLite.OpenDB(Statics.Constant.FilePathDB);
                sql = "SELECT Demo FROM Configuration;";
                DataTable configuration = sqLite.ExecuteSelect(sql);
                demoStatus = (DemoStatus)Convert.ToInt32(configuration.Rows[0]["Demo"]);
            }
            catch (Exception)
            {
            }

            return demoStatus;
        }

        private void SetPersonalFolder()
        {
            Statics.Constant.PersonalPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\MediaBox";

            if (Directory.Exists($"{Statics.Constant.PersonalPath}\\Temp") == false)
            {
                Directory.CreateDirectory($"{Statics.Constant.PersonalPath}\\Temp");
            }

            if (Directory.Exists($"{Statics.Constant.PersonalPath}\\Data") == false)
            {
                Directory.CreateDirectory($"{Statics.Constant.PersonalPath}\\Data");
            }

            if (Directory.Exists($"{Statics.Constant.PersonalPath}\\Data") == false)
            {
                Directory.CreateDirectory($"{Statics.Constant.PersonalPath}\\Data");
            }

            Environment.CurrentDirectory = $"{Statics.Constant.PersonalPath}\\Temp";

        }

        private void MediaBox_Load(object sender, EventArgs e)
        {
            Statics.MainForm = this;
            Media media = new Media();
            try
            {
                
                Statics.Constant.MainPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

                Common common = new Common();
                Utility utility = new Utility();

                SetPersonalFolder();

                common.LoadFonts();

                SetFonts();

                OpenCloseManageDirectory();
                OpenClosePersonalizeColor();
                OpenCloseGeneralSettings();
                OpenCloseMusic();
                OpenCloseInfo();

                SetDB();

                Statics.FormParam.Demo = Get_DemoStatus();

                if (Statics.FormParam.Demo == DemoStatus.OpenMenu)
                {
                    TopPnl.Visible = false;
                    LeftPnl.Visible = false;
                    BottomPnl.Visible = false;
                }

                SetDemoDB();
                EvievInit();
                ReadData();

                if ( Statics.FormParam.Demo == DemoStatus.NotAvviable )
                {
                    previousStatusHideMenu = true;
                    waitHideMediaBoxMenu.Enabled = true;
                }
                else if ( Statics.FormParam.Demo == DemoStatus.OpenMenu )
                {
                    previousStatusHideMenu = true;
                    waitHideMediaBoxMenu.Enabled = false;

                    Statics.MenuHide = true;
                    bool show = true;
                    while (show)
                    {
                        Application.DoEvents();
                        show = MediaBoxMenuHideShow();
                    }
                    TopPnl.Visible = true;
                    LeftPnl.Visible = true;
                    BottomPnl.Visible = true;
                }
                else
                {
                    previousStatusHideMenu = false;
                    waitHideMediaBoxMenu.Enabled = false;
                }

                if (Statics.FormParam.AutoRunAnimationMedia == true)
                {
                    PresentationTmr.Enabled = true;
                    StartStopAnimate();
                }
                this.Opacity = 100D;

                if (Statics.FormParam.Demo == DemoStatus.Avviable)
                {
                    ShowDemo();
                }

                if (Statics.FormParam.Demo != DemoStatus.Avviable && Statics.FormParam.Demo != DemoStatus.NotAvviable)
                {
                    StartDemo();
                }

                SetToolTips();

                common.ChangeLogo(this);

                InitService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        private void SetToolTips()
        {
            Common common = new Common();
            
            toolTips.RemoveAll();

            toolTips.SetToolTip(OpenCloseManageDirectoryBtn, common.GetTraslation(OpenCloseManageDirectoryBtn.Name));            
            toolTips.SetToolTip(UpdateCatalogBtn, common.GetTraslation(UpdateCatalogBtn.Name));
            toolTips.SetToolTip(OpenClosePersonalizeColorBtn, common.GetTraslation(UpdateCatalogBtn.Name));
            toolTips.SetToolTip(OpenCloseGeneralSettingsBtn, common.GetTraslation(OpenCloseGeneralSettingsBtn.Name));
            toolTips.SetToolTip(StartStopAnimateBtn, common.GetTraslation(StartStopAnimateBtn.Name));
            toolTips.SetToolTip(OpenCloseMusicBtn, common.GetTraslation(OpenCloseMusicBtn.Name));
            toolTips.SetToolTip(OpenCloseInfoBtn, common.GetTraslation(OpenCloseInfoBtn.Name));
            toolTips.SetToolTip(AutoRunAnimationMediaBtn, common.GetTraslation(AutoRunAnimationMediaBtn.Name));
            toolTips.SetToolTip(MediaBoxMenuBtn, common.GetTraslation(MediaBoxMenuBtn.Name));
        }

        public void StartDemo()
        {
            Statics.FormParam.DemoRuning = true;
            Demo demo = new Demo();
            Common common = new Common();
            Utility utility = new Utility();
            DemoPnl.Visible = false;
            LeftPnl.Enabled = false;
            BottomPnl.Enabled = false;
            MinimizeMediaBoxBtn.Enabled = false;
            MaximizeMediaBoxBtn.Enabled = false;
           
           if (Statics.FormParam.Demo == DemoStatus.OpenMenu)
            {
                System.Threading.Thread.Sleep(500);
                if (Statics.MenuHide == true) { MediaBoxMenu(); }
                demo.OpenMenu(this);
            }

            if (Statics.FormParam.Demo == DemoStatus.HowToClose)
            { demo.HowToClose(this); }

            if (Statics.FormParam.Demo == DemoStatus.SetDirectories)
            { demo.SetDirectories(this); }

            if (Statics.FormParam.Demo == DemoStatus.UpdateCatalog)
            { demo.UpdateCatalog(this); return; }

            if (Statics.FormParam.Demo == DemoStatus.PersonalizeColor)
            { demo.PersonalizeColor(this); }

            if (Statics.FormParam.Demo == DemoStatus.GeneralSettings)
            { demo.GeneralSettings(this); }

            if (Statics.FormParam.Demo == DemoStatus.Music)
            { demo.Music(this); }

            if (Statics.FormParam.Demo == DemoStatus.Info)
            { demo.Info(this); }

            if (Statics.FormParam.Demo == DemoStatus.Animate)
            { demo.Animate(this); }

            StopDemo();
            
            MessageBox.Show(common.GetTraslation("FinishDemo"),"MediaBox");
        }

        public void StopDemo()
        {
            Common common = new Common();
            common.SetDemo(DemoStatus.NotAvviable);
            DemoPnl.Visible = false;
            LeftPnl.Enabled = true;
            BottomPnl.Enabled = true;
            MinimizeMediaBoxBtn.Enabled = true;
            MaximizeMediaBoxBtn.Enabled = true;
            File.Delete($"{Statics.Constant.MainPath}\\Data\\MediaBoxDemoDB.sqlite");
        }

        public void InitService()
        {
            Services.StartServices(this);
        }

        public void EvievInit()
        {
            Common common = new Common();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            PicturePnl.Top = Statics.Constant.WidthBorder;
            PicturePnl.Left = Statics.Constant.WidthBorder;
            GetConfiguration();
            GetTranslater();
            EvievInitColor();
            PopolatePlayListMusic();
            PopolateMusicPlayList();
        }

        public void GetTranslater()
        {
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            string sql = "SELECT t.ID AS IDTranslate, t.Name, t.IDLanguage, l.CultureName, t.Text FROM Translater t LEFT JOIN Language l ON l.ID = t.IDLanguage";
            DataTable TranslateData = sqLite.ExecuteSelect(sql);

            Statics.Translates = new List<TranslateData>() { };

            foreach (DataRow row in TranslateData.Rows)
            {
                Statics.Translates.Add
                (
                    new TranslateData
                    {
                        IDTranslate = Convert.ToInt32(row["IDTranslate"]),
                        Name = row["Name"].ToString(),
                        IDLanguage = Convert.ToInt32(row["IDLanguage"]),
                        CultureName = row["CultureName"].ToString(),
                        Text = row["Text"].ToString()
                    }
                );
            }

        }

        public void GetConfiguration()
        {
            string[] BackGrondColorMain = new string[3] { "250", "250", "250" };
            string[] ForeColor = new string[3] { "30", "30", "30" };
            string[] BorderColorMedia = new string[3] { "255", "255", "255" };

            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            string sql = "SELECT * FROM Configuration;";
            DataTable configuration = sqLite.ExecuteSelect(sql);

            sql = "SELECT Type, Path FROM Directory;";
            DataTable Directory = sqLite.ExecuteSelect(sql);

            foreach (DataRow row in Directory.Rows)
            {
                Statics.Directories.Add( new DirectoryParam { Type= row["Type"].ToString(), Path = row["Path"].ToString() });
            }

            sqLite.CloseDB();
            if (configuration.Rows.Count > 0)
            {
                Statics.FormParam.Width = Convert.ToInt32(configuration.Rows[0]["WidthMediaBox"]);
                Statics.FormParam.Height = Convert.ToInt32(configuration.Rows[0]?["HeightMediaBox"]);
                Statics.FormParam.Top = Convert.ToInt32(configuration.Rows[0]["TopMediaBox"]);
                Statics.FormParam.Left = Convert.ToInt32(configuration.Rows[0]["LeftMediaBox"]);
                Statics.FormParam.PictureInterval = Convert.ToDouble(configuration.Rows[0]["PictureInterval"]);
                Statics.FormParam.VideoInterval = Convert.ToDouble(configuration.Rows[0]["VideoInterval"]);
                Statics.FormParam.WidthBorderMedia = Convert.ToInt32(configuration.Rows[0]["WidthBorderMedia"]);
                Statics.FormParam.AutoRunAnimationMedia = Convert.ToBoolean(configuration.Rows[0]["AutoRunAnimationMedia"]);
                
                Statics.Constant.DefaultPictureSize = Convert.ToInt32(configuration.Rows[0]["DefaultPictureSize"]);
                Statics.Constant.WidthBorder = Convert.ToInt32(configuration.Rows[0]["WidthBorder"]);
                Statics.Constant.nPicturesOverPanel = Convert.ToInt32(configuration.Rows[0]["nPicturesOverPanel"]);

                Statics.Music.SelectedPlayListName = configuration.Rows[0]["SelectedPlayListName"].ToString();

                Statics.FormParam.VolumeAudio = Convert.ToInt32(configuration.Rows[0]["VolumeAudio"]);

                Statics.FormParam.MediaMovementSpeed = Convert.ToInt32(configuration.Rows[0]["MediaMovementSpeed"]);

                Statics.FormParam.LanguageSelected = (Languages)Enum.ToObject(typeof(Languages),Convert.ToInt32(configuration.Rows[0]["IDLanguage"]));

                SetVolumeAudioPlayer();

                if (configuration.Rows[0]["BackGrondColorMain"].ToString().Length > 0)
                {
                    BackGrondColorMain = configuration.Rows[0]["BackGrondColorMain"].ToString().Split(',');
                }
                if (configuration.Rows[0]["ForeColor"].ToString().Length > 0)
                {
                    ForeColor = configuration.Rows[0]["ForeColor"].ToString().Split(',');
                }
                if (configuration.Rows[0]["BorderColorMedia"].ToString().Length > 0)
                {
                    BorderColorMedia = configuration.Rows[0]["BorderColorMedia"].ToString().Split(',');
                }
            }
            Statics.FormParam.BackGrondColorMain = Color.FromArgb(
                    255,
                    int.Parse(BackGrondColorMain[0]),
                    int.Parse(BackGrondColorMain[1]),
                    int.Parse(BackGrondColorMain[2])
            );
            Statics.FormParam.ForeColor = Color.FromArgb(
                    255,
                    int.Parse(ForeColor[0]),
                    int.Parse(ForeColor[1]),
                    int.Parse(ForeColor[2])
            );
            Statics.FormParam.BorderColorMedia = Color.FromArgb(
                    255,
                    int.Parse(BorderColorMedia[0]),
                    int.Parse(BorderColorMedia[1]),
                    int.Parse(BorderColorMedia[2])
            );          
        }

        private void SetDimensionAndPosition()
        {
            Utility utility = new Utility();
            bool resoluionValid = true;
            int widthSecondScreen = SystemInformation.VirtualScreen.Width - Screen.GetBounds(this).Width;
            int heightSecondScreen = SystemInformation.VirtualScreen.Height - Screen.GetBounds(this).Height;
            int widthAllScreen = SystemInformation.VirtualScreen.Width;
            int heightAllScreen = SystemInformation.VirtualScreen.Height;

            if (
                (Statics.FormParam.Left < 0 && widthSecondScreen < Math.Abs(Statics.FormParam.Left))
                || (Statics.FormParam.Top < 0 && heightSecondScreen < Math.Abs(Statics.FormParam.Top))
                )
            { resoluionValid = false; }

            if (
                widthAllScreen < Statics.FormParam.Width
                || heightAllScreen < Statics.FormParam.Height
                )
            { resoluionValid = false; }

            if (
                (Statics.FormParam.Left) + Statics.FormParam.Width > widthAllScreen
                || (Statics.FormParam.Top) + Statics.FormParam.Top > heightAllScreen
                )
            { resoluionValid = false; }

            if (resoluionValid && Statics.FormParam.Width > 0 && Statics.FormParam.Height > 0)
            {
                this.Width = Statics.FormParam.Width;
                this.Height = Statics.FormParam.Height;
                this.Location = new Point(Statics.FormParam.Left, Statics.FormParam.Top);
            }
            else
            {
                utility.MaximizeOrDefault(this);    
            }
            SetArea();
        }

        public void EvievInitColor()
        {
            Common common = new Common();
            Utility utility = new Utility();
            Additional.ProgressBar progressBar = new Additional.ProgressBar();
            Additional.ProgressBar.StatusOption statusOption = new Additional.ProgressBar.StatusOption();

            Statics.FormParam.BackGrondColorControl = Color.FromArgb(
                    240,
                    Statics.FormParam.BackGrondColorMain.R,
                    Statics.FormParam.BackGrondColorMain.G,
                    Statics.FormParam.BackGrondColorMain.B
            );

            Statics.FormParam.BackGrondColorControlSelected = Color.FromArgb(
                    150,
                    Statics.FormParam.BackGrondColorMain.R,
                    Statics.FormParam.BackGrondColorMain.G,
                    Statics.FormParam.BackGrondColorMain.B
            );

            Statics.FormParam.BackForeColor = Color.FromArgb(
                    100,
                    Statics.FormParam.ForeColor.R,
                    Statics.FormParam.ForeColor.G,
                    Statics.FormParam.ForeColor.B
            );
            progressBar.Create(this.BottomPnl, 2, 0, 200, 20, Statics.FormParam.BackForeColor);

            var ProgressBarPnl = this.Controls["BottomPnl"].Controls["ProgressBarPnl"];

            common.ProgressBar(100, utility.GetCurrentMethod().Name, this, statusOption.Start);
            this.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorMain);
            this.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);

            SetDimensionAndPosition();

            this.Opacity = 100D;
            foreach (Control ControlLevel0 in this.Controls)
            {
                Application.DoEvents();
                if (ControlLevel0.Name == "PicturePnl") { continue; }
                if (ControlLevel0.Name == "MediaBoxMenuBtn")
                {
                    ControlLevel0.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
                    ControlLevel0.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl);
                    continue;
                }
                foreach (Control ControlLevel1 in ControlLevel0.Controls)
                {
                    Application.DoEvents();
                    foreach (Control ControlLevel2 in ControlLevel1.Controls)
                    {
                        Application.DoEvents();
                        foreach (Control ControlLevel3 in ControlLevel2.Controls)
                        {
                            Application.DoEvents();
                            foreach (Control ControlLevel4 in ControlLevel3.Controls)
                            {
                                Application.DoEvents();
                                foreach (Control ControlLevel5 in ControlLevel4.Controls)
                                {
                                    Application.DoEvents();
                                    try
                                    {
                                        ControlLevel5.BackColor = Color.FromArgb(0, 220, 220, 220);
                                    }
                                    catch (Exception) { ControlLevel5.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl); }
                                    ControlLevel5.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
                                }
                                try
                                {
                                    ControlLevel4.BackColor = Color.FromArgb(0, 220, 220, 220);
                                }
                                catch (Exception) { ControlLevel4.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl); }
                                ControlLevel4.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
                            }

                            try
                            {
                                ControlLevel3.BackColor = Color.FromArgb(0, 220, 220, 220);
                            }
                            catch (Exception) { ControlLevel3.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl); }
                            ControlLevel3.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
                        }
                        try
                        {
                            ControlLevel2.BackColor = Color.FromArgb(0, 220, 220, 220);
                        }
                        catch (Exception) { ControlLevel2.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl); }
                        ControlLevel2.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
                    }
                    try
                    {
                        ControlLevel1.BackColor = Color.FromArgb(0, 220, 220, 220);
                    }
                    catch (Exception){ ControlLevel1.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl); }
                    ControlLevel1.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
                }
                ControlLevel0.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControl);
                ControlLevel0.ForeColor = utility.ConvertARGBtoRGB(Statics.FormParam.ForeColor);
            }

            common.ProgressBar(100, utility.GetCurrentMethod().Name, this, statusOption.End);
        }

        private void MaximizeOrDefaultMediaBoxBtn_Click(object sender, EventArgs e)
        {
            Utility utility = new Utility();
            utility.MaximizeOrDefault(this);
            SetArea();
        }

        private void MediaBoxMenuBtn_Click(object sender, EventArgs e)
        {
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart && Statics.FormParam.Demo != DemoStatus.Avviable && Statics.FormParam.Demo != DemoStatus.OpenMenu) { return; }
            MediaBoxMenu();
        }

        private void MediaBoxMenu()
        {
            if (previousStatusHideMenu == null) { return; }
            previousStatusHideMenu = Statics.MenuHide;
            if (Statics.MenuHide == true) { Statics.MenuHide = false; }
            else { Statics.MenuHide = true; }
            SetArea();
            MediaBoxMenuTmr.Enabled = true;
        }

        public void SetArea()
        {
            Statics.FormParam.Width = this.Width;
            Statics.FormParam.Height = this.Height;
            if (Statics.MenuHide == true)
            { Statics.FormParam.WidthArea = this.Width; Statics.FormParam.LeftArea = 0; }
            else { Statics.FormParam.WidthArea = this.Width - this.LeftPnl.Width; Statics.FormParam.LeftArea = this.LeftPnl.Width; }

            if (Statics.MenuHide == true)
            { Statics.FormParam.HeightArea = this.Height; Statics.FormParam.TopArea = 0; }
            else { Statics.FormParam.HeightArea = this.Height - (this.TopPnl.Height + this.BottomPnl.Height); Statics.FormParam.TopArea = (this.TopPnl.Height); }
        }

        public void OpenMediaBoxMenu()
        {  
            if (Statics.MenuHide == true) { Statics.MenuHide = false; MediaBoxMenuTmr.Enabled = true; }
        }

        public void ProgressBarStart()
        {
            this.LeftPnl.Enabled = false;
            OpenMediaBoxMenu();
        }

        public void ProgressBarEnd()
        {
            this.LeftPnl.Enabled = true;
            CloseMediaBoxMenu();
        }

        public void CloseMediaBoxMenu()
        {
            if (Statics.MenuHide == false && previousStatusHideMenu == false) { previousStatusHideMenu = true; Statics.MenuHide = true; MediaBoxMenuTmr.Enabled = true; }
        }

        private void MediaBoxMenuTmr_Tick(object sender, EventArgs e)
        {
            MediaBoxMenuTmr.Enabled = MediaBoxMenuHideShow(); 
        }

        private bool MediaBoxMenuHideShow()
        {
            bool result = true;
            //Application.DoEvents();
            int i = 0;
            if (TopPnl.Top > -TopPnl.Height && Statics.MenuHide == true) { TopPnl.Top -= 10; } else if (Statics.MenuHide == true) { i++; }
            if (LeftPnl.Left > -LeftPnl.Width && Statics.MenuHide == true) { LeftPnl.Left -= 10; } else if (Statics.MenuHide == true) { i++; }
            if (BottomPnl.Top < this.Height && Statics.MenuHide == true) { BottomPnl.Top += 10; } else if (Statics.MenuHide == true) { i++; }

            if (TopPnl.Top < 0 && Statics.MenuHide == false) { TopPnl.Top += 10; } else if (Statics.MenuHide == false) { i++; }
            if (LeftPnl.Left < 0 && Statics.MenuHide == false) { LeftPnl.Left += 10; } else if (Statics.MenuHide == false) { i++; }
            if (BottomPnl.Top > this.Height - BottomPnl.Height && Statics.MenuHide == false) { BottomPnl.Top -= 10; } else if (Statics.MenuHide == false) { i++; }
            if (i == 3) { result = false; }
            return result;
        }

        private void WaitHideMediaBoxMenu_Tick(object sender, EventArgs e)
        {
            waitHideMediaBoxMenu.Enabled = false;
            MediaBoxMenu();
        }

        private void OpenClosePersonalizeColorBtn_Click(object sender, EventArgs e)
        {
            OpenClosePersonalizeColor();
        }

        private void OpenClosePersonalizeColor()
        {
            if (PersonalizeColorPnl.Height == 0)
            {
                Common common = new Common();

                ChoiceColorCmb.Items.Clear();
                ChoiceColorCmb.Items.Add(common.GetTraslation("ColorTypeBackGrondColorMain"));
                ChoiceColorCmb.Items.Add(common.GetTraslation("ColorTypeForeColor"));
                ChoiceColorCmb.Items.Add(common.GetTraslation("ColorTypeBorderColorMedia"));
                ChoiceColorCmb.Text = common.GetTraslation("ColorTypeDefault");


                RedLbl.Text = common.GetTraslation(RedLbl.Name);
                GreenLbl.Text = common.GetTraslation(GreenLbl.Name);
                BlueLbl.Text = common.GetTraslation(BlueLbl.Name);

                Utility utility = new Utility();

                CreateScrollBarPersonalizeColor();

                OpenMenuPanel(PersonalizeColorPnl);
            }
            else { CloseMenuPanel(PersonalizeColorPnl); }
        }

        private void CreateScrollBarPersonalizeColor()
        {
            PersonalizeColorRedPnl.Controls.Clear();
            PersonalizeColorGreenPnl.Controls.Clear();
            PersonalizeColorBluePnl.Controls.Clear();

            ScrollBarParameters parametersScrollBarPersonalizeColorRed = new ScrollBarParameters { form = this, container = PersonalizeColorRedPnl, name = "PersonalizeColorRed", percentageValue = 255, top = 0, left = 0, width = 20, height = 295, color = Statics.FormParam.BackGrondColorControlSelected, foreColor = Statics.FormParam.ForeColor, type = ScrollBarType.Incremental };
            scrollBarPersonalizeColorRed.CreateVertical(parametersScrollBarPersonalizeColorRed);

            ScrollBarParameters parametersScrollBarPersonalizeColorGreen = new ScrollBarParameters { form = this, container = PersonalizeColorGreenPnl, name = "PersonalizeColorGreen", percentageValue = 255, top = 0, left = 0, width = 20, height = 295, color = Statics.FormParam.BackGrondColorControlSelected, foreColor = Statics.FormParam.ForeColor, type = ScrollBarType.Incremental };
            scrollBarPersonalizeColorGreen.CreateVertical(parametersScrollBarPersonalizeColorGreen);

            ScrollBarParameters parametersScrollBarPersonalizeColorBlue = new ScrollBarParameters { form = this, container = PersonalizeColorBluePnl, name = "PersonalizeColorBlue", percentageValue = 255, top = 0, left = 0, width = 20, height = 295, color = Statics.FormParam.BackGrondColorControlSelected, foreColor = Statics.FormParam.ForeColor, type = ScrollBarType.Incremental };
            scrollBarPersonalizeColorBlue.CreateVertical(parametersScrollBarPersonalizeColorBlue);

            InitscrollBarPersonalizeColor();
        }

        private void OpenMenuPanel(Panel pnl)
        {
            Utility utility = new Utility();
            Control Separator = utility.GetControlByChildName(pnl, "Separator");
            int x = 0;
            while (x < Separator.Top + 50)
            {
                Application.DoEvents();
                x += 10;
                if (x > Separator.Top + 50) { x = Separator.Top + 50; }
                pnl.Height = x;
                RepositionButtonMenu();
                utility.Sleep(1);
            }
        }

        private void CloseMenuPanel(Panel pnl)
        {
            Utility utility = new Utility();
            Control Separator = utility.GetControlByChildName(pnl, "Separator");
            int x = pnl.Height;
            while (x > 0)
            {
                Application.DoEvents();
                x -= 10;
                if (x < 0) { x = 0; }
                pnl.Height = x;
                RepositionButtonMenu();
                utility.Sleep(5);
            }
        }

        private void InitscrollBarPersonalizeColor()
        {
            GetColor();
            scrollBarPersonalizeColorRed.SetValue(this, "PersonalizeColorRed", CurrentPersonalizeColor.R, ScrollBarOrientation.Vertical);
            scrollBarPersonalizeColorGreen.SetValue(this, "PersonalizeColorGreen", CurrentPersonalizeColor.G, ScrollBarOrientation.Vertical);
            scrollBarPersonalizeColorBlue.SetValue(this, "PersonalizeColorBlue", CurrentPersonalizeColor.B, ScrollBarOrientation.Vertical);
        }

        public static void SetCurrentPersonalizeColor(int R, int G, int B)
        {
            CurrentPersonalizeColor = Color.FromArgb
            (
               255,
               R,
               G,
               B
            );
            Statics.MainForm.Controls["LeftPnl"].Controls["PersonalizeColorPnl"].Controls["ViewColor"].BackColor = CurrentPersonalizeColor;
        }

        private static void SetViewColor()
        {
            Statics.MainForm.Controls["LeftPnl"].Controls["PersonalizeColorPnl"].Controls["ViewColor"].BackColor = Color.FromArgb
            (
                255,
                CurrentPersonalizeColor.R,
                CurrentPersonalizeColor.G,
                CurrentPersonalizeColor.B
            );
        }

        private void ChoiceColorCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitscrollBarPersonalizeColor();
        }

        private void GetColor()
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            if (ChoiceColorCmb.SelectedIndex == (int)ColorType.BackGrondColorMain)
            {
                red = Statics.FormParam.BackGrondColorMain.R;
                green = Statics.FormParam.BackGrondColorMain.G;
                blue = Statics.FormParam.BackGrondColorMain.B;
            }
            if (ChoiceColorCmb.SelectedIndex == (int)ColorType.ForeColor)
            {
                red = Statics.FormParam.ForeColor.R;
                green = Statics.FormParam.ForeColor.G;
                blue = Statics.FormParam.ForeColor.B;
            }

            if (ChoiceColorCmb.SelectedIndex == (int)ColorType.BorderColorMedia)
            {
                red = Statics.FormParam.BorderColorMedia.R;
                green = Statics.FormParam.BorderColorMedia.G;
                blue = Statics.FormParam.BorderColorMedia.B;
            }

            CurrentPersonalizeColor = Color.FromArgb
            (
                255,
                red,
                green,
                blue
            );
        }

        private void SetColorBtn_Click(object sender, EventArgs e)
        {
            string sql = String.Empty;
            if (ChoiceColorCmb.SelectedIndex == (int)ColorType.BackGrondColorMain)
            {
                Statics.FormParam.BackGrondColorMain = Color.FromArgb(
                    255,
                    CurrentPersonalizeColor.R,
                    CurrentPersonalizeColor.G,
                    CurrentPersonalizeColor.B
                );
                sql = $"UPDATE `Configuration` SET `BackGrondColorMain`= '{Statics.FormParam.BackGrondColorMain.R},{Statics.FormParam.BackGrondColorMain.G},{Statics.FormParam.BackGrondColorMain.B}' WHERE ID = 1;";
            }
            else if (ChoiceColorCmb.SelectedIndex == (int)ColorType.ForeColor)
            {
                Statics.FormParam.ForeColor = Color.FromArgb(
                    255,
                    CurrentPersonalizeColor.R,
                    CurrentPersonalizeColor.G,
                    CurrentPersonalizeColor.B
                );
                sql = $"UPDATE `Configuration` SET `ForeColor`= '{Statics.FormParam.ForeColor.R},{Statics.FormParam.ForeColor.G},{Statics.FormParam.ForeColor.B}' WHERE ID = 1;";
            }
            else if (ChoiceColorCmb.SelectedIndex == (int)ColorType.BorderColorMedia)
            {
                Statics.FormParam.BorderColorMedia = Color.FromArgb(
                    255,
                    CurrentPersonalizeColor.R,
                    CurrentPersonalizeColor.G,
                    CurrentPersonalizeColor.B
                );
                sql = $"UPDATE `Configuration` SET `BorderColorMedia`= '{Statics.FormParam.BorderColorMedia.R},{Statics.FormParam.BorderColorMedia.G},{Statics.FormParam.BorderColorMedia.B}' WHERE ID = 1;";
            }
            else { MessageBox.Show(ChoiceColorCmb.Text); return; }
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            sqLite.ExecuteCommand(sql);
            sqLite.CloseDB();
            EvievInitColor();
            CreateScrollBarPersonalizeColor();
            Common common = new Common();
            common.ChangeLogo(this);
        }

        public void Restart()
        {
            Statics.FormParam.Restarting = true;
            CloseMediaBox();
            Application.Restart();
        }

        private void ChoiceColorCmb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void OpenCloseManageDirectoryBtn_Click(object sender, EventArgs e)
        {
            OpenCloseManageDirectory();
        }

        private void OpenCloseManageDirectory()
        {
            DirectoryType directoryType = new DirectoryType();
            if (ManageDirectoryPnl.Height == 0)
            {
                DirectoryList.Items.Clear();
                foreach (DirectoryParam item in Statics.Directories)
                {
                    DirectoryList.Items.Add(item.Type + ", " + item.Path);
                }

                OpenMenuPanel(ManageDirectoryPnl);
            }
            else { CloseMenuPanel(ManageDirectoryPnl); }
        }

        private void DirectoryAddBtn_Click(object sender, EventArgs e)
        {
            DirectoryType directoryType = new DirectoryType();
            DirectoryList.Items.Add(directoryType.Catalog + ", " + DirectoryTxt.Text);
            string sql = String.Empty;
            DeleteAllDirectory();
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            string type = String.Empty;
            string path = String.Empty;
            foreach (string item in DirectoryList.Items)
            {
                if (item != String.Empty)
                {
                    type = item.Split(',')[0].Trim();
                    path = item.Split(',')[1].Trim();
                    if (type != String.Empty && path != String.Empty)
                    {
                        Statics.Directories.Add(new DirectoryParam { Type = type, Path = path });
                        sql = $"INSERT INTO `Directory` (`Type`,`Path`) VALUES ('{type}','{path}');";
                        sqLite.ExecuteCommand(sql);
                    }
                }
            }
            sqLite.CloseDB();
            UpdateDirectory();
        }

        private void UpdateDirectory()
        {
            Statics.Directories = new List<DirectoryParam>() { };
            foreach (string item in DirectoryList.Items)
            {
                if (item != String.Empty)
                {
                    Statics.Directories.Add(new DirectoryParam { Type = item.Split(',')[0].Trim(), Path = item.Split(',')[1].Trim() });
                }
            }
        }

        private void DirectoryOpenBtn_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    DirectoryTxt.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void DirectoryDelBtn_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(DirectoryList);
            selectedItems = DirectoryList.SelectedItems;
            if (DirectoryList.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                {
                    DeleteAllDirectory($"Type = '{selectedItems[i].ToString().Split(',')[0].Trim()}' AND Path = '{selectedItems[i].ToString().Split(',')[1].Trim()}'");
                    DirectoryList.Items.Remove(selectedItems[i]);
                }
            }
            UpdateDirectory();
        }

        private void DeleteAllDirectory(string where = null)
        {
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            if (where != null) { where = " WHERE " + where; }
            else { where = String.Empty; }
            string sql = $"DELETE FROM Directory{where};";
            sqLite.ExecuteCommand(sql);
            sqLite.CloseDB();
        }

        private void RepositionButtonMenu()
        {
            int top = 0;
            var obj  = (dynamic) null;
            top = 5;
            obj = OpenCloseManageDirectoryBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 0;
            obj = ManageDirectoryPnl;
            obj.Top = top;

            top = obj.Top + obj.Height + 5;
            obj = UpdateCatalogBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 5;
            obj = OpenClosePersonalizeColorBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 0;
            obj = PersonalizeColorPnl;
            obj.Top = top;

            top = obj.Top + obj.Height + 5;
            obj = OpenCloseGeneralSettingsBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 0;
            obj = GeneralSettingsPnl;
            obj.Top = top;

            top = obj.Top + obj.Height + 5;
            obj = StartStopAnimateBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 5;
            obj = OpenCloseMusicBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 0;
            obj = MusicPnl;
            obj.Top = top;

            top = obj.Top + obj.Height + 5;
            obj = OpenCloseInfoBtn;
            obj.Top = top;

            top = obj.Top + obj.Height + 0;
            obj = InfoPnl;
            obj.Top = top;
        }

        private void MediaBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Statics.FormParam.Restarting == true) { return; }
            CloseMediaBox();
        }

        private void CloseMediaBox()
        {
            Services.active = false;
            UpdateDimensionAndPosition();
        }

        private void UpdateDimensionAndPosition()
        {
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `WidthMediaBox`={this.Width}, `HeightMediaBox`={this.Height}, `TopMediaBox`={this.Location.Y}, `LeftMediaBox`={this.Location.X} WHERE ID = 1;");
            sqLite.CloseDB();
        }

        private void OpenCloseGeneralSettingsBtn_Click(object sender, EventArgs e)
        {
            OpenCloseGeneralSettings();
        }

        private void OpenCloseGeneralSettings()
        {
            Common common = new Common();
            Utility utility = new Utility();
            if (GeneralSettingsPnl.Height == 0)
            {
                if (Statics.FormParam.Demo == DemoStatus.NotAvviable)
                    SetLanguage(Statics.FormParam.LanguageSelected);

                PictureIntervalLbl.Text = common.GetTraslation(PictureIntervalLbl.Name);
                VideoIntervalLbl.Text = common.GetTraslation(VideoIntervalLbl.Name);
                WidthBorderMediaLbl.Text = common.GetTraslation(WidthBorderMediaLbl.Name);
                MediaMovementSpeedLbl.Text = common.GetTraslation(MediaMovementSpeedLbl.Name);
                AutoRunAnimationMediaLbl.Text = common.GetTraslation(AutoRunAnimationMediaLbl.Name);
                

                PictureIntervalTxt.Text = Statics.FormParam.PictureInterval.ToString();
                VideoIntervalTxt.Text = Statics.FormParam.VideoInterval.ToString();
                WidthBorderMediaTxt.Text = Statics.FormParam.WidthBorderMedia.ToString();

                if (Statics.FormParam.AutoRunAnimationMedia == true)
                { AutoRunAnimationMediaBtn.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControlSelected); }
                else { AutoRunAnimationMediaBtn.BackColor = Color.Transparent; }

                ScrollBarParameters parametersScrollBarMediaMovementSpeed = new ScrollBarParameters { form = this, container = MediaMovementSpeedPnl, name = "MediaMovementSpeed", percentageValue = 100, top = 0, left = 0, width = 118, height = 20, color = Statics.FormParam.BackGrondColorControlSelected, foreColor = Statics.FormParam.ForeColor, type = ScrollBarType.Incremental };

                scrollBarMediaMovementSpeed.CreateHorizontal(parametersScrollBarMediaMovementSpeed);
                scrollBarMediaMovementSpeed.SetValue(this, "MediaMovementSpeed", Statics.FormParam.MediaMovementSpeed, ScrollBarOrientation.Horizontal);


                OpenMenuPanel(GeneralSettingsPnl);
            }
            else { CloseMenuPanel(GeneralSettingsPnl); }
        }

        private void OpenCloseMusicBtn_Click(object sender, EventArgs e)
        {
            OpenCloseMusic();
        }

        private void OpenCloseInfoBtn_Click(object sender, EventArgs e)
        {
            OpenCloseInfo();
        }

        private void OpenCloseMusic()
        {
            if (MusicPnl.Height == 0)
            {
                PlayListMusicCmb.Text = Statics.Music.SelectedPlayListName;
                PopolatePlayListMusic();
                Utility utility = new Utility();

                ScrollBarParameters parametersScrollBarAudio = new ScrollBarParameters { form = this, container = AudioPnl, name = "Audio", percentageValue = 100, top = 0, left = 0, width = 118, height = 20, color = Statics.FormParam.BackGrondColorControlSelected, foreColor = Statics.FormParam.ForeColor, type = ScrollBarType.Incremental };

                scrollBarAudio.CreateHorizontal(parametersScrollBarAudio);
                scrollBarAudio.SetValue(this, "Audio", Statics.FormParam.VolumeAudio, ScrollBarOrientation.Horizontal);


                OpenMenuPanel(MusicPnl);
            }
            else { CloseMenuPanel(MusicPnl); }
        }

        public void PopolatePlayListMusic()
        {
            PlayListMusicCmb.Items.Clear();
            PlayListMusicCmb.Items.Add("# No PlayList");
            IWMPPlaylistArray playListArray = MusicPlayer.playlistCollection.getAll();

            if (playListArray == null) { return; }
            for (int i = 0; i < playListArray.count; i++)
            {
                try
                {
                    if (playListArray.Item(i).getItemInfo("PlaylistType") != "Auto")
                    {
                        PlayListMusicCmb.Items.Add(playListArray.Item(i).name);
                    }
                }
                catch (Exception)
                {}
            }
        }

        private void OpenCloseInfo()
        {
            if (InfoPnl.Height == 0)
            {
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                VersionLbl.Text = "Version: " + version.ToString();
                CreatedBy_1Lbl.Text = "Created by:";
                CreatedBy_2Lbl.Text = "Daniele Marassi";
                OpenMenuPanel(InfoPnl);
            }
            else { CloseMenuPanel(InfoPnl); }
        }

        private void MediaBox_Resize(object sender, EventArgs e)
        {
            PicturePnl.Top = Statics.Constant.WidthBorder;
            PicturePnl.Left = Statics.Constant.WidthBorder;
        }

        private void StartStopAnimateBtn_Click(object sender, EventArgs e)
        {
            StartStopAnimate();
        }

        public void StartStopAnimate()
        {
            Common common = new Common();
            Utility utility = new Utility();
            Presentation presentation = new Presentation();
            presentation.ActiveDeactivatePresentation();
            presentation.StartStopPresentationTmr();
            
            if (Statics.Presentation.Active == false)
            {
                PresentationTmr.Enabled = false;
                Statics.Presentation.Running = false;
                PictureBox pictureBox = null;
                pictureBox = (PictureBox)Statics.MainForm.Controls["PicturePnl"].Controls[$"PictureBox_{Statics.Presentation.Between}"];
            }

            if (Statics.Presentation.Start == true)
            {
                PresentationTmr.Enabled = true;
                presentation.NextMedia();
                PresentationTmr.Interval = 100;
            }
            
            if (Statics.Presentation.Active == true)
            {
                StartStopAnimateBtn.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControlSelected);
                //StartStopAnimateMediaBtn.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControlSelected);
            }
            else
            {
                StartStopAnimateBtn.BackColor = Color.Transparent;
                //StartStopAnimateMediaBtn.BackColor = Color.Transparent;
            }
        }

        private void PresentationTmr_Tick(object sender, EventArgs e)
        {
            if (Math.Abs(Statics.Presentation.Between - Statics.Presentation.Next) > 9) { PresentationTmr.Interval = 100; }
            if (Math.Abs(Statics.Presentation.Between - Statics.Presentation.Next) == 9) { PresentationTmr.Interval = 300; }
            if (Math.Abs(Statics.Presentation.Between - Statics.Presentation.Next) == 6) { PresentationTmr.Interval = 400; }
            if (Math.Abs(Statics.Presentation.Between - Statics.Presentation.Next) == 3) { PresentationTmr.Interval = 500; }

            if (Statics.Presentation.Start == true)
            {
                Presentation presentation = new Presentation();
                presentation.StartStopPresentationTmr();
                Statics.MainForm = this;
                bool founded = presentation.Execution(this);
                if (founded == false && Statics.Presentation.Active == true) { presentation.StartStopPresentationTmr(); }
            }
        }

        private void ResizeMediaBoxLbl_MouseDown(object sender, MouseEventArgs e)
        {
            Statics.Mouse.Down = true;
            Statics.Position = e.Location;
        }

        private void ResizeMediaBoxLbl_MouseMove(object sender, MouseEventArgs e)
        {
            int width = (Cursor.Position.X - (this.Location.X - Statics.Position.X));
            int height = (Cursor.Position.Y - (this.Location.Y - Statics.Position.Y));
            if (Statics.Mouse.Down && width > 600 && height > 150)
            {
                this.Width = width;
                this.Height = height;
                SetArea();
                this.Update();
                PicturePnl.AutoSize = true;
                PicturePnl.Update();
            }
        }

        private void ResizeMediaBoxLbl_MouseUp(object sender, MouseEventArgs e)
        {
            Statics.Mouse.Down = false;
        }

        private void DirectoryTypeCmb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void PlayStopMusicBtn_Click(object sender, EventArgs e)
        {
            PlayStopMusic(false);
        }

        public bool PlayListNotExists()
        {
            bool notExists = false;
            if (PlayListMusicCmb.Items.Count == 0) { notExists = true; MessageBox.Show("You must first create a playlist, open Windows Media Player and create list."); }
            return notExists;
        }

        public bool PlayListNotSelected()
        {
            bool notSelected = false;
            if (Statics.Music.SelectedPlayListName == "") { notSelected = true; MessageBox.Show("You must first select a playlist."); }
            return notSelected;
        }

        public void PlayStopMusic(bool stop)
        {
            //bool notExists = PlayListNotExists();
            //if (notExists) { return; }
            //bool notSelected = PlayListNotSelected();
            //if (notSelected) { return; }
            Common common = new Common();
            Utility utility = new Utility();

            if (Statics.Music.Play == true || stop == true)
            { Statics.Music.Play = false; PlayStopMusicBtn.BackColor = Color.Transparent; PlayStopMusicBtn.Text = "4"; MusicPlayer.Ctlcontrols.stop(); }
            else
            {
                IWMPPlaylistArray playListArray = MusicPlayer.playlistCollection.getByName(Statics.Music.SelectedPlayListName);
                if (playListArray.count > 0)
                {
                    Statics.Music.Play = true;
                    PlayStopMusicBtn.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControlSelected);
                    PlayStopMusicBtn.Text = "<";
                    MusicPlayer.Ctlcontrols.play();
                }
            }
        }

        private void AutoRunAnimationMedia()
        {
            Common common = new Common();
            Utility utility = new Utility();
            if (Statics.FormParam.AutoRunAnimationMedia == true)
            { Statics.FormParam.AutoRunAnimationMedia = false; }
            else { Statics.FormParam.AutoRunAnimationMedia = true; }

            if (Statics.FormParam.AutoRunAnimationMedia == true)
            { AutoRunAnimationMediaBtn.BackColor = utility.ConvertARGBtoRGB(Statics.FormParam.BackGrondColorControlSelected); }
            else { AutoRunAnimationMediaBtn.BackColor = Color.Transparent; }
        }

        private void AutoRunAnimationMediaBtn_Click(object sender, EventArgs e)
        {
            AutoRunAnimationMedia();
        }

        private void BackMusicBtn_Click(object sender, EventArgs e)
        {
            bool notExists = PlayListNotExists();
            if (notExists) { return; }
            bool notSelected = PlayListNotSelected();
            if (notSelected) { return; }
            MusicPlayer.Ctlcontrols.previous();
        }

        private void ForwardMusicBtn_Click(object sender, EventArgs e)
        {
            bool notExists = PlayListNotExists();
            if (notExists) { return; }
            bool notSelected = PlayListNotSelected();
            if (notSelected) { return; }
            MusicPlayer.Ctlcontrols.next();
        }

        public void PopolateMusicPlayList()
        {
            IWMPPlaylistArray playListArray = MusicPlayer.playlistCollection.getByName(Statics.Music.SelectedPlayListName);
            if (playListArray.count > 0)
            {
                IWMPPlaylist playList = playListArray.Item(0);
                MusicPlayer.currentPlaylist = playList;
                Common common = new Common();
                Utility utility = new Utility();
                utility.Sleep(100);
                MusicPlayer.Ctlcontrols.stop();
                if (Statics.FormParam.AutoRunAnimationMedia == true && playList.count > 0) { PlayStopMusic(false); }
            }
            else
            {
                PlayStopMusic(true);
            }
            //MusicPlayer.Ctlenabled = false;
        }

        private void PlayListMusicCmb_SelectedValueChanged(object sender, EventArgs e)
        {
            if (PlayListMusicCmb_Enabled == false) { return; }
            Statics.Music.SelectedPlayListName = PlayListMusicCmb.Text;
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `SelectedPlayListName`='{Statics.Music.SelectedPlayListName}' WHERE ID = 1;");
            sqLite.CloseDB();
            PopolateMusicPlayList();
        }

        private void PlayListMusicCmb_Leave(object sender, EventArgs e)
        {
            PlayListMusicCmb_Enabled = false;
        }

        private void PlayListMusicCmb_Enter(object sender, EventArgs e)
        {
            PlayListMusicCmb_Enabled = true;
        }

        public static void SetVolumeAudioPlayer(bool save = false , dynamic valueVolumeAudio = null)
        {
            if(valueVolumeAudio != null)
            {
                Statics.FormParam.VolumeAudio = valueVolumeAudio;
            }

            AxWMPLib.AxWindowsMediaPlayer _MusicPlayer = (AxWMPLib.AxWindowsMediaPlayer)Statics.MainForm.Controls["LeftPnl"].Controls["MusicPnl"].Controls["MusicPlayer"];
            _MusicPlayer.settings.volume = Statics.FormParam.VolumeAudio;

            if (save == true)
            {
                SqLite sqLite = new SqLite();
                if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
                { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
                else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
                sqLite.ExecuteCommand($"UPDATE `Configuration` SET `VolumeAudio`={Statics.FormParam.VolumeAudio} WHERE ID = 1;");
                sqLite.CloseDB();
            }
        }

        private void DemoBtn_Click(object sender, EventArgs e)
        {
            Statics.FormParam.Demo = DemoStatus.ReStart;
            ShowDemo();
        }

        private void ShowDemo()
        {
            Common common = new Common();
            DemoPnl.Top = Statics.FormParam.TopArea;
            DemoPnl.Left = Statics.FormParam.LeftArea;
            DemoPnl.Width = Statics.FormParam.WidthArea;
            DemoPnl.Height = Statics.FormParam.HeightArea;
            SetDemoDB();
            DurationLblDemo.Text = common.GetTraslation(DurationLblDemo.Name);
            DemoPnl.Visible = true;
            SetLanguageDemo(Statics.FormParam.LanguageSelected);
            DemoPnl.BringToFront();
            TopPnl.BringToFront();
            LeftPnl.BringToFront();
            BottomPnl.BringToFront();
            MediaBoxMenuBtn.BringToFront();
        }

        private void StartDemoBtn_Click(object sender, EventArgs e)
        {
            Common common = new Common();
            SetArea();
            UpdateDimensionAndPosition();
            common.SetDemo(DemoStatus.OpenMenu);
            UpdateDimensionAndPosition();
            Restart();
        }

        private void ExitDemoBtn_Click(object sender, EventArgs e)
        {
            StopDemo();
        }

        private void LanguageitITBtn_Click(object sender, EventArgs e)
        {
            SetLanguage(Languages.itIT);
        }

        private void LanguageenUSBtn_Click(object sender, EventArgs e)
        {
            SetLanguage(Languages.enUS);
        }

        public void SetLanguage(Languages language)
        {
            
            if (language == Languages.itIT)
            {
                LanguageitITLbl.BackColor = Statics.FormParam.ForeColor;
                LanguageenUSLbl.BackColor = Color.Transparent;
                Statics.FormParam.LanguageSelected = language;
            }
            if (language == Languages.enUS)
            {
                LanguageitITLbl.BackColor = Color.Transparent;
                LanguageenUSLbl.BackColor = Statics.FormParam.ForeColor;
                Statics.FormParam.LanguageSelected = language;
            } 
        }

        private void SaveLanguage()
        {
            SqLite sqLite = new SqLite();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `IDLanguage`={((int)Statics.FormParam.LanguageSelected).ToString()} WHERE ID = 1;");
            sqLite.CloseDB();
        }

        private void LanguageitITDemoBtn_Click(object sender, EventArgs e)
        {
            SetLanguageDemo(Languages.itIT);
        }

        private void LanguageenUSDemoBtn_Click(object sender, EventArgs e)
        {
            SetLanguageDemo(Languages.enUS); 
        }

        public void SetLanguageDemo(Languages language)
        {
            Common common = new Common();
            if (language == Languages.itIT)
            {
                LanguageitITDemoLbl.BackColor = Statics.FormParam.ForeColor;
                LanguageenUSDemoLbl.BackColor = Color.Transparent;
                Statics.FormParam.LanguageSelected = language;
            }
            if (language == Languages.enUS)
            {
                LanguageitITDemoLbl.BackColor = Color.Transparent;
                LanguageenUSDemoLbl.BackColor = Statics.FormParam.ForeColor;
                Statics.FormParam.LanguageSelected = language;
            }
            DurationLblDemo.Text = common.GetTraslation(DurationLblDemo.Name);
            SaveLanguageDemo();

        }

        private void SaveLanguageDemo()
        {
            SqLite sqLite = new SqLite();
            sqLite.OpenDB(Statics.Constant.FilePathDemoDB); 
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `IDLanguage`={((int)Statics.FormParam.LanguageSelected).ToString()} WHERE ID = 1;");
            sqLite.CloseDB();
        }

        private void SetGeneralSettingsBtn_Click(object sender, EventArgs e)
        {
            SqLite sqLite = new SqLite();

            Statics.FormParam.PictureInterval = double.Parse(PictureIntervalTxt.Text.Replace('.', ','));
            Statics.FormParam.VideoInterval = double.Parse(VideoIntervalTxt.Text.Replace('.', ','));
            Statics.FormParam.WidthBorderMedia = int.Parse(WidthBorderMediaTxt.Text);

            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `PictureInterval`={Statics.FormParam.PictureInterval.ToString().Replace(',', '.')} WHERE ID = 1;");
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `VideoInterval`={Statics.FormParam.VideoInterval.ToString().Replace(',', '.')} WHERE ID = 1;");
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `WidthBorderMedia`={Statics.FormParam.WidthBorderMedia.ToString()} WHERE ID = 1;");
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `AutoRunAnimationMedia`={Convert.ToInt32(Statics.FormParam.AutoRunAnimationMedia).ToString()} WHERE ID = 1;");
            sqLite.ExecuteCommand($"UPDATE `Configuration` SET `MediaMovementSpeed`={Convert.ToInt32(Statics.FormParam.MediaMovementSpeed).ToString()} WHERE ID = 1;");
            sqLite.CloseDB();
            SetToolTips();
            SaveLanguage();
        }

        public void StartStopAnimateMedia()
        {
            StartStopAnimate();
        }
    }
}
