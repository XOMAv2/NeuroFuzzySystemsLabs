using Neuro.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neuro.Models
{
    [Serializable]
    class Perceptron : IPerceptron
    {
        public int EntryCount { get; }

        public int LayerCount { get; }

        public IList<ILayer> Layers { get; }

        public ITransferFunction TransferFunction { get; }

        /// <param name="entryCount"> Количество входов перцептрона. </param>
        /// <param name="sizes">
        /// Количества нейронов на слоях перцептрона от сенсорного к реагирующему.
        /// </param>
        public Perceptron(int entryCount, IList<int> sizes)
        {
            if (sizes == null || sizes.Count == 0) throw new Exception();
            if (entryCount < 1) throw new Exception();

            EntryCount = entryCount;
            LayerCount = sizes.Count;
            Layers = new Layer[LayerCount];
            TransferFunction = new LogisticTransferFunction();

            for (int i = 0; i < LayerCount; i++)
            {
                Neuron[] neurons = new Neuron[sizes[i]];

                for (int j = 0; j < sizes[i]; j++)
                {
                    neurons[j] = new Neuron(i == 0 ? EntryCount : sizes[i - 1], TransferFunction);
                }

                Layers[i] = new Layer(sizes[i], neurons);
            }
        }

        public IList<float> Recognize(IList<float> perceptronInputs)
        {
            if (perceptronInputs.Count != EntryCount) throw new Exception();

            IList<float> inputs = perceptronInputs;
            IList<float> outputs;

            for (int i = 0; i < LayerCount; i++)
            {
                outputs = new float[Layers[i].Size];
                Parallel.For(0, Layers[i].Size, j => outputs[j] = Layers[i].Neurons[j].Transfer(inputs));
                inputs = outputs;
            }

            return inputs;
        }

        ///// <param name="alpha"> 
        ///// Коэффициент инерциальности для сглаживания резких скачков при
        ///// перемещении по поверхности целевой функции.
        ///// </param>
        public void Backpropagation(IList<float> perceptronInputs, IList<float> targetOutputs, float learningRate = 1/*, float alpha = 0*/)
        {
            Recognize(perceptronInputs);

            // Ошибки.
            float[][] deltas = new float[LayerCount][];

            for (int i = 0; i < LayerCount; i++)
            {
                deltas[i] = new float[Layers[i].Size];
            }

            Parallel.For(0, Layers[LayerCount - 1].Size, i =>
            {
                float output = Layers[LayerCount - 1].Neurons[i].LastOutput;
                deltas[LayerCount - 1][i] = TransferFunction.Derivative(output) * (targetOutputs[i] - output);
            });

            for (int i = LayerCount - 2; i >= 0; i--)
            {
                Parallel.For(0, Layers[i].Size, j =>
                {
                    // Сумма ошибок от нейронов в последующем слое.
                    float sum = 0;

                    for (int k = 0; k < Layers[i + 1].Size; k++)
                    {
                        sum += deltas[i + 1][k] * Layers[i + 1].Neurons[k].Weights[j];
                    }

                    deltas[i][j] = TransferFunction.Derivative(Layers[i].Neurons[j].LastOutput) * sum;
                });
            }

            for (int i = 0; i < LayerCount; i++)
            {
                Parallel.For(0, Layers[i].Size, j =>
                {
                    // Изменение весов связей.
                    if (i == 0)
                    {
                        for (int k = 0; k < perceptronInputs.Count; k++)
                        {
                            float weightDelta = learningRate * deltas[i][j] * perceptronInputs[k];
                            Layers[i].Neurons[j].Weights[k] += weightDelta;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < Layers[i - 1].Size; k++)
                        {
                            float weightDelta = learningRate * deltas[i][j] * Layers[i - 1].Neurons[k].LastOutput;
                            Layers[i].Neurons[j].Weights[k] += weightDelta;
                        }
                    }

                    // Корректировки смещения.
                    Layers[i].Neurons[j].Biases += learningRate * deltas[i][j];
                });
            }
        }
    }
}
