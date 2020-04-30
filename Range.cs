namespace Endowdly.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Range : IEnumerable<int>
    {
        private readonly int start;

        private readonly int step;

        private readonly int stop;

        private Range(int from, int to, int n)
        {
            start = from;
            stop = to;
            step = n;
        }

        private Range(int from, int to)
        {
            start = from;
            stop = to;
            step = from < to ? 1 : -1;
        }

        private Range(int n)
        {
            start = stop = n;
            step = 1;
        }

        public static Range From(int n) => new Range(n);

        public Range By(int n) => new Range(start, stop, n);

        public void Do(Action<int> f)
        {
            foreach (var n in this) f(n);
        }

        public IEnumerator<int> GetEnumerator()
        {
            var min = Math.Min(start, stop);
            var max = Math.Max(start, stop);

            for (var n = start; n >= min && n <= max; n += step) yield return n;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Range To(int n) => new Range(start, n);

        public override string ToString() => string.Format("[{0} .. {1} : {2}]", start, stop, step);

        public IEnumerable Where(Func<int, bool> f)
        {
            foreach (var n in this)
            {
                if (f(n)) yield return n;
            }
        }
    }
}