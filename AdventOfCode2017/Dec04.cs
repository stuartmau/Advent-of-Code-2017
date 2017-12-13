using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2017
{
    public static class Dec04
    {
        public static void Run(string path)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 4th - High-Entropy Passphrases -");
            Console.WriteLine("Part1");

            Part1(Path.Combine(path, "dec04.txt"), 455);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec04part2test.txt"),2);
            Part2(Path.Combine(path, "dec04.txt"), 186);
        }

        /// <summary>
        /// Valid lines do not have duplicate words.
        /// </summary>
        public static void Part1(string filename, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            int sum = 0;

            foreach (var password in strings)
            {
                List<string> words = new List<string>(password.Split(' '));
                words.Sort();

                //check duplicates
                bool valid = true;
                for(int i=1; i < words.Count; i++)
                {
                    if (words[i] == words[i - 1])
                        valid = false;
                }

                if (valid)
                    sum++;
            }

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(sum, expected);
        }


        /// <summary>
        /// Valid lines do not have duplicate words or annagrams. 
        /// </summary>
        public static void Part2(string filename, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            int sum = 0;

            foreach (var password in strings)
            {
                List<string> words = new List<string>(password.Split(' '));

                //sort letters for each word
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < words.Count; i++)
                {
                    var chararr = new List<char>(words[i].ToCharArray());
                    chararr.Sort();

                    sb.Clear();
                    foreach (var character in chararr)
                        sb.Append(character);

                    words[i] = sb.ToString();
                }

                words.Sort();

                //check duplicates
                bool valid = true;
                for (int i = 1; i < words.Count; i++)
                {
                    if (words[i] == words[i - 1])
                        valid = false;
                }

                if (valid)
                    sum++;
            }

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(sum, expected);
        }
    }
}
