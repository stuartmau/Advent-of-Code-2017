using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2017
{
    public static class ConsoleUtils
    {
        public static void WriteColoufulBool(bool value)
        {
            WriteColoufulBool(value, ConsoleColor.Green, ConsoleColor.Red);
        }

        public static void WriteColoufulBool(bool value, ConsoleColor truecolour, ConsoleColor falsecolour)
        {
            var curColour = Console.ForegroundColor;
            Console.ForegroundColor = truecolour;

            if (!value)
                Console.ForegroundColor = falsecolour;

            Console.Write(value);

            Console.ForegroundColor = curColour;
        }
    }
}
