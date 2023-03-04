using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
      
        public override void Run()
        {
            List<float> sum = new List<float>();
            List<int> indices = new List<int>();
            float accSum = 0;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                accSum += InputSignal.Samples[i];
                sum.Add(accSum);
            }
            OutputSignal = new Signal(sum,false);
        }
    }
}
