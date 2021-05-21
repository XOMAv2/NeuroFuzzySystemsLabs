using System.Collections.Generic;

namespace Neuro.Models.Interaces
{
    interface IPerceptron
    {
        /// <summary> Количество сенсорных элементов. </summary>
        int SCount { get; }

        /// <summary> Количество ассоциативных элементов. </summary>
        int ACount { get; }

        /// <summary> Количество реагирующих элементов. </summary>
        int RCount { get; }

        /// <summary> Распознавание образа. </summary>
        /// <param name="inputSignals"> Сигналы на входах перцептрона. </param>
        /// <returns> Выходы R-элементов. </returns>
        IList<byte> Recognize(IList<byte> inputSignals);

        /// <summary> Распознавание образа. </summary>
        /// <param name="inputSignals"> Сигналы на входах перцептрона. </param>
        /// <param name="aTransfers"> Значения на выходах A-элементов перцоптрона, на который подали inputSignals. </param>
        /// <param name="isATransfersIsSet"> Если true, то считается, что переменная aTransfers уже вычислена.  </param>
        IList<byte> Recognize(IList<byte> inputSignals, IList<byte> aTransfers);

        /// <summary>
        /// Получение значений на выходах A-элементов перцептрона,
        /// на который подали inputSignals.
        /// </summary>
        /// <param name="inputSignals"> Сигналы на входах перцептрона. </param>
        IList<byte> GetATransfers(IList<byte> inputSignals);

        ///// <summary> Инициализация начальных весов. </summary>
        //void InitWeights(int max);

        /// <summary> Обучение перцептрона. </summary>
        /// <param name="inputSignals"> Сигналы на входах перцептрона. </param>
        /// <param name="outputSignals"> Выходы R-элементов. </param>
        /// <param name="aTransfers"> Значения на выходах A-элементов перцоптрона, на который подали inputSignals. </param>
        public void Teach(IList<byte> inputSignals, IList<byte> outputSignals, IList<byte> aTransfers);
    }
}
