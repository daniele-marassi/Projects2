using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lettera
{
    public partial class Lettera : Form
    {
        public Lettera()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MaximizedBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            Foglio.Width = 500;
            Foglio.Height = 740;
            pictureBox1.Width = Foglio.Width;
            pictureBox1.Height = Foglio.Height;
            pictureBox1.Left = -30;

        }

        private void Lettera_MouseDown(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
            {
                currentPos = new Point(e.X, e.Y);
            }
            
            mouseDown = true;

        }

        private static bool mouseDown = false;
        private static Point currentPos = new Point(0,0);

        private void Lettera_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseDown)
            {
                this.Top = Cursor.Position.Y - currentPos.Y;
                this.Left = Cursor.Position.X - currentPos.X;
            }

        }

        private void Lettera_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
