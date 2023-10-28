using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shut
{
    public static class ThreadHelperClass
    {
        delegate void SetTopCallback(Form f, int top);
        delegate void SetLeftCallback(Form f, int left);

        /// <summary>
        /// Set Top of form
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="top"></param>
        public static void SetTop(Form form, int top)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (form.InvokeRequired)
            {
                SetTopCallback d = new SetTopCallback(SetTop);
                form.Invoke(d, new object[] { form, top });
            }
            else
            {
                form.Top = top;
            }
        }

        /// <summary>
        /// Set Left of form
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="left"></param>
        public static void SetLeft(Form form, int left)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (form.InvokeRequired)
            {
                SetLeftCallback d = new SetLeftCallback(SetLeft);
                form.Invoke(d, new object[] { form, left });
            }
            else
            {
                form.Left = left;
            }
        }
    }
}
