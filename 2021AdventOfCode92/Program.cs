using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode092
{
    internal static class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var rows = Data.Data.GetData<Row>(DataFileKeys.HEIGHT);

            var width = rows.First().Heights.Length;

            var notBoundries = new List<Coordinate>();
            var lowPoints = new List<Coordinate>();

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
                        lowPoints.Add(new Coordinate(row, column));
                    }

                    if (currentHeight != 9)
                    {
                        notBoundries.Add(new Coordinate(row, column));
                    }
                }
            }

            var sizes = new List<int>();
            foreach (var lowPoint in lowPoints)
            {
                var visited = new List<Coordinate>();
                 sizes.Add(lowPoint.GetNeighbours(rows));
            }

            var output = sizes.OrderByDescending(size => size).Take(3).ToList();
            var total = output.Aggregate(1, (acc, val) => acc * val);
            Console.WriteLine($"{total}");
        }

        public static int GetNeighbours(this Coordinate lowPoint, List<Row> rows)
        {
            var width = rows.First().Heights;
            var height = rows.Count;
            var visited = new List<Coordinate>();

            var neighbours = lowPoint.GetNeighbours(ref visited, rows);
            return neighbours.Count();
        }

        public static List<Coordinate> GetNeighbours(this Coordinate position, ref List<Coordinate> visited, List<Row> rows)
        {
            var neighbours = new List<Coordinate>();
            visited.Add(position);
            neighbours.Add(position);

            var leftCoordinate = new Coordinate(position.Row, position.Column - 1);
            var leftHeight = rows.GetHeight(position.Row, position.Column - 1);

            var rightCoordinate = new Coordinate(position.Row, position.Column + 1);
            var rightHeight = rows.GetHeight(position.Row, position.Column + 1);

            var topCoordinate = new Coordinate(position.Row - 1, position.Column);
            var topHeight = rows.GetHeight(position.Row - 1, position.Column);

            var bottomCoordinate = new Coordinate(position.Row + 1, position.Column);
            var bottomHeight = rows.GetHeight(position.Row + 1, position.Column);

            if (leftHeight == 9 && rightHeight == 9 && topHeight == 9 && bottomHeight == 9)
            {
                return new List<Coordinate> { position };
            }

            if (leftHeight != 9 && !visited.Any(v => v == leftCoordinate))
            {
                var otherNeighbours = leftCoordinate.GetNeighbours(ref visited, rows);
                neighbours.AddRange(otherNeighbours);
            }
            if (rightHeight != 9 && !visited.Any(v => v == rightCoordinate))
            {
                var otherNeighbours = rightCoordinate.GetNeighbours(ref visited, rows);
                neighbours.AddRange(otherNeighbours);
            }
            if (topHeight != 9 && !visited.Any(v => v == topCoordinate))
            {
                var otherNeighbours = topCoordinate.GetNeighbours(ref visited, rows);
                neighbours.AddRange(otherNeighbours);
            }
            if (bottomHeight != 9 && !visited.Any(v => v == bottomCoordinate))
            {
                var otherNeighbours = bottomCoordinate.GetNeighbours(ref visited, rows);
                neighbours.AddRange(otherNeighbours);
            }
            return neighbours;
        }


        public static List<Basin> GetBasins(this List<Row> rows)
        {
            var output = new List<Basin>();
            for (int index = 0; index < rows.Count; index++)
            {
                var rowBasins = rows[index].GetRowBasins(index);
                if (output.Count == 0)
                {
                    output.AddRange(rowBasins);
                }
                else
                {
                    foreach (var rowBasin in rowBasins)
                    {
                        List<Basin> twins = new List<Basin>();
                        var conjoinedBasins = output.Where(basin => rowBasin.IsConjoined(basin)).ToList();
                        var isConjoined = conjoinedBasins.Count() > 0;
                        foreach (var basin in conjoinedBasins)
                        {
                            basin.Add(rowBasin);
                        }

                        var parent = conjoinedBasins.FirstOrDefault();
                        if (!isConjoined)
                        {
                            output.Add(rowBasin);
                        }
                        else if (twins.Count > 0)
                        {
                            var others = conjoinedBasins.Skip(1).ToList();

                            parent.AddRange(others);
                            foreach (var twin in others)
                            {
                                output.Remove(twin);
                            }
                        }
                        if (parent != null)
                        {
                        }
                    }
                }
            }

            return output;
        }

        public static List<Basin> GetRowBasins(this Row row, int index)
        {
            var output = new List<Basin>();
            var basin = new List<Coordinate>();
            for (int column = 0; column < row.Heights.Length; column++)
            {
                var item = row.Heights[column];
                if (item == 9 && basin.Count != 0)
                {
                    output.Add(new Basin(basin));
                    basin = new List<Coordinate>();
                }
                else if (item != 9)
                {
                    basin.Add(new Coordinate(index, column));
                }
            }
            if (basin.Count != 0)
            {
                output.Add(new Basin(basin));
            }
            return output;
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

    public class Basin
    {

        public List<Coordinate> Contents { get; set; }

        public int Size => Contents.Count;

        public Basin(List<Coordinate> contents)
        {
            Contents = contents;
        }

        public void Add(Basin basin)
        {
            foreach (var coordinate in basin.Contents)
            {
                if (!Contents.Any(c => c == coordinate))
                {
                    Contents.AddRange(basin.Contents);
                }
            }
        }

        public void AddRange(List<Basin> basins)
        {
            foreach (var basin in basins)
            {
                Add(basin);
            }
        }

        public bool HasOverlap(Basin basin)
        {
            foreach (var coord in Contents)
            {
                if (basin.Contents.Any(c => coord.Row == c.Row && coord.Column == c.Column))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsConjoined(Basin basin)
        {
            var isConjoined = false;
            foreach (var coord in Contents)
            {
                var possibleContents = basin.Contents.Where(c => c.Row == coord.Row - 1).ToList();
                if (possibleContents.Any(c => coord.Column == c.Column))
                {
                    isConjoined = true;
                    break;
                }
            }
            return isConjoined;
        }

        public Basin Copy()
        {
            var output = new Basin(Contents);
            return output;
        }

        public static bool operator ==(Basin basinA, Basin basinB)
        {
            return basinA.Contents.All(coord => basinB.Contents.Any(c => c == coord));
        }

        public static bool operator !=(Basin basinA, Basin basinB)
        {
            return !basinA.Contents.All(coord => basinB.Contents.Any(c => c == coord));
        }

        public override bool Equals(object obj)
        {
            return Contents.All(coord => ((Basin)obj).Contents.Any(c => c == coord));
        }
    }
}