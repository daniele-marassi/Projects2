using ContextMenu.Properties;
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
    public partial class ContexMenuFrm : Form
    {


        public ContexMenuFrm()
        {
            InitializeComponent();
        }

        private void ContexMenuFrm_Load(object sender, EventArgs e)
        {
            PopulateDataGrid();

            //System.Windows.Forms.ContextMenu cm = new System.Windows.Forms.ContextMenu();
            //cm.MenuItems.Add("Item 1");
            //cm.MenuItems.Add("Item 2");

            //dataGridView1.ContextMenu = cm;

        }

        private void PopulateDataGrid()
        {
            DataTable People = new DataTable("People");
            DataColumn c0 = new DataColumn("Name");
            DataColumn c1 = new DataColumn("Phone");
            DataColumn c2 = new DataColumn("Address");
            People.Columns.Add(c0);
            People.Columns.Add(c1);
            People.Columns.Add(c2);
            DataRow row, row1, row2;
            row = People.NewRow();
            row["Name"] = "Fred";
            row["Phone"] = "555-1234";
            row["Address"] = "Hollywood CA";
            row1 = People.NewRow();
            row1["Name"] = "Bob";
            row1["Phone"] = "555-4567";
            row1["Address"] = "Washington DC";
            row2 = People.NewRow();
            row2["Name"] = "Sam";
            row2["Phone"] = "555-7890";
            row2["Address"] = "Milwaukee WI";
            People.Rows.Add(row);
            People.Rows.Add(row1);
            People.Rows.Add(row2);
            dataGridView1.DataSource = People;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var value = dataGridView1.Rows[e.RowIndex].Cells["Name"].FormattedValue.ToString();
                System.Windows.Forms.ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
                var edit = menu.Items.Add("Edit");
                var close = menu.Items.Add("Close");
                edit.Image = Resources.Edit;
                close.Image = Resources.Close;
                edit.Click += (sender2, e2) => MenuEdit_Click(sender2, e2, value);
                close.Click += new EventHandler(MenuClose_Click);
                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                menu.Show(dataGridView1, new Point(e.X, e.Y));
            }
        }

        void MenuEdit_Click(object sender, EventArgs e, string value)
        {
            ToolStripItem clickedItem = sender as ToolStripItem;
            using (Modalfrm modal = new Modalfrm())
            {
                modal.OpenModalFrm(this, value);
            } 
        }

        void MenuClose_Click(object sender, EventArgs e)
        {
            ToolStripItem clickedItem = sender as ToolStripItem;
        }
    }
}
