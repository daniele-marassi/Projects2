using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wait
{
    public partial class WaitShadow : Form
    {
        public WaitShadow(int widthShadow = 0, int heightShadow=0, int topShadow=0, int leftShadow=0, int widthBar=0, int heightBar=0, Color backColor= default(Color), Color color= default(Color))
        {
            InitializeComponent();

            this.Show();
            this.Top = topShadow;
            this.Left = leftShadow;
            this.Height = heightShadow;
            this.Width = widthShadow;
            this.BackColor = backColor;
            this.TopMost = true;
            WaitBar waitBar = new WaitBar();
            waitBar.Show();
            waitBar.Top = (this.Top / 2) - (waitBar.Height / 2);
            waitBar.Left = (this.Left / 2) - (waitBar.Width / 2);
            waitBar.TopMost = true;
            waitBar.StartWaitBar(widthBar, heightBar, backColor, color);
        }
    }
}
