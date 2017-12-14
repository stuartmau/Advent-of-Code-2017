using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace AdventOfCode2017
{
    public static class Dec10
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 10th - Knot Hash -");
            Console.WriteLine("Part1");

            Part1("3,4,1,5", 5, 12);
            string input = "106,16,254,226,55,2,1,166,177,247,93,0,255,228,60,36";

            Part1(input, 256, 11413);

            Console.WriteLine();
            Console.WriteLine("Part2");

            Part2("", 256, "a2582a3a0e66e6e86e3812dcb672a272");
            Part2("AoC 2017", 256, "33efeb34ea91902bb2f59c9920caa6cd");
            Part2("1,2,3", 256, "3efbe78a8d82f29979031a4aa0b16a9d");
            Part2("1,2,4", 256, "63960835bcdc130f0b66d7ff4f6a5a8e");

            Part2(input, 256, "7adfd64c2a03a4968cf708d1b7fd418d");

        }

        /// <summary>
        /// Product of first two elements in a list after swaping order of subelements.
        /// </summary>
        public static void Part1(string input, int ropelength, int? expected = null)
        {
            List<int> rope = new List<int>();
            for(int i =0; i < ropelength; i++)
                rope.Add(i);


            List<int> lengths = new List<int>();
            var split = input.Split(',');

            foreach (var length in split)
                lengths.Add(int.Parse(length));

            int current = 0;
            int skipsize = 0;

            foreach (var length in lengths)
            {
                //reverse sublist of length
                int lengthcount = length / 2;
                for(int i = 0; i < lengthcount; i++)
                {
                    int index1 = (i + current) % ropelength;
                    int index2 = (current + length - 1 - i) % ropelength;
                    int temp = rope[index2];
                    rope[index2] = rope[index1];
                    rope[index1] = temp;
                }

                //increment current index and skip size
                current = (current + length + skipsize) % ropelength;
                skipsize++;
            }

            int elementSum = rope[0] * rope[1];

            Utilities.WriteOutput(elementSum, expected);
        }


        /// <summary>
        /// Product of first two elements in a list after swaping order of subelements.
        /// </summary>
        public static void Part2(string input, int ropelength, string expected = null)
        {
            List<int> rope = new List<int>();
            for (int i = 0; i < ropelength; i++)
                rope.Add(i);

            List<char> lengths = new List<char>();

            lengths.AddRange(input.ToCharArray());

            //some extra inputs
            char[] extra = { (char)17, (char)31, (char)73, (char)47, (char)23 };
            lengths.AddRange(extra);

            int current = 0;
            int skipsize = 0;


            //compute sparse hash
            for (int roundcount = 0; roundcount< 64; roundcount++)
            {
                //don't use the extra lengths? 
                //for (int j = 0; j < lengths.Count - extra.Length; j++)
                //{
                //    var length = lengths[j];
                foreach (var length in lengths)
                {
                    //reverse sublist of length
                    int lengthcount = length / 2;
                    for (int i = 0; i < lengthcount; i++)
                    {
                        int index1 = (i + current) % ropelength;
                        int index2 = (current + length - 1 - i) % ropelength;
                        int temp = rope[index2];
                        rope[index2] = rope[index1];
                        rope[index1] = temp;
                    }

                    //increment current index and skip size
                    current = (current + length + skipsize) % ropelength;
                    skipsize++;
                }
            }

            //compute dense hash and convert to hex
            List<int> dense = new List<int>();
            string output = "";

            for(int i = 0; i < 16; i++)
            {
                int index = 16 * i;
                // XOR first and second elements of the block
                int result = rope[index] ^ rope[index + 1];

                // XOR the rest of the block
                for (int j = 2; j < 16; j++)
                {
                    result = result ^ rope[index + j];
                }

                dense.Add(result);
                string resultstr = result.ToString("X").ToLower().PadLeft(2, '0');
                output += resultstr;
            }

            //convert dense hash to hex. 
            Utilities.WriteOutput(output, expected);
        }
    }
}
