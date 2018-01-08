using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace AdventOfCode2017
{
    
    public static class Dec07
    {
        public static void Run(string path)
        {
            Console.WriteLine(Utilities.LineSeparator);
            Console.WriteLine("December 7th - Recursive Circus -");
            Console.WriteLine("Part1");

            Part1(Path.Combine(path, "dec07test.txt"), "tknk");
            Part1(Path.Combine(path, "dec07.txt"), "vmpywg");

            Console.WriteLine();
            Console.WriteLine("Part2");

            Part2(Path.Combine(path, "dec07test.txt"), 60);
            Part2(Path.Combine(path, "dec07.txt"), 1674);
        }

        /// <summary>
        /// Find the root element of a tree.
        /// </summary>
        public static Result Part1(string filename, string expected = null)
        {
            /*
            pbga (66)
            xhth (57)
            ebii (61)
            havc (66)
            ktlj (57)
            fwft (72) -> ktlj, cntj, xhth
            qoyq (66)
            padx (45) -> pbga, havc, qoyq
            tknk (41) -> ugml, padx, fwft
            jptl (61)
            ugml (68) -> gyxo, ebii, jptl
            gyxo (61)
            cntj (57)

                            gyxo
                          /     
                     ugml - ebii
                   /      \     
                  |         jptl
                  |        
                  |         pbga
                 /        /
            tknk --- padx - havc
                 \        \
                  |         qoyq
                  |             
                  |         ktlj
                   \      /     
                     fwft - cntj
                          \     
                            xhth

            */

            //load input and create nodes
            var input = Utilities.LoadStrings(filename);
            Dictionary<string,Node> nodes = new Dictionary<string, Node>();

            foreach (var line in input)
            {
                Node node = new Node();
                node.Set(line);
                nodes.Add(node.name, node);
            }

            //connect children to parents.
            foreach(var pair in nodes)
            {
                var node = pair.Value;
                foreach (string childName in node.childNames)
                {
                    nodes[childName].parents.Add(node);
                }
            }

            //find root node. //assume all are connected. 
            Node current = nodes.First().Value;
            
            while(current.parents.Count >0)
            {
                current = current.parents.First();
            }

            string rootstring = current.name;

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput(rootstring, expected);
        }


        /// <summary>
        /// Exactly one element is the wrong weight. Find the correct weight.
        /// </summary>
        public static Result Part2(string filename, int? expected = null)
        {
            //load input and create nodes
            var input = Utilities.LoadStrings(filename);
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            foreach (var line in input)
            {
                Node node = new Node();
                node.Set(line);
                nodes.Add(node.name, node);
            }

            //connect parents to children
            foreach (var pair in nodes)
            {
                var node = pair.Value;
                foreach (string childName in node.childNames)
                {
                    nodes[childName].parents.Add(node);
                    node.children.Add(nodes[childName]);
                }
            }

            //find root node. 
            Node current = nodes.First().Value;

            while (current.parents.Count > 0)
                current = current.parents.First();

            current.SetChildWeight();


            //find unballanced child. (of the smallest weight
            int? correctweight = null;
            int? minexpected = null;

            foreach (var pair in nodes)
            {
                var node = pair.Value;

                
                int maxweight = int.MinValue;
                int minweight = int.MaxValue;
                foreach(var child in node.children)
                {
                    if (child.totalweight > maxweight)
                        maxweight = child.totalweight;

                    if (child.totalweight < minweight)
                        minweight = child.totalweight;
                }

                int expectedIsLowerCount = 0;
                int expectedIsHigherCount = 0;
                foreach (var child in node.children)
                {
                    if (child.totalweight == maxweight)
                        expectedIsHigherCount++;

                    if (child.totalweight == minweight)
                        expectedIsLowerCount++;
                }

                int expectedweight = minweight;

                if (expectedIsHigherCount > expectedIsLowerCount)
                    expectedweight = maxweight;

                //find 
                foreach (var child in node.children)
                {
                    if (child.totalweight != expectedweight)
                    {
                        int thisweight = 0;
                        if (expectedIsLowerCount > expectedIsHigherCount)
                            thisweight = child.value - ( child.totalweight - expectedweight);
                        else
                            thisweight = child.value + ( child.totalweight - expectedweight);

                        if (minexpected == null)
                        {
                            minexpected = expectedweight;
                            correctweight = thisweight;
                        }
                        else if (expectedweight< minexpected)
                        {
                            minexpected = expectedweight;
                            correctweight = thisweight;
                        }

                        break;
                    }
                }
            }

            Utilities.WriteInputFile(filename);
            return Utilities.WriteOutput((int)correctweight, expected);
        }


        [DebuggerDisplay("Node name = {name}, value = {value},  childWeight = {childWeight}, totalweight = {totalweight}, childcount = {children.Count}")]
        private class Node
        {
            public string name = "";
            public int value = 0;
            public int totalweight;
            public int childWeight;
            private bool set = false;

            public List<string> childNames = new List<string>();

            public List<Node> parents = new List<Node>();
            public List<Node> children = new List<Node>();

            internal void Set(string line)
            {
                //remve braces and commas
                string trimedline = Regex.Replace(line, "[()]*,*", "", RegexOptions.CultureInvariant);
                //remove ->
                trimedline = Regex.Replace(trimedline, @"\s*->\s*", " ", RegexOptions.CultureInvariant);

                //split into name and value, and children
                //get name and value
                List<string> split = new List<string>(trimedline.Split(" "));
                name = split[0];
                value = int.Parse(split[1]);

                //get child names
                if (split.Count > 2)
                    childNames.AddRange(split.GetRange(2, split.Count - 2));
            }

            /// <summary>
            /// Calculate child weight (and return) the weight of node and children
            /// </summary>
            internal int SetChildWeight()
            {
                if (set)
                    return totalweight;

                totalweight = value;
                childWeight = 0;

                foreach (var child in children)
                {
                    totalweight += child.SetChildWeight();
                    childWeight += child.totalweight;
                }

                return totalweight;
            }

            internal void SetChildDepth(int v)
            {
                throw new NotImplementedException();
            }
        }
    }
}
