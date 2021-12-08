using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode82
{
    internal static class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var displays = Data.Data.GetData<Display>(DataFileKeys.DISPLAY);

            var count = 0;
            foreach (var display in displays)
            {
                var output = display.Translate().Output();
                Console.WriteLine($"Display output:\t {output}");
                count += output;
            }

            Console.WriteLine($"Display output total:\t {count}");
        }

        private static List<TranslatedSevenSegment> Translate(this Display display)
        {
            SevenSegment one = display.SampleModules.FirstOrDefault(IsOne);
            SevenSegment four = display.SampleModules.FirstOrDefault(IsFour);
            SevenSegment seven = display.SampleModules.FirstOrDefault(IsSeven);
            SevenSegment eight = display.SampleModules.FirstOrDefault(IsEight);

            SevenSegment zero = null;
            SevenSegment six = null;
            SevenSegment nine = null;

            SevenSegment segmentA = null;
            SevenSegment segmentB = null;
            SevenSegment segmentC = null;
            SevenSegment segmentD = null;
            SevenSegment segmentE = null;
            SevenSegment segmentF = null;
            SevenSegment segmentG = null;

            var onesIndexes = one.GetIndexes();

            segmentA = (seven - one);

            var completed = false;
            do
            {
                foreach (var item in display.SampleModules)
                {
                    if ((one == item || four == item || seven == item || eight == item)
                        || (zero == item)
                        || (six == item)
                        || (nine == item))
                    {
                        continue;
                    }

                    if (item.CountActiveSegments() == 6)
                    {
                        if (zero == null)
                        {
                            if (six != null && six != item
                             && nine != null && nine != item)
                            {
                                zero = item;
                                segmentD = eight - zero;
                                segmentB = four - one - segmentD;
                            }
                        }

                        if (six == null)
                        {
                            if (onesIndexes.Count(index => item.HasMatchingIndexes(index)) == 1)
                            {
                                six = item;
                                segmentC = one - six;
                                segmentF = one - segmentC;
                            }
                        }

                        if (nine == null)
                        {
                            var fourPlusSeven = (four + seven);

                            var indexDifference = item.GetDifference(fourPlusSeven);
                            if (indexDifference.Count == 2)
                            {
                                nine = item;
                                segmentE = eight - nine;
                                segmentG = nine - fourPlusSeven;
                            }
                        }
                    }

                    if (segmentA != null
                     && segmentB != null
                     && segmentC != null
                     && segmentD != null
                     && segmentE != null
                     && segmentF != null
                     && segmentG != null)
                    {
                        completed = true;
                        break;
                    }
                }
            } while (!completed);

            return display.DisplayOutput.Select(module =>
            {
                var indexes = module.GetIndexes();
                var translatedSegmentA = indexes.Contains(segmentA.GetSingleIndex());
                var translatedSegmentB = indexes.Contains(segmentB.GetSingleIndex());
                var translatedSegmentC = indexes.Contains(segmentC.GetSingleIndex());
                var translatedSegmentD = indexes.Contains(segmentD.GetSingleIndex());
                var translatedSegmentE = indexes.Contains(segmentE.GetSingleIndex());
                var translatedSegmentF = indexes.Contains(segmentF.GetSingleIndex());
                var translatedSegmentG = indexes.Contains(segmentG.GetSingleIndex());
                return new TranslatedSevenSegment(translatedSegmentA, translatedSegmentB, translatedSegmentC, translatedSegmentD, translatedSegmentE, translatedSegmentF, translatedSegmentG);
            }).ToList();
        }

        private static int Output(this List<TranslatedSevenSegment> translatedModule)
        {
            var digits = translatedModule[0].Output().ToString()
                       + translatedModule[1].Output().ToString()
                       + translatedModule[2].Output().ToString()
                       + translatedModule[3].Output().ToString();

            var output = Convert.ToInt32(digits);
            return output;
        }

        private static int CountActiveSegments(this SevenSegment module)
        {
            var activeSegmentCount = 0;
            activeSegmentCount += (module.SegmentA) ? 1 : 0;
            activeSegmentCount += (module.SegmentB) ? 1 : 0;
            activeSegmentCount += (module.SegmentC) ? 1 : 0;
            activeSegmentCount += (module.SegmentD) ? 1 : 0;
            activeSegmentCount += (module.SegmentE) ? 1 : 0;
            activeSegmentCount += (module.SegmentF) ? 1 : 0;
            activeSegmentCount += (module.SegmentG) ? 1 : 0;
            return activeSegmentCount;
        }

        private static List<int> GetIndexes(this SevenSegment module)
        {
            var activeSegmentCount = new List<int>();
            activeSegmentCount.Add((module.SegmentA) ? 1 : 0);
            activeSegmentCount.Add((module.SegmentB) ? 2 : 0);
            activeSegmentCount.Add((module.SegmentC) ? 3 : 0);
            activeSegmentCount.Add((module.SegmentD) ? 4 : 0);
            activeSegmentCount.Add((module.SegmentE) ? 5 : 0);
            activeSegmentCount.Add((module.SegmentF) ? 6 : 0);
            activeSegmentCount.Add((module.SegmentG) ? 7 : 0);
            return activeSegmentCount.Where(index => index != 0).ToList();
        }

        private static List<int> GetDifference(this SevenSegment moduleA, SevenSegment moduleB)
        {
            var segmentDifferences = new List<int>();
            segmentDifferences.Add(!(moduleA.SegmentA && moduleB.SegmentA) ? 1 : 0);
            segmentDifferences.Add(!(moduleA.SegmentB && moduleB.SegmentB) ? 2 : 0);
            segmentDifferences.Add(!(moduleA.SegmentC && moduleB.SegmentC) ? 3 : 0);
            segmentDifferences.Add(!(moduleA.SegmentD && moduleB.SegmentD) ? 4 : 0);
            segmentDifferences.Add(!(moduleA.SegmentE && moduleB.SegmentE) ? 5 : 0);
            segmentDifferences.Add(!(moduleA.SegmentF && moduleB.SegmentF) ? 6 : 0);
            segmentDifferences.Add(!(moduleA.SegmentG && moduleB.SegmentG) ? 7 : 0);
            return segmentDifferences.Where(index => index != 0).ToList();
        }

        private static bool HasMatchingIndexes(this SevenSegment module, int index) 
            => module.GetIndexes().Any(i => i == index);

        private static int GetSingleIndex(this SevenSegment module) 
            => module.GetIndexes().Single();

        private static bool IsOne(this SevenSegment module)
            => module.CountActiveSegments() == 2;

        private static bool IsFour(this SevenSegment module)
            => module.CountActiveSegments() == 4;

        private static bool IsSeven(this SevenSegment module)
            => module.CountActiveSegments() == 3;

        private static bool IsEight(this SevenSegment module)
            => module.CountActiveSegments() == 7;
    }

    public class TranslatedSevenSegment : SevenSegment
    {
        public TranslatedSevenSegment(bool a, bool b, bool c, bool d, bool e, bool f, bool g)
        {
            SegmentA = a;
            SegmentB = b;
            SegmentC = c;
            SegmentD = d;
            SegmentE = e;
            SegmentF = f;
            SegmentG = g;
        }

        public TranslatedSevenSegment(string input) : base(input)
        {
        }

        public int Output()
        {
            if (SegmentA && SegmentB && SegmentC && !SegmentD && SegmentE && SegmentF && SegmentG)
                return 0;
            if (!SegmentA && !SegmentB && SegmentC && !SegmentD && !SegmentE && SegmentF && !SegmentG)
                return 1;
            if (SegmentA && !SegmentB && SegmentC && SegmentD && SegmentE && !SegmentF && SegmentG)
                return 2;
            if (SegmentA && !SegmentB && SegmentC && SegmentD && !SegmentE && SegmentF && SegmentG)
                return 3;
            if (!SegmentA && SegmentB && SegmentC && SegmentD && !SegmentE && SegmentF && !SegmentG)
                return 4;
            if (SegmentA && SegmentB && !SegmentC && SegmentD && !SegmentE && SegmentF && SegmentG)
                return 5;
            if (SegmentA && SegmentB && !SegmentC && SegmentD && SegmentE && SegmentF && SegmentG)
                return 6;
            if (SegmentA && !SegmentB && SegmentC && !SegmentD && !SegmentE && SegmentF && !SegmentG)
                return 7;
            if (SegmentA && SegmentB && SegmentC && SegmentD && SegmentE && SegmentF && SegmentG)
                return 8;
            if (SegmentA && SegmentB && SegmentC && SegmentD && !SegmentE && SegmentF && SegmentG)
                return 9;
            else throw new ArgumentException("Untranslated Segment");
        }
    }
}