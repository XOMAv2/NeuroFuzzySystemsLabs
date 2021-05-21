using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Models.Interfaces
{
    interface ILayer
    {
        /// <summary> Количество нейронов на слое. </summary>
        int Size { get; }

        /// <summary> Нейроны слоя. </summary>
        IList<INeuron> Neurons { get; }
    }
}
