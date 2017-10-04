using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public class FileFilter
    {
        public const char WildCard = '*';
        public const char FilterSeparator = '|';
        public const char ExtensionSeparator = ';';
        public const string AnyExtension = ".*";

        public string DisplayName
        {
            get;
            private set;
        }

        public string Extensions
        {
            get;
            private set;
        }
        public string Filter
        {
            get;
            private set;
        }

        public FileFilter(string displayName, List<string> extensions)
        {
            if (displayName == null)
                throw new ArgumentNullException(nameof(displayName));

            DisplayName = displayName;
            Extensions = GenerateExtensionsList(extensions);
            Filter = GenerateFileFilter(true);
        }

        public string GenerateFileFilter(bool showExtensions)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(DisplayName);
            
            if (showExtensions)
            {
                sb.Append(" (");
                sb.Append(Extensions);
                sb.Append(')');
            }

            sb.Append(FilterSeparator);
            sb.Append(Extensions);

            return sb.ToString();
        }

        public static string GenerateExtensionsList(List<string> extensions)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (string extension in extensions)
            {
                sb.Append(WildCard);
                sb.Append(extension);
                sb.Append(ExtensionSeparator);
            }
            sb.Length -= 1; //Removes the last extension separator.

            return sb.ToString();
        }
    }
}
