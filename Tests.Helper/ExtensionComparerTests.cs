using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class ExtensionComparerTests
    {
        [TestMethod]
        public void CompareExtensionTests()
        {
            string[] left =
            {
                ".exe",
                ".BIN",
                ".Txt",
                ".png",
                "Document.doc",
                "No extension"
            };
            string[] right =
            {
                ".exe",
                ".bin",
                ".TXT",
                "Image.png",
                @"C:\path\to\..\file.doc",
                "Any text here is fine without a period"
            };

            Assert.AreEqual(left.Length, right.Length, "Unequal number of test cases");

            var comparer = new ExtensionComparer();
            for (var i = 0; i < left.Length; i++)
            {
                Assert.AreEqual(comparer.Compare(left[i], right[i]), 0, left[i] + "!=" + right[i]);
                Assert.AreEqual(comparer.GetHashCode(left[i]), comparer.GetHashCode(right[i]), "Hash: " + left[i] + "!=" + right[i]);
            }
        }
    }
}
