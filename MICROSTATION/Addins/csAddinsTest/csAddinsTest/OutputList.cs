using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bentley.MicroStation.WinForms;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace csAddinsTest
{
    public partial class OutputList : //Form
                                      Adapter
    {
        private static string _output = String.Empty;

        public OutputList()
        {
            InitializeComponent();
        }

        public void AddItem(string value)
        {
            this.listBox1.Items.Add(value);
        }

        private void OutputList_Load(object sender, EventArgs e)
        {

        }
    }
}