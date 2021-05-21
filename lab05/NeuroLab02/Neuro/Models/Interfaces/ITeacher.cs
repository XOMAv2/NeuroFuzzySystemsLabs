using System;
using System.Collections.Generic;

namespace Neuro.Models.Interfaces
{
    interface ITeacher
    {
        /// <summary> Перцептрон. </summary>
        IPerceptron Perceptron { get; }

        /// <summary>
        /// Точности перцептрона после каждой обучающей итерации.
        /// </summary>
        IList<float> PrecisionLog { get; }

        /// <summary>
        /// Очистка лога с точностями.
        /// </summary>
        void ClearPrecisionLog();

        /// <summary>
        /// Рассчёт точности перцептрона в текущем состоянии.
        /// </summary>
        /// <param name="testImages"> Тестовая выборка. </param>
        /// <param name="eps">
        /// Отклонение от максимального значения выхода перцептрона,
        /// то есть единицы, при котором будет засчитано распознавание.
        /// </param>
        float GetCurrentPrecision(IList<ImageData> testImages, float eps = 0.1f);

        /// <summary> Обучение перцептрона. </summary>
        /// <param name="teachImages"> Образы для обучения. </param>
        /// <param name="eraCount"> Количество циклов обучения. </param>
        /// <param name="batchSize">
        /// Количество обучающих образов,
        /// которое будет пропущено через сеть за одну эпоху.
        /// </param>
        /// <returns>
        /// Возвращает пару (Номер обучающего изображения, номер эры)
        /// с помощью оператора yield return.
        /// </returns>
        IEnumerable<Tuple<int, int>> Teach(IList<ImageData> teachImages, int eraCount, uint? batchSize = null);

        /// <summary> Обучение перцептрона. </summary>
        /// <param name="teachImages"> Обучающая выборка. </param>
        /// <param name="testImages"> Тестовая выборка. </param>
        /// <param name="eraCount"> Количество циклов обучения. </param>
        /// <param name="batchSize">
        /// Количество обучающих образов,
        /// которое будет пропущено через сеть за одну эпоху.
        /// </param>
        /// <returns>
        /// Возвращает пару (Номер обучающего изображения, номер эры)
        /// с помощью оператора yield return.
        /// </returns>
        IEnumerable<Tuple<int, int>> Teach(IList<ImageData> teachImages, IList<ImageData> testImages, int eraCount, uint? batchSize = null);
    }
}
