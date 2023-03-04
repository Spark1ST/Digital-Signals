using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        private float normalization() 
        {
            float normalizationFactor;
            //=============================================================
            //normalization factor
            float sum1 = 0;
            float sum2 = 0;

            for (int l = 0; l < InputSignal1.Samples.Count; l++)
            {
                sum1 += (float)Math.Pow(InputSignal1.Samples[l], 2);
            }
            for (int s2 = 0; s2 < InputSignal1.Samples.Count; s2++)
            {
                sum2 += (float)Math.Pow(InputSignal2.Samples[s2], 2);
            }

            normalizationFactor = (float)Math.Sqrt(sum1 * sum2);
            normalizationFactor /= Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count);
            return normalizationFactor;
        }
        public override void Run()
        {
            List<float> outputNON = new List<float>();
            List<float> output = new List<float>();
            float normalization_factor = 0.0f;
            float sum = 0.0f;
            //==================================
            // THE RULE
            //r(i)=(1/N)*SUM[x(n)*x(n+j)]
            //==================================
            InputSignal2 = (InputSignal2 == null) ? InputSignal1 : InputSignal2;
            normalization_factor = normalization();
            //=============================================================
            for (int i = 0; i < Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count); i++)
            {
                sum = 0.0f;
                for (int j = 0; j < Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count); j++)
                {
                    //===================================================================================
                    int ij = i + j;
                    if (InputSignal1.Periodic == true || InputSignal2.Periodic == true)
                    { 
                        ij = (i + j) % InputSignal2.Samples.Count;//el 3azma 34an y3ml cycle 
                    }
                        //===================================================================================
                        if (ij < InputSignal2.Samples.Count && j < InputSignal1.Samples.Count)//if el 3zma variable less than count 34an mygb4 out of bond or sth
                        {
                            sum += (InputSignal1.Samples[j] * InputSignal2.Samples[ij]);
                        }
                    }
                    sum /= Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count);
                    outputNON.Add(sum);
                    output.Add(sum / normalization_factor);
                }
            OutputNormalizedCorrelation = output;
            OutputNonNormalizedCorrelation = outputNON;
        }
    }
}