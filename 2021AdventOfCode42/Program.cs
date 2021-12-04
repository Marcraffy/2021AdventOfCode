using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode42
{
    class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var raw = Data.Data.GetData<BingoBoard>(DataFileKeys.BINGO);
            var numbers = raw.First().Numbers;
            var boards = raw.Select(board => new Board(board)).ToList();
            var score = GetLastWinningBoardScore(numbers, boards);

            Console.WriteLine($"Last Bingo Winner Score: {score}");
        }

        private static int? GetLastWinningBoardScore(int[] numbers, List<Board> boards)
        {
            var winningBoardsScores = new Stack<int>();
            var numberIndex = 0;
            foreach (var number in numbers)
            {
                var winningBoards = new List<Board>();
                foreach (var board in boards)
                {
                    if (!board.HasWon && board.CheckNumber(number))
                    {
                        winningBoards.Add(board);
                    }
                }
                if (winningBoards.Any())
                {
                    var score = winningBoards.Last().Score;
                    winningBoardsScores.Push(winningBoards.Last().Score);
                }
                numberIndex++;
            }

            return winningBoardsScores.Any() ? winningBoardsScores.Pop() : 0;
        }
    }
}
