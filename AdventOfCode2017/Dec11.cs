using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec11
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 11th - Hex Ed -");
            Console.WriteLine("Part1");

            Part1Evaluate("ne,ne,ne", 10, 3);
            Part1Evaluate("ne,ne,sw,sw", 10, 0);
            Part1Evaluate("ne,ne,s,s", 10, 2);
            Part1Evaluate("se,sw,se,sw,sw", 10, 3);

            Part1(Path.Combine(path, "dec11.txt"), 3000, 805);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec11.txt"), 3000, 1535);
        }

        /// <summary>
        /// Find the shortest path across a hex grid.
        /// </summary>
        public static void Part1(string filename, int gridsize, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            foreach (var line in strings)
                Part1Evaluate(line, gridsize, expected);
        }

        /// <summary>
        /// Find the shortest path across a hex grid.
        /// </summary>
        public static void Part1Evaluate(string input, int gridsize, int? expected = null)
        {
            var walk = input.Split(',');

            List<Hex> grid = new List<Hex>();

            //make a grid of hexagons
            // row 1:  1   3   5      
            //       0   2 │ 4
            // row 2:  1  >3<  5
            //       0   2 │ 4
            // row 3:  1   3   5
            //       0   2   4
            List<List<Hex>> grid2d = new List<List<Hex>>();
            List<Hex> network = new List<Hex>(grid);


            for (int i = 0; i < gridsize; i++)
            {

                List<Hex> row = new List<Hex>();
                for (int j = 0; j < gridsize; j++)
                {
                    var hex = new Hex();
                    row.Add(hex);
                    network.Add(hex);

                }
                grid2d.Add(row);
            }

            //connect and set neighbours
            for (int i = 1; i < gridsize - 1; i++)
            {
                List<Hex> above = grid2d[i - 1];
                List<Hex> row = grid2d[i];
                List<Hex> below = grid2d[i + 1];

                for (int j = 1; j < gridsize - 1; j++)
                {
                    var node = row[j];

                    //note that each column has a different connection pattern
                    if (j % 2 == 1)
                    {
                        node.nw = above[j - 1];
                        node.n = above[j];
                        node.ne = above[j + 1];

                        node.sw = row[j - 1];
                        node.s = below[j];
                        node.se = row[j + 1];
                    }
                    else
                    {
                        node.nw = row[j - 1];
                        node.n = above[j];
                        node.ne = row[j + 1];

                        node.sw = below[j - 1];
                        node.s = below[j];
                        node.se = below[j + 1];
                    }
                }
            }

            //Walk along input path to find child location. 
            var current = grid2d[gridsize / 2][gridsize / 2];
            Hex home = current;

            //count out from home
            FillHexDistances(home);

            foreach (var direction in walk)
            {
                current = current.GetNeighbour(direction);
            }

            Utilities.WriteOutput(current.dist, expected);

        }

        private static void FillHexDistances(Hex home)
        {
            home.distcheck = true;
            int dist = 0;

            List<Hex> neighbours = new List<Hex>();
            neighbours.Add(home);

            while (neighbours.Count> 0)
            {
                List<Hex> newNeighbours = new List<Hex>();
                //Find neighbours to work on
                foreach (var current in neighbours)
                {
                    if (current.n != null && !current.n.distcheck)
                        SetNeighbour(newNeighbours, current.n);

                    if (current.nw != null && !current.nw.distcheck)
                        SetNeighbour(newNeighbours, current.nw);

                    if (current.ne != null && !current.ne.distcheck)
                        SetNeighbour(newNeighbours, current.ne);

                    if (current.s != null && !current.s.distcheck)
                        SetNeighbour(newNeighbours, current.s);

                    if (current.sw != null && !current.sw.distcheck)
                        SetNeighbour(newNeighbours, current.sw);

                    if (current.se != null && !current.se.distcheck)
                        SetNeighbour(newNeighbours, current.se);
                }

                //set distance for the new neighbours
                //and check if they are the child
                foreach (var neighbour in newNeighbours)
                    neighbour.dist = dist + 1;

                dist++;

                neighbours = newNeighbours;
            }
        }

        private static void SetNeighbour(List<Hex> newNeighbours, Hex hex)
        {
            newNeighbours.Add(hex);
            hex.distcheck = true;
        }


        /// <summary>
        ///  Find the furthest distance from home on hex grid
        /// </summary>
        public static void Part2(string filename, int gridsize, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            foreach (var line in strings)
                Part2Evaluate(line, gridsize, expected);
        }


        /// <summary>
        /// Find the shortest path across a hex grid.
        /// </summary>
        public static void Part2Evaluate(string input, int gridsize, int? expected = null)
        {
            var walk = input.Split(',');

            List<Hex> grid = new List<Hex>();

            //make a grid of hexagons
            // row 1:  1   3   5      
            //       0   2 │ 4
            // row 2:  1  >3<  5
            //       0   2 │ 4
            // row 3:  1   3   5
            //       0   2   4
            List<List<Hex>> grid2d = new List<List<Hex>>();
            List<Hex> network = new List<Hex>(grid);


            for (int i = 0; i < gridsize; i++)
            {

                List<Hex> row = new List<Hex>();
                for (int j = 0; j < gridsize; j++)
                {
                    var hex = new Hex();
                    row.Add(hex);
                    network.Add(hex);

                }
                grid2d.Add(row);
            }

            //connect and set neighbours
            for (int i = 1; i < gridsize - 1; i++)
            {
                List<Hex> above = grid2d[i - 1];
                List<Hex> row = grid2d[i];
                List<Hex> below = grid2d[i + 1];

                for (int j = 1; j < gridsize - 1; j++)
                {
                    var node = row[j];

                    //note that each column has a different connection pattern
                    if (j % 2 == 1)
                    {
                        node.nw = above[j - 1];
                        node.n = above[j];
                        node.ne = above[j + 1];

                        node.sw = row[j - 1];
                        node.s = below[j];
                        node.se = row[j + 1];
                    }
                    else
                    {
                        node.nw = row[j - 1];
                        node.n = above[j];
                        node.ne = row[j + 1];

                        node.sw = below[j - 1];
                        node.s = below[j];
                        node.se = below[j + 1];
                    }
                }
            }


            //Walk along input path to find child location. 
            var current = grid2d[gridsize / 2][gridsize / 2];
            Hex home = current;

            FillHexDistances(home);

            //walk path checking max distance 
            int maxdist = 0;
            foreach (var direction in walk)
            {
                current = current.GetNeighbour(direction);
                if (current.dist > maxdist)
                    maxdist = current.dist;
            }

            Utilities.WriteOutput(maxdist, expected);
        }


        [DebuggerDisplay("{id}")]
        private class Hex
        {
            //  \ n  /
            //nw +--+ ne
            //  /    \
            //-+      +-
            //  \    /
            //sw +--+ se
            //  / s  \

            public Hex nw = null;
            public Hex n = null;
            public Hex ne = null;
            public Hex sw = null;
            public Hex s = null;
            public Hex se = null;

            public bool distcheck = false;
            public int dist;

            internal Hex GetNeighbour(string direction)
            {
                switch (direction)
                {
                    case "nw":
                        return nw;
                    case "n":
                        return n;
                    case "ne":
                        return ne;
                    case "sw":
                        return sw;
                    case "s":
                        return s;
                    case "se":
                        return se;
                    default:
                        throw new Exception("unknown direction: " + direction);
                }
            }




        }
    }
}
