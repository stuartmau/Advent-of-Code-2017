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
            Part1(Path.Combine(path, "dec06.txt"), 14029);

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
            Dictionary<BigInteger, int> states = new Dictionary<BigInteger, int>();
            
            int cyclecount = 0;

            var bank = input[0].ToArray();
            int bankcount = bank.Length;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            byte[] bytearr = new byte[sizeof(int)* bankcount];

            while (true)
            {
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                
                for (int i = 0; i < bankcount; i++)
                {
                    if (bank[i] > max)
                    {
                        max = bank[i];
                        maxindex = i;
                    }
                }

                //concatonate bank values as a simple hash. 
                Buffer.BlockCopy(bank, 0, bytearr, 0, sizeof(int) * bankcount);
                BigInteger bi = new BigInteger(bytearr);

                //add the current state to the list of states. Finish when the key already exists. 
                if (!states.TryAdd(bi, cyclecount))
                    break;

                //redistribute to other banks
                int everybankgets = max / bankcount;
                int extra = max - (everybankgets * bankcount);
                //max bank
                bank[maxindex] = everybankgets;
                //the other banks
                int index = (maxindex + 1) % bankcount;
                while (index != maxindex)
                {
                    bank[index] += everybankgets;

                    if (extra > 0)
                    {
                        bank[index]++;
                        extra--;
                    }
                    index = (index + 1) % bankcount;
                }

                cyclecount++;
            }

            sw.Stop();

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cyclecount, expected);
            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// reports the length of the cycle
        /// </summary>
        public static void Part2(string filename, int? expected = null)
        {
            var input = Utilities.LoadIntArrays(filename);
            Dictionary<BigInteger, int> states = new Dictionary<BigInteger, int>();

            int cyclecount = 0;

            var bank = input[0].ToArray();
            int bankcount = bank.Length;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            byte[] bytearr = new byte[sizeof(int) * bankcount];
            BigInteger bi = new BigInteger();

            while (true)
            {
                //find maxbank. lowest bank wins tie.
                int max = -1;
                int maxindex = -1;
                for (int i = 0; i < bankcount; i++)
                {
                    if (bank[i] > max)
                    {
                        max = bank[i];
                        maxindex = i;
                    }
                }

                //generate simple hash
                Buffer.BlockCopy(bank, 0, bytearr, 0, sizeof(int) * bankcount);
                bi = new BigInteger(bytearr);

                //add the current state to the list of states. Finish when the key already exists. 
                if (!states.TryAdd(bi, cyclecount))
                    break;


                //redistribute to other banks
                int everybankgets = max / bankcount;
                int extra = max - (everybankgets * bankcount);
                //max bank
                bank[maxindex] = everybankgets; 
                //the other banks
                int index = (maxindex + 1) % bankcount;
                while(index != maxindex)
                {
                    bank[index] += everybankgets;
                    
                    if(extra>0)
                    {
                        bank[index]++;
                        extra--;
                    }
                    index = (index + 1) % bankcount;
                }

                cyclecount++;

            }

            int cycleLenght = cyclecount - states[bi];
            sw.Stop();

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(cycleLenght, expected);
            Console.WriteLine("milliseconds: " + sw.ElapsedMilliseconds);
        }

    }
}
