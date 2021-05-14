namespace csAddinsTest
{
    partial class BatchConvertForms
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
            this.components = new System.ComponentModel.Container();
            this.SelFilesBtn = new System.Windows.Forms.Button();
            this.FilesSelectedLst = new System.Windows.Forms.ListBox();
            this.FormatToConvertCmb = new System.Windows.Forms.ComboBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.MakeBcnvFileBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // SelFilesBtn
            // 
            this.SelFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelFilesBtn.Location = new System.Drawing.Point(12, 12);
            this.SelFilesBtn.Name = "SelFilesBtn";
            this.SelFilesBtn.Size = new System.Drawing.Size(207, 31);
            this.SelFilesBtn.TabIndex = 0;
            this.SelFilesBtn.Text = "Seleziona files";
            this.SelFilesBtn.UseVisualStyleBackColor = true;
            this.SelFilesBtn.Click += new System.EventHandler(this.SelFilesBtn_Click);
            // 
            // FilesSelectedLst
            // 
            this.FilesSelectedLst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesSelectedLst.FormattingEnabled = true;
            this.FilesSelectedLst.Location = new System.Drawing.Point(14, 60);
            this.FilesSelectedLst.Name = "FilesSelectedLst";
            this.FilesSelectedLst.Size = new System.Drawing.Size(205, 212);
            this.FilesSelectedLst.TabIndex = 1;
            // 
            // FormatToConvertCmb
            // 
            this.FormatToConvertCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormatToConvertCmb.FormattingEnabled = true;
            this.FormatToConvertCmb.Location = new System.Drawing.Point(14, 302);
            this.FormatToConvertCmb.Name = "FormatToConvertCmb";
            this.FormatToConvertCmb.Size = new System.Drawing.Size(205, 21);
            this.FormatToConvertCmb.TabIndex = 2;
            // 
            // MakeBcnvFileBtn
            // 
            this.MakeBcnvFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MakeBcnvFileBtn.Location = new System.Drawing.Point(14, 346);
            this.MakeBcnvFileBtn.Name = "MakeBcnvFileBtn";
            this.MakeBcnvFileBtn.Size = new System.Drawing.Size(207, 31);
            this.MakeBcnvFileBtn.TabIndex = 3;
            this.MakeBcnvFileBtn.Text = "Crea bcnv file";
            this.MakeBcnvFileBtn.UseVisualStyleBackColor = true;
            this.MakeBcnvFileBtn.Click += new System.EventHandler(this.MakeBcnvFileBtn_Click);
            // 
            // BatchConvertForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 387);
            this.Controls.Add(this.MakeBcnvFileBtn);
            this.Controls.Add(this.FormatToConvertCmb);
            this.Controls.Add(this.FilesSelectedLst);
            this.Controls.Add(this.SelFilesBtn);
            this.Name = "BatchConvertForms";
            this.Text = "Batch Convert";
            this.Load += new System.EventHandler(this.BatchConvertForms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SelFilesBtn;
        private System.Windows.Forms.ListBox FilesSelectedLst;
        private System.Windows.Forms.ComboBox FormatToConvertCmb;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button MakeBcnvFileBtn;
    }
}