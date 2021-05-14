namespace Avaiable
{
    partial class Avaiable
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
            this.AvaiableTmr = new System.Windows.Forms.Timer(this.components);
            this.TextBox = new System.Windows.Forms.TextBox();
            this.CloseTxt = new System.Windows.Forms.TextBox();
            this.StartBreak1Txt = new System.Windows.Forms.TextBox();
            this.CloseBreak1Txt = new System.Windows.Forms.TextBox();
            this.StartBreak2Txt = new System.Windows.Forms.TextBox();
            this.CloseBreak2Txt = new System.Windows.Forms.TextBox();
            this.StartTxt = new System.Windows.Forms.TextBox();
            this.CloseBreak3Txt = new System.Windows.Forms.TextBox();
            this.StartBreak3Txt = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // AvaiableTmr
            // 
            this.AvaiableTmr.Enabled = true;
            this.AvaiableTmr.Interval = 10000;
            this.AvaiableTmr.Tick += new System.EventHandler(this.AvaiableTmr_Tick);
            // 
            // TextBox
            // 
            this.TextBox.Location = new System.Drawing.Point(16, 8);
            this.TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox.Multiline = true;
            this.TextBox.Name = "TextBox";
            this.TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox.Size = new System.Drawing.Size(207, 38);
            this.TextBox.TabIndex = 0;
            // 
            // CloseTxt
            // 
            this.CloseTxt.Location = new System.Drawing.Point(53, 360);
            this.CloseTxt.Margin = new System.Windows.Forms.Padding(4);
            this.CloseTxt.Name = "CloseTxt";
            this.CloseTxt.Size = new System.Drawing.Size(132, 22);
            this.CloseTxt.TabIndex = 1;
            this.CloseTxt.Text = "18:00";
            this.CloseTxt.TextChanged += new System.EventHandler(this.CloseTxt_TextChanged);
            // 
            // StartBreak1Txt
            // 
            this.StartBreak1Txt.Location = new System.Drawing.Point(53, 109);
            this.StartBreak1Txt.Margin = new System.Windows.Forms.Padding(4);
            this.StartBreak1Txt.Name = "StartBreak1Txt";
            this.StartBreak1Txt.Size = new System.Drawing.Size(132, 22);
            this.StartBreak1Txt.TabIndex = 2;
            this.StartBreak1Txt.Text = "11:00";
            this.StartBreak1Txt.TextChanged += new System.EventHandler(this.StartBreak1Txt_TextChanged);
            // 
            // CloseBreak1Txt
            // 
            this.CloseBreak1Txt.Location = new System.Drawing.Point(53, 138);
            this.CloseBreak1Txt.Margin = new System.Windows.Forms.Padding(4);
            this.CloseBreak1Txt.Name = "CloseBreak1Txt";
            this.CloseBreak1Txt.Size = new System.Drawing.Size(132, 22);
            this.CloseBreak1Txt.TabIndex = 3;
            this.CloseBreak1Txt.Text = "11:15";
            this.CloseBreak1Txt.TextChanged += new System.EventHandler(this.CloseBreak1Txt_TextChanged);
            // 
            // StartBreak2Txt
            // 
            this.StartBreak2Txt.Location = new System.Drawing.Point(53, 187);
            this.StartBreak2Txt.Margin = new System.Windows.Forms.Padding(4);
            this.StartBreak2Txt.Name = "StartBreak2Txt";
            this.StartBreak2Txt.Size = new System.Drawing.Size(132, 22);
            this.StartBreak2Txt.TabIndex = 4;
            this.StartBreak2Txt.Text = "13:00";
            this.StartBreak2Txt.TextChanged += new System.EventHandler(this.StartBreak2Txt_TextChanged);
            // 
            // CloseBreak2Txt
            // 
            this.CloseBreak2Txt.Location = new System.Drawing.Point(53, 218);
            this.CloseBreak2Txt.Margin = new System.Windows.Forms.Padding(4);
            this.CloseBreak2Txt.Name = "CloseBreak2Txt";
            this.CloseBreak2Txt.Size = new System.Drawing.Size(132, 22);
            this.CloseBreak2Txt.TabIndex = 5;
            this.CloseBreak2Txt.Text = "14:00";
            this.CloseBreak2Txt.TextChanged += new System.EventHandler(this.CloseBreak2Txt_TextChanged);
            // 
            // StartTxt
            // 
            this.StartTxt.Location = new System.Drawing.Point(53, 55);
            this.StartTxt.Margin = new System.Windows.Forms.Padding(4);
            this.StartTxt.Name = "StartTxt";
            this.StartTxt.Size = new System.Drawing.Size(132, 22);
            this.StartTxt.TabIndex = 6;
            this.StartTxt.Text = "09:00";
            this.StartTxt.TextChanged += new System.EventHandler(this.StartTxt_TextChanged);
            // 
            // CloseBreak3Txt
            // 
            this.CloseBreak3Txt.Location = new System.Drawing.Point(53, 301);
            this.CloseBreak3Txt.Margin = new System.Windows.Forms.Padding(4);
            this.CloseBreak3Txt.Name = "CloseBreak3Txt";
            this.CloseBreak3Txt.Size = new System.Drawing.Size(132, 22);
            this.CloseBreak3Txt.TabIndex = 8;
            this.CloseBreak3Txt.Text = "16:15";
            this.CloseBreak3Txt.TextChanged += new System.EventHandler(this.CloseBreak3Txt_TextChanged);
            // 
            // StartBreak3Txt
            // 
            this.StartBreak3Txt.Location = new System.Drawing.Point(53, 270);
            this.StartBreak3Txt.Margin = new System.Windows.Forms.Padding(4);
            this.StartBreak3Txt.Name = "StartBreak3Txt";
            this.StartBreak3Txt.Size = new System.Drawing.Size(132, 22);
            this.StartBreak3Txt.TabIndex = 7;
            this.StartBreak3Txt.Text = "16:00";
            this.StartBreak3Txt.TextChanged += new System.EventHandler(this.StartBreak3Txt_TextChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(13, 403);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(67, 21);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Active";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(13, 431);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(93, 21);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.Text = "Close App";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(13, 459);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(91, 21);
            this.radioButton3.TabIndex = 11;
            this.radioButton3.Text = "Shutdown";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // Avaiable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 500);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.CloseBreak3Txt);
            this.Controls.Add(this.StartBreak3Txt);
            this.Controls.Add(this.StartTxt);
            this.Controls.Add(this.CloseBreak2Txt);
            this.Controls.Add(this.StartBreak2Txt);
            this.Controls.Add(this.CloseBreak1Txt);
            this.Controls.Add(this.StartBreak1Txt);
            this.Controls.Add(this.CloseTxt);
            this.Controls.Add(this.TextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Avaiable";
            this.Text = "Avaiable";
            this.Load += new System.EventHandler(this.Avaiable_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer AvaiableTmr;
        private System.Windows.Forms.TextBox TextBox;
        private System.Windows.Forms.TextBox CloseTxt;
        private System.Windows.Forms.TextBox StartBreak1Txt;
        private System.Windows.Forms.TextBox CloseBreak1Txt;
        private System.Windows.Forms.TextBox StartBreak2Txt;
        private System.Windows.Forms.TextBox CloseBreak2Txt;
        private System.Windows.Forms.TextBox StartTxt;
        private System.Windows.Forms.TextBox CloseBreak3Txt;
        private System.Windows.Forms.TextBox StartBreak3Txt;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}

