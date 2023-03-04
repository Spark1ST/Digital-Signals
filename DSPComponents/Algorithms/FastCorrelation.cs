using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        public float normalizationFactorCalc()
        {
            //calc normalization
            //law = 1/N * sqrt((sum1^2)(sum2^2))
            float sum1 = 0;
            float sum2 = 0;
            float normalization_factor = 0.0f;

            for (int l = 0; l < InputSignal1.Samples.Count; l++)
            {
                sum1 += (float)Math.Pow(InputSignal1.Samples[l], 2);
            }
            for (int s2 = 0; s2 < InputSignal1.Samples.Count; s2++)
            {
                sum2 += (float)Math.Pow(InputSignal2.Samples[s2], 2);
            }

            normalization_factor = (float)Math.Sqrt(sum1 * sum2);
            normalization_factor /= InputSignal1.Samples.Count;
            return normalization_factor;
        }
        public override void Run()
        {
            //law signal2 = null 5leha = sig1 
            InputSignal2 = (InputSignal2 == null) ? InputSignal1 : InputSignal2;
            //=============================================
            //dft -> multiply * conj -> hat el conj ->idft
            //============================================
            //dft
            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal = new Signal(InputSignal1.Samples, InputSignal1.Periodic);
            DFT1.Run();
            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            DFT2.InputTimeDomainSignal = new Signal(InputSignal2.Samples, InputSignal1.Periodic);
            DFT2.Run();
            Signal DFTsignal1 = DFT1.OutputFreqDomainSignal;
            Signal DFTsignal2 = DFT2.OutputFreqDomainSignal;
            //============================================

            List<float> Amp = new List<float>();
            List<float> Phase = new List<float>();
            /*for (int z = 0; z < InputSignal1.Samples.Count; z++) 
            {
                float real = DFTsignal1.getRealVal()[z] * DFTsignal2.getRealVal()[z];
                float img = DFTsignal1.getImagVal()[z]*-DFTsignal2.getRealVal()[z];
                Amp.Add(real);
                Phase.Add(img);
            }*/

            for (int i = 0; i < DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex complex1 = new Complex(DFTsignal1.FrequenciesAmplitudes[i] * (float)Math.Cos(DFTsignal1.FrequenciesPhaseShifts[i]), DFTsignal1.FrequenciesAmplitudes[i] * (float)Math.Sin(DFTsignal1.FrequenciesPhaseShifts[i]));
                Complex complex2 = new Complex(DFTsignal2.FrequenciesAmplitudes[i] * (float)Math.Cos(DFTsignal2.FrequenciesPhaseShifts[i]), DFTsignal2.FrequenciesAmplitudes[i] * (float)Math.Sin(DFTsignal2.FrequenciesPhaseShifts[i]));
                complex1 = Complex.Multiply(complex1, Complex.Conjugate(complex2));
                Complex complex3 = Complex.Conjugate(complex1);
                Amp.Add((float)complex3.Magnitude);
                Phase.Add((float)(Math.Atan2(complex3.Imaginary, complex3.Real)));
            }
            //============================================
            //idft
            InverseDiscreteFourierTransform IDFTsignal1 = new InverseDiscreteFourierTransform();
            IDFTsignal1.InputFreqDomainSignal = new Signal(false, Amp, Amp, Phase);
            IDFTsignal1.Run();
            //============================================
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            float normFac = normalizationFactorCalc();
            for (int i = 0; i < IDFTsignal1.OutputTimeDomainSignal.Samples.Count(); i++)
            {
                OutputNonNormalizedCorrelation.Add(IDFTsignal1.OutputTimeDomainSignal.Samples[i]/ IDFTsignal1.OutputTimeDomainSignal.Samples.Count());
                OutputNormalizedCorrelation.Add(IDFTsignal1.OutputTimeDomainSignal.Samples[i] / IDFTsignal1.OutputTimeDomainSignal.Samples.Count() / normFac);
            }
        }
    }
}