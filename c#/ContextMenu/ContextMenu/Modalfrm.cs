using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContextMenu
{
    public partial class Modalfrm : Form
    {
        private static string _value = String.Empty;
        public Modalfrm()
        {
            InitializeComponent();
        }

        public void OpenModalFrm(Form mainForm, string value)
        {
            _value = value;
            this.ShowDialog(mainForm);
        }

        private void Modalfrm_Load(object sender, EventArgs e)
        {
            this.Text = "Edit " + _value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_value + " - " + textBox1.Text);
        }
    }
}
