using Additional;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MediaBox
{
    public class Media
    {
        public static System.Timers.Timer OpenMediaTmr = null;
        public static System.Timers.Timer CloseMediaTmr = null;
        public static System.Timers.Timer ShowHideTitleTmr = null;
        private static int LeftTopOpenMedia = 0;
        private static int AmountIncrement = 0;
        private static bool showTitle = true;
        private static ShadowFrm shadowFrm = new ShadowFrm();
        private static MediaFrm mediaFrm = new MediaFrm();
        private static WaitFrm waitFrm = new WaitFrm();
        private static TitleMedia titleMedia = new TitleMedia();
        private static int durationInSeconds = 0;

        public bool InizializeAnimate(Form form, int top, int left, int width, int height)
        {
            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
            DataRow row = Statics.DB.TblCatalog.Data.Rows[Statics.Media.Index];
            bool exists = true;
            if (!File.Exists(row["PathFile"].ToString()))
            {
                if (Statics.Presentation.Active == true)
                {
                    Presentation presentation = new Presentation();
                    presentation.NextMedia();
                }
                exists = false;
                return exists;
            }

            Statics.Media.Title = row["Title"].ToString();
            Statics.Media.Type = row["Type"].ToString();
            if (width >= height) { Statics.Media.SizeThumbnail = width; }
            else { Statics.Media.SizeThumbnail = height; }
            
            if (Statics.Media.Type == "Image")
            {
                int amountStauration = 0;
                using (Bitmap bitmpaFromFile = (Bitmap)Image.FromFile(row["PathFile"].ToString()))
                {
                    Statics.Media.BitmapMediaList = new List<Bitmap>() { };
                    for (int i = 0; i <= 100; i++)
                    {
                        Application.DoEvents();
                        if (i >= 50 ) { amountStauration += 2; }
                        Statics.Media.BitmapMediaList.Add
                        (
                            new Bitmap
                            (
                                multimediaCatalog.GetThumbnail(bitmpaFromFile, Statics.Media.SizeThumbnail, 100 - i, amountStauration, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, Statics.FormParam.WidthBorderMedia)
                            )
                        );
                        if (i == 100)
                        {
                            Statics.Media.Width = Statics.Media.BitmapMediaList[100].Width;
                            Statics.Media.Height = Statics.Media.BitmapMediaList[100].Height;
                            Statics.Media.Top = form.Controls["PicturePnl"].Location.Y + top;
                            Statics.Media.Left = form.Controls["PicturePnl"].Location.X + left;
                        }
                    }
                }
            }
            if (Statics.Media.Type == "Video")
            {
                Statics.Media.Width = int.Parse(row["WidthMedia"].ToString()) - 0;
                Statics.Media.Height = int.Parse(row["HeightMedia"].ToString()) - 0;
                Statics.Media.Top = form.Controls["PicturePnl"].Location.Y + top;
                Statics.Media.Left = form.Controls["PicturePnl"].Location.X + left;
            }
            return exists;
        }
    
        public void OpenMedia(int index, Form form, int top, int left, int width, int height, bool animateOpen)
        {
            titleMedia.ShowTitle(String.Empty, (Statics.FormParam.LeftArea + Statics.MainForm.Left), (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height), Statics.FormParam.WidthArea);
            titleMedia.CloseTitle();
            StartWait();
            Common common = new Common();
            Utility utility = new Utility();
            var rnd = new Random(DateTime.Now.Millisecond);

            LeftTopOpenMedia = rnd.Next(0, 2);
            if (LeftTopOpenMedia == 2) { LeftTopOpenMedia = 1; }

            AmountIncrement = rnd.Next(2, 7);
            if (AmountIncrement == 7) { AmountIncrement = 6; }

            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
            DataRow row = Statics.DB.TblCatalog.Data.Rows[index];
            Statics.Media.Type = row["Type"].ToString();

            Statics.Media.Title = row["Title"].ToString();

            string pathFile = row["PathFile"].ToString();
            if (!File.Exists(pathFile)) { return; }
            if (width >= height) { Statics.Media.SizeThumbnail = width; }
            else { Statics.Media.SizeThumbnail = height; }

            double _durationInSeconds = Math.Round(double.Parse(row["DurationInSeconds"].ToString()));

            durationInSeconds = (int) _durationInSeconds;

            try
            {
                mediaFrm.Visible = false;
                mediaFrm.Width = 0;
                mediaFrm.Height = 0;
                mediaFrm.Show();
            }
            catch (Exception ex)
            {
                mediaFrm = new MediaFrm();
                mediaFrm.Visible = false;
                mediaFrm.Width = 0;
                mediaFrm.Height = 0;
                mediaFrm.Show();
            }
            Statics.MediaForm = mediaFrm;
            
            mediaFrm.TopMost = true;
            mediaFrm.TopMost = false;
            mediaFrm.Top = form.Top + top + (form.Controls["PicturePnl"].Location.Y) + Statics.FormParam.TopArea;
            mediaFrm.Left = form.Left + left + (form.Controls["PicturePnl"].Location.X) + Statics.FormParam.LeftArea;

            mediaFrm.Controls["MediaPnl"].Width = mediaFrm.Width;
            mediaFrm.Controls["MediaPnl"].Height = mediaFrm.Height;
            mediaFrm.Controls["MediaPnl"].Top = 0;
            mediaFrm.Controls["MediaPnl"].Left = 0;
            mediaFrm.Controls["MediaPnl"].Visible = false;
            mediaFrm.Controls["MediaPnl"].AutoSize = false;
            mediaFrm.Controls["MaximizeOrDefaultMediaBtn"].Visible = false;
            mediaFrm.Controls["StartStopAnimateMediaBtn"].Visible = false;

            if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
            {
                mediaFrm.Controls["MaximizeOrDefaultMediaBtn"].Visible = false;

                mediaFrm.Controls["StartStopAnimateMediaBtn"].Visible = false;

                PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];
                PictureMedia.Visible = false;
                PictureMedia.Top = 0;
                PictureMedia.Left = 0;
                PictureMedia.Width = 0;
                PictureMedia.Height = 0;
                AxWMPLib.AxWindowsMediaPlayer MediaPlayer = (AxWMPLib.AxWindowsMediaPlayer)mediaFrm.Controls["MediaPnl"].Controls["MediaPlayer"];
                MediaPlayer.settings.volume = Statics.FormParam.VolumeAudio;
                MediaPlayer.Visible = true;
                MediaPlayer.Top = 0;
                MediaPlayer.Left = 0;
                Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Convert.ToInt32(row["WidthMedia"].ToString()), Convert.ToInt32(row["HeightMedia"].ToString()), Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);

                MediaPlayer.Width = dimension["Width"] - (Statics.FormParam.WidthBorderMedia * 2);
                MediaPlayer.Height = dimension["Height"] - (Statics.FormParam.WidthBorderMedia * 2);
                MediaPlayer.AutoSize = true;

                try
                {
                    MediaPlayer.URL = pathFile;
                }
                catch (Exception)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return;
                }
                
                //MediaPlayer.settings.setMode("loop", true);
                MediaPlayer.Ctlcontrols.stop();                
                if (animateOpen == false)
                {
                    mediaFrm.Controls["MaximizeOrDefaultMediaBtn"].Visible = false;
                    mediaFrm.Controls["StartStopAnimateMediaBtn"].Visible = false;
                    
                    Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], 0, 0, 100);

                    mediaFrm.Top = form.Top + location["Top"] + Statics.FormParam.TopArea ; //- MediaBoxStatic.FormParam.WidthBorderMedia;
                    mediaFrm.Left = form.Left + location["Left"] + Statics.FormParam.LeftArea; //- MediaBoxStatic.FormParam.WidthBorderMedia;

                    mediaFrm.SetForm(dimension["Width"] - (Statics.FormParam.WidthBorderMedia * 2), dimension["Height"] - (Statics.FormParam.WidthBorderMedia * 2));

                    MediaPlayer.Visible = true;
                    
                    MediaPlayer.Width = dimension["Width"] - (Statics.FormParam.WidthBorderMedia * 2);
                    MediaPlayer.Height = dimension["Height"] - (Statics.FormParam.WidthBorderMedia * 2);
                    MediaPlayer.AutoSize = true;

                    if (Statics.Music.Play == true)
                    {
                        AxWMPLib.AxWindowsMediaPlayer MusicPlayer = (AxWMPLib.AxWindowsMediaPlayer)Statics.MainForm.Controls["LeftPnl"].Controls["MusicPnl"].Controls["MusicPlayer"];
                        MusicPlayer.Ctlcontrols.stop();
                    }

                    StopWait();


                    if (Statics.Media.Title != null && Statics.Media.Title.Length > 0)
                    {

                        titleMedia.ShowTitle(Statics.Media.Title, (Statics.FormParam.LeftArea + Statics.MainForm.Left), (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height), Statics.FormParam.WidthArea);
                        if (ShowHideTitleTmr == null) { ShowHideTitleTmr_Set(); }
                        ShowHideTitleTmr.Enabled = true;
                    }
                    MediaPlayer.Ctlcontrols.play();
                }
            }

            if (Statics.Media.Type == "Image")
            {
                AxWMPLib.AxWindowsMediaPlayer MediaPlayer = (AxWMPLib.AxWindowsMediaPlayer)mediaFrm.Controls["MediaPnl"].Controls["MediaPlayer"];
                MediaPlayer.Ctlcontrols.stop();
                MediaPlayer.Visible = false;
                if (animateOpen == true)
                {
                    mediaFrm.Controls["MaximizeOrDefaultMediaBtn"].Visible = false;
                    mediaFrm.Controls["StartStopAnimateMediaBtn"].Visible = true;
                    PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];
                    PictureMedia.Visible = true;
                    PictureMedia.Top = 0;
                    PictureMedia.Left = 0;
                    PictureMedia.Width = 0;
                    PictureMedia.Height = 0;
                }
                else
                {
                    OpenMediaDefaulted(form, Statics.Media.Index);
                }
            }

            Statics.MainForm = form;
            if (animateOpen == true)
            {
                if (OpenMediaTmr == null) { OpenMediaTmr_Set(); }
                OpenMediaTmr.Enabled = true;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void OpenMediaTmr_Set()
        {
            OpenMediaTmr = new System.Timers.Timer(10000);
            OpenMediaTmr.Elapsed += new ElapsedEventHandler(OpenMediaTmr_Tick);
            OpenMediaTmr.Interval = 1;
        }

        private void CloseMediaTmr_Set()
        {
            if (CloseMediaTmr == null)
            {
                CloseMediaTmr = new System.Timers.Timer(10000);
                CloseMediaTmr.Elapsed += new ElapsedEventHandler(CloseMediaTmr_Tick);
            }
            if (Statics.Media.Type == "Image")
                CloseMediaTmr.Interval = Statics.FormParam.PictureInterval * 1000;
            if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
            {
                if(durationInSeconds > Statics.FormParam.VideoInterval)
                    CloseMediaTmr.Interval = Statics.FormParam.VideoInterval * 1000;
                else
                    CloseMediaTmr.Interval = (durationInSeconds + 2) * 1000;
            }
            CloseMediaTmr.Enabled = true;
        }

        private void ShowHideTitleTmr_Set()
        {
            ShowHideTitleTmr = new System.Timers.Timer(10000);
            ShowHideTitleTmr.Elapsed += new ElapsedEventHandler(ShowHideTitleTmr_Tick);
            ShowHideTitleTmr.Interval = 100;
        }

        private void ShowHideTitleTmr_Tick(object source, ElapsedEventArgs e)
        {
            Application.DoEvents();
            try
            {

                double interval = 0;
                if (Statics.Media.Type == "Image")
                    interval = Statics.FormParam.PictureInterval * 1000;
                if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
                    interval = Statics.FormParam.VideoInterval * 1000;

                if (interval > 4000) interval = 4000; 

                if (interval < 2200)
                {
                    ShowHideTitleTmr.Enabled = false;
                    titleMedia.CloseTitle();
                    return;
                }

                int range = titleMedia.GetWidthTitle() / 10;

                if (showTitle && ((-titleMedia.GetLeftTitle() + titleMedia.Width) < titleMedia.GetWidthTitle()))
                {
                    if (ShowHideTitleTmr.Interval != 101) ShowHideTitleTmr.Interval = 101;
                    
                    titleMedia.MoveTitle(titleMedia.GetLeftTitle() - range, (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height));
                }
                else if (showTitle && ((-titleMedia.GetLeftTitle() + titleMedia.Width) >= titleMedia.GetWidthTitle()))
                {

                    interval -= 2200;
                    if (interval < 1) { interval = 1; }
                    showTitle = false;
                    if (ShowHideTitleTmr.Interval != interval)   ShowHideTitleTmr.Interval = interval;
                }
                else if (!showTitle && titleMedia.GetLeftTitle() < titleMedia.Width)
                {
                    if (ShowHideTitleTmr.Interval != 101) ShowHideTitleTmr.Interval = 101;
                    titleMedia.MoveTitle(titleMedia.GetLeftTitle() + range, (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height));
                }
                else if (!showTitle && titleMedia.GetLeftTitle() >= titleMedia.Width)
                {
                    ShowHideTitleTmr.Interval = 100;
                    ShowHideTitleTmr.Enabled = false;
                    showTitle = true;
                    titleMedia.CloseTitle();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void StartShadow()
        {
            //Form formOpen = Application.OpenForms["ShadowFrm"];
            //if (formOpen != null) return;

            shadowFrm.Show();
            shadowFrm.Visible = false;
            shadowFrm.Top = Statics.FormParam.TopArea + Statics.MainForm.Top;
            shadowFrm.Left = Statics.FormParam.LeftArea + Statics.MainForm.Left;
            shadowFrm.Height = Statics.FormParam.HeightArea;
            shadowFrm.Width = Statics.FormParam.WidthArea;
            shadowFrm.Visible = true;
            shadowFrm.TopMost = true;
            shadowFrm.TopMost = false;
        }

        private void DisableMenu()
        {
            Statics.MainForm.Controls["LeftPnl"].Enabled = false;
            Statics.MainForm.Controls["TopPnl"].Enabled = false;
            Statics.MainForm.Controls["MediaBoxMenuBtn"].Enabled = false;
        }

        private void EnableMenu()
        {
            Statics.MainForm.Controls["LeftPnl"].Enabled = true;
            Statics.MainForm.Controls["TopPnl"].Enabled = true;
            Statics.MainForm.Controls["MediaBoxMenuBtn"].Enabled = true;
        }

        public void StopShadow()
        {
            try
            {
                shadowFrm.Visible = false;
                shadowFrm.Top = 0;
                shadowFrm.Left = 0;
                shadowFrm.Height = 0;
                shadowFrm.Width = 0;
                shadowFrm.Hide();
            }
            catch (Exception ex)
            {
            }
        }

        private void OpenMediaTmr_Tick(object source, ElapsedEventArgs e)
        {
            OpenMediaTmr.Enabled = false;
            Common common = new Common();
            Utility utility = new Utility();
            int newWidthBorder = 0;
            Cursor.Current = Cursors.WaitCursor;
            if (Statics.Media.IncrementPercent <= 300)
            {
                try
                {
                    if (Statics.Media.Type == "Image")
                    {
                        MultimediaCatalog multimediaCatalog = new MultimediaCatalog();

                        if (Statics.Media.IncrementPercent == 0)
                        {
                            StopWait();
                            StopShadow();
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 0) / 100);
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent);
                            dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 100, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);

                            mediaFrm.Top = Statics.MainForm.Top + location["Top"] + Statics.FormParam.TopArea - newWidthBorder;
                            mediaFrm.Left = Statics.MainForm.Left + location["Left"] + Statics.FormParam.LeftArea - newWidthBorder;

                            mediaFrm.SetForm(dimension["Width"], dimension["Height"]);

                            PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];
                            try
                            {
                                PictureMedia.Image = Statics.Media.BitmapMediaList[0];
                            }
                            catch (Exception)
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                return;
                            }

                            Application.DoEvents();
                        }

                        if (Statics.Media.IncrementPercent >= 0 && Statics.Media.IncrementPercent <= 100)
                        {
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 0) / 100);
                            if (Statics.Media.IncrementPercent + AmountIncrement >= 100) { Statics.Media.IncrementPercent = 100; }
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent);
                            if (LeftTopOpenMedia == 0) { mediaFrm.Top = Statics.MainForm.Top + location["Top"] + Statics.FormParam.TopArea - newWidthBorder; }
                            if (LeftTopOpenMedia == 1) { mediaFrm.Left = Statics.MainForm.Left + location["Left"] + Statics.FormParam.LeftArea - newWidthBorder; }
                        }

                        if (Statics.Media.IncrementPercent >= 100 + AmountIncrement && Statics.Media.IncrementPercent <= 200)
                        {
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 100) / 100);
                            if (Statics.Media.IncrementPercent + AmountIncrement >= 200) { Statics.Media.IncrementPercent = 200; }
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent-100);
                            if (LeftTopOpenMedia == 1) { mediaFrm.Top = Statics.MainForm.Top + location["Top"] + Statics.FormParam.TopArea - newWidthBorder; }
                            if (LeftTopOpenMedia == 0) { mediaFrm.Left = Statics.MainForm.Left + location["Left"] + Statics.FormParam.LeftArea - newWidthBorder; }
                        }

                        if (Statics.Media.IncrementPercent >= 200 + AmountIncrement && Statics.Media.IncrementPercent <= 300)
                        {
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 200) / 100);
                            if (Statics.Media.IncrementPercent == 200 + AmountIncrement)
                            {
                                var rnd = new Random(DateTime.Now.Millisecond);
                                AmountIncrement = rnd.Next(1, 7);
                                if (AmountIncrement == 7) { AmountIncrement = 6; }
                            }
                            if (Statics.Media.IncrementPercent + AmountIncrement > 300) { Statics.Media.IncrementPercent = 300; }
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 300 - Statics.Media.IncrementPercent, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];
                            if (dimension["Width"] > PictureMedia.Width)
                            {
                                mediaFrm.SetForm(dimension["Width"], dimension["Height"]);
                                try
                                {
                                    PictureMedia.Image = Statics.Media.BitmapMediaList[Statics.Media.IncrementPercent - 200];
                                }
                                catch (Exception)
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    return;
                                }
                            }

                            if (Statics.Media.IncrementPercent == 300)
                            {
                                Cursor.Current = Cursors.Default;
                                StartShadow();
                                mediaFrm.TopMost = true;
                                mediaFrm.TopMost = false;

                                if (Statics.Media.Title != null && Statics.Media.Title.Length > 0)
                                {
                                    titleMedia.ShowTitle(Statics.Media.Title, (Statics.FormParam.LeftArea + Statics.MainForm.Left), (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height), Statics.FormParam.WidthArea);
                                    if (ShowHideTitleTmr == null) { ShowHideTitleTmr_Set(); }
                                    ShowHideTitleTmr.Enabled = true;
                                }
                            }
                        }
                        Statics.Media.IncrementPercent += AmountIncrement;
                    }

                    if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
                    {
                        if (Statics.Media.IncrementPercent == 0)
                        {
                            StopWait();
                            StopShadow();
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 0) / 100);
                            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea); 
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent);
                            dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 100, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            AxWMPLib.AxWindowsMediaPlayer MediaPlayer = (AxWMPLib.AxWindowsMediaPlayer)mediaFrm.Controls["MediaPnl"].Controls["MediaPlayer"];

                            mediaFrm.SetForm(dimension["Width"] - (Statics.FormParam.WidthBorderMedia * 2), dimension["Height"] - (Statics.FormParam.WidthBorderMedia * 2));
                        }

                        if (Statics.Media.IncrementPercent >= 0 && Statics.Media.IncrementPercent <= 100)
                        {
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 0) / 100);
                            if (Statics.Media.IncrementPercent + AmountIncrement >= 100) { Statics.Media.IncrementPercent = 100; }
                            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent);
                            if (LeftTopOpenMedia == 0) { mediaFrm.Top = Statics.MainForm.Top + location["Top"] + Statics.FormParam.TopArea; }
                            if (LeftTopOpenMedia == 1) { mediaFrm.Left = Statics.MainForm.Left + location["Left"] + Statics.FormParam.LeftArea; }
                        }

                        if (Statics.Media.IncrementPercent >= 100 + AmountIncrement && Statics.Media.IncrementPercent <= 200)
                        {
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 100) / 100);
                            if (Statics.Media.IncrementPercent + AmountIncrement >= 200)
                            {
                                Statics.Media.IncrementPercent = 200;
                            }

                            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 0, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, dimension["Width"], dimension["Height"], Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent-100);
                            if (LeftTopOpenMedia == 1) { mediaFrm.Top = Statics.MainForm.Top + location["Top"] + Statics.FormParam.TopArea; }
                            if (LeftTopOpenMedia == 0) { mediaFrm.Left = Statics.MainForm.Left + location["Left"] + Statics.FormParam.LeftArea; }
                        }

                        if (Statics.Media.IncrementPercent >= 200 + AmountIncrement && Statics.Media.IncrementPercent <= 300)
                        {
                            newWidthBorder = (Statics.FormParam.WidthBorderMedia * (Statics.Media.IncrementPercent - 200) / 100);
                            if (Statics.Media.IncrementPercent == 200 + AmountIncrement)
                            {
                                var rnd = new Random(DateTime.Now.Millisecond);
                                AmountIncrement = rnd.Next(1, 7);
                                if (AmountIncrement == 7) { AmountIncrement = 6; }
                            }
                            if (Statics.Media.IncrementPercent + AmountIncrement >= 300 )
                            {
                                Statics.Media.IncrementPercent = 300;
                            }
                            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
                            Dictionary<string, int> dimension = multimediaCatalog.GetSizeObject(Statics.Media.Width, Statics.Media.Height, Statics.Media.SizeThumbnail, 300 - Statics.Media.IncrementPercent, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea);
                            AxWMPLib.AxWindowsMediaPlayer MediaPlayer = (AxWMPLib.AxWindowsMediaPlayer)mediaFrm.Controls["MediaPnl"].Controls["MediaPlayer"];
                            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, Statics.Media.Width, Statics.Media.Height, Statics.Media.Top, Statics.Media.Left, Statics.Media.IncrementPercent - 200);

                            mediaFrm.SetForm(dimension["Width"] - (Statics.FormParam.WidthBorderMedia * 2), dimension["Height"] - (Statics.FormParam.WidthBorderMedia * 2));
                            MediaPlayer.Visible = true;

                            if (Statics.Media.IncrementPercent == 300)
                            {
                                Cursor.Current = Cursors.Default;
                                StartShadow();
                                mediaFrm.TopMost = true;
                                mediaFrm.TopMost = false;
                                if (Statics.Music.Play == true)
                                {
                                    AxWMPLib.AxWindowsMediaPlayer MusicPlayer = (AxWMPLib.AxWindowsMediaPlayer)Statics.MainForm.Controls["LeftPnl"].Controls["MusicPnl"].Controls["MusicPlayer"];
                                    MusicPlayer.Ctlcontrols.stop();
                                }

                                MediaPlayer.Ctlcontrols.play();

                                if (Statics.Media.Title != null && Statics.Media.Title.Length > 0)
                                {
                                    titleMedia.ShowTitle(Statics.Media.Title, (Statics.FormParam.LeftArea + Statics.MainForm.Left), (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height), Statics.FormParam.WidthArea);
                                    if (ShowHideTitleTmr == null) { ShowHideTitleTmr_Set(); }
                                    ShowHideTitleTmr.Enabled = true;
                                }
                            }
                        }
                        Statics.Media.IncrementPercent += AmountIncrement;
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
                finally
                {
                    OpenMediaTmr.Enabled = true;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            else
            {
                OpenMediaTmr.Enabled = false;
                Statics.Media.BitmapMediaList = null;
                Statics.Media.IncrementPercent = 0;



                if (Statics.Presentation.ImagesSequence == true)
                {
                    int index = 0;

                    DataRow row = Statics.DB.TblCatalog.Data.Rows[Statics.Media.Index];
                    DateTimeOffset currentDateCreated = DateTimeOffset.Parse(((dynamic)row["DateCreated"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"));
                    DateTimeOffset nextDateCreated = currentDateCreated;
                    string currentPath = Path.GetDirectoryName(row["PathFile"].ToString());
                    string nextPath = String.Empty;
                    for (int i = 1; i <= Statics.Presentation.MaximumNumberOfImagesInSequence; i++)
                    {
                        if ((Statics.Media.Index + i) < Statics.Presentation.CountElements)
                        { index = (Statics.Media.Index + i); }
                        row = Statics.DB.TblCatalog.Data.Rows[index];
                        nextDateCreated = DateTimeOffset.Parse(((dynamic)row["DateCreated"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"));
                        nextPath = Path.GetDirectoryName(row["PathFile"].ToString());
                        if (
                            utility.DiffInMillisecondsBetweenTwoDate(currentDateCreated, nextDateCreated) <= Statics.Presentation.MaximumDifferenceBetweenImagesInMilliseconds 
                            && currentDateCreated != nextDateCreated
                            && nextPath == currentPath
                            )
                        {
                            Application.DoEvents();
                            utility.Sleep(Statics.Presentation.IntervalImagesSequenceInMilliseconds);
                            OpenMediaDefaulted(Statics.MainForm, index);
                        }
                    }

                }
                CloseMediaTmr_Set();
                CloseMediaTmr.Enabled = true;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void CloseMediaTmr_Tick(object source, ElapsedEventArgs e)
        {
            Application.DoEvents();
            try
            {
                CloseMediaTmr.Enabled = false;

                if (!Statics.Presentation.Active) { return; }
                PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];
                PictureMedia.Visible = false;
                PictureMedia.Top = 0;
                PictureMedia.Left = 0;
                PictureMedia.Width = 0;
                PictureMedia.Height = 0;
                AxWMPLib.AxWindowsMediaPlayer MediaPlayer = (AxWMPLib.AxWindowsMediaPlayer)mediaFrm.Controls["MediaPnl"].Controls["MediaPlayer"];
                MediaPlayer.Ctlcontrols.stop();
                MediaPlayer.Visible = false;
                mediaFrm.Visible = false;
                mediaFrm.Hide();

                if (Statics.Music.Play == true)
                {
                    AxWMPLib.AxWindowsMediaPlayer MusicPlayer = (AxWMPLib.AxWindowsMediaPlayer)Statics.MainForm.Controls["LeftPnl"].Controls["MusicPnl"].Controls["MusicPlayer"];
                    MusicPlayer.Ctlcontrols.play();
                }

                StopShadow();
            }
            catch (Exception)
            {
            }
            finally
            {
                Presentation presentation = new Presentation();
                presentation.NextMedia();
                presentation.StartStopPresentationTmr();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void StartWait()
        {
            //Form formOpen = Application.OpenForms["WaitFrm"];
            //if (formOpen != null) return;

            StartShadow();
            DisableMenu();
            waitFrm.Show();
            waitFrm.Visible = false;
            waitFrm.Top = 0;
            waitFrm.Left = 0;
            waitFrm.Width = 0;
            waitFrm.Height = 0;
            int width = 100;
            int height = 20;
            int top = Statics.MainForm.Top + Statics.FormParam.TopArea + ((Statics.FormParam.HeightArea / 2) - (height / 2));
            int left = Statics.MainForm.Left + Statics.FormParam.LeftArea + ((Statics.FormParam.WidthArea / 2) - (width / 2));
            Color color = Statics.FormParam.ForeColor;
            Color backColor = Statics.FormParam.BackForeColor;
            waitFrm.StartWaitBar(waitFrm, top, left, width, height, backColor, color);
            waitFrm.TopMost = true;
            waitFrm.TopMost = false;
        }

        public void StopWait()
        {
            waitFrm.Visible = false;
            waitFrm.Top = 0;
            waitFrm.Left = 0;
            waitFrm.Width = 0;
            waitFrm.Height = 0;
            waitFrm.Hide();
            EnableMenu();
        }

        public void OpenMediaDefaulted(Form form , int index)
        {
            Application.DoEvents();
            Bitmap bitmap = null;
            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
            DataRow row = Statics.DB.TblCatalog.Data.Rows[index];
            waitFrm.TopMost = true;
            try
            {
                using (Bitmap bitmpaFromFile = (Bitmap)Image.FromFile(row["PathFile"].ToString()))
                {
                    bitmap = multimediaCatalog.GetThumbnail(bitmpaFromFile, Statics.Media.SizeThumbnail, 0, 100, Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, Statics.FormParam.WidthBorderMedia);
                }
            }
            catch (Exception ex)
            {
                return;
            }

            mediaFrm.Controls["MaximizeOrDefaultMediaBtn"].Visible = true;
            mediaFrm.Controls["StartStopAnimateMediaBtn"].Visible = false;
            
            Common common = new Common();
            Utility utility = new Utility();
           
            int widthThumbnail = Convert.ToInt32(row["WidthThumbnail"]);
            int heightThumbnail = Convert.ToInt32(row["HeightThumbnail"]);
            int width = Convert.ToInt32(row["WidthMedia"]);
            int height = Convert.ToInt32(row["HeightMedia"]);

            if (widthThumbnail >= heightThumbnail) { Statics.Media.SizeThumbnail = widthThumbnail; }
            else { Statics.Media.SizeThumbnail = heightThumbnail; }
            
            try
            {
                Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, bitmap.Width, bitmap.Height, 0, 0, 100);
                mediaFrm.Top = form.Top + location["Top"] + Statics.FormParam.TopArea - Statics.FormParam.WidthBorderMedia;
                mediaFrm.Left = form.Left + location["Left"] + Statics.FormParam.LeftArea - Statics.FormParam.WidthBorderMedia;
                if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
                {
                    mediaFrm.SetForm(bitmap.Width - (Statics.FormParam.WidthBorderMedia * 2), bitmap.Height - (Statics.FormParam.WidthBorderMedia * 2));
                }
                if (Statics.Media.Type == "Image")
                {
                    mediaFrm.SetForm(bitmap.Width, bitmap.Height);
                }
            }
            catch (Exception ex)
            { }

            PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];
            waitFrm.TopMost = true;
            try
            {
                PictureMedia.Image = bitmap;
            }
            catch (Exception ex)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return;
            }

            waitFrm.TopMost = false;
            mediaFrm.TopMost = true;
            mediaFrm.TopMost = false;
            StopWait();

            Statics.Media.Title = row["Title"].ToString();

            if (Statics.Media.Title != null && Statics.Media.Title.Length > 0)
            {
                titleMedia.ShowTitle(Statics.Media.Title, (Statics.FormParam.LeftArea + Statics.MainForm.Left), (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height), Statics.FormParam.WidthArea);

                if (ShowHideTitleTmr == null) { ShowHideTitleTmr_Set(); }
                ShowHideTitleTmr.Enabled = true;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void OpenMediaMaximixed(Form form)
        {
            Application.DoEvents();
            StartWait();
            waitFrm.TopMost = true;
            
            DataRow row = Statics.DB.TblCatalog.Data.Rows[Statics.Media.Index];
            Bitmap bitmap = null;
            Utility utility = new Utility();
            using (Bitmap bitmpaFromFile = (Bitmap)Image.FromFile(row["PathFile"].ToString()))
            {
                bitmap = new Bitmap(bitmpaFromFile);
            }
            if(form.Top >= 0) mediaFrm.Top = 0;
            else mediaFrm.Top = - Screen.GetBounds(form).Height;
            if (form.Left >= 0) mediaFrm.Left = 0;
            else mediaFrm.Left = -Screen.GetBounds(form).Width;

            mediaFrm.SetForm(bitmap.Width, bitmap.Height);

            PictureBox PictureMedia = (PictureBox)mediaFrm.Controls["MediaPnl"].Controls["PictureMedia"];

            mediaFrm.Controls["MaximizeOrDefaultMediaBtn"].Visible = true;
            mediaFrm.Controls["StartStopAnimateMediaBtn"].Visible = false;
            waitFrm.TopMost = true;
            try
            {
                PictureMedia.Image = bitmap;
            }
            catch (Exception)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return;
            }

            waitFrm.TopMost = false;
            mediaFrm.TopMost = true;
            mediaFrm.TopMost = false;
            StopWait();
            StopShadow();

            Statics.Media.Title = row["Title"].ToString();

            if (Statics.Media.Title != null && Statics.Media.Title.Length > 0)
            {
                titleMedia.ShowTitle(Statics.Media.Title, (Statics.FormParam.LeftArea + Statics.MainForm.Left), (Statics.FormParam.TopArea + Statics.MainForm.Top + Statics.FormParam.HeightArea - titleMedia.Height), Statics.FormParam.WidthArea);

                if (ShowHideTitleTmr == null) { ShowHideTitleTmr_Set(); }
                ShowHideTitleTmr.Enabled = true;
            }

        }
    }
}
