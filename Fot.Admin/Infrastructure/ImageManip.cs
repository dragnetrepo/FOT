using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Fot.Admin.Infrastructure
{
    public class ImageManip

    {
        public static Image ProcessFile(Image img, int width, int height)

        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            Graphics graphics = Graphics.FromImage((bitmap));
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(0, 0, img.Width, img.Height),
                               GraphicsUnit.Pixel);
            graphics.Dispose();
            return (bitmap);
        }

        public static Image ProcessFileByHeight(Image img, int maxHeight)

        {
            if (img.Height <= maxHeight)
            {
                return img;
            }
            else
            {
                var double1 = ((Convert.ToDouble(maxHeight)/(img.Height))*100D);
                var double2 = (((img.Width)/100D)*double1);

                return ProcessFile(img, ((int) double2), maxHeight);
            }
        }

        public static Image ProcessFileByWidth(Image img, int maxWidth)

        {
            if (img.Width <= maxWidth)
            {
                return img;
            }
            else
            {
                var double1 = ((Convert.ToDouble(maxWidth)/(img.Width))*100D);
                var double2 = (((img.Height)/100D)*double1);
                return ProcessFile(img, maxWidth, ((int) double2));
            }
        }


    }
}