using Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Additional.ScrollBar;

namespace MediaBox
{
    public static class Services
    {
        private static int old_valueColorRed = 0;
        private static int old_valueColorGreen = 0;
        private static int old_valueColorBlue = 0;
        private static int old_valueAudio = 0;
        private static int old_valueMediaMovementSpeed = 0;
        

        public static bool active = true;

        public static void StartServices(Form form)
        {
            
            while (active)
            {

               
                try
                {
                    //Application.DoEvents();
                    ServiceAudio(form);
                    SetPersonalizeColor(form);
                    ServiceMediaMovementSpeed(form);
                    //Utility utility = new Utility();
                    //utility.Sleep(1);
                    Sleep(50);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //   throw;
                }
            }
        }

        public static bool Sleep(int milliseconds)
        {
            DateTime endTime = DateTime.Now;
            endTime = endTime.AddMilliseconds(milliseconds);
            while (DateTime.Now < endTime)
            {
                try
                {
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return true;
        }

        private static void ServiceAudio(Form form)
        {
            int valueAudio = MediaBox.scrollBarAudio.GetValue(form, "Audio",ScrollBarOrientation.Horizontal);
            if (valueAudio != old_valueAudio)
            {
                MediaBox.SetVolumeAudioPlayer(true, valueAudio);
                old_valueAudio = valueAudio;
            }
        }

        public static void SetPersonalizeColor(Form form)
        {
            int valueColorRed = MediaBox.scrollBarPersonalizeColorRed.GetValue(form, "PersonalizeColorRed", ScrollBarOrientation.Vertical);
            int valueColorGreen = MediaBox.scrollBarPersonalizeColorGreen.GetValue(form, "PersonalizeColorGreen", ScrollBarOrientation.Vertical);
            int valueColorBlue = MediaBox.scrollBarPersonalizeColorBlue.GetValue(form, "PersonalizeColorBlue", ScrollBarOrientation.Vertical);
            if (valueColorRed != old_valueColorRed || valueColorGreen != old_valueColorGreen || valueColorBlue != old_valueColorBlue)
            {
                MediaBox.SetCurrentPersonalizeColor(valueColorRed, valueColorGreen, valueColorBlue);
                old_valueColorRed = valueColorRed;
                old_valueColorGreen = valueColorGreen;
                old_valueColorBlue = valueColorBlue;
            }
        }

        public static void ServiceMediaMovementSpeed(Form form)
        {
            int valueMediaMovementSpeed = MediaBox.scrollBarMediaMovementSpeed.GetValue(form, "MediaMovementSpeed", ScrollBarOrientation.Horizontal);
            if (valueMediaMovementSpeed != old_valueMediaMovementSpeed)
            {
                Statics.FormParam.MediaMovementSpeed = valueMediaMovementSpeed;
                old_valueMediaMovementSpeed = valueMediaMovementSpeed;
            }
        }
    }
}
