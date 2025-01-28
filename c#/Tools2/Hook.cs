using System;
using System.Collections.Generic;
using Additional;
using System.Reflection;
using System.Windows.Forms;

namespace Tools
{
    public class Hook
    {

        public static bool controlKeyDown = false;
        public static string controlKey = null;
        public static string keyUp = null;
        public static string keyDown = null;
        public static string methodOfUse = null;
        public static void Start()
        {
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;
            //HookManager.MouseDown += HookManager_MouseDown;
        }

        public static void Stop()
        {
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.KeyUp -= HookManager_KeyUp;
            //HookManager.MouseDown -= HookManager_MouseDown;
        }

        private static void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            Stop();
            Console.WriteLine("MouseDown");
            //Utility utility = new Utility();

            //TODO: azione
            Start();
        }

        private static void HookManager_KeyUp(object sender, EventArgs e)
        {
            Stop();

            Type type = e.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            string keyCode = String.Empty;
            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                if (propName == "KeyCode")
                {
                    object propValue = prop.GetValue(e, null);
                    keyCode = propValue.ToString();
                    break;
                }
            }

            //Console.WriteLine(string.Format("KeyUp - {0}\n", keyCode));

            if (controlKey.Trim().ToLower().Contains(keyCode.Trim().ToLower())) controlKeyDown = false;

            Start();
        }


        private static void HookManager_KeyDown(object sender, EventArgs e)
        {
            Stop();

            Type type = e.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            string keyCode = String.Empty;
            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                if (propName == "KeyCode")
                {
                    object propValue = prop.GetValue(e, null);
                    keyCode = propValue.ToString();
                    break;
                }
            }

            //Console.WriteLine(string.Format("KeyDown - {0}\n", keyCode));

            if (controlKey.Trim().ToLower().Contains(keyCode.Trim().ToLower())) controlKeyDown = true;

            if ((controlKeyDown == true || controlKey == String.Empty) && (keyCode == keyUp || keyCode == keyDown))
            {
                Utility utility = new Utility();

                utility.CallMethodByName("Tools.ManagementVolume", "SetVolume", keyCode);
            }

            Start();
        }
    }
}
