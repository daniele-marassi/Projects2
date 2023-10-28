using Additional;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace Tools
{
    public partial class ExceptionsManagementFrm : Form
    {
        public ExceptionsManagementFrm(Color backGrounColor = new Color(), Color foreColor = new Color())
        {
            _backGrounColor = backGrounColor;
            _foreColor = foreColor;
            InitializeComponent();

            this.Show();
        }

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
                this.Icon = new Icon($"{Path.GetDirectoryName(Application.ExecutablePath)}\\Resources\\ExceptionsManagement.ico");
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

                this.labelTitle.Text = "Exceptions Management";
                this.labelTitle.ForeColor = _foreColor;

                var exceptionTypes = System.Configuration.ConfigurationManager.AppSettings["ExceptionTypes"];

                if (exceptionTypes == null || exceptionTypes == String.Empty)
                {
                    exceptionTypes = "BoxRepositioningExceptions,TODOExceptions";

                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    config.AppSettings.Settings.Add("ExceptionTypes", exceptionTypes);

                    config.Save(ConfigurationSaveMode.Modified);
                }

                var exceptionTypeList = exceptionTypes.Split(',');

                foreach (var exceptionType in exceptionTypeList)
                {
                    this.ExceptionTypesCmb.Items.Add(exceptionType);
                }

                foreach (var item in this.ExceptionTypesCmb.Items)
                {
                    try
                    {
                        var values = System.Configuration.ConfigurationManager.AppSettings[item.ToString()];

                        if(values != null && values != String.Empty) 
                        { 
                            var valueList = values.Split(',');
                            foreach (var value in valueList)
                            {
                                ExceptionsList.Items.Add(item + " " + value);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

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


                this.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExceptionsManagementFrm_Load(object sender, EventArgs e)
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

        private void AddException_Click(object sender, EventArgs e)
        {
            if (ExceptionTypesCmb.Text == String.Empty || ExceptionTypesCmb.Text == null)
                MessageBox.Show("Select Exception Type");
            else
            {
                ExceptionsList.Items.Add(ExceptionTypesCmb.Text + " " + ExceptionTxt.Text);
            }
        }

        private void SelectFileBtn_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExceptionTxt.Text =  Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
            }
        }

        private void DelExceptionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ExceptionsList.Items.RemoveAt(ExceptionsList.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }

        private void AddExceptionsBtn_Click(object sender, EventArgs e)
        {
            var exceptions = new Dictionary<string, List<string>>();

            foreach (var item in ExceptionsList.Items)
            {
                var key = item.ToString().Split(' ')[0];
                var value = item.ToString().Split(' ')[1];
                if (exceptions.ContainsKey(key))
                {
                    exceptions[key].Add(value);
                }
                else
                {
                    exceptions.Add(key, new List<string>() { value });
                }
            }

            foreach (var item in exceptions)
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); // Add an Application Setting.
                try
                {
                    config.AppSettings.Settings.Remove(item.Key);
                }
                catch (Exception)
                {
                }
                
                config.AppSettings.Settings.Add(item.Key, String.Join(",", item.Value));

                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}