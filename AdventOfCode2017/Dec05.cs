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
            Console.WriteLine("December 5th - A Maze of Twisty Trampolines, All Alike -");
            Console.WriteLine("Part1");

            Part1(Path.Combine(path, "dec05test.txt"), 5, Path.Combine(path, "dec05part1testfinalstate.txt"));
            Part1(Path.Combine(path, "dec05.txt"), 376976);


            Console.WriteLine();
            Console.WriteLine("Part2");

            Part2(Path.Combine(path, "dec05test.txt"), 10, Path.Combine(path, "dec05part2testfinalstate.txt"));

            Console.WriteLine("normal");
            Part2(Path.Combine(path, "dec05.txt"), 29227751);

            Console.WriteLine("unsafe");
            Part2Unsafe(Path.Combine(path, "dec05.txt"), 29227751);

            //Console.WriteLine("parallel");
            //Part2parallel(Path.Combine(path, "dec05.txt"), 29227751);
        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions incrementing the instructions as each is run.
        /// </summary>
        public static Result Part1(string filename, int? expected = null, string expectedFinalState = null)
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

            return Utilities.WriteOutput(totalsteps, expected);
        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions increment and decrementing the instructions as each is run. 
        /// </summary>
        public static Result Part2(string filename, int? expected = null, string expectedFinalState = null)
        {
            var instructions = Utilities.LoadIntColumn(filename).ToArray();
            int instrucitonCount = instructions.Length;

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

            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
            return Utilities.WriteOutput(totalsteps, expected);
        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions increment and decrementing the instructions as each is run. 
        ///  while loop bounds checking removed for unsafe version. 
        /// </summary>
        public static unsafe Result Part2Unsafe(string filename, int? expected = null, string expectedFinalState = null)
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

            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
            return Utilities.WriteOutput(totalsteps, expected);
        }


        static int RunInstructions2(int[] instructions, int instrucitonCount, ref int index, ref bool[] lockarray, ref bool[] updating, ref bool done)
        {
            //increment index
            //modify current. 

            int stepsCounted = 0;

            while (true)
            {
                if (done)
                    return stepsCounted;

                int currentIndex = 0;
                int instruction = 0;
                bool UpdateInstruction = false;

                //increment index to next instruction and lock current instruction
                lock (instructions)
                {
                    currentIndex = index;

                    if (currentIndex < 0 || currentIndex >= instrucitonCount)
                        return stepsCounted;

                    if (!updating[currentIndex] && !lockarray[currentIndex])
                    {
                        lockarray[currentIndex] = true;
                        updating[currentIndex] = true;
                        instruction = instructions[currentIndex];
                        index = index + instruction;
                        UpdateInstruction = true;
                    }
                    else
                    {
                        //1 look ahead
                        currentIndex = currentIndex + instructions[currentIndex];

                        if (currentIndex < 0 || currentIndex >= instrucitonCount)
                        {
                            done = true;
                            return stepsCounted;
                        }

                        if (!updating[currentIndex] && !lockarray[currentIndex])
                        {
                            lockarray[currentIndex] = true;
                            updating[currentIndex] = true;
                            instruction = instructions[currentIndex];
                            index = currentIndex + instruction;
                            UpdateInstruction = true;
                        }

                    }
                }

                // modify current instruction
                if (UpdateInstruction)
                {

                    if (instruction >= 3)
                        instructions[currentIndex]--;
                    else
                        instructions[currentIndex]++;

                    lockarray[currentIndex] = false;
                    updating[currentIndex] = false;

                    if (currentIndex < 0 || currentIndex >= instrucitonCount)
                    {
                        done = true;
                        return stepsCounted;
                    }
                    else
                    {
                        stepsCounted++;
                    }
                }

            }

        }

        static int RunInstructions3(int[] instructions, int instrucitonCount,
                                    ref int index,
                                    ref bool done,
                                    int threadindex, int threadcount,
                                    ref int?[] currentInstructions)
        {

            //increment index
            //modify current. 

            int stepsCounted = 0;
            //int lookaheadmax = 1;


            while (true)
            {
                //another thread found the offending instruction. return found steps.
                if (done)
                    return stepsCounted;

                int currentindex = index;
                int instruction = 0;
                bool found = false;

                //Get next instruction
                lock (instructions)
                {
                    ////find the next instruction that isn't being worked on and doesn't point to an instruction that is being worked on. 
                    //for(int lookaheadcounter = 0; lookaheadcounter< lookaheadmax; lookaheadcounter++)
                    //{
                    //Check if it is currently being worked on, or is pointed to by an instruction that is being worked on. 
                    bool available = true;
                    for (int i = 0; i < threadcount; i++)
                    {
                        if (currentInstructions[i] != null && currentindex == currentInstructions[i])
                        {
                            available = false;
                            break;
                        }
                    }

                    if (available)
                    {
                        found = true;
                        //break;
                    }

                    //}

                    if (found)
                    {
                        //lock chosen instruction by adding it to the current instructions list. 
                        currentInstructions[threadindex] = currentindex;
                        //get instruction
                        instruction = instructions[currentindex];
                        index = currentindex + instruction;

                        // this instruction went beyond the array bounds. thefore signal done and return steps
                        if (!done && index + instruction < 0 || index + instruction >= instrucitonCount)
                        {
                            done = true;
                            return stepsCounted;
                        }
                    }
                }

                //modify chosen instruction and release lock?
                if (found)
                {
                    if (instruction >= 3)
                        instruction--;
                    else
                        instruction++;

                    stepsCounted++;

                    lock (instructions)
                    {
                        instructions[currentindex] = instruction;
                        currentInstructions[threadindex] = null;
                    }
                }
            }

        }

        /// <summary>
        ///  Find the number of steps to jump through the instructions increment and decrementing the instructions as each is run. 
        /// </summary>
        public static Result Part2parallel(string filename, int? expected = null, string expectedFinalState = null)
        {
            var instructions = Utilities.LoadIntColumn(filename).ToArray();
            int instrucitonCount = instructions.Length;

            bool[] lockarray = new bool[instrucitonCount];
            bool[] completearray = new bool[instrucitonCount];

            bool done = false;
            int index = 0;
            int totalsteps = 0;

            int threadcount = 1;
            int?[] currentinstructions = new int?[threadcount];

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            List<Task<int>> tasks = new List<Task<int>>();

            for (int i = 0; i < threadcount; i++)
            {
                int threadindex = i; // when i is used as the paramater, it is being modifed somehow :( 
                                     // either I have the wrong expectations/understanding :( or there is a bug. for now, use a local variable to solve the problem. 
                tasks.Add(Task<int>.Factory.StartNew(() => RunInstructions3(instructions, instrucitonCount, ref index, ref done, threadindex, threadcount, ref currentinstructions), TaskCreationOptions.LongRunning));
            }

            foreach (var task in tasks)
                totalsteps += task.Result;

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

            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
            return Utilities.WriteOutput(totalsteps, expected);
        }

    }
}
