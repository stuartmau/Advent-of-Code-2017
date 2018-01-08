using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec22
    {
        enum Direction { Up, Down,Left, Right, Reverse };

        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 22nd - Sporifica Virus -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path, "dec22test.txt"), 7, 5);
            Part1(Path.Combine(path, "dec22test.txt"), 70, 41);
            Part1(Path.Combine(path, "dec22test.txt"), 10_000, 5587);
            Part1(Path.Combine(path, "dec22.txt"), 10_000, 5259);

            Console.WriteLine();
            Console.WriteLine("Part2");

            Part2(Path.Combine(path, "dec22test.txt"), 100, 20, 26);
            Part2(Path.Combine(path, "dec22test.txt"), 10_000_000, 500, 2511944);
            Part2(Path.Combine(path, "dec22.txt"), 10_000_000, 500, 2511722);
        }

        /// <summary>
        /// Count number of infections whilst wandering a grid 
        /// </summary>
        public static Result Part1(string filename, int itterations, int? expected = null)
        {
            var input = Utilities.LoadStrings(filename);
            int columns = input[0].Length;
            int rows = input.Count;

            //ensure odd number of rows and columns
            int expandedColumns = (columns + itterations) % 2 == 0 ? columns + itterations + 1 : columns + itterations;
            int expandedRows = (rows + itterations) % 2 == 0 ? rows + itterations + 1 : rows + itterations;

            bool[,] grid = new bool[expandedColumns, expandedRows];
            int offsetX = (expandedColumns - columns) /2 ;
            int offsetY = (expandedRows - rows ) / 2;
            int row = 0;
            foreach (var line in input)
            {
                for(int i = 0;i < line.Length; i++)
                {
                    if (line[i] == '#')
                        grid[i + offsetX, row + offsetY] = true;
                }
                row++;
            }

            int x = expandedColumns / 2;
            int y = expandedRows / 2;

            //WriteGrid(grid, expandedColumns, expandedRows, x, y);

            int directionX = 0;
            int directionY = -1;
            int infectioncounter = 0;

            for (int itteration = 0; itteration < itterations; itteration++)
            {

                if (directionX == 0)
                {
                    if (grid[x, y])
                        directionX = 1;
                    else
                        directionX = -1;

                    if (directionY == 1)
                        directionX *= -1;

                    directionY = 0;
                }
                else
                {

                    if (grid[x, y])
                        directionY = -1;
                    else
                        directionY = 1;

                    if (directionX == 1)
                        directionY *= -1;

                    directionX = 0;
                }

                grid[x, y] = !grid[x, y];


                if (grid[x, y])
                    infectioncounter++;

                x += directionX;
                y += directionY;
            }

            //WriteGrid(grid, expandedColumns, expandedRows, x, y);

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(infectioncounter, expected);
        }


        /// <summary>
        /// How many itterations are required to infect a node with extra state
        /// </summary>
        public static Result Part2(string filename, int itterations, int arraysize, int? expected = null)
        {
            if (arraysize % 2 != 1)
                arraysize ++;

            var input = Utilities.LoadStrings(filename);
            int columns = input[0].Length;
            int rows = input.Count;

            char infected = '#';
            char clean = '.';
            char weakened = 'W';
            char flagged = 'F';

            //ensure odd number of rows and columns
            char[,] grid = new char[arraysize, arraysize];
            int offsetX = (arraysize - columns) / 2;
            int offsetY = (arraysize - rows) / 2;
            int row = 0;

            //Initalise each cell to '.' 
            for(int i = 0; i < arraysize; i++)
            {
                for (int j = 0; j < arraysize; j++)
                {
                    grid[j, i] = clean;
                }
            }

            //set infected. 
            foreach (var line in input)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == infected)
                        grid[i + offsetX, row + offsetY] = infected;
                }
                row++;
            }

            int x = arraysize / 2;
            int y = arraysize / 2;

            //WriteGrid(grid, expandedColumns, expandedRows, x, y);

            Direction direction = Direction.Up;
            int infectioncounter = 0;

            for (int itteration = 0; itteration < itterations; itteration++)
            {
                int directionX = 0;
                int directionY = 0;
                char current = grid[x, y];

                //find next direction
                Direction nextdirection = direction;
                if (current == clean)
                    nextdirection = Direction.Left;
                else if (current == infected)
                    nextdirection = Direction.Right;
                else if (current == flagged)
                    nextdirection = Direction.Reverse;

                var stepDirection = direction;
                if (current != weakened)
                    stepDirection = GetNextDirection(direction, nextdirection);

                //modify current
                if (current == clean)
                    current = weakened;
                else if (current == weakened)
                    current = infected;
                else if (current == infected)
                    current = flagged;
                else if (current == flagged)
                    current = clean;

                //increment infections
                if (current == infected)
                    infectioncounter++;

                grid[x, y] = current;


                //step forward
                directionX = 0;
                directionY = 0;
                if (stepDirection == Direction.Up)
                    directionY = -1;
                else if (stepDirection == Direction.Down)
                    directionY = 1;
                else if (stepDirection == Direction.Left)
                    directionX = -1;
                else if (stepDirection == Direction.Right)
                    directionX = 1;

                direction = stepDirection;
                x += directionX;
                y += directionY;

                //WriteGrid(grid, expandedColumns, expandedRows, x, y);
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(infectioncounter, expected);
        }

        private static Direction GetNextDirection(Direction direction, Direction next)
        {
            if (next == Direction.Reverse)
            {
                switch (direction)
                {
                    case Direction.Up:
                        return Direction.Down;
                    case Direction.Down:
                        return Direction.Up;
                    case Direction.Left:
                        return Direction.Right;
                    case Direction.Right:
                        return Direction.Left;
                    default:
                        throw new Exception("Direction error");
                }
            }
            else if (next == Direction.Left)
            {
                switch (direction)
                {
                    case Direction.Up:
                        return Direction.Left;
                    case Direction.Down:
                        return Direction.Right;
                    case Direction.Left:
                        return Direction.Down;
                    case Direction.Right:
                        return Direction.Up;
                    default:
                        throw new Exception("Direction error");
                }
            }
            else if (next == Direction.Right)
            {
                switch (direction)
                {
                    case Direction.Up:
                        return Direction.Right;
                    case Direction.Down:
                        return Direction.Left;
                    case Direction.Left:
                        return Direction.Up;
                    case Direction.Right:
                        return Direction.Down;
                    default:
                        throw new Exception("Direction error");
                }
            }
            else
                throw new Exception("Direction error");

        }


        private static void WriteGrid(bool[,] grid, int rows, int columns, int x, int y)
        {
            Console.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (x == j && y == i)
                    {
                        if (grid[j, i])
                            Console.Write("X");
                        else
                            Console.Write("O");
                    }
                    else
                    {
                        if (grid[j, i])
                            Console.Write("#");
                        else
                            Console.Write(".");
                    }
                }
                Console.WriteLine("");

            }
            Console.WriteLine("--");
        }

        private static void WriteGrid(char[,] grid, int rows, int columns, int x, int y)
        {
            Console.Clear();
            var curcol = Console.ForegroundColor;
            var highlight = ConsoleColor.Red;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (x == j && y == i)
                        Console.ForegroundColor = highlight;
                    else
                        Console.ForegroundColor = curcol;

                    Console.Write(grid[j, i]);

                }
                Console.WriteLine("");

            }
            Console.ForegroundColor = curcol;

            Console.WriteLine("--");
        }

    }
}

