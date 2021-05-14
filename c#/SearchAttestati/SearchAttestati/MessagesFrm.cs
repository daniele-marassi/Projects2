using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchAttestati
{
    public partial class MessagesFrm : Form
    {
        public MessagesFrm()
        {
            InitializeComponent();
        }

        private void Messages_Load(object sender, EventArgs e)
        {

        }

        public void ShowMessages(string messages)
        {
            MessagesTxt.Text = messages;
        }
    }
}
