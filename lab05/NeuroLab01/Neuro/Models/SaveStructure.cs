using Neuro.Models.Interaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.Models
{
    [Serializable]
    class SaveStructure
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ACount { get; set; }
        public int RCount { get; set; }
        public IPerceptron Perceptron { get; set; }
    }
}
