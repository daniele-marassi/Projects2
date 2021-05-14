using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Additional;
using static Additional.ScrollBar;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Utility.mainForm = this;
            Additional.ScrollBar scrollBar1 = new Additional.ScrollBar();
            Additional.ScrollBar scrollBar2 = new Additional.ScrollBar();

            Color foreColor = Color.FromArgb(255,240, 240, 240);

            Color bgColor = Color.FromArgb(255, 80, 80, 80);
            Utility utility = new Utility();


            ScrollBarParameters parametersScrollBar1 = new ScrollBarParameters { form = this, container = panel1, index = 0, percentageValue = 100, top = 0, left = 0, width = 150, height = 20, color = bgColor, foreColor = foreColor, type = ScrollBarType.Incremental, classFullPathofMethod = utility.GetFullPathClass(this), MethodNameOnChange = "ScrollBar1_OnChange", MethodNameOnLeave = "" };
            ScrollBarParameters parametersScrollBar2 = new ScrollBarParameters { form = this, container = panel2, index = 0, percentageValue = 255, top = 0, left = 0, width = 20, height = 295, color = bgColor, foreColor = foreColor, type = ScrollBarType.Directional, classFullPathofMethod = utility.GetFullPathClass(this), MethodNameOnChange = "ScrollBar2_OnChange", MethodNameOnLeave = "" };


            scrollBar1.CreateHorizontal(parametersScrollBar1);

            scrollBar1.SetValue(this, 0, 100, ScrollBarOrientation.Horizontal);

            scrollBar2.CreateVertical(parametersScrollBar2);
            scrollBar2.SetValue(this, 0, 57, ScrollBarOrientation.Vertical);

            var ttt = scrollBar1.GetValue(this, 0, ScrollBarOrientation.Horizontal);
        }

        public void ScrollBar1_OnChange(string stringValue)
        {
            Utility.mainForm.Controls["textBox1"].Text = stringValue;
        }
        public void ScrollBar2_OnChange(string stringValue)
        {
            Utility.mainForm.Controls["textBox1"].Text = stringValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Demo demo = new Demo();
            demo.Volume(this);
        }
    }
}
