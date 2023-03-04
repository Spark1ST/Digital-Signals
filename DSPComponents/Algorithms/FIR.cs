using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }
        public int calcN(float factor,float transitionWidth)
        {
            double N = factor / transitionWidth; //calc N
            N = Math.Ceiling(N);
            if (N % 2 == 0)
            {
                N++;
            }
            return (int)N;
        }
        public List<float> windowMethod() 
        {
            //============================================
            float transitionWidth = InputTransitionBand / InputFS; //normalized transition
            List<float> WofnList=new List<float>();
            //============================================
            if (InputStopBandAttenuation >= 13 && InputStopBandAttenuation <= 21)//rectangle
            {
                int N = calcN(0.9f, transitionWidth);
                for (int i = 0; i < N; i++)
                {
                    WofnList.Add(1.0f);
                }
            }
            else if (InputStopBandAttenuation >= 31 && InputStopBandAttenuation <= 44)//hanning
            {
                int N = calcN(3.1f, transitionWidth);
                for (int i = 0; i < N; i++)
                {
                    //============================================
                    //7rka say3a 34an a3ml el signal symmetric
                    int index = (int)(i - ((N - 1) / 2));
                    int absIndex = Math.Abs(index);
                    //============================================
                    float wOfN = 0.5f + (0.5f * (float)Math.Cos((2 * absIndex * (float)Math.PI) / N));
                    WofnList.Add(wOfN);

                }
            }
            //====================================================================================================
            else if (InputStopBandAttenuation >= 41 && InputStopBandAttenuation <= 53)//hamming
            {
                //delta f = t/fs

                int N=calcN(3.3f, transitionWidth);
                for (int i = 0; i < N; i++)
                {
                    //============================================
                    //7rka say3a 34an a3ml el signal symmetric
                    int index = (int)(i - ((N - 1 )/ 2));
                    int absIndex=Math.Abs(index);
                    //============================================
                    float wOfN = 0.54f + (0.46f * (float)Math.Cos((2 * absIndex * (float)Math.PI) / N));
                    WofnList.Add(wOfN);
                }
            }
            //====================================================================================================
            else if (InputStopBandAttenuation >= 57 && InputStopBandAttenuation <= 74)//blackman
            {
                int N = calcN(5.5f, transitionWidth);
                for (int i = 0; i < N; i++)
                {
                    //============================================
                    //7rka say3a 34an a3ml el signal symmetric
                    int index = (int)(i - ((N-1) / 2));
                    int absIndex = Math.Abs(index);
                    //============================================
                    float wOfN = 0.42f + (0.5f * (float)Math.Cos((2 * absIndex * (float)Math.PI) / (N-1) ))+ (0.08f * (float)Math.Cos((4 * absIndex * (float)Math.PI) / (N - 1)));
                    WofnList.Add(wOfN);
                }

            }
            return WofnList;
        }
        public void lowPassF() 
        {
            //============================================
            //float transitionWidth = InputTransitionBand / InputFS; //normalized transition
            float fc = (float)(InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;//new fc
            //shift more to be closer to ideal
            List<float> wOfN = windowMethod();
            int N = wOfN.Count;
            //============================================
            List<float> hn = new List<float>();
            List<int> indices = new List<int>();
            //============================================
            for (int i = 0; i < N; i++)
            {
                //============================================
                //7rka say3a 34an a3ml el signal symmetric
                int index = (int)(i - ((N - 1) / 2));
                indices.Add(index);
                int absIndex = Math.Abs(index);
                //============================================
                if (absIndex == 0)
                {
                    hn.Add(2 * fc);
                }
                else
                {
                    //h delta(n)
                    float hDeltaOfN;
                    hDeltaOfN = (2 * fc) * (float)Math.Sin(2 * absIndex * (float)Math.PI * fc) / (2 * absIndex * (float)Math.PI * fc);
                    //h(n)= h delta (n)* w(n)
                    hn.Add(hDeltaOfN * wOfN[i]);
                }
            }
            OutputHn = new Signal(hn, indices, false);
        }
        public void highPassF()
        {
            //============================================
            float fc = (float)(InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;//new fc
            //shift less to be closer to ideal
            List<float> wOfN = windowMethod();
            int N = wOfN.Count;
            //============================================
            List<float> hn = new List<float>();
            List<int> indices = new List<int>();
            //============================================
            for (int i = 0; i < N; i++)
            {
                //============================================
                //7rka say3a 34an a3ml el signal symmetric
                int index = (int)(i - ((N - 1) / 2));
                indices.Add(index);
                int absIndex = Math.Abs(index);
                //============================================
                if (absIndex == 0)
                {
                    hn.Add(1-(2 * fc));//far2 3n el low el 1-
                }
                else
                {
                    //h delta(n)
                    float hDeltaOfN;
                    hDeltaOfN = (-2 * fc) * (float)Math.Sin(2 * absIndex * (float)Math.PI * fc) / (2 * absIndex * (float)Math.PI * fc);//el far2 3n el low el -2 *fc bd 2fc 
                    //h(n)= h delta (n)* w(n)
                    hn.Add(hDeltaOfN * wOfN[i]);
                }
            }
            OutputHn = new Signal(hn, indices, false);

        }
        public void bandPass() 
        {
            //============================================
            List<float> wOfN = windowMethod();
            float f1 = (float)(InputF1 - (InputTransitionBand / 2)) / InputFS;//shift less to be closer to ideal
            float f2 = (float)(InputF2 + (InputTransitionBand / 2)) / InputFS;//shift more to be closer to ideal
            int N = wOfN.Count;
            //============================================
            List<float> hn = new List<float>();
            List<int> indices = new List<int>();
            //============================================
            for (int i = 0; i < N; i++)
            {
                //============================================
                //7rka say3a 34an a3ml el signal symmetric
                int index = (int)(i - ((N - 1) / 2));
                indices.Add(index);
                int absIndex = Math.Abs(index);
                //============================================
                if (absIndex == 0)
                {
                    hn.Add(2.0f * (float)(f2 - f1));
                }
                else
                {
                    //h delta(n)
                    float hDeltaOfN1 = (float)((-2 * f1) * (float)Math.Sin(2 * absIndex *Math.PI * (float)f1) / (2 * absIndex * Math.PI * (float)f1));
                    float hDeltaOfN2 = (float)((2 * f2) * (float)Math.Sin(2 * absIndex *Math.PI * (float)f2) / (2 * absIndex * Math.PI * (float)f2));
                    hn.Add((hDeltaOfN1+ hDeltaOfN2) * wOfN[i]);
                }
            }
            OutputHn = new Signal(hn, indices, false);
        }
        public void bandStop()
        {
            //============================================
            List<float> wOfN = windowMethod();
            float f1 = (float)(InputF1 + (InputTransitionBand / 2)) / InputFS;//shift more to be closer to ideal
            float f2 = (float)(InputF2 - (InputTransitionBand / 2)) / InputFS;//shift less to be closer to ideal
            int N = wOfN.Count;
            //============================================
            List<float> hn = new List<float>();
            List<int> indices = new List<int>();
            //============================================
            for (int i = 0; i < N; i++)
            {
                //============================================
                //7rka say3a 34an a3ml el signal symmetric
                int index = (int)(i - ((N - 1) / 2));
                indices.Add(index);
                int absIndex = Math.Abs(index);
                //============================================
                if (absIndex == 0)
                {
                    hn.Add(1.0f-(2.0f * (float)(f2 - f1)));
                }
                else
                {
                    //h delta(n)
                    float hDeltaOfN1 = (float)((2 * f1) * (float)Math.Sin(2 * absIndex * Math.PI * (float)f1) / (2 * absIndex * Math.PI * (float)f1));
                    float hDeltaOfN2 = (float)((-2 * f2) * (float)Math.Sin(2 * absIndex * Math.PI * (float)f2) / (2 * absIndex * Math.PI * (float)f2));
                    hn.Add((hDeltaOfN1 + hDeltaOfN2) * wOfN[i]);
                }
            }
            OutputHn = new Signal(hn, indices, false);
        }
        public override void Run()
        {
            //=========================================================================
            //first i need to check which window to use from table 3 that gives me w(n)
            //then i use the function named after the filter to get h(n)=hdelta(n)*w(n)
            //then i do convolvution signal between the input time domain signal , h(n)
            //=========================================================================
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                lowPassF();
  /*              DiscreteFourierTransform DFTlowwPassF = new DiscreteFourierTransform();
                DFTlowwPassF.InputTimeDomainSignal = InputTimeDomainSignal;
                DFTlowwPassF.Run();*/
                DirectConvolution dcLowPassConv =new DirectConvolution();
                dcLowPassConv.InputSignal1 = OutputHn;
                dcLowPassConv.InputSignal2 = InputTimeDomainSignal;
                dcLowPassConv.Run();
                OutputYn = dcLowPassConv.OutputConvolvedSignal;

            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                highPassF();
                DirectConvolution dcHighPassConv = new DirectConvolution();
                dcHighPassConv.InputSignal1 = OutputHn;
                dcHighPassConv.InputSignal2 = InputTimeDomainSignal;
                dcHighPassConv.Run();
                OutputYn = dcHighPassConv.OutputConvolvedSignal;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                bandPass();
                DirectConvolution dcBandPassConv = new DirectConvolution();
                dcBandPassConv.InputSignal1 = OutputHn;
                dcBandPassConv.InputSignal2 = InputTimeDomainSignal;
                dcBandPassConv.Run();
                OutputYn = dcBandPassConv.OutputConvolvedSignal;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP) 
            {
                bandStop();
                DirectConvolution dcBandStopConv = new DirectConvolution();
                dcBandStopConv.InputSignal1 = OutputHn;
                dcBandStopConv.InputSignal2 = InputTimeDomainSignal;
                dcBandStopConv.Run();
                OutputYn = dcBandStopConv.OutputConvolvedSignal;
            }
        }
    }
}
