namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var captchaList = input
                .ToCharArray()
                .Select(x => int.Parse(x.ToString())).ToArray();
            var count = 0;
            var wrap = captchaList.Length / 2;
            for (int i = 0; i < captchaList.Length; i++)
            {
                var curr = captchaList[i];
                var stepLeft = captchaList.Length - 1 - i;
                var thisWrap = stepLeft - wrap;
                var comp = thisWrap < 0 ? captchaList[thisWrap * -1 - 1] : captchaList[captchaList.Length - 1 - thisWrap];
                count += curr == comp ? curr : 0;
            }

            Console.WriteLine(count);
            Console.Read();
        }
    }
}
