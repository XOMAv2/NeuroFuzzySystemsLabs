using Neuro.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Models
{
    [Serializable]
    class ReLUTransferFunction : ITransferFunction
    {
        public float Activator(float sum) => sum < 0 ? 0 : sum;

        public float Derivative(float x) => x < 0 ? 0 : 1;
    }
}
