using Neuro.Helpers;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neuro.ViewModels
{
    class PlotViewModel : Observable
    {
        private string title;
        private IList<DataPoint> points;
        private IList<float> precision;

        public PlotViewModel()
        {
            Title = "Изменение точности распознавания в процессе обучения по эпохам";
            Precision = new List<float>();
        }

        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        public IList<DataPoint> Points
        {
            get => points;
            set => Set(ref points, value);
        }

        public IList<float> Precision
        {
            get => precision;
            set
            {
                precision = value;
                List<DataPoint> dots = new List<DataPoint>();

                for (int i = 0; i < precision.Count; i++)
                {
                    dots.Add(new DataPoint(i + 1, precision[i]));
                }

                Points = dots;
            }
        }
    }
}
