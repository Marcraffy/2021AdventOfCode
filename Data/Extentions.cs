using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public static class Extentions
    {
        public static List<int> GetBoundries(this List<VentLine> ventLines)
        {
            var minimumRow = int.MaxValue;
            var maximumRow = 0;
            var minimumColumn = int.MaxValue;
            var maximumColumn = 0;

            var coordinates = ventLines.SelectMany(line => new List<Coordinate> { line.First, line.Second }).ToList();

            foreach (var coordinate in coordinates)
            {
                minimumRow = coordinate.Row < minimumRow ? coordinate.Row : minimumRow;
                maximumRow = coordinate.Row > maximumRow ? coordinate.Row : maximumRow;
                minimumColumn = coordinate.Column < minimumColumn ? coordinate.Column : minimumColumn;
                maximumColumn = coordinate.Column > maximumColumn ? coordinate.Column : maximumColumn;
            }

            return new List<int>
            {
                minimumRow,
                maximumRow,
                minimumColumn,
                maximumColumn
            };
        }

        public static int Gradient(this VentLine line)
        {
            var rowDelta = (line.First.Row - line.Second.Row);
            var columnDelta = (line.First.Column - line.Second.Column);
            var gradient = Convert.ToDouble(rowDelta) / columnDelta;

            return gradient == 0 ? 0 : (gradient > 0 ? 1 : -1);
        }
    }
}