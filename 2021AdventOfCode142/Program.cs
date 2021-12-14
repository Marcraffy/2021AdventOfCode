using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode142
{
    class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var instructions = Data.Data.GetData<PolymerizationInstruction>(DataFileKeys.POLYMER);

            var seed = instructions.First().Seed;
            var last = seed.Last();
            var pairs = GetPairs(seed);

            for (int step = 0; step < 10; step++)
            {
                var newSeed = string.Empty;
                foreach (var pair in pairs)
                {
                    var hasBeenPolymerized = false;
                    foreach (var instruction in instructions)
                    {
                        if (pair == instruction.Pair)
                        {
                            newSeed += instruction.Insert;
                            hasBeenPolymerized = true;
                            break;
                        }
                    }
                    if (!hasBeenPolymerized)
                    {
                        newSeed += pair;
                    }
                }

                newSeed += last;
                seed = newSeed;
                pairs = GetPairs(newSeed);
            }

            var grouped = seed.GroupBy(character => character).OrderByDescending(group => group.Count());
            var most = grouped.First().Count();
            var least = grouped.Last().Count();
            var diff = most - least;


            Console.WriteLine($"The final diff is: {diff}");
        }

        private static List<string> GetPairs(string seed)
        {
            var pairs = new List<string>();

            for (int index = 0; index < seed.Length - 1; index++)
            {
                pairs.Add($"{seed[index]}{seed[index+1]}");
            }

            return pairs;
        }
    }
}
