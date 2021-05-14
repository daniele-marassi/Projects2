namespace GoogleDriveCreateCredentials
{
    partial class GoogleDriveCreateCredentialsForm
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
            this.EnableTheDriveAPIBtn = new System.Windows.Forms.Button();
            this.SendCredentialsBtn = new System.Windows.Forms.Button();
            this.GoogleDriveAccountLbl = new System.Windows.Forms.Label();
            this.GoogleDriveAccountTxt = new System.Windows.Forms.TextBox();
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
            this.SuspendLayout();
            // 
            // EnableTheDriveAPIBtn
            // 
            this.EnableTheDriveAPIBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EnableTheDriveAPIBtn.Location = new System.Drawing.Point(12, 96);
            this.EnableTheDriveAPIBtn.Name = "EnableTheDriveAPIBtn";
            this.EnableTheDriveAPIBtn.Size = new System.Drawing.Size(459, 38);
            this.EnableTheDriveAPIBtn.TabIndex = 2;
            this.EnableTheDriveAPIBtn.Text = "Enable the Drive API";
            this.EnableTheDriveAPIBtn.UseVisualStyleBackColor = true;
            this.EnableTheDriveAPIBtn.Click += new System.EventHandler(this.EnableTheDriveAPIBtn_Click);
            // 
            // SendCredentialsBtn
            // 
            this.SendCredentialsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendCredentialsBtn.Location = new System.Drawing.Point(15, 464);
            this.SendCredentialsBtn.Name = "SendCredentialsBtn";
            this.SendCredentialsBtn.Size = new System.Drawing.Size(459, 38);
            this.SendCredentialsBtn.TabIndex = 8;
            this.SendCredentialsBtn.Text = "Send Credentials";
            this.SendCredentialsBtn.UseVisualStyleBackColor = true;
            this.SendCredentialsBtn.Click += new System.EventHandler(this.SendCredentialsBtn_Click);
            // 
            // GoogleDriveAccountLbl
            // 
            this.GoogleDriveAccountLbl.AutoSize = true;
            this.GoogleDriveAccountLbl.Location = new System.Drawing.Point(12, 212);
            this.GoogleDriveAccountLbl.Name = "GoogleDriveAccountLbl";
            this.GoogleDriveAccountLbl.Size = new System.Drawing.Size(146, 17);
            this.GoogleDriveAccountLbl.TabIndex = 2;
            this.GoogleDriveAccountLbl.Text = "Google Drive Account";
            // 
            // GoogleDriveAccountTxt
            // 
            this.GoogleDriveAccountTxt.Location = new System.Drawing.Point(170, 210);
            this.GoogleDriveAccountTxt.Name = "GoogleDriveAccountTxt";
            this.GoogleDriveAccountTxt.Size = new System.Drawing.Size(201, 22);
            this.GoogleDriveAccountTxt.TabIndex = 3;
            // 
            // ResetBtn
            // 
            this.ResetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetBtn.Location = new System.Drawing.Point(12, 12);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(459, 38);
            this.ResetBtn.TabIndex = 1;
            this.ResetBtn.Text = "Reset";
            this.ResetBtn.UseVisualStyleBackColor = true;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // AddCredentialBtn
            // 
            this.AddCredentialBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddCredentialBtn.Location = new System.Drawing.Point(12, 267);
            this.AddCredentialBtn.Name = "AddCredentialBtn";
            this.AddCredentialBtn.Size = new System.Drawing.Size(459, 38);
            this.AddCredentialBtn.TabIndex = 4;
            this.AddCredentialBtn.Text = "Add Credential";
            this.AddCredentialBtn.UseVisualStyleBackColor = true;
            this.AddCredentialBtn.Click += new System.EventHandler(this.AddCredentialBtn_Click);
            // 
            // PasswordTxt
            // 
            this.PasswordTxt.Location = new System.Drawing.Point(173, 436);
            this.PasswordTxt.Name = "PasswordTxt";
            this.PasswordTxt.Size = new System.Drawing.Size(301, 22);
            this.PasswordTxt.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 438);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Password";
            // 
            // UserNameTxt
            // 
            this.UserNameTxt.Location = new System.Drawing.Point(173, 408);
            this.UserNameTxt.Name = "UserNameTxt";
            this.UserNameTxt.Size = new System.Drawing.Size(301, 22);
            this.UserNameTxt.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 410);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Username";
            // 
            // AddressTxt
            // 
            this.AddressTxt.Location = new System.Drawing.Point(173, 379);
            this.AddressTxt.Name = "AddressTxt";
            this.AddressTxt.Size = new System.Drawing.Size(301, 22);
            this.AddressTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 382);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Address";
            // 
            // FolderToFilterTxt
            // 
            this.FolderToFilterTxt.Location = new System.Drawing.Point(170, 239);
            this.FolderToFilterTxt.Name = "FolderToFilterTxt";
            this.FolderToFilterTxt.Size = new System.Drawing.Size(301, 22);
            this.FolderToFilterTxt.TabIndex = 12;
            // 
            // FolderToFilterLbl
            // 
            this.FolderToFilterLbl.AutoSize = true;
            this.FolderToFilterLbl.Location = new System.Drawing.Point(12, 241);
            this.FolderToFilterLbl.Name = "FolderToFilterLbl";
            this.FolderToFilterLbl.Size = new System.Drawing.Size(79, 17);
            this.FolderToFilterLbl.TabIndex = 11;
            this.FolderToFilterLbl.Text = "Filter folder";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "@google.com";
            // 
            // GoogleDriveCreateCredentialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(483, 514);
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
            this.Controls.Add(this.GoogleDriveAccountTxt);
            this.Controls.Add(this.GoogleDriveAccountLbl);
            this.Controls.Add(this.SendCredentialsBtn);
            this.Controls.Add(this.EnableTheDriveAPIBtn);
            this.Name = "GoogleDriveCreateCredentialsForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Google Drive Create Credentials";
            this.Load += new System.EventHandler(this.GoogleDriveCreateCredentialsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnableTheDriveAPIBtn;
        private System.Windows.Forms.Button SendCredentialsBtn;
        private System.Windows.Forms.Label GoogleDriveAccountLbl;
        private System.Windows.Forms.TextBox GoogleDriveAccountTxt;
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
    }
}

