using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<int> ots = new List<int>();
            List<float> ots2 = new List<float>();
            float sum;
            for (int i = 0; i < InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1; i++)//i assumed en th output signal can't be more than this
            {
                sum = 0.0f;
                for (int l = 0; l < i + 1; l++) // lazem +1 34an feh conditions i need el l==i like i=0 && l==0  
                {

                    //=======================================================================================================
                    //lazem el x(l) mtb2a4 zero fel kanon we hna teb2a exists 
                    if (l < InputSignal1.Samples.Count && i - l < InputSignal2.Samples.Count)
                    {
                        sum += (float)InputSignal1.Samples[l] * InputSignal2.Samples[i - l];
                    }
                }
                //=======================================================================================================
                //validation eno wsl el 2bl el a5r be 1 we m4 bytl3 zero sums 34an feh 1 condition bt54 fel loob ely foo2
                if ((i >= InputSignal1.Samples.Count + InputSignal2.Samples.Count - 2) && sum == 0)
                {
                    continue;
                }
                //=======================================================================================================
                ots2.Add(sum);
            }
            
        
            //=====================da fe 7alet eno mfe4 indices ttzbt==================================
            if (InputSignal1.SamplesIndices == null || (InputSignal1.SamplesIndices[0] == 0 && InputSignal2.SamplesIndices[0] == 0))
            {
                OutputConvolvedSignal = new Signal(ots2, false);
            }
            //=========================================================================================
            else // feh index ytzbt
            {
                int min = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
                for (int i = 0; i <ots2.Count; i++) 
                {
                    ots.Add(min);//gebt el min index we kol mara bazwd 3leh wa7d 
                    min++;
                }
                OutputConvolvedSignal = new Signal(ots2, ots, false);
            }
            
        }

    }
}
