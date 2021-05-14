using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bentley.MicroStation.InteropServices;
using csAddinsTest;
using BCOM = Bentley.Interop.MicroStationDGN;
using Bentley.Interop.MicroStationDGN;


namespace GetAllElements
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationObjectConnector applicationObjectConnector = new ApplicationObjectConnectorClass();
                Bentley.Interop.MicroStationDGN.Application app = applicationObjectConnector.Application;
                app.Visible = false;
                app.CadInputQueue.SendKeyin("csAddinsTest ScanElement Utility");
                DesignFile designFile = app.OpenDesignFile(@"C:\Users\Administrator\Documents\JVPC\dms00068\006271-ACS10111B-HVAC.dgn", false, MsdV7Action.UpgradeToV8);
                app.CadInputQueue.SendKeyin("csAddinsTest ScanElement Utility");
                //designFile.Save();
                designFile.Close();
                app.Quit();
                MessageBox.Show("Utility Executed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
