using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data
{
    public static class DataFileKeys
    {
        public const string DEPTH_MEASUREMENTS = "DEPTH_MEASUREMENTS";
        public const string PATH = "PATH";
        public const string DIAGNOSTICS = "DIAGNOSTICS";
        public const string BINGO = "BINGO";
        public const string VENTS = "VENTS";
        public const string LIFE = "LIFE";
        public const string CRABS = "CRABS";
        public const string DISPLAY = "DISPLAY";
        public const string HEIGHT = "HEIGHT";
    }

    public static class Data
    {
        public static Dictionary<string, string> DataFiles = new Dictionary<string, string>
        {
            {DataFileKeys.DEPTH_MEASUREMENTS, "data.txt"},
            {DataFileKeys.PATH, "path.txt"},
            {DataFileKeys.DIAGNOSTICS, "binary.txt"},
            {DataFileKeys.BINGO, "bingo.txt"},
            {DataFileKeys.VENTS, "vents.txt"},
            {DataFileKeys.LIFE, "life.txt"},
            {DataFileKeys.CRABS, "crabs.txt"},
            {DataFileKeys.DISPLAY, "display.txt"},
            {DataFileKeys.HEIGHT, "height.txt"},
        };

        public static List<T> GetData<T>(string dataFileKey) where T: class
        {
            var data = File.ReadAllText(DataFiles[dataFileKey]);
            var dataSplitOnNewLine = data.Split('\n').Select(item => item.Trim());
            switch (dataFileKey)
            {
                case DataFileKeys.BINGO:
                    var numbers = dataSplitOnNewLine.First();
                    var boards = dataSplitOnNewLine.Skip(1).Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
                    var bingoBoards = new List<T>();
                    for (int index = 0; index < boards.Count; index+=5)
                    {
                        var board = boards.Skip(index).Take(5).Aggregate("", (acc, val) => acc + val + ' ');
                        bingoBoards.Add(new BingoBoard(board, numbers) as T);
                    }
                    return bingoBoards;
                default:
                    return data.Split('\n').Select(input => MapData<T>(dataFileKey, input.Trim())).ToList();
            }
        }

        private static T MapData<T>(string dataFileKey, string input) where T: class
        {
            return dataFileKey switch
            {
                DataFileKeys.DEPTH_MEASUREMENTS => new Depth(input) as T,
                DataFileKeys.PATH => new PathInstruction(input) as T,
                DataFileKeys.DIAGNOSTICS => new Diagnostic(input) as T,
                DataFileKeys.VENTS => new VentLine(input) as T,
                DataFileKeys.LIFE => new Lives(input) as T,
                DataFileKeys.CRABS => new Horizontal(input) as T,
                DataFileKeys.DISPLAY => new Display(input) as T,
                DataFileKeys.HEIGHT => new Row(input) as T,
                _ => null,
            };
        }
    }

    public class Row
    {
        public int[] Heights;

        public Row(string input)
        {
            Heights = input.Select(character => (int)Char.GetNumericValue(character)).ToArray();
        }
    }

    public class Display
    {
        public List<SevenSegment> SampleModules { get; set; }
        public List<SevenSegment> DisplayOutput { get; set; }

        public Display(string input)
        {
            var inputs = input.Split('|').Select(set => set.Trim()).ToArray();

            SampleModules = inputs[0].Split(' ').Select(module => new SevenSegment(module)).ToList();
            DisplayOutput = inputs[1].Split(' ').Select(module => new SevenSegment(module)).ToList();
        }
    }

    public class SevenSegment
    {
        public bool SegmentA { get; set; }
        public bool SegmentB { get; set; }
        public bool SegmentC { get; set; }
        public bool SegmentD { get; set; }
        public bool SegmentE { get; set; }
        public bool SegmentF { get; set; }
        public bool SegmentG { get; set; }

        public SevenSegment(string input)
        {
            SegmentA = input.Contains('a');
            SegmentB = input.Contains('b');
            SegmentC = input.Contains('c');
            SegmentD = input.Contains('d');
            SegmentE = input.Contains('e');
            SegmentF = input.Contains('f');
            SegmentG = input.Contains('g');
        }

        public SevenSegment() { }

        public static bool operator ==(SevenSegment moduleA, SevenSegment moduleB)
        {
            return moduleA?.SegmentA == moduleB?.SegmentA
                && moduleA?.SegmentB == moduleB?.SegmentB
                && moduleA?.SegmentC == moduleB?.SegmentC
                && moduleA?.SegmentD == moduleB?.SegmentD
                && moduleA?.SegmentE == moduleB?.SegmentE
                && moduleA?.SegmentF == moduleB?.SegmentF
                && moduleA?.SegmentG == moduleB?.SegmentG;
        }

        public static bool operator !=(SevenSegment moduleA, SevenSegment moduleB)
        {
            return !(moduleA?.SegmentA == moduleB?.SegmentA
                  && moduleA?.SegmentB == moduleB?.SegmentB
                  && moduleA?.SegmentC == moduleB?.SegmentC
                  && moduleA?.SegmentD == moduleB?.SegmentD
                  && moduleA?.SegmentE == moduleB?.SegmentE
                  && moduleA?.SegmentF == moduleB?.SegmentF
                  && moduleA?.SegmentG == moduleB?.SegmentG);
        }

        public static SevenSegment operator +(SevenSegment moduleA, SevenSegment moduleB)
        {
            var segments = new SevenSegment();
            segments.SegmentA = moduleA.SegmentA || moduleB.SegmentA ? true : false;
            segments.SegmentB = moduleA.SegmentB || moduleB.SegmentB ? true : false;
            segments.SegmentC = moduleA.SegmentC || moduleB.SegmentC ? true : false;
            segments.SegmentD = moduleA.SegmentD || moduleB.SegmentD ? true : false;
            segments.SegmentE = moduleA.SegmentE || moduleB.SegmentE ? true : false;
            segments.SegmentF = moduleA.SegmentF || moduleB.SegmentF ? true : false;
            segments.SegmentG = moduleA.SegmentG || moduleB.SegmentG ? true : false;
            return segments;
        }

        public static SevenSegment operator -(SevenSegment moduleA, SevenSegment moduleB)
        {
            var segments = new SevenSegment();
            segments.SegmentA = moduleA.SegmentA && moduleB.SegmentA ? false : moduleA.SegmentA;
            segments.SegmentB = moduleA.SegmentB && moduleB.SegmentB ? false : moduleA.SegmentB;
            segments.SegmentC = moduleA.SegmentC && moduleB.SegmentC ? false : moduleA.SegmentC;
            segments.SegmentD = moduleA.SegmentD && moduleB.SegmentD ? false : moduleA.SegmentD;
            segments.SegmentE = moduleA.SegmentE && moduleB.SegmentE ? false : moduleA.SegmentE;
            segments.SegmentF = moduleA.SegmentF && moduleB.SegmentF ? false : moduleA.SegmentF;
            segments.SegmentG = moduleA.SegmentG && moduleB.SegmentG ? false : moduleA.SegmentG;
            return segments;
        }
    }

    public class Horizontal
    {
        public List<int> Positions { get; set; }

        public Horizontal(string input)
        {
            Positions = input.Split(',').Select(position => Convert.ToInt32(position)).ToList();
        }
    }

    public class Lives
    {
        public List<int> StartingLives;

        public Lives(string input)
        {
            StartingLives = input.Split(',').Select(life => Convert.ToInt32(life)).ToList();
        }
    }

    public class VentLine
    {
        public Coordinate First { get; set; }
        public Coordinate Second { get; set; }
        public VentLine(string input)
        {
            var data = input.Split(" -> ");
            First = new Coordinate(data[0]);
            Second = new Coordinate(data[1]);
        }
    }

    public class Coordinate
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Coordinate(string input)
        {
            var data = input.Split(',');
            Column = Convert.ToInt32(data[0]);
            Row = Convert.ToInt32(data[1]);
        }

        public override string ToString()
        {
            return $"Row: {Row}, Column: {Column}";
        }

        public override bool Equals(object obj)
        {
            return Row == ((Coordinate)obj).Row
                && Column == ((Coordinate)obj).Column;
        }

        public static bool operator ==(Coordinate coordinateA, Coordinate coordinateB)
            => coordinateA.Row == coordinateB.Row && coordinateA.Column == coordinateB.Column;

        public static bool operator !=(Coordinate coordinateA, Coordinate coordinateB)
            => !(coordinateA.Row == coordinateB.Row && coordinateA.Column == coordinateB.Column);
    }

    public class BingoBoard
    {
        public int[,] Board { get; set; }
        public int[] Numbers { get; set; }

        public BingoBoard(string input, string numbers)
        {
            Board = GetBoard(input);
            Numbers = numbers.Split(',').Select(number => Convert.ToInt32(number)).ToArray();
        }

        private int[,] GetBoard(string input)
        {
            var numbers = input.Split(' ').Where(number => !string.IsNullOrWhiteSpace(number)).Select(number => Convert.ToInt32(number)).ToList();
            var board = new int[5, 5];
            var row = 0;
            for (int index = 0; index < 25; index++)
            {
                board[row, index % 5] = numbers[index];
                if (index % 5 == 4)
                    row++;
            }
            return board;
        }
    }

    public class Diagnostic
    {

        public Diagnostic(string input)
        {
            Data = Convert.ToUInt32(input, 2);
            BitLength = input.Length;
        }

        public uint Data { get; set; }
        public int BitLength { get; set; }
    }

    public class Depth
    {

        public Depth(string input)
        {
            _Depth = Convert.ToInt32(input);
        }

        public int _Depth { get; set; }
    }

    public class PathInstruction
    {

        public PathInstruction(string input)
        {
            var inputs = input.Split(' ');
            Direction = inputs[0];
            Distance = Convert.ToInt32(inputs[1]);
        }

        public string Direction { get; set; }
        public int Distance { get; set; }
    }
}
