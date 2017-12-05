using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    using System.IO;

    class Program
    {
        private static IEnumerable<int> Input
        {
            get
            {
                var fileInput = File.ReadAllLines("input.txt");
                return fileInput
                    .Select(int.Parse);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Chomp(Part1));
            Console.WriteLine(Chomp(Part2));
            Console.Read();
        }

        static int Chomp(Func<int,int,int> calcNewValue)
        {
            var steps = Input.ToArray();

            var index = 0;
            var count = 0;
            var target = steps.Length;
            while (true)
            {
                var hopDistance = steps[index];
                if (index + hopDistance >= target || index + hopDistance < 0)
                {
                    count++;
                    return count;
                }
                steps[index] = calcNewValue(steps[index], hopDistance);
                index += hopDistance;
                count++;
            }
        }

        static int Part1(int value, int hopDistance) => value + 1;

        static int Part2(int value, int hopDistance) => Math.Abs(hopDistance) >= 3
            ? hopDistance > 0
                ? value - 1
                : hopDistance + 1
            : value + 1;
    }
}
