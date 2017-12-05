using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2017
{
    public static class Dec05
    {
        public static void Run(string path)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 5th - A Maze of Twisty Trampolines, All Alike - Part1");

            Part1(Path.Combine(path, "dec05test.txt"), 5, Path.Combine(path, "dec05part1testfinalstate.txt"));
            Part1(Path.Combine(path, "dec05.txt"), 376976);


            Console.WriteLine();
            Console.WriteLine("December 5th Part2: ");

            Part2(Path.Combine(path, "dec05test.txt"), 10, Path.Combine(path, "dec05part2testfinalstate.txt"));

            Console.WriteLine("normal");
            Part2(Path.Combine(path, "dec05.txt"), 29227751);

            Console.WriteLine("unsafe");
            Part2Unsafe(Path.Combine(path, "dec05.txt"), 29227751);
        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions incrementing the instructions as each is run
        /// </summary>
        public static void Part1(string filename, int? expected = null, string expectedFinalState = null)
        {
            var instructions = Utilities.LoadIntColumn(filename);
            int instrucitonCount = instructions.Count;

            int index = 0;
            int totalsteps = 0;

            while (true)
            {
                totalsteps++;
                index = index + instructions[index]++;

                if (index < 0 || index >= instrucitonCount)
                    break;
            }


            Utilities.WriteInputFile(filename);

            //Check final state
            if (expectedFinalState != null)
            {
                var finalState = Utilities.LoadIntColumn(expectedFinalState);
                bool OK = true;
                for(int i = 0; i < instrucitonCount; i++)
                {
                    if (instructions[i] != finalState[i])
                        OK = false;
                }

                Console.Write("final state: ");
                if (OK)
                    Utilities.WriteColourfultext("OK", ConsoleColor.Green);
                else
                    Utilities.WriteColourfultext("incorrect", ConsoleColor.Red);

                Console.WriteLine();
            }

            Utilities.WriteOutput(totalsteps, expected);
        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions increment and decrementing the instructions as each is run. 
        /// </summary>
        public static void Part2(string filename, int? expected = null, string expectedFinalState = null)
        {
            var instructions = Utilities.LoadIntColumn(filename);
            int instrucitonCount = instructions.Count;

            int index = 0;
            int totalsteps = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            while (true)
            {
                totalsteps++;
                int instruction = instructions[index];
                int newindex = index + instruction;

                if (instruction >= 3)
                    instructions[index]--;
                else
                    instructions[index]++;

                index = newindex;

                if (index < 0 || index >= instrucitonCount)
                    break;
            }

            sw.Stop();
            Utilities.WriteInputFile(filename);

            //Check final state
            if (expectedFinalState != null)
            {
                var finalState = Utilities.LoadIntColumn(expectedFinalState);
                bool OK = true;
                for (int i = 0; i < instrucitonCount; i++)
                {
                    if (instructions[i] != finalState[i])
                        OK = false;
                }

                Console.Write("final state: ");
                if (OK)
                    Utilities.WriteColourfultext("OK", ConsoleColor.Green);
                else
                    Utilities.WriteColourfultext("incorrect", ConsoleColor.Red);

                Console.WriteLine();
            }

            Utilities.WriteOutput(totalsteps, expected);

            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions increment and decrementing the instructions as each is run. 
        ///  while loop bounds checking removed for unsafe version. 
        /// </summary>
        public static unsafe void Part2Unsafe(string filename, int? expected = null, string expectedFinalState = null)
        {
            var instructions = Utilities.LoadIntColumn(filename).ToArray();
            int instrucitonCount = (short)instructions.Length;

            int totalsteps = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            //skip bounds checking for unsafe version
            fixed (int* p1 = instructions)
            {
                int* p2 = p1;
                int* low = p1;
                int* high = p1 + instrucitonCount;

                while (true)
                {
                    totalsteps++;
                    int instruction = *p2;

                    if (instruction >= 3)
                        *p2 = instruction - 1;
                    else
                        *p2 = instruction + 1;

                    p2 += instruction;

                    if (p2 < low || p2 >= high)
                        break;
                }
            }

            sw.Stop();

            Utilities.WriteInputFile(filename);
            
            

            //Check final state
            if (expectedFinalState != null)
            {
                var finalState = Utilities.LoadIntColumn(expectedFinalState);
                bool OK = true;
                for (int i = 0; i < instrucitonCount; i++)
                {
                    if (instructions[i] != finalState[i])
                        OK = false;
                }

                Console.Write("final state: ");
                if (OK)
                    Utilities.WriteColourfultext("OK", ConsoleColor.Green);
                else
                    Utilities.WriteColourfultext("incorrect", ConsoleColor.Red);

                Console.WriteLine();
            }

            Utilities.WriteOutput(totalsteps, expected);

            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }

    }
}
