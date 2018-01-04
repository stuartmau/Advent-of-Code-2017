using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace AdventOfCode2017
{
    public static class Dec18
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 18th - Duet  -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path, "dec18test.txt"), 4);
            Part1(Path.Combine(path, "dec18.txt"), 8600);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec18.txt"), 7239);

            Console.WriteLine("Part2 threaded version");
            Part2B(Path.Combine(path, "dec18.txt"), 7239);
        }

        /// <summary>
        /// Find the value of the recovered frequency. 
        /// </summary>
        public static void Part1(string filename, int? expected = null)
        {
            var instructions = Utilities.LoadStrings(filename);

            Dictionary<string, int> registers = new Dictionary<string, int>();
            List<long> reg = new List<long>();

            //Find all registers
            int count = 0;
            foreach (var line in instructions)
            {
                var split = line.Split(" ");

                if (split.Length > 1 && char.IsLetter(split[1].ToCharArray()[0]))
                {
                    if (registers.TryAdd(split[1], count))
                    {
                        reg.Add(0);
                        count++;
                    }
                }
                if (split.Length > 2 && char.IsLetter(split[2].ToCharArray()[0]))
                {
                    if (registers.TryAdd(split[2], count))
                    {
                        reg.Add(0);
                        count++;
                    }
                }
            }

            //process instructions
            long lastSnd = -1;
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
                    case "snd":
                        lastSnd = GetValue(registers, reg, split[X]);
                        break;
                    case "set":
                        Set(registers, reg, split[X], split[Y]);
                        break;
                    case "add":
                        Add(registers, reg, split[X], split[Y]);
                        break;
                    case "mul":
                        Multiply(registers, reg, split[X], split[Y]);
                        break;
                    case "mod":
                        Mod(registers, reg, split[X], split[Y]);
                        break;
                    case "rcv":
                        int xValue = (int) GetValue(registers, reg, split[X]);
                        if (xValue != 0)
                        {
                            done = true; //found result
                        }
                        break;
                    case "jgz":
                        jumpValue = Jump(registers, reg, split[X], split[Y]);
                        break;
                    default:
                        throw new Exception("unrecognised instruction" + instructions[instructionIndex]);
                }


                //go to next instruction
                if (jumpValue == null)
                    instructionIndex++;
                else
                    instructionIndex += (int)jumpValue;

            }

            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput((int)lastSnd, expected);
        }



        /// <summary>
        /// How many times did program 1 send a value
        /// </summary>
        public static void Part2(string filename, int? expected = null)
        {
            Interpreter programA = new Interpreter();
            programA.Load(0, filename);

            Interpreter programB = new Interpreter();
            programB.Load(1, filename);

            while (true)
            {
                //run prog A until it stops
                programA.Run(programB.sendqueue);

                //run prog B until it stops
                programB.Run(programA.sendqueue);

                //check if our programs have deadlocked. 
                if (programA.sendqueue.Count == 0 && programB.sendqueue.Count == 0)
                    break;
            }

            Utilities.WriteOutput(programB.sendcount, expected);
        }

        private class Interpreter
        {
            public int id;
            public Dictionary<string, int> registers = new Dictionary<string, int>();
            public List<long> reg = new List<long>();
            public Queue<long> sendqueue = new Queue<long>();
            public List<string> instructions;
            public int sendcount = 0;
            public int instructionIndex = 0;

            public void Load(int id, string filename)
            {
                this.id = id;
                instructions = Utilities.LoadStrings(filename);

                //Find all registers
                int count = 0;
                foreach (var line in instructions)
                {
                    var split = line.Split(" ");
                    if (split.Length > 1 && char.IsLetter(split[1].ToCharArray()[0]))
                    {
                        if (registers.TryAdd(split[1], count))
                        {
                            reg.Add(0);
                            count++;
                        }
                    }
                    if (split.Length > 2 && char.IsLetter(split[2].ToCharArray()[0]))
                    {
                        if (registers.TryAdd(split[2], count))
                        {
                            reg.Add(0);
                            count++;
                        }
                    }
                }

                //initalise register p to the program id. 
                reg[registers["p"]] = id;
            }


            public void Run(Queue<long> inputQueue)
            {
                //process instructions
                bool done = false;
                long lastSnd = -1;

                while (!done && instructionIndex >= 0 && instructionIndex < instructions.Count)
                {
                    var split = instructions[instructionIndex].Split(" ");

                    int name = 0;
                    int X = 1;
                    int Y = 2;
                    long? jumpValue = null;

                    switch (split[name])
                    {
                        case "snd":
                            lastSnd = GetValue(registers, reg, split[X]);
                            sendqueue.Enqueue(lastSnd);
                            sendcount++;
                            break;
                        case "set":
                            Set(registers, reg, split[X], split[Y]);
                            break;
                        case "add":
                            Add(registers, reg, split[X], split[Y]);
                            break;
                        case "mul":
                            Multiply(registers, reg, split[X], split[Y]);
                            break;
                        case "mod":
                            Mod(registers, reg, split[X], split[Y]);
                            break;
                        case "rcv":
                            long value;
                            if (inputQueue.TryDequeue(out value))
                            {
                                int registerX = registers[split[X]];
                                reg[registerX] = value;
                            }
                            else
                            {
                                return; //waiting for more input
                            }
                            break;
                        case "jgz":
                            jumpValue = Jump(registers, reg, split[X], split[Y]);
                            break;
                        default:
                            throw new Exception("unrecognised instruction" + instructions[instructionIndex]);
                    }


                    //go to next instruction
                    if (jumpValue == null)
                        instructionIndex++;
                    else
                        instructionIndex += (int)jumpValue;

                }
            }

        }


        /// <summary>
        /// How many times did program 1 send a value, run in parallel. 
        /// </summary>
        public static void Part2B(string filename, int? expected = null)
        {
            InterpreterThread programA = new InterpreterThread();
            programA.Load(0, filename);

            InterpreterThread programB = new InterpreterThread();
            programB.Load(1, filename);

            programA.SetOtherProgram(programB);
            programB.SetOtherProgram(programA);


            ////run prog A and B until they stop
            List<Task> tasks = new List<Task>();
            tasks.Add(Task<int>.Factory.StartNew(() => programA.Run()));
            tasks.Add(Task<int>.Factory.StartNew(() => programB.Run()));

            Task.WaitAll(tasks.ToArray());


            Utilities.WriteInputFile(filename);
            Utilities.WriteOutput(programB.sendcount, expected);
        }

 
        private class InterpreterThread
        {
            public int id;
            public Dictionary<string, int> registers = new Dictionary<string, int>();
            public List<long> reg = new List<long>();
            public List<string> instructions;
            public int sendcount = 0;
            public int instructionIndex = 0;


            //note that blocking collection defaults to wrapping ConcurrentQueue (FIFO)
            //https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview
            //public BlockingCollection<long> queue = new BlockingCollection<long>();
            public ConcurrentQueue<long> queue = new ConcurrentQueue<long>();
            InterpreterThread other = null;
            public object locker = new object();
            public volatile bool running = true;

            public void Load(int id, string filename)
            {
                this.id = id;
                instructions = Utilities.LoadStrings(filename);

                //Find all registers
                int count = 0;
                foreach (var line in instructions)
                {
                    var split = line.Split(" ");
                    if (split.Length > 1 && char.IsLetter(split[1].ToCharArray()[0]))
                    {
                        if (registers.TryAdd(split[1], count))
                        {
                            reg.Add(0);
                            count++;
                        }
                    }
                    if (split.Length > 2 && char.IsLetter(split[2].ToCharArray()[0]))
                    {
                        if (registers.TryAdd(split[2], count))
                        {
                            reg.Add(0);
                            count++;
                        }
                    }
                }

                //initalise register p to the program id. 
                reg[registers["p"]] = id;
            }

            public void SetOtherProgram(InterpreterThread other)
            {
                this.other = other;
            }

            public int Run()
            {
                //process instructions
                long lastSnd = -1;

                while (instructionIndex >= 0 && instructionIndex < instructions.Count)
                {
                    //toggle that this thread's instruction has finished. 

                    var split = instructions[instructionIndex].Split(" ");

                    int name = 0;
                    int X = 1;
                    int Y = 2;
                    long? jumpValue = null;

                    switch (split[name])
                    {
                        case "snd":
                            lastSnd = GetValue(registers, reg, split[X]);
                            queue.Enqueue(lastSnd);
                            sendcount++;
                            break;
                        case "set":
                            Set(registers, reg, split[X], split[Y]);
                            break;
                        case "add":
                            Add(registers, reg, split[X], split[Y]);
                            break;
                        case "mul":
                            Multiply(registers, reg, split[X], split[Y]);
                            break;
                        case "mod":
                            Mod(registers, reg, split[X], split[Y]);
                            break;
                        case "rcv":
                            long value;
                            while (true)
                            {
                                if (other.queue.TryDequeue(out value))
                                {
                                    lock (locker)
                                        running = true;

                                    int registerX = registers[split[X]];
                                    reg[registerX] = value;

                                    break;
                                }
                                else
                                {
                                    lock (locker)
                                        running = false;

                                    lock (other.locker)
                                        if (!other.running)
                                            return 0;

                                }
                            }
                            break;
                        case "jgz":
                            jumpValue = Jump(registers, reg, split[X], split[Y]);
                            break;
                        default:
                            throw new Exception("unrecognised instruction" + instructions[instructionIndex]);
                    }

                    //go to next instruction
                    if (jumpValue == null)
                        instructionIndex++;
                    else
                        instructionIndex += (int)jumpValue;
                }

                return 1;
            }

        }

        private static void Set(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] = value;
        }

        private static void Add(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] += value;
        }

        private static void Multiply(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] *= value;
        }

        private static void Mod(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int registerX = registers[X];

            long value = GetValue(registers, reg, Y);

            reg[registerX] %= value;
        }

        private static int? Jump(Dictionary<string, int> registers, List<long> reg, string X, string Y)
        {
            int? jumpValue = null;

            long valueX = GetValue(registers, reg, X);

            long valueY = GetValue(registers, reg, Y);

            if (valueX > 0)
                jumpValue = (int)valueY;

            return jumpValue;
        }


        private static long GetValue(Dictionary<string, int> registers, List<long> reg, string input)
        {
            long value;
            if (!long.TryParse(input, out value))
            {
                int index = registers[input];
                value = reg[index];
            }
            return value;
        }


    }
}
