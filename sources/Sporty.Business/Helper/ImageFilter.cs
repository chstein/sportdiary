using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporty.Business.Helper
{
    public class ImageFilter
    {
        public bool CheckAndResizeImage(string filepath, Stream filestream)
        {
            double maxSize = 80;
            try
            {
                using (var img = Image.FromStream(filestream))
                {
                    if (img.Width <= maxSize && img.Height <= maxSize)
                    {
                        img.Save(filepath);
                    }
                    else
                    {
                        var widthRation = maxSize / (double)img.Width;
                        var heightRatio = maxSize / (double)img.Height;

                        var ratio = widthRation < heightRatio ? widthRation : heightRatio;
                        int newWidth = (int)(ratio * img.Width);
                        int newHeight = (int)(ratio * img.Height);



                        Image thumbNail = new Bitmap(newWidth, newHeight, img.PixelFormat);
                        Graphics g = Graphics.FromImage(thumbNail);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Rectangle rect = new Rectangle(0, 0, newWidth, newHeight);
                        g.DrawImage(img, rect);

                        thumbNail.Save(filepath);
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                ;
            }
            return false;
        }
    }
}
