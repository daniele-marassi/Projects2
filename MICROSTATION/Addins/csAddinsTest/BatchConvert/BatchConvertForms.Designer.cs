namespace BatchConvert
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
            this.SelFilesBtn = new System.Windows.Forms.Button();
            this.SelectedFilesLst = new System.Windows.Forms.ListBox();
            this.ConvertToFormatCmb = new System.Windows.Forms.ComboBox();
            this.MakeBcnvFileBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DestinationDirTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // SelFilesBtn
            // 
            this.SelFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelFilesBtn.Location = new System.Drawing.Point(12, 12);
            this.SelFilesBtn.Name = "SelFilesBtn";
            this.SelFilesBtn.Size = new System.Drawing.Size(248, 31);
            this.SelFilesBtn.TabIndex = 0;
            this.SelFilesBtn.Text = "Seleziona files";
            this.SelFilesBtn.UseVisualStyleBackColor = true;
            this.SelFilesBtn.Click += new System.EventHandler(this.SelFilesBtn_Click);
            // 
            // SelectedFilesLst
            // 
            this.SelectedFilesLst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedFilesLst.FormattingEnabled = true;
            this.SelectedFilesLst.Location = new System.Drawing.Point(76, 60);
            this.SelectedFilesLst.Name = "SelectedFilesLst";
            this.SelectedFilesLst.Size = new System.Drawing.Size(184, 316);
            this.SelectedFilesLst.TabIndex = 1;
            // 
            // ConvertToFormatCmb
            // 
            this.ConvertToFormatCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConvertToFormatCmb.FormattingEnabled = true;
            this.ConvertToFormatCmb.Location = new System.Drawing.Point(76, 431);
            this.ConvertToFormatCmb.Name = "ConvertToFormatCmb";
            this.ConvertToFormatCmb.Size = new System.Drawing.Size(184, 21);
            this.ConvertToFormatCmb.TabIndex = 2;
            // 
            // MakeBcnvFileBtn
            // 
            this.MakeBcnvFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MakeBcnvFileBtn.Location = new System.Drawing.Point(14, 475);
            this.MakeBcnvFileBtn.Name = "MakeBcnvFileBtn";
            this.MakeBcnvFileBtn.Size = new System.Drawing.Size(248, 31);
            this.MakeBcnvFileBtn.TabIndex = 3;
            this.MakeBcnvFileBtn.Text = "Crea bcnv file";
            this.MakeBcnvFileBtn.UseVisualStyleBackColor = true;
            this.MakeBcnvFileBtn.Click += new System.EventHandler(this.MakeBcnvFileBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 434);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Format";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "File selezionati";
            // 
            // DestinationDirTxt
            // 
            this.DestinationDirTxt.Location = new System.Drawing.Point(76, 400);
            this.DestinationDirTxt.Name = "DestinationDirTxt";
            this.DestinationDirTxt.Size = new System.Drawing.Size(184, 20);
            this.DestinationDirTxt.TabIndex = 6;
            this.DestinationDirTxt.Text = "C:\\BentleyV8i\\WorkSpace\\projects\\untitled\\out\\";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 403);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Destination Dir";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // BatchConvertForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 516);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DestinationDirTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MakeBcnvFileBtn);
            this.Controls.Add(this.ConvertToFormatCmb);
            this.Controls.Add(this.SelectedFilesLst);
            this.Controls.Add(this.SelFilesBtn);
            this.Name = "BatchConvertForms";
            this.Text = "Batch Convert";
            this.Load += new System.EventHandler(this.BatchConvertForms_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelFilesBtn;
        private System.Windows.Forms.ListBox SelectedFilesLst;
        private System.Windows.Forms.ComboBox ConvertToFormatCmb;
        private System.Windows.Forms.Button MakeBcnvFileBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DestinationDirTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}