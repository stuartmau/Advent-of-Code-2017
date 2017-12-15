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

            //Part1BFS("ne,ne,ne", 10, 3);
            //Part1BFS("ne,ne,sw,sw", 10, 0);
            //Part1BFS("ne,ne,s,s", 10, 2);
            //Part1BFS("se,sw,se,sw,sw", 10, 3);
            //Part1BFSinput(Path.Combine(path, "dec11.txt"), 2000, 805);

            Part1Coordinate("ne,ne,ne", 3);
            Part1Coordinate("ne,ne,sw,sw",  0);
            Part1Coordinate("ne,ne,s,s", 2);
            Part1Coordinate("se,sw,se,sw,sw",  3);
            Part1Coordinateinput(Path.Combine(path, "dec11.txt"), 805);


            Console.WriteLine();
            Console.WriteLine("Part2");
            //Part2BFSinput(Path.Combine(path, "dec11.txt"), 3000, 1535);
            Part2Coordinateinput(Path.Combine(path, "dec11.txt"), 1535);
        }

        /// <summary>
        /// Find the shortest path across a hex grid using BFS.
        /// </summary>
        public static void Part1BFSinput(string filename, int gridsize, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            foreach (var line in strings)
                Part1BFS(line, gridsize, expected);
        }

        /// <summary>
        /// Find the shortest path across a hex grid using cube coordinates. 
        /// </summary>
        public static void Part1Coordinateinput(string filename, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            foreach (var line in strings)
                Part1Coordinate(line, expected);
        }

        /// <summary>
        ///  Find the furthest distance from home on hex grid using BFS.
        /// </summary>
        public static void Part2BFSinput(string filename, int gridsize, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            foreach (var line in strings)
                Part2BFS(line, gridsize, expected);
        }

        /// <summary>
        ///  Find the furthest distance from home on hex grid using cube coordinates.
        /// </summary>
        public static void Part2Coordinateinput(string filename, int? expected = null)
        {
            var strings = Utilities.LoadStrings(filename);

            foreach (var line in strings)
                Part2Coordinate(line, expected);
        }

        /// <summary>
        /// Find the shortest path across a hex grid using cube coordinates. 
        /// https://www.redblobgames.com/grids/hexagons/
        /// </summary>
        public static void Part1Coordinate(string input, int? expected = null)
        {
            var walk = input.Split(',');

            int a = 0, b = 0, c = 0;

            for (int i = 0; i < walk.Length; i++)
            {
                switch (walk[i])
                {
                    case "nw":
                        a++;
                        b--;
                        break;
                    case "n":
                        a++;
                        c--;
                        break;
                    case "ne":
                        b++;
                        c--;
                        break;
                    case "se":
                        a--;
                        b++;
                        break;
                    case "s":
                        a--;
                        c++;
                        break;
                    case "sw":
                        b--;
                        c++;
                        break;
                }
            }

            //caluclate distance
            int distance = CubeDistance(a, b, c, 0, 0, 0);
            Utilities.WriteOutput(distance, expected);
        }


        /// <summary>
        /// Find the shortest path across a hex grid using cube coordinates. 
        /// https://www.redblobgames.com/grids/hexagons/
        /// </summary>
        public static void Part2Coordinate(string input, int? expected = null)
        {
            var walk = input.Split(',');

            int a = 0, b = 0, c = 0;
            int maxdist = 0;

            for (int i = 0; i < walk.Length; i++)
            {
                switch (walk[i])
                {
                    case "nw":
                        a++;
                        b--;
                        break;
                    case "n":
                        a++;
                        c--;
                        break;
                    case "ne":
                        b++;
                        c--;
                        break;
                    case "se":
                        a--;
                        b++;
                        break;
                    case "s":
                        a--;
                        c++;
                        break;
                    case "sw":
                        b--;
                        c++;
                        break;
                }

                int distance = CubeDistance(a, b, c, 0, 0, 0);

                if (distance > maxdist)
                    maxdist = distance;
            }
           
            Utilities.WriteOutput(maxdist, expected);
        }

        /// <summary>
        /// Hexagon Cube distance
        /// </summary>
        private static int CubeDistance(int a, int b, int c, int d, int e, int f)
        {
            return (Math.Abs(a - d) + Math.Abs(b - e) + Math.Abs(c - f)) / 2;
        }

        /// <summary>
        /// Find the shortest path across a hex grid brute force.
        /// </summary>
        public static void Part1BFS(string input, int gridsize, int? expected = null)
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

            //simplify walk
            int[] counter = new int[6];

            foreach (var direction in walk)
            {
                if (direction == "nw")
                    counter[0]++;
                else if (direction == "n")
                    counter[1]++;
                else if (direction == "ne")
                    counter[2]++;
                else if (direction == "sw")
                    counter[3]++;
                else if (direction == "s")
                    counter[4]++;
                else if (direction == "se")
                    counter[5]++;
            }

            int nwse = counter[0] - counter[5];
            int ns = counter[1] - counter[4];
            int nesw = counter[2] - counter[3];

            //travers simplified walk
            for(int i =0; i< Math.Abs(nwse); i++)
                    current = current.GetNeighbourNwSe(nwse);
            for (int i = 0; i < Math.Abs(ns); i++)
                current = current.GetNeighbourNS(ns);
            for (int i = 0; i < Math.Abs(nesw); i++)
                current = current.GetNeighbourNeSw(nesw);

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
        /// Find the shortest path across a hex grid.
        /// </summary>
        public static void Part2BFS(string input, int gridsize, int? expected = null)
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

            internal Hex GetNeighbourNwSe(int count)
            {
                if (count > 0)
                    return nw;
                else if (count < 0)
                    return se;
                else
                    return this;
            }

            internal Hex GetNeighbourNS(int count)
            {
                if (count > 0)
                    return n;
                else if (count < 0)
                    return s;
                else
                    return this;
            }

            internal Hex GetNeighbourNeSw(int count)
            {
                if (count > 0)
                    return ne;
                else if (count < 0)
                    return sw;
                else
                    return this;
            }
        }
    }
}
