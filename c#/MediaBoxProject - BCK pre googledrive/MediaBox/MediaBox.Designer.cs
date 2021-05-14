using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace MediaBox
{
    partial class MediaBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaBox));
            this.PicturePnl = new System.Windows.Forms.Panel();
            this.LeftPnl = new System.Windows.Forms.Panel();
            this.ManageDirectoryPnl = new System.Windows.Forms.Panel();
            this.Separator1 = new System.Windows.Forms.Label();
            this.DirectoryOpenBtn = new System.Windows.Forms.Button();
            this.DirectoryTxt = new System.Windows.Forms.TextBox();
            this.DirectoryDelBtn = new System.Windows.Forms.Button();
            this.DirectoryAddBtn = new System.Windows.Forms.Button();
            this.DirectoryList = new System.Windows.Forms.ListBox();
            this.PersonalizeColorPnl = new System.Windows.Forms.Panel();
            this.PersonalizeColorBluePnl = new System.Windows.Forms.Panel();
            this.PersonalizeColorGreenPnl = new System.Windows.Forms.Panel();
            this.PersonalizeColorRedPnl = new System.Windows.Forms.Panel();
            this.Separator2 = new System.Windows.Forms.Label();
            this.SetColorBtn = new System.Windows.Forms.Button();
            this.ViewColor = new System.Windows.Forms.Label();
            this.BlueLbl = new System.Windows.Forms.Label();
            this.GreenLbl = new System.Windows.Forms.Label();
            this.RedLbl = new System.Windows.Forms.Label();
            this.ChoiceColorCmb = new System.Windows.Forms.ComboBox();
            this.GeneralSettingsPnl = new System.Windows.Forms.Panel();
            this.VideoIntervalTxt = new System.Windows.Forms.TextBox();
            this.VideoIntervalLbl = new System.Windows.Forms.Label();
            this.MediaMovementSpeedPnl = new System.Windows.Forms.Panel();
            this.MediaMovementSpeedLbl = new System.Windows.Forms.Label();
            this.AutoRunAnimationMediaLbl = new System.Windows.Forms.Label();
            this.SetGeneralSettingsBtn = new System.Windows.Forms.Button();
            this.LanguageenUSLbl = new System.Windows.Forms.Label();
            this.LanguageitITLbl = new System.Windows.Forms.Label();
            this.LanguageenUSBtn = new System.Windows.Forms.Button();
            this.LanguageitITBtn = new System.Windows.Forms.Button();
            this.AutoRunAnimationMediaBtn = new System.Windows.Forms.Button();
            this.WidthBorderMediaTxt = new System.Windows.Forms.TextBox();
            this.WidthBorderMediaLbl = new System.Windows.Forms.Label();
            this.Separator3 = new System.Windows.Forms.Label();
            this.PictureIntervalTxt = new System.Windows.Forms.TextBox();
            this.PictureIntervalLbl = new System.Windows.Forms.Label();
            this.MusicPnl = new System.Windows.Forms.Panel();
            this.AudioPnl = new System.Windows.Forms.Panel();
            this.AudioLbl = new System.Windows.Forms.Label();
            this.MusicPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.PlayListMusicCmb = new System.Windows.Forms.ComboBox();
            this.ForwardMusicBtn = new System.Windows.Forms.Button();
            this.PlayStopMusicBtn = new System.Windows.Forms.Button();
            this.BackMusicBtn = new System.Windows.Forms.Button();
            this.Separator5 = new System.Windows.Forms.Label();
            this.InfoPnl = new System.Windows.Forms.Panel();
            this.DemoBtn = new System.Windows.Forms.Button();
            this.CreatedBy_2Lbl = new System.Windows.Forms.Label();
            this.CreatedBy_1Lbl = new System.Windows.Forms.Label();
            this.VersionLbl = new System.Windows.Forms.Label();
            this.Separator4 = new System.Windows.Forms.Label();
            this.OpenCloseInfoBtn = new System.Windows.Forms.Button();
            this.OpenCloseMusicBtn = new System.Windows.Forms.Button();
            this.OpenCloseGeneralSettingsBtn = new System.Windows.Forms.Button();
            this.OpenCloseManageDirectoryBtn = new System.Windows.Forms.Button();
            this.StartStopAnimateBtn = new System.Windows.Forms.Button();
            this.OpenClosePersonalizeColorBtn = new System.Windows.Forms.Button();
            this.UpdateCatalogBtn = new System.Windows.Forms.Button();
            this.TopPnl = new System.Windows.Forms.Panel();
            this.TitleLbl = new System.Windows.Forms.Label();
            this.MaximizeMediaBoxBtn = new System.Windows.Forms.Button();
            this.MinimizeMediaBoxBtn = new System.Windows.Forms.Button();
            this.CloseMediaBoxBtn = new System.Windows.Forms.Button();
            this.MediaBoxMenuBtn = new System.Windows.Forms.Button();
            this.BottomPnl = new System.Windows.Forms.Panel();
            this.ResizeMediaBoxLbl = new System.Windows.Forms.Label();
            this.MediaBoxMenuTmr = new System.Windows.Forms.Timer(this.components);
            this.waitHideMediaBoxMenu = new System.Windows.Forms.Timer(this.components);
            this.PresentationTmr = new System.Windows.Forms.Timer(this.components);
            this.DemoPnl = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DurationLblDemo = new System.Windows.Forms.Label();
            this.LanguageenUSDemoLbl = new System.Windows.Forms.Label();
            this.LanguageitITDemoLbl = new System.Windows.Forms.Label();
            this.LanguageenUSDemoBtn = new System.Windows.Forms.Button();
            this.LanguageitITDemoBtn = new System.Windows.Forms.Button();
            this.ExitDemoBtn = new System.Windows.Forms.Button();
            this.DemoLbl = new System.Windows.Forms.Label();
            this.StartDemoBtn = new System.Windows.Forms.Button();
            this.LeftPnl.SuspendLayout();
            this.ManageDirectoryPnl.SuspendLayout();
            this.PersonalizeColorPnl.SuspendLayout();
            this.GeneralSettingsPnl.SuspendLayout();
            this.MusicPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MusicPlayer)).BeginInit();
            this.InfoPnl.SuspendLayout();
            this.TopPnl.SuspendLayout();
            this.BottomPnl.SuspendLayout();
            this.DemoPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PicturePnl
            // 
            this.PicturePnl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicturePnl.BackColor = System.Drawing.Color.Transparent;
            this.PicturePnl.Location = new System.Drawing.Point(361, 279);
            this.PicturePnl.Name = "PicturePnl";
            this.PicturePnl.Size = new System.Drawing.Size(382, 186);
            this.PicturePnl.TabIndex = 0;
            this.PicturePnl.Visible = false;
            this.PicturePnl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicturePnl_MouseDown);
            this.PicturePnl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicturePnl_MouseMove);
            this.PicturePnl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PicturePnl_MouseUp);
            // 
            // LeftPnl
            // 
            this.LeftPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LeftPnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.LeftPnl.Controls.Add(this.ManageDirectoryPnl);
            this.LeftPnl.Controls.Add(this.PersonalizeColorPnl);
            this.LeftPnl.Controls.Add(this.GeneralSettingsPnl);
            this.LeftPnl.Controls.Add(this.MusicPnl);
            this.LeftPnl.Controls.Add(this.InfoPnl);
            this.LeftPnl.Controls.Add(this.OpenCloseInfoBtn);
            this.LeftPnl.Controls.Add(this.OpenCloseMusicBtn);
            this.LeftPnl.Controls.Add(this.OpenCloseGeneralSettingsBtn);
            this.LeftPnl.Controls.Add(this.OpenCloseManageDirectoryBtn);
            this.LeftPnl.Controls.Add(this.StartStopAnimateBtn);
            this.LeftPnl.Controls.Add(this.OpenClosePersonalizeColorBtn);
            this.LeftPnl.Controls.Add(this.UpdateCatalogBtn);
            this.LeftPnl.Location = new System.Drawing.Point(0, 40);
            this.LeftPnl.Name = "LeftPnl";
            this.LeftPnl.Size = new System.Drawing.Size(136, 1013);
            this.LeftPnl.TabIndex = 2;
            // 
            // ManageDirectoryPnl
            // 
            this.ManageDirectoryPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManageDirectoryPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ManageDirectoryPnl.Controls.Add(this.Separator1);
            this.ManageDirectoryPnl.Controls.Add(this.DirectoryOpenBtn);
            this.ManageDirectoryPnl.Controls.Add(this.DirectoryTxt);
            this.ManageDirectoryPnl.Controls.Add(this.DirectoryDelBtn);
            this.ManageDirectoryPnl.Controls.Add(this.DirectoryAddBtn);
            this.ManageDirectoryPnl.Controls.Add(this.DirectoryList);
            this.ManageDirectoryPnl.Location = new System.Drawing.Point(5, 48);
            this.ManageDirectoryPnl.Name = "ManageDirectoryPnl";
            this.ManageDirectoryPnl.Size = new System.Drawing.Size(126, 131);
            this.ManageDirectoryPnl.TabIndex = 7;
            // 
            // Separator1
            // 
            this.Separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Separator1.AutoSize = true;
            this.Separator1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator1.ForeColor = System.Drawing.Color.White;
            this.Separator1.Location = new System.Drawing.Point(8, 281);
            this.Separator1.Name = "Separator1";
            this.Separator1.Size = new System.Drawing.Size(112, 13);
            this.Separator1.TabIndex = 12;
            this.Separator1.Text = "---------------------";
            // 
            // DirectoryOpenBtn
            // 
            this.DirectoryOpenBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectoryOpenBtn.BackColor = System.Drawing.Color.Transparent;
            this.DirectoryOpenBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DirectoryOpenBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DirectoryOpenBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirectoryOpenBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.DirectoryOpenBtn.Location = new System.Drawing.Point(74, 105);
            this.DirectoryOpenBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DirectoryOpenBtn.Name = "DirectoryOpenBtn";
            this.DirectoryOpenBtn.Size = new System.Drawing.Size(47, 30);
            this.DirectoryOpenBtn.TabIndex = 11;
            this.DirectoryOpenBtn.Text = "L";
            this.DirectoryOpenBtn.UseVisualStyleBackColor = false;
            this.DirectoryOpenBtn.Click += new System.EventHandler(this.DirectoryOpenBtn_Click);
            // 
            // DirectoryTxt
            // 
            this.DirectoryTxt.Location = new System.Drawing.Point(5, 176);
            this.DirectoryTxt.Name = "DirectoryTxt";
            this.DirectoryTxt.Size = new System.Drawing.Size(116, 20);
            this.DirectoryTxt.TabIndex = 10;
            this.DirectoryTxt.Text = "c:\\...";
            // 
            // DirectoryDelBtn
            // 
            this.DirectoryDelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectoryDelBtn.BackColor = System.Drawing.Color.Transparent;
            this.DirectoryDelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DirectoryDelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DirectoryDelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirectoryDelBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.DirectoryDelBtn.Location = new System.Drawing.Point(4, 104);
            this.DirectoryDelBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DirectoryDelBtn.Name = "DirectoryDelBtn";
            this.DirectoryDelBtn.Size = new System.Drawing.Size(46, 30);
            this.DirectoryDelBtn.TabIndex = 8;
            this.DirectoryDelBtn.Text = "U";
            this.DirectoryDelBtn.UseVisualStyleBackColor = false;
            this.DirectoryDelBtn.Click += new System.EventHandler(this.DirectoryDelBtn_Click);
            // 
            // DirectoryAddBtn
            // 
            this.DirectoryAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectoryAddBtn.BackColor = System.Drawing.Color.Transparent;
            this.DirectoryAddBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DirectoryAddBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DirectoryAddBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirectoryAddBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.DirectoryAddBtn.Location = new System.Drawing.Point(5, 202);
            this.DirectoryAddBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DirectoryAddBtn.Name = "DirectoryAddBtn";
            this.DirectoryAddBtn.Size = new System.Drawing.Size(116, 30);
            this.DirectoryAddBtn.TabIndex = 7;
            this.DirectoryAddBtn.Text = ".";
            this.DirectoryAddBtn.UseVisualStyleBackColor = false;
            this.DirectoryAddBtn.Click += new System.EventHandler(this.DirectoryAddBtn_Click);
            // 
            // DirectoryList
            // 
            this.DirectoryList.ForeColor = System.Drawing.Color.White;
            this.DirectoryList.FormattingEnabled = true;
            this.DirectoryList.Location = new System.Drawing.Point(3, 4);
            this.DirectoryList.Name = "DirectoryList";
            this.DirectoryList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.DirectoryList.Size = new System.Drawing.Size(120, 95);
            this.DirectoryList.TabIndex = 0;
            // 
            // PersonalizeColorPnl
            // 
            this.PersonalizeColorPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PersonalizeColorPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PersonalizeColorPnl.Controls.Add(this.PersonalizeColorBluePnl);
            this.PersonalizeColorPnl.Controls.Add(this.PersonalizeColorGreenPnl);
            this.PersonalizeColorPnl.Controls.Add(this.PersonalizeColorRedPnl);
            this.PersonalizeColorPnl.Controls.Add(this.Separator2);
            this.PersonalizeColorPnl.Controls.Add(this.SetColorBtn);
            this.PersonalizeColorPnl.Controls.Add(this.ViewColor);
            this.PersonalizeColorPnl.Controls.Add(this.BlueLbl);
            this.PersonalizeColorPnl.Controls.Add(this.GreenLbl);
            this.PersonalizeColorPnl.Controls.Add(this.RedLbl);
            this.PersonalizeColorPnl.Controls.Add(this.ChoiceColorCmb);
            this.PersonalizeColorPnl.Location = new System.Drawing.Point(5, 283);
            this.PersonalizeColorPnl.Name = "PersonalizeColorPnl";
            this.PersonalizeColorPnl.Size = new System.Drawing.Size(126, 107);
            this.PersonalizeColorPnl.TabIndex = 4;
            // 
            // PersonalizeColorBluePnl
            // 
            this.PersonalizeColorBluePnl.Location = new System.Drawing.Point(89, 46);
            this.PersonalizeColorBluePnl.Name = "PersonalizeColorBluePnl";
            this.PersonalizeColorBluePnl.Size = new System.Drawing.Size(22, 317);
            this.PersonalizeColorBluePnl.TabIndex = 24;
            // 
            // PersonalizeColorGreenPnl
            // 
            this.PersonalizeColorGreenPnl.Location = new System.Drawing.Point(48, 46);
            this.PersonalizeColorGreenPnl.Name = "PersonalizeColorGreenPnl";
            this.PersonalizeColorGreenPnl.Size = new System.Drawing.Size(22, 317);
            this.PersonalizeColorGreenPnl.TabIndex = 23;
            // 
            // PersonalizeColorRedPnl
            // 
            this.PersonalizeColorRedPnl.Location = new System.Drawing.Point(10, 46);
            this.PersonalizeColorRedPnl.Name = "PersonalizeColorRedPnl";
            this.PersonalizeColorRedPnl.Size = new System.Drawing.Size(22, 317);
            this.PersonalizeColorRedPnl.TabIndex = 22;
            // 
            // Separator2
            // 
            this.Separator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Separator2.AutoSize = true;
            this.Separator2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator2.ForeColor = System.Drawing.Color.White;
            this.Separator2.Location = new System.Drawing.Point(7, 487);
            this.Separator2.Name = "Separator2";
            this.Separator2.Size = new System.Drawing.Size(112, 13);
            this.Separator2.TabIndex = 21;
            this.Separator2.Text = "---------------------";
            // 
            // SetColorBtn
            // 
            this.SetColorBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetColorBtn.BackColor = System.Drawing.Color.Transparent;
            this.SetColorBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetColorBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetColorBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.SetColorBtn.Location = new System.Drawing.Point(4, 412);
            this.SetColorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SetColorBtn.Name = "SetColorBtn";
            this.SetColorBtn.Size = new System.Drawing.Size(116, 30);
            this.SetColorBtn.TabIndex = 20;
            this.SetColorBtn.Text = ".";
            this.SetColorBtn.UseVisualStyleBackColor = false;
            this.SetColorBtn.Click += new System.EventHandler(this.SetColorBtn_Click);
            // 
            // ViewColor
            // 
            this.ViewColor.BackColor = System.Drawing.Color.White;
            this.ViewColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewColor.Location = new System.Drawing.Point(13, 365);
            this.ViewColor.Name = "ViewColor";
            this.ViewColor.Size = new System.Drawing.Size(94, 40);
            this.ViewColor.TabIndex = 19;
            // 
            // BlueLbl
            // 
            this.BlueLbl.AutoSize = true;
            this.BlueLbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlueLbl.ForeColor = System.Drawing.Color.White;
            this.BlueLbl.Location = new System.Drawing.Point(86, 29);
            this.BlueLbl.Name = "BlueLbl";
            this.BlueLbl.Size = new System.Drawing.Size(32, 13);
            this.BlueLbl.TabIndex = 18;
            this.BlueLbl.Text = "Blue";
            // 
            // GreenLbl
            // 
            this.GreenLbl.AutoSize = true;
            this.GreenLbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GreenLbl.ForeColor = System.Drawing.Color.White;
            this.GreenLbl.Location = new System.Drawing.Point(42, 29);
            this.GreenLbl.Name = "GreenLbl";
            this.GreenLbl.Size = new System.Drawing.Size(42, 13);
            this.GreenLbl.TabIndex = 17;
            this.GreenLbl.Text = "Green";
            // 
            // RedLbl
            // 
            this.RedLbl.AutoSize = true;
            this.RedLbl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RedLbl.ForeColor = System.Drawing.Color.White;
            this.RedLbl.Location = new System.Drawing.Point(7, 29);
            this.RedLbl.Name = "RedLbl";
            this.RedLbl.Size = new System.Drawing.Size(29, 13);
            this.RedLbl.TabIndex = 16;
            this.RedLbl.Text = "Red";
            // 
            // ChoiceColorCmb
            // 
            this.ChoiceColorCmb.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ChoiceColorCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChoiceColorCmb.FormattingEnabled = true;
            this.ChoiceColorCmb.Location = new System.Drawing.Point(2, 5);
            this.ChoiceColorCmb.Name = "ChoiceColorCmb";
            this.ChoiceColorCmb.Size = new System.Drawing.Size(121, 21);
            this.ChoiceColorCmb.TabIndex = 5;
            this.ChoiceColorCmb.SelectedIndexChanged += new System.EventHandler(this.ChoiceColorCmb_SelectedIndexChanged);
            this.ChoiceColorCmb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChoiceColorCmb_KeyPress);
            // 
            // GeneralSettingsPnl
            // 
            this.GeneralSettingsPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GeneralSettingsPnl.Controls.Add(this.VideoIntervalTxt);
            this.GeneralSettingsPnl.Controls.Add(this.VideoIntervalLbl);
            this.GeneralSettingsPnl.Controls.Add(this.MediaMovementSpeedPnl);
            this.GeneralSettingsPnl.Controls.Add(this.MediaMovementSpeedLbl);
            this.GeneralSettingsPnl.Controls.Add(this.AutoRunAnimationMediaLbl);
            this.GeneralSettingsPnl.Controls.Add(this.SetGeneralSettingsBtn);
            this.GeneralSettingsPnl.Controls.Add(this.LanguageenUSLbl);
            this.GeneralSettingsPnl.Controls.Add(this.LanguageitITLbl);
            this.GeneralSettingsPnl.Controls.Add(this.LanguageenUSBtn);
            this.GeneralSettingsPnl.Controls.Add(this.LanguageitITBtn);
            this.GeneralSettingsPnl.Controls.Add(this.AutoRunAnimationMediaBtn);
            this.GeneralSettingsPnl.Controls.Add(this.WidthBorderMediaTxt);
            this.GeneralSettingsPnl.Controls.Add(this.WidthBorderMediaLbl);
            this.GeneralSettingsPnl.Controls.Add(this.Separator3);
            this.GeneralSettingsPnl.Controls.Add(this.PictureIntervalTxt);
            this.GeneralSettingsPnl.Controls.Add(this.PictureIntervalLbl);
            this.GeneralSettingsPnl.Location = new System.Drawing.Point(5, 438);
            this.GeneralSettingsPnl.Name = "GeneralSettingsPnl";
            this.GeneralSettingsPnl.Size = new System.Drawing.Size(126, 75);
            this.GeneralSettingsPnl.TabIndex = 9;
            // 
            // VideoIntervalTxt
            // 
            this.VideoIntervalTxt.Location = new System.Drawing.Point(4, 108);
            this.VideoIntervalTxt.Name = "VideoIntervalTxt";
            this.VideoIntervalTxt.Size = new System.Drawing.Size(117, 20);
            this.VideoIntervalTxt.TabIndex = 36;
            // 
            // VideoIntervalLbl
            // 
            this.VideoIntervalLbl.AutoSize = true;
            this.VideoIntervalLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.VideoIntervalLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.VideoIntervalLbl.Location = new System.Drawing.Point(3, 93);
            this.VideoIntervalLbl.Name = "VideoIntervalLbl";
            this.VideoIntervalLbl.Size = new System.Drawing.Size(106, 13);
            this.VideoIntervalLbl.TabIndex = 35;
            this.VideoIntervalLbl.Text = "Video Wait (sedonds)";
            // 
            // MediaMovementSpeedPnl
            // 
            this.MediaMovementSpeedPnl.Location = new System.Drawing.Point(3, 198);
            this.MediaMovementSpeedPnl.Name = "MediaMovementSpeedPnl";
            this.MediaMovementSpeedPnl.Size = new System.Drawing.Size(118, 44);
            this.MediaMovementSpeedPnl.TabIndex = 34;
            // 
            // MediaMovementSpeedLbl
            // 
            this.MediaMovementSpeedLbl.AutoSize = true;
            this.MediaMovementSpeedLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.MediaMovementSpeedLbl.Location = new System.Drawing.Point(3, 178);
            this.MediaMovementSpeedLbl.Name = "MediaMovementSpeedLbl";
            this.MediaMovementSpeedLbl.Size = new System.Drawing.Size(123, 13);
            this.MediaMovementSpeedLbl.TabIndex = 33;
            this.MediaMovementSpeedLbl.Text = "Media Movement Speed";
            // 
            // AutoRunAnimationMediaLbl
            // 
            this.AutoRunAnimationMediaLbl.AutoSize = true;
            this.AutoRunAnimationMediaLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.AutoRunAnimationMediaLbl.Location = new System.Drawing.Point(42, 269);
            this.AutoRunAnimationMediaLbl.Name = "AutoRunAnimationMediaLbl";
            this.AutoRunAnimationMediaLbl.Size = new System.Drawing.Size(52, 13);
            this.AutoRunAnimationMediaLbl.TabIndex = 32;
            this.AutoRunAnimationMediaLbl.Text = "Auto Run";
            // 
            // SetGeneralSettingsBtn
            // 
            this.SetGeneralSettingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetGeneralSettingsBtn.BackColor = System.Drawing.Color.Transparent;
            this.SetGeneralSettingsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SetGeneralSettingsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetGeneralSettingsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetGeneralSettingsBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.SetGeneralSettingsBtn.Location = new System.Drawing.Point(3, 294);
            this.SetGeneralSettingsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SetGeneralSettingsBtn.Name = "SetGeneralSettingsBtn";
            this.SetGeneralSettingsBtn.Size = new System.Drawing.Size(116, 30);
            this.SetGeneralSettingsBtn.TabIndex = 31;
            this.SetGeneralSettingsBtn.Text = ".";
            this.SetGeneralSettingsBtn.UseVisualStyleBackColor = false;
            this.SetGeneralSettingsBtn.Click += new System.EventHandler(this.SetGeneralSettingsBtn_Click);
            // 
            // LanguageenUSLbl
            // 
            this.LanguageenUSLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LanguageenUSLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.LanguageenUSLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageenUSLbl.Location = new System.Drawing.Point(83, 33);
            this.LanguageenUSLbl.Name = "LanguageenUSLbl";
            this.LanguageenUSLbl.Size = new System.Drawing.Size(38, 7);
            this.LanguageenUSLbl.TabIndex = 30;
            // 
            // LanguageitITLbl
            // 
            this.LanguageitITLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LanguageitITLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.LanguageitITLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageitITLbl.Location = new System.Drawing.Point(4, 33);
            this.LanguageitITLbl.Name = "LanguageitITLbl";
            this.LanguageitITLbl.Size = new System.Drawing.Size(38, 7);
            this.LanguageitITLbl.TabIndex = 29;
            // 
            // LanguageenUSBtn
            // 
            this.LanguageenUSBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LanguageenUSBtn.AutoSize = true;
            this.LanguageenUSBtn.BackColor = System.Drawing.Color.Transparent;
            this.LanguageenUSBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LanguageenUSBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LanguageenUSBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.LanguageenUSBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageenUSBtn.Image = global::MediaBox.Properties.Resources.flag_ENG_30x20;
            this.LanguageenUSBtn.Location = new System.Drawing.Point(83, 5);
            this.LanguageenUSBtn.Margin = new System.Windows.Forms.Padding(0);
            this.LanguageenUSBtn.Name = "LanguageenUSBtn";
            this.LanguageenUSBtn.Size = new System.Drawing.Size(38, 28);
            this.LanguageenUSBtn.TabIndex = 28;
            this.LanguageenUSBtn.UseVisualStyleBackColor = false;
            this.LanguageenUSBtn.Click += new System.EventHandler(this.LanguageenUSBtn_Click);
            // 
            // LanguageitITBtn
            // 
            this.LanguageitITBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LanguageitITBtn.AutoSize = true;
            this.LanguageitITBtn.BackColor = System.Drawing.Color.Transparent;
            this.LanguageitITBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LanguageitITBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LanguageitITBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.LanguageitITBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageitITBtn.Image = global::MediaBox.Properties.Resources.flag_ITA_30x20;
            this.LanguageitITBtn.Location = new System.Drawing.Point(4, 5);
            this.LanguageitITBtn.Margin = new System.Windows.Forms.Padding(0);
            this.LanguageitITBtn.Name = "LanguageitITBtn";
            this.LanguageitITBtn.Size = new System.Drawing.Size(38, 28);
            this.LanguageitITBtn.TabIndex = 27;
            this.LanguageitITBtn.UseVisualStyleBackColor = false;
            this.LanguageitITBtn.Click += new System.EventHandler(this.LanguageitITBtn_Click);
            // 
            // AutoRunAnimationMediaBtn
            // 
            this.AutoRunAnimationMediaBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoRunAnimationMediaBtn.BackColor = System.Drawing.Color.Transparent;
            this.AutoRunAnimationMediaBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoRunAnimationMediaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoRunAnimationMediaBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.AutoRunAnimationMediaBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.AutoRunAnimationMediaBtn.Location = new System.Drawing.Point(5, 246);
            this.AutoRunAnimationMediaBtn.Margin = new System.Windows.Forms.Padding(0);
            this.AutoRunAnimationMediaBtn.Name = "AutoRunAnimationMediaBtn";
            this.AutoRunAnimationMediaBtn.Size = new System.Drawing.Size(35, 35);
            this.AutoRunAnimationMediaBtn.TabIndex = 26;
            this.AutoRunAnimationMediaBtn.Text = ">";
            this.AutoRunAnimationMediaBtn.UseVisualStyleBackColor = false;
            this.AutoRunAnimationMediaBtn.Click += new System.EventHandler(this.AutoRunAnimationMediaBtn_Click);
            // 
            // WidthBorderMediaTxt
            // 
            this.WidthBorderMediaTxt.Location = new System.Drawing.Point(4, 148);
            this.WidthBorderMediaTxt.Name = "WidthBorderMediaTxt";
            this.WidthBorderMediaTxt.Size = new System.Drawing.Size(117, 20);
            this.WidthBorderMediaTxt.TabIndex = 24;
            // 
            // WidthBorderMediaLbl
            // 
            this.WidthBorderMediaLbl.AutoSize = true;
            this.WidthBorderMediaLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.WidthBorderMediaLbl.Location = new System.Drawing.Point(3, 133);
            this.WidthBorderMediaLbl.Name = "WidthBorderMediaLbl";
            this.WidthBorderMediaLbl.Size = new System.Drawing.Size(101, 13);
            this.WidthBorderMediaLbl.TabIndex = 23;
            this.WidthBorderMediaLbl.Text = "Width Border Media";
            // 
            // Separator3
            // 
            this.Separator3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Separator3.AutoSize = true;
            this.Separator3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator3.ForeColor = System.Drawing.Color.White;
            this.Separator3.Location = new System.Drawing.Point(6, 345);
            this.Separator3.Name = "Separator3";
            this.Separator3.Size = new System.Drawing.Size(112, 13);
            this.Separator3.TabIndex = 22;
            this.Separator3.Text = "---------------------";
            // 
            // PictureIntervalTxt
            // 
            this.PictureIntervalTxt.Location = new System.Drawing.Point(4, 67);
            this.PictureIntervalTxt.Name = "PictureIntervalTxt";
            this.PictureIntervalTxt.Size = new System.Drawing.Size(117, 20);
            this.PictureIntervalTxt.TabIndex = 1;
            // 
            // PictureIntervalLbl
            // 
            this.PictureIntervalLbl.AutoSize = true;
            this.PictureIntervalLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.PictureIntervalLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.PictureIntervalLbl.Location = new System.Drawing.Point(3, 52);
            this.PictureIntervalLbl.Name = "PictureIntervalLbl";
            this.PictureIntervalLbl.Size = new System.Drawing.Size(111, 13);
            this.PictureIntervalLbl.TabIndex = 0;
            this.PictureIntervalLbl.Text = "Picture Wait (sedonds)";
            // 
            // MusicPnl
            // 
            this.MusicPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MusicPnl.Controls.Add(this.AudioPnl);
            this.MusicPnl.Controls.Add(this.AudioLbl);
            this.MusicPnl.Controls.Add(this.MusicPlayer);
            this.MusicPnl.Controls.Add(this.PlayListMusicCmb);
            this.MusicPnl.Controls.Add(this.ForwardMusicBtn);
            this.MusicPnl.Controls.Add(this.PlayStopMusicBtn);
            this.MusicPnl.Controls.Add(this.BackMusicBtn);
            this.MusicPnl.Controls.Add(this.Separator5);
            this.MusicPnl.Location = new System.Drawing.Point(5, 613);
            this.MusicPnl.Name = "MusicPnl";
            this.MusicPnl.Size = new System.Drawing.Size(126, 91);
            this.MusicPnl.TabIndex = 11;
            // 
            // AudioPnl
            // 
            this.AudioPnl.Location = new System.Drawing.Point(3, 113);
            this.AudioPnl.Name = "AudioPnl";
            this.AudioPnl.Size = new System.Drawing.Size(118, 44);
            this.AudioPnl.TabIndex = 31;
            // 
            // AudioLbl
            // 
            this.AudioLbl.AutoSize = true;
            this.AudioLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AudioLbl.Location = new System.Drawing.Point(42, 82);
            this.AudioLbl.Name = "AudioLbl";
            this.AudioLbl.Size = new System.Drawing.Size(36, 31);
            this.AudioLbl.TabIndex = 30;
            this.AudioLbl.Text = "m";
            // 
            // MusicPlayer
            // 
            this.MusicPlayer.Enabled = true;
            this.MusicPlayer.Location = new System.Drawing.Point(11, 215);
            this.MusicPlayer.Name = "MusicPlayer";
            this.MusicPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MusicPlayer.OcxState")));
            this.MusicPlayer.Size = new System.Drawing.Size(38, 39);
            this.MusicPlayer.TabIndex = 26;
            this.MusicPlayer.Visible = false;
            // 
            // PlayListMusicCmb
            // 
            this.PlayListMusicCmb.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PlayListMusicCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayListMusicCmb.FormattingEnabled = true;
            this.PlayListMusicCmb.Location = new System.Drawing.Point(2, 5);
            this.PlayListMusicCmb.Name = "PlayListMusicCmb";
            this.PlayListMusicCmb.Size = new System.Drawing.Size(119, 21);
            this.PlayListMusicCmb.Sorted = true;
            this.PlayListMusicCmb.TabIndex = 27;
            this.PlayListMusicCmb.SelectedValueChanged += new System.EventHandler(this.PlayListMusicCmb_SelectedValueChanged);
            this.PlayListMusicCmb.Enter += new System.EventHandler(this.PlayListMusicCmb_Enter);
            this.PlayListMusicCmb.Leave += new System.EventHandler(this.PlayListMusicCmb_Leave);
            // 
            // ForwardMusicBtn
            // 
            this.ForwardMusicBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForwardMusicBtn.BackColor = System.Drawing.Color.Transparent;
            this.ForwardMusicBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ForwardMusicBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ForwardMusicBtn.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.ForwardMusicBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.ForwardMusicBtn.Location = new System.Drawing.Point(83, 32);
            this.ForwardMusicBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ForwardMusicBtn.Name = "ForwardMusicBtn";
            this.ForwardMusicBtn.Size = new System.Drawing.Size(38, 33);
            this.ForwardMusicBtn.TabIndex = 25;
            this.ForwardMusicBtn.Text = ":";
            this.ForwardMusicBtn.UseVisualStyleBackColor = false;
            this.ForwardMusicBtn.Click += new System.EventHandler(this.ForwardMusicBtn_Click);
            // 
            // PlayStopMusicBtn
            // 
            this.PlayStopMusicBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayStopMusicBtn.BackColor = System.Drawing.Color.Transparent;
            this.PlayStopMusicBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PlayStopMusicBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayStopMusicBtn.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.PlayStopMusicBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.PlayStopMusicBtn.Location = new System.Drawing.Point(42, 32);
            this.PlayStopMusicBtn.Margin = new System.Windows.Forms.Padding(0);
            this.PlayStopMusicBtn.Name = "PlayStopMusicBtn";
            this.PlayStopMusicBtn.Size = new System.Drawing.Size(38, 33);
            this.PlayStopMusicBtn.TabIndex = 24;
            this.PlayStopMusicBtn.Text = "4";
            this.PlayStopMusicBtn.UseVisualStyleBackColor = false;
            this.PlayStopMusicBtn.Click += new System.EventHandler(this.PlayStopMusicBtn_Click);
            // 
            // BackMusicBtn
            // 
            this.BackMusicBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackMusicBtn.BackColor = System.Drawing.Color.Transparent;
            this.BackMusicBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BackMusicBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackMusicBtn.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.BackMusicBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.BackMusicBtn.Location = new System.Drawing.Point(2, 32);
            this.BackMusicBtn.Margin = new System.Windows.Forms.Padding(0);
            this.BackMusicBtn.Name = "BackMusicBtn";
            this.BackMusicBtn.Size = new System.Drawing.Size(38, 33);
            this.BackMusicBtn.TabIndex = 23;
            this.BackMusicBtn.Text = "9";
            this.BackMusicBtn.UseVisualStyleBackColor = false;
            this.BackMusicBtn.Click += new System.EventHandler(this.BackMusicBtn_Click);
            // 
            // Separator5
            // 
            this.Separator5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Separator5.AutoSize = true;
            this.Separator5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator5.ForeColor = System.Drawing.Color.White;
            this.Separator5.Location = new System.Drawing.Point(6, 156);
            this.Separator5.Name = "Separator5";
            this.Separator5.Size = new System.Drawing.Size(112, 13);
            this.Separator5.TabIndex = 22;
            this.Separator5.Text = "---------------------";
            // 
            // InfoPnl
            // 
            this.InfoPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoPnl.Controls.Add(this.DemoBtn);
            this.InfoPnl.Controls.Add(this.CreatedBy_2Lbl);
            this.InfoPnl.Controls.Add(this.CreatedBy_1Lbl);
            this.InfoPnl.Controls.Add(this.VersionLbl);
            this.InfoPnl.Controls.Add(this.Separator4);
            this.InfoPnl.Location = new System.Drawing.Point(5, 752);
            this.InfoPnl.Name = "InfoPnl";
            this.InfoPnl.Size = new System.Drawing.Size(126, 140);
            this.InfoPnl.TabIndex = 13;
            // 
            // DemoBtn
            // 
            this.DemoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DemoBtn.BackColor = System.Drawing.Color.Transparent;
            this.DemoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DemoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DemoBtn.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DemoBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.DemoBtn.Location = new System.Drawing.Point(5, 76);
            this.DemoBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DemoBtn.Name = "DemoBtn";
            this.DemoBtn.Size = new System.Drawing.Size(113, 40);
            this.DemoBtn.TabIndex = 26;
            this.DemoBtn.Text = "Demo";
            this.DemoBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.DemoBtn.UseVisualStyleBackColor = false;
            this.DemoBtn.Click += new System.EventHandler(this.DemoBtn_Click);
            // 
            // CreatedBy_2Lbl
            // 
            this.CreatedBy_2Lbl.AutoSize = true;
            this.CreatedBy_2Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.CreatedBy_2Lbl.ForeColor = System.Drawing.Color.LightCoral;
            this.CreatedBy_2Lbl.Location = new System.Drawing.Point(2, 46);
            this.CreatedBy_2Lbl.Name = "CreatedBy_2Lbl";
            this.CreatedBy_2Lbl.Size = new System.Drawing.Size(82, 13);
            this.CreatedBy_2Lbl.TabIndex = 25;
            this.CreatedBy_2Lbl.Text = "Daniele Marassi";
            // 
            // CreatedBy_1Lbl
            // 
            this.CreatedBy_1Lbl.AutoSize = true;
            this.CreatedBy_1Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.CreatedBy_1Lbl.ForeColor = System.Drawing.Color.LightCoral;
            this.CreatedBy_1Lbl.Location = new System.Drawing.Point(2, 31);
            this.CreatedBy_1Lbl.Name = "CreatedBy_1Lbl";
            this.CreatedBy_1Lbl.Size = new System.Drawing.Size(61, 13);
            this.CreatedBy_1Lbl.TabIndex = 24;
            this.CreatedBy_1Lbl.Text = "Created by:";
            // 
            // VersionLbl
            // 
            this.VersionLbl.AutoSize = true;
            this.VersionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.VersionLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.VersionLbl.Location = new System.Drawing.Point(2, 5);
            this.VersionLbl.Name = "VersionLbl";
            this.VersionLbl.Size = new System.Drawing.Size(45, 13);
            this.VersionLbl.TabIndex = 23;
            this.VersionLbl.Text = "Version:";
            // 
            // Separator4
            // 
            this.Separator4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Separator4.AutoSize = true;
            this.Separator4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator4.ForeColor = System.Drawing.Color.White;
            this.Separator4.Location = new System.Drawing.Point(6, 138);
            this.Separator4.Name = "Separator4";
            this.Separator4.Size = new System.Drawing.Size(112, 13);
            this.Separator4.TabIndex = 22;
            this.Separator4.Text = "---------------------";
            // 
            // OpenCloseInfoBtn
            // 
            this.OpenCloseInfoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenCloseInfoBtn.BackColor = System.Drawing.Color.Transparent;
            this.OpenCloseInfoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenCloseInfoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenCloseInfoBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenCloseInfoBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.OpenCloseInfoBtn.Location = new System.Drawing.Point(5, 707);
            this.OpenCloseInfoBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OpenCloseInfoBtn.Name = "OpenCloseInfoBtn";
            this.OpenCloseInfoBtn.Size = new System.Drawing.Size(60, 40);
            this.OpenCloseInfoBtn.TabIndex = 12;
            this.OpenCloseInfoBtn.Text = "?";
            this.OpenCloseInfoBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OpenCloseInfoBtn.UseVisualStyleBackColor = false;
            this.OpenCloseInfoBtn.Click += new System.EventHandler(this.OpenCloseInfoBtn_Click);
            // 
            // OpenCloseMusicBtn
            // 
            this.OpenCloseMusicBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenCloseMusicBtn.BackColor = System.Drawing.Color.Transparent;
            this.OpenCloseMusicBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenCloseMusicBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenCloseMusicBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenCloseMusicBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.OpenCloseMusicBtn.Location = new System.Drawing.Point(5, 568);
            this.OpenCloseMusicBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OpenCloseMusicBtn.Name = "OpenCloseMusicBtn";
            this.OpenCloseMusicBtn.Size = new System.Drawing.Size(60, 40);
            this.OpenCloseMusicBtn.TabIndex = 10;
            this.OpenCloseMusicBtn.Text = "m";
            this.OpenCloseMusicBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OpenCloseMusicBtn.UseVisualStyleBackColor = false;
            this.OpenCloseMusicBtn.Click += new System.EventHandler(this.OpenCloseMusicBtn_Click);
            // 
            // OpenCloseGeneralSettingsBtn
            // 
            this.OpenCloseGeneralSettingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenCloseGeneralSettingsBtn.BackColor = System.Drawing.Color.Transparent;
            this.OpenCloseGeneralSettingsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenCloseGeneralSettingsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenCloseGeneralSettingsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenCloseGeneralSettingsBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.OpenCloseGeneralSettingsBtn.Location = new System.Drawing.Point(5, 393);
            this.OpenCloseGeneralSettingsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OpenCloseGeneralSettingsBtn.Name = "OpenCloseGeneralSettingsBtn";
            this.OpenCloseGeneralSettingsBtn.Size = new System.Drawing.Size(60, 40);
            this.OpenCloseGeneralSettingsBtn.TabIndex = 8;
            this.OpenCloseGeneralSettingsBtn.Text = "S";
            this.OpenCloseGeneralSettingsBtn.UseVisualStyleBackColor = false;
            this.OpenCloseGeneralSettingsBtn.Click += new System.EventHandler(this.OpenCloseGeneralSettingsBtn_Click);
            // 
            // OpenCloseManageDirectoryBtn
            // 
            this.OpenCloseManageDirectoryBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenCloseManageDirectoryBtn.BackColor = System.Drawing.Color.Transparent;
            this.OpenCloseManageDirectoryBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenCloseManageDirectoryBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenCloseManageDirectoryBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenCloseManageDirectoryBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.OpenCloseManageDirectoryBtn.Location = new System.Drawing.Point(5, 5);
            this.OpenCloseManageDirectoryBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OpenCloseManageDirectoryBtn.Name = "OpenCloseManageDirectoryBtn";
            this.OpenCloseManageDirectoryBtn.Size = new System.Drawing.Size(60, 40);
            this.OpenCloseManageDirectoryBtn.TabIndex = 6;
            this.OpenCloseManageDirectoryBtn.Text = "F";
            this.OpenCloseManageDirectoryBtn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.OpenCloseManageDirectoryBtn.UseVisualStyleBackColor = false;
            this.OpenCloseManageDirectoryBtn.Click += new System.EventHandler(this.OpenCloseManageDirectoryBtn_Click);
            // 
            // StartStopAnimateBtn
            // 
            this.StartStopAnimateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartStopAnimateBtn.BackColor = System.Drawing.Color.Transparent;
            this.StartStopAnimateBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartStopAnimateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartStopAnimateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartStopAnimateBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.StartStopAnimateBtn.Location = new System.Drawing.Point(5, 516);
            this.StartStopAnimateBtn.Margin = new System.Windows.Forms.Padding(0);
            this.StartStopAnimateBtn.Name = "StartStopAnimateBtn";
            this.StartStopAnimateBtn.Size = new System.Drawing.Size(60, 40);
            this.StartStopAnimateBtn.TabIndex = 5;
            this.StartStopAnimateBtn.Text = "M";
            this.StartStopAnimateBtn.UseVisualStyleBackColor = false;
            this.StartStopAnimateBtn.Click += new System.EventHandler(this.StartStopAnimateBtn_Click);
            // 
            // OpenClosePersonalizeColorBtn
            // 
            this.OpenClosePersonalizeColorBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenClosePersonalizeColorBtn.BackColor = System.Drawing.Color.Transparent;
            this.OpenClosePersonalizeColorBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenClosePersonalizeColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenClosePersonalizeColorBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenClosePersonalizeColorBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.OpenClosePersonalizeColorBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.OpenClosePersonalizeColorBtn.Location = new System.Drawing.Point(5, 233);
            this.OpenClosePersonalizeColorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OpenClosePersonalizeColorBtn.Name = "OpenClosePersonalizeColorBtn";
            this.OpenClosePersonalizeColorBtn.Size = new System.Drawing.Size(60, 40);
            this.OpenClosePersonalizeColorBtn.TabIndex = 3;
            this.OpenClosePersonalizeColorBtn.Text = "Rn";
            this.OpenClosePersonalizeColorBtn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.OpenClosePersonalizeColorBtn.UseVisualStyleBackColor = false;
            this.OpenClosePersonalizeColorBtn.Click += new System.EventHandler(this.OpenClosePersonalizeColorBtn_Click);
            // 
            // UpdateCatalogBtn
            // 
            this.UpdateCatalogBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdateCatalogBtn.BackColor = System.Drawing.Color.Transparent;
            this.UpdateCatalogBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UpdateCatalogBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateCatalogBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateCatalogBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.UpdateCatalogBtn.Location = new System.Drawing.Point(5, 182);
            this.UpdateCatalogBtn.Margin = new System.Windows.Forms.Padding(0);
            this.UpdateCatalogBtn.Name = "UpdateCatalogBtn";
            this.UpdateCatalogBtn.Size = new System.Drawing.Size(60, 40);
            this.UpdateCatalogBtn.TabIndex = 2;
            this.UpdateCatalogBtn.Text = "V";
            this.UpdateCatalogBtn.UseVisualStyleBackColor = false;
            this.UpdateCatalogBtn.Click += new System.EventHandler(this.UpdateCatalogBtn_Click);
            // 
            // TopPnl
            // 
            this.TopPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TopPnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.TopPnl.Controls.Add(this.TitleLbl);
            this.TopPnl.Controls.Add(this.MaximizeMediaBoxBtn);
            this.TopPnl.Controls.Add(this.MinimizeMediaBoxBtn);
            this.TopPnl.Controls.Add(this.CloseMediaBoxBtn);
            this.TopPnl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TopPnl.Location = new System.Drawing.Point(0, 0);
            this.TopPnl.Name = "TopPnl";
            this.TopPnl.Size = new System.Drawing.Size(1070, 40);
            this.TopPnl.TabIndex = 3;
            this.TopPnl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopPnl_MouseDown);
            this.TopPnl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TopPnl_MouseMove);
            this.TopPnl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TopPnl_MouseUp);
            // 
            // TitleLbl
            // 
            this.TitleLbl.AutoSize = true;
            this.TitleLbl.BackColor = System.Drawing.Color.Transparent;
            this.TitleLbl.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLbl.ForeColor = System.Drawing.Color.White;
            this.TitleLbl.Location = new System.Drawing.Point(31, 8);
            this.TitleLbl.Name = "TitleLbl";
            this.TitleLbl.Size = new System.Drawing.Size(167, 35);
            this.TitleLbl.TabIndex = 6;
            this.TitleLbl.Text = "edia Box";
            // 
            // MaximizeMediaBoxBtn
            // 
            this.MaximizeMediaBoxBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizeMediaBoxBtn.BackColor = System.Drawing.Color.Transparent;
            this.MaximizeMediaBoxBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaximizeMediaBoxBtn.Font = new System.Drawing.Font("Webdings", 14F);
            this.MaximizeMediaBoxBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.MaximizeMediaBoxBtn.Location = new System.Drawing.Point(1000, 5);
            this.MaximizeMediaBoxBtn.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeMediaBoxBtn.Name = "MaximizeMediaBoxBtn";
            this.MaximizeMediaBoxBtn.Size = new System.Drawing.Size(30, 30);
            this.MaximizeMediaBoxBtn.TabIndex = 5;
            this.MaximizeMediaBoxBtn.Text = "1";
            this.MaximizeMediaBoxBtn.UseVisualStyleBackColor = false;
            this.MaximizeMediaBoxBtn.Click += new System.EventHandler(this.MaximizeOrDefaultMediaBoxBtn_Click);
            // 
            // MinimizeMediaBoxBtn
            // 
            this.MinimizeMediaBoxBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizeMediaBoxBtn.BackColor = System.Drawing.Color.Transparent;
            this.MinimizeMediaBoxBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeMediaBoxBtn.Font = new System.Drawing.Font("Verdana", 12F);
            this.MinimizeMediaBoxBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.MinimizeMediaBoxBtn.Location = new System.Drawing.Point(965, 5);
            this.MinimizeMediaBoxBtn.Margin = new System.Windows.Forms.Padding(0);
            this.MinimizeMediaBoxBtn.Name = "MinimizeMediaBoxBtn";
            this.MinimizeMediaBoxBtn.Size = new System.Drawing.Size(30, 30);
            this.MinimizeMediaBoxBtn.TabIndex = 4;
            this.MinimizeMediaBoxBtn.Text = "_";
            this.MinimizeMediaBoxBtn.UseVisualStyleBackColor = false;
            this.MinimizeMediaBoxBtn.Click += new System.EventHandler(this.MinimizeMediaBoxBtn_Click);
            // 
            // CloseMediaBoxBtn
            // 
            this.CloseMediaBoxBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseMediaBoxBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseMediaBoxBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseMediaBoxBtn.Font = new System.Drawing.Font("Webdings", 12F);
            this.CloseMediaBoxBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.CloseMediaBoxBtn.Location = new System.Drawing.Point(1035, 5);
            this.CloseMediaBoxBtn.Margin = new System.Windows.Forms.Padding(0);
            this.CloseMediaBoxBtn.Name = "CloseMediaBoxBtn";
            this.CloseMediaBoxBtn.Size = new System.Drawing.Size(30, 30);
            this.CloseMediaBoxBtn.TabIndex = 3;
            this.CloseMediaBoxBtn.Text = "r";
            this.CloseMediaBoxBtn.UseVisualStyleBackColor = false;
            this.CloseMediaBoxBtn.Click += new System.EventHandler(this.CloseMediaBoxBtn_Click);
            // 
            // MediaBoxMenuBtn
            // 
            this.MediaBoxMenuBtn.BackColor = System.Drawing.Color.Firebrick;
            this.MediaBoxMenuBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MediaBoxMenuBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MediaBoxMenuBtn.FlatAppearance.BorderSize = 0;
            this.MediaBoxMenuBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MediaBoxMenuBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MediaBoxMenuBtn.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.MediaBoxMenuBtn.Location = new System.Drawing.Point(5, 4);
            this.MediaBoxMenuBtn.Name = "MediaBoxMenuBtn";
            this.MediaBoxMenuBtn.Size = new System.Drawing.Size(35, 35);
            this.MediaBoxMenuBtn.TabIndex = 10;
            this.MediaBoxMenuBtn.Text = "M";
            this.MediaBoxMenuBtn.UseVisualStyleBackColor = false;
            this.MediaBoxMenuBtn.Click += new System.EventHandler(this.MediaBoxMenuBtn_Click);
            // 
            // BottomPnl
            // 
            this.BottomPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BottomPnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.BottomPnl.Controls.Add(this.ResizeMediaBoxLbl);
            this.BottomPnl.Location = new System.Drawing.Point(136, 953);
            this.BottomPnl.Name = "BottomPnl";
            this.BottomPnl.Size = new System.Drawing.Size(934, 100);
            this.BottomPnl.TabIndex = 9;
            // 
            // ResizeMediaBoxLbl
            // 
            this.ResizeMediaBoxLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResizeMediaBoxLbl.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.ResizeMediaBoxLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ResizeMediaBoxLbl.Font = new System.Drawing.Font("Marlett", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResizeMediaBoxLbl.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ResizeMediaBoxLbl.Location = new System.Drawing.Point(914, 78);
            this.ResizeMediaBoxLbl.Margin = new System.Windows.Forms.Padding(0);
            this.ResizeMediaBoxLbl.Name = "ResizeMediaBoxLbl";
            this.ResizeMediaBoxLbl.Size = new System.Drawing.Size(18, 21);
            this.ResizeMediaBoxLbl.TabIndex = 10;
            this.ResizeMediaBoxLbl.Text = "o";
            this.ResizeMediaBoxLbl.UseCompatibleTextRendering = true;
            this.ResizeMediaBoxLbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResizeMediaBoxLbl_MouseDown);
            this.ResizeMediaBoxLbl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ResizeMediaBoxLbl_MouseMove);
            this.ResizeMediaBoxLbl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResizeMediaBoxLbl_MouseUp);
            // 
            // MediaBoxMenuTmr
            // 
            this.MediaBoxMenuTmr.Interval = 1;
            this.MediaBoxMenuTmr.Tick += new System.EventHandler(this.MediaBoxMenuTmr_Tick);
            // 
            // waitHideMediaBoxMenu
            // 
            this.waitHideMediaBoxMenu.Interval = 1000;
            this.waitHideMediaBoxMenu.Tick += new System.EventHandler(this.WaitHideMediaBoxMenu_Tick);
            // 
            // PresentationTmr
            // 
            this.PresentationTmr.Tick += new System.EventHandler(this.PresentationTmr_Tick);
            // 
            // DemoPnl
            // 
            this.DemoPnl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DemoPnl.BackColor = System.Drawing.Color.Transparent;
            this.DemoPnl.Controls.Add(this.pictureBox1);
            this.DemoPnl.Controls.Add(this.DurationLblDemo);
            this.DemoPnl.Controls.Add(this.LanguageenUSDemoLbl);
            this.DemoPnl.Controls.Add(this.LanguageitITDemoLbl);
            this.DemoPnl.Controls.Add(this.LanguageenUSDemoBtn);
            this.DemoPnl.Controls.Add(this.LanguageitITDemoBtn);
            this.DemoPnl.Controls.Add(this.ExitDemoBtn);
            this.DemoPnl.Controls.Add(this.DemoLbl);
            this.DemoPnl.Controls.Add(this.StartDemoBtn);
            this.DemoPnl.Location = new System.Drawing.Point(358, 59);
            this.DemoPnl.Name = "DemoPnl";
            this.DemoPnl.Size = new System.Drawing.Size(382, 203);
            this.DemoPnl.TabIndex = 12;
            this.DemoPnl.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Location = new System.Drawing.Point(37, 60);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 67);
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // DurationLblDemo
            // 
            this.DurationLblDemo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DurationLblDemo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DurationLblDemo.ForeColor = System.Drawing.Color.White;
            this.DurationLblDemo.Location = new System.Drawing.Point(29, 130);
            this.DurationLblDemo.Name = "DurationLblDemo";
            this.DurationLblDemo.Size = new System.Drawing.Size(333, 15);
            this.DurationLblDemo.TabIndex = 35;
            this.DurationLblDemo.Text = "Duration Demo";
            this.DurationLblDemo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LanguageenUSDemoLbl
            // 
            this.LanguageenUSDemoLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LanguageenUSDemoLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LanguageenUSDemoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.LanguageenUSDemoLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageenUSDemoLbl.Location = new System.Drawing.Point(212, 111);
            this.LanguageenUSDemoLbl.Name = "LanguageenUSDemoLbl";
            this.LanguageenUSDemoLbl.Size = new System.Drawing.Size(38, 7);
            this.LanguageenUSDemoLbl.TabIndex = 34;
            // 
            // LanguageitITDemoLbl
            // 
            this.LanguageitITDemoLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LanguageitITDemoLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LanguageitITDemoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.LanguageitITDemoLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageitITDemoLbl.Location = new System.Drawing.Point(133, 111);
            this.LanguageitITDemoLbl.Name = "LanguageitITDemoLbl";
            this.LanguageitITDemoLbl.Size = new System.Drawing.Size(38, 7);
            this.LanguageitITDemoLbl.TabIndex = 33;
            // 
            // LanguageenUSDemoBtn
            // 
            this.LanguageenUSDemoBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LanguageenUSDemoBtn.AutoSize = true;
            this.LanguageenUSDemoBtn.BackColor = System.Drawing.Color.Transparent;
            this.LanguageenUSDemoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LanguageenUSDemoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LanguageenUSDemoBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.LanguageenUSDemoBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageenUSDemoBtn.Image = global::MediaBox.Properties.Resources.flag_ENG_30x20;
            this.LanguageenUSDemoBtn.Location = new System.Drawing.Point(212, 83);
            this.LanguageenUSDemoBtn.Margin = new System.Windows.Forms.Padding(0);
            this.LanguageenUSDemoBtn.Name = "LanguageenUSDemoBtn";
            this.LanguageenUSDemoBtn.Size = new System.Drawing.Size(38, 28);
            this.LanguageenUSDemoBtn.TabIndex = 32;
            this.LanguageenUSDemoBtn.UseVisualStyleBackColor = false;
            this.LanguageenUSDemoBtn.Click += new System.EventHandler(this.LanguageenUSDemoBtn_Click);
            // 
            // LanguageitITDemoBtn
            // 
            this.LanguageitITDemoBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LanguageitITDemoBtn.AutoSize = true;
            this.LanguageitITDemoBtn.BackColor = System.Drawing.Color.Transparent;
            this.LanguageitITDemoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LanguageitITDemoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LanguageitITDemoBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.LanguageitITDemoBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.LanguageitITDemoBtn.Image = global::MediaBox.Properties.Resources.flag_ITA_30x20;
            this.LanguageitITDemoBtn.Location = new System.Drawing.Point(133, 83);
            this.LanguageitITDemoBtn.Margin = new System.Windows.Forms.Padding(0);
            this.LanguageitITDemoBtn.Name = "LanguageitITDemoBtn";
            this.LanguageitITDemoBtn.Size = new System.Drawing.Size(38, 28);
            this.LanguageitITDemoBtn.TabIndex = 31;
            this.LanguageitITDemoBtn.UseVisualStyleBackColor = false;
            this.LanguageitITDemoBtn.Click += new System.EventHandler(this.LanguageitITDemoBtn_Click);
            // 
            // ExitDemoBtn
            // 
            this.ExitDemoBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ExitDemoBtn.BackColor = System.Drawing.Color.Transparent;
            this.ExitDemoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitDemoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitDemoBtn.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitDemoBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.ExitDemoBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ExitDemoBtn.Location = new System.Drawing.Point(247, 151);
            this.ExitDemoBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ExitDemoBtn.Name = "ExitDemoBtn";
            this.ExitDemoBtn.Size = new System.Drawing.Size(116, 40);
            this.ExitDemoBtn.TabIndex = 11;
            this.ExitDemoBtn.Text = "EXIT";
            this.ExitDemoBtn.UseVisualStyleBackColor = false;
            this.ExitDemoBtn.Click += new System.EventHandler(this.ExitDemoBtn_Click);
            // 
            // DemoLbl
            // 
            this.DemoLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DemoLbl.AutoSize = true;
            this.DemoLbl.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DemoLbl.ForeColor = System.Drawing.Color.LightCoral;
            this.DemoLbl.Location = new System.Drawing.Point(82, -6);
            this.DemoLbl.Name = "DemoLbl";
            this.DemoLbl.Size = new System.Drawing.Size(221, 78);
            this.DemoLbl.TabIndex = 10;
            this.DemoLbl.Text = "Demo";
            // 
            // StartDemoBtn
            // 
            this.StartDemoBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StartDemoBtn.BackColor = System.Drawing.Color.Transparent;
            this.StartDemoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartDemoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartDemoBtn.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDemoBtn.ForeColor = System.Drawing.Color.LightCoral;
            this.StartDemoBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.StartDemoBtn.Location = new System.Drawing.Point(32, 152);
            this.StartDemoBtn.Margin = new System.Windows.Forms.Padding(0);
            this.StartDemoBtn.Name = "StartDemoBtn";
            this.StartDemoBtn.Size = new System.Drawing.Size(116, 40);
            this.StartDemoBtn.TabIndex = 4;
            this.StartDemoBtn.Text = "START";
            this.StartDemoBtn.UseVisualStyleBackColor = false;
            this.StartDemoBtn.Click += new System.EventHandler(this.StartDemoBtn_Click);
            // 
            // MediaBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1070, 1053);
            this.ControlBox = false;
            this.Controls.Add(this.DemoPnl);
            this.Controls.Add(this.MediaBoxMenuBtn);
            this.Controls.Add(this.BottomPnl);
            this.Controls.Add(this.TopPnl);
            this.Controls.Add(this.LeftPnl);
            this.Controls.Add(this.PicturePnl);
            this.Name = "MediaBox";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "MediaBox";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MediaBox_FormClosed);
            this.Load += new System.EventHandler(this.MediaBox_Load);
            this.Move += new System.EventHandler(this.MediaBox_Move);
            this.Resize += new System.EventHandler(this.MediaBox_Resize);
            this.LeftPnl.ResumeLayout(false);
            this.ManageDirectoryPnl.ResumeLayout(false);
            this.ManageDirectoryPnl.PerformLayout();
            this.PersonalizeColorPnl.ResumeLayout(false);
            this.PersonalizeColorPnl.PerformLayout();
            this.GeneralSettingsPnl.ResumeLayout(false);
            this.GeneralSettingsPnl.PerformLayout();
            this.MusicPnl.ResumeLayout(false);
            this.MusicPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MusicPlayer)).EndInit();
            this.InfoPnl.ResumeLayout(false);
            this.InfoPnl.PerformLayout();
            this.TopPnl.ResumeLayout(false);
            this.TopPnl.PerformLayout();
            this.BottomPnl.ResumeLayout(false);
            this.DemoPnl.ResumeLayout(false);
            this.DemoPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PicturePnl;
        private System.Windows.Forms.Panel LeftPnl;
        private System.Windows.Forms.Button UpdateCatalogBtn;
        private System.Windows.Forms.Panel TopPnl;
        private System.Windows.Forms.Button MinimizeMediaBoxBtn;
        private System.Windows.Forms.Button CloseMediaBoxBtn;
        private System.Windows.Forms.Button MaximizeMediaBoxBtn;
        private System.Windows.Forms.Panel BottomPnl;
        private System.Windows.Forms.Button MediaBoxMenuBtn;
        private System.Windows.Forms.Timer MediaBoxMenuTmr;
        private System.Windows.Forms.Timer waitHideMediaBoxMenu;
        private System.Windows.Forms.Label TitleLbl;
        private System.Windows.Forms.Button OpenClosePersonalizeColorBtn;
        private System.Windows.Forms.Panel PersonalizeColorPnl;
        private System.Windows.Forms.Button StartStopAnimateBtn;
        private System.Windows.Forms.Label BlueLbl;
        private System.Windows.Forms.Label GreenLbl;
        private System.Windows.Forms.Label RedLbl;
        private System.Windows.Forms.ComboBox ChoiceColorCmb;
        private System.Windows.Forms.Label ViewColor;
        private System.Windows.Forms.Button SetColorBtn;
        private System.Windows.Forms.Button OpenCloseManageDirectoryBtn;
        private System.Windows.Forms.Panel ManageDirectoryPnl;
        private System.Windows.Forms.ListBox DirectoryList;
        private System.Windows.Forms.Button DirectoryDelBtn;
        private System.Windows.Forms.Button DirectoryAddBtn;
        private System.Windows.Forms.Button DirectoryOpenBtn;
        private System.Windows.Forms.TextBox DirectoryTxt;
        private System.Windows.Forms.Label Separator1;
        private System.Windows.Forms.Label Separator2;
        private System.Windows.Forms.Panel GeneralSettingsPnl;
        private System.Windows.Forms.Button OpenCloseGeneralSettingsBtn;
        private System.Windows.Forms.Label PictureIntervalLbl;
        private System.Windows.Forms.TextBox PictureIntervalTxt;
        private System.Windows.Forms.Label Separator3;
        private System.Windows.Forms.Timer PresentationTmr;
        private System.Windows.Forms.TextBox WidthBorderMediaTxt;
        private System.Windows.Forms.Label WidthBorderMediaLbl;
        private System.Windows.Forms.Label ResizeMediaBoxLbl;
        private System.Windows.Forms.Panel MusicPnl;
        private System.Windows.Forms.Label Separator5;
        private System.Windows.Forms.Button OpenCloseMusicBtn;
        private System.Windows.Forms.Panel InfoPnl;
        private System.Windows.Forms.Label Separator4;
        private System.Windows.Forms.Button OpenCloseInfoBtn;
        private System.Windows.Forms.Button ForwardMusicBtn;
        private System.Windows.Forms.Button PlayStopMusicBtn;
        private System.Windows.Forms.Button BackMusicBtn;
        private System.Windows.Forms.Label VersionLbl;
        private System.Windows.Forms.Button AutoRunAnimationMediaBtn;
        private System.Windows.Forms.Label CreatedBy_1Lbl;
        private System.Windows.Forms.Label CreatedBy_2Lbl;
        private AxWMPLib.AxWindowsMediaPlayer MusicPlayer;
        private System.Windows.Forms.ComboBox PlayListMusicCmb;
        private System.Windows.Forms.Label AudioLbl;
        private System.Windows.Forms.Panel AudioPnl;
        private System.Windows.Forms.Panel PersonalizeColorRedPnl;
        private System.Windows.Forms.Panel PersonalizeColorBluePnl;
        private System.Windows.Forms.Panel PersonalizeColorGreenPnl;
        private System.Windows.Forms.Panel DemoPnl;
        private System.Windows.Forms.Label DemoLbl;
        private System.Windows.Forms.Button StartDemoBtn;
        private System.Windows.Forms.Button DemoBtn;
        private System.Windows.Forms.Button ExitDemoBtn;
        private System.Windows.Forms.Label LanguageenUSLbl;
        private System.Windows.Forms.Label LanguageitITLbl;
        private System.Windows.Forms.Button LanguageenUSBtn;
        private System.Windows.Forms.Button LanguageitITBtn;
        private System.Windows.Forms.Label LanguageenUSDemoLbl;
        private System.Windows.Forms.Label LanguageitITDemoLbl;
        private System.Windows.Forms.Button LanguageenUSDemoBtn;
        private System.Windows.Forms.Button LanguageitITDemoBtn;
        private Button SetGeneralSettingsBtn;
        private Label AutoRunAnimationMediaLbl;
        private Label DurationLblDemo;
        private PictureBox pictureBox1;
        private Panel MediaMovementSpeedPnl;
        private Label MediaMovementSpeedLbl;
        private TextBox VideoIntervalTxt;
        private Label VideoIntervalLbl;
    }
}

