using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2017
{
    public static class Dec25
    {
        public static void Run(string path = null)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 25th - The Halting Problem -");
            Console.WriteLine("Part1");
            Part1(Path.Combine(path,"dec25test.txt"), 3);
            Part1(Path.Combine(path, "dec25.txt"), 3145);

            Console.WriteLine();
            Utilities.WriteColourfultext("Part2 - 2017 Complete! -", ConsoleColor.Green);
            Utilities.WriteColourfultext(" 50 stars deposited,", ConsoleColor.Yellow);
            Utilities.WriteColourfultext(" printing is off to Santa.", ConsoleColor.Red);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Part1(string filename, int? expected = null)
        {
            var blueprints = Utilities.LoadStrings(filename);

            string line = blueprints[0].Split(' ').Last();
            char startstate = line.Substring(0, line.Length - 1)[0];

            int steps = int.Parse(blueprints[1].Split(' ')[5]);

            Dictionary<char, State> states = new Dictionary<char, State>();

            for (int i = 3; i < blueprints.Count; i++)
            {
                char name = blueprints[i++].Split(' ').Last().Replace(":", "")[0];
                List<Action> actions = new List<Action>();

                for (int j = 0; j < 2; j++)
                {
                    int value = int.Parse(blueprints[i++].Split(' ').Last().Replace(":", ""));
                    int write = int.Parse(blueprints[i++].Split(' ').Last().Replace(".", ""));

                    int direction = 1;
                    if (blueprints[i++].Split(' ').Last() == "left.")
                        direction = -1;

                    char nextState = blueprints[i++].Split(' ').Last().Replace(".", "")[0];

                    actions.Add(new Action(value, write, direction, nextState));
                }

                State state = new State(name,actions);
                states.Add(name, state);
            }


            int index = 0;
            List<int> tape = new List<int>();
            tape.Add(0);
 

            State current = states[startstate];

            for(int i = 0; i < steps; i++)
            {
                //modify tape, change index, and move to next state. 
                current = current.Next(states, tape, ref index);

                //expand tape as needed
                if ( index < 0)
                {
                    tape.Insert(0, 0);
                    index = 0;
                }
                else if (index >= tape.Count)
                {
                    tape.Add(0);
                }
            }

            int checksum = 0;
            foreach (var value in tape)
                checksum += value;

            Utilities.WriteOutput(checksum, expected);
        }


        class State
        {
            char name;

            List<Action> actions = new List<Action>();

            public State(char name, List<Action> actions)
            {
                this.name = name;
                this.actions = actions;
            }

            /// <summary>
            /// Modify tape, change index, and move to next state. 
            /// </summary>
            public State Next(Dictionary<char, State> states, List<int> tape, ref int index)
            {
                foreach(Action action in actions)
                {
                    if (action.current == tape[index])
                    {
                        tape[index] = action.newVale;
                        index += action.direction;
                        return states[action.nextstate];
                    }
                }

                throw new Exception("unknown state");
            }

        }

        class Action
        {
            public int current;
            public int newVale;
            public int direction;
            public char nextstate;

            public Action(int current, int set, int direction, char nextstate)
            {
                this.current = current;
                this.newVale = set;
                this.direction = direction;
                this.nextstate = nextstate;
            }
        }


    }
}
 