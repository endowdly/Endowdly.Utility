namespace Endowdly.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    public class IndentTree
    {
        private List<IndentTree> children = new List<IndentTree>();

        public IndentTree(int n, string s)
        {
            Level = n;
            Value = s;
        }

        public static IndentTree Empty
        {
            get { return new IndentTree(0, string.Empty); }
        }

        public ReadOnlyCollection<IndentTree> Children
        {
            get { return new ReadOnlyCollection<IndentTree>(children); }
        }

        public int Level { get; private set; }

        public string Value { get; private set; }

        const int rootLevel = -1;

        public static IndentTree Parse(IEnumerable<string> lines)
        { 
            if (lines == null) return Empty;

            var tree = new Stack<IndentTree>();
            var root = new IndentTree(rootLevel, string.Empty);

            Action<IndentTree> pushToTree = branch =>
            {
                tree.Peek().children.Add(branch);
                tree.Push(branch);
            };

            tree.Push(root);

            foreach (var line in lines)
            {
                var lineAsStatement = new Statement(line);
                var level = lineAsStatement.Level;
                var value = lineAsStatement.Value; 
                var branch = new IndentTree(level, value);

                if (level > tree.Peek().Level)
                {
                    pushToTree(branch);
                }
                else
                {
                    while (tree.Peek().Level >= level) tree.Pop();

                    pushToTree(branch);
                }
            }

            root.NormalizeIndentation(rootLevel);

            return root;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            BuildString(builder, string.Empty);

            return builder.ToString();
        }

        private void BuildString(StringBuilder builder, string s)
        {
            const string TwoSpaces = "  ";

            if (Level > -1)
            {
                builder.Append(s);
                builder.AppendLine(Value);

                s += TwoSpaces;
            }

            foreach (var child in Children)
            {
                child.BuildString(builder, s);
            }
        }

        private void NormalizeIndentation(int n)
        {
            Level = n;

            foreach (var child in children) child.NormalizeIndentation(n + 1);
        }
    }

    internal class Statement
    {
        public Statement(string s)
        {
            Level = GetIndentLevel(s);
            Value = s.Trim();
        }

        public int Level { get; }

        public string Value { get; }

        private int GetIndentLevel(string s) => s
            .ToCharArray()
            .TakeWhile(c => char.IsWhiteSpace(c))
            .Count(); 
                
    }
}