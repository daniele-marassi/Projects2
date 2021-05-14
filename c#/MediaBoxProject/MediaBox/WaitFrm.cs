using Additional;
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
    public partial class WaitFrm : Form
    {
        private Additional.ProgressBar progressBar = null;
        private Additional.ProgressBar.StatusOption statusOption = null;
        public WaitFrm()
        {
            this.Visible = false;
            InitializeComponent();  
        }

        private void ProgressBar(int countElement, string info, Form form, string status = null, Color backColor = default(Color), Color color = default(Color))
        {
            Additional.ProgressBar.StatusOption statusOption = new Additional.ProgressBar.StatusOption();
            if (status == statusOption.Start) { ProgressBarStart(); }
            string currentStatusOption = progressBar.Step(countElement, info, backColor, color, form, status,100);
            if (currentStatusOption == statusOption.End) { ProgressBarEnd(); }
        }

        private void ProgressBarStart()
        {

        }

        private void ProgressBarEnd()
        {

        }

        public void StartWaitBar(Form form, int top, int left, int width, int height, Color backColor, Color color)
        {
            try
            {
                Utility utility = new Utility();
                progressBar = new Additional.ProgressBar();
                statusOption = new Additional.ProgressBar.StatusOption();
                progressBar.Create(this, 0, 0, width, height, backColor, false);
                this.Height = height;
                this.Width = width;
                this.Top = top;
                this.Left = left;
                if (this.Visible == null) return;
                this.Visible = true;
                this.TopMost = true;
                this.TopMost = false;
                var ProgressBarPnl = this.Controls["ProgressBarPnl"];
                Application.DoEvents();
                ProgressBar(100, "", this, statusOption.Start, backColor, color);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
