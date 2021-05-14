namespace MediaBox
{
    partial class TitleMedia
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
            this.Lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Lbl
            // 
            this.Lbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Lbl.AutoSize = true;
            this.Lbl.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl.ForeColor = System.Drawing.Color.Black;
            this.Lbl.Location = new System.Drawing.Point(2, 3);
            this.Lbl.Name = "Lbl";
            this.Lbl.Size = new System.Drawing.Size(116, 58);
            this.Lbl.TabIndex = 11;
            this.Lbl.Text = "Title";
            this.Lbl.Visible = false;
            // 
            // TitleMedia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 70);
            this.ControlBox = false;
            this.Controls.Add(this.Lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TitleMedia";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TitleMedia";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Lbl;
    }
}