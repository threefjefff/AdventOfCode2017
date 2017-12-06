namespace Day6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    class Program
    {
        private static int[] Input => "10	3	15	10	5	15	5	15	9	2	5	8	5	2	3	6"
            .Split('\t')
            .Select(int.Parse)
            .ToArray();

        private static int[] _memBanks = Input;
        private static HashSet<string> _seenStates = new HashSet<string>();

        static void Main(string[] args)
        {
            Console.WriteLine(DetectLoop());
            Console.WriteLine(DetectLoop());
            Console.Read();
        }

        static int DetectLoop()
        {
            var count = 0;
            do
            {
                _memBanks = Redistribute(_memBanks);
                count++;
            } while (_seenStates.Add(Pickle(_memBanks)));
            _seenStates.Clear();
            _seenStates.Add(Pickle(_memBanks));
            return count;
        }

        private static string Pickle(IEnumerable<int> collection) => string.Join("", collection.ToArray());

        private static int[] Redistribute(int[] collection)
        {
            var max = collection.Max();
            var index = 0;
            var left = 0;
            for (var i = 0; i < collection.Length; i++)
            {
                if (collection[i] != max) continue;
                left = collection[i];
                index = i;
                collection[i] = 0;
                break;
            }
            index = WrapIncrement(index, collection.Length - 1);
            while (left > 0)
            {
                collection[index] = collection[index] + 1;
                left--;
                index = WrapIncrement(index, collection.Length - 1);
            }

            return collection;
        }

        private static int WrapIncrement(int value, int max) => value == max ? 0 : value + 1;
    }
}
