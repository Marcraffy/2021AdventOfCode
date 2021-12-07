using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode71
{
    static class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var positions = Data.Data.GetData<Horizontal>(DataFileKeys.CRABS).Single().Positions;

            var maxPosition = positions.Max();
            var minPosition = positions.Min();
            var costs = new List<int>();

            for (int alignmentPosition = minPosition; alignmentPosition <= maxPosition; alignmentPosition++)
            {
                var fuelCosts = positions.Select(position => Math.Abs(position - alignmentPosition)).ToList();
                var totalFuelCost = fuelCosts.Sum();
                costs.Add(totalFuelCost);
            }

            var minimumnFuelCost = costs.Min();
            Console.WriteLine($"Optimal Fuel Cost:\t {minimumnFuelCost}");
        }
    }
}
