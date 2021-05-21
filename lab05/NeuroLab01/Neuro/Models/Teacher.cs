using Neuro.Models.Interaces;
using System;
using System.Collections.Generic;
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

        public IList<double> PrecisionLog { get; private set; }

        public void ClearPrecisionLog() => PrecisionLog = new List<double>();

        /// <param name="perceptron"> Перцептрон. </param>
        public Teacher(IPerceptron perceptron) => (Perceptron, PrecisionLog) = (perceptron, new List<double>());

        public double GetCurrentPrecision(IList<ImageData> testImages, IList<IList<byte>> aTransfers)
        {
            if (testImages.Count != aTransfers.Count) throw new Exception();

            int sum = 0;

            for (int i = 0; i < testImages.Count; i++)
            {
                IList<byte> output = Perceptron.Recognize(testImages[i].Data, aTransfers[i]);
                List<int> classes = new List<int>();

                for (int j = 0; j < output.Count; j++)
                {
                    if (output[j] != 0)
                    {
                        classes.Add(j);
                    }
                }

                if (classes.Count == 1 && classes[0] == testImages[i].Class)
                {
                    sum++;
                }
            }

            return sum / (double)testImages.Count;
        }

        public void Teach(IList<ImageData> teachImages, int n)
        {            
            IList<byte>[] aTeachArray = new IList<byte>[teachImages.Count];
            Parallel.For(0, teachImages.Count, i => aTeachArray[i] = Perceptron.GetATransfers(teachImages[i].Data));

            SetClassVector(ref teachImages);

            while (n-- > 0)
            {
                for (int i = 0; i < teachImages.Count; i++)
                {
                    Perceptron.Teach(teachImages[i].Data, teachImages[i].ClassVector, aTeachArray[i]);
                }

                PrecisionLog.Add(-1);
            }
        }

        public void Teach(IList<ImageData> teachImages, IList<ImageData> testImages, int n)
        {
            IList<byte>[] aTeachArray = new IList<byte>[teachImages.Count];
            Parallel.For(0, teachImages.Count, i => aTeachArray[i] = Perceptron.GetATransfers(teachImages[i].Data));

            IList<byte>[] aTestArray = new IList<byte>[testImages.Count];
            Parallel.For(0, testImages.Count, i => aTestArray[i] = Perceptron.GetATransfers(testImages[i].Data));

            SetClassVector(ref teachImages);

            while (n-- > 0)
            {
                for (int i = 0; i < teachImages.Count; i++)
                {
                    Perceptron.Teach(teachImages[i].Data, teachImages[i].ClassVector, aTeachArray[i]);
                }

                PrecisionLog.Add(GetCurrentPrecision(testImages, aTestArray));
            }
        }

        /// <summary>
        /// Устновка ожидаемых значений на выходах R-элементов для изображений.
        /// </summary>
        private void SetClassVector(ref IList<ImageData> images)
        {
            foreach (ImageData image in images)
            {
                image.ClassVector = new byte[Perceptron.RCount];

                // iamge.Class - это индекс активного R-элемента
                // (на этой позиции будет 1, на остальных - 0).
                if (Perceptron.RCount > image.Class)
                {
                    image.ClassVector[image.Class] = 1;
                }
            }
        }
    }
}
