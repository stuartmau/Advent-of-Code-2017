using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec17
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 17th - Spinlock -");
            Console.WriteLine("Part1");
            Part1(3, 638);
            Part1(303,1971);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(303, 17202899);
            Part2a(303, 17202899);
        }

        /// <summary>
        /// Find the value after the final insertion index. 
        /// </summary>
        public static Result Part1(int input, int? expected = null)
        {
            List<int> buffer = new List<int>{0};

            int index = 0;

            for (int i = 1; i <= 2017; i++)
            {
                index = (index + input) % buffer.Count() + 1;
                buffer.Insert(index, i);
            }

            var result = buffer[(index + 1) % buffer.Count];
            return Utilities.WriteOutput(result, expected);
        }


        /// <summary>
        /// Find the value in index 1 after the final insertion index. 
        /// </summary>
        public static Result Part2(int input, int? expected = null)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int[] buffer = new int[50_000_000];
            int index = 0;

            for (int i = 1; i <= 50_000_000; i++)
            {
                index = (index + input) % i + 1;
                buffer[index] = i;
            }
            sw.Stop();
            
            var result = buffer[1];
            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds.ToString());
            return Utilities.WriteOutput(result, expected);
        }

        /// <summary>
        /// Find the value in index 1 after the final insertion index 
        /// without allocating the entire array. 
        /// </summary>
        public static Result Part2a(int input, int? expected = null)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int index = 0;
            int result = 0;
            for (int i = 1; i <= 50_000_000; i++)
            {
                index = (index + input) % i + 1;
                if (index == 1)
                    result = i;
            }
            sw.Stop();

            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds.ToString());
            return Utilities.WriteOutput(result, expected);

        }

    }
}
