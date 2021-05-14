using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaBox
{
    public partial class TitleMedia : Form
    {
        private bool showed = false;
        public TitleMedia()
        {
            InitializeComponent();
        }

        public void ShowTitle(string title, int left, int top, int width)
        {
            Lbl.Text = title;
            if (showed) return;

            this.Show();
            this.Visible = false;
            this.Left = left;
            this.Width = width;
            this.Top = top;
            this.BackColor = Statics.FormParam.BackGrondColorMain;
            this.Opacity = .60;
            Lbl.BackColor = Color.Transparent;
            Lbl.Left = width;
            Lbl.Parent = this;
            Lbl.ForeColor = Statics.FormParam.ForeColor;
            Lbl.Visible = true;
            this.Visible = true;
            showed = true;
        }

        public void CloseTitle()
        {
            this.Hide();
            showed = false;
        }

        public bool TitleIsShowed()
        {
            return showed;
        }

        public void MoveTitle(int left, int top)
        {
            try
            {
                this.Top = top;
                Lbl.Left = left;
                this.TopMost = true;
                this.TopMost = false;
                Lbl.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //throw;
            }

        }

        public int GetLeftTitle()
        {
            return Lbl.Left;
        }

        public int GetWidthTitle()
        {
            return Lbl.Width;
        }
    }
}
