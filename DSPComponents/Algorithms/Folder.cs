using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }
        static List<Signal> foldedSignal=new List<Signal>();
        public bool isFolded(Signal inputSig) 
        {
            for (int i = 0; i < foldedSignal.Count; i++)
            {
                if (foldedSignal[i] == inputSig) 
                {
                    return true;
                }       
            }
            return false;
        }
        public void addShiftedFolded(Signal inpSignal)
        {
            foldedSignal.Add(inpSignal);
        }
        public override void Run()
        {
            Shifter s = new Shifter();
            List<float> sampls= new List<float>();
            List<int> indices= new List<int>();
           
            for (int i = 0; i < InputSignal.Samples.Count; i++) 
            {

                sampls.Add(InputSignal.Samples[InputSignal.Samples.Count - 1 - i]); //values of samples based on indices 
                indices.Add(-1 * (InputSignal.SamplesIndices[InputSignal.Samples.Count - 1 - i])); //values of samples based on indices 
                
              // OutputFoldedSignal.SamplesIndices[i]=InputSignal.SamplesIndices[InputSignal.Samples.Count-1-i];   //values of samples based on indices               
            }
            Signal sig = new Signal(sampls, indices, InputSignal.Periodic);
           
            if (isFolded(InputSignal) == true||isFolded(sig)==true)
            {

                foldedSignal.Remove(InputSignal);
                foldedSignal.Remove(sig);
            }
            else
            {
                foldedSignal.Add(InputSignal);
                foldedSignal.Add(sig);
            }
            OutputFoldedSignal = sig;

        }
    }
}
