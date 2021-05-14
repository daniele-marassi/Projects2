using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBox
{
    public class Models
    {

        public class TranslateData
        {
            public int IDTranslate { get; set; }
            public string Name { get; set; }
            public int IDLanguage { get; set; }
            public string CultureName { get; set; }
            public string Text { get; set; }
        }

        public enum DemoStatus
        {
            Avviable,
            OpenMenu,
            HowToClose,
            SetDirectories,
            UpdateCatalog,
            PersonalizeColor,
            GeneralSettings,
            Music,
            Info,
            Animate,
            NotAvviable,
            ReStart,
            Undefined
        }

        public enum Languages
        {
            Nothing,
            itIT,
            enUS
        }

        public enum ColorType
        {
            BackGrondColorMain,
            ForeColor,
            BorderColorMedia
        }

        public class Config
        {
            public int PictureSize { get; set; }
            public int MinPictureSize { get; set; }
            public int MaxPictureSize { get; set; }
            public int WidthBorder { get; set; }
            public int WidthScreen { get; set; }
            public int HeightScreen { get; set; }
            public int MaxPicturesHorizontal { get; set; }
            public int MaxPicturesVertical { get; set; }
        }

        public class NewPosition
        {
            public int Left { get; set; }
            public int Top { get; set; }
        }
        public class Catalog
        {
            public int ID { get; set; }
            public string PathFile { get; set; }
            public string Title { get; set; }
            public DateTimeOffset DateCreated { get; set; }
            public int WidthMedia { get; set; }
            public int HeightMedia { get; set; }
            public double DurationInSeconds { get; set; }
            public string Type { get; set; }
            public int WidthThumbnail { get; set; }
            public int HeightThumbnail { get; set; }
            public int TopThumbnail { get; set; }
            public int LeftThumbnail { get; set; }
            public Byte[] Thumbnail { get; set; }
        }

        public class SourceType
        {
            public string Directory { get; set; } = "Directory";
            public string Drive { get; set; } = "Drive";
        }

        public class MediaFile
        {
            public string PathFile { get; set; }
            public DateTimeOffset DateCreated { get; set; }
        }

        public class SourceParam
        {
            public int Index { get; set; }
            public string ValueStringJson { get; set; }
        }

        [Serializable]
        public class SourceValue
        {
            public string Type { get; set; }
            public string Path { get; set; }
            public string AccountDrive { get; set; }
            public string PathAuthenticateJsonToDrive { get; set; }
            public string FolderNameToFilterDrive { get; set; }
        }

        public class MediaParam
        {
            public List<Catalog> ListCatalog { get; set; }
            public List<Bitmap> ListThumbnail { get; set; }
            public int WidthPanel { get; set; }
            public int HeightPanel { get; set; }
            public int CountElements { get; set; }

        }
        public class DB
        {
            public TblCatalog TblCatalog { get; set; }
            public TblConfiguration TblConfiguration { get; set; }
        }

        public class TblCatalog
        {
            public DataTable Data { get; set; }
            public DataTable TypeFields { get; set; }
        }

        public class TblConfiguration
        {
            public int ID { get; set; }
            public int WidthPanel { get; set; }
            public int HeightPanel { get; set; }
            public int CountElements  { get; set; }
        }
    }
}
