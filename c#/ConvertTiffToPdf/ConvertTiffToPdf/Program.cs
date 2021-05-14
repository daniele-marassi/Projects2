using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ConvertTiffToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Image> images = new List<Image>() { };

            string file = "c:\\BD_ST41.0003_001_04.tiff";
            images.AddRange(GetAllPages(file));

            int i = 0;
            foreach (var image in images)
            {
                image.Save($"c:\\myfile{i}.tiff", ImageFormat.Tiff);
                i++;
            }



        }

        private static List<Image> GetAllPages(string file)
        {
            List<Image> images = new List<Image>();
            Bitmap bitmap = (Bitmap)Image.FromFile(file);
            int count = bitmap.GetFrameCount(FrameDimension.Page);
            for (int idx = 0; idx < count; idx++)
            {
                // save each frame to a bytestream
                bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();
                bitmap.Save(byteStream, ImageFormat.Tiff);

                // and then create a new Image from it
                images.Add(Image.FromStream(byteStream));
            }
            return images;
        }

    }
}
