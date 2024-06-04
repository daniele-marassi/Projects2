using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckAvailablePortOnIP
{
    public partial class CheckAvailablePortOnIPFrm : Form
    {
        public CheckAvailablePortOnIPFrm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            var start = int.Parse(PortStartTxt.Text);

            var end = int.Parse(PortEndTxt.Text);

            for (int i = start; i <= end; i++)
            {
                try
                {
                    var c = new TcpClient("192.168.1.33", i);

                    PortAvviableList.Items.Add(i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                PortTestedLbl.Text = i.ToString();
                PortTestedLbl.Refresh();
                PortAvviableList.Refresh();
                this.Refresh();
            }

            this.Enabled = true;
        }
    }
}
