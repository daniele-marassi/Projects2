using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Additional
{
    public class Utility
    {
        public static Form mainForm { get; set; }

        public string GetFullPathClass(object _class)
        {
            string pathClass = String.Empty;
            if (_class.GetType().Assembly.FullName == this.GetType().Assembly.FullName)
            { pathClass = _class.GetType().Namespace + "." + _class.GetType().Name; }
            else { pathClass = _class.GetType().Namespace + "." + _class.GetType().Name + ", " + _class.GetType().Assembly.FullName; }

            return pathClass;
        }

        //private bool ValidValue(dynamic istanceModel, string value)
        //{
        //    bool valid = false;
        //    try
        //    {
        //        foreach (var prop in istanceModel.GetType().GetProperties())
        //        {
        //            if (prop.GetValue(istanceModel, null).ToString().ToLower() == value.ToLower())
        //            { valid = true; break; }
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //    return valid;
        //}

        public class Method
        {
            public string Name { get; set; }
            public string ParentPath { get; set; }
        }
        public Method GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            var method = sf.GetMethod();
            return new Method { Name = method.Name, ParentPath = method.DeclaringType.FullName };
        }
        public Font GetFonts(string nameFont, float sizeFont, PrivateFontCollection fonts)
        {
            FontFamily fontFamily = default(FontFamily);
            var result = fonts.Families.Where(_ => _.Name == nameFont).FirstOrDefault();
            if (result != null) { fontFamily = result; }
            return new Font(fontFamily, sizeFont);
        }

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
        public bool Sleep(int milliseconds)
        {
            DateTime endTime = DateTime.Now;
            endTime = endTime.AddMilliseconds(milliseconds);
            while (DateTime.Now < endTime)
            {
                Application.DoEvents();
            }
            return true;
        }
         
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


        public List<string> GetProperty(Type typeofModel)
        {
            List<string> props = new List<string>() { };

            foreach (var prop in typeofModel.GetProperties())
            {
                props.Add(prop.Name);
            }
            return props;
        }

        public Byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
        }
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
    }
}
