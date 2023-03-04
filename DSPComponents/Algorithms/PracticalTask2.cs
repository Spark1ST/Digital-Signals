﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }
        private void displaySignal(Signal inpSig,string signalPath) 
        {
            using (StreamWriter writer = new StreamWriter(signalPath))
            {
                bool isItFreq = false;
                //======================================
        
                if (inpSig.FrequenciesAmplitudes==null)
                {
                    writer.WriteLine(0);//0 for time 
                    isItFreq = false;

                }
                else if (inpSig.Samples==null)
                {
                    writer.WriteLine(1);//1 for freq
                    isItFreq = true;
                }
                //======================================
                if (inpSig.Periodic == true)
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }
                //======================================
                if (isItFreq == true)
                {
                    writer.WriteLine(inpSig.FrequenciesAmplitudes.Count);

                    for (int i = 0; i < inpSig.FrequenciesAmplitudes.Count; i++)
                    {
                        writer.Write(inpSig.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(inpSig.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.Write(inpSig.FrequenciesPhaseShifts[i]);
                        writer.Write("\n");
                    }

                }
                //======================================
                else
                {
                    writer.WriteLine(inpSig.Samples.Count);
                    for (int i = 0; i < inpSig.Samples.Count; i++)
                    {
                        writer.Write(inpSig.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.Write(inpSig.Samples[i]);
                        writer.Write("\n");
                    }
                }
            }
        
        
        }
        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            List<float> temp = new List<float>();
            Signal outs = new Signal(temp,false);
            string tstPath = "C:/Users/Flasha/Desktop/DSPTestSignals";
            //====================================== 2)
            FIR uFIR = new FIR();
            uFIR.InputStopBandAttenuation = 50;
            uFIR.InputTransitionBand = 500;
            uFIR.InputTimeDomainSignal = InputSignal;
            uFIR.InputF1 = miniF;
            uFIR.InputF2 = maxF;
            uFIR.InputFS = Fs;
            uFIR.InputFilterType = FILTER_TYPES.BAND_PASS;
            uFIR.Run();
            outs = uFIR.OutputYn;
            //======================================
            displaySignal(outs, tstPath + "/tstFir.txt");
            outs = LoadSignal(tstPath + "/tstFir.txt");
            //====================================== 3)
            if (newFs >= 2 * maxF)
            {
                Sampling samplingSignal = new Sampling();
                samplingSignal.InputSignal = outs;
                samplingSignal.L = L;
                samplingSignal.M = M;
                samplingSignal.Run();
                outs = samplingSignal.OutputSignal;
                //======================================
                displaySignal(outs, tstPath + "/tstSampling.txt");
                outs = LoadSignal(tstPath + "/tstSampling.txt");
                //======================================
            }
            else 
            {
                Console.WriteLine("newFs is not valid\n");
            
            }
            //====================================== 4)
            DC_Component dcC =new DC_Component();
            dcC.InputSignal = outs;
            dcC.Run();
            outs = dcC.OutputSignal;
            //======================================
            displaySignal(outs, tstPath + "/tstDC.txt");//5)
            outs = LoadSignal(tstPath + "/tstDC.txt");
            //====================================== 6)
            Normalizer normOuts = new Normalizer();
            normOuts.InputSignal = outs;
            normOuts.InputMaxRange = 1;
            normOuts.InputMinRange = -1;
            normOuts.Run();
            outs = normOuts.OutputNormalizedSignal;
            //====================================== 7)
            displaySignal(outs, tstPath + "/tstNormalizer.txt");
            outs = LoadSignal(tstPath + "/tstNormalizer.txt");
            //====================================== 8)
            DiscreteFourierTransform dftSignal = new DiscreteFourierTransform();
            dftSignal.InputTimeDomainSignal = outs;
            dftSignal.InputSamplingFrequency = Fs;
            //fs=1000
            dftSignal.Run();
            outs = dftSignal.OutputFreqDomainSignal;
            //======================================
            displaySignal(outs, tstPath + "/tstDFT.txt");// 9)
            //======================================
            OutputFreqDomainSignal = outs;
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
