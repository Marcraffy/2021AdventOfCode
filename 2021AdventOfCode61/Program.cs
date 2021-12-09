using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode061
{
    static class Program
    {
        private const bool DEBUG = false;

        static void Main(string[] args)
        {
            var lives = Data.Data.GetData<Lives>(DataFileKeys.LIFE).First().StartingLives;
            if (DEBUG) Console.WriteLine($"Initial State: \t {lives.GetLives()}");

            for (int day = 1; day <= 80; day++)
            {
                lives = lives.SelectMany(Procreate).ToList();
                if (DEBUG) Console.WriteLine($"After {day} day{(day == 1 ? "" : "s")}: \t {lives.GetLives()}");
            }

            Console.WriteLine($"Number of Lanternfish: {lives.Count}");
        }

        private static IEnumerable<int> Procreate(int life)
        {
            if (life == 0)
            {
                return new List<int> { 6, 8 };
            }

            return new List<int> { life - 1 };
        }

        private static string GetLives(this List<int> lives) => lives.Aggregate("", (acc, val) => acc + val + ',').Trim(',');
    }
}
