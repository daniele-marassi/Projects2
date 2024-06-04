namespace CheckAvailablePortOnIP
{
    partial class CheckAvailablePortOnIPFrm
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
            this.SearchBtn = new System.Windows.Forms.Button();
            this.IpTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PortStartTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PortEndTxt = new System.Windows.Forms.TextBox();
            this.PortAvviableList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PortTestedLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(195, 12);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(126, 72);
            this.SearchBtn.TabIndex = 0;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // IpTxt
            // 
            this.IpTxt.Location = new System.Drawing.Point(74, 12);
            this.IpTxt.Name = "IpTxt";
            this.IpTxt.Size = new System.Drawing.Size(100, 20);
            this.IpTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ip";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port Start";
            // 
            // PortStartTxt
            // 
            this.PortStartTxt.Location = new System.Drawing.Point(74, 38);
            this.PortStartTxt.Name = "PortStartTxt";
            this.PortStartTxt.Size = new System.Drawing.Size(100, 20);
            this.PortStartTxt.TabIndex = 3;
            this.PortStartTxt.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port End";
            // 
            // PortEndTxt
            // 
            this.PortEndTxt.Location = new System.Drawing.Point(74, 64);
            this.PortEndTxt.Name = "PortEndTxt";
            this.PortEndTxt.Size = new System.Drawing.Size(100, 20);
            this.PortEndTxt.TabIndex = 5;
            // 
            // PortAvviableList
            // 
            this.PortAvviableList.FormattingEnabled = true;
            this.PortAvviableList.Location = new System.Drawing.Point(20, 90);
            this.PortAvviableList.Name = "PortAvviableList";
            this.PortAvviableList.Size = new System.Drawing.Size(301, 108);
            this.PortAvviableList.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Port:";
            // 
            // PortTestedLbl
            // 
            this.PortTestedLbl.AutoSize = true;
            this.PortTestedLbl.Location = new System.Drawing.Point(52, 204);
            this.PortTestedLbl.Name = "PortTestedLbl";
            this.PortTestedLbl.Size = new System.Drawing.Size(13, 13);
            this.PortTestedLbl.TabIndex = 9;
            this.PortTestedLbl.Text = "0";
            // 
            // CheckAvailablePortOnIPFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 226);
            this.Controls.Add(this.PortTestedLbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PortAvviableList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PortEndTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PortStartTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IpTxt);
            this.Controls.Add(this.SearchBtn);
            this.Name = "CheckAvailablePortOnIPFrm";
            this.Text = "Check Available Port On IP";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox IpTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PortStartTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PortEndTxt;
        private System.Windows.Forms.ListBox PortAvviableList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label PortTestedLbl;
    }
}

