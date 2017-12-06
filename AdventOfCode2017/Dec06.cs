using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Numerics;

namespace AdventOfCode2017
{

    public static class Dec06
    {
        public static void Run(string path)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 6th - Memory Reallocation -");
            Console.WriteLine("Part1");

            Part1(Path.Combine(path, "dec06test.txt"), 5);
            //Part1(Path.Combine(path, "dec06.txt"), 14029); //slow

            Console.WriteLine("hash version");
            Part1Hash(Path.Combine(path, "dec06.txt"), 14029);

            Console.WriteLine();
            Console.WriteLine("Part2");
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

            var bank = input[0].ToArray();
            int bankcount = bank.Length;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            while (!found)
            {
                int[] currentCopy = new int[bankcount];
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                for (int i = 0; i < bankcount; i++)
                {
                    int bankval = bank[i];

                    currentCopy[i] = bankval;

                    if (bankval > max)
                    {
                        max = bankval;
                        maxindex = i;
                    }
                }

                //Check if this state has been seen before
                for (int i = 0; i < states.Count; i++)
                {
                    var prevstate = states[i];
                    int foundcount = 0;

                    for (int j = 0; j < bankcount; j++)
                    {
                        if (currentCopy[j] == prevstate[j])
                            foundcount++;
                    }

                    if (foundcount == bankcount)
                    {
                        found = true;
                        break;
                    }
                }

                states.Add(currentCopy);

                //redistribute the max block to each bank. 
                if (!found)
                {
                    //remove from max bank
                    bank[maxindex] -= max;

                    //redistribute to other banks
                    int everybankgets = max / bankcount;

                    for (int i = 0; i < bankcount; i++)
                        bank[i] += everybankgets;

                    //remove remaining 
                    int extra = max - (everybankgets * bankcount);
                    int extraindex = (maxindex + 1) % bankcount;
                    while (extra > 0)
                    {
                        bank[extraindex]++;
                        extra--;
                        extraindex = (extraindex + 1) % bankcount;
                    }

                    cyclecount++;
                }

                
            }

            sw.Stop();


            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cyclecount, expected);
            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }


        /// <summary>
        /// redistribute memory and find when state cycles
        /// </summary>
        public static void Part1Hash(string filename, int? expected = null)
        {
            var input = Utilities.LoadIntArrays(filename);
            Dictionary<BigInteger, int> states = new Dictionary<BigInteger, int>();

            bool found = false;
            int cyclecount = 0;

            var bank = input[0].ToArray();
            int bankcount = bank.Length;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            byte[] bytearr = new byte[sizeof(int)* bankcount];

            while (!found)
            {
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                ulong hash = 0;

                
                for (int i = 0; i < bankcount; i++)
                {
                    int bankval = bank[i];
                    //incramental.AppendData(bankval)
                    if (bankval > max)
                    {
                        max = bankval;
                        maxindex = i;
                    }
                }


                Buffer.BlockCopy(bank, 0, bytearr, 0, sizeof(int) * bankcount);
                BigInteger bi = new BigInteger(bytearr);


                //add the current state to the list of states. signal done when the key already exists. 
                if (!states.TryAdd(bi, cyclecount))
                    found = true;

                //redistribute the max block to each bank. 
                if (!found)
                {
                    //remove from max bank
                    bank[maxindex] -= max;

                    //redistribute to other banks
                    int everybankgets = max / bankcount;

                    for (int i = 0; i < bankcount; i++)
                        bank[i] += everybankgets;

                    //remove remaining 
                    int extra = max - (everybankgets * bankcount);
                    int extraindex = (maxindex + 1) % bankcount;
                    while (extra > 0)
                    {
                        bank[extraindex]++;
                        extra--;
                        extraindex = (extraindex + 1) % bankcount;
                    }

                    cyclecount++;
                }


            }

            sw.Stop();


            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cyclecount, expected);
            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// cound the lenght of the state cycle
        /// </summary>
        public static void Part2(string filename, int? expected = null)
        {
            var input = Utilities.LoadIntArrays(filename);
            Dictionary<BigInteger, int> states = new Dictionary<BigInteger, int>();

            bool found = false;
            int cyclecount = 0;

            var bank = input[0].ToArray();
            int bankcount = bank.Length;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            byte[] bytearr = new byte[sizeof(int) * bankcount];
            BigInteger bi = new BigInteger();

            while (!found)
            {

                int[] bankstate = new int[bankcount];
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                for (int i = 0; i < bankcount; i++)
                {
                    int bankval = bank[i];

                    bankstate[i] = bankval;

                    if (bankval > max)
                    {
                        max = bankval;
                        maxindex = i;
                    }
                }

                Buffer.BlockCopy(bank, 0, bytearr, 0, sizeof(int) * bankcount);
                bi = new BigInteger(bytearr);

                //add the current state to the list of states. signal done when the key already exists. 
                if (!states.TryAdd(bi, cyclecount))
                    found = true;


                if (!found)
                {
                    //remove from max bank
                    bank[maxindex] -= max;

                    //redistribute to other banks
                    int everybankgets = max / bankcount;

                    for (int i = 0; i < bankcount; i++)
                        bank[i] += everybankgets;

                    int extra = max - (everybankgets * bankcount);
                    int extraindex = (maxindex + 1) % bankcount;
                    while (extra > 0)
                    {
                        bank[extraindex]++;
                        extra--;
                        extraindex = (extraindex + 1) % bankcount;
                    }

                    cyclecount++;
                }


            }

            int cycleLenght = cyclecount - states[bi];
            sw.Stop();

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cycleLenght, expected);
            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }

    }
}
