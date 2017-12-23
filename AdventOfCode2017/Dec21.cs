using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec21
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 21st - Fractal Art -");
            Console.WriteLine("Part1");

            Part1(Path.Combine(path, "dec21test.txt"), 2, 12);
            Part1(Path.Combine(path, "dec21.txt"), 5, 150);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part1(Path.Combine(path, "dec21.txt"), 18, 2606275);
        }


        /// <summary>
        /// Enhance image and calculate number of active pixels. 
        /// </summary>
        public static void Part1(string filename, int itterations, int? expected = null)
        {
            //Load rules
            var input = Utilities.LoadStrings(filename);
            Dictionary<string, Segment> rules = new Dictionary<string, Segment>();
            foreach (var line in input)
            {
                string str = line.Replace(" => ", "|").Replace("/", "");
                var split = str.Split('|');

                Segment outpattern = new Segment(split[1]);
                rules.Add(split[0], outpattern);
            }

            //Add missing rules
            var inputRules = rules.Keys.ToList();

            foreach(string inpattern in inputRules)
            {
                Segment outputPattern = rules[inpattern];
                //flip input 
                Segment temp = new Segment(inpattern);
                temp.Flip();

                Segment output;
                if (!rules.TryGetValue(temp.Pattern, out output))
                    rules.Add(temp.Pattern, outputPattern);

                Segment current = new Segment(inpattern);
                //Check rotated and flipped 
                for (int i = 0; i < 3; i++)
                {
                    //Rotate input clockwise
                    temp = new Segment(current.Pattern);
                    current = temp;
                    temp.Rotate();

                    if (!rules.TryGetValue(temp.Pattern, out output))
                        rules.Add(temp.Pattern, outputPattern);

                    //Flip input 
                    temp = new Segment(temp.Pattern);
                    temp.Flip();

                    if (!rules.TryGetValue(temp.Pattern, out output))
                        rules.Add(temp.Pattern, outputPattern);
                }
            }

            //Start with input grid
            char[,] grid = new char[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    grid[i, j] = '.';
            }
            grid[0, 1] = '#';
            grid[1, 2] = '#';
            grid[2, 0] = '#';
            grid[2, 1] = '#';
            grid[2, 2] = '#';

            int pixelcount = 0;
            for (int itteration = 0; itteration < itterations; itteration++)
            {
                //Break grid into segments.
                List<Segment> segments = new List<Segment>();

                int gridsize = 2;
                if (grid.GetLength(0) % 2 != 0)
                    gridsize = 3;

                int columnCount = grid.GetLength(0) / gridsize;

                int offsetx = 0;
                int offsety = 0;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < columnCount * columnCount; i++)
                {
                    sb.Clear();
                    for (int y = 0; y < gridsize; y++)
                    {
                        for (int x = 0; x < gridsize; x++)
                            sb.Append(grid[x + offsetx, y + offsety]);
                    }
                    segments.Add(new Segment(sb.ToString()));

                    offsetx += gridsize;

                    if (offsetx % (gridsize * columnCount) == 0)
                    {
                        offsetx = 0;
                        offsety += gridsize;
                    }
                        
                }

                //Expand segments
                List<Segment> expanded = new List<Segment>();
                foreach (var inpattern in segments)
                    expanded.Add(rules[inpattern.Pattern]);

                //Set new grid
                int newgridsize = gridsize + 1;
                char[,] newgrid = new char[columnCount * newgridsize, columnCount * newgridsize];

                int row = 0;
                int column = 0;
                foreach (var img in expanded)
                {
                    var pattern = img.Pattern;
                    offsetx = column * newgridsize;
                    offsety = row * newgridsize;

                    for (int i = 0; i < newgridsize; i++)
                    {
                        for (int j = 0; j < newgridsize; j++)
                            newgrid[offsetx + j, offsety + i] = pattern[j * newgridsize + i];
                    }

                    column = (column + 1) % columnCount;
                    if (column == 0)
                        row++;
                }

                grid = newgrid;

                //Count pixels
                pixelcount = 0;
                foreach (var cell in grid)
                {
                    if (cell == '#')
                        pixelcount++;
                }
                    
            }

            Utilities.WriteOutput(pixelcount, expected);
        }

        private class Segment
        {
            string line1 = "";
            string line2 = "";
            string line3 = "";
            string line4 = "";
            public string Pattern { get; private set; } = "";
            public int Size { get; } = 2;

            public Segment(string input)
            {
                if (input.Length == 4)
                {
                    line1 = input.Substring(0, 2);
                    line2 = input.Substring(2, 2);
                    Size = 2;
                }
                else if (input.Length == 9)
                {
                    line1 = input.Substring(0, 3);
                    line2 = input.Substring(3, 3);
                    line3 = input.Substring(6, 3);
                    Size = 3;
                }
                else if (input.Length == 16)
                {
                    line1 = input.Substring(0, 4);
                    line2 = input.Substring(4, 4);
                    line3 = input.Substring(8, 4);
                    line4 = input.Substring(12, 4);
                    Size = 4;
                }
                else
                {
                    throw new ArgumentException("Unrecognised pattern: " + input);
                }
                Pattern = input;
            }

            private void GenerateCombinedString()
            {
                Pattern = line1 + line2;

                if (!string.IsNullOrWhiteSpace(line3))
                    Pattern += line3;

                if (!string.IsNullOrWhiteSpace(line4))
                    Pattern += line4;
            }

            public override bool Equals(object obj)
            {
                var other = obj as Segment;
                if (other == null)
                    return false;

                return Pattern.Equals(other.Pattern);
            }

            public bool Equals(string other)
            {
                return Pattern.Equals(other);
            }

            public override string ToString()
            {
                return Pattern;
            }

            internal void Flip()
            {
                if(Size == 2)
                {
                    var temp = string.Copy(line1);
                    line1 = string.Copy(line2);
                    line2 = temp;
                }
                else if (Size == 3)
                {
                    var temp = string.Copy(line1);
                    line1 = string.Copy(line3);
                    line3 = temp;
                }

                GenerateCombinedString();
            }

            internal void Rotate()
            {
                if (Size == 2)
                {
                    string row1 = new string(new char[] { line2[0], line1[0]});
                    string row2 = new string(new char[] { line2[1], line1[1]});

                    line1 = row1;
                    line2 = row2;
                }
                else if (Size == 3)
                {
                    string row1 = new string(new char[] { line3[0], line2[0], line1[0] });
                    string row2 = new string(new char[] { line3[1], line2[1], line1[1] });
                    string row3 = new string(new char[] { line3[2], line2[2], line1[2] });

                    line1 = row1;
                    line2 = row2;
                    line3 = row3;
                }

                GenerateCombinedString();
            }

            public override int GetHashCode()
            {
                return -891421391 + EqualityComparer<string>.Default.GetHashCode(Pattern);
            }
        }


        

    }
}


