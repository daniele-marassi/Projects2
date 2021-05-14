using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using static MediaBox.Models;
using System.Data.SQLite;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using static Additional.SqLite;
using WMPLib;
using AxWMPLib;
using MediaToolkit;
using System.Timers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Globalization;
using Additional;
using static Additional.ProgressBar;

namespace MediaBox
{
    class MultimediaCatalog
    {

        public List<Catalog> GetMedia(string pathFolder, Form form)
        {
            List<string> getFilesResult = Directory.GetFiles(pathFolder, "*.*", System.IO.SearchOption.AllDirectories).ToList();

            List<MediaFile> mediaFilesTemp = new List<MediaFile>() { };

            DateTimeOffset dateCreated = new DateTimeOffset();
            DateTimeOffset creationTime = new DateTimeOffset();
            DateTimeOffset lastWrite = new DateTimeOffset();

            foreach (string pathFile in getFilesResult)
            {
                dateCreated = new DateTimeOffset();
                creationTime = File.GetCreationTimeUtc(pathFile);
                lastWrite = File.GetLastWriteTimeUtc(pathFile);

                if (creationTime < lastWrite) { dateCreated = creationTime; }
                else { dateCreated = lastWrite; }

                mediaFilesTemp.Add(
                        new MediaFile { PathFile = pathFile, DateCreated = dateCreated }
                    );
            }

            IOrderedEnumerable<MediaFile> mediaFiles = mediaFilesTemp.OrderBy(_ => _.DateCreated).ThenBy(_ => _.PathFile);

            List <Catalog> catalog = new List<Catalog> () { };
            Catalog catalogParam = new Catalog();
            Common common = new Common();
            Utility utility = new Utility();
            int width = 0;
            int height = 0;
            string type = "";
            double durationInSeconds = 0;

            if (mediaFiles?.Count() > 0) common.ProgressBar(mediaFiles.Count(), utility.GetCurrentMethod().Name, form);
            foreach (MediaFile mediaFile in mediaFiles)
            {
                Application.DoEvents();
                try
                {
                    width = 0;
                    height = 0;
                    type = "";

                    string extension = Path.GetExtension(mediaFile.PathFile);
                    var splitPathFile = Path.GetDirectoryName(mediaFile.PathFile).Split('\\');
                    string title = Path.GetDirectoryName(mediaFile.PathFile) != pathFolder ? splitPathFile[splitPathFile.Count() - 1] : "";
                    if (title == String.Empty)
                    {
                        title = mediaFile.DateCreated.ToString("ddd, dd MMM yyy HH':'mm");
                    }

                    if (utility.IsImage(extension))
                    {
                        type = "Image";

                        using (Bitmap img = (Bitmap)Image.FromFile(mediaFile.PathFile))
                        {
                            width = img.Width;
                            height = img.Height;

                        }
                        catalog.Add
                        (
                            new Catalog()
                            {
                                PathFile = mediaFile.PathFile,
                                Title = title,
                                DateCreated = mediaFile.DateCreated,
                                WidthMedia = width,
                                HeightMedia = height,
                                DurationInSeconds = 0,
                                Type = type
                            }
                        );
                    }
                    if (utility.IsVideo(extension))
                    {
                        type = "Video";

                        var inputFile = new MediaToolkit.Model.MediaFile { Filename = mediaFile.PathFile };
                        using (var engine = new Engine())
                        {
                            engine.GetMetadata(inputFile);
                        }

                        width = int.Parse(inputFile.Metadata.VideoData.FrameSize.Split('x')[0]);
                        height = int.Parse(inputFile.Metadata.VideoData.FrameSize.Split('x')[1]);
                        durationInSeconds = inputFile.Metadata.Duration.TotalSeconds;

                        catalog.Add
                        (
                            new Catalog()
                            {
                                PathFile = mediaFile.PathFile,
                                Title = title,
                                DateCreated = mediaFile.DateCreated,
                                WidthMedia = width,
                                HeightMedia = height,
                                DurationInSeconds = durationInSeconds,
                                Type = type
                            }
                        );
                    }

                    if (utility.IsAudio(extension))
                    {
                        type = "Audio";

                        var inputFile = new MediaToolkit.Model.MediaFile { Filename = mediaFile.PathFile };
                        using (var engine = new Engine())
                        {
                            engine.GetMetadata(inputFile);
                        }

                        width = 200;
                        height = 200;
                        durationInSeconds = inputFile.Metadata.Duration.TotalSeconds;

                        catalog.Add
                        (
                            new Catalog()
                            {
                                PathFile = mediaFile.PathFile,
                                Title = title,
                                DateCreated = mediaFile.DateCreated,
                                WidthMedia = width,
                                HeightMedia = height,
                                DurationInSeconds = durationInSeconds,
                                Type = type
                            }
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //throw;
                }
                common.ProgressBar(mediaFiles.Count(), utility.GetCurrentMethod().Name, form);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return catalog;
        }

        public Dictionary<string, int> GetSizeObject(int originalWidth , int originalHeight, int defaultPictureSize, int percentThumbnail, int maxWidth = 0, int maxHeight = 0)
        {
            Dictionary<string, int> dimension = new Dictionary<string, int>() { };

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
            return dimension;
        }

        public Bitmap GetThumbnail(Bitmap bitmap, int defaultPictureSize, int percentThumbnail, int percentSaturation, int maxWidth = 0, int maxHeight = 0, int widthBorder = 0)
        {
            Bitmap pThumbnail = null;
            try
            {
                Common common = new Common();
                Utility utility = new Utility();
                
                Bitmap bitmapTmp = bitmap;

                Dictionary<string, int> dimension = GetSizeObject(bitmapTmp.Width, bitmapTmp.Height, defaultPictureSize, percentThumbnail, maxWidth, maxHeight);

                int width = dimension["Width"];
                int height = dimension["Height"];
                int newWidthBorder = (widthBorder * (100 -percentThumbnail) / 100);
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
                Console.WriteLine(ex.ToString());
                //throw;
            }
            return pThumbnail;
        }

        public void LoadImage(System.Windows.Forms.Panel panel, Bitmap image, int x, int y,int index, Form form)
        {
            if (image == null) { return; }
            Presentation presentation = new Presentation();
            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new System.Drawing.Point(x, y);
            pictureBox.Name = "PictureBox_" + index.ToString();
            pictureBox.Size = new System.Drawing.Size(image.Width, image.Height);
            pictureBox.Image = image;
            pictureBox.BackColor = Color.Black;
            pictureBox.MouseDown += delegate (object sender, MouseEventArgs e)
            {
                Cursor.Current = Cursors.Hand;
                Statics.Position.Y = y + e.Location.Y;
                Statics.Position.X = x + e.Location.X;
                Statics.Mouse.Down = true;
            };
            pictureBox.MouseUp += delegate 
            {
                Cursor.Current = Cursors.Default;
                Statics.Mouse.Down = false;
            };

            pictureBox.MouseMove += delegate
            {
                if (Statics.Mouse.Down)
                {
                    Application.DoEvents();
                    int top = Cursor.Position.Y - (form.Location.Y + Statics.Position.Y);
                    int left = Cursor.Position.X - (form.Location.X + Statics.Position.X);
                    if (top <= 0 && (Math.Abs(top) <= Math.Abs(form.Height - panel.Height) && panel.Height > form.Height)) { panel.Top = top; }
                    if (left <= 0 && (Math.Abs(left) <= Math.Abs(form.Width - panel.Width) && panel.Width > form.Width)) { panel.Left = left; }
                }
            };

            pictureBox.DoubleClick += delegate
            {
                Statics.Media.Index = index;
                if (Statics.Presentation.Active == true) { return; }
                PictureBox_DoubleClick(index, form,y,x,image.Width, image.Height);
            };

            pictureBox.MouseHover += delegate
            {
                if (Statics.Presentation.Active == true) { return; }
                presentation.DeselectPictureBox(pictureBox, index);
                presentation.SelectPictureBox(pictureBox, index);
            };

            pictureBox.MouseLeave += delegate
            {
                if (Statics.Presentation.Active == true) { return; }
                presentation.DeselectPictureBox(pictureBox, index);
            };

            panel.Controls.Add(pictureBox);
        }

        public void PictureBox_DoubleClick(int index, Form form, int top, int left , int width, int height)
        {
            Statics.MainForm = form;
            Media media = new Media();
            media.OpenMedia(index, Statics.MainForm, top, left, width, height, false);
        }


        public class Config
        {
            Common common = new Common();
            Utility utility = new Utility();

            public Models.Config Get(Form form, int nPictures)
            {
                Models.Config config = new Models.Config { };
                try
                {
                    int widthScreen = Screen.GetBounds(form).Width;
                    int heightScreen = Screen.GetBounds(form).Height;
                    int maxPicturesVertical = 0;
                    int maxPicturesHorizontal = 0;
                    int maximumCommonDivisor = utility.MaximumCommonDivisor(widthScreen, heightScreen);
                    int dim1 = Convert.ToInt32(((Math.Sqrt(nPictures) * Math.Sqrt(widthScreen / maximumCommonDivisor)) / Math.Sqrt(heightScreen / maximumCommonDivisor)));
                    int dim2 = Convert.ToInt32(Math.Round((double)nPictures / (double)dim1));
                    if (dim1 > dim2)
                    {
                        if (widthScreen > heightScreen)
                        {
                            maxPicturesHorizontal = dim1;
                            maxPicturesVertical = dim2;
                        }
                        else
                        {
                            maxPicturesHorizontal = dim2;
                            maxPicturesVertical = dim1;
                        }
                    }
                    else
                    {
                        if (heightScreen > widthScreen)
                        {
                            maxPicturesHorizontal = dim1;
                            maxPicturesVertical = dim2;
                        }
                        else
                        {
                            maxPicturesHorizontal = dim2;
                            maxPicturesVertical = dim1;
                        }
                    }

                    int sizeHorizontal = 0;
                    int sizeVertical = 0;
                    int pictureSize = 0;

                    int multiplerSizeHorizontal = maxPicturesHorizontal - Statics.Constant.nPicturesOverPanel;

                    int multiplerSizeVertical = maxPicturesVertical - Statics.Constant.nPicturesOverPanel;

                    if (multiplerSizeHorizontal <= 0) { multiplerSizeHorizontal = 1; }
                    if (multiplerSizeVertical <= 0) { multiplerSizeVertical = 1; }

                    while ((sizeHorizontal * multiplerSizeHorizontal) < widthScreen)
                    {
                        sizeHorizontal += Statics.Constant.DefaultPictureSize;
                    }
                   
                    while ((sizeVertical * multiplerSizeVertical) < heightScreen)
                    {
                        sizeVertical += Statics.Constant.DefaultPictureSize;
                    }

                    if (sizeHorizontal > sizeVertical)
                    { pictureSize = sizeHorizontal; }
                    else { pictureSize = sizeVertical; }

                    //pictureSize = 100;////////////////////////////////////////////////////////////////

                    int minPictureSize = Convert.ToInt32(pictureSize / 2);
                    int maxPictureSize = minPictureSize * 3;


                    config = new Models.Config
                    {
                        PictureSize = pictureSize,
                        MinPictureSize = minPictureSize,
                        MaxPictureSize = maxPictureSize,
                        WidthBorder = Statics.Constant.WidthBorder,
                        WidthScreen = widthScreen,
                        HeightScreen = heightScreen,
                        MaxPicturesHorizontal = maxPicturesHorizontal,
                        MaxPicturesVertical = maxPicturesVertical
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //throw;
                }
                return config;
            }
        }

        public MediaParam PopolateCatalogFromSource(string path, Form form, int nPictures)
        {
            Common common = new Common();
            Utility utility = new Utility();
            List<Bitmap> listThumbnail = new List<Bitmap>() { };
            List<Catalog> listCatalog = GetMedia(path,form);

            Bitmap bitmap = null;
            Config config = new Config();
            string pathFile = String.Empty;
            Models.Config configResult = config.Get(form, nPictures);
            int countListCatalog = listCatalog.Count;
            if (countListCatalog > 0) common.ProgressBar(countListCatalog, utility.GetCurrentMethod().Name, form);
            int rndSize = 0;
            Catalog catalog = null;
            
            for (int i = 0; i < countListCatalog; i++)
            {
                Application.DoEvents();
                var rnd = new Random(DateTime.Now.Millisecond);

                rndSize = rnd.Next(configResult.MinPictureSize, configResult.MaxPictureSize);
                catalog = listCatalog[i];
                
                if (catalog.Type == "Image")
                {
                    pathFile = listCatalog[i].PathFile;
                }
                if (catalog.Type == "Video")
                {
                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                    pathFile = listCatalog[i].PathFile;
                    
                    ffMpeg.GetVideoThumbnail(listCatalog[i].PathFile, $"{Statics.Constant.PersonalPath}\\Temp\\VideoThumbnail.jpeg");
                    pathFile = $"{Statics.Constant.PersonalPath}\\Temp\\VideoThumbnail.jpeg";     
                }
                if (catalog.Type == "Audio")
                {
                    pathFile = $"{Statics.Constant.MainPath}\\Resources\\AudioThumbnail.gif";
                }
                using (Bitmap bitmpaFromFile = (Bitmap)Image.FromFile(pathFile))
                {
                    bitmap = GetThumbnail(bitmpaFromFile, rndSize, 100,0);
                    listCatalog[i].WidthThumbnail = bitmap.Width;
                    listCatalog[i].HeightThumbnail = bitmap.Height;

                    listThumbnail.Add(bitmap);               
                }
                common.ProgressBar(countListCatalog, utility.GetCurrentMethod().Name, form);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return new MediaParam { ListThumbnail = listThumbnail, ListCatalog = listCatalog };

        }

        public void FillMapPanel(ref string[,] mapPanel, int widthPicture, int heightPicture, int top, int left, int nPicture)
        {
            for (int y = top; y <= heightPicture + top; y++)
            {
                for (int x = left; x <= widthPicture + left; x++)
                {
                    if (y == mapPanel.GetLength(0) || x == mapPanel.GetLength(1) || x < 0 || y < 0 ) { break; }
                    mapPanel[y, x] = nPicture.ToString();
                }
            }
        }

        public Models.NewPosition GetNewPositionMapPanel(ref string[,] mapPanel, int width, int height )
        {
            Models.NewPosition newPosition = FindNewPositionMapPanel(ref mapPanel, width, height);

            int top = newPosition.Top;
            int left = newPosition.Left;

            bool isRnd = false;
            int widthSpace = 0;
            int heightSpace = 0;
            var rnd = new Random(DateTime.Now.Millisecond);
            int rndSize = rnd.Next(1, 4);
            if (top == 0) { heightSpace = Statics.Constant.DefaultPictureSize / rndSize; isRnd = true; }
            rnd = new Random(DateTime.Now.Millisecond);
            rndSize = rnd.Next(1, 4);
            if (left == 0) { widthSpace = Statics.Constant.DefaultPictureSize / rndSize; isRnd = true; }

            if (isRnd == true)
            {
                isRnd = false;
                int widthBorder = Statics.Constant.WidthBorder;
                bool free = CheckPositionMapPanel(ref mapPanel, (width + widthSpace + widthBorder), (height + heightSpace + widthBorder), top, left);
                if (free == true)
                {
                    FillMapPanel(ref mapPanel, (widthSpace + widthBorder), (heightSpace + widthBorder), top, left, -1);
                    newPosition = new Models.NewPosition { Top = (top + heightSpace), Left = (left + widthSpace) };
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            return newPosition;
        }

        public bool CheckPositionMapPanel(ref string[,] mapPanel, int width, int height, int top, int left)
        {
            bool free = true;
            try
            {
                for (int y = top; y <= (top + height); y++)
                {
                    for (int x = left; x <= (left + width); x++)
                    {
                        if (mapPanel[y, x] != null && mapPanel[y, x] != String.Empty)
                        {
                            free = false;
                            if (free == false) { break; }
                        }
                    }
                    if (free == false) { break; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                free = false;
            }   

            return free;
        }

        public Models.NewPosition FindNewPositionMapPanel(ref string[,] mapPanel, int width, int height)
        {
            int top = -1;
            int left = -1;
            bool free = true;
            for (int y = 0; y < mapPanel.GetLength(0); y++)
            {
                for (int x = 0; x < mapPanel.GetLength(1); x++)
                {
                    if (mapPanel[y, x] == null || mapPanel[y, x] == String.Empty)
                    {
                        free = true;
                        for (int xLineTop = x; xLineTop <= (x + width); xLineTop++)
                        {
                            if (mapPanel.GetLength(0) == y || mapPanel.GetLength(1) == xLineTop) { free = false; break; }
                            if (mapPanel[y, xLineTop] != null && mapPanel[y, xLineTop] != String.Empty) { free = false; break; }
                        }
                        if (free == true)
                        {
                            for (int xLineBottom = x; xLineBottom <= (x + width); xLineBottom++)
                            {
                                if (mapPanel[y + height, xLineBottom] != null && mapPanel[y + height, xLineBottom] != String.Empty) { free = false; break; }
                            }
                        }
                        if (free == true)
                        {
                            for (int yLineLeft = y; yLineLeft <= (y + height); yLineLeft++)
                            {
                                if (mapPanel[yLineLeft, x] != null && mapPanel[yLineLeft, x] != String.Empty) { free = false; break; }
                            }
                        }
                        if (free == true)
                        {
                            for (int yLineRight = y; yLineRight <= (y + height); yLineRight++)
                            {
                                if (mapPanel[yLineRight, x + width] != null && mapPanel[yLineRight, x + width] != String.Empty) { free = false; break; }
                            }
                        }
                        if (free == true)
                        {
                            top = y;
                            left = x;
                            break;
                        }
                    }
                }
                if (top > -1 && left > -1) { break; }
            }

            Models.NewPosition newPosition = new Models.NewPosition { Top = top, Left = left };
            return newPosition;
        }

        public MediaParam PicturePosition(Models.MediaParam media, Form form)
        {
            Models.MediaParam outputMedia = new Models.MediaParam();
            List<Catalog> listCatalog = media.ListCatalog;
            List<Bitmap> listThumbnail = media.ListThumbnail;
            int nPictures = 0;
            if (listThumbnail  != null) { nPictures = listThumbnail.Count; }
            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();
            MultimediaCatalog.Config config = new MultimediaCatalog.Config();
            Common common = new Common();
            Utility utility = new Utility();
            Models.Config configResult = config.Get(form, nPictures);

            int maxPicturesHorizontal = configResult.MaxPicturesHorizontal;
            int maxPicturesVertical = configResult.MaxPicturesVertical;
            int widthBorder = configResult.WidthBorder;
            int left = configResult.WidthBorder;
            int top = configResult.WidthBorder;
            int percent = 40;
            int widthPanel = (widthBorder + ((maxPicturesHorizontal - (maxPicturesHorizontal * percent / 100)) * (configResult.MaxPictureSize + widthBorder)));
            int heightPanel = (widthBorder + ((maxPicturesVertical ) * (configResult.MaxPictureSize + widthBorder)));
            string[,] mapPanel = new string[heightPanel, widthPanel];
            Models.NewPosition newPosition = null;

            if (nPictures > 0) common.ProgressBar(nPictures, utility.GetCurrentMethod().Name, form);
            for (int i = 0; i < nPictures; i++)
            {
                Application.DoEvents();
                newPosition = GetNewPositionMapPanel(ref mapPanel, (listThumbnail[i].Width + widthBorder), (listThumbnail[i].Height + widthBorder));
                listCatalog[i].TopThumbnail = newPosition.Top;
                listCatalog[i].LeftThumbnail = newPosition.Left;
                listCatalog[i].Thumbnail = utility.BitmapToByteArray(listThumbnail[i]);

                FillMapPanel(ref mapPanel, listThumbnail[i].Width + widthBorder, listThumbnail[i].Height + widthBorder, newPosition.Top, newPosition.Left, i);
                common.ProgressBar(nPictures, utility.GetCurrentMethod().Name, form);
            }
            if (nPictures > 0) { outputMedia = new Models.MediaParam { ListThumbnail = listThumbnail, ListCatalog = listCatalog, WidthPanel = widthPanel, HeightPanel = heightPanel, CountElements = listThumbnail.Count }; }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return outputMedia;
        }

        public void SaveData(Models.MediaParam media, Form form)
        {
            Common common = new Common();
            Utility utility = new Utility();
            StatusOption statusOption = new StatusOption();
            common.ProgressBar(100, utility.GetCurrentMethod().Name, form, statusOption.Start);
            SqLite sqLite = new SqLite();
            Data data = new Data();
            List<dynamic> rows = media.ListCatalog?.Cast<dynamic>().ToList();
            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            if (rows != null) { data.PopolateCatalogInDataBase("Catalog", rows, form); }
            sqLite.CloseDB();
            List<dynamic> configuration = new List<dynamic>() {
                new TblConfiguration
                {
                    WidthPanel = media.WidthPanel,
                    HeightPanel = media.HeightPanel,
                    CountElements = media.CountElements
                }
            };

            if (Statics.FormParam.Demo != DemoStatus.NotAvviable && Statics.FormParam.Demo != DemoStatus.ReStart)
            { sqLite.OpenDB(Statics.Constant.FilePathDemoDB); }
            else { sqLite.OpenDB(Statics.Constant.FilePathDB); }
            string sql = sqLite.CreateUpdateSql("Configuration", configuration, form, "ID = 1");
            sqLite.ExecuteCommand(sql);
            sqLite.CloseDB();
            common.ProgressBar(100, utility.GetCurrentMethod().Name, form, statusOption.End);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void LoadThumbnail(Panel panel, Models.DB db, Form form)
        {
            MultimediaCatalog multimediaCatalog = new MultimediaCatalog();

            Common common = new Common();
            Utility utility = new Utility();
            panel.Top = Statics.Constant.WidthBorder;
            panel.Left = Statics.Constant.WidthBorder;
            panel.Visible = true;
            panel.AutoSize = true;
            Bitmap bitmap = null;
            Statics.Presentation.CountElements = db.TblCatalog.Data.Rows.Count;
            if (Statics.Presentation.CountElements > 0) common.ProgressBar(Statics.Presentation.CountElements, utility.GetCurrentMethod().Name, form);
            for (int i = 0; i < Statics.Presentation.CountElements; i++)
            {
                Application.DoEvents();
                if (db.TblCatalog.Data.Rows[i]["Thumbnail"] != null)
                {
                    bitmap = (Bitmap)utility.ByteArrayToImage(db.TblCatalog.Data.Rows[i]["Thumbnail"] as Byte[]);
                    LoadImage(panel, bitmap, int.Parse(db.TblCatalog.Data.Rows[i]["LeftThumbnail"].ToString()), int.Parse(db.TblCatalog.Data.Rows[i]["TopThumbnail"].ToString()), i,form);
                }
                common.ProgressBar(Statics.Presentation.CountElements, utility.GetCurrentMethod().Name, form);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}