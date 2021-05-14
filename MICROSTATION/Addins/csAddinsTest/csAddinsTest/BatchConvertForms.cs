using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace csAddinsTest
{
    public partial class BatchConvertForms : Form
    {
        public BatchConvertForms()
        {
            InitializeComponent();
        }

        private void SelFilesBtn_Click(object sender, EventArgs e)
        {

        }

        private void MakeBcnvFileBtn_Click(object sender, EventArgs e)
        {

        }

        private void BatchConvertForms_Load(object sender, EventArgs e)
        {
            FilesSelectedLst.Items.Add("DXF");
            FilesSelectedLst.Items.Add("DWG");
            FilesSelectedLst.Items.Add("V7");
            FilesSelectedLst.Items.Add("V8");
        }
    }
}
