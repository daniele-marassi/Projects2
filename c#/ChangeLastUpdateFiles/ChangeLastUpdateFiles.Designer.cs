namespace ChangeLastUpdateFiles
{
    partial class ChangeLastUpdateFiles
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
            this.ChangeBtn = new System.Windows.Forms.Button();
            this.PathTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ChangeBtn
            // 
            this.ChangeBtn.Location = new System.Drawing.Point(12, 38);
            this.ChangeBtn.Name = "ChangeBtn";
            this.ChangeBtn.Size = new System.Drawing.Size(75, 23);
            this.ChangeBtn.TabIndex = 0;
            this.ChangeBtn.Text = "Change";
            this.ChangeBtn.UseVisualStyleBackColor = true;
            this.ChangeBtn.Click += new System.EventHandler(this.ChangeBtn_Click);
            // 
            // PathTxt
            // 
            this.PathTxt.Location = new System.Drawing.Point(12, 12);
            this.PathTxt.Name = "PathTxt";
            this.PathTxt.Size = new System.Drawing.Size(512, 20);
            this.PathTxt.TabIndex = 1;
            // 
            // ChangeLastUpdateFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 104);
            this.Controls.Add(this.PathTxt);
            this.Controls.Add(this.ChangeBtn);
            this.Name = "ChangeLastUpdateFiles";
            this.Text = "Change LastUpdate Files";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChangeBtn;
        private System.Windows.Forms.TextBox PathTxt;
    }
}

