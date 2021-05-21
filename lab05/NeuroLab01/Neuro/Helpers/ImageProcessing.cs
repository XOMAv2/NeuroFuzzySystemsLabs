using Neuro.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Neuro.Helpers
{
    class ImageProcessing
    {
        /// <summary> Загрузки изображений из датасета. </summary>
        /// <param name="folderPath"> Путь к датасету. </param>
        /// <param name="pattern">
        /// Регулярное выражение для выделения индекса
        /// класса из имени файла с изображением.
        /// </param>
        public static ImageData[] LoadImages(string folderPath, string pattern)
        {
            string[] filenames = Directory.GetFiles(folderPath);
            List<ImageData> images = new List<ImageData>();
            Regex regex = new Regex(pattern);

            Parallel.ForEach(filenames, filename => images.Add(new ImageData
            {
                Data = ImageToSignalVector(new Bitmap(filename)),
                Class = int.Parse(regex.Match(Path.GetFileNameWithoutExtension(filename)).Value)
            }));

            return images.ToArray();
        }

        /// <summary>
        /// Преобразует картинку в целочисленный массив с пороговой функцией цвета.
        /// </summary>
        public static byte[] ImageToSignalVector(Bitmap image)
        {
            byte[] pixels = new byte[image.Height * image.Width];
            int k = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    pixels[k++] = Step(image.GetPixel(i, j));
                }
            }

            return pixels;
        }

        private static byte Step(Color color)
        {
            bool pixel = color.R < 100;

            if (!pixel)
                return color.G < 100 ? (byte)1 : (byte)0;
            if (!pixel)
                return color.B < 100 ? (byte)1 : (byte)0;

            return pixel ? (byte)1 : (byte)0;
        }
    }
}
