using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> firstD = new List<float>();
            
            List<float> secondD = new List<float>();
            firstD.Add(InputSignal.Samples[0]);
            
            //Y(n)=x(n)-x(n-1)
            for (int i = 1; i < InputSignal.Samples.Count-1; i++)
            {
                firstD.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
                
                secondD.Add((-2 * i * InputSignal.Samples[i]) + (i * InputSignal.Samples[i - 1]) + (i * InputSignal.Samples[i + 1]));
                if (i == 1) {
                secondD.Add(0); //at i =0 Second d =0 0*samples[i-1]-2*0 * samples[i]+0*samples [i+1]
                    
                }
                
            }
            FirstDerivative = new Signal(firstD, false);
            SecondDerivative = new Signal(secondD, false);
        }
    }
}
