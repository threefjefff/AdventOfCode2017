using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Day3
{
    class Program
    {
        private static int Input => 347991;

        struct IncRank
        {
            public int Increment;
            public int Rank;
        }

        enum Compass
        {
            East = 0,
            North,
            West,
            South
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
            Console.Read();
        }

        static int Part2()
        {
            //OK FINE I'LL GENERATE YOUR STUPID GRID GOD
            var x = 0;
            var y = 0;
            var ringDepth = 0;
            var numInRing = 0;
            var numInDirection = 0;
            Compass direction = Compass.East;
            var firstSide = true;
            var grid = new Dictionary<string, int>();
            var latestValue = 0;
            grid["00"] = 1;
            do
            {
                if (numInRing == 0)
                {
                    ringDepth++;
                    numInRing = ValuesInRing(ringDepth);
                    numInDirection = 1;
                    direction = Compass.East; //Redundant, but worth stating
                    firstSide = true;
                }
                if (numInDirection == 0)
                {
                    direction = (int)direction + 1 > 3 ? 0 : direction+1;
                    numInDirection = ringDepth * 2;
                    if (firstSide)
                    {
                        numInDirection -= 1;
                        firstSide = false;
                    }
                }
                switch (direction)
                {
                    case Compass.East:
                        x++;
                        break;
                    case Compass.North:
                        y++;
                        break;
                    case Compass.West:
                        x--;
                        break;
                    case Compass.South:
                        y--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                latestValue = getValue(grid, x, y);
                grid["" + x + y] = latestValue;
                numInDirection--;
                numInRing--;
            } while (latestValue < Input);

            return latestValue;
        }

        static int getValue(Dictionary<string, int> grid, int x, int y)
        {
            var sum = SafeAccess(grid, x - 1,y);
            sum += SafeAccess(grid, x - 1,y - 1);
            sum += SafeAccess(grid,x,y -1);
            sum += SafeAccess(grid, x + 1,y - 1);
            sum += SafeAccess(grid, x + 1, y);
            sum += SafeAccess(grid, x + 1,y + 1);
            sum += SafeAccess(grid, x, y + 1);
            sum += SafeAccess(grid, x - 1, y + 1);
            return sum;
        }

        static int SafeAccess(Dictionary<string, int> grid, int x, int y)
        {
            grid.TryGetValue("" + x + y, out int value);
            return value;
        }

        static int ValuesInRing(int depth)
        {
            var prevRank = 0;
            var rank = 1;
            for (var i = 0; i < depth; i++)
            {
                prevRank = rank;
                rank += 2;
            }
            return rank * rank - prevRank * prevRank;
        }

        static int Part1()
        {
            return CalcDistance(CalcRank());
        }

        static IncRank CalcRank()
        {
            var rank = 1;
            for(var i=0;;i++)
            {
                if (rank*rank >= Input)
                    return new IncRank{Increment= i, Rank = rank};
                rank+=2;
            }
        }

        static int CalcDistance(IncRank incRank)
        {
            var max = incRank.Rank * incRank.Rank;
            var nextCompassRoseSteps = incRank.Increment; //e.g. steps to next rose dir, e.g. S -> SE
            var cornerNum = max - nextCompassRoseSteps * 2;
            for (var i = 0; i < 3; i++)
            {
                if (Input == cornerNum)
                {
                    return incRank.Increment + nextCompassRoseSteps;
                }
                if (Input > cornerNum)
                {
                    var stepsFromMidToInput = cornerNum + nextCompassRoseSteps <= Input
                        ? Input - (cornerNum + nextCompassRoseSteps) //closer to next corner
                        : nextCompassRoseSteps - (Input - cornerNum); //closer to this corner
                    return stepsFromMidToInput + incRank.Increment;
                }
                cornerNum -= nextCompassRoseSteps * 2;
            }
            return -1;
        }
    }
}