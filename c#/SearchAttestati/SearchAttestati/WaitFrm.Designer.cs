namespace SearchAttestati
{
    partial class WaitFrm
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
            this.WaitGif = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.WaitGif)).BeginInit();
            this.SuspendLayout();
            // 
            // WaitGif
            // 
            this.WaitGif.Image = global::SearchAttestati.Properties.Resources.Wait_Please;
            this.WaitGif.Location = new System.Drawing.Point(-9, -14);
            this.WaitGif.Name = "WaitGif";
            this.WaitGif.Size = new System.Drawing.Size(119, 128);
            this.WaitGif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WaitGif.TabIndex = 8;
            this.WaitGif.TabStop = false;
            // 
            // WaitFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.ControlBox = false;
            this.Controls.Add(this.WaitGif);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WaitFrm";
            this.Opacity = 0.5D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Wait";
            ((System.ComponentModel.ISupportInitialize)(this.WaitGif)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox WaitGif;
    }
}