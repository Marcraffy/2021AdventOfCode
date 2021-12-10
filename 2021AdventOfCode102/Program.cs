using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode102
{
    static class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var syntaxRows = Data.Data.GetData<Syntax>(DataFileKeys.SYNTAX);
            var missingRows = new List<List<char>>();

            foreach (var syntax in syntaxRows)
            {
                var stack = new Stack<char>();
                var isCorrupt = false;

                foreach (var character in syntax.Brackets)
                {
                    if (Syntax.AcceptedOpeningBrackets.Any(bracket => bracket == character))
                    {
                        stack.Push(character);
                    }
                    else if (Syntax.AcceptedClosingBrackets.Any(bracket => bracket == character))
                    {
                        var matchingOpeningBracket = Syntax.AcceptedOpeningBrackets[Array.IndexOf(Syntax.AcceptedClosingBrackets, character)];
                        if (stack.Count == 0)
                        {
                            isCorrupt = true;
                            break;
                        }

                        var last = stack.Peek();
                        if (!Syntax.AcceptedOpeningBrackets.Any(bracket => bracket == last))
                        {
                            isCorrupt = true;
                            break;
                        }

                        if (matchingOpeningBracket != last)
                        {
                            isCorrupt = true;
                            break;
                        }

                        stack.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("Must be bracket");
                    }
                }

                if (stack.Count > 0 && !isCorrupt)
                {
                    var missingBrackets = new List<char>();
                    while (stack.Count > 0)
                    {
                        var next = stack.Pop();
                        var matchingClosingBracket = Syntax.AcceptedClosingBrackets[Array.IndexOf(Syntax.AcceptedOpeningBrackets, next)];
                        missingBrackets.Add(matchingClosingBracket);
                    }
                    missingRows.Add(missingBrackets);
                }
            }

            var finalScores = new List<long>();

            foreach (var missingBrackets in missingRows)
            {
                var total = 0L;

                foreach (var missingBracket in missingBrackets)
                {
                    total *= 5;
                    total += missingBracket switch
                    {
                        ')' => 1,
                        ']' => 2,
                        '}' => 3,
                        '>' => 4,
                        _ => throw new ArgumentException()
                    };
                }

                finalScores.Add(total);
            }

            var orderedScores = finalScores.OrderBy(score => score);
            var score = orderedScores.Skip((orderedScores.Count() - 1) / 2).First();

            Console.WriteLine($"The final score is: {score}");
        }
    }
}
