namespace Shut
{
    partial class ShutBack
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
            this.Back = new System.Windows.Forms.Label();
            this.ToolTipLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Back
            // 
            this.Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Back.AutoSize = true;
            this.Back.Font = new System.Drawing.Font("Webdings", 760.25F);
            this.Back.Location = new System.Drawing.Point(-864, 532);
            this.Back.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(1437, 1014);
            this.Back.TabIndex = 2;
            this.Back.Text = "n";
            this.Back.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ToolTipLbl
            // 
            this.ToolTipLbl.AutoSize = true;
            this.ToolTipLbl.BackColor = System.Drawing.Color.Transparent;
            this.ToolTipLbl.Font = new System.Drawing.Font("Verdana", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolTipLbl.Location = new System.Drawing.Point(576, 390);
            this.ToolTipLbl.Name = "ToolTipLbl";
            this.ToolTipLbl.Size = new System.Drawing.Size(154, 42);
            this.ToolTipLbl.TabIndex = 3;
            this.ToolTipLbl.Text = "toolTip";
            // 
            // ShutBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 760);
            this.ControlBox = false;
            this.Controls.Add(this.ToolTipLbl);
            this.Controls.Add(this.Back);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShutBack";
            this.Opacity = 0.3D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ShutBack";
            this.Load += new System.EventHandler(this.ShutBack_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Back;
        private System.Windows.Forms.Label ToolTipLbl;
    }
}