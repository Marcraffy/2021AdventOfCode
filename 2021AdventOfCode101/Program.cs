using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021AdventOfCode101
{
    static class Program
    {
        private const bool DEBUG = true;

        private static void Main(string[] args)
        {
            var syntaxRows = Data.Data.GetData<Syntax>(DataFileKeys.SYNTAX);
            var illegalErrors = new List<char>();

            foreach (var syntax in syntaxRows)
            {
                var stack = new Stack<char>();
                var hasFailed = false;
                var intendedOpeningValue = ' ';
                var providedCharacter = ' ';
                var stackLast = ' ';

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
                            hasFailed = true;
                            intendedOpeningValue = matchingOpeningBracket;
                            providedCharacter = character;
                            stackLast = '0';
                            break;
                        }

                        var last = stack.Peek();
                        if (!Syntax.AcceptedOpeningBrackets.Any(bracket => bracket == last))
                        {
                            hasFailed = true;
                            intendedOpeningValue = matchingOpeningBracket;
                            providedCharacter = character;
                            stackLast = last;
                            break;
                        }

                        if (matchingOpeningBracket != last)
                        {
                            hasFailed = true;
                            intendedOpeningValue = matchingOpeningBracket;
                            providedCharacter = character;
                            stackLast = last;
                            break;
                        }
                        
                        stack.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("Must be bracket");
                    }
                }

                if (hasFailed)
                {
                    illegalErrors.Add(providedCharacter);
                }
            }

            var score1 = illegalErrors.Where(error => error == ')').Count() * 3;
            var score2 = illegalErrors.Where(error => error == ']').Count() * 57;
            var score3 = illegalErrors.Where(error => error == '}').Count() * 1197;
            var score4 = illegalErrors.Where(error => error == '>').Count() * 25137;
            var finalScore = score1 + score2 + score3 + score4;

            Console.WriteLine($"The final score is: {finalScore}");
        }
    }
}
