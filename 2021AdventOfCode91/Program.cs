using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode091
{
    static class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var rows = Data.Data.GetData<Row>(DataFileKeys.HEIGHT);

            var width = rows.First().Heights.Length;
            var riskLevels = new List<int>();

            for (int row = 0; row < rows.Count; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var currentHeight = rows.GetHeight(row, column);

                    var isTopHigher = currentHeight < rows.GetHeight(row - 1, column);
                    var isBottomHigher = currentHeight < rows.GetHeight(row + 1, column);
                    var isLeftHigher = currentHeight < rows.GetHeight(row, column - 1);
                    var isRightHigher = currentHeight < rows.GetHeight(row, column + 1);
                    if (isTopHigher && isBottomHigher && isRightHigher && isLeftHigher)
                    {
                        Console.WriteLine($"Low Point Found: Height:\t {currentHeight}");

                        riskLevels.Add(currentHeight + 1);
                    }
                }
            }

            var riskLevelTotal =  riskLevels.Sum();
            Console.WriteLine($"The risk level is:\t {riskLevelTotal}");
        }

        public static int GetHeight(this List<Row> rows, int row, int column)
        {
            var width = rows.First().Heights.Length;
            if (row < 0 || column < 0 || row >= rows.Count || column >= width)
            {
                return 9;
            }
            return rows[row].Heights[column];
        }
    }
}
