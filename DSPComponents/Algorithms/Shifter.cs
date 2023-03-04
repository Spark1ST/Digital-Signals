using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            // i have a value postive for advance negative for delay
            // i have samples as values and indices as origin and shit
            //ShiftingValue;
            List<int> indices = new List<int>();
            List<float> samples = new List<float>();
            Folder f = new Folder();
            bool isFolded = f.isFolded(InputSignal);
            if (isFolded==false) // law not folded
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    indices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                    samples.Add(InputSignal.Samples[i]);
                    
                }
                OutputShiftedSignal = new Signal(samples, indices, InputSignal.Periodic);
            }
            else //law folded
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    indices.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                    samples.Add(InputSignal.Samples[i]);
                    
                }
                OutputShiftedSignal = new Signal(samples, indices, InputSignal.Periodic);
                f.addShiftedFolded(OutputShiftedSignal);
            }
       
        }
    }
}
