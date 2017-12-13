using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2017
{
    public static class Dec00
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December xx - title -");
            Console.WriteLine("Part1");
            Part1("", 0);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2("", 6);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Part1(string input, int? expected = null)
        {
            int sum = 0;
            Utilities.WriteOutput(sum, expected);
        }


        /// <summary>
        /// 
        /// </summary>
        public static void Part2(string input, int? expected = null)
        {
            int sum = 0;
            Utilities.WriteOutput(sum, expected);
        }
    }
}
