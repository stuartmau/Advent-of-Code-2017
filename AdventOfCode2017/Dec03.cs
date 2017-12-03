using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2017
{
    public static class Dec03
    {
        public static void Run()
        {
            Console.WriteLine(Utilities.LineSeparator);

            Console.WriteLine("December 3rd Part1");

            Part1(1, 0);

            Part1(2, 1);
            Part1(3, 2);
            Part1(8, 1);
            Part1(9, 2);

            Part1(23, 2);
            Part1(24, 3);
            Part1(25, 4);
            Part1(10, 3);
            Part1(11, 2);

            Part1(28, 3);
            Part1(26, 5);
            Part1(49, 6);


            Part1(1024, 31);

            Part1(368078, 371);


            Console.WriteLine();
            Console.WriteLine("December 3rd Part2");

            Part2(2,4);
            Part2(4,5);
            Part2(5,10);
            Part2(10,11);
            Part2(11,23);
            Part2(23,25);
            Part2(25,26);
            Part2(26,54);
            Part2(54,57);
            Part2(57,59);
            Part2(59,122);
            Part2(330,351);
            Part2(747,806);

            Part2(368078, 369601);
        }

        /// <summary>
        /// arbitray sized rings OK!
        /// </summary>
        public static void Part1(int input, int? expected = null)
        {
            //check for center
            int steps = 0;

            if (input == 1)
            {
                Utilities.WriteOutput(steps, expected);
                return;
            }

            //find ring count
            int rank = (int)Math.Truncate(Math.Sqrt(input-1)) ;
            int rankmod = rank;

            if (rank % 2 == 0)
                rankmod--;

            // number of elements per row/column of ring rank, eg rank 2 has five elements per row and column
            int rootmod = rankmod + 2;

            int max = rootmod * rootmod;
            int min = (rootmod - 2) * (rootmod -2) +1;

            //items in outer ring 
            int ringelementcount = max - min +1;
            int edgeindex = input - min;

            //find ring rank
            int ringrank = (int)Math.Round((double)rankmod / 2, MidpointRounding.AwayFromZero);

            //find index in outer ring (right center is index 0)
            int shiftededgeindex = edgeindex - (ringrank-1);
            shiftededgeindex = (shiftededgeindex + ringelementcount) % ringelementcount;

            //calculate extra offset due to position in ring
            //i.e. axis are 0, corners of square = rank
            int var1 = shiftededgeindex % (ringrank *2);
            int var2 = 0;
            if (var1 > ringrank)
                var2 = (shiftededgeindex % ringrank) *2;

            int var3 = var1 - var2;

            //add ring and offset count to find final moves
            int moves = ringrank + var3;

            Console.Write("input: " + input + " ");
            Utilities.WriteOutput(moves, expected);
        }


        /// <summary>
        /// walk around the array and calculate
        /// </summary>
        public static void Part2(int input, int? expected = null)
        {
            int result = 0;

            int width = 700; // from part one result we know that puzzel input 368078 is about 606th ring, an array slightly larger will suffice
            int halfwidth = width / 2;
            int[,] s = new int[width, width]; //0,0 top left

            int i = halfwidth; //column
            int j = halfwidth; //row

            s[i, j] = 1;
            i++;

            //set second cell, immediately to the right 
            s[i, j] = Sum(s, i, j);


            while (true)
            {
                //get direction
                var direction = GetDirection(s, i, j);

                //increment
                if (direction == Direction.Left)
                    i--;
                if (direction == Direction.Right)
                    i++;
                if (direction == Direction.Up)
                    j--;
                if (direction == Direction.Down)
                    j++;

                //set
                s[i, j] = Sum(s, i, j);

                if (s[i, j] > input)
                {
                    result = s[i, j];
                    break;
                }
            }

            Console.Write("input: " + input + " ");
            Utilities.WriteOutput(result, expected);

        }


        public enum Direction { Left, Up, Right, Down };

        /// <summary>
        /// get next direction
        /// </summary>
        private static Direction GetDirection(int[,] s, int i, int j)
        {
            //          1                   1
            //    0   right   0       1   right   0
            //         0                    0

            //      0                       0   
            //  1   up  0               1   up  0  
            //      0                       1

            //    0                     0
            //0   left    0       0   left    1
            //    1                     1

            //    0                     1
            //0   down    1       0   down    1
            //    0                     0

            //left && right && up && down

            bool left =     HasValue(s, i - 1, j);
            bool right =    HasValue(s, i + 1, j);
            bool up =       HasValue(s, i, j - 1);
            bool down =     HasValue(s, i, j + 1);


            if (!left && !right && up && !down)
                return Direction.Right;

            if (left && !right && up && !down)
                return Direction.Right;


            if (left && !right && !up && !down)
                return Direction.Up;

            if (left && !right && !up && down)
                return Direction.Up;

            if (!left && !right && !up && down)
                return Direction.Left;

            if (!left && right && !up && down)
                return Direction.Left;


            if (!left && right && !up && !down)
                return Direction.Down;

            if (!left && right && up && !down)
                return Direction.Down;


            throw new Exception();
        }

        /// <summary>
        /// returns true if cell has been set != 0
        /// </summary>
        private static bool HasValue(int[,] s, int i, int j)
        {
            if (s[i, j] != 0)
                return true;
            else
                return false;
        }



        private static int Sum(int[,] square, int i, int j)
        {
            int sum = 0;
            //top 
            sum += square[i - 1, j - 1];
            sum += square[i    , j - 1];
            sum += square[i + 1, j - 1];

            sum += square[i - 1, j];
            sum += square[i + 1, j];

            //bottom
            sum += square[i - 1, j + 1];
            sum += square[i    , j + 1];
            sum += square[i + 1, j + 1];

            return sum;
        }
    }
}
