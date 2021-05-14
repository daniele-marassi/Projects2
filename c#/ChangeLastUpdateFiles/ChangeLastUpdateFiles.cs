using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChangeLastUpdateFiles
{
    public partial class ChangeLastUpdateFiles : Form
    {

        static DateTime now = DateTime.Now;
        public ChangeLastUpdateFiles()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ChangeBtn_Click(object sender, EventArgs e)
        {
            var path = PathTxt.Text;
            if (Directory.Exists(path))
            {
                // This path is a directory
                ProcessDirectory(path);
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }
            MessageBox.Show("Fatto");

        }
        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            bool loop = true;
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            try
            {
                Directory.SetLastWriteTime(targetDirectory, now);
                Directory.SetCreationTime(targetDirectory, now);
                Directory.SetLastAccessTime(targetDirectory, now);
                Console.WriteLine("Processed Directory '{0}'.", targetDirectory);
            }
            catch (Exception)
            {
                MessageBox.Show("Folder: " + targetDirectory + " in use");
                loop = false;
            }

            foreach (string fileName in fileEntries)
            {
                try
                {
                    File.SetLastWriteTime(fileName, now);
                    File.SetCreationTime(fileName, now);
                    File.SetLastAccessTime(fileName, now);
                    Console.WriteLine("Processed file '{0}'.", fileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("File: " + fileName + " in use");
                    loop = false;
                    break;
                }
            }

            if (loop)
            {
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
            }
        }
    }
}