using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec13
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 13th - Packet Scanners -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path, "dec13test.txt"), 24);
            Part1(Path.Combine(path, "dec13.txt"), 1704);

            Console.WriteLine();
            Console.WriteLine("Part2");

            //Part2(Path.Combine(path, "dec13test.txt"), 10);
            //Part2(Path.Combine(path, "dec13.txt"), 3970918);
            //Part2ArrayVersion(Path.Combine(path, "dec13.txt"), 3970918);
            Part2cycleVersion(Path.Combine(path, "dec13test.txt"), 10);
            Part2cycleVersion(Path.Combine(path, "dec13.txt"), 3970918);
        }

        /// <summary>
        /// Count the number of times caught by the scanner.
        /// </summary>
        public static Result Part1(string filename, int? expected = null)
        {
            //depth: range
            //0: 3
            //1: 2
            //4: 4
            //6: 4
            // 0   1   2   3   4   5   6
            //[ ] [ ] ... ... [ ] ... [ ]
            //[ ] [ ]         [ ]     [ ]
            //[ ]             [ ]     [ ]
            //                [ ]     [ ]

            var lines = Utilities.LoadStrings(filename);

            Dictionary<int, int[]> grid = new Dictionary<int, int[]>();


            //add scanner input
            foreach(var line in lines)
            {
                var split = line.Replace(":", "").Split(' ');
                int id = int.Parse(split[0]);

                //depth, scanner location, 
                int[] range = new int[2];
                range[0] = int.Parse(split[1]);
                range[1] = 0;

                grid.Add(id, range);
            }

            //add remaining input
            int max = grid.Last().Key;
            for(int i = 0; i< max; i++)
            {
                if (!grid.ContainsKey(i))
                {
                    grid.Add(i, new[] { 0, 0 });
                }
            }

            int layer = 0;
            int severity = 0;
            while (layer <= max)
            {
                var range = grid[layer];

                if (range[0] > 0 && range[1] == 0)
                {
                    //caught at this layer!
                    //severity increases by layer x depth 
                    severity = severity + layer * range[0];
                }

                layer++;
                SetScanner(layer, grid);
                
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(severity, expected);
        }

        /// <summary>
        /// Scanner sweeps back and forth.
        /// </summary>
        private static void SetScanner(int layer, Dictionary<int, int[]> grid)
        {
            foreach(var pair in grid)
            {
                var range = pair.Value;
                if (range[0] > 0)
                {
                    int count = range[0] - 1;
                    int mod = layer  %  count;
                    int mod2 = layer % (count * 2);

                    range[1] = mod;
                    if (mod2 >= count)
                        range[1] = count - mod;
                }

            }
        }

        /// <summary>
        /// Scanner sweeps back and forth.
        /// </summary>
        private static void SetScanner(int layer, List<int[]> grid)
        {
            foreach (var range in grid)
            {
                if (range[0] > 0)
                {
                    int count = range[0] - 1;
                    int mod = layer % count;
                    int mod2 = layer % (count * 2);

                    range[1] = mod;
                    if (mod2 >= count)
                        range[1] = count - mod;
                }
            }
        }


        /// <summary>
        /// Scanner sweeps back and forth. Set the entire run.
        /// </summary>
        private static void SetAllLayersScanner(int layer, List<int[]> grid)
        {
            foreach (var range in grid)
            {
                if (range[0] > 0)
                {
                    int count = range[0] - 1;
                    int mod = layer % count;
                    int mod2 = layer % (count * 2);

                    range[1] = mod;
                    if (mod2 >= count)
                        range[1] = count - mod;
                }
                layer++;
            }
        }


        /// <summary>
        /// Find the delay at the beginning of the trip so as not to be
        /// caught by the scanner using a dictionary. 
        /// </summary>
        public static Result Part2(string filename, int? expected = null)
        {
            var lines = Utilities.LoadStrings(filename);

            Dictionary<int, int[]> grid = new Dictionary<int, int[]>();

            //Add scanner input
            foreach (var line in lines)
            {
                var split = line.Replace(":", "").Split(' ');
                int id = int.Parse(split[0]);

                //depth, scanner location, 
                int[] range = new int[2];
                range[0] = int.Parse(split[1]);
                range[1] = 0;

                grid.Add(id, range);
            }

            //Add remaining input
            int max = grid.Last().Key;

            for (int i = 0; i < max; i++)
            {
                if (!grid.ContainsKey(i))
                    grid.Add(i, new[] { 0, 0 });
            }
            

            int count = grid.Count;
            int[] saved = new int[count];
            int delay = 0;
            bool found = false;

            while (!found)
            {
                //reset state
                for (int i = 0; i < count; i++)
                    grid[i][1] = saved[i];

                //Delay
                if (delay > 0)
                    SetScanner(delay, grid);

                //Save state
                for (int i = 0; i < count; i++)
                    saved[i] = grid[i][1];

                //Check if route through firewall. 
                int layer = 0;
                int severity = 0;


                bool caught = false;
                while (layer < count)
                {
                    var range = grid[layer];

                    if (range[0] > 0 && range[1] == 0)
                    {
                        //caught at this layer!
                        //severity increases by layer x depth 
                        severity = severity + layer * range[0];
                        caught = true;
                        break;
                    }

                    layer++;
                    if (layer < count)
                        SetScanner(delay + layer, grid);
                }

                if (!caught)
                    found = true;

                delay++;
            }


            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(delay-1, expected);
        }

        /// <summary>
        /// Find the delay at the beginning of the trip so as not to be
        /// caught by the scanner using arrays.
        /// </summary>
        public static Result Part2ArrayVersion(string filename, int? expected = null)
        {
            var lines = Utilities.LoadStrings(filename);

            int max = 0;

            //Add scanner input
            foreach (var line in lines)
            {
                var split = line.Replace(":", "").Split(' ');
                int id = int.Parse(split[0]);
                if (id > max)
                    max = id;
            }

            //generate grid
            List<int[]> grid = new List<int[]>();
            for (int i = 0; i <= max; i++)
                grid.Add(new int[2]);

            //Add scanner input
            foreach (var line in lines)
            {
                var split = line.Replace(":", "").Split(' ');
                int id = int.Parse(split[0]);
                grid[id][0] = int.Parse(split[1]);
            }


            int count = grid.Count;
            int[] saved = new int[count];
            int delay = 0;
            bool found = false;

            while (!found)
            {
                //reset state
                for (int i = 0; i < count; i++)
                    grid[i][1] = saved[i];

                //Delay
                if (delay > 0)
                    SetScanner(delay, grid);

                //Save state
                for (int i = 0; i < count; i++)
                    saved[i] = grid[i][1];

                //Check if route through firewall. 
                int layer = 0;
                int severity = 0;

                bool caught = false;
                while (layer < count)
                {
                    var range = grid[layer];

                    if (range[0] > 0 && range[1] == 0)
                    {
                        //caught at this layer!
                        //severity increases by layer x depth 
                        severity = severity + layer * range[0];
                        caught = true;
                        break;
                    }

                    layer++;
                    if (layer < count)
                        SetScanner(delay + layer, grid);
                }

                if (!caught)
                    found = true;

                delay++;
            }


            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(delay - 1, expected);
        }

        /// <summary>
        /// Find the delay at the beginning of the trip so as not to be caught by the scanner
        /// by cheking the delay and offset for each level. 
        /// </summary>
        public static Result Part2cycleVersion(string filename, int? expected = null)
        {
            var lines = Utilities.LoadStrings(filename);

            int max = 0;

            //Add scanner input
            foreach (var line in lines)
            {
                var split = line.Replace(":", "").Split(' ');
                int id = int.Parse(split[0]);
                if (id > max)
                    max = id;
            }

            //generate grid
            List<int[]> grid = new List<int[]>();
            for (int i = 0; i <= max; i++)
                grid.Add(new int[2]);

            //Add scanner input max into index 0
            foreach (var line in lines)
            {
                var split = line.Replace(":", "").Split(' ');
                int id = int.Parse(split[0]);
                grid[id][0] = int.Parse(split[1]);
            }


            int count = grid.Count;
            int[] saved = new int[count];
            int delay = 0;
            bool found = false;

            //set the cycle for each layer where the scanner catches the program
            List<int> skiplist = new List<int>();

            for (int i = 0; i <= max; i++)
            {
                int value = grid[i][0];
                if (value>0)
                {
                    int modval = (value - 2) * 2 + 2;
                    skiplist.Add(modval);
                }
                else
                {
                    skiplist.Add(0);
                }
                    
            }
                

            while (!found)
            {
                bool caught = false;

                //Check route through firewall. 
                for (int i = 0; i <= max; i++)
                {
                    if (skiplist[i] != 0 && (delay + i) % skiplist[i] == 0)
                    {
                        caught = true;
                        break;
                    }
                }

                if (!caught)
                    found = true;
                else
                    delay++;
            }


            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(delay, expected);
        }

    }
}
