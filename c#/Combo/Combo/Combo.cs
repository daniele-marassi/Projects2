using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Combo
{
    public partial class Combo : Form
    {
        public Combo()
        {
            InitializeComponent();
        }

        private void ElaborateBtn_Click(object sender, EventArgs e)
        {
            var result = Elaborate(ValueTxt.Text);

            foreach (var item in result)
            {
                ResultLb.Items.Add(item);
            }
        }

        private List<string> Elaborate(string value)
        {
            var result = new List<string>() { };

            var unitCount = value.Length;



            var CombinateCount = 0;
           

            for (int i = 0; i < unitCount; i++)
            {
                unitCount - i;



            }


            return result;
        }
    }
}
