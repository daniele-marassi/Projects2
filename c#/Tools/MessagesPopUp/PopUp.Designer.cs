
namespace Tools.MessagesPopUp
{
    partial class PopUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopUp));
            this.Title = new System.Windows.Forms.Label();
            this.Ico = new System.Windows.Forms.PictureBox();
            this.Message = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Ico)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(192)))), ((int)(((byte)(100)))));
            this.Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Title.Font = new System.Drawing.Font("Toledo", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(0, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(270, 33);
            this.Title.TabIndex = 0;
            this.Title.Text = "Tools";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Ico
            // 
            this.Ico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(192)))), ((int)(((byte)(100)))));
            this.Ico.Image = ((System.Drawing.Image)(resources.GetObject("Ico.Image")));
            this.Ico.Location = new System.Drawing.Point(0, 0);
            this.Ico.Name = "Ico";
            this.Ico.Size = new System.Drawing.Size(32, 32);
            this.Ico.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Ico.TabIndex = 1;
            this.Ico.TabStop = false;
            // 
            // Message
            // 
            this.Message.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Message.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Message.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Message.ForeColor = System.Drawing.SystemColors.Window;
            this.Message.Location = new System.Drawing.Point(0, 36);
            this.Message.Multiline = true;
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Message.Size = new System.Drawing.Size(288, 105);
            this.Message.TabIndex = 3;
            // 
            // PopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(270, 142);
            this.ControlBox = false;
            this.Controls.Add(this.Message);
            this.Controls.Add(this.Ico);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PopUp";
            this.Opacity = 0.7D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.DarkRed;
            this.Load += new System.EventHandler(this.PopUp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Ico)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.PictureBox Ico;
        private System.Windows.Forms.TextBox Message;
    }
}

