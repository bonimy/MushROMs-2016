using System;

namespace Helper
{
    public class PathComparer : StringComparer
    {
        private StringComparer Comparer
        {
            get;
            set;
        }

        public PathComparer()
        {
            Comparer = OrdinalIgnoreCase;
        }

        public override int Compare(string x, string y)
        {
            return IOHelper.ComparePaths(x, y);
        }

        public override bool Equals(string x, string y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(string obj)
        {
            var path = IOHelper.NormalizePath(obj);
            return Comparer.GetHashCode(path);
        }
    }
}