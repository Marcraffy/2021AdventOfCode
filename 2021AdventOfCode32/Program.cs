using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode32
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var raw = Data.Data.GetData<Diagnostic>(DataFileKeys.DIAGNOSTICS);
            var bitLength = raw.First().BitLength;
            var diagnostic = raw.Select(diagnostic => diagnostic.Data).ToList();
            var positionSum = new uint[bitLength];

            var cO2ScrubberRating = RecursiveFilter(diagnostic, false, bitLength);
            var oxygenGeneratorRating = RecursiveFilter(diagnostic, true, bitLength);
            var lifeSupportRating = cO2ScrubberRating * oxygenGeneratorRating;
            Console.WriteLine($"Life Support Rating: {lifeSupportRating}");
        }

        public static uint RecursiveFilter(List<uint> input, bool checkValue, int bitLength, int column = 0)
        {
            if (input.Count == 0)
            {
                throw new ArgumentException("Empty lists not accepted");
            }

            if (input.Count == 1)
            {
                return input.Single();
            }

            var sum = input.Aggregate(0, (acc, val) =>
            {
                var offset = bitLength - 1 - column;
                var data = (val & (uint)Math.Pow(2, offset)) >> offset;
                return (int)(acc + data);
            });

            var average = Convert.ToDouble(sum) / input.Count;
            var roundedAverage = average >= 0.5;

            Func<uint, bool> checkBit = (uint data) =>
            {
                var offset = bitLength - 1 - column;
                var dataValue = data & ((uint)Math.Pow(2, offset));
                var boolValue = Convert.ToBoolean(dataValue);

                if (roundedAverage && checkValue && boolValue)
                {
                    return true;
                }

                var checks = new List<bool> { roundedAverage, checkValue, boolValue };
                var checkCount = checks.Where(check => check).Count();

                return checkCount == 1 || checkCount == 3;
            };

            var newInput = input.Where(checkBit).ToList();
            return RecursiveFilter(newInput, checkValue, bitLength, ++column);
        }
    }
}
