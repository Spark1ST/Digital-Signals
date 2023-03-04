using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

using System.ComponentModel;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {


        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }
        public override void Run()
        {

            List<float> realOut = new List<float>(); 
            List<float> imgOut = new List<float>(); 
            int size = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            List<Compl> complexList = new List<Compl>();
            float x, y;
            Compl c;
            for (int i = 0; i < size; i++)
            {
                x = InputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                y = InputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                c = new Compl(x, y);
                complexList.Add(c);
            }
            for (int i = 0; i < size; i++)
            {
                Compl iteration = new Compl();
                Compl c2, temp;

                for (int j = 0; j < size; j++)
                {
                    x = y = 0;
                    c2 = new Compl((float)Math.Cos(i * 2 * Math.PI * j / size), (float)Math.Sin(i * 2 * Math.PI * j / size));
                    x = (float)(c2.real * complexList[j].real - c2.imagin * complexList[j].imagin);
                    y = (float)(c2.imagin * complexList[j].real + c2.real * complexList[j].imagin);
                    temp = new Compl(x, y);
                    iteration.real += temp.real;
                    iteration.imagin += temp.imagin;
                }
                iteration.real /= size;

                realOut.Add((float)iteration.real);
                imgOut.Add((float)iteration.imagin);
            }
            OutputTimeDomainSignal = new Signal(realOut, false);
        }
    }
}