using Neuro.Helpers;
using Neuro.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neuro.Models
{
    [Serializable]
    class Neuron : INeuron
    {
        public int EntryCount { get; }

        private IList<float> weights;
        public IList<float> Weights
        {
            get => weights;
            set => weights = value.Count == EntryCount
                ? value
                : throw new Exception("Число весов не равно числу входов нейрона");
        }

        public float Biases { get; set; }

        public ITransferFunction TransferFunction { get; }

        public float LastOutput { get; private set; }

        /// <param name="entryCount"> Количество входов нейрона. </param>
        /// <param name="biases"> Смещение активационной функции. </param>
        /// <param name="transferFunction"> Передаточная функция. </param>
        public Neuron(int entryCount, ITransferFunction transferFunction)
            : this(entryCount, -1, 1, transferFunction) { }

        /// <param name="entryCount"> Количество входов нейрона. </param>
        /// <param name="fromInclusive">
        /// Нижняя включённая граница значений весов и смещения активационной функции.
        /// </param>
        /// <param name="toExclusive">
        /// Верхняя исключённая граница значений весов и смещения активационной функции.
        /// </param>
        /// <param name="transferFunction"> Передаточная функция. </param>
        public Neuron(int entryCount, float fromInclusive, float toExclusive, ITransferFunction transferFunction)
        {
            EntryCount = entryCount;
            InitWeights(fromInclusive, toExclusive);
            Biases = (float)(new Random().NextDouble(fromInclusive, toExclusive));
            TransferFunction = transferFunction;
            LastOutput = 0;
        }

        public void InitWeights(float fromInclusive, float toExclusive)
        {
            Random rand = new Random();
            weights = new float[EntryCount];

            for (int i = 0; i < EntryCount; i++)
            {
                weights[i] = (float)rand.NextDouble(fromInclusive, toExclusive);
            }
        }

        /// <summary> Взвешенный сумматор. </summary> 
        /// <param name="neuronInputs"> Сигналы на входах нейрона. </param> 
        private float Adder(IEnumerable<float> neuronInputs)
        {
            return neuronInputs.Select((signal, i) => signal * weights[i]).Sum() + Biases;
        }

        public float Transfer(IList<float> neuronInputs)
        {
            if (neuronInputs.Count != EntryCount)
            {
                throw new Exception("Число входных сигналов не равно числу входов нейрона");
            }

            LastOutput = TransferFunction.Activator(Adder(neuronInputs));
            OnTransfer?.Invoke(LastOutput);

            return LastOutput;
        }

        public event Action<float> OnTransfer;
    }
}
