namespace SearchAttestati
{
    partial class MessagesFrm
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
            this.MessagesTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // MessagesTxt
            // 
            this.MessagesTxt.AcceptsReturn = true;
            this.MessagesTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessagesTxt.Location = new System.Drawing.Point(12, 12);
            this.MessagesTxt.Multiline = true;
            this.MessagesTxt.Name = "MessagesTxt";
            this.MessagesTxt.ReadOnly = true;
            this.MessagesTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MessagesTxt.Size = new System.Drawing.Size(526, 208);
            this.MessagesTxt.TabIndex = 0;
            // 
            // MessagesFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 232);
            this.Controls.Add(this.MessagesTxt);
            this.Name = "MessagesFrm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messages";
            this.Load += new System.EventHandler(this.Messages_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MessagesTxt;
    }
}