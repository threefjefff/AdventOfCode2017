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
            public Node Parent { get; set; } 
            public IDictionary<string, Node> Children = new Dictionary<string, Node>();

            public Node() { }
            public Node(string input)
            {
                var matches = Regex.Matches(input, @"(?:(\w+))");
                Name = matches[0].Value;
                Weight = int.Parse(matches[1].Value);
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
            Console.WriteLine(FindUnbalancedNode(root).Name);
            Console.Read();
        }

        static Node FindUnbalancedNode(Node root)
        {
            foreach (var childNode in root.Children.Values)
            {
                if (IsUnbalanced(childNode))
                {
                    return childNode;
                }
            }
            return root;
        }

        static bool IsUnbalanced(Node toTest)
        {
            if (toTest.Children.Count == 0)
            {
                return false;
            }

            var children = toTest.Children.Values;
            var firstChild = children.First();
            return children.Any(x => x.Weight != firstChild.Weight);
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
