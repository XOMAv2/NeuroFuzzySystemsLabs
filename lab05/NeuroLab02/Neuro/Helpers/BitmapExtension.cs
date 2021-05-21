using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Neuro.Helpers
{
    public static class BitmapExtension
    {
        /// <summary>
        /// Изменение размера Bitmap'а.
        /// </summary>
        /// <param name="width"> Ширина нового изображения. </param>
        /// <param name="height"> Высота нового изображения. </param>
        public static Bitmap Resize(this Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage
                (
                    image,
                    new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height),
                    GraphicsUnit.Pixel
                );
            }

            return resizedImage;
        }

        /// <summary>
        /// Возвращает массив из освещенностей каждого пикселя изображения.
        /// </summary>
        public static float[] GetBrightnessArray(this Bitmap image)
        {
            float[] pixels = new float[image.Height * image.Width];
            int k = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    pixels[k++] = image.GetPixel(i, j).GetBrightness();
                }
            }

            return pixels;
        }
    }
}
