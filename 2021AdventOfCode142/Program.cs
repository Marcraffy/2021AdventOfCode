using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode142
{
    class Program
    {
        private static void Main(string[] args)
        {
            var instructions = Data.Data.GetData<PolymerizationInstruction>(DataFileKeys.POLYMER);

            var seed = instructions.First().Seed;
            var count = new CharacterCount(seed);
            var pairs = GetPairs(seed.ToList());

            var dictionary = instructions.ToDictionary(instruction => instruction, instruction => 0L);
            var temp = instructions.Where(instruction => pairs.Contains(instruction.Pair));
            var tempAll = temp.Select(_ => _.Addition);
            var temp2 = tempAll.Where(_ => _ == 'F').ToList();

            foreach (var instruction in instructions.Where(instruction => pairs.Contains(instruction.Pair)))
            {
                var pairCount = pairs.Count(pair => pair == instruction.Pair);
                dictionary[instruction] += pairCount;
                count.Count(instruction.Addition, pairCount);
            }

            for (int step = 1; step < 40; step++)
            {
                Console.WriteLine($"Step: {step} start");

                var newDictionary = instructions.ToDictionary(instruction => instruction, instruction => 0L);
                foreach (var instruction in instructions)
                {
                    foreach (var instructionKeyValue in dictionary.Where(kv => kv.Value > 0 && kv.Key.Full.Contains(instruction.Pair)))
                    {
                        newDictionary[instruction] += instructionKeyValue.Value;
                        count.Count(instruction.Addition, instructionKeyValue.Value);
                    }
                }

                dictionary = newDictionary;

                Console.WriteLine($"Step: {step} complete");
            }

            var ordered = count.CountMap.OrderByDescending(c => c.Value);
            var most = ordered.First().Value;
            var least = ordered.Last().Value;
            var diff = most - least;

            Console.WriteLine($"The final diff is: {diff}");
        }

        private static List<string> GetPairs(List<char> seed)
        {
            var pairs = new List<string>();

            for (int index = 0; index < seed.Count - 1; index++)
            {
                pairs.Add($"{seed[index]}{seed[index + 1]}");
            }

            pairs.Add(seed.Last().ToString());

            return pairs;
        }
    }
    
    public class CharacterCount
    {
        public Dictionary<char, long> CountMap { get; set; }

        public CharacterCount(string seed)
        {
            CountMap = new Dictionary<char, long>();
            foreach (var character in seed)
                Count(character);
        }

        public void Count(char character, long number = 1)
        {
            if (CountMap.ContainsKey(character))
                CountMap[character] += number;
            else
                CountMap.Add(character, number);
        }

        public void CountAll(List<char> characters)
        {
            var counts = characters.GroupBy(character => character);
            foreach (var character in counts)
            {
                Count(character.Key, character.Count());
            }
        }
    }
}
