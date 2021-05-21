using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Models.Interfaces
{
    interface ITransferFunction
    {
        /// <summary>
        /// Функция активации.
        /// </summary>
        /// <param name="sum"> Взвешенная сумма. </param>
        float Activator(float sum);

        /// <summary> Производная функции активации нейронов. </summary>
        float Derivative(float x);
    }
}
