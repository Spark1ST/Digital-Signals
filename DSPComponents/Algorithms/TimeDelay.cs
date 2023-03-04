using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //===========================================
            /*
               1) Calculate correlation
               2) find the max absolute value
               3) save its lag (j)
               4) Time delay = j * Ts
            */
            
            //===========================================
            DirectCorrelation fc = new DirectCorrelation();
            fc.InputSignal1 = InputSignal1;
            fc.InputSignal2 = InputSignal2;
            fc.Run();
            float max = 0;
            int index = 0;
            for (int i =0; i< fc.OutputNonNormalizedCorrelation.Count; i++) 
            {
                if (Math.Abs(fc.OutputNonNormalizedCorrelation[i]) > max) 
                {
                    max = Math.Abs(fc.OutputNonNormalizedCorrelation[i]);
                    index = i;
                }
            }
            OutputTimeDelay = index * InputSamplingPeriod;

                
        }
    }
}
