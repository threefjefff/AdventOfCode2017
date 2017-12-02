using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        private static IEnumerable<int[]> Input
        {
            get
            {
                var fileInput = File.ReadAllLines("input.txt");
                var jagInput = fileInput
                    .Select(row =>
                        row.Split('\t')
                            .Select(int.Parse)
                            .ToArray());
                return jagInput;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Chomp(calcRow1));
            Console.WriteLine(Chomp(calcRow2));
            Console.Read();
        }

        static int Chomp(Func<int[],int> rowCalc)
        {
            var jagInput = Input;
            var checkSum = jagInput
                .Select(rowCalc)
                .Aggregate((count, x) => count + x);
            return checkSum;
        }

        static int calcRow1(int[] row) => row.Max() - row.Min();

        static int calcRow2(int[] row)
        {
            var seq = row.Where(x =>
                    row.Where(y => y != x)
                        .Any(y => y % x == 0 || x % y == 0))
                .ToArray();
            return seq[0] % seq[1] == 0 ? seq[0] / seq[1] : seq[1] / seq[0];
        }
    }
}