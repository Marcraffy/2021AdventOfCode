using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2021AdventOfCode051
{
    static class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var ventLines = Data.Data.GetData<VentLine>(DataFileKeys.VENTS);
            var intersections = ventLines.CountDistinctIntersections();

            Console.WriteLine($"Number of overlaps: {intersections}");
        }

        public static int CountDistinctIntersections(this List<VentLine> ventLines)
        {
            var grid = new Grid(ventLines.GetBoundries());

            foreach (var line in ventLines)
            {
                grid.AddLine(line);
            }
            return grid.CountIntersections();
        }
    }

    public class Grid
    {
        private int[,] grid;

        public int Height { get; set; }
        public int Width { get; set; }

        public int MinimumRow { get; set; }
        public int MaximunRow { get; set; }
        public int MinimumColumn { get; set; }
        public int MaximunColumn { get; set; }

        public Grid(List<int> boundries)
        {
            MinimumRow = boundries[0];
            MaximunRow = boundries[1];
            MinimumColumn = boundries[2];
            MaximunColumn = boundries[3];
            Height = MaximunRow - MinimumRow + 1;
            Width = MaximunColumn - MinimumColumn + 1;
            grid = new int[Height, Width];
        }

        public int GetRowIndex(int row) => row - MinimumRow;
        public int GetColumnIndex(int column) => column - MinimumColumn;

        public void AddLine(VentLine line)
        {
            var isHorizontal = line.First.Row == line.Second.Row;
            var isVeritical = line.First.Column == line.Second.Column;

            if (isHorizontal)
            {
                var minimumColumn = line.First.Column <= line.Second.Column ? line.First.Column : line.Second.Column;
                var maximumColumn = line.First.Column >= line.Second.Column ? line.First.Column : line.Second.Column;
                for (int column = minimumColumn; column <= maximumColumn; column++)
                {
                    grid[GetRowIndex(line.First.Row), GetColumnIndex(column)]++;
                }
            }
            else if (isVeritical)
            {
                var minimumRow = line.First.Row <= line.Second.Row ? line.First.Row : line.Second.Row;
                var maximumRow = line.First.Row >= line.Second.Row ? line.First.Row : line.Second.Row;
                for (int row = minimumRow; row <= maximumRow; row++)
                {
                    grid[GetRowIndex(row), GetColumnIndex(line.First.Column)]++;
                }
            }
        }

        public int CountIntersections()
        {
            var count = 0;
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (grid[row, column] > 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (grid[row, column] == 0)
                    {
                        builder.Append(".");
                    }
                    else
                    {
                        builder.Append(grid[row, column]);
                    }
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
