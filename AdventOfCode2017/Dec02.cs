using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2017
{
    class Dec02
    {
        internal static void Run()
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 2nd Part1");
            string path = @"C:\Users\stuart\Documents\Visual Studio 2017\Projects\AdventOfCode2017\AdventOfCode2017\input\";

            Part1(Path.Combine(path, "dec02part1test.txt"), 18);
            Part1(Path.Combine(path, "dec02.txt"), 46402);

            Console.WriteLine();
            Console.WriteLine("December 2nd Part2: ");
            Part2(Path.Combine(path, "dec02part2test.txt"), 9);
            Part2(Path.Combine(path, "dec02.txt"), 265);
        }


        private static void Part1(string filename, int? expected = null)
        {
            List<List<int>> spreadsheet = Utilities.LoadIntArrays(filename);

            //for each row, find largest, and smallest values. 
            //sum the difference each for each row. 

            int sum = 0;
            foreach(var row in spreadsheet)
            {
                int min = int.MaxValue;
                int max = int.MinValue;

                foreach (var cell in row)
                {
                    if (cell > max)
                        max = cell;
                    if (cell < min)
                        min = cell;
                }

                sum += max - min;
            }

            //report
            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(sum, expected);


        }

        private static void Part2(string filename, int? expected = null)
        {
            List<List<int>> spreadsheet = Utilities.LoadIntArrays(filename);

            //for each row, find largest, and smallest values. 
            //sum the difference each for each row. 

            int sum = 0;
            foreach (var row in spreadsheet)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    for (int j = i+1; j < row.Count; j++)
                    {
                        //check for remainder
                        int remainder;
                        int var1 = Math.Max(row[i], row[j]);
                        int var2 = Math.Min(row[i], row[j]);

                        int value = Math.DivRem(var1, var2, out remainder);

                        if ( remainder == 0)
                            sum += value;
                    }

                }

            }

            //report
            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(sum, expected);

        }


    }


}

