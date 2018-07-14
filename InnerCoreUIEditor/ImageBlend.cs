using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    static class ImageBlend
    {        
        public static void Blend(Image back, Image front)
        {
            if (back == null || front == null) return;
            Bitmap _back = (Bitmap)back;
            Bitmap _front = (Bitmap)front;
            if(_back.Height < _front.Height || _back.Width < _front.Width)
            {
                _back = new Bitmap(ResizeImage(_back, _front.Width, _front.Height));
            }
            for (int x = 0; x < _front.Width; x++)
            {
                for(int y = 0; y < _front.Height; y++)
                {
                    int a = _front.GetPixel(x, y).A;
                    try
                    {
                        if (a < 255)
                        {
                            Color newColor = _back.GetPixel(x, y);
                            _front.SetPixel(x, y, newColor);
                        }
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        break;
                        //MessageBox.Show("Дополнительный слой не подходит по размеру под основной");                        
                    }
                }
            }
        }

        internal static Image ToPanelColor(Image activeImage)
        {
            Bitmap bitmap = new Bitmap(activeImage);
            Color panelColor = Global.panelWorkspace.BackColor;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int a = bitmap.GetPixel(x, y).A;
                    if (a < 255)
                    {
                        bitmap.SetPixel(x, y, panelColor);
                    }
                }
            }
            return bitmap;
        }

        internal static Image MergeWithPanel(Bitmap bitmap, Point point)
        {
            return ToPanelColor(bitmap);
        }

        internal static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        internal static Image CropVertical(Image scaledActiveImage, int value)
        {
            Bitmap bitmap = new Bitmap(scaledActiveImage);
            if (value == 100) return bitmap;
            int newHeight = scaledActiveImage.Height * value / 100;
            if (newHeight == 0) return bitmap;
            Bitmap newBitmap = new Bitmap(scaledActiveImage.Width, newHeight);
            int delta = scaledActiveImage.Height - newHeight;
            for(int i = delta; i < scaledActiveImage.Height; i++)
            {
                for(int j = 0; j < scaledActiveImage.Width; j++)
                {
                    Color color = bitmap.GetPixel(j, i);
                    newBitmap.SetPixel(j, i - delta, color);
                }
            }
            return newBitmap;
        }

        internal static Image CropHorizontal(Image scaledActiveImage, int value)
        {
            Bitmap bitmap = new Bitmap(scaledActiveImage);
            if (value == 100) return bitmap;
            int newWidth = scaledActiveImage.Width * value / 100;
            if (newWidth == 0) return null;
            Bitmap newBitmap = new Bitmap(newWidth, scaledActiveImage.Height);
            int delta = scaledActiveImage.Width - newWidth;
            for (int i = 0; i < scaledActiveImage.Height; i++)
            {
                for (int j = delta; j < scaledActiveImage.Width; j++)
                {
                    Color color = bitmap.GetPixel(j, i);
                    newBitmap.SetPixel(j - delta, i, color);
                }
            }
            return newBitmap;
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 