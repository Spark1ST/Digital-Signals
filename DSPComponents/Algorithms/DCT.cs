using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //=============================================================
            //y(k)=root (2\N)*sum[x(n)*cos((pie/4N)*(2n-1)(2k-1))]
            //=============================================================
            List<float> outputs = new List<float>();
            for (int k = 0; k < InputSignal.Samples.Count; k++)
            {
                float sum = 0.0f;
                for (int n = 0; n < InputSignal.Samples.Count; n++) 
                {
                    sum += InputSignal.Samples[n] * (float)Math.Cos((float)(Math.PI / (4 * InputSignal.Samples.Count))*(2*n-1)*(2*k-1));
                }
                double d=2.0f/(InputSignal.Samples.Count); 
                sum=sum*(float)Math.Sqrt(d);
                outputs.Add(sum); 
            }
            OutputSignal=new Signal(outputs,false);
        }
    }
}
