namespace Shut
{
    partial class Shut
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
            this.components = new System.ComponentModel.Container();
            this.ShutDown = new System.Windows.Forms.Label();
            this.Exit = new System.Windows.Forms.Label();
            this.Lock = new System.Windows.Forms.Label();
            this.Restart = new System.Windows.Forms.Label();
            this.Info = new System.Windows.Forms.Label();
            this.Hibernate = new System.Windows.Forms.Label();
            this.LogOff = new System.Windows.Forms.Label();
            this.OpenTmr = new System.Windows.Forms.Timer(this.components);
            this.CloseTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomShutDownTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomExitTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomLockTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomRestartTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomInfoTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomHibernateTmr = new System.Windows.Forms.Timer(this.components);
            this.ColorAndZoomLogOffTmr = new System.Windows.Forms.Timer(this.components);
            this.ZoomTmr = new System.Windows.Forms.Timer(this.components);
            this.ServiceTmr = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ShutDown
            // 
            this.ShutDown.AutoSize = true;
            this.ShutDown.Font = new System.Drawing.Font("multimedia icons", 84.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.ShutDown.Location = new System.Drawing.Point(432, 403);
            this.ShutDown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ShutDown.Name = "ShutDown";
            this.ShutDown.Size = new System.Drawing.Size(162, 113);
            this.ShutDown.TabIndex = 0;
            this.ShutDown.Text = "?";
            // 
            // Exit
            // 
            this.Exit.AutoSize = true;
            this.Exit.Font = new System.Drawing.Font("Webdings", 200F);
            this.Exit.Location = new System.Drawing.Point(-162, 386);
            this.Exit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(379, 267);
            this.Exit.TabIndex = 1;
            this.Exit.Text = "n";
            // 
            // Lock
            // 
            this.Lock.AutoSize = true;
            this.Lock.Font = new System.Drawing.Font("multimedia icons", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.Lock.Location = new System.Drawing.Point(163, 72);
            this.Lock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lock.Name = "Lock";
            this.Lock.Size = new System.Drawing.Size(126, 96);
            this.Lock.TabIndex = 2;
            this.Lock.Text = "o";
            // 
            // Restart
            // 
            this.Restart.AutoSize = true;
            this.Restart.Font = new System.Drawing.Font("Wingdings 3", 80.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.Restart.Location = new System.Drawing.Point(388, 266);
            this.Restart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(148, 122);
            this.Restart.TabIndex = 3;
            this.Restart.Text = "P";
            // 
            // Info
            // 
            this.Info.AutoSize = true;
            this.Info.Font = new System.Drawing.Font("Byom Icons", 28F);
            this.Info.Location = new System.Drawing.Point(130, 326);
            this.Info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(62, 45);
            this.Info.TabIndex = 4;
            this.Info.Text = "I";
            // 
            // Hibernate
            // 
            this.Hibernate.AutoSize = true;
            this.Hibernate.Font = new System.Drawing.Font("cellpic", 99.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hibernate.Location = new System.Drawing.Point(287, 144);
            this.Hibernate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Hibernate.Name = "Hibernate";
            this.Hibernate.Size = new System.Drawing.Size(157, 133);
            this.Hibernate.TabIndex = 5;
            this.Hibernate.Text = "I";
            // 
            // LogOff
            // 
            this.LogOff.AutoSize = true;
            this.LogOff.Font = new System.Drawing.Font("Wingdings 3", 80.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.LogOff.Location = new System.Drawing.Point(-10, 9);
            this.LogOff.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LogOff.Name = "LogOff";
            this.LogOff.Size = new System.Drawing.Size(148, 122);
            this.LogOff.TabIndex = 6;
            this.LogOff.Text = "R";
            // 
            // OpenTmr
            // 
            this.OpenTmr.Interval = 15;
            this.OpenTmr.Tick += new System.EventHandler(this.OpenTmr_Tick);
            // 
            // CloseTmr
            // 
            this.CloseTmr.Interval = 15;
            this.CloseTmr.Tick += new System.EventHandler(this.CloseTmr_Tick);
            // 
            // ColorAndZoomShutDownTmr
            // 
            this.ColorAndZoomShutDownTmr.Interval = 20;
            this.ColorAndZoomShutDownTmr.Tick += new System.EventHandler(this.ColorAndZoomShutDownTmr_Tick);
            // 
            // ColorAndZoomExitTmr
            // 
            this.ColorAndZoomExitTmr.Enabled = true;
            this.ColorAndZoomExitTmr.Interval = 20;
            this.ColorAndZoomExitTmr.Tick += new System.EventHandler(this.ColorAndZoomExitTmr_Tick);
            // 
            // ColorAndZoomLockTmr
            // 
            this.ColorAndZoomLockTmr.Interval = 20;
            this.ColorAndZoomLockTmr.Tick += new System.EventHandler(this.ColorAndZoomLockTmr_Tick);
            // 
            // ColorAndZoomRestartTmr
            // 
            this.ColorAndZoomRestartTmr.Interval = 20;
            this.ColorAndZoomRestartTmr.Tick += new System.EventHandler(this.ColorAndZoomRestartTmr_Tick);
            // 
            // ColorAndZoomInfoTmr
            // 
            this.ColorAndZoomInfoTmr.Interval = 20;
            this.ColorAndZoomInfoTmr.Tick += new System.EventHandler(this.ColorAndZoomInfoTmr_Tick);
            // 
            // ColorAndZoomHibernateTmr
            // 
            this.ColorAndZoomHibernateTmr.Interval = 20;
            this.ColorAndZoomHibernateTmr.Tick += new System.EventHandler(this.ColorAndZoomHibernateTmr_Tick);
            // 
            // ColorAndZoomLogOffTmr
            // 
            this.ColorAndZoomLogOffTmr.Interval = 20;
            this.ColorAndZoomLogOffTmr.Tick += new System.EventHandler(this.ColorAndZoomLogOffTmr_Tick);
            // 
            // ServiceTmr
            // 
            this.ServiceTmr.Interval = 1;
            this.ServiceTmr.Tick += new System.EventHandler(this.ServiceTmr_Tick);
            // 
            // Shut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 530);
            this.ControlBox = false;
            this.Controls.Add(this.LogOff);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.Lock);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Hibernate);
            this.Controls.Add(this.Restart);
            this.Controls.Add(this.ShutDown);
            this.Font = new System.Drawing.Font("Breezi Icon Set", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Shut";
            this.ShowInTaskbar = false;
            this.Text = "Shut";
            this.Load += new System.EventHandler(this.Shut_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ShutDown;
        private System.Windows.Forms.Label Exit;
        private System.Windows.Forms.Label Lock;
        private System.Windows.Forms.Label Restart;
        private System.Windows.Forms.Label Info;
        private System.Windows.Forms.Label Hibernate;
        public System.Windows.Forms.Label LogOff;
        private System.Windows.Forms.Timer OpenTmr;
        private System.Windows.Forms.Timer CloseTmr;
        private System.Windows.Forms.Timer ColorAndZoomShutDownTmr;
        private System.Windows.Forms.Timer ColorAndZoomExitTmr;
        private System.Windows.Forms.Timer ColorAndZoomLockTmr;
        private System.Windows.Forms.Timer ColorAndZoomRestartTmr;
        private System.Windows.Forms.Timer ColorAndZoomInfoTmr;
        private System.Windows.Forms.Timer ColorAndZoomHibernateTmr;
        private System.Windows.Forms.Timer ColorAndZoomLogOffTmr;
        private System.Windows.Forms.Timer ZoomTmr;
        private System.Windows.Forms.Timer ServiceTmr;
    }
}

