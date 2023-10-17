using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

using NAudio.CoreAudioApi;
using Microsoft.Win32;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Security.Authentication;

namespace Additional
{
    public class Utility
    {
        public static Form mainForm { get; set; }

        /// <summary>
        /// Method class
        /// </summary>
        public class Method
        {
            public string Name { get; set; }
            public string ParentPath { get; set; }
        }

        // Define the FindWindow API function.
        [DllImport("user32.dll", EntryPoint = "FindWindow",
            SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly,
            string lpWindowName);

        // Define the SetWindowPos API function.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            SetWindowPosFlags uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("User32.dll")]
        private static extern IntPtr SetFocus(HandleRef hWnd);

        // Define the SetWindowPosFlags enumeration.
        [Flags()]
        private enum SetWindowPosFlags : uint
        {
            SynchronousWindowPosition = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = 0x0020,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            DoNotActivate = 0x0010,
            DoNotCopyBits = 0x0100,
            IgnoreMove = 0x0002,
            DoNotChangeOwnerZOrder = 0x0200,
            DoNotRedraw = 0x0008,
            DoNotReposition = 0x0200,
            DoNotSendChangingEvent = 0x0400,
            IgnoreResize = 0x0001,
            IgnoreZOrder = 0x0004,
            ShowWindow = 0x0040,
        }

        /// <summary>
        /// Set Registry Key
        /// </summary>
        /// <param name="root"></param>
        /// <param name="KeyPath"></param>
        /// <param name="valueName"></param>
        /// <param name="valueData"></param>
        /// <param name="registryValueKind"></param>
        /// <param name="registryType"></param>
        public void SetRegistryKey(RegistryHive root, string KeyPath, string valueName, object valueData, RegistryValueKind registryValueKind, RegistryView registryType)
        {
            var regKeySpecific = RegistryKey.OpenBaseKey(root, registryType);
            var registryKey = regKeySpecific.OpenSubKey(KeyPath, true);
            registryKey.SetValue(valueName, valueData, registryValueKind);
            registryKey.Close();
        }

        /// <summary>
        /// Get Registry Key
        /// </summary>
        /// <param name="root"></param>
        /// <param name="KeyPath"></param>
        /// <param name="valueName"></param>
        /// <param name="registryType"></param>
        /// <returns></returns>
        public object GetRegistryKey(RegistryHive root, string KeyPath, string valueName, RegistryView registryType)
        {
            object result;

            var regKeySpecific = RegistryKey.OpenBaseKey(root, registryType);
            var registryKey = regKeySpecific.OpenSubKey(KeyPath, true);
            result = registryKey.GetValue(valueName);
            registryKey.Close();

            return result;
        }

        /// <summary>
        /// Reconnect Bluetooth Device
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="password"></param>
        /// <param name="removeDevice"></param>
        /// <param name="forceReset"></param>
        /// <returns></returns>
        public (string Message, bool Successful, bool PairAlreadyExists) ReconnectBluetoothDevice(string deviceName, string password, bool removeDevice, bool forceReset = false)
        {
            (string Message, bool Successful, bool PairAlreadyExists) result;
            result.Message = "";
            result.Successful = false;
            result.PairAlreadyExists = false;

            var bluetoothPairedDeviceInfo = GetBluetoothDeviceByName(GetBluetoothPairedDevices(), deviceName);
            var bluetoothDiscoverDeviceInfo = GetBluetoothDeviceByName(GetBluetoothDiscoverDevices(), deviceName);

            var defaultAudioDeviceName = GetDefaultAudioDeviceName();

            if (bluetoothPairedDeviceInfo == null | bluetoothDiscoverDeviceInfo != null || defaultAudioDeviceName.ToLower().Contains(deviceName.ToLower()) == false || forceReset)
            {
                SetRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\PolicyManager\\default\\Connectivity\\AllowBluetooth", "value", 0, RegistryValueKind.DWord, RegistryView.Registry64);

                System.Threading.Thread.Sleep(1000);

                SetRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\PolicyManager\\default\\Connectivity\\AllowBluetooth", "value", 2, RegistryValueKind.DWord, RegistryView.Registry64);

                System.Threading.Thread.Sleep(1000);

                bluetoothDiscoverDeviceInfo = GetBluetoothDeviceByName(GetBluetoothDiscoverDevices(), deviceName);

                if (bluetoothDiscoverDeviceInfo != null)
                {
                    var connectResult = ConnectBluetoothSpeakers(bluetoothDiscoverDeviceInfo?.DeviceAddress, password, removeDevice);

                    result.Message = connectResult.Message;
                    result.Successful = connectResult.Successful;
                    result.PairAlreadyExists = false;
                }
                else
                {
                    result.Message = $"Device {deviceName} not found!";
                    result.Successful = false;
                    result.PairAlreadyExists = false;
                }
            }
            else
            {
                result.Message = "The device is already paired.";
                result.Successful = true;
                result.PairAlreadyExists = true;
            }

            return result;
        }

        /// <summary>
        /// Get Bluetooth Discover Devices
        /// </summary>
        /// <returns></returns>
        public List<BluetoothDeviceInfo> GetBluetoothDiscoverDevices()
        {
            var result = new List<BluetoothDeviceInfo>() { };

            BluetoothClient bluetoothClient = null;

            try
            {
                bluetoothClient = new BluetoothClient();
                var buleRadio = BluetoothRadio.Default;//PrimaryRadio;
                buleRadio.Mode = RadioMode.Connectable; //.Connectable;
                var devices = bluetoothClient.DiscoverDevices();

                var _devices = bluetoothClient.PairedDevices.ToList();

                result.AddRange(devices);
            }
            catch (Exception)
            {
                try
                {
                    bluetoothClient.Close();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Get Bluetooth Paired Devices
        /// </summary>
        /// <returns></returns>
        public List<BluetoothDeviceInfo> GetBluetoothPairedDevices()
        {
            var result = new List<BluetoothDeviceInfo>() { };

            BluetoothClient bluetoothClient = null;

            try
            {
                bluetoothClient = new BluetoothClient();
                var buleRadio = BluetoothRadio.Default;//PrimaryRadio;
                buleRadio.Mode = RadioMode.Connectable; //.Connectable;
                var devices = bluetoothClient.PairedDevices.ToList();

                result.AddRange(devices);
            }
            catch (Exception)
            {
                try
                {
                    bluetoothClient.Close();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Get Bluetooth Device By Name
        /// </summary>
        /// <param name="bluetoothDevices"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public BluetoothDeviceInfo GetBluetoothDeviceByName(List<BluetoothDeviceInfo> bluetoothDevices, string deviceName)
        {
            BluetoothDeviceInfo result;

            result = bluetoothDevices.Where(_ => _.DeviceName.Trim().ToLower() == deviceName.Trim().ToLower()).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Connect Bluetooth Speakers
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public (string Message, bool Successful) ConnectBluetoothSpeakers(BluetoothAddress bluetoothAddress, string password, bool removeDevice)
        {
            (string Message, bool Successful) result;
            result.Message = "";
            result.Successful = false;

            BluetoothClient bluetoothClient = null;

            try
            {
                if (removeDevice) RemoveBluetoothDevice(bluetoothAddress);
                var device = new BluetoothDeviceInfo(bluetoothAddress);
                BluetoothSecurity.PairRequest(bluetoothAddress, password);
                bluetoothClient = new BluetoothClient();
                bluetoothClient.Connect(device.DeviceAddress, BluetoothService.Handsfree);

                if (bluetoothClient.Connected)
                {
                    result.Message = "The pairing is successful.";
                    result.Successful = true;
                }
                else
                {
                    result.Message = "The pairing is failed.";
                    result.Successful = false;
                }

                var stream = bluetoothClient.GetStream();
                stream.Close();

                bluetoothClient.Close();
                device.SetServiceState(BluetoothService.Handsfree, true);
                device.SetServiceState(BluetoothService.AudioSink, true);
                device.SetServiceState(BluetoothService.AVRemoteControl, true);
                device.SetServiceState(BluetoothService.GenericAudio, true);
                device.SetServiceState(BluetoothService.AudioVideo, true);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Successful = false;

                try
                {
                    bluetoothClient.Close();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Remove Bluetooth Device
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <returns></returns>
        public void RemoveBluetoothDevice(BluetoothAddress bluetoothAddress)
        {
            BluetoothSecurity.RemoveDevice(bluetoothAddress);
        }

        /// <summary>
        /// SetVolume
        /// </summary>
        /// <param name="percent"></param>
        public void SetVolume(double percent)
        {
            var defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = percent;
        }

        /// <summary>
        /// GetDefaultAudioDeviceName
        /// </summary>
        /// <returns></returns>
        public string GetDefaultAudioDeviceName()
        {
            var defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            return defaultPlaybackDevice.FullName;
        }

        /// <summary>
        /// Get FullPath Class
        /// </summary>
        /// <param name="_class"></param>
        /// <returns></returns>
        public string GetFullPathClass(object _class)
        {
            string pathClass = String.Empty;
            if (_class.GetType().Assembly.FullName == this.GetType().Assembly.FullName)
            { pathClass = _class.GetType().Namespace + "." + _class.GetType().Name; }
            else { pathClass = _class.GetType().Namespace + "." + _class.GetType().Name + ", " + _class.GetType().Assembly.FullName; }

            return pathClass;
        }

        /// <summary>
        /// Valid Value
        /// </summary>
        /// <param name="istanceModel"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ValidValue(dynamic istanceModel, string value)
        {
            bool valid = false;
            try
            {
                foreach (var prop in istanceModel.GetType().GetProperties())
                {
                    if (prop.GetValue(istanceModel, null).ToString().ToLower() == value.ToLower())
                    { valid = true; break; }
                }
            }
            catch (Exception)
            { }
            return valid;
        }

        /// <summary>
        /// Get CurrentMethod
        /// </summary>
        /// <returns></returns>
        public Method GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            var method = sf.GetMethod();
            return new Method { Name = method.Name, ParentPath = method.DeclaringType.FullName };
        }

        /// <summary>
        /// Get Fonts
        /// </summary>
        /// <param name="nameFont"></param>
        /// <param name="sizeFont"></param>
        /// <param name="fonts"></param>
        /// <param name="defaultScaleFactor"></param>
        /// <returns></returns>
        public Font GetFonts(string nameFont, float sizeFont, PrivateFontCollection fonts, float defaultScaleFactor = 96f)
        {
            Font font = default(Font);
            try
            {
                FontFamily fontFamily = default(FontFamily);
                var result = fonts.Families.Where(_ => _.Name == nameFont).FirstOrDefault();
                if (result != null) { fontFamily = result; }

                float scaleFactor = GetScaleFactor(defaultScaleFactor);

                if (scaleFactor > 100)
                {
                    sizeFont -= ((((scaleFactor - 100) - 4) * sizeFont) / 100);
                }

                if (scaleFactor < 100)
                {
                    sizeFont += (sizeFont - (((scaleFactor - 4) * sizeFont) / 100));
                }

                font = new Font(fontFamily, sizeFont);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return font;
        }

        /// <summary>
        /// Get Scale Factor
        /// </summary>
        /// <param name="defaultScaleFactor"></param>
        /// <returns></returns>
        public float GetScaleFactor(float defaultScaleFactor)
        {
            float result = 100f;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                result = (dpiX / defaultScaleFactor) * 100;
            }
            return result;
        }

        /// <summary>
        /// Get Position In Form
        /// </summary>
        /// <param name="controlFather"></param>
        /// <returns></returns>
        public Point GetPositionInForm(Control controlFather)
        {
            Point p = controlFather.Location;
            Control parent = controlFather.Parent;
            while (!(parent is Form))
            {
                p.Offset(parent.Location.X, parent.Location.Y);
                parent = parent.Parent;
            }
            return p;
        }

        /// <summary>
        /// Get Position Absolute
        /// </summary>
        /// <param name="form"></param>
        /// <param name="controlFather"></param>
        /// <returns></returns>
        public Point GetPositionAbsolute(Form form, Control controlFather)
        {
            Point p = controlFather.Location;
            Control parent = controlFather.Parent;
            int HeightBorderForm = form.Height - form.ClientRectangle.Height;
            int WidthBorderForm = form.Width - form.ClientRectangle.Width;

            while (!(parent is Form))
            {
                p.Offset(parent.Location.X, parent.Location.Y);
                parent = parent.Parent;
            }
            p.X = (p.X + form.Location.X + WidthBorderForm);
            p.Y = (p.Y + form.Location.Y + HeightBorderForm);
            return p;
        }

        /// <summary>
        /// Create Png From Control
        /// </summary>
        /// <param name="form"></param>
        /// <param name="control"></param>
        /// <param name="destinationPath"></param>
        public void CreatePngFromControl(Form form, Control control, string destinationPath)
        {
            Application.DoEvents();
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(control.Width,
                                           control.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);



            Point point = GetPositionAbsolute(form, control);

            Size size = new Size(control.Width, control.Height);
            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(point.X,
                                        point.Y,
                                        0,
                                        0,
                                        size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(destinationPath, ImageFormat.Png);
        }

        /// <summary>
        /// Convert ARGB to RGB
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Color ConvertARGBtoRGB(Color color)
        {
            if (color.A == 255)
            {
                return Color.FromArgb(
                        255,
                        color.R,
                        color.G,
                        color.B
                );
            }
            int newRed = 0;
            int newGreen = 0;
            int newBlue = 0;
            int diffAlpha = 255 - color.A;
            newRed = color.R + diffAlpha;
            if (newRed > 255) { newRed = 255 - (newRed - 255); }
            newGreen = color.G + diffAlpha;
            if (newGreen > 255) { newGreen = 255 - (newGreen - 255); }
            newBlue = color.B + diffAlpha;
            if (newBlue > 255) { newBlue = 255 - (newBlue - 255); }
            return Color.FromArgb(
                    255,
                    newRed,
                    newGreen,
                    newBlue
            );
        }

        /// <summary>
        /// Sleep
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool Sleep(int milliseconds)
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
                }
                
            }
            return true;
        }

        /// <summary>
        /// Call Method By Name
        /// </summary>
        /// <param name="project_Domain_Class"></param>
        /// <param name="methodName"></param>
        /// <param name="stringParam"></param>
        /// <returns></returns>
        public bool CallMethodByName(string project_Domain_Class, string methodName, string stringParam)
        {
            bool result = false;
            try
            {
                Type type = Type.GetType(project_Domain_Class);
                if (type == null)
                {
                    string pathDLL = System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase.ToString();
                    var assembly = Assembly.LoadFrom(pathDLL);
                    type = assembly.GetType(project_Domain_Class);
                }

                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                object ClassObject = constructor.Invoke(new object[] { });
                MethodInfo method = type.GetMethod(methodName);
                object Value = method.Invoke(ClassObject, new object[] { stringParam });
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Is Image
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public bool IsImage(string extension)
        {
            bool ret = false;
            extension = extension.ToLower();
            if (extension == ".bmp") { ret = true; }
            if (extension == ".cdp") { ret = true; }
            if (extension == ".djvu") { ret = true; }
            if (extension == ".djv") { ret = true; }
            if (extension == ".eps") { ret = true; }
            if (extension == ".gif") { ret = true; }
            if (extension == ".gpd") { ret = true; }
            if (extension == ".jpd") { ret = true; }
            if (extension == ".jpg") { ret = true; }
            if (extension == ".jpeg") { ret = true; }
            if (extension == ".pict") { ret = true; }
            if (extension == ".png") { ret = true; }
            if (extension == ".tga") { ret = true; }
            if (extension == ".tiff") { ret = true; }
            if (extension == ".tif") { ret = true; }
            if (extension == ".pcx") { ret = true; }
            if (extension == ".psd") { ret = true; }
            if (extension == ".webp") { ret = true; }
            return ret;
        }

        /// <summary>
        /// Is Video
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public bool IsVideo(string extension)
        {
            bool ret = false;
            extension = extension.ToLower();
            if (extension == ".3gp") { ret = true; }
            if (extension == ".asf") { ret = true; }
            if (extension == ".avi") { ret = true; }
            if (extension == ".divx") { ret = true; }
            if (extension == ".flv") { ret = true; }
            if (extension == ".swf") { ret = true; }
            if (extension == ".mpeg") { ret = true; }
            if (extension == ".ogm") { ret = true; }
            if (extension == ".wmv") { ret = true; }
            if (extension == ".mov") { ret = true; }
            if (extension == ".mkv") { ret = true; }
            if (extension == ".nbr") { ret = true; }
            if (extension == ".rm") { ret = true; }
            if (extension == ".vob") { ret = true; }
            if (extension == ".sfd") { ret = true; }
            if (extension == ".webm") { ret = true; }
            if (extension == ".mp4") { ret = true; }
            return ret;
        }

        /// <summary>
        /// Is Audio
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public bool IsAudio(string extension)
        {
            bool ret = false;
            extension = extension.ToLower();
            if (extension == ".aac") { ret = true; }
            if (extension == "ac3") { ret = true; }
            if (extension == ".aiff") { ret = true; }
            if (extension == ".ape") { ret = true; }
            if (extension == ".amr") { ret = true; }
            if (extension == ".bmf") { ret = true; }
            if (extension == ".cda") { ret = true; }
            if (extension == ".dts") { ret = true; }
            if (extension == ".flac") { ret = true; }
            if (extension == ".tracker") { ret = true; }
            if (extension == ".iff") { ret = true; }
            if (extension == ".midi") { ret = true; }
            if (extension == ".mid") { ret = true; }
            if (extension == ".mka") { ret = true; }
            if (extension == ".mod") { ret = true; }
            if (extension == ".mp1") { ret = true; }
            if (extension == ".mp2") { ret = true; }
            if (extension == ".mp3") { ret = true; }
            if (extension == ".m4a") { ret = true; }
            if (extension == ".pca") { ret = true; }
            if (extension == ".ra") { ret = true; }
            if (extension == ".rm") { ret = true; }
            if (extension == ".s3m") { ret = true; }
            if (extension == ".sfa") { ret = true; }
            if (extension == ".w64") { ret = true; }
            if (extension == ".ogg") { ret = true; }
            if (extension == ".opus") { ret = true; }
            if (extension == ".wav") { ret = true; }
            if (extension == ".wma") { ret = true; }
            if (extension == ".xm") { ret = true; }
            if (extension == ".tkkdl") { ret = true; }
            return ret;
        }

        /// <summary>
        /// Get Property
        /// </summary>
        /// <param name="typeofModel"></param>
        /// <returns></returns>
        public List<string> GetProperty(Type typeofModel)
        {
            List<string> props = new List<string>() { };

            foreach (var prop in typeofModel.GetProperties())
            {
                props.Add(prop.Name);
            }
            return props;
        }

        /// <summary>
        /// Bitmap To Byte Array
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public Byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Byte Array To Image
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            System.Drawing.Image returnImage = null;
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                returnImage = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception)
            {
                throw;
            }
            return returnImage;
        }

        /// <summary>
        /// Maximum Common Divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int MaximumCommonDivisor(int a, int b)
        {
            int remnant;
            while (b != 0)
            {
                remnant = a % b;
                a = b;
                b = remnant;
            }
            return a;
        }

        /// <summary>
        /// Change Stauration To Bitmap
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public Bitmap ChangeStaurationToBitmap(Bitmap bitmapSource, int percent)
        {
            float rwgt = 0.3086f;
            float gwgt = 0.6094f;
            float bwgt = 0.0820f;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMatrix colorMatrix = new ColorMatrix();
            float saturation = 1.0f;
            saturation = 1f - ((100f - (float)percent) / 100f);

            float baseSat = 1.0f - saturation;

            colorMatrix[0, 0] = baseSat * rwgt + saturation;
            colorMatrix[0, 1] = baseSat * rwgt;
            colorMatrix[0, 2] = baseSat * rwgt;
            colorMatrix[1, 0] = baseSat * gwgt;
            colorMatrix[1, 1] = baseSat * gwgt + saturation;
            colorMatrix[1, 2] = baseSat * gwgt;
            colorMatrix[2, 0] = baseSat * bwgt;
            colorMatrix[2, 1] = baseSat * bwgt;
            colorMatrix[2, 2] = baseSat * bwgt + saturation;

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Bitmap newBitmap = new Bitmap(bitmapSource.Width, bitmapSource.Height);
            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage
                (
                   bitmapSource,
                   new Rectangle(0, 0, bitmapSource.Width, bitmapSource.Height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   bitmapSource.Width,       // width of source rectangle
                   bitmapSource.Height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes
                );
            }

            return newBitmap;
        }

        /// <summary>
        /// Maximize Or Default
        /// </summary>
        /// <param name="form"></param>
        public void MaximizeOrDefault(Form form)
        {
            int widthScreen = Screen.GetWorkingArea(form).Width;
            int heightScreen = Screen.GetWorkingArea(form).Height;
            if (form.Width != widthScreen && form.Height != heightScreen)
            {
                if (form.Top < -(form.Height / 2)) { form.Top = -Screen.GetBounds(form).Height; }
                else { form.Top = 0; }
                if (form.Left < -(form.Width / 2)) { form.Left = -Screen.GetBounds(form).Width; }
                else { form.Left = 0; }
                form.Height = heightScreen;
                form.Width = widthScreen;
            }
            else
            {
                if (form.Top < -(form.Height / 2)) { form.Top = ((heightScreen / 3) / 2) - heightScreen; }
                else { form.Top = ((heightScreen / 3) / 2); }
                if (form.Left < -(form.Width / 2)) { form.Left = ((widthScreen / 3) / 2) - widthScreen; }
                else { form.Left = ((widthScreen / 3) / 2); }
                form.Height = Convert.ToInt32((heightScreen / 3) * 2);
                form.Width = Convert.ToInt32((widthScreen / 3) * 2); ;
            }
        }

        /// <summary>
        /// Get Center Location
        /// </summary>
        /// <param name="formWidth"></param>
        /// <param name="formHeight"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetCenterLocation(int formWidth, int formHeight, int width, int height, int top, int left, int percent)
        {
            Dictionary<string, int> location = new Dictionary<string, int>() { };
            double newLeft = (((double)formWidth - (double)width) / 2d);
            double newTop = (((double)formHeight - (double)height) / 2d);

            if (newLeft < 0) { newLeft = 0; }
            if (newTop < 0) { newTop = 0; }

            int difference = 0;

            if (top > newTop)
            {
                difference = Convert.ToInt32(((double)(top - newTop) / 100d) * (double)percent);
                location["Top"] = Math.Abs(top - difference);
            }
            else
            {
                difference = Convert.ToInt32(((double)(newTop - top) / 100d) * (double)percent);
                location["Top"] = Math.Abs(top + difference);
            }

            if (left > newLeft)
            {
                difference = Convert.ToInt32(((double)(left - newLeft) / 100d) * (double)percent);
                location["Left"] = Math.Abs(left - difference);
            }
            else
            {
                difference = Convert.ToInt32(((double)(newLeft - left) / 100d) * (double)percent);
                location["Left"] = Math.Abs(left + difference);
            }
            return location;
        }

        public double DiffInMillisecondsBetweenTwoDate(DateTimeOffset time1, DateTimeOffset time2)
        {
            double milliseconds = 0;
            if (time1 > time2)
            { milliseconds = (time1 - time2).TotalMilliseconds; }
            else { milliseconds = (time2 - time1).TotalMilliseconds; }
            return milliseconds;
        }

        /// <summary>
        /// Get Control By ChildName
        /// </summary>
        /// <param name="controlFather"></param>
        /// <param name="controlChildName"></param>
        /// <returns></returns>
        public Control GetControlByChildName(Control controlFather, string controlChildName)
        {
            Control control = new Control();
            foreach (Control x in controlFather.Controls)
            {
                if (x.Name.Contains(controlChildName))
                    return control = x;
                if (x.Controls.Count > 0)
                {
                    control = GetControlByChildName(x, controlChildName);
                    if (control.Name != "")
                        return control;
                }
                if (control.Name != "")
                    return control;
            }
            return control;
        }

        /// <summary>
        /// Move Mouse To Control
        /// </summary>
        /// <param name="form"></param>
        /// <param name="controlName"></param>
        public void MoveMouseToControl(Form form, string controlName)
        {
            Utility utility = new Utility();

            Control control;
            Point positionControl;
            control = utility.GetControlByChildName(form, controlName);
            positionControl = utility.GetPositionInForm(control);


            int HeightBorderForm = form.Height - form.ClientRectangle.Height;
            int WidthBorderForm = form.Width - form.ClientRectangle.Width;

            Point newPosition = new Point(positionControl.X + form.Location.X + WidthBorderForm, positionControl.Y + form.Location.Y + HeightBorderForm);
            int x = 0;
            int y = 0;

            bool mousePositionatedX = false;
            bool mousePositionatedY = false;

            while (mousePositionatedX == false || mousePositionatedY == false)
            {
                Application.DoEvents();

                if (mousePositionatedX == false)
                {
                    if (Cursor.Position.X <= newPosition.X) { x = 1; } else { x = -1; }
                    Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y);
                    if (Cursor.Position.X == newPosition.X) { mousePositionatedX = true; }
                }

                if (mousePositionatedY == false)
                {
                    if (Cursor.Position.Y <= newPosition.Y) { y = 1; } else { y = -1; }
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + y);
                    if (Cursor.Position.Y == newPosition.Y) { mousePositionatedY = true; }
                }
                Sleep(1);
            }
        }

        /// <summary>
        /// Set Folder Permission
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileSystemRights"></param>
        public void SetFolderPermission(string folderPath, FileSystemRights fileSystemRights)
        {
            var directoryInfo = new DirectoryInfo(folderPath);
            var directorySecurity = directoryInfo.GetAccessControl();
            var currentUserIdentity = WindowsIdentity.GetCurrent();
            var fileSystemRule = new FileSystemAccessRule(currentUserIdentity.Name,
                                                          fileSystemRights,
                                                          InheritanceFlags.ObjectInherit |
                                                          InheritanceFlags.ContainerInherit,
                                                          PropagationFlags.None,
                                                          AccessControlType.Allow);

            directorySecurity.AddAccessRule(fileSystemRule);
            directoryInfo.SetAccessControl(directorySecurity);
        }

        /// <summary>
        /// Is Folder Writable
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public bool IsFolderWritable(string folderPath)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        folderPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Color InvertMeAColour(Color ColourToInvert)
        {
            int RGBMAX = 255;
            return Color.FromArgb(RGBMAX - ColourToInvert.R,
              RGBMAX - ColourToInvert.G, RGBMAX - ColourToInvert.B);
        }

        /// <summary>
        /// Get Most Used Color
        /// </summary>
        /// <param name="bitMap"></param>
        /// <returns></returns>
        public Color GetMostUsedColor(Bitmap bitMap)
        {
            var colorIncidence = new Dictionary<int, int>();
            for (var x = 0; x < bitMap.Size.Width; x++)
                for (var y = 0; y < bitMap.Size.Height; y++)
                {
                    var pixelColor = bitMap.GetPixel(x, y).ToArgb();
                    if (colorIncidence.Keys.Contains(pixelColor))
                        colorIncidence[pixelColor]++;
                    else
                        colorIncidence.Add(pixelColor, 1);
                }
            return Color.FromArgb(colorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).First().Key);
        }

        /// <summary>
        /// Assembly Title
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// Assembly Version
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Assembly Description
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Assembly Product
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Assembly Copyright
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Assembly Company
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary>
        /// Program Is Running
        /// </summary>
        /// <param name="programName"></param>
        /// <returns></returns>
        public bool ProgramIsRunning(string programName)
        {
            Process[] processlist = Process.GetProcesses();
            bool isRunning = false;

            foreach (Process process in processlist)
            {
                if (process.ProcessName.ToLower().Contains(programName.ToLower()))
                {
                    isRunning = true;
                    break;
                }
            }

            return isRunning;
        }

        /// <summary>
        /// Is Open Form
        /// </summary>
        /// <param name="openForms"></param>
        /// <param name="formName"></param>
        /// <returns></returns>
        public bool IsOpenForm(FormCollection openForms, string formName)
        {
            bool result = false;
            foreach (Form frm in openForms)
            {
                if (frm.Name == formName) result = true;
            }
            return result;
        }

        /// <summary>
        /// Program Running Count
        /// </summary>
        /// <param name="programName"></param>
        /// <returns></returns>
        public int ProgramRunningCount(string programName)
        {
            Process[] processlist = Process.GetProcesses();
            int count = 0;

            foreach (Process process in processlist)
            {
                if (process.ProcessName.ToLower().Contains(programName.ToLower()))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Generate Rgba
        /// </summary>
        /// <param name="_color"></param>
        /// <returns></returns>
        public Color GenerateRgba(string _color)
        {
            if (_color.PadLeft(1) != "#") { _color = "#" + _color; }
            Color color = default(Color);
            try
            {
                color = ColorTranslator.FromHtml(_color);
            }
            catch (Exception ex)
            {
            }
            return color;
        }

        /// <summary>
        /// Get Thumbnail
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="defaultPictureSize"></param>
        /// <param name="percentThumbnail"></param>
        /// <param name="percentSaturation"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="widthBorder"></param>
        /// <returns></returns>
        public Bitmap GetThumbnail(Bitmap bitmap, int defaultPictureSize, int percentThumbnail, int percentSaturation, int maxWidth = 0, int maxHeight = 0, int widthBorder = 0)
        {
            Bitmap pThumbnail = null;
            try
            {
                Utility utility = new Utility();

                Bitmap bitmapTmp = bitmap;

                Dictionary<string, int> dimension = GetSizeObject(bitmapTmp.Width, bitmapTmp.Height, defaultPictureSize, percentThumbnail, maxWidth, maxHeight);

                int width = dimension["Width"];
                int height = dimension["Height"];
                int newWidthBorder = (widthBorder * (100 - percentThumbnail) / 100);
                if (width > (widthBorder * 2)) { width -= newWidthBorder * 2; }
                if (height > (widthBorder * 2)) { height -= newWidthBorder * 2; }

                Bitmap bitmapResize = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(bitmapResize))
                {
                    graphics.DrawImage(bitmapTmp, 0, 0, width, height);
                }

                pThumbnail = utility.ChangeStaurationToBitmap(bitmapResize, percentSaturation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pThumbnail;
        }

        /// <summary>
        /// Get Size Object
        /// </summary>
        /// <param name="originalWidth"></param>
        /// <param name="originalHeight"></param>
        /// <param name="defaultPictureSize"></param>
        /// <param name="percentThumbnail"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetSizeObject(int originalWidth, int originalHeight, int defaultPictureSize, int percentThumbnail, int maxWidth = 0, int maxHeight = 0)
        {
            Dictionary<string, int> dimension = new Dictionary<string, int>() { };
            try
            {
                int width = 0;
                int height = 0;
                double coefficient = 0;
                bool resolutionValid = false;
                if (maxWidth == 0) { maxWidth = originalWidth; }
                if (maxHeight == 0) { maxHeight = originalHeight; }
                if (originalWidth > originalHeight)
                {
                    if (originalWidth == 0) { originalWidth = defaultPictureSize; }
                    width = originalWidth;
                    coefficient = ((double)originalWidth / (double)width);
                    if (coefficient.ToString() == "NaN") { coefficient = 0; }
                    height = Convert.ToInt32((double)originalHeight / (double)coefficient);
                    if (height == 0) { height = defaultPictureSize; }
                    if (width <= maxWidth && height <= maxHeight) { resolutionValid = true; }
                    while (!resolutionValid)
                    {
                        width -= 1;
                        coefficient = ((double)originalWidth / (double)width);
                        if (coefficient.ToString() == "NaN") { coefficient = 0; }
                        height = Convert.ToInt32((double)originalHeight / (double)coefficient);
                        if (height == 0) { height = defaultPictureSize; }
                        if (width <= maxWidth && height <= maxHeight) { resolutionValid = true; }
                    }

                    width = Convert.ToInt32((double)width * (double)(100 - percentThumbnail) / 100d);
                    if (width == 0) { width = defaultPictureSize; }
                    coefficient = ((double)originalWidth / (double)width);
                    if (coefficient.ToString() == "NaN") { coefficient = 0; }
                    height = Convert.ToInt32((double)originalHeight / (double)coefficient);
                    if (height == 0) { height = defaultPictureSize; }
                }
                else
                {
                    if (originalHeight == 0) { originalHeight = defaultPictureSize; }
                    height = originalHeight;
                    coefficient = ((double)originalHeight / (double)height);
                    if (coefficient.ToString() == "NaN") { coefficient = 0; }
                    width = Convert.ToInt32((double)originalWidth / (double)coefficient);
                    if (width == 0) { width = defaultPictureSize; }
                    if (width <= maxWidth && height <= maxHeight) { resolutionValid = true; }
                    while (!resolutionValid)
                    {
                        height -= 1;
                        coefficient = ((double)originalHeight / (double)height);
                        if (coefficient.ToString() == "NaN") { coefficient = 0; }
                        width = Convert.ToInt32((double)originalWidth / (double)coefficient);
                        if (width == 0) { width = defaultPictureSize; }
                        if (width <= maxWidth && height <= maxHeight) { resolutionValid = true; }
                    }

                    height = Convert.ToInt32((double)height * (double)(100 - percentThumbnail) / 100d);
                    if (height == 0) { height = defaultPictureSize; }
                    coefficient = ((double)originalHeight / (double)height);
                    if (coefficient.ToString() == "NaN") { coefficient = 0; }
                    width = Convert.ToInt32((double)originalWidth / (double)coefficient);
                    if (width == 0) { width = defaultPictureSize; }
                }
                dimension["Width"] = width;
                dimension["Height"] = height;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dimension;
        }

        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="keyValuePairs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(HttpMethod methodType, string baseUrl, string apiUrl, Dictionary<string, string> keyValuePairs = null, string token = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                //init
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMinutes(10);

                var par = "";

                //add parameters in Uri
                if (((keyValuePairs != null && keyValuePairs.Count > 0) && (methodType == HttpMethod.Get || methodType == HttpMethod.Delete)))
                {
                    par = JsonToQuery(JsonConvert.SerializeObject(keyValuePairs));
                }

                //create request
                var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl + par);

                if (token != null && token != String.Empty)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                //add parameters in headers
                if ((keyValuePairs != null && keyValuePairs.Count > 0) && (methodType == HttpMethod.Post || methodType == HttpMethod.Put))
                {
                    var content = new FormUrlEncodedContent(keyValuePairs);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    request.Content = content;
                }
                //Console.WriteLine(request);
                //call
                response = await client.SendAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Json To Query
        /// </summary>
        /// <param name="jsonQuery"></param>
        /// <returns></returns>
        public string JsonToQuery(string jsonQuery)
        {
            string str = "?";
            str += jsonQuery.Replace(":", "=").Replace("{", "").
                        Replace("}", "").Replace(",", "&").
                            Replace("\"", "");
            return str;
        }

        /// <summary>
        /// Get Public IP Address
        /// </summary>
        /// <returns></returns>
        public string GetPublicIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

        /// <summary>
        /// Convert Bytes To Mega bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        /// <summary>
        /// Run AS
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="hidden"></param>
        /// <param name="loadUserProfile"></param>
        /// <param name="waitForExit"></param>
        /// <returns></returns>
        public (int? ProcessId, string Error) RunAS(string path, string filename, string arguments, string domain, string userName, string password, bool hidden = false, bool loadUserProfile = false, bool waitForExit = false)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            (int? ProcessId, string Error) output;
            output.ProcessId = null;
            output.Error = null;

            System.Security.SecureString ssPwd = new System.Security.SecureString();
            startInfo.UseShellExecute = false;
            startInfo.FileName = Path.Combine(path, filename);

            startInfo.WorkingDirectory = path;
            startInfo.Arguments = arguments;
            startInfo.Domain = domain;
            startInfo.UserName = userName;

            for (int x = 0; x < password.Length; x++)
            {
                ssPwd.AppendChar(password[x]);
            }
            startInfo.Password = ssPwd;
            startInfo.RedirectStandardError = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.LoadUserProfile = loadUserProfile;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;

            if (hidden)
            {
                Process processTemp = new Process();
                processTemp.StartInfo = startInfo;
                processTemp.EnableRaisingEvents = true;
                processTemp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                try
                {
                    processTemp.Start();
                    output.ProcessId = processTemp.Id;
                    if (waitForExit) processTemp.WaitForExit();
                }
                catch (Exception ex)
                {
                    output.Error = ex.Message;
                }
            }
            else
            {
                Process processTemp = new Process();
                processTemp.StartInfo = startInfo;
                processTemp.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                processTemp.Start();
                output.ProcessId = processTemp.Id;
                if (waitForExit) processTemp.WaitForExit();
            }

            return output;
        }

        /// <summary>
        /// Kill Process By Id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public bool KillProcessById(int processId)
        {
            var result = false;
            try
            {
                Process proc = Process.GetProcessById(processId);
                proc.Kill();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Kill Process By Window Caption
        /// </summary>
        /// <param name="windowCaption"></param>
        /// <returns></returns>
        public bool KillProcessByWindowCaption(string windowCaption)
        {
            var result = false;
            try
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process proc in processes)
                {
                    if (proc.MainWindowTitle.ToLower().Trim().Contains(windowCaption.ToLower().Trim()))
                        proc.Kill();
                }

                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Process Is Active By Window Caption
        /// </summary>
        /// <param name="windowCaption"></param>
        /// <returns></returns>
        public bool ProcessIsActiveByWindowCaption(string windowCaption)
        {
            var result = false;
            try
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process proc in processes)
                {
                    if (proc.MainWindowTitle.ToLower().Trim().Contains(windowCaption.ToLower().Trim()))
                        result = true;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        ///  Move Ext Window
        /// </summary>
        /// <param name="windowCaption"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public IntPtr MoveExtWindow(string windowCaption, int x, int y, int width, int height)
        {
            IntPtr mainWindowHandle =
              FindWindowByCaption(IntPtr.Zero, windowCaption);

            SetWindowPos(mainWindowHandle, IntPtr.Zero,
                x, y, width, height, 0);

            return mainWindowHandle;
        }

        /// <summary>
        /// Get Active Window Name
        /// </summary>
        /// <returns></returns>
        public string GetActiveWindowName()
        {
            try
            {
                var activatedHandle = GetForegroundWindow();

                Process[] processes = Process.GetProcesses();
                foreach (Process clsProcess in processes)
                {

                    if (activatedHandle == clsProcess.MainWindowHandle)
                    {
                        string processName = clsProcess.ProcessName;

                        return processName;
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Get Active Window Title
        /// </summary>
        /// <returns></returns>
        public string GetActiveWindowTitle()
        {
            try
            {
                var activatedHandle = GetForegroundWindow();

                Process[] processes = Process.GetProcesses();
                foreach (Process clsProcess in processes)
                {

                    if (activatedHandle == clsProcess.MainWindowHandle)
                    {
                        string processName = clsProcess.MainWindowTitle;

                        return processName;
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Move Ext Window By ProcessId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public IntPtr MoveExtWindowByProcessId(int? processId, int x, int y, int width, int height)
        {
            IntPtr mainWindowHandle = default(IntPtr);

            try
            {
                var activatedHandle = GetForegroundWindow();

                Process[] processes = Process.GetProcesses();
                foreach (Process clsProcess in processes)
                {
                    if (processId == clsProcess.Id)
                    {
                        mainWindowHandle = clsProcess.MainWindowHandle;
                    }
                }

                SetWindowPos(mainWindowHandle, IntPtr.Zero,
                    x, y, width, height, 0);
            }
            catch (Exception)
            {

            }

            return mainWindowHandle;
        }

        /// <summary>
        /// Set WindowStyle By ProcessId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="processWindowStyle"></param>
        public void SetWindowStyleByProcessId(int? processId, ProcessWindowStyle processWindowStyle)
        {
            try
            {
                var activatedHandle = GetForegroundWindow();

                Process[] processes = Process.GetProcesses();
                foreach (Process clsProcess in processes)
                {
                    if (processId == clsProcess.Id)
                    {
                        clsProcess.StartInfo.WindowStyle = processWindowStyle;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Set Focus By ProcessName
        /// </summary>
        /// <param name="processName"></param>
        public void SetFocusByProcessName(string processName)
        {
            try
            {
                var activatedHandle = GetForegroundWindow();

                Process[] processes = Process.GetProcesses();
                foreach (Process clsProcess in processes)
                {
                    if (clsProcess.ProcessName.Trim().ToLower() == processName.Trim().ToLower())
                    {
                        SetFocus(new HandleRef(null, clsProcess.MainWindowHandle));
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Set Focus By Proces
        /// </summary>
        /// <param name="proces"></param>
        public void SetFocusByProcess(Process proces)
        {
            try
            {
                var activatedHandle = GetForegroundWindow();

                SetFocus(new HandleRef(null, proces.MainWindowHandle));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Get Enum Items
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<string> GetEnumItems(Type type)
        {
            var result = new List<string>();
            var values = Enum.GetValues(type);
            foreach (var item in values)
            {
                result.Add(item.ToString().ToLower());
            }

            return result;
        }

        /// <summary>
        /// SplitCamelCase
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        /// <summary>
        /// First Letter To Upper
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        /// <summary>
        /// Get Md5 Hash
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify Md5 Hash
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        /// <summary>
        /// Set Default Audio Device
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="exeFullPath"> FullPath of the file SetDefaultAudioDeviceByName.exe</param>
        /// <returns></returns>
        public (string Output, string Error) SetDefaultAudioDevice(string deviceName, string exeFullPath)
        {
            (string Output, string Error) result = RunExe(exeFullPath, @"""" + deviceName + @"""", true).GetAwaiter().GetResult();

            return result;
        }

        /// <summary>
        /// GetDefaultAudioDevice
        /// </summary>
        /// <returns></returns>
        public string GetDefaultAudioDevice()
        {
            var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();

            var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            return defaultDevice.DeviceFriendlyName;
        }

        /// <summary>
        /// Run Exe
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="arguments"></param>
        /// <param name="async"></param>
        /// <returns></returns>
        public async Task<(string Output, string Error)> RunExe(string fullPath, string arguments, bool async)
        {
            (string Output, string Error) result;
            result.Output = null;
            result.Error = null;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = fullPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = async;
                process.StartInfo.RedirectStandardError = async;
                process.Start();

                if (async)
                {
                    result.Output = process.StandardOutput.ReadToEnd();

                    result.Error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            finally
            {
                if (result.Output != null && result.Output != String.Empty) Console.WriteLine("Output: " + result.Output);
                if (result.Error != null && result.Error != String.Empty) Console.WriteLine("Error: " + result.Error);
            }

            if (result.Output != null) result.Output = result.Output.Replace(System.Environment.NewLine, "");
            if (result.Error != null) result.Error = result.Error.Replace(System.Environment.NewLine, "");

            return result;
        }

        public class ApiParameters
        {
            public Dictionary<string, string> ParametresForUrlQuery { get; set; }
            public object Object { get; set; }
            public Dictionary<string, string> Headers { get; set; }
        }

        /// <summary>
        /// Call Api
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="baseUrl"></param>
        /// <param name="apiUrl"></param>
        /// <param name="apiParameters"></param>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        /// <param name="timeoutInMinutes"></param>
        /// <param name="skipCertificateValidation"></param>
        /// <returns></returns>
        public async Task<T> CallApi<T>(HttpMethod methodType, string baseUrl, string apiUrl, ApiParameters apiParameters = null, string token = null, string tokenType = null, int timeoutInMinutes = 10, bool skipCertificateValidation = false)
        {
            T result;

            if(skipCertificateValidation)
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var handler = new HttpClientHandler()
            {
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls
            };

            try
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    //init
                    client.BaseAddress = new Uri(baseUrl);
                    client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);

                    var par = "";

                    //add parameters in Uri
                    if (apiParameters != null && apiParameters.ParametresForUrlQuery != null && apiParameters.ParametresForUrlQuery.Count > 0)
                    {
                        par = DictionaryToQuery(apiParameters.ParametresForUrlQuery);
                    }

                    //create request
                    var request = new HttpRequestMessage(methodType, client.BaseAddress + apiUrl + par);

                    if (token != null && token != String.Empty)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    }

                    //add parameters in headers
                    if (apiParameters != null && apiParameters.Object != null)
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(apiParameters.Object), Encoding.UTF8, "application/json");

                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        request.Content = content;
                    }

                    //add additional headers
                    if (apiParameters != null && apiParameters.Headers != null)
                    {
                        foreach (var header in apiParameters.Headers)
                        {
                            try
                            {
                                request.Headers.Add(header.Key, header.Value);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    //call
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode == false)
                    {
                        throw new Exception($"Call Api Error - ReasonPhrase:{response.ReasonPhrase}, StatusCode:{response.StatusCode}");
                    }
                    else
                    {
                        var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        result = JsonConvert.DeserializeObject<T>(content);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Dictionary To Query
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public string DictionaryToQuery(Dictionary<string, string> keyValuePairs)
        {
            var result = "";
            var query = "";

            foreach (var kvp in keyValuePairs)
            {
                if (query != "") query += "&";
                query += $"{kvp.Key}={kvp.Value}";
            }

            if (query != "") result = "?" + query;

            return result;
        }
    }
}