using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode172
{
    class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var target = Data.Data.GetData<Target>(DataFileKeys.TARGET).First();
            var minStartingYVelocity = target.LowerYBound;
            var maxStartingYVelocity = -1 * target.LowerYBound;
            var maxStartingXVelocity = target.UpperXBound;

            var hitCount = 0;

            for (int xVelocity = 0; xVelocity <= maxStartingXVelocity; xVelocity++)
            {
                for (int yVelocity = minStartingYVelocity; yVelocity <= maxStartingYVelocity; yVelocity++)
                {
                    if (xVelocity == 0 && yVelocity == 0)
                        continue;

                    if ((xVelocity >= target.LowerXBound && xVelocity <= target.UpperXBound)
                        && (yVelocity >= target.LowerYBound && yVelocity <= target.UpperYBound))
                    {
                        hitCount++;
                        continue;
                    }
                    else
                    {
                        var trajectory = Trajectory(xVelocity, yVelocity, new Coordinate(target.LowerYBound, target.UpperXBound));
                        if (trajectory.Any(coord => coord.Column >= target.LowerXBound && coord.Column <= target.UpperXBound
                                                 && coord.Row    >= target.LowerYBound && coord.Row    <= target.UpperYBound ))
                        {
                            hitCount++;
                            continue;
                        }
                    }
                }
            }

            Console.WriteLine($"The number of possible firing solutions is: {hitCount}");
        }

        private static List<Coordinate> Trajectory(int xVelocity, int yVelocity, Coordinate bounds, Coordinate start = null, List<Coordinate> lastCoords = null)
        {
            if (start == null)
            {
                start = new Coordinate(0, 0);
                lastCoords = new List<Coordinate> { new Coordinate(start.Row, start.Column) };
            }

            if (start.Column + xVelocity > bounds.Column
                || start.Row + yVelocity < bounds.Row)
            {
                return lastCoords;
            }

            var newColumn = start.Column + xVelocity;
            var newRow = start.Row + yVelocity;
            var newStart = new Coordinate(newRow, newColumn);

            var newXVelocity = xVelocity;
            if (newXVelocity > 0)
            {
                newXVelocity--;
            }

            var newYVelocity = yVelocity - 1;

            lastCoords.Add(newStart);
            return Trajectory(newXVelocity, newYVelocity, bounds, newStart, lastCoords);
        }
    }
}
