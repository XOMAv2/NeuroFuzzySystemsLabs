using Neuro.Models.Interaces;
using System;
using System.Collections.Generic;

namespace Neuro.Models
{
    [Serializable]
    class SElement : INeuron
    {
        public int EntryCount { get; }
        public IList<double> Weights { get; private set; }
        public double Limit { get; }

        public SElement()
        {
            EntryCount = 1;
            Limit = 1;
            InitWeights();
        }

        public void ChangeWeights(int v, int delta, IList<byte> inputSignals) { }

        /// <summary>
        /// Инициализирует вес сигнала на входе нейрона единицей.
        /// </summary>
        public void InitWeights()
        {
            Weights = new double[] { 1 };
        }

        public byte Transfer(IList<byte> inputSignals)
        {
            if (inputSignals.Count != 1) throw new Exception();

            byte result = inputSignals[0] >= 1 ? (byte)1 : (byte)0;
            OnTransfer?.Invoke(result);

            return result;
        }

        public event Action<byte> OnTransfer;
    }
}
