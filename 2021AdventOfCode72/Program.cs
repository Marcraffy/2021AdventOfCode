using Data;
using System;
using System.Linq;

namespace _2021AdventOfCode72
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var positions = Data.Data.GetData<Horizontal>(DataFileKeys.CRABS).Single().Positions;

            var maxPosition = positions.Max();
            var minPosition = positions.Min();

            var costs = Enumerable.Range(minPosition, maxPosition).Select(alignmentPosition => positions.Select(position => GetFuelCost(position, alignmentPosition)).Sum());
            var minimumnFuelCost = costs.Min();

            Console.WriteLine($"Optimal Fuel Cost:\t {minimumnFuelCost}");
        }

        private static int GetFuelCost(int position, int destination)
        {
            var distance = Math.Abs(position - destination);
            var fuelCost = (distance * distance + distance)/2; //Triangular Number: https://en.wikipedia.org/wiki/Triangular_number
            return fuelCost;
        }
    }
}
