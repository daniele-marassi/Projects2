namespace Tools
{
    partial class ExceptionsManagementFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.BackgroundLbl = new System.Windows.Forms.Label();
            this.LogoRingLbl = new System.Windows.Forms.Label();
            this.LogoSign1Lbl = new System.Windows.Forms.Label();
            this.LogoSign2Lbl = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.Closelbl = new System.Windows.Forms.Label();
            this.ExceptionTypesCmb = new System.Windows.Forms.ComboBox();
            this.ExceptionsList = new System.Windows.Forms.ListBox();
            this.AddExceptionsBtn = new System.Windows.Forms.Button();
            this.DelExceptionBtn = new System.Windows.Forms.Button();
            this.ExceptionTxt = new System.Windows.Forms.TextBox();
            this.SelectFileBtn = new System.Windows.Forms.Button();
            this.AddException = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BackgroundLbl
            // 
            this.BackgroundLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundLbl.Font = new System.Drawing.Font("We Spray", 136F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackgroundLbl.Location = new System.Drawing.Point(12, 0);
            this.BackgroundLbl.Name = "BackgroundLbl";
            this.BackgroundLbl.Size = new System.Drawing.Size(1093, 692);
            this.BackgroundLbl.TabIndex = 34;
            this.BackgroundLbl.Text = "9";
            // 
            // LogoRingLbl
            // 
            this.LogoRingLbl.AutoSize = true;
            this.LogoRingLbl.BackColor = System.Drawing.Color.Transparent;
            this.LogoRingLbl.Font = new System.Drawing.Font("Cafeina Dig_v2", 250F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogoRingLbl.Location = new System.Drawing.Point(-24, -158);
            this.LogoRingLbl.Name = "LogoRingLbl";
            this.LogoRingLbl.Size = new System.Drawing.Size(611, 638);
            this.LogoRingLbl.TabIndex = 27;
            this.LogoRingLbl.Text = "S";
            // 
            // LogoSign1Lbl
            // 
            this.LogoSign1Lbl.AutoSize = true;
            this.LogoSign1Lbl.Font = new System.Drawing.Font("Papyrus", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogoSign1Lbl.Location = new System.Drawing.Point(133, 93);
            this.LogoSign1Lbl.Name = "LogoSign1Lbl";
            this.LogoSign1Lbl.Size = new System.Drawing.Size(161, 94);
            this.LogoSign1Lbl.TabIndex = 28;
            this.LogoSign1Lbl.Text = "DM";
            // 
            // LogoSign2Lbl
            // 
            this.LogoSign2Lbl.AutoSize = true;
            this.LogoSign2Lbl.Font = new System.Drawing.Font("Papyrus", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogoSign2Lbl.Location = new System.Drawing.Point(136, 151);
            this.LogoSign2Lbl.Name = "LogoSign2Lbl";
            this.LogoSign2Lbl.Size = new System.Drawing.Size(159, 74);
            this.LogoSign2Lbl.TabIndex = 29;
            this.LogoSign2Lbl.Text = "artifex";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Verdana", 15F);
            this.labelTitle.Location = new System.Drawing.Point(351, 168);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(66, 31);
            this.labelTitle.TabIndex = 19;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Closelbl
            // 
            this.Closelbl.AutoSize = true;
            this.Closelbl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Closelbl.Font = new System.Drawing.Font("Byom Icons", 30F);
            this.Closelbl.Location = new System.Drawing.Point(76, 277);
            this.Closelbl.Name = "Closelbl";
            this.Closelbl.Size = new System.Drawing.Size(80, 59);
            this.Closelbl.TabIndex = 35;
            this.Closelbl.Text = "X";
            this.Closelbl.Click += new System.EventHandler(this.Closelbl_Click);
            this.Closelbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Closelbl_MouseDown);
            this.Closelbl.MouseLeave += new System.EventHandler(this.Closelbl_MouseLeave);
            this.Closelbl.MouseHover += new System.EventHandler(this.Closelbl_MouseHover);
            this.Closelbl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Closelbl_MouseUp);
            // 
            // ExceptionTypesCmb
            // 
            this.ExceptionTypesCmb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExceptionTypesCmb.FormattingEnabled = true;
            this.ExceptionTypesCmb.ItemHeight = 24;
            this.ExceptionTypesCmb.Location = new System.Drawing.Point(213, 250);
            this.ExceptionTypesCmb.Name = "ExceptionTypesCmb";
            this.ExceptionTypesCmb.Size = new System.Drawing.Size(121, 32);
            this.ExceptionTypesCmb.TabIndex = 36;
            // 
            // ExceptionsList
            // 
            this.ExceptionsList.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExceptionsList.FormattingEnabled = true;
            this.ExceptionsList.ItemHeight = 16;
            this.ExceptionsList.Location = new System.Drawing.Point(340, 288);
            this.ExceptionsList.Name = "ExceptionsList";
            this.ExceptionsList.Size = new System.Drawing.Size(327, 132);
            this.ExceptionsList.TabIndex = 37;
            // 
            // AddExceptionsBtn
            // 
            this.AddExceptionsBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddExceptionsBtn.Location = new System.Drawing.Point(213, 382);
            this.AddExceptionsBtn.Name = "AddExceptionsBtn";
            this.AddExceptionsBtn.Size = new System.Drawing.Size(121, 31);
            this.AddExceptionsBtn.TabIndex = 38;
            this.AddExceptionsBtn.Text = "Add Exceptions";
            this.AddExceptionsBtn.UseVisualStyleBackColor = true;
            this.AddExceptionsBtn.Click += new System.EventHandler(this.AddExceptionsBtn_Click);
            // 
            // DelExceptionBtn
            // 
            this.DelExceptionBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DelExceptionBtn.Location = new System.Drawing.Point(213, 288);
            this.DelExceptionBtn.Name = "DelExceptionBtn";
            this.DelExceptionBtn.Size = new System.Drawing.Size(121, 88);
            this.DelExceptionBtn.TabIndex = 39;
            this.DelExceptionBtn.Text = "Del Exception Seleected";
            this.DelExceptionBtn.UseMnemonic = false;
            this.DelExceptionBtn.UseVisualStyleBackColor = true;
            this.DelExceptionBtn.Click += new System.EventHandler(this.DelExceptionBtn_Click);
            // 
            // ExceptionTxt
            // 
            this.ExceptionTxt.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExceptionTxt.Location = new System.Drawing.Point(340, 250);
            this.ExceptionTxt.Multiline = true;
            this.ExceptionTxt.Name = "ExceptionTxt";
            this.ExceptionTxt.Size = new System.Drawing.Size(233, 32);
            this.ExceptionTxt.TabIndex = 40;
            // 
            // SelectFileBtn
            // 
            this.SelectFileBtn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectFileBtn.Location = new System.Drawing.Point(579, 250);
            this.SelectFileBtn.Name = "SelectFileBtn";
            this.SelectFileBtn.Size = new System.Drawing.Size(38, 32);
            this.SelectFileBtn.TabIndex = 41;
            this.SelectFileBtn.Text = "File";
            this.SelectFileBtn.UseVisualStyleBackColor = true;
            this.SelectFileBtn.Click += new System.EventHandler(this.SelectFileBtn_Click);
            // 
            // AddException
            // 
            this.AddException.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddException.Location = new System.Drawing.Point(623, 250);
            this.AddException.Name = "AddException";
            this.AddException.Size = new System.Drawing.Size(44, 32);
            this.AddException.TabIndex = 42;
            this.AddException.Text = "Add";
            this.AddException.UseVisualStyleBackColor = true;
            this.AddException.Click += new System.EventHandler(this.AddException_Click);
            // 
            // ExceptionsManagementFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1093, 692);
            this.Controls.Add(this.AddException);
            this.Controls.Add(this.SelectFileBtn);
            this.Controls.Add(this.ExceptionTxt);
            this.Controls.Add(this.DelExceptionBtn);
            this.Controls.Add(this.AddExceptionsBtn);
            this.Controls.Add(this.ExceptionsList);
            this.Controls.Add(this.ExceptionTypesCmb);
            this.Controls.Add(this.Closelbl);
            this.Controls.Add(this.LogoSign1Lbl);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.LogoSign2Lbl);
            this.Controls.Add(this.LogoRingLbl);
            this.Controls.Add(this.BackgroundLbl);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionsManagementFrm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.ExceptionsManagementFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BackgroundLbl;
        private System.Windows.Forms.Label LogoRingLbl;
        private System.Windows.Forms.Label LogoSign1Lbl;
        private System.Windows.Forms.Label LogoSign2Lbl;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label Closelbl;
        private System.Windows.Forms.ComboBox ExceptionTypesCmb;
        private System.Windows.Forms.ListBox ExceptionsList;
        private System.Windows.Forms.Button AddExceptionsBtn;
        private System.Windows.Forms.Button DelExceptionBtn;
        private System.Windows.Forms.TextBox ExceptionTxt;
        private System.Windows.Forms.Button SelectFileBtn;
        private System.Windows.Forms.Button AddException;
    }
}
