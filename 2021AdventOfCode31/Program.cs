using Data;
using System;
using System.Linq;

namespace _2021AdventOfCode31
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var raw = Data.Data.GetData<Diagnostic>(DataFileKeys.DIAGNOSTICS);
            var bitLength = raw.First().BitLength;
            var diagnostic = raw.Select(diagnostic => diagnostic.Data).ToList();
            var opCount = 0;
            var positionSum = new uint[bitLength];
            foreach (var data in diagnostic)
            {
                opCount++;

                for (int column = 0; column < bitLength; column++)
                {
                    var offset = bitLength - 1 - column;
                    positionSum[column] += (data & (uint)Math.Pow(2, offset)) >> offset;
                }
            }
            var gammaRate = Convert.ToUInt32(positionSum.Select(sum => Convert.ToDouble(sum) / diagnostic.Count).Aggregate("", (acc, val) => val >= 0.5 ? acc + '1' : acc + '0'), 2);
            var epsilonRate = gammaRate ^ ((uint)Math.Pow(2, bitLength) - 1);
            var powerConsumption = gammaRate * epsilonRate;
            Console.WriteLine($"Power consumption: {powerConsumption}");
        }
    }

}
