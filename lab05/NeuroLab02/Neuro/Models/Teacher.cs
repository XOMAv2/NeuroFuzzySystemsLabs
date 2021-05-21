using Neuro.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neuro.Models
{
    /// <summary>
    /// Учитель учит перцептрон распознаванию.
    /// </summary>
    class Teacher : ITeacher
    {
        public IPerceptron Perceptron { get; }

        public IList<float> PrecisionLog { get; private set; }

        public void ClearPrecisionLog() => PrecisionLog = new List<float>();

        /// <param name="perceptron"> Перцептрон. </param>
        public Teacher(IPerceptron perceptron) => (Perceptron, PrecisionLog) = (perceptron, new List<float>());

        public float GetCurrentPrecision(IList<ImageData> testImages, float eps = 0.1f)
        {
            int sum = 0;

            for (int i = 0; i < testImages.Count; i++)
            {
                IList<float> output = Perceptron.Recognize(testImages[i].Data);
                List<int> classes = new List<int>();

                for (int j = 0; j < output.Count; j++)
                {
                    if (output[j] + Math.Abs(eps) > 1)
                    {
                        classes.Add(j);
                    }
                }

                if (classes.Count == 1 && classes[0] == testImages[i].Class)
                {
                    sum++;
                }
            }

            return sum / (float)testImages.Count;
        }

        public IEnumerable<Tuple<int, int>> Teach(IList<ImageData> teachImages, int eraCount, uint? batchSize = null)
        {
            SetClassVectors(ref teachImages);
            Random rand = new Random();

            for (int i = 1; i <= eraCount; i++)
            {
                if (batchSize != null)
                {
                    for (int j = 0; j < batchSize; j++)
                    {
                        int k = rand.Next(teachImages.Count);
                        Perceptron.Backpropagation(teachImages[k].Data, teachImages[k].ClassVector);
                        yield return new Tuple<int, int>(j + 1, i);
                    }
                }
                else
                {
                    for (int j = 0; j < teachImages.Count; j++)
                    {
                        Perceptron.Backpropagation(teachImages[j].Data, teachImages[j].ClassVector);
                        yield return new Tuple<int, int>(j + 1, i);
                    }
                }

                PrecisionLog.Add(-1);
            }
        }

        public IEnumerable<Tuple<int, int>> Teach(IList<ImageData> teachImages, IList<ImageData> testImages, int eraCount, uint? batchSize = null)
        {
            SetClassVectors(ref teachImages);
            Random rand = new Random();

            for (int i = 1; i <= eraCount; i++)
            {
                if (batchSize != null)
                {
                    for (int j = 0; j < batchSize; j++)
                    {
                        int k = rand.Next(teachImages.Count);
                        Perceptron.Backpropagation(teachImages[k].Data, teachImages[k].ClassVector);
                        yield return new Tuple<int, int>(j + 1, i);
                    }
                }
                else
                {
                    for (int j = 0; j < teachImages.Count; j++)
                    {
                        Perceptron.Backpropagation(teachImages[j].Data, teachImages[j].ClassVector);
                        yield return new Tuple<int, int>(j + 1, i);
                    }
                }

                PrecisionLog.Add(GetCurrentPrecision(testImages));
            }
        }

        /// <summary>
        /// Устновка ожидаемых значений на выходах R-элементов для изображений.
        /// </summary>
        private void SetClassVectors(ref IList<ImageData> images)
        {
            foreach (ImageData image in images)
            {
                image.ClassVector = new float[Perceptron.Layers[Perceptron.LayerCount - 1].Size];

                // iamge.Class - это индекс активного R-элемента
                // (на этой позиции будет 1, на остальных - 0).
                if (Perceptron.Layers[Perceptron.LayerCount - 1].Size > image.Class)
                {
                    image.ClassVector[image.Class] = 1;
                }
            }
        }
    }
}
