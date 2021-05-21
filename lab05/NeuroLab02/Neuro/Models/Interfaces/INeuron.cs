using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuro.Models.Interfaces
{
    interface INeuron
    {
        /// <summary> Количество входов нейрона. </summary>
        int EntryCount { get; }

        /// <summary> Веса сигналов на входах нейрона. </summary>
        IList<float> Weights { get; set; }

        /// <summary>
        /// Смещение активационной функции.
        /// Грубо говоря, вес дополнительного единичного входного сигнала.
        /// </summary>
        float Biases { get; set; }

        /// <summary> Передаточная функция. </summary>
        ITransferFunction TransferFunction { get; }

        /// <summary>
        /// Значение последнего выходного сигнала нейрона.
        /// </summary>
        float LastOutput { get; }

        /// <summary>
        /// Инициализирует веса сигналов на входах значениями из диапазона от fromInclusive до toExclusive.
        /// </summary>
        /// <param name="fromInclusive"> Нижняя включённая граница. </param>
        /// <param name="toExclusive"> Верхняя исключённая граница. </param>
        void InitWeights(float fromInclusive, float toExclusive);

        /// <summary> Передаточная функция. </summary>
        /// <param name="neuronInputs"> Сигналы на входах нейрона. </param>
        /// <returns> Значение выходного сигнала нейрона. </returns>
        float Transfer(IList<float> neuronInputs);

        event Action<float> OnTransfer;
    }
}
