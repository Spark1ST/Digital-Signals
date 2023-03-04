using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int countSum = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            for (int i = 0; i < countSum; i++)
            {
                if (i >= InputSignal1.Samples.Count)
                {
                    InputSignal1.Samples.Add(0);
                }

                if (i >= InputSignal2.Samples.Count)
                {

                    InputSignal2.Samples.Add(0);
                }
            }
            //============================================
            //dft
            DiscreteFourierTransform DFTsignal1 = new DiscreteFourierTransform();
            DFTsignal1.InputTimeDomainSignal = new Signal(InputSignal1.Samples, InputSignal1.Periodic);
            DFTsignal1.Run();
            DiscreteFourierTransform DFTsignal2 = new DiscreteFourierTransform();
         
            DFTsignal2.InputTimeDomainSignal = new Signal(InputSignal2.Samples, InputSignal1.Periodic);
            DFTsignal2.Run();
            //============================================
            List<float> Amp = new List<float>();
            List<float> Phase = new List<float>();
            //============================================
            //multply
            for (int i = 0; i < DFTsignal2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex complex1 = new Complex(DFTsignal2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(DFTsignal2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]), DFTsignal2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(DFTsignal2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                Complex complex2 = new Complex(DFTsignal1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(DFTsignal1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]), DFTsignal1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(DFTsignal1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                complex1 = complex1 * complex2;
                Amp.Add((float)complex1.Magnitude);
                Phase.Add((float)(Math.Atan2(complex1.Imaginary, complex1.Real)));
            }
            //============================================
            //idft
            InverseDiscreteFourierTransform IDFTsignal1 = new InverseDiscreteFourierTransform();
            OutputConvolvedSignal = new Signal(new List<float>(), new List<int>(), false);
            IDFTsignal1.InputFreqDomainSignal = new Signal(false, Amp, Amp, Phase);
            IDFTsignal1.Run();
            //============================================

            for (int i = 0; i < IDFTsignal1.OutputTimeDomainSignal.Samples.Count(); i++)
            {
                OutputConvolvedSignal.Samples.Add(IDFTsignal1.OutputTimeDomainSignal.Samples[i]);
            }
            //============================================


        }
    }
}