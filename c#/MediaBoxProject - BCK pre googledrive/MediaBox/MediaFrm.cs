using Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace MediaBox
{
    public partial class MediaFrm : Form
    {
        private bool maximizeMedia = false;
        public MediaFrm()
        {
            InitializeComponent();
            this.Visible = false;
            SetColor();
        }

        private void CloseMediaBtn_Click(object sender, EventArgs e)
        {
            Media media = new Media();
            media.StopWait();
            media.StopShadow();
            if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
            {
                this.MediaPlayer.close();
                if (Statics.Music.Play == true)
                {
                    MediaPlayer.Ctlcontrols.play();
                }
            }
            this.MediaPnl.Visible = false;
            this.Close();
        }

        private void MaximizeOrDefaultMediaBtn_Click(object sender, EventArgs e)
        {
            if (maximizeMedia == false) { maximizeMedia = true; }
            else { maximizeMedia = false; }
            Media media = new Media();
            if (maximizeMedia == true)
            {
                media.OpenMediaMaximixed(this);
            }
            else
            {
                media.OpenMediaDefaulted(this, Statics.Media.Index);
            }
        }

        private void StartStopAnimateMediaBtn_Click(object sender, EventArgs e)
        {
            MediaBox mediaBox = new MediaBox();
            mediaBox.StartStopAnimate();
            if (Statics.Presentation.Active == false)
            {
               MaximizeOrDefaultMediaBtn.Visible = true;
            }
            else
            {
                MaximizeOrDefaultMediaBtn.Visible = false;
            }
        }

        private void MediaFrm_Load(object sender, EventArgs e)
        {
            Statics.MediaForm = this;
            Utility utility = new Utility();
            this.StartStopAnimateMediaBtn.Font = utility.GetFonts("Web Symbols", 10F, Statics.Fonts);
            this.MaximizeOrDefaultMediaBtn.Font = utility.GetFonts("Webdings", 14F, Statics.Fonts);
            this.CloseMediaBtn.Font = utility.GetFonts("Webdings", 12F, Statics.Fonts);
            Common common = new Common();
            common.ChangeLogo(this);
        }

        private void SetColor()
        {
            this.BackColor = Statics.FormParam.BorderColorMedia;
            MediaPnl.BackColor = Statics.FormParam.BackGrondColorMain;
            PictureMedia.BackColor = Statics.FormParam.BackGrondColorMain;
            CloseMediaBtn.BackColor = Statics.FormParam.BackGrondColorMain;
            MaximizeOrDefaultMediaBtn.BackColor = Statics.FormParam.BackGrondColorMain;
            StartStopAnimateMediaBtn.BackColor = Statics.FormParam.BackGrondColorMain;

            CloseMediaBtn.ForeColor = Statics.FormParam.ForeColor;
            MaximizeOrDefaultMediaBtn.ForeColor = Statics.FormParam.ForeColor;
            StartStopAnimateMediaBtn.ForeColor = Statics.FormParam.ForeColor;
        }

        public void SetForm(int width, int height)
        {
            SetColor();

            this.Width = width + (Statics.FormParam.WidthBorderMedia * 2);
            this.Height = height + (Statics.FormParam.WidthBorderMedia * 2);

            MediaPnl.AutoSize = false;
            MediaPnl.Top = Statics.FormParam.WidthBorderMedia;
            MediaPnl.Left = Statics.FormParam.WidthBorderMedia;
            MediaPnl.Width = width;
            MediaPnl.Height = height;

            if (Statics.Media.Type == "Video" || Statics.Media.Type == "Audio")
            {
                PictureMedia.Visible = false;
                MediaPlayer.Visible = true;
            }

            if (Statics.Media.Type == "Image")
            {
                PictureMedia.AutoSize = false;
                PictureMedia.Top = 0;
                PictureMedia.Left = 0;
                PictureMedia.Width = width;
                PictureMedia.Height = height;
                MediaPlayer.Visible = false;
                PictureMedia.Visible = true;
                PictureMedia.Update();
            }

            this.Visible = true;
            MediaPnl.Visible = true;
        }

        private void PictureMedia_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            Statics.Position.Y = e.Location.Y;
            Statics.Position.X = e.Location.X;
            Statics.Mouse.Down = true;
        }

        private void PictureMedia_MouseMove(object sender, MouseEventArgs e)
        {
            if (Statics.Mouse.Down)
            {
                int top = Cursor.Position.Y - (this.Location.Y + Statics.Position.Y);
                int left = Cursor.Position.X - (this.Location.X + Statics.Position.X);

                if ((top + PictureMedia.Height) > 100 && PictureMedia.Top < MediaPnl.Height - 100){ PictureMedia.Top = top; }
                if ((left + PictureMedia.Width) > 100 && PictureMedia.Left < MediaPnl.Width - 100) { PictureMedia.Left = left; }

                if (PictureMedia.Top >= MediaPnl.Height - 100) { PictureMedia.Top = MediaPnl.Height - 101; }
                if (PictureMedia.Left >= MediaPnl.Width - 100) { PictureMedia.Left = MediaPnl.Width - 101; }
            }
        }

        private void PictureMedia_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            Statics.Mouse.Down = false;
        }


    }
}
