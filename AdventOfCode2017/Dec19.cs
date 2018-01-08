using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec19
    {
        enum Direction { up, down, left, right, none };

        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 19th - A Series of Tubes -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path,"dec19test.txt"), "ABCDEF");
            Part1(Path.Combine(path, "dec19.txt"), "MOABEUCWQS");

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec19test.txt"), 38);
            Part2(Path.Combine(path, "dec19.txt"),18058);
        }

        /// <summary>
        /// Find the text along a path
        /// </summary>
        public static Result Part1(string filename, string expected = null)
        {
            //read input and generate grid
            var lines = Utilities.LoadStrings(filename);

            int gridwidth = lines[0].Length;
            int gridheight = lines.Count;

            char[,] grid = new char[gridwidth, gridheight];

            for (int i = 0; i < gridheight; i++)
            {
                string line = lines[i];
                for (int j = 0; j < gridwidth; j++)
                {
                    grid[j,i] = line[j];
                }
            }

            //Find starting position
            int x = 0, y = 0;

            for (int i = 0; i < gridwidth; i++)
            {
                if (grid[i,0] == '|')
                {
                    x = i;
                    break;
                }
            }

            //navigate maze
            StringBuilder sb = new StringBuilder();
            Direction direction = Direction.down;

            while(true)
            {
                switch(direction)
                {
                    case Direction.down:
                        y++; break;
                    case Direction.up:
                        y--; break;
                    case Direction.left:
                        x--; break;
                    case Direction.right:
                        x++; break;
                }

                char letter = grid[x, y];

                if (char.IsWhiteSpace(letter))
                    break;

                if (char.IsLetter(grid[x, y]))
                    sb.Append(grid[x, y]);

                if (letter == '+' && (direction == Direction.up || direction == Direction.down))
                {
                    //change direction to left or right
                    if (x > 0 && !char.IsWhiteSpace(grid[x - 1, y]))
                        direction = Direction.left;
                    else if(!char.IsWhiteSpace(grid[x + 1, y]))
                        direction = Direction.right;
                    else
                        direction = Direction.none;

                }
                else if (letter == '+' && (direction == Direction.left || direction == Direction.right))
                {
                    //change direction to up or down
                    if (y > 0 && !char.IsWhiteSpace(grid[x, y-1]))
                        direction = Direction.up;
                    else if (!char.IsWhiteSpace(grid[x, y + 1]))
                        direction = Direction.down;
                    else
                        direction = Direction.none;
                }
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(sb.ToString(), expected);
        }


        /// <summary>
        /// Count steps along a path. 
        /// </summary>
        public static Result Part2(string filename, int? expected = null)
        {
            //read input and generate grid
            var lines = Utilities.LoadStrings(filename);

            int gridwidth = lines[0].Length;
            int gridheight = lines.Count;

            char[,] grid = new char[gridwidth, gridheight];

            for (int i = 0; i < gridheight; i++)
            {
                string line = lines[i];
                for (int j = 0; j < gridwidth; j++)
                {
                    grid[j, i] = line[j];
                }
            }

            //Find starting position
            int x = 0, y = 0;

            for (int i = 0; i < gridwidth; i++)
            {
                if (grid[i, 0] == '|')
                {
                    x = i;
                    break;
                }
            }

            //navigate maze
            StringBuilder sb = new StringBuilder();
            Direction direction = Direction.down;
            int count = 0;

            while (true)
            {
                switch (direction)
                {
                    case Direction.down:
                        y++; break;
                    case Direction.up:
                        y--; break;
                    case Direction.left:
                        x--; break;
                    case Direction.right:
                        x++; break;
                }
                
                char letter = grid[x, y];
                count++;

                if (char.IsWhiteSpace(letter))
                    break;

                

                if (char.IsLetter(grid[x, y]))
                    sb.Append(grid[x, y]);

                if (letter == '+' && (direction == Direction.up || direction == Direction.down))
                {
                    //change direction to left or right
                    if (x > 0 && !char.IsWhiteSpace(grid[x - 1, y]))
                        direction = Direction.left;
                    else if (!char.IsWhiteSpace(grid[x + 1, y]))
                        direction = Direction.right;
                    else
                        direction = Direction.none;

                }
                else if (letter == '+' && (direction == Direction.left || direction == Direction.right))
                {
                    //change direction to up or down
                    if (y > 0 && !char.IsWhiteSpace(grid[x, y - 1]))
                        direction = Direction.up;
                    else if (!char.IsWhiteSpace(grid[x, y + 1]))
                        direction = Direction.down;
                    else
                        direction = Direction.none;
                }

            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(count, expected);
        }
    }
}
