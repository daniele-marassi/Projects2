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
using DevExpress.XtraPdfViewer;

namespace ViewDrawing
{
    public partial class ViewDrawing : Form
    {
        private static int nLevelNewNode = 0;
        private bool mouseDown;
        private Point lastLocation;
        private static TreeNode treeNode = new TreeNode();

        public ViewDrawing()
        {
            InitializeComponent();
        }

        private void ViewDrawing_Load(object sender, EventArgs e)
        {
            try
            {
                PopolateTreeView();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }  
        }

        private DataTable GetaDataTable()
        {
            SqLite sqLite = new SqLite();
            string sql = "";
            string mainPath = System.IO.Directory.GetParent
                (
                    System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')).ToString()
                ).ToString();
            string filePathDB = $"{mainPath}\\Data\\ViewDrawingDB.sqlite";
            if (!File.Exists(filePathDB))
            {
                sqLite.CreateDB(filePathDB);
                sqLite.OpenDB(filePathDB);
                sql = System.IO.File.ReadAllText($"{mainPath}\\Data\\ViewDrawingDB.sql");
                try
                {
                    sqLite.ExecuteCommand(sql);
                }
                catch (Exception)
                {
                    throw;
                }
                sqLite.CloseDB();
            }
            sqLite.OpenDB(filePathDB);
            sql = "Select pl.Name ProductLineName , p.Name ProjectName, i.Name ItemName, d.Name DrawingName, d.PathFile from Drawing d left join Item i on d.IDItem = i.ID left join Project p on i.IDProject = p.ID left join ProductLine pl on p.IDProductLine = pl.ID;";
            DataTable queryResult = sqLite.ExecuteSelect(sql);
            sqLite.CloseDB();
            return queryResult;
        }
        
        private void PopolateTreeView()
        {
            DataTable queryResult = GetaDataTable();
            int countColumn = queryResult.Columns.Count;
            TreeNode parentNode = null;
            parentNode = new TreeNode();
            string strNode = String.Empty;
            foreach (DataRow row in queryResult.Rows)
            {
                nLevelNewNode = 0;
                NewNode(ref treeNode, countColumn, row);
            }
            foreach (TreeNode node in treeNode.Nodes)
            {
                treeViewDrawing.Nodes.Add(node);
            }      
        }

        private bool ValidExtensions(string pathFile)
        {
            bool valid = false;
            string[] splitPathFile = pathFile.Split('.');
            if (splitPathFile[splitPathFile.Count() - 1].ToLower() == "pdf") { valid = true; }
            if (splitPathFile[splitPathFile.Count() - 1].ToLower() == "tif") { valid = true; }
            if (splitPathFile[splitPathFile.Count() - 1].ToLower() == "tiff") { valid = true; }
            return valid;
        }

        private void NewNode(ref TreeNode node, int countColumn, DataRow row)
        {
            string strNode = row[nLevelNewNode].ToString();
            string strNodeNext = String.Empty;
            bool validExtensions = false;
            if ((nLevelNewNode + 1) <= countColumn)
            {
                strNodeNext = row[nLevelNewNode+1].ToString();
                validExtensions = ValidExtensions(strNodeNext);
            }
            TreeNode nodeNext = null;
            TreeNode node_tmp = new TreeNode();
            node_tmp.Name = strNode;
            node_tmp.Text = strNode;
            if (validExtensions) { node_tmp.Tag = strNodeNext; }
            if (node != null && node.Nodes.Find(strNode, false).Count() == 0)
            {
                node.Nodes.Add(node_tmp);
            }
            nodeNext = node.Nodes[strNode];
            nLevelNewNode++;
            if (nLevelNewNode < countColumn && !validExtensions)
            {
                NewNode(ref nodeNext, countColumn, row);
            }
        }

        private void treeViewDrawing_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (treeViewDrawing.SelectedNode.Tag != null)
                {
                    string filePath = treeViewDrawing.SelectedNode?.Tag.ToString();
                    if (!File.Exists(filePath)){ throw new Exception($"The file {filePath} not exists"); }
                    ViewPnl.Controls.Clear();
                    if (filePath.ToLower().EndsWith(".pdf") == true)
                    {
                        PdfViewer DrawingPdfBox = new PdfViewer();
                        DrawingPdfBox.Name = "DrawingPdfBox";
                        DrawingPdfBox.Dock = DockStyle.Fill;
                        DrawingPdfBox.LoadDocument(filePath);
                        ViewPnl.Controls.Add(DrawingPdfBox);
                    }
                    else
                    {
                        PictureBox DrawingPBox = new PictureBox();
                        DrawingPBox.Name = "DrawingPBox";
                        DrawingPBox.Image = Image.FromFile(filePath);
                        DrawingPBox.MouseDown += DrawingPBox_MouseDown;
                        DrawingPBox.MouseMove += DrawingPBox_MouseMove;
                        DrawingPBox.MouseUp += DrawingPBox_MouseUp;
                        DrawingPBox.SizeMode = PictureBoxSizeMode.AutoSize; 
                        ViewPnl.Controls.Add(DrawingPBox);
                    }
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void DrawingPBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void DrawingPBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                ViewPnl.Controls["DrawingPBox"].Top = Cursor.Position.Y  - (this.Top + ViewPnl.Top + lastLocation.Y);
                ViewPnl.Controls["DrawingPBox"].Left = Cursor.Position.X - (this.Left + ViewPnl.Left + lastLocation.X);
            }
        }

        private void DrawingPBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            FindNodesByString(treeViewDrawing, SearchTxt.Text);
        }

        private void FindNodesByString(TreeView treeView, string text)
        {
            foreach (TreeNode currentNode in treeView.Nodes)
            {
                FindNodeByString(currentNode, text);
            }
        }

        private void FindNodeByString(TreeNode parentNode, string text)
        {
            FindMatch(parentNode, text);
            foreach (TreeNode currentNode in parentNode.Nodes)
            {
                FindMatch(currentNode, text);
                FindNodeByString(currentNode, text);
            }
        }

        private void FindMatch(TreeNode currentNode, string text)
        {
            if (currentNode.Text.ToUpper().Contains(text.ToUpper()))
            {
                currentNode.Expand();
                currentNode.Checked = true;
                treeViewDrawing.SelectedNode = currentNode;
                treeViewDrawing.Focus();
            }
            else
            {
                currentNode.Collapse();
                currentNode.Checked = false;
            }
        }

        private void ExpandAllBtn_Click(object sender, EventArgs e)
        {
            FindNodesByString(treeViewDrawing, "");
        }

        private void CollapseAllBtn_Click(object sender, EventArgs e)
        {
            FindNodesByString(treeViewDrawing, "***Collapse***");
        }

        private void RestoreImagePositionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ViewPnl.Controls["DrawingPBox"].Top = 0;
                ViewPnl.Controls["DrawingPBox"].Left = 0;
            }
            catch (Exception)
            {
            }
        }
    }
}
