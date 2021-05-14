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

namespace Wait
{
    public partial class WaitBar : Form
    {
        private Additional.ProgressBar progressBar = new Additional.ProgressBar();

        public WaitBar()
        {
            InitializeComponent();
        }

        private void ProgressBar(int countElement, string info, Form form, string status = null, Color backColor = default(Color), Color color = default(Color))
        {
            Additional.ProgressBar.StatusOption statusOption = new Additional.ProgressBar.StatusOption();
            if (status == statusOption.Start) { ProgressBarStart(); }
            string currentStatusOption = progressBar.Step(countElement, info, backColor, color, form, status);
            if (currentStatusOption == statusOption.End) { ProgressBarEnd(); }
        }

        private void ProgressBarStart()
        {

        }

        private void ProgressBarEnd()
        {

        }

        public void StartWaitBar(int width, int height, Color backColor, Color color)
        {
            Utility utility = new Utility();
            Additional.ProgressBar progressBar = new Additional.ProgressBar();
            Additional.ProgressBar.StatusOption statusOption = new Additional.ProgressBar.StatusOption();
            progressBar.Create(this, 0, 0, width, height, backColor, false);
            this.Height = height;
            var ProgressBarPnl = this.Controls["ProgressBarPnl"];
            ProgressBar(100, "", this, statusOption.Start, backColor, color);
        }

        private void WaitBar_Load(object sender, EventArgs e)
        {
            
        }
    }
}
