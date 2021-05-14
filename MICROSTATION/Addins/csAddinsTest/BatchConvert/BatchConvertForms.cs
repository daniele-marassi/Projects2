using BatchConvert.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BatchConvert
{
    public partial class BatchConvertForms : Form
    {
        public BatchConvertForms()
        {
            InitializeComponent();
        }

        private void SelFilesBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            openFileDialog1.Multiselect = true;

            foreach (var file in openFileDialog1.FileNames)
            {
                SelectedFilesLst.Items.Add(file);
            }
        }

        private void MakeBcnvFileBtn_Click(object sender, EventArgs e)
        {
            if (ConvertToFormatCmb.SelectedIndex == -1) 
            { 
                MessageBox.Show("Selezionare Formato!"); 
                return; 
            }
            if (!Directory.Exists(DestinationDirTxt.Text))
            {
                MessageBox.Show("Inserire una directory valida di destinazione!"); 
                return;
            }
            if(SelectedFilesLst.Items.Count == 0) 
            { 
                MessageBox.Show("Selezionare i file da converire!"); 
                return; 
            }

            string strBCNVfile = ""; // Resources.BCNVfile.ToString();


            byte[] _BCNVfile = Resources.BCNVfile;
            Stream stream = new MemoryStream(_BCNVfile);

            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    strBCNVfile += line + System.Environment.NewLine;
                }
            }

            strBCNVfile = strBCNVfile.Replace("#DESTINATIONDIR#", DestinationDirTxt.Text);
            strBCNVfile = strBCNVfile.Replace("#CONVERTTOFORMAT#", ConvertToFormatCmb.Text);

            var _selectedFiles = "";

            foreach (var item in SelectedFilesLst.Items)
            {
                _selectedFiles += CreateSourceParameters(item.ToString(), ConvertToFormatCmb.Text, DestinationDirTxt.Text);
            }

            strBCNVfile = strBCNVfile.Replace("#SELECTEDFILES#", _selectedFiles);

            System.IO.File.WriteAllText(@"C:\BentleyV8i\MicroStation\mdlapps\BCNVfile.bcnv", strBCNVfile);

            MessageBox.Show(@"Creato il file: C:\BentleyV8i\MicroStation\mdlapps\BCNVfile.bcnv");

        }

        private string CreateSourceParameters(string sourcePath, string format, string destPath)
        {
            var result = "";

            result += @"[SOURCE="+ sourcePath+"]";
            result += System.Environment.NewLine;
            result += @"        DESTFMT="+ format;
            result += System.Environment.NewLine;
            result += @"        DESTDIR=" + destPath;
            result += System.Environment.NewLine;

            return result;
        }


        private void BatchConvertForms_Load(object sender, EventArgs e)
        {
            ConvertToFormatCmb.Items.Add("DXF");
            ConvertToFormatCmb.Items.Add("DWG");
            ConvertToFormatCmb.Items.Add("V7");
            ConvertToFormatCmb.Items.Add("V8");
            ConvertToFormatCmb.SelectedIndex = -1;
        }
    }
}
