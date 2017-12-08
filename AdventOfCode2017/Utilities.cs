using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2017
{
    public static class Utilities
    {
        public static string LineSeparator = System.Environment.NewLine + "--------------------------------" + System.Environment.NewLine;

        /// <summary>
        /// Writes a colourful ascii christmas tree to the console
        /// </summary>
        /// <example>
        ///               .
        ///             _.|,_
        ///              '|`
        ///              / \
        ///             /`,o\
        ///            /_* ~_\
        ///            / o .'\
        ///           /_,~' *_\
        ///           /`. *  *\
        ///          /   `~. o \
        ///         /_ *    `~,_\
        ///         /   o  *  ~'\
        ///        / *    .~~'  o\
        ///       /_,.~~'`    *  _\
        ///       /`~..  o        \
        ///      / *   `'~..   *   \
        ///     /_     o    ``~~.,,_\
        ///     /  *      *     ..~'\
        ///    /*    o   _..~~`'*   o\
        ///    `-.__.~'`'   *   ___.-'
        ///          ":-------:"
        ///        hjw \_____/ sm
        /// </example>
        /// <remarks>
        /// hjw : Flump, Hayley Jane Wakenshaw - hayley@kersbergen.com
        /// </remarks>
        public static void WriteConsoleChirstmasTree()
        {
            var curColour = Console.ForegroundColor;
            Random rnd = new Random();

            List<ConsoleColor> colours = new List<ConsoleColor>();
            colours.Add(ConsoleColor.Yellow);
            colours.Add(ConsoleColor.Red);
            colours.Add(ConsoleColor.Cyan);
            colours.Add(ConsoleColor.Green);
            colours.Add(ConsoleColor.Magenta);
            colours.Add(ConsoleColor.Blue);
            colours.Add(ConsoleColor.White);

            ConsoleColor y = ConsoleColor.Yellow;
            ConsoleColor g = ConsoleColor.DarkGreen;
            ConsoleColor col = colours[rnd.Next(colours.Count)];

            Console.ForegroundColor = ConsoleColor.Yellow;
            WLC(@"               .", y);
            WLC(@"             _.|,_", y);
            WLC(@"              '|`", y);

            WLC(@"              / \", g);

            WC(@"             /", g);
            WC(@"`,o", col, colours, rnd);
            WLC(@"\", g);


            WC(@"            /_", g);
            WC(@"* ~_", col, colours, rnd);
            WLC(@"\", g);


            WC(@"            /", g);
            WC(@" o .'", col, colours, rnd);
            WLC(@"\", g);


            WC(@"           /", g);
            WC(@"_,~' *", col, colours, rnd);
            WLC(@"_\", g);

            WC(@"           /", g);
            WC(@"`. *  *", col, colours, rnd);
            WLC(@"\", g);


            WC(@"          /", g);
            WC(@"   `~. o ", col, colours, rnd);
            WLC(@"\", g);


            WC(@"         /_", g);
            WC(@" *    `~,_", col, colours, rnd);
            WLC(@"\", g);

            WC(@"         /", g);
            WC(@"   o  *  ~'", col, colours, rnd);
            WLC(@"\", g);


            WC(@"        /", g);
            WC(@" *    .~~'  o", col, colours, rnd);
            WLC(@"\", g);


            WC(@"       /", g);
            WC(@"_,.~~'`    *  ", col, colours, rnd);
            WLC(@"_\", g);



            WC(@"       /", g);
            WC(@"`~..  o        ", col, colours, rnd);
            WLC(@"\", g);


            WC(@"      /", g);
            WC(@" *   `'~..   *   ", col, colours, rnd);
            WLC(@"\", g);


            WC(@"     /_", g);
            WC(@"     o    ``~~.,,_", col, colours, rnd);
            WLC(@"\", g);

            WC(@"     /", g);
            WC(@"  *      *     ..~'", col, colours, rnd);
            WLC(@"\", g);

            WC(@"    /", g);
            WC(@"*    o   _..~~`'*   o", col, colours, rnd);
            WLC(@"\", g);

            WC(@"    `-.__", g);
            WC(@".~'`'   *", col, colours, rnd);
            WLC(@"   ___.-'", g);

            WLC(@"          "":-------:""", g);
            WLC(@"        hjw \_____/ sm", ConsoleColor.DarkYellow);

            Console.ForegroundColor = curColour;
        }

        private static void WC(string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
        }

        private static void WC(string str, ConsoleColor defaultcolour, List<ConsoleColor> colours, Random rnd)
        {
            foreach(var letter in str)
            {
                if (letter == '*')
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if(letter == 'o')
                    Console.ForegroundColor = colours[rnd.Next(colours.Count)];
                else
                    Console.ForegroundColor = defaultcolour;

                Console.Write(letter);
            }
            
        }

        private static void WLC(string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }

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

        public static void WriteOutput(int? sum, int? expected)
        {
            bool found = sum == expected;
            
            if(sum == null)
            {
                Utilities.WriteColourfultext("Output not found", ConsoleColor.Red);
            }


            if (expected != null)
            {
                Console.Write("output: " + sum.ToString() + " expected: ");
                Utilities.WriteColoufulBool(found);
                Console.WriteLine();
            }
            else
                Console.WriteLine("output: " + sum.ToString());
        }

        public static void WriteOutput(string resut, string expected)
        {
            bool found = resut.Equals(expected);

            if (expected != null)
            {
                Console.Write("output: " + resut + " expected: ");
                Utilities.WriteColoufulBool(found);
                Console.WriteLine();
            }
            else
                Console.WriteLine("output: " + resut);
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
