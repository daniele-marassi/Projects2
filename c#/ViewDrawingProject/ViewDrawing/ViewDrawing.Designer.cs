namespace ViewDrawing
{
    partial class ViewDrawing
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
            this.treeViewDrawing = new System.Windows.Forms.TreeView();
            this.ViewPnl = new System.Windows.Forms.Panel();
            this.SearchTxt = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.ExpandAllBtn = new System.Windows.Forms.Button();
            this.CollapseAllBtn = new System.Windows.Forms.Button();
            this.ExpandLbl = new System.Windows.Forms.Label();
            this.RestoreImagePositionBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewDrawing
            // 
            this.treeViewDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewDrawing.Location = new System.Drawing.Point(12, 67);
            this.treeViewDrawing.Name = "treeViewDrawing";
            this.treeViewDrawing.Size = new System.Drawing.Size(196, 546);
            this.treeViewDrawing.TabIndex = 0;
            this.treeViewDrawing.DoubleClick += new System.EventHandler(this.treeViewDrawing_DoubleClick);
            // 
            // ViewPnl
            // 
            this.ViewPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewPnl.AutoSize = true;
            this.ViewPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewPnl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ViewPnl.Location = new System.Drawing.Point(212, 38);
            this.ViewPnl.Name = "ViewPnl";
            this.ViewPnl.Size = new System.Drawing.Size(780, 575);
            this.ViewPnl.TabIndex = 2;
            // 
            // SearchTxt
            // 
            this.SearchTxt.Location = new System.Drawing.Point(12, 12);
            this.SearchTxt.Name = "SearchTxt";
            this.SearchTxt.Size = new System.Drawing.Size(93, 20);
            this.SearchTxt.TabIndex = 3;
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(111, 12);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(95, 23);
            this.SearchBtn.TabIndex = 4;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // ExpandAllBtn
            // 
            this.ExpandAllBtn.Location = new System.Drawing.Point(58, 38);
            this.ExpandAllBtn.Name = "ExpandAllBtn";
            this.ExpandAllBtn.Size = new System.Drawing.Size(24, 23);
            this.ExpandAllBtn.TabIndex = 5;
            this.ExpandAllBtn.Text = "+";
            this.ExpandAllBtn.UseVisualStyleBackColor = true;
            this.ExpandAllBtn.Click += new System.EventHandler(this.ExpandAllBtn_Click);
            // 
            // CollapseAllBtn
            // 
            this.CollapseAllBtn.Location = new System.Drawing.Point(81, 38);
            this.CollapseAllBtn.Name = "CollapseAllBtn";
            this.CollapseAllBtn.Size = new System.Drawing.Size(24, 23);
            this.CollapseAllBtn.TabIndex = 6;
            this.CollapseAllBtn.Text = "-";
            this.CollapseAllBtn.UseVisualStyleBackColor = true;
            this.CollapseAllBtn.Click += new System.EventHandler(this.CollapseAllBtn_Click);
            // 
            // ExpandLbl
            // 
            this.ExpandLbl.AutoSize = true;
            this.ExpandLbl.Location = new System.Drawing.Point(11, 45);
            this.ExpandLbl.Name = "ExpandLbl";
            this.ExpandLbl.Size = new System.Drawing.Size(43, 13);
            this.ExpandLbl.TabIndex = 7;
            this.ExpandLbl.Text = "Expand";
            // 
            // RestoreImagePositionBtn
            // 
            this.RestoreImagePositionBtn.Location = new System.Drawing.Point(856, 9);
            this.RestoreImagePositionBtn.Name = "RestoreImagePositionBtn";
            this.RestoreImagePositionBtn.Size = new System.Drawing.Size(136, 23);
            this.RestoreImagePositionBtn.TabIndex = 8;
            this.RestoreImagePositionBtn.Text = "Restore Image Position";
            this.RestoreImagePositionBtn.UseVisualStyleBackColor = true;
            this.RestoreImagePositionBtn.Click += new System.EventHandler(this.RestoreImagePositionBtn_Click);
            // 
            // ViewDrawing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 625);
            this.Controls.Add(this.RestoreImagePositionBtn);
            this.Controls.Add(this.ExpandLbl);
            this.Controls.Add(this.CollapseAllBtn);
            this.Controls.Add(this.ExpandAllBtn);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.SearchTxt);
            this.Controls.Add(this.ViewPnl);
            this.Controls.Add(this.treeViewDrawing);
            this.Name = "ViewDrawing";
            this.Text = "ViewDrawing";
            this.Load += new System.EventHandler(this.ViewDrawing_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewDrawing;
        private System.Windows.Forms.Panel ViewPnl;
        private System.Windows.Forms.TextBox SearchTxt;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.Button ExpandAllBtn;
        private System.Windows.Forms.Button CollapseAllBtn;
        private System.Windows.Forms.Label ExpandLbl;
        private System.Windows.Forms.Button RestoreImagePositionBtn;
    }
}

