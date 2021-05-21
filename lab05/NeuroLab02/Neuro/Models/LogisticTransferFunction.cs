using Neuro.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Models
{
    [Serializable]
    class LogisticTransferFunction : ITransferFunction
    {
        public float Activator(float sum)
        {
            return (float)(1 / (1 + Math.Exp(-sum)));
        }

        public float Derivative(float x)
        {
            return x * (1 - x);
        }
    }
}
