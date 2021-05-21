using System;
using System.Collections.Generic;

namespace Neuro.Models.Interaces
{
    interface INeuron
    {
        /// <summary> Количество входов нейрона. </summary>
        int EntryCount { get; }

        /// <summary> Веса сигналов на входах нейрона. </summary>
        IList<double> Weights { get; }

        /// <summary> Пороговое значение нейрона. </summary>
        double Limit { get; }

        /// <summary> Передаточная функция. </summary>
        /// <param name="inputSignals"> Сигналы на входах нейрона. </param>
        /// <returns> Значение выходного сигнала нейрона. </returns>
        byte Transfer(IList<byte> inputSignals);

        event Action<byte> OnTransfer;

        /// <summary> Инициализация весов сигналов на входе нейрона </summary>
        void InitWeights();

        /// <summary>
        /// Модификация весов сигналов на входе нейрона для обучения.
        /// </summary>
        /// <param name="v"> Скорость обучения. </param>
        /// <param name="delta"> Разница между реальным выходом нейрона и желаемым. </param>
        /// <param name="inputSignals"> Сигналы на входах нейрона. </param>
        void ChangeWeights(int v, int delta, IList<byte> inputSignals);
    }
}
