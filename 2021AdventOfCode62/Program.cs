using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode62
{
    static class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var lives = Data.Data.GetData<Lives>(DataFileKeys.LIFE).First().StartingLives;
            if (DEBUG) Console.WriteLine($"Initial State: \t {lives.Count}");

            var lanternfish = new LanternfishCount(lives);
            lanternfish.AdvanceDays(256);

            Console.WriteLine($"Number of Lanternfish:\t {lanternfish.TotalNumberOfFish}");
        }
    }

    public class LanternfishCount
    {
        public long NumberOfFishWithTimer0 { get; set; }
        public long NumberOfFishWithTimer1 { get; set; }
        public long NumberOfFishWithTimer2 { get; set; }
        public long NumberOfFishWithTimer3 { get; set; }
        public long NumberOfFishWithTimer4 { get; set; }
        public long NumberOfFishWithTimer5 { get; set; }
        public long NumberOfFishWithTimer6 { get; set; }
        public long NumberOfFishWithTimer7 { get; set; }
        public long NumberOfFishWithTimer8 { get; set; }

        public long TotalNumberOfFish => NumberOfFishWithTimer0
                                       + NumberOfFishWithTimer1
                                       + NumberOfFishWithTimer2
                                       + NumberOfFishWithTimer3
                                       + NumberOfFishWithTimer4
                                       + NumberOfFishWithTimer5
                                       + NumberOfFishWithTimer6
                                       + NumberOfFishWithTimer7
                                       + NumberOfFishWithTimer8;

        public LanternfishCount(List<int> lives)
        {
            NumberOfFishWithTimer0 = CountFishByTimer(lives, 0);
            NumberOfFishWithTimer1 = CountFishByTimer(lives, 1);
            NumberOfFishWithTimer2 = CountFishByTimer(lives, 2);
            NumberOfFishWithTimer3 = CountFishByTimer(lives, 3);
            NumberOfFishWithTimer4 = CountFishByTimer(lives, 4);
            NumberOfFishWithTimer5 = CountFishByTimer(lives, 5);
            NumberOfFishWithTimer6 = CountFishByTimer(lives, 6);
            NumberOfFishWithTimer7 = CountFishByTimer(lives, 7);
            NumberOfFishWithTimer8 = CountFishByTimer(lives, 8);
        }

        private int CountFishByTimer(List<int> lives, int timer) => lives.Where(life => life == timer).Count();

        public long AdvanceOneDay()
        {
            var numberOfFishSpawning = NumberOfFishWithTimer0;
            NumberOfFishWithTimer0 = NumberOfFishWithTimer1;
            NumberOfFishWithTimer1 = NumberOfFishWithTimer2;
            NumberOfFishWithTimer2 = NumberOfFishWithTimer3;
            NumberOfFishWithTimer3 = NumberOfFishWithTimer4;
            NumberOfFishWithTimer4 = NumberOfFishWithTimer5;
            NumberOfFishWithTimer5 = NumberOfFishWithTimer6;
            NumberOfFishWithTimer6 = NumberOfFishWithTimer7;
            NumberOfFishWithTimer7 = NumberOfFishWithTimer8;

            NumberOfFishWithTimer8 = numberOfFishSpawning;
            NumberOfFishWithTimer6 += numberOfFishSpawning;

            return TotalNumberOfFish;
        }

        public long AdvanceDays(int days)
        {
            if (days == 0)
            {
                return TotalNumberOfFish;
            }

            AdvanceOneDay();

            return AdvanceDays(days - 1);
        }
    }
}
