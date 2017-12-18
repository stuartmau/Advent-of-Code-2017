using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec16
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 16th - Permutation Promenad -");
            Console.WriteLine("Part1");
            var result = Part1Evaluate(5, "s1,x3/4,pe/b");
            Utilities.WriteOutput(result, "baedc");

            result = Part1(Path.Combine(path, "dec16.txt"));
            Utilities.WriteOutput(result, "cknmidebghlajpfo");


            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec16.txt"), 1, "cknmidebghlajpfo");

            Part2(Path.Combine(path, "dec16.txt"), 1_000_000_000, "cbolhmkgfpenidaj");
            
        }

        /// <summary>
        /// Run the dance once and check the final pattern. 
        /// </summary>
        public static string Part1(string filename, string expected = null)
        {
            var input = Utilities.LoadStrings(filename);
            return Part1Evaluate(16, input[0], expected);
        }


        /// <summary>
        /// Run the dance once and check the final pattern. 
        /// </summary>
        public static string Part1Evaluate(int dancerCount, string input, string expected = null)
        {
            //s1        - spin     - move the last x dancers to the front. 
            //x3 / 4    - exchange - swap two dancers positions by index
            //pe / b    - partner  - swap two dancers positions by name

            List<int> dancers = new List<int>();
            for (int i = 0; i < dancerCount; i++)
                dancers.Add(i);

            var dancemoves = input.Split(',');

            RunDance(dancerCount, dancers, dancemoves);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dancers.Count; i++)
            {
                sb.Append((char)(dancers[i] + (int)'a'));
            }
            string result = sb.ToString();

            return sb.ToString();
        }


        /// <summary>
        /// Run the dance up to a billion times checking when the pattern repeats and return the appropriate billionth pattern. 
        /// </summary>
        public static void Part2(string filename, int maxcount, string expected = null)
        {
            int dancerCount = 16;
            List<int> dancers = new List<int>();

            for (int i = 0; i < dancerCount; i++)
                dancers.Add(i);

            var input = Utilities.LoadStrings(filename);
            var dancemoves = input[0].Split(',');

            int counter = 0;
            int patternlength = 0;
            Dictionary<System.Numerics.BigInteger, int> hashes = new Dictionary<System.Numerics.BigInteger, int>();
            byte[] bytearr = new byte[sizeof(int) * dancerCount];

            while (counter++ < maxcount)
            {
                RunDance(dancerCount, dancers, dancemoves);

                Buffer.BlockCopy(dancers.ToArray(), 0, bytearr, 0, sizeof(int) * dancerCount);
                System.Numerics.BigInteger bi = new System.Numerics.BigInteger(bytearr);

                if (!hashes.TryAdd(bi, counter))
                {
                    patternlength = counter;
                    break;
                }
            }

            //If the pattern repeats, find the appropriate pattern for the billionth itteration. 
            if (patternlength != 0)
            {
                maxcount = maxcount % (patternlength -1);

                //reset
                for (int i = 0; i < dancerCount; i++)
                    dancers[i] = i;

                //run the number of patterns
                for (int i = 0; i < maxcount; i++)
                    RunDance(dancerCount, dancers, dancemoves);
            }


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dancerCount; i++)
                sb.Append((char)(dancers[i] + (int)'a'));

            string result = sb.ToString();

            Utilities.WriteOutput(result, expected);
        }

        private static void RunDance(int dancerCount, List<int> dancers, string[] dancemoves)
        {
            foreach (var move in dancemoves)
            {
                if (move[0] == 's')
                {
                    // spin dancers
                    int index = int.Parse(move.Substring(1));
                    List<int> start = dancers.GetRange(0, dancerCount - index);
                    List<int> end = dancers.GetRange(dancerCount - index, index);
                    dancers.Clear();
                    dancers.AddRange(end);
                    dancers.AddRange(start);
                }
                else if (move[0] == 'x')
                {
                    //swap dancers by index
                    var splitmove = move.Substring(1).Split('/');
                    int indexA = int.Parse(splitmove[0]);
                    int indexB = int.Parse(splitmove[1]);

                    SwapDancerIndex(dancers, indexA, indexB);
                }
                else if (move[0] == 'p')
                {
                    //swap dancers by name
                    var splitmove = move.Substring(1).Split('/');
                    int dancerA = (int)splitmove[0].ToCharArray()[0] - (int)'a';
                    int dancerB = (int)splitmove[1].ToCharArray()[0] - (int)'a';

                    int dancerAIndex = -1;
                    int dancerBIndex = -1;

                    for (int i = 0; i < dancers.Count; i++)
                    {
                        if (dancers[i] == dancerA)
                            dancerAIndex = i;

                        if (dancers[i] == dancerB)
                            dancerBIndex = i;

                        if (dancerAIndex >= 0 && dancerBIndex >= 0)
                            break;
                    }

                    SwapDancerIndex(dancers, dancerAIndex, dancerBIndex);
                }
                else
                    throw new Exception("Unrecognised dance move: " + move);
            }
        }


        private static void SwapDancerIndex(List<int> dancers, int indexA, int indexB)
        {
            int temp = dancers[indexA];
            dancers[indexA] = dancers[indexB];
            dancers[indexB] = temp;
        }



    }
}
