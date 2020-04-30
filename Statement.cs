namespace Endowdly.Utility
{
    using System;
    using System.Linq;

    internal sealed class Statement 
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