using Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emulation
{
    public partial class MouseClicking : Form
    {
        private static int _fontSize = 12;
        private static Point _location = new Point();
        private static Color _color = new Color();
        private static string mainPath = null;

        public MouseClicking(int fontSize = 12, Point location = new Point(), Color color = new Color())
        {
            InitializeComponent();
            _fontSize = fontSize;
            _location = location;
            _color = color;
        }

        private void MouseClicking_Load(object sender, EventArgs e)
        {
            mainPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').ToString();

            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;

            Font font = new Font("Webdings", _fontSize);
            Clicking.Font = font;

            if (_color == new Color())
            {
                _color = Color.FromArgb(255, 255, 255, 255);
            }

            Clicking.ForeColor = _color;
            this.Width = Clicking.Width;
            this.Height = Clicking.Height;
            this.Location = _location;
            this.Opacity = 0;  
        }

        public void Down(bool soundActive = false)
        {
            if (soundActive == true)
            {
                SoundPlayer simpleSound = new SoundPlayer($@"{mainPath}\\Resources\\SoundEffects\\ClickDown.wav");
                simpleSound.PlaySync();
            }
            for (int i = 0; i <= 70; i++)
            {
                Application.DoEvents();
                this.Opacity = (double)(i / 100d);
                Utility utility = new Utility();
                utility.Sleep(2);
            }
        }

        public void Up(bool soundActive = false)
        {
            if (soundActive == true)
            {
                SoundPlayer simpleSound = new SoundPlayer($@"{mainPath}\\Resources\\SoundEffects\\ClickUp.wav");
                simpleSound.PlaySync();
            }
            for (int i = 70; i >= 0; i--)
            {
                Application.DoEvents();
                Opacity = (double)(i / 100d);
                Utility utility = new Utility();
                utility.Sleep(2);
            }
        }


    }
}
