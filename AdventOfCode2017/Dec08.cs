using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2017
{
    public static class Dec08
    {
        private enum Operator { increase, descrease };
        private enum Condition { LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual, Equal, NotEqual };

        public static void Run(string path)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 8th - I Heard You Like Registers -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path, "dec08test.txt"), 1);
            Part1(Path.Combine(path, "dec08.txt"),3880);

            Console.WriteLine();
            Console.WriteLine("Part2");
            Part2(Path.Combine(path, "dec08test.txt"), 10);
            Part2(Path.Combine(path, "dec08.txt"), 5035);
        }

        /// <summary>
        /// Compute instructions and find largest register at end. 
        /// </summary>
        public static Result Part1(string filename, int? expected = null)
        {
            //load
            var strings = Utilities.LoadStrings(filename);

            //tokanise 
            List<Instruction> instructions = new List<Instruction>();

            foreach(string line in strings)
            {
                Instruction instruction = new Instruction();
                instruction.Initialise(line);
                instructions.Add(instruction);
            }

            //generate possible registers
            Dictionary<string, int> registers = new Dictionary<string, int>();
            
            foreach(var instruction in instructions)
            {
                if (!registers.ContainsKey(instruction.registerA))
                    registers.Add(instruction.registerA, 0);

                if (!registers.ContainsKey(instruction.registerB))
                    registers.Add(instruction.registerB, 0);
            }

            //Calculate

            //b inc 5 if a > 1
            //a inc 1 if b < 5
            //c dec -10 if a >= 1
            //c inc -20 if c == 10
            foreach (var instruction in instructions)
            {
                if(EvaluateCondtion(registers[instruction.registerB], instruction.condition, instruction.conditionValue))
                {
                    registers[instruction.registerA] += EvaluateOperation(instruction.opp, instruction.oppValue);
                }

            }

            //report largest
            int largestRegister = int.MinValue;

            foreach (var pair in registers )
            {
                int regvalue = pair.Value;
                if (regvalue > largestRegister)
                    largestRegister = regvalue;
            }
            

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(largestRegister, expected);
        }




        /// <summary>
        /// Compute instructions and find largest register ever set. 
        /// </summary>
        public static Result Part2(string filename, int? expected = null)
        {
            //largest register value
            int largestRegister = int.MinValue;

            //load
            var strings = Utilities.LoadStrings(filename);

            //tokanise 
            List<Instruction> instructions = new List<Instruction>();

            foreach (string line in strings)
            {
                Instruction instruction = new Instruction();
                instruction.Initialise(line);
                instructions.Add(instruction);
            }

            //generate possible registers
            Dictionary<string, int> registers = new Dictionary<string, int>();

            foreach (var instruction in instructions)
            {
                if (!registers.ContainsKey(instruction.registerA))
                    registers.Add(instruction.registerA, 0);

                if (!registers.ContainsKey(instruction.registerB))
                    registers.Add(instruction.registerB, 0);
            }


            //Calculate

            //b inc 5 if a > 1
            //a inc 1 if b < 5
            //c dec -10 if a >= 1
            //c inc -20 if c == 10
            foreach (var instruction in instructions)
            {
                if (EvaluateCondtion(registers[instruction.registerB], instruction.condition, instruction.conditionValue))
                {
                    registers[instruction.registerA] += EvaluateOperation(instruction.opp, instruction.oppValue);
                }

                int regvalue = registers[instruction.registerA];
                if (regvalue > largestRegister)
                    largestRegister = regvalue;
            }


            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(largestRegister, expected);
        }

        private static bool EvaluateCondtion(int registervalue, Condition condition, int conditionValue)
        {
            switch (condition)
            {
                case Condition.LessThan:
                    return registervalue < conditionValue;
                case Condition.GreaterThan:
                    return registervalue > conditionValue;
                case Condition.LessThanOrEqual:
                    return registervalue <= conditionValue;
                case Condition.GreaterThanOrEqual:
                    return registervalue >= conditionValue;
                case Condition.Equal:
                    return registervalue == conditionValue;
                case Condition.NotEqual:
                    return registervalue != conditionValue;
                default:
                    throw new Exception("unidentified condition when evaluating: " + Instruction.GetConditionString(condition));
            }
        }

        private static int EvaluateOperation(Operator opp, int modificationValue)
        {
            switch (opp)
            {
                case Operator.increase:
                    return  modificationValue;
                case Operator.descrease:
                    return -modificationValue;
                default:
                    throw new Exception("unidentified condition when evaluating: " + Instruction.GetOpperatorString(opp));
            }
        }


        [DebuggerDisplay("Instruction registerA = {registerA}, opp = {oppStr}, modificationValue = {modificationValue}, " +
            "registerB = {registerB}, condition = {conditionStr}, conditionValue = {conditionValue}")]
        private class Instruction
        {
            public string registerA;
            public Operator opp;
            public string oppStr;
            public int oppValue;

            public string registerB;
            public Condition condition;
            public string conditionStr;
            public int conditionValue;


            /// <summary>
            /// Parse instruciton line into fields from into fields
            /// </summary>
            /// <example>
            /// "b inc 5 if a > 1"
            /// </example>
            internal void Initialise(string line)
            {
                //b inc 5 if a > 1
                //c dec -10 if a >= 1

                var split = line.Split();

                registerA = split[0];
                opp = GetOpperator(split[1]);
                oppStr = split[1];
                oppValue = int.Parse(split[2]);
                //if
                registerB = split[4];
                condition = GetCondition(split[5]);
                conditionStr = split[5];
                conditionValue = int.Parse(split[6]);
            }


            public static Operator GetOpperator(string opp)
            {
                switch (opp)
                {
                    case "inc":
                        return Operator.increase;
                    case "dec":
                        return Operator.descrease;
                    default:
                        throw new Exception("unidentified opperator: " + opp);
                }
            }

            public static string GetOpperatorString(Operator opp)
            {
                switch (opp)
                {
                    case Operator.increase:
                        return "inc";
                    case Operator.descrease:
                        return "dec";
                    default:
                        throw new Exception("unidentified opperator: " + opp);
                }
            }

            public static Condition GetCondition(string condition)
            {
                switch (condition)
                {
                    case "<":
                        return Condition.LessThan;
                    case ">":
                        return Condition.GreaterThan;
                    case "<=":
                        return Condition.LessThanOrEqual;
                    case ">=":
                        return Condition.GreaterThanOrEqual;
                    case "==":
                        return Condition.Equal;
                    case "!=":
                        return Condition.NotEqual;
                    default:
                        throw new Exception("unidentified condition: " + condition);
                }
            }

            public static string GetConditionString(Condition condition)
            {
                switch (condition)
                {
                    case Condition.LessThan:
                        return "<";
                    case Condition.GreaterThan:
                        return ">";
                    case Condition.LessThanOrEqual:
                        return "<=";
                    case Condition.GreaterThanOrEqual:
                        return ">=";
                    case Condition.Equal:
                        return "==";
                    case Condition.NotEqual:
                        return "!=";
                    default:
                        throw new Exception("unidentified condition: " + condition);
                }
            }

        }
    }
}