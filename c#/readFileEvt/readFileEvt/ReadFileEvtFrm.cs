using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.IO;

namespace readFileEvt
{
    public partial class ReadFileEvtFrm : Form
    {
        public ReadFileEvtFrm()
        {
            InitializeComponent();
            feelFiterCmb();
        }

        public void feelFiterCmb()
        {
            PropertyInfo[] props = typeof(System.Diagnostics.Eventing.Reader.EventRecord).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                string propName = prop.Name;
                FilterCmb.Items.Add(propName);
            }
            FilterCmb.Items.Add("Object Name");
        }

        public void Search()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Event Files|*.evt";
            openFileDialog1.Title = "Select a Event File";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                sr.Close();
            }

            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            string filePath = "C:\\Temp\\"+DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + "_ResultList.csv";

            string text = "ID;VESRSION;QUALIFIERS;LEVEL;TASK;OPCODE;KEYWORDS;RECORDID;PROVIDERNAME;PROVIDERID;LOGNAME;PROCESSID;THREADID;MACHINENAME;USERID;TIMECREATED;ACTIVITYID;RELATEDACTIVITY;CONTAINERLOG;MATCHEDQUERYIDS;BOOKMARKS;LEVELDISPLAY;OPCOCEDISPLAYNAME;TASKDISPLAYNAME;KEYWORDSDISPLAYNAMES;PROPERTIES;PROPERTY[0];PROPERTY[1];PROPERTY[2];PROPERTY[3];PROPERTY[4];PROPERTY[5];PROPERTY[6];PROPERTY[7];PROPERTY[8];PROPERTY[9];PROPERTY[10];PROPERTY[11];PROPERTY[12];PROPERTY[13];PROPERTY[14];PROPERTY[15];PROPERTY[16];PROPERTY[17];PROPERTY[18];PROPERTY[19];PROPERTY[20];PROPERTY[21];PROPERTY[22];PROPERTY[23]";
            SaveInFile(text, filePath);
            int nFile = 0;
            foreach (var fileName in openFileDialog1.FileNames)
            {
                Application.DoEvents();
                nFile++;
                EventList.Items.Add($"{nFile} - {openFileDialog1.FileNames.Count()}");

                using (var reader = new EventLogReader(fileName, PathType.FilePath))
                {
                    EventRecord record;
                    while ((record = reader.ReadEvent()) != null)
                    {
                        using (record)
                        {
                            int countMatch = 0;
                            foreach (string item in FilterList.Items)
                            {
                                string[] filter = item.Split('=');
                                string prop = filter[0];
                                string value = filter[1];
                                int resultFind = -1;
                                List<string> propsResult = new List<string>() { };
                                var resultGetProperty = record.GetType().GetProperty(prop);

                                if (resultGetProperty != null) { propsResult.Add(record.GetType().GetProperty(prop).GetValue(record, null)?.ToString()); }

                                if (propsResult.Count <= 0)
                                {
                                    foreach (var properties in record.Properties)
                                    {
                                        propsResult.Add(properties.Value.ToString());
                                    }
                                }
                                foreach (var propResult in propsResult)
                                {
                                    resultFind = propResult.ToString().IndexOf(value.Replace("*", ""));
                                    if (resultFind >= 0)
                                    {
                                        countMatch++;
                                    }
                                }
                            }
                            if (countMatch == FilterList.Items.Count)
                            {
                                text = String.Empty;
                                foreach (var prop in record.GetType().GetProperties())
                                {
                                    string value = prop.GetValue(record, null)?.ToString(); 
                                    if (value == String.Empty || value == null) { value = "''"; }
                                    if (text != String.Empty) { text += "; "; }
                                    text += $"{prop.Name?.Replace("\r\n", " ")}={value?.Replace("\r\n", " ")}";
                                }
                                int x = 0;
                                foreach (var prop in record.Properties)
                                {
                                    string value = prop.Value?.ToString(); 
                                    if (value == String.Empty || value == null) { value = "''"; }
                                    if (text != String.Empty) { text += "; "; }
                                    text += $"Property[{x.ToString()}]={value?.Replace("\r\n", " ")}";
                                    x++;
                                }
                                //EventList.Items.Add(text);
                                SaveInFile(text,filePath);
                            }
                        }
                    }
                }
            }
            this.UseWaitCursor = false;
            this.Cursor = Cursors.Default;
            MessageBox.Show("Finito!");
        }

        private void SaveInFile(string text,string filePath)
        {
            // This text is added only once to the file.
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(text);

                }
            }
            else
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(text);
                }
            }

        }

        private void AddFilterBtn_Click(object sender, EventArgs e)
        {
            FilterList.Items.Add($"{FilterCmb.Text}=*{FilterTxt.Text}*");
        }

        private void ClearSelectFilterBtn_Click(object sender, EventArgs e)
        {
            while (FilterList.SelectedItems.Count > 0)
            {
                FilterList.Items.Remove(FilterList.SelectedItems[0]);
            }
        }

        private void ClearAllFilterBtn_Click(object sender, EventArgs e)
        {
            FilterList.Items.Clear();
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void ClearEventList_Click(object sender, EventArgs e)
        {
            EventList.Items.Clear();
        }

        private void CopySelectedEvents_Click(object sender, EventArgs e)
        {
            try
            {
                string text = String.Empty;

                for (int i = 0; i < EventList.SelectedItems.Count; i++)
                {
                    text += EventList.SelectedItems[i].ToString() + "\n\r";
                }

                Clipboard.SetText(text);
               
            }
            catch (Exception)
            {
            }
        }

        private void SelectAllEventsBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < EventList.SelectedItems.Count; i++)
            {
                EventList.SelectedItem = EventList.Items.IndexOf(i);
            }
        }

        private void ClearSelectBtn_Click(object sender, EventArgs e)
        {
            EventList.ClearSelected();
        }
    }
}
