namespace csAddinsTest
{
    partial class ToolbarForm
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
            this.btnCreateElement = new System.Windows.Forms.Button();
            this.btnTool = new System.Windows.Forms.Button();
            this.btnLevelChanged = new System.Windows.Forms.Button();
            this.btnModelFromCurrent = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateElement
            // 
            this.btnCreateElement.BackgroundImage = global::csAddinsTest.Properties.Resources.Modeling;
            this.btnCreateElement.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCreateElement.Location = new System.Drawing.Point(4, 10);
            this.btnCreateElement.Name = "btnCreateElement";
            this.btnCreateElement.Size = new System.Drawing.Size(25, 25);
            this.btnCreateElement.TabIndex = 1;
            this.btnCreateElement.UseVisualStyleBackColor = true;
            this.btnCreateElement.Click += new System.EventHandler(this.btnCreateElement_Click);
            // 
            // btnTool
            // 
            this.btnTool.BackgroundImage = global::csAddinsTest.Properties.Resources.Tool;
            this.btnTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTool.Location = new System.Drawing.Point(30, 10);
            this.btnTool.Name = "btnTool";
            this.btnTool.Size = new System.Drawing.Size(25, 25);
            this.btnTool.TabIndex = 2;
            this.btnTool.UseVisualStyleBackColor = true;
            this.btnTool.Click += new System.EventHandler(this.btnTool_Click);
            // 
            // btnLevelChanged
            // 
            this.btnLevelChanged.BackgroundImage = global::csAddinsTest.Properties.Resources.Alert;
            this.btnLevelChanged.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLevelChanged.Location = new System.Drawing.Point(56, 10);
            this.btnLevelChanged.Name = "btnLevelChanged";
            this.btnLevelChanged.Size = new System.Drawing.Size(25, 25);
            this.btnLevelChanged.TabIndex = 3;
            this.btnLevelChanged.UseVisualStyleBackColor = true;
            this.btnLevelChanged.Click += new System.EventHandler(this.btnLevelChanged_Click);
            // 
            // btnModelFromCurrent
            // 
            this.btnModelFromCurrent.BackgroundImage = global::csAddinsTest.Properties.Resources.ModelFromCurrent;
            this.btnModelFromCurrent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnModelFromCurrent.Location = new System.Drawing.Point(82, 10);
            this.btnModelFromCurrent.Name = "btnModelFromCurrent";
            this.btnModelFromCurrent.Size = new System.Drawing.Size(25, 25);
            this.btnModelFromCurrent.TabIndex = 4;
            this.btnModelFromCurrent.UseVisualStyleBackColor = true;
            this.btnModelFromCurrent.Click += new System.EventHandler(this.btnModelFromCurrent_Click);
            // 
            // ToolbarForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(200, 42);
            this.Controls.Add(this.btnModelFromCurrent);
            this.Controls.Add(this.btnLevelChanged);
            this.Controls.Add(this.btnTool);
            this.Controls.Add(this.btnCreateElement);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolbarForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Toolbar";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateElement;
        private System.Windows.Forms.Button btnTool;
        private System.Windows.Forms.Button btnLevelChanged;
        private System.Windows.Forms.Button btnModelFromCurrent;
    }
}