using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2017
{
    public static class Dec09
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 9th - Stream Processing -");
            Console.WriteLine("Part1");

            Part1Evaluate("{}", 1);
            Part1Evaluate("{{{}}}", 6);
            Part1Evaluate("{{},{}}", 5);
            Part1Evaluate("{<a>,<a>,<a>,<a>}", 1);
            Part1Evaluate("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9);
            Part1Evaluate("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9);
            Part1Evaluate("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3);

            Part1(Path.Combine(path, "dec09.txt"), 16021);

            Console.WriteLine();
            Console.WriteLine("Part2");

            Part2Evaluate("<>", 0);
            Part2Evaluate("<random characters>", 17);
            Part2Evaluate("<<<<>", 3);
            Part2Evaluate("<{!>}>", 2);
            Part2Evaluate("<!!>", 0);
            Part2Evaluate("<!!!>>", 0);
            Part2Evaluate(@"<{o""i!a,<{i<a>", 10);

            Part2(Path.Combine(path, "dec09.txt"), 7685);
        }

        /// <summary>
        /// Count parsed groups.
        /// </summary>
        public static void Part1(string filename, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            Utilities.WriteInputFile(filename);

            foreach (var line in strings)
                Part1Evaluate(line, expected);
        }


        /// <summary>
        /// Count non cancelleed characters within garbarge.
        /// </summary>
        public static void Part2(string filename, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            Utilities.WriteInputFile(filename);

            foreach (var line in strings)
                Part2Evaluate(line, expected);
        }

        /// <summary>
        /// Count parsed groups.
        /// </summary>
        private static int Part1Evaluate(string input, int? expected = null)
        {
            int index = 0;
            int group = 0;
            int score = 0;
            bool garbage = false;

            while (index < input.Length)
            {
                char c = input[index++];

                if (c == '!')
                    index++;
                else if (!garbage && c == '{')
                    group++;
                else if (!garbage && c == '}')
                {
                    score += group;
                    group--;
                }
                else if (!garbage && c == '<')
                    garbage = true;
                else if (garbage && c == '>')
                    garbage = false;

            }

            if (input.Length < Console.BufferWidth)
                Console.WriteLine(input);

            Utilities.WriteOutput(score, expected);

            return score;
        }

        /// <summary>
        /// Count non cancelleed characters within garbarge.
        /// </summary>
        private static int Part2Evaluate(string input, int? expected = null)
        {
            int index = 0;
            int group = 0;
            int score = 0;
            int garbageCount = 0;
            bool garbage = false;

            while (index < input.Length)
            {
                char c = input[index++];

                if (c == '!')
                    index++;
                else if (!garbage && c == '{')
                    group++;
                else if (!garbage && c == '}')
                {
                    score += group;
                    group--;
                }
                else if (!garbage && c == '<')
                    garbage = true;
                else if (garbage && c == '>')
                    garbage = false;
                else if (garbage)
                    garbageCount++;

            }

            if (input.Length < Console.BufferWidth)
                Console.WriteLine(input);

            Utilities.WriteOutput(garbageCount, expected);

            return score;
        }
    }
}
