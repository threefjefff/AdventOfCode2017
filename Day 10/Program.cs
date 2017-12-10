using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Day_10
{
    class Program
    {
        private static IEnumerable<int> Input => "3,4,1,5"//"192,69,168,160,78,1,166,28,0,83,198,2,254,255,41,12"
            .Split(',')
            .Select(int.Parse);

        private const int MaxIndex = 5;
        private static List<int> _register = Enumerable.Range(0, MaxIndex).ToList();

        static void Main(string[] args)
        {
            Part1();
            Console.WriteLine();
        }

        static void Part1()
        {
            var startIndex = 0;
            var skip = 0;
            foreach (var length in Input)
            {
                OverwriteRegisters(startIndex, length);
                startIndex = WrapIncrement(startIndex, length + skip);
                skip++;
            }
        }

        static int WrapIncrement(int index, int steps)
        {
            return index + steps > MaxIndex - 1
                ? index + steps - MaxIndex - 1
                : index + steps;
        }

        static void OverwriteRegisters(int index, int steps)
        {
            var stepsToTake = index + steps > MaxIndex ? MaxIndex- index : steps;
            var stepsLeft = index + steps > MaxIndex ? index + steps - MaxIndex : 0;
            var untouched = index + steps > MaxIndex
                ? _register.GetRange(stepsLeft, MaxIndex - stepsLeft - stepsToTake)
                : _register.GetRange(steps, MaxIndex - stepsLeft - stepsToTake);
            var valuesToOverwrite = _register.GetRange(index, stepsToTake);
            if (stepsLeft > 0)
            {
                valuesToOverwrite.AddRange(_register.GetRange(0, stepsLeft));
            }

            valuesToOverwrite.Reverse();
            _register.Clear();

            if (stepsLeft > 0)
            {
                _register = valuesToOverwrite
                    .Skip(stepsToTake)
                    .Take(stepsLeft)
                    .ToList();
                valuesToOverwrite.RemoveRange(stepsToTake,stepsLeft);
                _register.AddRange(untouched);
                _register.AddRange(valuesToOverwrite.Take(stepsToTake));
            }
            else
            {
                _register.AddRange(valuesToOverwrite.Take(stepsToTake));
                _register.AddRange(untouched);
            }
        }
    }
}