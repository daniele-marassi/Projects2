namespace Lettera
{
    partial class Lettera
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
            this.Foglio = new System.Windows.Forms.Panel();
            this.MaximizedBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Foglio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Foglio
            // 
            this.Foglio.BackColor = System.Drawing.Color.White;
            this.Foglio.Controls.Add(this.pictureBox1);
            this.Foglio.Location = new System.Drawing.Point(0, 0);
            this.Foglio.Name = "Foglio";
            this.Foglio.Size = new System.Drawing.Size(800, 453);
            this.Foglio.TabIndex = 0;
            // 
            // MaximizedBtn
            // 
            this.MaximizedBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizedBtn.BackColor = System.Drawing.Color.Black;
            this.MaximizedBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaximizedBtn.ForeColor = System.Drawing.Color.White;
            this.MaximizedBtn.Location = new System.Drawing.Point(1197, 12);
            this.MaximizedBtn.Name = "MaximizedBtn";
            this.MaximizedBtn.Size = new System.Drawing.Size(75, 23);
            this.MaximizedBtn.TabIndex = 1;
            this.MaximizedBtn.Text = "Maximized";
            this.MaximizedBtn.UseVisualStyleBackColor = false;
            this.MaximizedBtn.Click += new System.EventHandler(this.MaximizedBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.BackColor = System.Drawing.Color.Black;
            this.CloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseBtn.ForeColor = System.Drawing.Color.White;
            this.CloseBtn.Location = new System.Drawing.Point(1197, 41);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 2;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = false;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Lettera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1284, 775);
            this.ControlBox = false;
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.MaximizedBtn);
            this.Controls.Add(this.Foglio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Lettera";
            this.Text = "Lettera";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lettera_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lettera_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lettera_MouseUp);
            this.Foglio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Foglio;
        private System.Windows.Forms.Button MaximizedBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

