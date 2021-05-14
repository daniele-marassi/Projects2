using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emulation
{
    public partial class Tooltip : Form
    {

        public Tooltip()
        {
            InitializeComponent();
            this.Visible = false;
        }

        public void ShowTooltip(string text, Point position, Color foreColor = new Color(), Color backColor = new Color())
        {
            if (foreColor == new Color())
            {
                foreColor = Color.FromArgb(255, 255, 255, 255);
            }
            this.TooltipLbl.ForeColor = foreColor;
            if (backColor != new Color())
            {
                this.TooltipLbl.BackColor = backColor;
            }
            this.Top = position.Y;
            this.Left = position.X;
            this.Visible = true;
            TooltipLbl.Text = text;
            TooltipLbl.Update();
            TooltipLbl.Refresh();
            this.Update();
            this.Refresh();
            this.Height = TooltipLbl.Height;
            this.Width = TooltipLbl.Width;
            Move(position);
        }

        public void Move(Point position)
        {
            int widthScreen = Screen.GetWorkingArea(this).Width;
            int heightScreen = Screen.GetWorkingArea(this).Height;

            this.Top = position.Y;
            this.Left = position.X;

            if ((this.Top + this.Height) > heightScreen)
            {
                this.Top = (heightScreen - this.Height);
            }

            if ((this.Left + this.Width) > widthScreen)
            {
                this.Left = (widthScreen - this.Width);
            }

            TooltipLbl.Update();
            TooltipLbl.Refresh();
            this.Update();
            this.Refresh();
        }

        private void Tooltip_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;
        }
    }
}
