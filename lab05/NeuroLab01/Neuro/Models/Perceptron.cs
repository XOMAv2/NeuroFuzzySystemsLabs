using Neuro.Models.Interaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neuro.Models
{
    /// <summary>
    /// Перцептрон с одним скрытым слоем.
    /// </summary>
    [Serializable]
    class Perceptron : IPerceptron
    {
        public AElement[] AElements { get; }
        public RElement[] RElements { get; }

        public int SCount { get; }
        public int ACount { get; }
        public int RCount { get; }

        /// <param name="sCount"> Количество S-Элементов. </param>
        /// <param name="aCount"> Количество A-элементов. </param>
        public Perceptron(int sCount, int aCount, int rCount)
        {
            SCount = sCount;

            ACount = aCount;
            AElements = new AElement[aCount];

            for (int i = 0; i < aCount; i++)
            {
                AElements[i] = new AElement(sCount);
            }

            RCount = rCount;
            RElements = new RElement[rCount];

            for (int i = 0; i < rCount; i++)
            {
                RElements[i] = new RElement(aCount);
            }
        }

        public IList<byte> Recognize(IList<byte> inputSignals)
        {
            if (inputSignals.Count != SCount) throw new Exception();

            byte[] aTransfers = new byte[ACount];
            Parallel.For(0, ACount, i => aTransfers[i] = AElements[i].Transfer(inputSignals));

            byte[] rTransfers = new byte[RCount];
            Parallel.For(0, RCount, i => rTransfers[i] = RElements[i].Transfer(aTransfers));

            return rTransfers;
        }

        public IList<byte> Recognize(IList<byte> inputSignals, IList<byte> aTransfers)
        {
            if (inputSignals.Count != SCount) throw new Exception();
            if (aTransfers.Count != ACount) throw new Exception();

            byte[] rTransfers = new byte[RCount];
            Parallel.For(0, RCount, i => rTransfers[i] = RElements[i].Transfer(aTransfers));

            return rTransfers;
        }

        public IList<byte> GetATransfers(IList<byte> inputSignals)
        {
            if (inputSignals.Count != SCount) throw new Exception();

            byte[] aTransfers = new byte[ACount];
            Parallel.For(0, ACount, i => aTransfers[i] = AElements[i].Transfer(inputSignals));

            return aTransfers;
        }

        //public void InitWeights(int max)
        //{
        //    foreach (var neuron in _neurons)
        //        neuron.InitWeights(max);
        //}

        public void Teach(IList<byte> inputSignals, IList<byte> outputSignals, IList<byte> aTransfers)
        {
            const int v = 1; // Скорость обучения.
            IList<byte> t = Recognize(inputSignals, aTransfers);

            Parallel.For(0, RCount, i =>
            {
                int delta = outputSignals[i] - t[i];
                RElements[i].ChangeWeights(v, delta, aTransfers);
            });
        }
    }
}
