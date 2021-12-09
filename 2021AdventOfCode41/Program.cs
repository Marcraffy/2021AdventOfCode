using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode041
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var raw = Data.Data.GetData<BingoBoard>(DataFileKeys.BINGO);
            var numbers = raw.First().Numbers;
            var boards = raw.Select(board => new Board(board)).ToList();
            var winner = GetWinningBoard(numbers, boards);

            Console.WriteLine($"Bingo Winner Score: {winner.Score}");
        }

        private static Board GetWinningBoard(int[] numbers, List<Board> boards)
        {

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    var bingo = board.CheckNumber(number);
                    if (bingo)
                    {
                        return board;
                    }
                }
            }
            return null;
        }
    }
}
