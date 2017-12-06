using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2017
{

    public static class Dec06
    {
        public static void Run(string path)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 6th - Memory Reallocation - Part1");
            Part1(Path.Combine(path, "dec06test.txt"), 5);
            Part1(Path.Combine(path, "dec06.txt"), 14029);

            Console.WriteLine();
            Console.WriteLine("December 6th Part2: ");
            Part2(Path.Combine(path, "dec06test.txt"), 4);
            Part2(Path.Combine(path, "dec06.txt"), 2765);
        }

        /// <summary>
        /// redistribute memory and find when state cycles
        /// </summary>
        public static void Part1(string filename, int? expected = null)
        {
            var input = Utilities.LoadIntArrays(filename);
            List<int[]> states = new List<int[]>();

            bool found = false;
            int cyclecount = 0;

            var banks = input[0].ToArray();
            int bankcount = banks.Length;

            while (!found)
            {
                int[] bankstate = new int[bankcount];
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                for (int i = 0; i < bankcount; i++)
                {
                    int bankval = banks[i];

                    bankstate[i] = bankval;

                    if (bankval > max)
                    {
                        max = bankval;
                        maxindex = i;
                    }
                }

                //Ceck if this state has been seen before
                foreach (var prevstate in states)
                {
                    int foundcount = 0;
                    for (int i = 0; i < bankcount; i++)
                    {
                        if (bankstate[i] == prevstate[i])
                            foundcount++;
                    }
                    if (foundcount == bankcount)
                    {
                        found = true;
                        break;
                    }
                }

                //add the current state to the list of states
                states.Add(bankstate);

                //redistribute the max block to each bank. 
                if (!found)
                {
                    //remove from max bank
                    banks[maxindex] -= max;

                    //redistribute to other banks
                    int everybankgets = max / bankcount;

                    for (int i = 0; i < bankcount; i++)
                        banks[i] += everybankgets;

                    //remove remaining 
                    int extra = max - (everybankgets * bankcount);
                    int extraindex = (maxindex + 1) % bankcount;
                    while (extra > 0)
                    {
                        banks[extraindex]++;
                        extra--;
                        extraindex = (extraindex + 1) % bankcount;
                    }

                    cyclecount++;
                }

                
            }

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cyclecount, expected);
        }


        /// <summary>
        /// cound the lenght of the state cycle
        /// </summary>
        public static void Part2(string filename, int? expected = null)
        {
            var input = Utilities.LoadIntArrays(filename);
            List<int[]> states = new List<int[]>();

            bool found = false;
            int cyclecount = 0;
            int prevFoundIndex = 0;

            var banks = input[0].ToArray();
            int bankcount = banks.Length;

            while (!found)
            {

                int[] bankstate = new int[bankcount];
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                for (int i = 0; i < bankcount; i++)
                {
                    int bankval = banks[i];

                    bankstate[i] = bankval;

                    if (bankval > max)
                    {
                        max = bankval;
                        maxindex = i;
                    }
                }

                //Ceck if this state has been seen before
                for(int j =0; j < states.Count; j++)
                {
                    var prevstate = states[j];
                    int foundcount = 0;
                    for (int i = 0; i < bankcount; i++)
                    {
                        if (bankstate[i] == prevstate[i])
                            foundcount++;
                    }
                    if (foundcount == bankcount)
                    {
                        prevFoundIndex = j;
                        found = true;
                        break;
                    }
                }

                states.Add(bankstate);


                if (!found)
                {
                    //remove from max bank
                    banks[maxindex] -= max;

                    //redistribute to other banks
                    int everybankgets = max / bankcount;

                    for (int i = 0; i < bankcount; i++)
                        banks[i] += everybankgets;

                    int extra = max - (everybankgets * bankcount);
                    int extraindex = (maxindex + 1) % bankcount;
                    while (extra > 0)
                    {
                        banks[extraindex]++;
                        extra--;
                        extraindex = (extraindex + 1) % bankcount;
                    }

                    cyclecount++;
                }


            }

            int cycleLenght = states.Count - prevFoundIndex -1;

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cycleLenght, expected);
        }

    }
}
