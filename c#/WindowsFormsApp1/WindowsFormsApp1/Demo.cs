using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Additional;
using static Additional.ScrollBar;

namespace WindowsFormsApp1
{
    public class Demo
    {
        Emulation.MousePointer emulationMousePointer = null;
        public Demo()
        {
            emulationMousePointer = new Emulation.MousePointer(30);
            emulationMousePointer.Show();
        }

        public void Volume_old(Form form)
        {

            Utility utility = new Utility();

            utility.MoveMouseToControl(form, "ScrollBarHorizontalCursorBtn_0");

            VirtualMouse.LeftDown();
            VirtualMouse.LeftDown();

            for (int i = 0; i < 50; i++)
            {
                Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
                System.Threading.Thread.Sleep(5);
            }

            VirtualMouse.LeftUp();
        }

        public void Volume(Form form)
        {

            Utility utility = new Utility();
            Additional.ScrollBar scrollBar1 = new Additional.ScrollBar();

            emulationMousePointer.MoveToControl(form, "button2");
            System.Threading.Thread.Sleep(3000);

            emulationMousePointer.MoveToControl(form, "ScrollBarHorizontalCursorBtn_0");

            emulationMousePointer.Focus();
            Control ScrollBarHorizontalCursorBtn_0 = utility.GetControlByChildName(form, "ScrollBarHorizontalCursorBtn_0");
            emulationMousePointer.ClickDown(true);
            for (int i = 0; i < 50; i++)
            {
                ScrollBarHorizontalCursorBtn_0.Location = new Point(ScrollBarHorizontalCursorBtn_0.Location.X - 1, ScrollBarHorizontalCursorBtn_0.Location.Y);
                emulationMousePointer.MoveToLeft(-1);
                emulationMousePointer.ShowTooltip("aaaaaaaaaaa ggggggggggggg    bbbbbbbbbbbb");
                emulationMousePointer.MoveTooltip();
                System.Threading.Thread.Sleep(25);
            }

            emulationMousePointer.ClickUp(true);
            form.Focus();
            scrollBar1.SetValue(form, 0, scrollBar1.GetValue(form,0, ScrollBarOrientation.Horizontal), ScrollBarOrientation.Horizontal);
            emulationMousePointer.Focus();

            emulationMousePointer.MoveToControl(form, "ScrollBarHorizontalPlusBtn_0");
            
            emulationMousePointer.Focus();
            Button ScrollBarHorizontalPlusBtn_0 = (Button) utility.GetControlByChildName(form, "ScrollBarHorizontalPlusBtn_0");

            System.Threading.Thread.Sleep(500);
            emulationMousePointer.ClickDown(true);

            ScrollBarHorizontalPlusBtn_0.PerformClick();
            emulationMousePointer.ClickUp(true);
            System.Threading.Thread.Sleep(5);
            emulationMousePointer.ClickDown(true);
            ScrollBarHorizontalPlusBtn_0.PerformClick();
            emulationMousePointer.ClickUp(true);

        }
    }
}
