using Data;
using System;
using System.Linq;

namespace _2021AdventOfCode41
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var raw = Data.Data.GetData<BingoBoard>(DataFileKeys.BINGO);
            var numbers = raw.First().Numbers;
            var boards = raw.Select(board => board.Board).ToList();

            //Console.WriteLine($"Life Support Rating: {lifeSupportRating}");
        }
    }
}
