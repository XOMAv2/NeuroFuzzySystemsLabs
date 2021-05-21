using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Helpers
{
    public static class RandomExtension
    {
        /// <summary>
        /// Возвращает случайное число с плавающей точкой из диапазона от fromInclusive до toExclusive.
        /// </summary>
        /// <param name="fromInclusive"> Нижняя включённая граница. </param>
        /// <param name="toExclusive"> Верхняя исключённая граница. </param>
        public static double NextDouble(this Random rand, double fromInclusive, double toExclusive)
        {
            if (toExclusive <= fromInclusive)
            {
                throw new Exception("Значение toExclusive должно быть строго больше fromInclusive");
            }

            double delta = toExclusive - fromInclusive;

            return rand.NextDouble() * delta + fromInclusive;
        }
    }
}
