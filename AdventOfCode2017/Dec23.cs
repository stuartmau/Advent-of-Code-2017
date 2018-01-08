using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec23
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 23rd - Coprocessor Conflagration  -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path, "dec23.txt"), 9409);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2B(913);
        }

        /// <summary>
        /// Count the number of times the multiply instruction is executed. 
        /// </summary>
        public static Result Part1(string filename, int? expected = null)
        {
            var instructions = Utilities.LoadStrings(filename);

            Dictionary<string, int> registers = new Dictionary<string, int>();
            List<long> reg = new List<long>();

            for (int i = 0; i< 8; i++)
            {
                char regname = (char)('a' + i);
                registers.Add(regname.ToString(), i);
                reg.Add(0);
            }

            //process instructions
            long mulCount = 0;
            int instructionIndex = 0;
            bool done = false;

            while (!done && instructionIndex >= 0 && instructionIndex < instructions.Count) 
            {
                var split = instructions[instructionIndex].Split(" ");

                int name = 0;
                int X = 1;
                int Y = 2;
                int? jumpValue = null;

                switch (split[name])
                {
                    case "set":
                        Set(registers, reg, split[X], split[Y]);
                        break;
                    case "sub":
                        Sub(registers, reg, split[X], split[Y]);
                        break;
                    case "mul":
                        Multiply(registers, reg, split[X], split[Y]);
                        mulCount++;
                        break;
                    case "jnz":
                        jumpValue = Jump(registers, reg, split[X], split[Y]);
                        break;
                    default:
                        break;
                }


                //go to next instruction
                if (jumpValue == null)
                    instructionIndex++;
                else
                    instructionIndex += (int)jumpValue;

            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput((int)mulCount, expected);
        }


        /// <summary>
        /// C# version of input code without loop optimisation.
        /// </summary>
        public static Result Part2A(int? expected = null)
        {
            long b = (99 * 100) - -100_000;
            long c = b - -17000;
            long g = 0;
            long h = 0;

            do //loop A
            {
                long f = 1;
                long d = 2;

                do //loop B
                {
                    long e = 2;
                    {
                        do  //loop C
                        {
                            g = (d * e) - b;

                            if (g == 0)
                                f = 0;

                            e = e - -1;
                            g = e - b;
                        }
                        while (g != 0);
                    }

                    d = d - -1;
                    g = d - b;
                }
                while (g != 0);

                if (f == 0)
                    h = h - -1;

                g = b - c;

                if (g != 0)
                    b = b - -17;
                else
                    break;
            }
            while (true);

            return Utilities.WriteOutput((int)h, expected);
        }

        /// <summary>
        /// Count the number of non-primes in a sequence between two numbers. 
        /// </summary>
        public static Result Part2B(int? expected = null)
        {
            long b = 109_900;
            long c = 126_900;
            long h = 0;

            while (b - c <= 0)  //loop A 
            {
                //If b is prime, Loop B and C will not find 
                //a factor and therefore f will not increment h.
                //Increment h unless b is prime. 
                if (b % 2 == 0 || b % 3 == 0)
                    h++;
                else
                {
                    for (long d = 3; d <= Math.Ceiling(Math.Sqrt(b)); d += 2)
                    {
                        if (b % d == 0)
                        {
                            h++;
                            break;
                        }
                    }
                }

                //Increment b
                b += 17;
            }

            return Utilities.WriteOutput((int)h, expected);
        }

        /// <summary>
        /// Check if a number is prime for not very large numbers. 
        /// https://en.wikipedia.org/wiki/Primality_test
        /// </summary>
        private static bool IsPrime(long number)
        {
            if (number == 1)
                return false;
            if (number == 2)
                return true;
            if (number % 2 == 0 || number % 3 == 0)
                return false;

            for (long i = 5; i * i <= number; i+=6)
            {
                if (number % i == 0 || number % (i+2) == 0)
                    return false;
            }

            return true;
        }


        private static void Set(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] = value;
        }

        private static void Sub(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] -= value;
        }

        private static void Multiply(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] *= value;
        }


        private static int? Jump(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int? jumpValue = null;

            long valueX = GetValue(registers, reg, X);

            long valueY = GetValue(registers, reg, Y);

            if (valueX != 0)
                jumpValue = (int)valueY;

            return jumpValue;
        }

        [DebuggerStepThrough]
        private static long GetValue(Dictionary<string, int> registers, List<long> reg, string input)
        {
            if (!long.TryParse(input, out long value))
            {
                int index = registers[input];
                value = reg[index];
            }
            return value;
        }

    }
}
