using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    class Program
    {

        private static IEnumerable<List<string>> Input
        {
            get
            {
                var fileInput = File.ReadAllLines("input.txt");
                var jagInput = fileInput
                    .Select(row =>
                        row.Split(' ').ToList());
                return jagInput;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(ValidPassphrases(Part1));
            Console.WriteLine(ValidPassphrases(Part2));
            Console.ReadLine();
        }

        private static int ValidPassphrases(Func<List<string>,string,bool> compare)
        {
            return Input.Count(row => row.All(word => compare(row, word)));
        }

        private static bool Part1(List<string> phrase, string word) => phrase.Count(comp => word == comp) == 1;

        private static bool Part2(List<string> phrase, string word) => phrase.Count(comp => IsAnagram(word, comp)) == 1;

        private static bool IsAnagram(string word, string comp)
        {
            var sortedWord = string.Concat(word.OrderBy(c => c));
            var sortedComp = string.Concat(comp.OrderBy(c => c));
            return sortedWord.Equals(sortedComp);
        }
    }
}