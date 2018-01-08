using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public class Dec24
    {

        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 24th - Electromagnetic Moat -");
            Console.WriteLine("Part1");

            Part1(System.IO.Path.Combine(path, "dec24test.txt"), 31);
            Part1(System.IO.Path.Combine(path, "dec24.txt"), 1906);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(System.IO.Path.Combine(path, "dec24.txt"), 1824);
        }

        /// <summary>
        /// Find the strongest bridge.
        /// </summary>
        private static Result Part1(string filename, int? expected)
        {
            Bridges bridges = new Bridges();

            var input = Utilities.LoadStrings(filename);

            foreach (var line in input)
            {
                var split = line.Split('/');
                bridges.components.Add(new Component(int.Parse(split[0]), int.Parse(split[1])));
            }

            Recursive(0, 0, 0, bridges);

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(bridges.maxStrength, expected);
        }

        /// <summary>
        /// Find the strongest bridge of the longest bridges.
        /// </summary>
        private static Result Part2(string filename, int? expected)
        {
            Bridges bridges = new Bridges();

            var input = Utilities.LoadStrings(filename);

            foreach (var line in input)
            {
                var split = line.Split('/');
                bridges.components.Add(new Component(int.Parse(split[0]), int.Parse(split[1])));
            }

            Recursive(0, 0, 0, bridges);

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(bridges.maxStrengthLongest, expected);
        }


        /// <summary>
        /// Depth first search all possible bridges and calculate srengths. 
        /// </summary>
        private static void Recursive(int port, int length, int strength, Bridges bridges)
        {
            //Part 1
            bridges.maxStrength = Math.Max(bridges.maxStrength,strength);

            //Part 2
            if (length > bridges.maxLength)
            {
                bridges.maxStrengthLongest = strength;
                bridges.maxLength = length;
            }
            else if (length == bridges.maxLength)
            {
                bridges.maxStrengthLongest = Math.Max(bridges.maxStrengthLongest, strength);
            }
                
            //Find next parts to add to the bridge.
            foreach (var part in bridges.components)
            {
                if (!part.used && (part.a == port || part.b == port))
                {
                    part.used = true;

                    if (part.a == port)
                        Recursive(part.b, length + 1, strength + part.a + part.b, bridges);
                    else
                        Recursive(part.a, length + 1, strength + part.a + part.b, bridges);

                    part.used = false;
                }
            }
        }

        class Bridges
        {
            public int maxStrength = 0;
            public int maxLength = 0;
            public int maxStrengthLongest = 0;
            public List<Component> components = new List<Component>();
        }

        class Component
        {
            public int a, b;
            public bool used;

            public Component(int a, int b)
            {
                this.a = a;
                this.b = b;
            }
        }



        /// <summary>
        /// A neat solution from Sblom using Linq and ImmutableList version from Adventofcode reddit comments by Sblom
        /// https://www.reddit.com/r/adventofcode/comments/7lte5z/2017_day_24_solutions/drovr7b/
        /// </summary>
        private static void SblomLinq(string filename)
        {
            var lines = Utilities.LoadStrings(filename);

            IImmutableList<(int, int)> edges = ImmutableList<(int, int)>.Empty;

            foreach (var line in lines)
            {
                var nums = line.Split('/');
                edges = edges.Add((int.Parse(nums[0]), int.Parse(nums[1])));
            }

            int Search(IImmutableList<(int, int)> e, int cur = 0, int strength = 0)
            {
                return e.Where(x => x.Item1 == cur || x.Item2 == cur)
                    .Select(x => Search(e.Remove(x), x.Item1 == cur ? x.Item2 : x.Item1, strength + x.Item1 + x.Item2))
                    .Concat(Enumerable.Repeat(strength, 1)).Max();
            }

            (int, int) Search2(IImmutableList<(int, int)> e, int cur = 0, int strength = 0, int length = 0)
            {
                return e.Where(x => x.Item1 == cur || x.Item2 == cur)
                    .Select(x => Search2(e.Remove(x), x.Item1 == cur ? x.Item2 : x.Item1, strength + x.Item1 + x.Item2, length + 1))
                    .Concat(Enumerable.Repeat((strength, length), 1))
                    .OrderByDescending(x => x.Item2)
                    .ThenByDescending(x => x.Item1)
                    .First();
            }

            var part1 = Search(edges);
            var part2 = Search2(edges).Item1;
        }

    }
}
