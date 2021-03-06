﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode2017
{
    class AdventOfCode
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Utilities.WLC("      Advent of Code 2017", Console.ForegroundColor);
            Utilities.WLC("         by Stuart Mau", Console.ForegroundColor);
            Console.WriteLine();
            Utilities.WriteConsoleChirstmasTree(new Random());

            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = System.IO.Path.GetDirectoryName(location);
            path = System.IO.Path.Combine(path, "input");

            Dec01.Run();
            Dec02.Run(path);
            Dec03.Run();
            Dec04.Run(path);
            Dec05.Run(path);
            Dec06.Run(path);
            Dec07.Run(path);
            Dec08.Run(path);
            Dec09.Run(path);
            Dec10.Run(path);
            Dec11.Run(path);
            Dec12.Run(path);
            Dec13.Run(path);
            Dec14.Run(path);
            Dec15.Run(path);
            Dec16.Run(path);
            Dec17.Run(path);
            Dec18.Run(path);
            Dec19.Run(path);
            Dec20.Run(path);
            Dec21.Run(path);
            Dec22.Run(path);
            Dec23.Run(path);
            Dec24.Run(path);
            Dec25.Run(path);


            Utilities.WriteAnimatedConsoleChirstmasTree();

            Console.Read();
        }


    }
}
