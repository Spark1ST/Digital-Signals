using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }
        public List<float> levelsMidPoint { get; set; }
        public List<float> levelsEndPoints { get; set; }

        public List<float> quantizedSignal { get; set; }
        public void encodedAndIndices(List<float> levelsEndPoints,List<float>levelsMidPoints)
        {
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {

                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= levelsEndPoints[j] && j + 1 < levelsEndPoints.Count)//mygb4 exception
                    {
                        if (InputSignal.Samples[i] < levelsEndPoints[j + 1])
                        {
                            quantizedSignal.Add(levelsMidPoint[j]);
                            OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0')); // to convert Interval Indecies to Binary
                            OutputIntervalIndices.Add(j + 1);
                            break;
                        }
                    }
                    if (InputSignal.Samples[i] >= levelsEndPoints[InputLevel])
                    {
                        j = InputLevel - 1;
                        quantizedSignal.Add(levelsMidPoint[j]);
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0')); // to convert Interval Indecies to Binary
                        OutputIntervalIndices.Add(InputLevel);
                        break;
                    }

                }

            }
        }
        public List<float> intializeLevelsMidPoint(List<float> levelsEndPoints)
        {
            List<float> midPoints = new List<float>();
            float midPoint;
            for (int i = 0; i < levelsEndPoints.Count-1; i++)
            {
                midPoint = (levelsEndPoints[i] + levelsEndPoints[i + 1]) / 2;
                midPoints.Add(midPoint);
            }
            //outs count = inp.samples.count or lvlsEndPoints.count -1 
            return midPoints;
        }
        public List<float> intializeLevelsEndPoint(float startPoint, float offset)
        {
            List<float> levels = new List<float>();
            levels.Add(startPoint);
            for (int i = 0; i < InputLevel; i++)
            {
                levels.Add(startPoint + offset);
                startPoint = startPoint + offset;
            }
            //outs 1 element more than number of level due to adding the start point so i can use range precisely
            return levels;
        }
        public override void Run()
        {
            levelsMidPoint = new List<float>();
            levelsEndPoints = new List<float>();
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            quantizedSignal = new List<float>();
            //=========================================================
            // delta //
            float deltaOffset;
            //=========================================================
            // to get maximum & minimum values of the sampels
            float maximum = InputSignal.Samples.Max();
            float minimum = InputSignal.Samples.Min();
            //=========================================================
            // to use InputLevel OR InputNumBits ...
            InputLevel = (InputLevel <= 0) ? (int)Math.Pow(2, InputNumBits) : InputLevel;
            InputNumBits = (InputNumBits <= 0) ? (int)Math.Log(InputLevel, 2) : InputNumBits;
            //=========================================================
            //calculate delta
            deltaOffset = (maximum - minimum) / InputLevel;
            //=========================================================
            //get each level end point
            levelsEndPoints = intializeLevelsEndPoint(minimum, deltaOffset);

            //=========================================================
            // to get midpoint and put it in list midpoint ...
            levelsMidPoint = intializeLevelsMidPoint(levelsEndPoints);
            //=========================================================
            //get interval index and encoded signal
            encodedAndIndices(levelsEndPoints, levelsMidPoint);
            //=========================================================
            // to calculate Error Quantization ...
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float Quan_error = quantizedSignal[i] - InputSignal.Samples[i];
                OutputSamplesError.Add(Quan_error);
            }
            //=========================================================
            //output
            OutputQuantizedSignal = new Signal(quantizedSignal, false);
        }
    }
}