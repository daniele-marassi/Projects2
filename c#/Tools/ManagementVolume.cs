using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Tools
{
    public class ManagementVolume
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public void SetVolume(string param)
        {
           if (param == Hook.keyDown)
                keybd_event((byte)Keys.VolumeDown, 0, 0, 0);

            if (param == Hook.keyUp)
                keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
        }
    }
}
