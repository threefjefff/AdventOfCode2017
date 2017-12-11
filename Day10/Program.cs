using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Day_10
{
    using System.Text;

    class Program
    {
        private static IEnumerable<int> Input => "192,69,168,160,78,1,166,28,0,83,198,2,254,255,41,12"
            .Select(x => (int)x)
            .Concat(KeySequence);

        private static IEnumerable<int> KeySequence => "17,31,73,47,23"
            .Split(',')
            .Select(int.Parse);

        private const int MaxIndex = 256;
        private static List<int> _register = Enumerable.Range(0, MaxIndex).ToList();
        private static int _currentPosition;
        private static int _skip;

        static void Main(string[] args)
        {
            Console.WriteLine(FullHash());
            Console.Read();
        }

        static string FullHash()
        {
            for (var i = 0; i < 64; i++)
            {
                KnotHashPass();
            }

            var sb = new StringBuilder();
            for (var x = 0; x < 16; x++)
            {
                var denseDigit = _register[x*16];
                for (var y = 1; y < 16; y++)
                {
                    denseDigit = denseDigit ^ _register[x * 16 + y];
                }
                sb.AppendFormat("{0:x2}", denseDigit);
            }
            return sb.ToString();
        }

        static void KnotHashPass()
        {
            foreach (var length in Input)
            {
                OverwriteRegisters(_currentPosition, length);
                _currentPosition = WrapIncrement(_currentPosition, length + _skip);
                _skip++;
            }
        }

        static int WrapIncrement(int index, int steps)
        {
            var newIndex = index + steps;
            while (newIndex > MaxIndex)
            {
                newIndex -= MaxIndex;
            }
            return newIndex;
        }

        static void OverwriteRegisters(int index, int steps)
        {
            var stepsToTake = index + steps > MaxIndex ? MaxIndex - index : steps;
            var stepsLeft = index + steps > MaxIndex ? index + steps - MaxIndex : 0;
            var valuesToOverwrite = _register.GetRange(index, stepsToTake);
            if (stepsLeft > 0)
            {
                valuesToOverwrite.AddRange(_register.GetRange(0, stepsLeft));
            }

            valuesToOverwrite.Reverse();

            if (stepsLeft > 0)
            {
                var untouched = index + steps > MaxIndex
                    ? _register.GetRange(stepsLeft, MaxIndex - stepsLeft - stepsToTake)
                    : _register.GetRange(steps, MaxIndex - stepsLeft - stepsToTake);
                _register.Clear();
                _register = valuesToOverwrite
                    .Skip(stepsToTake)
                    .Take(stepsLeft)
                    .ToList();
                valuesToOverwrite.RemoveRange(stepsToTake, stepsLeft);
                _register.AddRange(untouched);
                _register.AddRange(valuesToOverwrite.Take(stepsToTake));
            }
            else
            {
                _register.RemoveRange(index, steps);
                _register.InsertRange(index, valuesToOverwrite);
            }
        }
    }
}