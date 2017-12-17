using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec14
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 14th - Disk Defragmentation -");
            Console.WriteLine("Part1");

            Part1("flqrgnkx", 8108);
            Part1("vbqugkhl", 8148);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2("flqrgnkx", 1242);
            Part2("vbqugkhl", 1180);
        }

        /// <summary>
        /// Fill a 128x128 array of bits from day 10's knot hash. Count number of used (on) bits. 
        /// </summary>
        public static void Part1(string input, int? expected = null)
        {
            bool[,] grid = new bool[128,128];

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 128; i++)
            {
                var denseHash = KnotHash(input + "-" + i.ToString());

                for (int j = 0; j < 32; j++)
                {
                    var intvalue = int.Parse(denseHash.Substring(j, 1), System.Globalization.NumberStyles.HexNumber);
                    sb.Append(Convert.ToString(intvalue, 2).PadLeft(4,'0'));
                }
                sb.AppendLine();
            }
            var str = sb.ToString();

            //count number of bits
            var count = str.Count(c => c == '1');

            Utilities.WriteOutput(count, expected);
        }


        /// <summary>
        /// From the filled grid, count number of regions of adjacent bits. not including diagonals. 
        /// each square is in one region. Isolated bits are their own region. 
        /// </summary>
        public static void Part2(string input, int? expected = null)
        {
            int gridsize = 128;
            bool[,] grid = new bool[gridsize, gridsize];


            for (int i = 0; i < gridsize; i++)
            {
                var denseHash = KnotHash(input + "-" + i.ToString());

                for (int j = 0; j < 32; j++)
                {
                    var intvalue = int.Parse(denseHash.Substring(j, 1), System.Globalization.NumberStyles.HexNumber);
                    var bitar = new BitArray(new int[] { intvalue });

                    for (int k = 0; k < 4; k++)
                    {
                        var val = bitar[3 - k];
                        grid[i, j * 4 + k] = val;
                    }
                }
            }

            int groupcount = 0;

            //find all adjecent cells, set them to zeo, and increment groupcount 
            Stack<Tuple<int, int>> adjacent = new Stack<Tuple<int, int>>();
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                    //if the bit is set, it hasn't previously been added to a group.
                    //add it to the stack to be worked upon. 
                    if (grid[i, j])
                    {
                        adjacent.Push(new Tuple<int, int>(i, j));
                        groupcount++;
                    }

                    //clear the current cell and add adjecent neighbours to be worked upon. 
                    while (adjacent.Count > 0)
                    {
                        var cur = adjacent.Pop();
                        int x = cur.Item1;
                        int y = cur.Item2;
                        grid[x, y] = false;

                        if (x > 0 && grid[x - 1, y])
                            adjacent.Push(new Tuple<int, int>(x - 1, y));

                        if (x < gridsize -1 && grid[x + 1, y])
                            adjacent.Push(new Tuple<int, int>(x + 1, y));

                        if (y > 0 && grid[x, y-1])
                            adjacent.Push(new Tuple<int, int>(x , y - 1));

                        if (y < gridsize -1 && grid[x , y+1])
                            adjacent.Push(new Tuple<int, int>(x , y + 1));
                    }

                }
            }

            Utilities.WriteOutput(groupcount, expected);
        }


        /// <summary>
        /// Dense hash following algorithm from day 10 
        /// </summary>
        public static string KnotHash(string input)
        {
            int ropelength = 256;

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
            for (int roundcount = 0; roundcount < 64; roundcount++)
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

            for (int i = 0; i < 16; i++)
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
            return output;
        }
    }
}
