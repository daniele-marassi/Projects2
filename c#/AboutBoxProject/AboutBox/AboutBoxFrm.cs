using Additional;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace AboutBox
{
    public partial class AboutBoxFrm : Form
	{
		public AboutBoxFrm(string assemblyTitle = null, string assemblyProduct = null, string assemblyVersion = null, string assemblyCopyright = null, string assemblyCompany = null, string assemblyDescription = null, Color backGrounColor = new Color(), Color foreColor = new Color())
		{
            _assemblyTitle = assemblyTitle;
            _assemblyProduct = assemblyProduct;
            _assemblyVersion = assemblyVersion;
            _assemblyCopyright = assemblyCopyright;
            _assemblyCompany = assemblyCompany;
            _assemblyDescription = assemblyDescription;
            _backGrounColor = backGrounColor;
            _foreColor = foreColor;
            InitializeComponent();

            this.Show();
        }

        private static string _assemblyTitle = String.Empty;
        private static string _assemblyProduct = String.Empty;
        private static string _assemblyVersion = String.Empty;
        private static string _assemblyCopyright = String.Empty;
        private static string _assemblyCompany = String.Empty;
        private static string _assemblyDescription = String.Empty;
        private static Color _backGrounColor = Color.Gray;
        private static Color _foreColor = Color.Red;
        private static PrivateFontCollection fonts = new PrivateFontCollection();
        private Utility utility = new Utility();
        private static Point locationCloselbl;

        private static string _project_Domain_Class_ToClose = null;
        private static string _methodName_ToClose = null;

        private void Init()
        {
            try
            {
                this.Icon = new Icon($"{Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\About.ico");
                this.Visible = false;
                
                if (_backGrounColor == default(Color)) _backGrounColor = Color.Gray;

                this.TransparencyKey = _backGrounColor;
                this.BackColor = _backGrounColor;
                this.Opacity = 0.9;

                locationCloselbl = new Point(this.Closelbl.Left, this.Closelbl.Top);

                fonts.AddFontFile($"{Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Cafed_v2.ttf");
                fonts.AddFontFile($"{Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\PAPYRUS.ttf");
                fonts.AddFontFile($"{Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\We Spray.ttf");
                fonts.AddFontFile($"{Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\Fonts\\Byom-Icons-Trial.ttf");

                this.BackgroundLbl.Font = utility.GetFonts("We Spray", 500F, fonts);
                this.LogoRingLbl.Font = utility.GetFonts("Cafeina Dig_v2", 250F, fonts);
                this.LogoSign1Lbl.Font = utility.GetFonts("Papyrus", 36F, fonts);
                this.LogoSign2Lbl.Font = utility.GetFonts("Papyrus", 28F, fonts);
                this.Closelbl.Font = utility.GetFonts("Byom Icons", 30F, fonts);

                this.labelTitle.Text = String.Format("About {0}", _assemblyTitle);
                this.labelProductName.Text = _assemblyProduct;
                this.labelVersion.Text = String.Format("Version {0}", _assemblyVersion);
                this.labelCopyright.Text = _assemblyCopyright;
                this.labelCompanyName.Text = _assemblyCompany;
                this.textBoxDescription.Text = _assemblyDescription;
                
                this.labelTitle.ForeColor = _foreColor;
                this.labelProductName.ForeColor = _foreColor;
                this.labelVersion.ForeColor = _foreColor;
                this.labelCopyright.ForeColor = _foreColor;
                this.labelCompanyName.ForeColor = _foreColor;
                this.textBoxDescription.ForeColor = _foreColor;
                
                this.LogoRingLbl.ForeColor = _foreColor;
                this.LogoSign1Lbl.ForeColor = _foreColor;
                this.LogoSign2Lbl.ForeColor = _foreColor;
                this.Closelbl.ForeColor = _foreColor;

                this.BackgroundLbl.Location = new System.Drawing.Point(-360, -50);
                this.BackgroundLbl.ForeColor = utility.ConvertARGBtoRGB(Color.FromArgb(254, _backGrounColor.R, _backGrounColor.G, _backGrounColor.B));
                this.LogoRingLbl.Parent = this.BackgroundLbl;
                this.LogoRingLbl.Location = new System.Drawing.Point(340, -110);
                this.LogoSign1Lbl.Parent = this.LogoRingLbl;
                this.LogoSign1Lbl.Location = new System.Drawing.Point(160, 250);
                this.LogoSign2Lbl.Parent = this.LogoRingLbl;
                this.LogoSign2Lbl.Location = new System.Drawing.Point(160, 320);
                
                this.Closelbl.BackColor = this.BackgroundLbl.ForeColor;
                this.labelTitle.BackColor = this.BackgroundLbl.ForeColor;
                this.labelProductName.BackColor = this.BackgroundLbl.ForeColor;
                this.labelVersion.BackColor = this.BackgroundLbl.ForeColor;
                this.labelCopyright.BackColor = this.BackgroundLbl.ForeColor;
                this.labelCompanyName.BackColor = this.BackgroundLbl.ForeColor;
                this.textBoxDescription.BackColor = this.BackgroundLbl.ForeColor;
                
                this.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void AboutBoxFrm_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Closelbl_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Closelbl_MouseHover(object sender, EventArgs e)
        {
            this.Closelbl.ForeColor = utility.ConvertARGBtoRGB(Color.FromArgb(200, _foreColor));
            this.Closelbl.Font = utility.GetFonts("Byom Icons", 34F, fonts);
            this.Closelbl.Location = locationCloselbl;
            this.Closelbl.Top -= 3;
            this.Closelbl.Left -= 4;
        }

        private void Closelbl_MouseLeave(object sender, EventArgs e)
        {
            this.Closelbl.ForeColor = utility.ConvertARGBtoRGB(Color.FromArgb(255, _foreColor));
            this.Closelbl.Font = utility.GetFonts("Byom Icons", 30F, fonts);
            this.Closelbl.Location = locationCloselbl;
        }

        private void Closelbl_MouseDown(object sender, MouseEventArgs e)
        {
            this.Closelbl.ForeColor = utility.ConvertARGBtoRGB(Color.FromArgb(150, _foreColor));
            this.Closelbl.Font = utility.GetFonts("Byom Icons", 26F, fonts);
            this.Closelbl.Location = locationCloselbl;
            this.Closelbl.Top += 3;
            this.Closelbl.Left += 4;
        }

        private void Closelbl_MouseUp(object sender, MouseEventArgs e)
        {
            this.Closelbl.ForeColor = utility.ConvertARGBtoRGB(Color.FromArgb(255, _foreColor));
            this.Closelbl.Font = utility.GetFonts("Byom Icons", 30F, fonts);
            this.Closelbl.Location = locationCloselbl;
        }
    }
}