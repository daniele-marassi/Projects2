using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bentley.MicroStation.InteropServices;
using Bentley.MicroStation.WinForms;

namespace csAddinsTest
{
    public partial class ToolbarForm : //Form
                                       Adapter, IGuiDockable
    {
        private Bentley.Interop.MicroStationDGN.Application app = null;

        public ToolbarForm()
        {
            InitializeComponent();
            app = Utilities.ComApp;
        }

        private void btnCreateElement_Click(object sender, EventArgs e)
        {
            app.CadInputQueue.SendKeyin("csAddinsTest CreateElement Mesh");
            app.CadInputQueue.SendKeyin("csAddinsTest CreateElement ShapeAndComplexShape");
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            app.CadInputQueue.SendKeyin("csAddinsTest LoadForms ToolSettings");
        }

        public bool GetDockedExtent(GuiDockPosition dockPosition, ref GuiDockExtent extentFlag, ref Size dockSize)
        {
            return false;
        }

        public bool WindowMoving(WindowMovingCorner corner, ref Size newSize)
        {
            newSize = new System.Drawing.Size(118, 34);
            return true;
        }

        private void btnLevelChanged_Click(object sender, EventArgs e)
        {
            app.CadInputQueue.SendKeyin("csAddinsTest LoadForms LevelChanged");
        }

        private void btnModelFromCurrent_Click(object sender, EventArgs e)
        {
            app.CadInputQueue.SendKeyin("csAddinsTest ScanElement Utility");
        }
    }
}
