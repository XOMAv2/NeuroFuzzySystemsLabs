using Neuro.Models.Interaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuro.Models
{
    [Serializable]
    class AElement : INeuron
    {
        public int EntryCount { get; }
        public IList<double> Weights { get; private set; }
        public double Limit { get; }

        public AElement(int entryCount, double limit = 50)
        {
            EntryCount = entryCount;
            InitWeights();
            Limit = limit;
        }

        public void ChangeWeights(int v, int delta, IList<byte> inputSignals) { }

        /// <summary>
        /// Инициализирует веса сигналов на входах значениями из диапазона { -1, 0, 1 }.
        /// </summary>
        public void InitWeights()
        {
            Random rand = new Random();
            Weights = new double[EntryCount];

            for (int i = 0; i < EntryCount; i++)
            {
                Weights[i] = rand.Next(-1, 2);
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

            byte result = Activator(Adder(inputSignals));
            OnTransfer?.Invoke(result);

            return result;
        }

        public event Action<byte> OnTransfer;

        /// <summary> Взвешенный сумматор. </summary> 
        /// <param name="inputSignals"> Сигналы на входах нейрона. </param> 
        private double Adder(IEnumerable<byte> inputSignals) =>
            inputSignals.Select((signal, i) => signal * Weights[i]).Sum();

        /// <summary>
        /// Функция активации, в данном случае - жесткая пороговая функция,
        /// имеющая область значений {0; 1}.
        /// </summary>
        /// <param name="sum"> Взвешенная сумма. </param>
        private byte Activator(double sum) => sum >= Limit ? (byte)1 : (byte)0;
    }
}
