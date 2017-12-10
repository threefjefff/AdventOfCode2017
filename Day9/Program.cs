using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Day9
{
    class Program
    {
        static ICollection<string> ignoredRemoved = new List<string>();
        static ICollection<string> rubbishRemoved = new List<string>();
        private static string Input = File.ReadAllText("input.txt");

        static void Main(string[] args)
        {
            var noIgnoredChars = RemoveIgnored(Input);
            var assertAllIgnored = ignoredRemoved.All(x => x[0] == '!');
            var assertNoIgnoredLeft = noIgnoredChars.IndexOf('!') == -1;
            var noRubbish = RemoveRubbish(noIgnoredChars);
            var assertAllRubbish = rubbishRemoved.All(x => x[0] == '<' && x.Last() == '>');
            var assertOnlyGroupsLeft = !noRubbish.Any(x => x != '{' && x != '}' && x != ',');
            var totalRubbish = rubbishRemoved.Sum(x => x.Length - 2);
            var groupCount = CountGroupScore(noRubbish);
            Console.WriteLine(groupCount);
            Console.WriteLine(totalRubbish);
            Console.Read();
        }

        static string RemoveIgnored(string input)
        {
            var charInput = input.ToList();
            var index = charInput.IndexOf('!');
            while (index >= 0)
            {
                ignoredRemoved.Add($"{charInput[index]}{charInput[index + 1]}");
                charInput.RemoveAt(index);
                charInput.RemoveAt(index);
                index = charInput.IndexOf('!');
            }

            return new string(charInput.ToArray());
        }

        static string RemoveRubbish(string input)
        {
            var charInput = input.ToList();
            var endIndex = charInput.LastIndexOf('>');
            while (endIndex >= 0)
            {
                var nextLast = endIndex;
                int startIndex;
                do
                {
                    nextLast = charInput.LastIndexOf('>', nextLast - 1);
                    if (nextLast == -1)
                    {
                        startIndex = charInput.IndexOf('<');
                        break;
                    }
                    startIndex = charInput.IndexOf('<', nextLast);
                } while (startIndex == -1);
                rubbishRemoved.Add(new string(charInput.GetRange(startIndex, endIndex - startIndex + 1).ToArray()));
                charInput.RemoveRange(startIndex, endIndex - startIndex + 1);
                endIndex = charInput.LastIndexOf('>');
            }

            return new string(charInput.ToArray());
        }

        static int CountGroupScore(string input)
        {
            var score = 0;
            var open = 0;
            foreach (var c in input)
            {
                if (c == '{')
                {
                    open++;
                }
                else if (c == '}')
                {
                    score += open;
                    open--;
                }
            }
            return score;
        }
    }
}