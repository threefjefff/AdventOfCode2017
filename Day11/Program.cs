namespace Day11
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Policy;

    class Program
    {
        private static IEnumerable<string> Input => File.ReadAllText("input.txt")
            .Split(',');

        struct CubeCoord
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }

        private static int _maxDistance;

        static void Main(string[] args)
        {
            var childAt = FollowPath();
            Console.WriteLine(ManhatanCube(childAt));
            Console.WriteLine(_maxDistance);
            Console.Read();
        }

        static CubeCoord FollowPath()
        {
            var pos = new CubeCoord();
            foreach (var direction in Input)
            {
                if (direction.Equals("n"))
                {
                    pos.Z--;
                    pos.Y++;
                }
                if (direction.Equals("s"))
                {
                    pos.Z++;
                    pos.Y--;
                }
                if (direction.Contains("ne"))
                {
                    pos.X++;
                    pos.Z--;
                }
                if (direction.Contains("nw"))
                {
                    pos.Y++;
                    pos.X--;
                }
                if (direction.Contains("se"))
                {
                    pos.Y--;
                    pos.X++;
                }
                if (direction.Contains("sw"))
                {
                    pos.X--;
                    pos.Z++;
                }
                var distance = ManhatanCube(pos);
                _maxDistance = distance > _maxDistance ? distance : _maxDistance;
            }

            return pos;
        }

        static int ManhatanCube(CubeCoord pos)
        {
            var max = Math.Max(Math.Abs(pos.X), Math.Abs(pos.Y));
            return Math.Max(max, Math.Abs(pos.Z));
        }
    }
}
