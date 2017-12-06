using System;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode2017
{
    class AdventOfCode
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\stuart\Documents\Visual Studio 2017\Projects\AdventOfCode2017\AdventOfCode2017\input\";

            Dec01.Run();
            Dec02.Run(path);
            Dec03.Run();
            Dec04.Run(path);
            Dec05.Run(path);
            Dec06.Run(path);

            Console.Read();
            Console.WriteLine("KTHNXBYE");
        }


    }
}
