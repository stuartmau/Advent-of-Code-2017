using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2017
{
    public enum Result { Expected, Error };

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
        ///            \_____/ ascii art:hjw
        /// </example>
        /// <remarks>
        /// hjw : Flump, Hayley Jane Wakenshaw - hayley@kersbergen.com
        /// </remarks>
        public static void WriteConsoleChirstmasTree(Random rnd)
        {
            var curColour = Console.ForegroundColor;

            List<ConsoleColor> colours = new List<ConsoleColor>
            {
                ConsoleColor.Yellow,
                ConsoleColor.Red,
                ConsoleColor.Cyan,
                ConsoleColor.Green,
                ConsoleColor.Magenta,
                ConsoleColor.Blue,
                ConsoleColor.White
            };

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
            WC(@".~'`'", col, colours, rnd);
            WLC(@" * art:hjw.-'", g);

            WLC(@"          "":-------:""", g);
            WLC(@"            \_____/", ConsoleColor.DarkYellow);
            Console.ForegroundColor = curColour;
        }

        public class Snow
        {
            public int xprev;
            public int yprev;
            public int x;
            public int y;
            public char flake = '#';

            public Snow(int x, int y)
            {
                this.x = x;
                this.y = y;
            }


            internal void Move(Random rnd)
            {
                xprev = x;
                yprev = y;

                int rndval = rnd.Next(3);

                switch (rndval)
                {
                    case 0:
                        x--;
                        break;
                    case 2:
                        x++;
                        break;
                    default:
                        break;
                }

                y++;
            }

            internal bool ChangedPosition()
            {
                if (xprev != x || yprev != y)
                    return true;
                else
                    return false;
            }
        }


        /// <summary>
        /// Same as writeConsoleChirstmastree but with 'snow' that fills up from the bottom of the screen if left running
        /// </summary>
        public static void WriteAnimatedConsoleChirstmasTree()
        {
            List<Snow> snow = new List<Snow>();
            Random rnd = new Random();
            Random rndtree = new Random();

            int height = Console.WindowHeight;
            int width = Console.WindowWidth;


            for (int i = 0; i < 30; i++)
                snow.Add(new Snow(rnd.Next(width),rnd.Next(height)));

            height = Console.WindowHeight - 1;
            width = Console.WindowWidth - 1;
            Console.Clear();
            Console.CursorVisible = false;

            StringBuilder sb = new StringBuilder();
            int counter = 0;
            int seed = 0;

            while (true)
            {
                if (width != Console.WindowWidth - 1 || height != Console.WindowHeight - 1)
                {
                    width = Console.WindowWidth - 1;
                    height = Console.WindowHeight - 1;
                    Console.Clear();
                }

                SetCursorPosition(0, 0);

                //if the flake moved, clear the old flake. 
                foreach (var flake in snow)
                {
                    if (flake.ChangedPosition())
                    {
                        width = Math.Min(Console.WindowWidth - 1, Console.BufferWidth);
                        height = Math.Min(Console.WindowHeight - 1, Console.BufferHeight);

                        if (flake.xprev >= 0 && flake.xprev < width && flake.yprev >= 0 && flake.yprev <= height)
                        {
                            SetCursorPosition(flake.xprev, flake.yprev);
                            Console.Write(" ");
                        }
                    }
                }

                SetCursorPosition(0, 0);

                Utilities.WriteColourfultext(" - 2017 Complete! - ", ConsoleColor.Green);
                Utilities.WriteColourfultext("50 stars deposited, ", ConsoleColor.Yellow);
                Utilities.WriteColourfultext("printing is off to Santa.", ConsoleColor.Red);


                //draw the tree
                if (counter % 5 == 0)
                    seed++;

                rndtree = new Random(seed);

                SetCursorPosition(0, 2);
                if (width >27)
                    WriteConsoleChirstmasTree(rndtree);

                //draw flakes
                foreach (var flake in snow)
                {
                    if (flake.ChangedPosition())
                    {
                        width = Math.Min(Console.WindowWidth - 1, Console.BufferWidth);
                        height = Math.Min(Console.WindowHeight - 1, Console.BufferHeight);

                        if (flake.x >= 0 && flake.x < width && flake.y >= 0 && flake.y <= height)
                        {
                            SetCursorPosition(flake.x, flake.y);
                            Console.Write(flake.flake);
                        }
                    }

                    flake.Move(rnd);
                }

                int count = snow.RemoveAll(s => s.y > height);
                for (int i = 0; i < count; i++)
                    snow.Add(new Snow(rnd.Next(width), 0));

                SetCursorPosition(0, 0);
                Console.CursorVisible = false;

                System.Threading.Thread.Sleep(750);
                counter++;

            }

            //Console.CursorVisible = true;


        }

        /// <summary>
        /// Ingore exceptions that arrise when the console resizes. 
        /// </summary>
        private static void SetCursorPosition(int left, int top)
        {
            try
            {
                Console.SetCursorPosition(left, top);
            }
            catch { }
        }

        public static void WLC(string str, ConsoleColor color)
        {
            WC(str, color);
            Console.WriteLine();
        }

        public static void WC(string str, ConsoleColor color)
        {
            int whitespace = str.TakeWhile(Char.IsWhiteSpace).Count();
            string trim = str.TrimStart();

            SetCursorPosition(Console.CursorLeft + whitespace, Console.CursorTop);


            foreach (var letter in trim)
            {
                if (letter == '*')
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = color;

                Console.Write(letter);
            }
        }


        public static void WC(string str, ConsoleColor defaultcolour, List<ConsoleColor> colours, Random rnd)
        {
            int whitespace = str.TakeWhile(Char.IsWhiteSpace).Count();
            string trim = str.TrimStart();
            SetCursorPosition(Console.CursorLeft + whitespace, Console.CursorTop);

            foreach (var letter in trim)
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


        public static void WriteInputFile(string filename, int maxCharacters = 35)
        {
            string path = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar;
            string file = Path.GetFileName(filename);

            maxCharacters = Math.Max(maxCharacters, 23);

            string outputstring = filename;
            if ( path.Length > maxCharacters)
            {
                outputstring = path.Substring(0,15);
                outputstring += " ... ";
                outputstring += path.Substring(Math.Max(path.Length - 15, 0));
            }

            var curColour = Console.ForegroundColor;
            Console.Write("Input: ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(outputstring);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(file);


            Console.ForegroundColor = curColour;
        }



        public static Result WriteOutput(int? sum, int? expected)
        {
            bool foundexpected = sum == expected;
            
            if(sum == null)
            {
                Utilities.WriteColourfultext("Output not found", ConsoleColor.Red);
            }

            if (expected != null)
            {
                Console.Write("output: " + sum.ToString() + " expected: ");
                Utilities.WriteColoufulBool(foundexpected);
                Console.WriteLine();
            }
            else
                Console.WriteLine("output: " + sum.ToString());

            if (foundexpected)
                return Result.Expected;
            else
            {
                System.Diagnostics.Debug.Assert(false);
                Environment.Exit(1);
                return Result.Error;
            }
                
        }

        public static Result WriteOutput(string resut, string expected)
        {
            bool foundexpected = resut.Equals(expected);

            if (expected != null)
            {
                Console.Write("output: " + resut + " expected: ");
                Utilities.WriteColoufulBool(foundexpected);
                Console.WriteLine();
            }
            else
                Console.WriteLine("output: " + resut);

            if (foundexpected)
                return Result.Expected;
            else
            {
                System.Diagnostics.Debug.Assert(false);
                Environment.Exit(1);
                return Result.Error;
            }
        }

        /// <summary>
        /// Console.Write with specified colour
        /// </summary>
        public static void WriteColourfultext(string colourtext, ConsoleColor colour)
        {
            var curColour = Console.ForegroundColor;

            Console.ForegroundColor = colour;
            Console.Write(colourtext);

            Console.ForegroundColor = curColour;
        }

        /// <summary>
        /// Console.Write with specified colour
        /// </summary>
        public static void WriteColourfultext(char colourchar, ConsoleColor colour)
        {
            var curColour = Console.ForegroundColor;

            Console.ForegroundColor = colour;
            Console.Write(colourchar);

            Console.ForegroundColor = curColour;
        }

        /// <summary>
        /// Write alternating non-whitespace characters with the specified colours
        /// </summary>
        public static void WriteColourfultextAlternating(string colourtext, ConsoleColor[] consoleColors = null )
        {
            if (consoleColors == null)
                consoleColors = new ConsoleColor[]{ ConsoleColor.Red, ConsoleColor.Blue };

            int index = 0;
            foreach (var character in colourtext)
            {
                if (char.IsWhiteSpace(character))
                    index = ++index % consoleColors.Length;

                WriteColourfultext(character, consoleColors[index]);
            }
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
        /// One column of integers
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
