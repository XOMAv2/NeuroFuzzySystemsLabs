using System.Collections.Generic;

namespace Neuro.Models.Interaces
{
    interface ITeacher
    {
        /// <summary> Перцептрон. </summary>
        IPerceptron Perceptron { get; }

        /// <summary>
        /// Точности перцептрона после каждой обучающей итерации.
        /// </summary>
        IList<double> PrecisionLog { get; }

        /// <summary>
        /// Очистка лога с точностями.
        /// </summary>
        void ClearPrecisionLog();

        /// <summary>
        /// Рассчёт точности перцептрона в текущем состоянии.
        /// </summary>
        /// <param name="testImages"> Тестовая выборка. </param>
        /// <param name="aTransfers"> Значения на выходах A-элементов перцоптрона. </param>
        /// <returns></returns>
        double GetCurrentPrecision(IList<ImageData> testImages, IList<IList<byte>> aTransfers);

        /// <summary> Обучение перцептрона. </summary>
        /// <param name="teachImages"> Образы для обучения. </param>
        /// <param name="n"> Количество циклов обучения. </param>
        void Teach(IList<ImageData> teachImages, int n);

        /// <summary> Обучение перцептрона. </summary>
        /// <param name="teachImages"> Обучающая выборка. </param>
        /// <param name="testImages"> Тестовая выборка. </param>
        /// <param name="n"> Количество циклов обучения. </param>
        void Teach(IList<ImageData> teachImages, IList<ImageData> testImages, int n);
    }
}
