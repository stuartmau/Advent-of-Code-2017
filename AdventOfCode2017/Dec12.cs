using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec12
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 12th - Digital Plumber -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path, "dec12test.txt"), 6);
            Part1(Path.Combine(path, "dec12.txt"),115);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec12test.txt"), 2);
            Part2(Path.Combine(path, "dec12.txt"), 221);
        }

        /// <summary>
        /// Count connected IDs
        /// </summary>
        public static Result Part1(string filename, int? expected = null)
        {
            var lines = Utilities.LoadStrings(filename);

            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();

            foreach(var line in lines)
            {
                var split = line.Replace(",", "").Split(' ');

                int id = int.Parse(split[0]);

                List<int> connections = new List<int>();
                for (int i = 2; i < split.Length; i++)
                    connections.Add(int.Parse(split[i]));

                map.Add(id, connections);
            }

            //Count connections to program id 0,
            List<int> found = new List<int>();
            List<int> searchIDs = new List<int>();

            found.Add(0);
            searchIDs.AddRange(map[0]);

            while (searchIDs.Count > 0)
            {
                List<int> nextToSeach = new List<int>();
                foreach (var other in searchIDs)
                {
                    if (!found.Contains(other))
                    {
                        found.Add(other);
                        nextToSeach.AddRange(map[other]);
                    }
                }

                searchIDs = nextToSeach;
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(found.Count, expected);
        }


        /// <summary>
        /// Count groups
        /// </summary>
        public static Result Part2(string filename, int? expected = null)
        {
            var lines = Utilities.LoadStrings(filename);

            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();

            foreach (var line in lines)
            {
                var split = line.Replace(",", "").Split(' ');

                int id = int.Parse(split[0]);

                List<int> connections = new List<int>();
                for (int i = 2; i < split.Length; i++)
                    connections.Add(int.Parse(split[i]));

                map.Add(id, connections);
            }


            //Count connections to program id 0,
            int groups = 0;
            while(map.Count > 0)
            {
                int startid = map.First().Key;
                List<int> found = FindConnected(startid, map);
                foreach(int foundid in found)
                {
                    map.Remove(foundid);
                }
                groups++;
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(groups, expected);
        }

        private static List<int> FindConnected(int startID, Dictionary<int, List<int>> map)
        {
            List<int> found = new List<int>();
            List<int> searchIDs = new List<int>();

            found.Add(startID);
            searchIDs.AddRange(map[startID]);

            while (searchIDs.Count > 0)
            {
                List<int> nextToSeach = new List<int>();
                foreach (var other in searchIDs)
                {
                    if (!found.Contains(other))
                    {
                        found.Add(other);
                        nextToSeach.AddRange(map[other]);
                    }
                }

                searchIDs = nextToSeach;
            }

            return found;
        }
    }
}
