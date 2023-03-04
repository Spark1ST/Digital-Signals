using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
public struct Compl {
  public  float imagin;
    public float real;
    public Compl(float r, float i)
    {
        imagin = i;
        real = r;
    }
  
}
namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

   
        public override void Run()
        {
            int N = InputTimeDomainSignal.Samples.Count;//number
            List<float> freq = new List<float>();
            List<float> AmplitudeX = new List<float>();//FrequenciesAmplitudes
            List<float> phaseShiftTheta = new List<float>();//phase shifts
            //x(n)=1/N * summ from 0 to k= N-1  x(K) * E^2*pi*n*k /N
            for (int n = 0; n < N; n++)
            {
                Compl Summ=new Compl(0,0);
                for (int k = 0; k < N; k++)
                {
                    Summ.real = Summ.real + InputTimeDomainSignal.Samples[k] * (float)Math.Cos((2 * k * Math.PI * n) / N);
                    Summ.imagin = Summ.imagin + InputTimeDomainSignal.Samples[k] * (float)Math.Sin((2 * k * Math.PI * n) / N) * -1;
                    //frequencies btb2a ely gwa el sin aw el cos ely hya 2Pie f n 
                }
                AmplitudeX.Add((float)Math.Sqrt(Math.Pow(Summ.imagin, 2) + Math.Pow(Summ.real, 2)));
                phaseShiftTheta.Add((float)Math.Atan2(Summ.imagin, Summ.real));             
                float fr = n *(2*(float)Math.PI*InputSamplingFrequency)/N;
                fr = (float)Math.Round(fr,1);
                freq.Add(fr);
            }
            //======================================
            OutputFreqDomainSignal = new Signal(false, freq, AmplitudeX, phaseShiftTheta);
        }
    }
}
