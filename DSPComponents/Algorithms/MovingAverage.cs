using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> sumAvg = new List<float>();
            for (int i = (InputWindowSize-1); i < InputSignal.Samples.Count; i++) 
            {
                float sum = 0;
                for (int l = i-(InputWindowSize-1); l < i+1; l++)
                {
                    sum += InputSignal.Samples[l];
                }
                sumAvg.Add(sum/InputWindowSize);
            }
            OutputAverageSignal = new Signal(sumAvg, false);
        }
    }
}
