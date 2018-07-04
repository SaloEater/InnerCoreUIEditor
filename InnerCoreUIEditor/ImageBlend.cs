using System;
using System.Collections.Generic;
using System.Drawing;
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
            Bitmap bitmap = (Bitmap)activeImage;
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
    }
}
