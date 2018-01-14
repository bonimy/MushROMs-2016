/* The extension comparer simply
 *
 * */

using System;
using System.IO;

namespace Helper
{
    /// <summary>
    /// Represents a <see cref="String"/> comparison operation that performs a comparison of extensions
    /// of path-based <see cref="String"/>s.
    /// </summary>
    public class ExtensionComparer : StringComparer
    {
        private StringComparer Comparer
        {
            get;
            set;
        }

        public ExtensionComparer()
        {
            Comparer = OrdinalIgnoreCase;
        }

        public override int Compare(string x, string y)
        {
            return IOHelper.CompareExtensions(x, y);
        }

        public override bool Equals(string x, string y)
        {
            return Compare(x, y) == 0;
        }

        public override int GetHashCode(string obj)
        {
            var ext = Path.GetExtension(obj);
            return Comparer.GetHashCode(ext);
        }
    }
}
