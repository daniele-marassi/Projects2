using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace MediaBox
{
    partial class MediaFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaFrm));
            this.MediaPnl = new System.Windows.Forms.Panel();
            this.PictureMedia = new System.Windows.Forms.PictureBox();
            this.MediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.StartStopAnimateMediaBtn = new System.Windows.Forms.Button();
            this.MaximizeOrDefaultMediaBtn = new System.Windows.Forms.Button();
            this.CloseMediaBtn = new System.Windows.Forms.Button();
            this.MediaPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureMedia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // MediaPnl
            // 
            this.MediaPnl.BackColor = System.Drawing.Color.Transparent;
            this.MediaPnl.Controls.Add(this.PictureMedia);
            this.MediaPnl.Controls.Add(this.MediaPlayer);
            this.MediaPnl.Location = new System.Drawing.Point(93, 136);
            this.MediaPnl.Name = "MediaPnl";
            this.MediaPnl.Size = new System.Drawing.Size(583, 351);
            this.MediaPnl.TabIndex = 12;
            this.MediaPnl.Visible = false;
            // 
            // PictureMedia
            // 
            this.PictureMedia.Location = new System.Drawing.Point(154, 5);
            this.PictureMedia.Name = "PictureMedia";
            this.PictureMedia.Size = new System.Drawing.Size(225, 50);
            this.PictureMedia.TabIndex = 7;
            this.PictureMedia.TabStop = false;
            this.PictureMedia.Visible = false;
            this.PictureMedia.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureMedia_MouseDown);
            this.PictureMedia.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureMedia_MouseMove);
            this.PictureMedia.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureMedia_MouseUp);
            // 
            // MediaPlayer
            // 
            this.MediaPlayer.Enabled = true;
            this.MediaPlayer.Location = new System.Drawing.Point(157, 88);
            this.MediaPlayer.Name = "MediaPlayer";
            this.MediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MediaPlayer.OcxState")));
            this.MediaPlayer.Size = new System.Drawing.Size(426, 260);
            this.MediaPlayer.TabIndex = 6;
            // 
            // StartStopAnimateMediaBtn
            // 
            this.StartStopAnimateMediaBtn.BackColor = System.Drawing.Color.Transparent;
            this.StartStopAnimateMediaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartStopAnimateMediaBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.StartStopAnimateMediaBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.StartStopAnimateMediaBtn.Location = new System.Drawing.Point(80, 9);
            this.StartStopAnimateMediaBtn.Margin = new System.Windows.Forms.Padding(0);
            this.StartStopAnimateMediaBtn.Name = "StartStopAnimateMediaBtn";
            this.StartStopAnimateMediaBtn.Size = new System.Drawing.Size(30, 30);
            this.StartStopAnimateMediaBtn.TabIndex = 15;
            this.StartStopAnimateMediaBtn.Text = "M";
            this.StartStopAnimateMediaBtn.UseVisualStyleBackColor = false;
            this.StartStopAnimateMediaBtn.Visible = false;
            this.StartStopAnimateMediaBtn.Click += new System.EventHandler(this.StartStopAnimateMediaBtn_Click);
            // 
            // MaximizeOrDefaultMediaBtn
            // 
            this.MaximizeOrDefaultMediaBtn.BackColor = System.Drawing.Color.Transparent;
            this.MaximizeOrDefaultMediaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaximizeOrDefaultMediaBtn.Font = new System.Drawing.Font("Webdings", 14F);
            this.MaximizeOrDefaultMediaBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.MaximizeOrDefaultMediaBtn.Location = new System.Drawing.Point(44, 9);
            this.MaximizeOrDefaultMediaBtn.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeOrDefaultMediaBtn.Name = "MaximizeOrDefaultMediaBtn";
            this.MaximizeOrDefaultMediaBtn.Size = new System.Drawing.Size(30, 30);
            this.MaximizeOrDefaultMediaBtn.TabIndex = 14;
            this.MaximizeOrDefaultMediaBtn.Text = "1";
            this.MaximizeOrDefaultMediaBtn.UseVisualStyleBackColor = false;
            this.MaximizeOrDefaultMediaBtn.Visible = false;
            this.MaximizeOrDefaultMediaBtn.Click += new System.EventHandler(this.MaximizeOrDefaultMediaBtn_Click);
            // 
            // CloseMediaBtn
            // 
            this.CloseMediaBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseMediaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseMediaBtn.Font = new System.Drawing.Font("Webdings", 12F);
            this.CloseMediaBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.CloseMediaBtn.Location = new System.Drawing.Point(9, 9);
            this.CloseMediaBtn.Margin = new System.Windows.Forms.Padding(0);
            this.CloseMediaBtn.Name = "CloseMediaBtn";
            this.CloseMediaBtn.Size = new System.Drawing.Size(30, 30);
            this.CloseMediaBtn.TabIndex = 13;
            this.CloseMediaBtn.Text = "r";
            this.CloseMediaBtn.UseVisualStyleBackColor = false;
            this.CloseMediaBtn.Click += new System.EventHandler(this.CloseMediaBtn_Click);
            // 
            // MediaFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 566);
            this.ControlBox = false;
            this.Controls.Add(this.StartStopAnimateMediaBtn);
            this.Controls.Add(this.MaximizeOrDefaultMediaBtn);
            this.Controls.Add(this.CloseMediaBtn);
            this.Controls.Add(this.MediaPnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MediaFrm";
            this.Text = "MediaFrm";
            this.Load += new System.EventHandler(this.MediaFrm_Load);
            this.MediaPnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureMedia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MediaPnl;
        private System.Windows.Forms.PictureBox PictureMedia;
        private AxWMPLib.AxWindowsMediaPlayer MediaPlayer;
        private Button StartStopAnimateMediaBtn;
        private Button MaximizeOrDefaultMediaBtn;
        private Button CloseMediaBtn;
    }
}