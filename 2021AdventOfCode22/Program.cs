using Data;
using System;
using System.Linq;

namespace _2021AdventOfCode21
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var path = Data.Data.GetData<PathInstruction>(DataFileKeys.PATH).Select(instruction => new Instruction(instruction)).ToList();
            var position = new Position();
            var opCount = 0;
            foreach (var instruction in path)
            {
                opCount++;
                position.ProcessInstruction(instruction);

                if (DEBUG) Console.WriteLine($"({opCount})\t({instruction.Direction.ToString().First()})\t({instruction.Distance})\t({position.Depth})\t({position.HorizontalPosition})\t({position.GeometricPosition})");
            }
            Console.WriteLine($"{position.GeometricPosition} final geometeric position");
        }
    }

    class Position
    {
        private int aim = 0;
        public int HorizontalPosition { get; set; }
        public int Depth { get; set; }

        public void ProcessInstruction(Instruction instruction)
        {
            switch (instruction.Direction)
            {
                case Instruction.DirectionEnum.Up:
                    aim -= instruction.Distance;
                    break;
                case Instruction.DirectionEnum.Down:
                    aim += instruction.Distance;
                    break;
                case Instruction.DirectionEnum.Forward:
                    HorizontalPosition += instruction.Distance;
                    Depth += aim*instruction.Distance;
                    break;
                default:
                    break;
            }
        }

        public int GeometricPosition => HorizontalPosition * Depth;
    }

    class Instruction
    {
        public enum DirectionEnum
        {
            Up,
            Down,
            Forward
        }

        public DirectionEnum Direction { get; set; }
        public int Distance { get; set; }

        public Instruction(PathInstruction instruction)
        {
            Distance = instruction.Distance;
            Direction = instruction.Direction switch
            {
                "forward" => DirectionEnum.Forward,
                "up" => DirectionEnum.Up,
                "down" => DirectionEnum.Down,
                _ => throw new ArgumentException(nameof(Direction))
            };
        }
    }
}
