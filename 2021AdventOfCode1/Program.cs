using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021AdventOfCode011
{
    class Program
    {
        static void Main(string[] args)
        {
            var depthMeasurements = Data.Data.GetData<Depth>(DataFileKeys.DEPTH_MEASUREMENTS).Select(depth => depth._Depth).ToList();
            var lastDepth = int.MaxValue;
            var increaseCount = 0;
            var opCount = 0;
            foreach (var depth in depthMeasurements)
            {
                opCount++;
                if (depth > lastDepth)
                {
                    increaseCount++;
                    Console.WriteLine($"({opCount})\t({depth})\tincreases ({increaseCount})");
                }
                else
                {
                    Console.WriteLine($"({opCount})\t({depth})\tdecreases");
                }
                lastDepth = depth;
            }
            Console.WriteLine($"{increaseCount} measurements");
        }
    }
}
