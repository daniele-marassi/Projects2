namespace Emulation
{
    partial class Tooltip
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
            this.TooltipLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TooltipLbl
            // 
            this.TooltipLbl.AutoSize = true;
            this.TooltipLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TooltipLbl.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TooltipLbl.Location = new System.Drawing.Point(0, 0);
            this.TooltipLbl.Margin = new System.Windows.Forms.Padding(0);
            this.TooltipLbl.Name = "TooltipLbl";
            this.TooltipLbl.Size = new System.Drawing.Size(64, 20);
            this.TooltipLbl.TabIndex = 8;
            this.TooltipLbl.Text = "tooltip";
            // 
            // Tooltip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(83, 82);
            this.ControlBox = false;
            this.Controls.Add(this.TooltipLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Tooltip";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "VirtualTooltip";
            this.Load += new System.EventHandler(this.Tooltip_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label TooltipLbl;
    }
}

