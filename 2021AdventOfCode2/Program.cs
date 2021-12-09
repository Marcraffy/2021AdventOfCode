using Data;
using System;
using System.Linq;

namespace _2021AdventOfCode012
{
    class Program
    {
        private const bool DEBUG = false;
        private const int TAKE_VALUE = 3;

        static void Main(string[] args)
        {
            var depthMeasurements = Data.Data.GetData<Depth>(DataFileKeys.DEPTH_MEASUREMENTS).Select(depth => depth._Depth).ToList();
            var lastDepthSum = int.MaxValue;
            var increaseCount = 0;
            var opCount = 0;
            for (int windowIndex = 0; windowIndex < depthMeasurements.Count - TAKE_VALUE + 1; windowIndex ++)
            {
                opCount++;
                var depthSum = depthMeasurements.Skip(windowIndex).Take(TAKE_VALUE).Sum();
                if (depthSum > lastDepthSum) increaseCount++;
                    
                if (DEBUG) Console.WriteLine($"({opCount})\t({depthSum})\t{(depthSum > lastDepthSum ? $"Increases({increaseCount})" : "Decreases")}");
                lastDepthSum = depthSum;
            }
            Console.WriteLine($"{increaseCount} measurements");
        }
    }
}
