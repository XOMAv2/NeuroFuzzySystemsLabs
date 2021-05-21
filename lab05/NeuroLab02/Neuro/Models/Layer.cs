using Neuro.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Neuro.Models
{
    [Serializable]
    class Layer : ILayer
    {
        public int Size { get; }

        public IList<INeuron> Neurons { get; }

        /// <param name="size"> Количество нейронов на слое. </param>
        /// <param name="neurons"> Нейроны слоя. </param>
        public Layer(int size, IList<INeuron> neurons) => (Size, Neurons) = (size, neurons);
    }
}
