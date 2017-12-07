namespace Day7
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;

    class Program
    {
        public static Node HEAD => new Node();
        public class Node
        {
            public string Name { get; set; }
            public int Weight { get; set; }
            public int TotalWeight { get; set; }
            public Node Parent { get; set; } 
            public IDictionary<string, Node> Children = new Dictionary<string, Node>();

            public Node() { }
            public Node(string input)
            {
                var matches = Regex.Matches(input, @"(?:(\w+))");
                Name = matches[0].Value;
                Weight = int.Parse(matches[1].Value);
                TotalWeight = Weight;
                Parent = HEAD;
                for (var i = 2; i < matches.Count; i++)
                {
                    Children.Add(matches[i].Value, new Node(this, matches[i].Value));
                }
            }

            public Node(Node parent, string name)
            {
                Name = name;
                Parent = parent;
            }
        }

        private static Dictionary<string, Node> Input
        {
            get
            {
                var fileInput = File.ReadAllLines("input.txt");
                return fileInput
                    .Select(x => new Node(x))
                    .ToDictionary(x => x.Name);
            }
        }

        static void Main(string[] args)
        {
            var root = Treeify();
            Console.WriteLine(root.Name);
            var unbalanced = FindUnbalancedNode(root);
            Console.WriteLine("Unbalanced Node : {0}", unbalanced.Name);
            Console.WriteLine("Should be : {0}", CorrectUnbalancedNode(unbalanced));
            Console.Read();
        }

        static int CorrectUnbalancedNode(Node unbalanced)
        {
            var parent = unbalanced.Parent;
            var goalWeight = parent.Children.Values
                .First(x => x.Name != unbalanced.Name)
                .TotalWeight;
            var childrenWeight = unbalanced.Children.Values
                .Sum(x => x.TotalWeight);
            return goalWeight - childrenWeight;
        }

        static Node FindUnbalancedNode(Node root)
        {
            Node unbalanced = null;
            foreach (var childNode in root.Children.Values)
            {
                var output = FindUnbalancedNode(childNode);
                if (output.Name != childNode.Name)
                {
                    unbalanced = output;
                    break;
                }
                unbalanced = GetUnbalancedChild(output);
                if (unbalanced != null)
                {
                    break;
                }
            }
            //unbalanced = unbalanced ?? GetUnbalancedChild(root);
            root.TotalWeight += root.Children.Count == 0 ? 0 : root.Children.Values.Sum(x => x.TotalWeight);
            return unbalanced ?? root;
        }

        static Node GetUnbalancedChild(Node toTest)
        {
            if (toTest.Children.Count == 0)
            {
                return null;
            }

            var children = toTest.Children.Values;
            var firstChild = children.First();
            var differentChildren = children
                .Where(x => x.TotalWeight != firstChild.TotalWeight)
                .ToList();
            var diffChildrenCount = differentChildren.Count;
            if (diffChildrenCount > 1)
            {
                return firstChild;
            }
            if (diffChildrenCount == 1)
            {
                return differentChildren.First();
            }
            return null;
        }

        static Node Treeify()
        {
            var input = Input;
            var root = HEAD;
            while (input.Count > 1)
            {
                var preRehome = input.ToList();
                var toRehome = root.Name == null ? preRehome[0].Value : preRehome[1].Value;
                if (preRehome.Any(t => RehomeOrphan(t.Value, toRehome)))
                {
                    input.Remove(toRehome.Name);
                }
                else
                {
                    root = toRehome;
                }
            }

            return input.First().Value;
        }

        public static bool RehomeOrphan(Node potential, Node orphan)
        {
            if (potential.Name == orphan.Name) return false;
            if (!potential.Children.ContainsKey(orphan.Name))
                return potential.Children.Any(child => RehomeOrphan(child.Value, orphan));

            orphan.Parent = potential;
            potential.Children[orphan.Name] = orphan;
            return true;
        }
    }
}
