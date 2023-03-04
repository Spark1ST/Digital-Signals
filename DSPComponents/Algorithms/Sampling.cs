﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public FIR userFIR(Signal inpSig) 
        {
            FIR userFIR = new FIR();
            userFIR.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            userFIR.InputFS = 8000;
            userFIR.InputStopBandAttenuation = 50;
            userFIR.InputCutOffFrequency = 1500;
            userFIR.InputTransitionBand = 500;
            userFIR.InputTimeDomainSignal = inpSig;
            userFIR.Run();
            return userFIR;
        }
        public Signal decimation (FIR uf)//=>> skipping
        {
            List<float> outs = new List<float>();

            int N = uf.OutputYn.Samples.Count;

            for (int i = 0; i < N; i++)
            {
                if (i % M == 0)
                {
                    outs.Add(uf.OutputYn.Samples[i]);
                }
            }
            Signal interP = new Signal(outs, true);
            return interP;
        }
        public Signal Interpolation() //==>> adding
        {
            List<float> outs = new List<float>();
            int index = 0;
            for (int i = 0; index < InputSignal.Samples.Count; i++)
            {
                if (i % L == 0)
                {
                    outs.Add(InputSignal.Samples[index]);
                    index++;
                }
                else
                    outs.Add(0);

            }
            Signal interP = new Signal(outs, true);
            return interP;
        }
        public override void Run()
        {
            if (L == 0 && M != 0)
            {
                FIR uf = userFIR(InputSignal);
                OutputSignal = decimation(uf);
            }
            else if (M == 0 && L != 0)
            {
                Signal interP = Interpolation();
                FIR uf = userFIR(interP);
                OutputSignal = uf.OutputYn;
            }
            else //b3ml up -> fir -> down
            {
                Signal interP = Interpolation();
                FIR uf = userFIR(interP);
                OutputSignal = decimation(uf);
            }

        }
    }

}