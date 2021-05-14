using SearchAttestati.Properties;
using System.Windows.Forms;

namespace SearchAttestati
{
    partial class SearchAttestati
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
            this.SearchTxt = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SelDeselBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FoundedAttestatiLbl = new System.Windows.Forms.Label();
            this.SaveProgressLbl = new System.Windows.Forms.Label();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.OpenPdf = new System.Windows.Forms.DataGridViewImageColumn();
            this.PositionWaitTmr = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchTxt
            // 
            this.SearchTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchTxt.Font = new System.Drawing.Font("Verdana", 11F);
            this.SearchTxt.Location = new System.Drawing.Point(12, 12);
            this.SearchTxt.Name = "SearchTxt";
            this.SearchTxt.Size = new System.Drawing.Size(791, 25);
            this.SearchTxt.TabIndex = 0;
            this.SearchTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTxt_KeyPress);
            // 
            // SearchBtn
            // 
            this.SearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBtn.Location = new System.Drawing.Point(809, 10);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(124, 27);
            this.SearchBtn.TabIndex = 1;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.OpenPdf});
            this.dataGrid.Location = new System.Drawing.Point(142, 54);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(790, 397);
            this.dataGrid.TabIndex = 2;
            this.dataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_CellClick);
            this.dataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_CellEndEdit);
            this.dataGrid.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_CellMouseEnter);
            this.dataGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGrid_MouseMove);
            // 
            // Selected
            // 
            this.Selected.HeaderText = "Selezione";
            this.Selected.Name = "Selected";
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Selected.Width = 70;
            // 
            // SelDeselBtn
            // 
            this.SelDeselBtn.Location = new System.Drawing.Point(12, 54);
            this.SelDeselBtn.Name = "SelDeselBtn";
            this.SelDeselBtn.Size = new System.Drawing.Size(124, 27);
            this.SelDeselBtn.TabIndex = 3;
            this.SelDeselBtn.Text = "Select all";
            this.SelDeselBtn.UseVisualStyleBackColor = true;
            this.SelDeselBtn.Click += new System.EventHandler(this.SelDeselBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(12, 87);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(124, 27);
            this.SaveBtn.TabIndex = 4;
            this.SaveBtn.Text = "Save selected items";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // FoundedAttestatiLbl
            // 
            this.FoundedAttestatiLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FoundedAttestatiLbl.Location = new System.Drawing.Point(677, 460);
            this.FoundedAttestatiLbl.Name = "FoundedAttestatiLbl";
            this.FoundedAttestatiLbl.Size = new System.Drawing.Size(255, 22);
            this.FoundedAttestatiLbl.TabIndex = 5;
            this.FoundedAttestatiLbl.Text = "0 Attestati Founded";
            this.FoundedAttestatiLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SaveProgressLbl
            // 
            this.SaveProgressLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveProgressLbl.Location = new System.Drawing.Point(141, 460);
            this.SaveProgressLbl.Name = "SaveProgressLbl";
            this.SaveProgressLbl.Size = new System.Drawing.Size(319, 22);
            this.SaveProgressLbl.TabIndex = 6;
            this.SaveProgressLbl.Text = "0 Saved on 0 Selected";
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::SearchAttestati.Properties.Resources.OpenPdf_icon;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 30;
            // 
            // OpenPdf
            // 
            this.OpenPdf.HeaderText = "";
            this.OpenPdf.Image = global::SearchAttestati.Properties.Resources.OpenPdf_icon;
            this.OpenPdf.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.OpenPdf.Name = "OpenPdf";
            this.OpenPdf.Width = 30;
            // 
            // PositionWaitTmr
            // 
            this.PositionWaitTmr.Tick += new System.EventHandler(this.PositionWaitTmr_Tick);
            // 
            // SearchAttestati
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(944, 482);
            this.Controls.Add(this.SaveProgressLbl);
            this.Controls.Add(this.FoundedAttestatiLbl);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.SelDeselBtn);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.SearchTxt);
            this.Name = "SearchAttestati";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Attestati";
            this.Load += new System.EventHandler(this.SearchAttestati_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SearchTxt;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewImageColumn OpenPdf;
        private System.Windows.Forms.Button SelDeselBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private Label FoundedAttestatiLbl;
        private Label SaveProgressLbl;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private Timer PositionWaitTmr;
    }
}

