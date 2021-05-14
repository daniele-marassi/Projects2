namespace readFileEvt
{
    partial class ReadFileEvtFrm
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
            this.SearchBtn = new System.Windows.Forms.Button();
            this.FilterTxt = new System.Windows.Forms.TextBox();
            this.EventList = new System.Windows.Forms.ListBox();
            this.FilterCmb = new System.Windows.Forms.ComboBox();
            this.FilterList = new System.Windows.Forms.ListBox();
            this.AddFilterBtn = new System.Windows.Forms.Button();
            this.ClearAllFilterBtn = new System.Windows.Forms.Button();
            this.ClearSelectFilterBtn = new System.Windows.Forms.Button();
            this.ClearEventList = new System.Windows.Forms.Button();
            this.CopySelectedEvents = new System.Windows.Forms.Button();
            this.SelectAllEventsBtn = new System.Windows.Forms.Button();
            this.ClearSelectBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(12, 159);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(524, 23);
            this.SearchBtn.TabIndex = 0;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // FilterTxt
            // 
            this.FilterTxt.Location = new System.Drawing.Point(265, 12);
            this.FilterTxt.Name = "FilterTxt";
            this.FilterTxt.Size = new System.Drawing.Size(190, 20);
            this.FilterTxt.TabIndex = 2;
            // 
            // EventList
            // 
            this.EventList.AllowDrop = true;
            this.EventList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EventList.FormattingEnabled = true;
            this.EventList.HorizontalScrollbar = true;
            this.EventList.Location = new System.Drawing.Point(93, 188);
            this.EventList.Name = "EventList";
            this.EventList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.EventList.Size = new System.Drawing.Size(445, 186);
            this.EventList.TabIndex = 3;
            // 
            // FilterCmb
            // 
            this.FilterCmb.FormattingEnabled = true;
            this.FilterCmb.Location = new System.Drawing.Point(12, 13);
            this.FilterCmb.Name = "FilterCmb";
            this.FilterCmb.Size = new System.Drawing.Size(247, 21);
            this.FilterCmb.Sorted = true;
            this.FilterCmb.TabIndex = 4;
            // 
            // FilterList
            // 
            this.FilterList.FormattingEnabled = true;
            this.FilterList.HorizontalScrollbar = true;
            this.FilterList.Location = new System.Drawing.Point(12, 45);
            this.FilterList.Name = "FilterList";
            this.FilterList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.FilterList.Size = new System.Drawing.Size(443, 108);
            this.FilterList.TabIndex = 5;
            // 
            // AddFilterBtn
            // 
            this.AddFilterBtn.Location = new System.Drawing.Point(461, 13);
            this.AddFilterBtn.Name = "AddFilterBtn";
            this.AddFilterBtn.Size = new System.Drawing.Size(75, 23);
            this.AddFilterBtn.TabIndex = 6;
            this.AddFilterBtn.Text = "Add Filter";
            this.AddFilterBtn.UseVisualStyleBackColor = true;
            this.AddFilterBtn.Click += new System.EventHandler(this.AddFilterBtn_Click);
            // 
            // ClearAllFilterBtn
            // 
            this.ClearAllFilterBtn.Location = new System.Drawing.Point(461, 74);
            this.ClearAllFilterBtn.Name = "ClearAllFilterBtn";
            this.ClearAllFilterBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearAllFilterBtn.TabIndex = 7;
            this.ClearAllFilterBtn.Text = "Clear All";
            this.ClearAllFilterBtn.UseVisualStyleBackColor = true;
            this.ClearAllFilterBtn.Click += new System.EventHandler(this.ClearAllFilterBtn_Click);
            // 
            // ClearSelectFilterBtn
            // 
            this.ClearSelectFilterBtn.Location = new System.Drawing.Point(461, 45);
            this.ClearSelectFilterBtn.Name = "ClearSelectFilterBtn";
            this.ClearSelectFilterBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearSelectFilterBtn.TabIndex = 8;
            this.ClearSelectFilterBtn.Text = "Clear Select";
            this.ClearSelectFilterBtn.UseVisualStyleBackColor = true;
            this.ClearSelectFilterBtn.Click += new System.EventHandler(this.ClearSelectFilterBtn_Click);
            // 
            // ClearEventList
            // 
            this.ClearEventList.Location = new System.Drawing.Point(12, 188);
            this.ClearEventList.Name = "ClearEventList";
            this.ClearEventList.Size = new System.Drawing.Size(75, 23);
            this.ClearEventList.TabIndex = 9;
            this.ClearEventList.Text = "Clear Events";
            this.ClearEventList.UseVisualStyleBackColor = true;
            this.ClearEventList.Click += new System.EventHandler(this.ClearEventList_Click);
            // 
            // CopySelectedEvents
            // 
            this.CopySelectedEvents.Location = new System.Drawing.Point(12, 259);
            this.CopySelectedEvents.Name = "CopySelectedEvents";
            this.CopySelectedEvents.Size = new System.Drawing.Size(75, 57);
            this.CopySelectedEvents.TabIndex = 10;
            this.CopySelectedEvents.Text = "Copy Selected Events";
            this.CopySelectedEvents.UseVisualStyleBackColor = true;
            this.CopySelectedEvents.Click += new System.EventHandler(this.CopySelectedEvents_Click);
            // 
            // SelectAllEventsBtn
            // 
            this.SelectAllEventsBtn.Location = new System.Drawing.Point(12, 322);
            this.SelectAllEventsBtn.Name = "SelectAllEventsBtn";
            this.SelectAllEventsBtn.Size = new System.Drawing.Size(75, 23);
            this.SelectAllEventsBtn.TabIndex = 11;
            this.SelectAllEventsBtn.Text = "Select All";
            this.SelectAllEventsBtn.UseVisualStyleBackColor = true;
            this.SelectAllEventsBtn.Click += new System.EventHandler(this.SelectAllEventsBtn_Click);
            // 
            // ClearSelectBtn
            // 
            this.ClearSelectBtn.Location = new System.Drawing.Point(12, 351);
            this.ClearSelectBtn.Name = "ClearSelectBtn";
            this.ClearSelectBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearSelectBtn.TabIndex = 12;
            this.ClearSelectBtn.Text = "Clear Select";
            this.ClearSelectBtn.UseVisualStyleBackColor = true;
            this.ClearSelectBtn.Click += new System.EventHandler(this.ClearSelectBtn_Click);
            // 
            // ReadFileEvtFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 395);
            this.Controls.Add(this.ClearSelectBtn);
            this.Controls.Add(this.SelectAllEventsBtn);
            this.Controls.Add(this.CopySelectedEvents);
            this.Controls.Add(this.ClearEventList);
            this.Controls.Add(this.ClearSelectFilterBtn);
            this.Controls.Add(this.ClearAllFilterBtn);
            this.Controls.Add(this.AddFilterBtn);
            this.Controls.Add(this.FilterList);
            this.Controls.Add(this.FilterCmb);
            this.Controls.Add(this.EventList);
            this.Controls.Add(this.FilterTxt);
            this.Controls.Add(this.SearchBtn);
            this.Name = "ReadFileEvtFrm";
            this.Text = "Read File Event";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox FilterTxt;
        private System.Windows.Forms.ListBox EventList;
        private System.Windows.Forms.ComboBox FilterCmb;
        private System.Windows.Forms.ListBox FilterList;
        private System.Windows.Forms.Button AddFilterBtn;
        private System.Windows.Forms.Button ClearAllFilterBtn;
        private System.Windows.Forms.Button ClearSelectFilterBtn;
        private System.Windows.Forms.Button ClearEventList;
        private System.Windows.Forms.Button CopySelectedEvents;
        private System.Windows.Forms.Button SelectAllEventsBtn;
        private System.Windows.Forms.Button ClearSelectBtn;
    }
}

