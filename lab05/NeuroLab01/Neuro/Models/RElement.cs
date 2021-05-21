using Neuro.Models.Interaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuro.Models
{
    [Serializable]
    class RElement : INeuron
    {
        public int EntryCount { get; }
        public IList<double> Weights { get; set; }
        public double Limit { get; }

        public RElement(int entryCount, double limit = 0)
        {
            EntryCount = entryCount;
            InitWeights();
            Limit = limit;
        }

        public void ChangeWeights(int v, int delta, IList<byte> inputSignals)
        {
            if (inputSignals.Count != EntryCount) throw new Exception();

            for (int i = 0; i < EntryCount; i++)
            {
                Weights[i] += v * delta * inputSignals[i];
            }
        }

        /// <summary>
        /// Инициализирует веса сигналов на входах значениями из интервала
        /// от -1 (включительно) до 1 (не включительно).
        /// </summary>
        public void InitWeights()
        {
            Random rand = new Random();
            Weights = new double[EntryCount];

            for (int i = 0; i < EntryCount; i++)
            {
                Weights[i] = rand.NextDouble() * 2 - 1;
            }
        }

        /// <summary>
        /// Инициализирует веса сигналов на входах значениями из диапазона от minValue до maxValue.
        /// </summary>
        /// <param name="minValue"> Нижняя включённая граница. </param>
        /// <param name="maxValue"> Верхняя исключённая граница. </param>
        public void InitWeights(double minValue, double maxValue)
        {
            if (maxValue <= minValue)
            {
                throw new Exception("Значение maxValue должно быть строго больше minValue");
            }

            Random rand = new Random();
            Weights = new double[EntryCount];
            double delta = maxValue - minValue;

            for (int i = 0; i < EntryCount; i++)
            {
                Weights[i] = rand.NextDouble() * delta + minValue;
            }
        }

        public byte Transfer(IList<byte> inputSignals)
        {
            if (inputSignals.Count != EntryCount) throw new Exception();

            byte result = Adder(inputSignals) >= Limit ? (byte)1 : (byte)0;
            OnTransfer?.Invoke(result);

            return result;
        }

        public event Action<byte> OnTransfer;

        /// <summary> Взвешенный сумматор. </summary> 
        /// <param name="inputSignals"> Сигналы на входах нейрона. </param> 
        private double Adder(IEnumerable<byte> inputSignals) =>
            inputSignals.Select((signal, i) => signal * Weights[i]).Sum();
    }
}
