
namespace Box
{
    partial class Form1
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
            this.txtvoice = new System.Windows.Forms.TextBox();
            this.Speak = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtvoice
            // 
            this.txtvoice.Location = new System.Drawing.Point(12, 35);
            this.txtvoice.Multiline = true;
            this.txtvoice.Name = "txtvoice";
            this.txtvoice.Size = new System.Drawing.Size(776, 193);
            this.txtvoice.TabIndex = 0;
            // 
            // Speak
            // 
            this.Speak.Location = new System.Drawing.Point(48, 358);
            this.Speak.Name = "Speak";
            this.Speak.Size = new System.Drawing.Size(75, 23);
            this.Speak.TabIndex = 1;
            this.Speak.Text = "Speak";
            this.Speak.UseVisualStyleBackColor = true;
            this.Speak.Click += new System.EventHandler(this.Speak_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(167, 358);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(285, 358);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Speak);
            this.Controls.Add(this.txtvoice);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtvoice;
        private System.Windows.Forms.Button Speak;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

