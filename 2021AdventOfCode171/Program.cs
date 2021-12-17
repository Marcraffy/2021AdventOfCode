using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode171
{
    class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var target = Data.Data.GetData<Target>(DataFileKeys.TARGET).First();
            var startingYVelocity = -1 * target.LowerYBound - 1;
            var startingXVelocity = Convert.ToInt32(Math.Floor(0.5d * (Math.Sqrt(8 * target.UpperXBound - 7) - 1)));

            var maxHeightAtStep = (startingYVelocity/2f) * (startingYVelocity + 1);

            Console.WriteLine($"The max height is: {maxHeightAtStep}");
        }
    }
}
