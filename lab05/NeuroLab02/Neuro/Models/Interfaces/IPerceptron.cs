using System;
using System.Collections.Generic;

namespace Neuro.Models.Interfaces
{
    interface IPerceptron
    {
        /// <summary> Количество входов перцептрона. </summary>
        int EntryCount { get; }

        /// <summary> Количество слоёв перцептрона. </summary>
        int LayerCount { get; }

        /// <summary> Слои перцептрона от сенсорного к реагирующему. </summary>
        IList<ILayer> Layers { get; }

        /// <summary> Передаточная функция. </summary>
        ITransferFunction TransferFunction { get; }

        /// <summary> Распознавание образа. </summary>
        /// <param name="perceptronInputs"> Сигналы на входах перцептрона. </param>
        /// <returns> Сигналы на выходах перцептрона. </returns>
        IList<float> Recognize(IList<float> perceptronInputs);

        /// <summary>
        /// Единоразовая корректировка весов перцептрона
        /// методом обратного распространения ошибки.
        /// </summary>
        /// <param name="perceptronInputs"> Сигналы на входах перцептрона. </param>
        /// <param name="targetOutputs"> Целевые значения на выходах перцептрона. </param>
        /// <param name="learningRate"> Скорость обучения. </param>
        void Backpropagation(IList<float> perceptronInputs, IList<float> targetOutputs, float learningRate = 1);
    }
}
