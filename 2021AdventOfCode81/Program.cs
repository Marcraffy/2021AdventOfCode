using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode81
{
    static class Program
    {
        private const bool DEBUG = true;

        static void Main(string[] args)
        {
            var displays = Data.Data.GetData<Display>(DataFileKeys.DISPLAY);

            var count = 0;
            foreach (var display in displays)
            {
                count += display.Count147And8s();
            }


            Console.WriteLine($"1, 4, 7, and 8 appear:\t {count} times");
        }

        static int Count147And8s(this Display display)
        {
            var count = 0;
            foreach (var module in display.DisplayOutput)
            {
                count += module.Is147Or8() ? 1 : 0;
            }

            return count;
        }

        static bool Is147Or8(this SevenSegment module)
        {
            var activeSegmentCount = 0;
            activeSegmentCount += (module.SegmentA) ? 1 : 0;
            activeSegmentCount += (module.SegmentB) ? 1 : 0;
            activeSegmentCount += (module.SegmentC) ? 1 : 0;
            activeSegmentCount += (module.SegmentD) ? 1 : 0;
            activeSegmentCount += (module.SegmentE) ? 1 : 0;
            activeSegmentCount += (module.SegmentF) ? 1 : 0;
            activeSegmentCount += (module.SegmentG) ? 1 : 0;

            return activeSegmentCount == 2
                || activeSegmentCount == 3
                || activeSegmentCount == 4
                || activeSegmentCount == 7;
        }
    }
}
