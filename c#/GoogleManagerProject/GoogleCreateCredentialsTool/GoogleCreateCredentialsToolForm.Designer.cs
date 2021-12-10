namespace GoogleCreateCredentials
{
    partial class GoogleCreateCredentialsToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoogleCreateCredentialsToolForm));
            this.EnableTheGoogleAPIBtn = new System.Windows.Forms.Button();
            this.SendCredentialsBtn = new System.Windows.Forms.Button();
            this.GoogleAccountLbl = new System.Windows.Forms.Label();
            this.GoogleAccountTxt = new System.Windows.Forms.TextBox();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.AddCredentialBtn = new System.Windows.Forms.Button();
            this.PasswordTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.UserNameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddressTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FolderToFilterTxt = new System.Windows.Forms.TextBox();
            this.FolderToFilterLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GoogleAccountTypeCmb = new System.Windows.Forms.ComboBox();
            this.GoogleAccountTypeLbl = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.GooglePublicKeyTxt = new System.Windows.Forms.TextBox();
            this.GooglePublicKeyLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EnableTheGoogleAPIBtn
            // 
            this.EnableTheGoogleAPIBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.EnableTheGoogleAPIBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EnableTheGoogleAPIBtn.Location = new System.Drawing.Point(50, 196);
            this.EnableTheGoogleAPIBtn.Margin = new System.Windows.Forms.Padding(2);
            this.EnableTheGoogleAPIBtn.Name = "EnableTheGoogleAPIBtn";
            this.EnableTheGoogleAPIBtn.Size = new System.Drawing.Size(344, 31);
            this.EnableTheGoogleAPIBtn.TabIndex = 2;
            this.EnableTheGoogleAPIBtn.Text = "Enable the Google API";
            this.EnableTheGoogleAPIBtn.UseVisualStyleBackColor = false;
            this.EnableTheGoogleAPIBtn.Click += new System.EventHandler(this.EnableTheGoogleAPIBtn_Click);
            // 
            // SendCredentialsBtn
            // 
            this.SendCredentialsBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SendCredentialsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendCredentialsBtn.Location = new System.Drawing.Point(50, 483);
            this.SendCredentialsBtn.Margin = new System.Windows.Forms.Padding(2);
            this.SendCredentialsBtn.Name = "SendCredentialsBtn";
            this.SendCredentialsBtn.Size = new System.Drawing.Size(344, 31);
            this.SendCredentialsBtn.TabIndex = 8;
            this.SendCredentialsBtn.Text = "Send Credentials";
            this.SendCredentialsBtn.UseVisualStyleBackColor = false;
            this.SendCredentialsBtn.Click += new System.EventHandler(this.SendCredentialsBtn_Click);
            // 
            // GoogleAccountLbl
            // 
            this.GoogleAccountLbl.AutoSize = true;
            this.GoogleAccountLbl.Location = new System.Drawing.Point(51, 282);
            this.GoogleAccountLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GoogleAccountLbl.Name = "GoogleAccountLbl";
            this.GoogleAccountLbl.Size = new System.Drawing.Size(84, 13);
            this.GoogleAccountLbl.TabIndex = 2;
            this.GoogleAccountLbl.Text = "Google Account";
            // 
            // GoogleAccountTxt
            // 
            this.GoogleAccountTxt.Location = new System.Drawing.Point(170, 281);
            this.GoogleAccountTxt.Margin = new System.Windows.Forms.Padding(2);
            this.GoogleAccountTxt.Name = "GoogleAccountTxt";
            this.GoogleAccountTxt.Size = new System.Drawing.Size(152, 20);
            this.GoogleAccountTxt.TabIndex = 3;
            // 
            // ResetBtn
            // 
            this.ResetBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ResetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetBtn.Location = new System.Drawing.Point(50, 8);
            this.ResetBtn.Margin = new System.Windows.Forms.Padding(2);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(344, 31);
            this.ResetBtn.TabIndex = 1;
            this.ResetBtn.Text = "Reset";
            this.ResetBtn.UseVisualStyleBackColor = false;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // AddCredentialBtn
            // 
            this.AddCredentialBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AddCredentialBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddCredentialBtn.Location = new System.Drawing.Point(50, 356);
            this.AddCredentialBtn.Margin = new System.Windows.Forms.Padding(2);
            this.AddCredentialBtn.Name = "AddCredentialBtn";
            this.AddCredentialBtn.Size = new System.Drawing.Size(344, 31);
            this.AddCredentialBtn.TabIndex = 4;
            this.AddCredentialBtn.Text = "Add Credential";
            this.AddCredentialBtn.UseVisualStyleBackColor = false;
            this.AddCredentialBtn.Click += new System.EventHandler(this.AddCredentialBtn_Click);
            // 
            // PasswordTxt
            // 
            this.PasswordTxt.Location = new System.Drawing.Point(170, 460);
            this.PasswordTxt.Margin = new System.Windows.Forms.Padding(2);
            this.PasswordTxt.Name = "PasswordTxt";
            this.PasswordTxt.PasswordChar = '*';
            this.PasswordTxt.Size = new System.Drawing.Size(224, 20);
            this.PasswordTxt.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 462);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Supp Password";
            // 
            // UserNameTxt
            // 
            this.UserNameTxt.Location = new System.Drawing.Point(170, 438);
            this.UserNameTxt.Margin = new System.Windows.Forms.Padding(2);
            this.UserNameTxt.Name = "UserNameTxt";
            this.UserNameTxt.Size = new System.Drawing.Size(224, 20);
            this.UserNameTxt.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 439);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Supp Username";
            // 
            // AddressTxt
            // 
            this.AddressTxt.Location = new System.Drawing.Point(170, 414);
            this.AddressTxt.Margin = new System.Windows.Forms.Padding(2);
            this.AddressTxt.Name = "AddressTxt";
            this.AddressTxt.Size = new System.Drawing.Size(224, 20);
            this.AddressTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 416);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Supp Address";
            // 
            // FolderToFilterTxt
            // 
            this.FolderToFilterTxt.Location = new System.Drawing.Point(170, 304);
            this.FolderToFilterTxt.Margin = new System.Windows.Forms.Padding(2);
            this.FolderToFilterTxt.Name = "FolderToFilterTxt";
            this.FolderToFilterTxt.Size = new System.Drawing.Size(224, 20);
            this.FolderToFilterTxt.TabIndex = 12;
            // 
            // FolderToFilterLbl
            // 
            this.FolderToFilterLbl.AutoSize = true;
            this.FolderToFilterLbl.Location = new System.Drawing.Point(51, 306);
            this.FolderToFilterLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FolderToFilterLbl.Name = "FolderToFilterLbl";
            this.FolderToFilterLbl.Size = new System.Drawing.Size(58, 13);
            this.FolderToFilterLbl.TabIndex = 11;
            this.FolderToFilterLbl.Text = "Filter folder";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(325, 283);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "@google.com";
            // 
            // GoogleAccountTypeCmb
            // 
            this.GoogleAccountTypeCmb.FormattingEnabled = true;
            this.GoogleAccountTypeCmb.Location = new System.Drawing.Point(170, 330);
            this.GoogleAccountTypeCmb.Name = "GoogleAccountTypeCmb";
            this.GoogleAccountTypeCmb.Size = new System.Drawing.Size(224, 21);
            this.GoogleAccountTypeCmb.TabIndex = 14;
            // 
            // GoogleAccountTypeLbl
            // 
            this.GoogleAccountTypeLbl.AutoSize = true;
            this.GoogleAccountTypeLbl.Location = new System.Drawing.Point(53, 333);
            this.GoogleAccountTypeLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GoogleAccountTypeLbl.Name = "GoogleAccountTypeLbl";
            this.GoogleAccountTypeLbl.Size = new System.Drawing.Size(106, 13);
            this.GoogleAccountTypeLbl.TabIndex = 15;
            this.GoogleAccountTypeLbl.Text = "Google account type";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(50, 53);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(344, 138);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label5.Location = new System.Drawing.Point(12, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 31);
            this.label5.TabIndex = 17;
            this.label5.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label6.Location = new System.Drawing.Point(12, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 31);
            this.label6.TabIndex = 18;
            this.label6.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label7.Location = new System.Drawing.Point(12, 356);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 31);
            this.label7.TabIndex = 19;
            this.label7.Text = "3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label8.Location = new System.Drawing.Point(12, 483);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 31);
            this.label8.TabIndex = 20;
            this.label8.Text = "4";
            // 
            // GooglePublicKeyTxt
            // 
            this.GooglePublicKeyTxt.Location = new System.Drawing.Point(170, 246);
            this.GooglePublicKeyTxt.Margin = new System.Windows.Forms.Padding(2);
            this.GooglePublicKeyTxt.Name = "GooglePublicKeyTxt";
            this.GooglePublicKeyTxt.Size = new System.Drawing.Size(224, 20);
            this.GooglePublicKeyTxt.TabIndex = 22;
            // 
            // GooglePublicKeyLbl
            // 
            this.GooglePublicKeyLbl.AutoSize = true;
            this.GooglePublicKeyLbl.Location = new System.Drawing.Point(51, 248);
            this.GooglePublicKeyLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GooglePublicKeyLbl.Name = "GooglePublicKeyLbl";
            this.GooglePublicKeyLbl.Size = new System.Drawing.Size(94, 13);
            this.GooglePublicKeyLbl.TabIndex = 21;
            this.GooglePublicKeyLbl.Text = "Google Public Key";
            // 
            // GoogleCreateCredentialsToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(408, 577);
            this.Controls.Add(this.GooglePublicKeyTxt);
            this.Controls.Add(this.GooglePublicKeyLbl);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.GoogleAccountTypeLbl);
            this.Controls.Add(this.GoogleAccountTypeCmb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FolderToFilterTxt);
            this.Controls.Add(this.FolderToFilterLbl);
            this.Controls.Add(this.AddressTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UserNameTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PasswordTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AddCredentialBtn);
            this.Controls.Add(this.ResetBtn);
            this.Controls.Add(this.GoogleAccountTxt);
            this.Controls.Add(this.GoogleAccountLbl);
            this.Controls.Add(this.SendCredentialsBtn);
            this.Controls.Add(this.EnableTheGoogleAPIBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GoogleCreateCredentialsToolForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Google Create Credentials Tool";
            this.Load += new System.EventHandler(this.GoogleCreateCredentialsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnableTheGoogleAPIBtn;
        private System.Windows.Forms.Button SendCredentialsBtn;
        private System.Windows.Forms.Label GoogleAccountLbl;
        private System.Windows.Forms.TextBox GoogleAccountTxt;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.Button AddCredentialBtn;
        private System.Windows.Forms.TextBox PasswordTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UserNameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AddressTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FolderToFilterTxt;
        private System.Windows.Forms.Label FolderToFilterLbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox GoogleAccountTypeCmb;
        private System.Windows.Forms.Label GoogleAccountTypeLbl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox GooglePublicKeyTxt;
        private System.Windows.Forms.Label GooglePublicKeyLbl;
    }
}

