using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> result = new List<float>();

            float Xmin = InputSignal.Samples[0], Xmax = InputSignal.Samples[0];

            // Loop to get min. and max. values in sample
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] < Xmin)
                {
                    Xmin = InputSignal.Samples[i];
                }
                if (InputSignal.Samples[i] > Xmax)
                {
                    Xmax = InputSignal.Samples[i];
                }
            }

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float normalized = (InputSignal.Samples[i] - Xmin) / (Xmax - Xmin);
                float normalizedWithinARange = normalized * (InputMaxRange - InputMinRange) + InputMinRange;
                result.Add(normalizedWithinARange);
            }
            OutputNormalizedSignal = new Signal(result, false);
        }
    }
}