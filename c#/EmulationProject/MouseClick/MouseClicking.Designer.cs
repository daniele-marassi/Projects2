namespace Emulation
{
    partial class MouseClicking
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
            this.Clicking = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Clicking
            // 
            this.Clicking.AutoSize = true;
            this.Clicking.Font = new System.Drawing.Font("Webdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.Clicking.Location = new System.Drawing.Point(0, 0);
            this.Clicking.Name = "Clicking";
            this.Clicking.Size = new System.Drawing.Size(25, 19);
            this.Clicking.TabIndex = 0;
            this.Clicking.Text = "n";
            // 
            // MouseClicking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(116, 73);
            this.ControlBox = false;
            this.Controls.Add(this.Clicking);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MouseClicking";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MouseClick";
            this.Load += new System.EventHandler(this.MouseClicking_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Clicking;
    }
}

