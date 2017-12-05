using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2017
{
    public static class Utilities
    {
        public static string LineSeparator = System.Environment.NewLine + "--------------" + System.Environment.NewLine;


        public static void WriteInputFile(string filename)
        {
            string path = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar;
            string file = Path.GetFileName(filename);

            var curColour = Console.ForegroundColor;
            Console.Write("Input: ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(path);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(file);


            Console.ForegroundColor = curColour;
        }

        public static void WriteOutput(int sum, int? expected)
        {
            bool found = sum == expected;

            if (expected != null)
            {
                Console.Write("output: " + sum.ToString() + " expected: ");
                Utilities.WriteColoufulBool(found);
                Console.WriteLine();
            }
            else
                Console.WriteLine("output: " + sum.ToString());
        }

        /// <summary>
        /// write start and end with normal, and error text with red. 
        /// </summary>
        public static void WriteColourfultext(string colourtext, ConsoleColor colour)
        {
            var curColour = Console.ForegroundColor;

            Console.ForegroundColor = colour;
            Console.Write(colourtext);

            Console.ForegroundColor = curColour;
        }

        public static void WriteColoufulBool(bool value)
        {
            WriteColoufulBool(value, ConsoleColor.Green, ConsoleColor.Red);
        }

        public static void WriteColoufulBool(bool value, ConsoleColor truecolour, ConsoleColor falsecolour)
        {
            var curColour = Console.ForegroundColor;
            Console.ForegroundColor = truecolour;

            if (!value)
                Console.ForegroundColor = falsecolour;

            Console.Write(value);

            Console.ForegroundColor = curColour;
        }

        /// <summary>
        /// array of array of integers
        /// </summary>
        public static List<List<int>> LoadIntArrays(string filename)
        {
            List<List<int>> spreadsheet = new List<List<int>>();

            using (var sr = File.OpenText(filename))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var arr = line.Split('\t', ' ', ',');

                    List<int> row = new List<int>();

                    foreach (var value in arr)
                        row.Add(int.Parse(value));

                    if (arr.Length > 0)
                        spreadsheet.Add(new List<int>(row));
                }
            }

            return spreadsheet;
        }

        /// <summary>
        /// one column of integers
        /// </summary>
        public static List<int> LoadIntColumn(string filename)
        {
            List<int> column = new List<int>();

            using (var sr = File.OpenText(filename))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    column.Add(int.Parse(line.Trim()));
                }
            }

            return column;
        }

        /// <summary>
        /// array of line strings
        /// </summary>
        public static List<string> LoadStrings(string filename)
        {
            List<string> lines = new List<string>();

            using (var sr = File.OpenText(filename))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}
