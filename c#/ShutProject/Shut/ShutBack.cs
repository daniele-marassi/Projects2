using Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shut
{
    public partial class ShutBack : Form
    {
        private static Utility utility = new Utility();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,  //x-coordinate of upper-left corner
            int nTopRect,  //y-coordinate of upper-left corner
            int nRightRect,  //x-coordinate of lower-left corner
            int nBottomRect,  //y-coordinate of lower-left corner
            int nWidthEllipse,  //width of ellipse
            int nHeightEllipseRect  //eight of ellipse
        );

        public ShutBack()
        {
            InitializeComponent();
            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;
            this.Opacity = 0;

            try
            {
                
                //Shut.style = "style_Classic";

                if (Shut.style == "style_Classic")
                {
                    Back.Text = "n";
                    Back.Font = utility.GetFonts("Webdings", 50, Shut.Fonts);
                    Back.Location = new Point((int)-((Back.Width / 2) - (Back.Width * ((Back.Height / Back.Width * 100) / 2) / 100)), this.Height - (Back.Height / 2));
                }
                if (Shut.style == "style_Spalsh")
                {
                    Back.Text = "O";
                    Back.Font = utility.GetFonts("Dog Rough", 50, Shut.Fonts);
                    Back.Location = new Point((int)-((Back.Width / 2) - (Back.Width * ((Back.Height / Back.Width * 100) / 2) / 100)), this.Height - (Back.Height / 2));
                }
            }
            catch (Exception ex)
            {

                //throw;
            }


            //Back.Font = new Font(Back.Font.FontFamily, 50, FontStyle.Regular);
        }

        public void SetToolTip(string text, Color color)
        {
            ToolTipLbl.ForeColor = color;
            ToolTipLbl.Text = text;
        }

        public void SetColor(Color color)
        {

            //Back.ForeColor = Color.FromArgb(color.ToArgb() ^ 0xffffff);

            //Back.ForeColor = Color.FromArgb(color.ToArgb());
            //Back.ForeColor = Color.FromArgb(Color.Black.ToArgb());
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string backColor = config.AppSettings.Settings["backColor"].Value;

            Back.ForeColor = utility.GenerateRgba(backColor);

            //Back.Font = new Font(Back.Font, FontStyle.Bold);
        }

        public void OpenClose(int opacity, int step)
        {
            this.Opacity = opacity/100d;
            float fontDimension = 0;

            if (step == 0)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 400;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 800;
            }
            if (step == 1)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 450;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 850;
            }
            if (step == 2)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 500;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 900;
            }
            if (step == 3)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 525;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 950;
            }
            if (step == 4)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 550;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1000;
            }
            if (step == 5)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 575;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1050;
            }
            if (step == 6)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 600;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1100;
            }
            if (step == 7)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 625;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1150;
            }
            if (step == 8)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 650;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1200;
            }
            if (step == 9)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 675;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1250;
            }
            if (step == 10)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 700;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1300;
            }
            if (step == 11)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 725;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1350;
            }
            if (step == 12)
            {
                if (Shut.style == "style_Classic")
                    fontDimension = 760;
                if (Shut.style == "style_Spalsh")
                    fontDimension = 1700;
            }
            //Back.Font = new Font(Back.Font.FontFamily, fontDimension, FontStyle.Regular);
            Back.Font =  utility.GetFonts(Back.Font.FontFamily.Name, fontDimension, Shut.Fonts);

            int x = 0;
            int y = 0;

            if(Shut.style == "style_Classic")
            {
                x = -(int)((Back.Width / 2d) - ((Back.Width - Back.Height) / 4)) + 50;
                y = this.Height - (int)(Back.Height / 2d) - 50;
            }
            if (Shut.style == "style_Spalsh")
            {
                x = -(int)((Back.Width / 2d) - ((Back.Width - Back.Height) / 4)) + 120;
                y = this.Height - (int)(Back.Height / 2d) - 60;
            }

            Back.Location = new Point(x, y);
            ToolTipLbl.Location = new Point(((int)((Back.Width + Back.Left) / 2d)) + 0, ToolTipLbl.Top);
        }

        public void Refresh()
        {
            this.TopMost=true;
        }

        private void ShutBack_Load(object sender, EventArgs e)
        {

        }
    }
}
