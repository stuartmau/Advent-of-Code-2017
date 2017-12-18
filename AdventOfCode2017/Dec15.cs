using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec15
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 15th - Dueling Generators -");
            Console.WriteLine("Part1");

            Part1(65, 16807, 8921, 48271, 2147483647, 588);
            Part1(699, 16807, 124, 48271, 2147483647, 600);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(65, 16807, 8921, 48271, 2147483647, 4, 8, 309);
            Part2(699, 16807, 124, 48271, 2147483647, 4, 8, 313);
        }

        /// <summary>
        /// From a simple sequence, count number of times the lowest 16 bits match. 
        /// </summary>
        public static void Part1(ulong A, ulong Af, ulong B, ulong Bf, ulong product, int? expected = null)
        {
            //long is Signed 64 bit
            long counter = 0;
            int match = 0;

            while( counter++ < 40_000_000)
            {
                A = (A * Af) % product;
                B = (B * Bf) % product;

                //check lowest 16 bits
                ulong Alow = (A << 48) >> 48;
                ulong Blow = (B << 48) >> 48;

                if (Alow == Blow)
                    match++;
            }

            Utilities.WriteOutput(match, expected);
        }


        /// <summary>
        /// From a slightly different simple sequence, count number of times the lowest 16 bits match. 
        /// </summary>
        public static void Part2(ulong A, ulong Af, ulong B, ulong Bf, ulong product, ulong Ac, ulong Bc, int? expected = null)
        {
            //long is Signed 64 bit
            long counter = 0;
            int match = 0;

            while (counter < 5_000_000)
            {
                //incrment A and B.
                A = (A * Af) % product;
                B = (B * Bf) % product;

                //incrment A and B if not multiples of their criteria. 
                while (A % Ac != 0)
                    A = (A * Af) % product;

                while (B % Bc != 0)
                    B = (B * Bf) % product;

                //check lowest 16 bits
                ulong Alow = (A << 48) >> 48;
                ulong Blow = (B << 48) >> 48;

                if (Alow == Blow)
                    match++;

                counter++;
            }

            Utilities.WriteOutput(match, expected);
        }

    }
}
