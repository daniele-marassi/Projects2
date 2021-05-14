namespace Emulation
{
    partial class MousePointer
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
            this.Pointer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Pointer
            // 
            this.Pointer.AutoSize = true;
            this.Pointer.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.Pointer.Location = new System.Drawing.Point(0, 0);
            this.Pointer.Margin = new System.Windows.Forms.Padding(0);
            this.Pointer.Name = "Pointer";
            this.Pointer.Size = new System.Drawing.Size(63, 63);
            this.Pointer.TabIndex = 7;
            this.Pointer.Text = "A";
            // 
            // MousePointer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(83, 82);
            this.ControlBox = false;
            this.Controls.Add(this.Pointer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MousePointer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "VirtualMousePointer";
            this.Load += new System.EventHandler(this.MousePointer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Pointer;
    }
}

