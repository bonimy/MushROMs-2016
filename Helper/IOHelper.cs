using System;
using System.Collections.Generic;
using System.IO;

namespace Helper
{
    public static class IOHelper
    {
        public const int MaxPath = 260;
        public const int MaxDirectoryLength = 255;

        public const char ExtensionPredicate = '.';

        public static bool IsValidPathChar(char c)
        {
            return ((IList<char>)Path.GetInvalidPathChars()).Contains(c);
        }

        public static bool IsValidPathName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            for (int i = name.Length; --i >= 0;)
                if (!IsValidPathChar(name[i]))
                    return false;
            return true;
        }

        public static bool IsValidFileNameChar(char c)
        {
            return IsValidPathChar(c) && c != ':' && c != '*' && c != '?' && c != '\\' && c != '/';
        }

        public static bool IsValidFileName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            for (int i = name.Length; --i >= 0;)
                if (!IsValidFileNameChar(name[i]))
                    return false;
            return true;
        }

        public static bool IsValidExtension(string extension)
        {
            if (String.IsNullOrEmpty(extension))
                return true;

            if (extension[0] != ExtensionPredicate)
                return false;

            for (int i = extension.Length; --i >= 1;)
            {
                var c = extension[i];
                if (!IsValidFileNameChar(c) || c == ExtensionPredicate)
                    return false;
            }
            return true;
        }

        public static string NormalizePath(string path)
        {
            /*
            path = Path.GetFullPath(path);
            path = new Uri(path).LocalPath;
            path = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return path;
            */
            return Path.GetFullPath(path);
        }

        public static int ComparePaths(string path1, string path2)
        {
            var normalizedPath1 = NormalizePath(path1);
            var normalizedPath2 = NormalizePath(path2);

            return String.Compare(normalizedPath1, normalizedPath2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool AreEqualPaths(string path1, string path2)
        {
            return ComparePaths(path1, path2) == 0;
        }

        public static int CompareExtensions(string path1, string path2)
        {
            var ext1 = Path.GetExtension(path1);
            var ext2 = Path.GetExtension(path2);

            return String.Compare(ext1, ext2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool HaveEqualExtensions(string path1, string path2)
        {
            return CompareExtensions(path1, path2) == 0;
        }
    }
}