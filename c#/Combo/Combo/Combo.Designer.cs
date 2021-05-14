namespace Combo
{
    partial class Combo
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.ValueTxt = new System.Windows.Forms.TextBox();
            this.ValueLbl = new System.Windows.Forms.Label();
            this.ElaborateBtn = new System.Windows.Forms.Button();
            this.ResultLb = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ValueTxt
            // 
            this.ValueTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueTxt.Location = new System.Drawing.Point(50, 12);
            this.ValueTxt.Name = "ValueTxt";
            this.ValueTxt.Size = new System.Drawing.Size(219, 20);
            this.ValueTxt.TabIndex = 0;
            this.ValueTxt.Text = "12345";
            // 
            // ValueLbl
            // 
            this.ValueLbl.AutoSize = true;
            this.ValueLbl.Location = new System.Drawing.Point(12, 15);
            this.ValueLbl.Name = "ValueLbl";
            this.ValueLbl.Size = new System.Drawing.Size(34, 13);
            this.ValueLbl.TabIndex = 1;
            this.ValueLbl.Text = "Value";
            // 
            // ElaborateBtn
            // 
            this.ElaborateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ElaborateBtn.Location = new System.Drawing.Point(275, 9);
            this.ElaborateBtn.Name = "ElaborateBtn";
            this.ElaborateBtn.Size = new System.Drawing.Size(75, 23);
            this.ElaborateBtn.TabIndex = 2;
            this.ElaborateBtn.Text = "Elaborate";
            this.ElaborateBtn.UseVisualStyleBackColor = true;
            this.ElaborateBtn.Click += new System.EventHandler(this.ElaborateBtn_Click);
            // 
            // ResultLb
            // 
            this.ResultLb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultLb.FormattingEnabled = true;
            this.ResultLb.Location = new System.Drawing.Point(12, 38);
            this.ResultLb.Name = "ResultLb";
            this.ResultLb.Size = new System.Drawing.Size(338, 160);
            this.ResultLb.TabIndex = 3;
            // 
            // Combo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 215);
            this.Controls.Add(this.ResultLb);
            this.Controls.Add(this.ElaborateBtn);
            this.Controls.Add(this.ValueLbl);
            this.Controls.Add(this.ValueTxt);
            this.Name = "Combo";
            this.Text = "Combo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ValueTxt;
        private System.Windows.Forms.Label ValueLbl;
        private System.Windows.Forms.Button ElaborateBtn;
        private System.Windows.Forms.ListBox ResultLb;
    }
}

