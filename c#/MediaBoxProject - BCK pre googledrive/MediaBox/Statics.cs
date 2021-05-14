using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MediaBox.Models;

namespace MediaBox
{
    public class Statics
    {
        public static Models.DB DB { get; set; }

        public static class FormParam
        {
            public static Color BackGrondColorMain { get; set; } = new Color();
            public static Color BackGrondColorControl { get; set; } = new Color();
            public static Color BackGrondColorControlSelected { get; set; } = new Color();
            public static Color BorderColorMedia { get; set; } = new Color();
            public static Color ForeColor { get; set; } = new Color();
            public static Color BackForeColor { get; set; } = new Color();
            public static int Width { get; set; }
            public static int Height { get; set; }
            public static int Top { get; set; }
            public static int Left { get; set; }
            public static int WidthArea { get; set; }
            public static int HeightArea { get; set; }
            public static int TopArea { get; set; }
            public static int LeftArea { get; set; }
            public static double PictureInterval { get; set; }
            public static double VideoInterval { get; set; }
            public static int WidthBorderMedia { get; set; }
            public static bool AutoRunAnimationMedia { get; set; } = false;
            public static int VolumeAudio { get; set; }
            public static int MediaMovementSpeed { get; set; }
            public static DemoStatus Demo { get; set; } = 0;
            public static bool DemoRuning { get; set; } = false;
            public static bool DemoFirstMedia { get; set; } = true;
            public static bool Restarting { get; set; } = false;
            public static Languages LanguageSelected { get; set; } = 0;
        }

        public static List<Models.TranslateData> Translates { get; set; }


        public static class Mouse
        {
            public static bool Down { get; set; }
        }

        public static Point Position;

        public static List<DirectoryParam> Directories = new List<DirectoryParam>() { };

        public static PrivateFontCollection Fonts { get; set; } = null;

        public static bool MenuHide { get; set; } = false;

        public static Form MainForm { get; set; }

        public static Form MediaForm { get; set; }

        public static class Music
        {
            public static bool Play { get; set; } = false;
            public static string SelectedPlayListName { get; set; }
        }

        public static class Constant
        {
            public static int DefaultPictureSize { get; set; }
            public static int WidthBorder { get; set; }
            public static int nPicturesOverPanel { get; set; }
            public static string FilePathDB { get; set; }
            public static string FilePathDemoDB { get; set; }
            public static string MainPath { get; set; }
            public static string PersonalPath { get; set; }
        }

        public static class Media
        {
            public static int IncrementPercent { get; set; }
            public static string Type { get; set; }
            public static string Title { get; set; }
            public static List<Bitmap> BitmapMediaList { get; set; }
            public static int Width { get; set; }
            public static int Height { get; set; }
            public static int Top { get; set; }
            public static int Left { get; set; }
            public static int SizeThumbnail { get; set; }
            public static int Index { get; set; }
        }

        public static class Presentation
        {
            public static int Next { get; set; } = 0;
            public static int Previous { get; set; } = -1;
            public static int Between { get; set; } = 0;
            public static int Selected { get; set; } = 0;
            public static List<int> Views { get; set; }
            public static int CountElements { get; set; } = 0;
            public static bool Start { get; set; } = false;
            public static int CountFounded { get; set; } = 0;
            public static bool Running { get; set; } = false;
            public static bool Active { get; set; } = false;
            public static double MaximumDifferenceBetweenImagesInMilliseconds { get; set; } = 2000d;
            public static int MaximumNumberOfImagesInSequence { get; set; } = 5;
            public static int IntervalImagesSequenceInMilliseconds { get; set; } = 500;
            public static bool ImagesSequence { get; set; } = false;
        }
    }
}