using System;
using Helper.ColorSpaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper.ColorSpaces
{
    [TestClass]
    public class ColorCmykTests
    {
        [TestMethod]
        public void ColorCmykTestConstructors()
        {
            float a = 0, c = 0.25f, m = 0.5f, y = 0.75f, k = 1;

            // test color properly assigns values
            var color = new ColorCmyk(a, c, m, y, k);
            Assert.AreEqual(color.Alpha, a);
            Assert.AreEqual(color.Cyan, c);
            Assert.AreEqual(color.Magenta, m);
            Assert.AreEqual(color.Yellow, y);
            Assert.AreEqual(color.Key, k);
            Assert.AreEqual(color, new ColorCmyk(a, c, m, y, k));

            // Test color properly assigns values with alpha assumed
            color = new ColorCmyk(c, m, y, k);
            Assert.AreEqual(color, new ColorCmyk(1, c, m, y, k));

            // test color clamps out of bounds values
            color = new ColorCmyk(-1, 2, y, k);
            Assert.AreEqual(color, new ColorCmyk(0, 1, y, k));

            // Test color clamps extremes and snaps near to 0 or 1
            color = new ColorCmyk(Single.PositiveInfinity, Single.Epsilon, Single.MinValue, 1 - 1e-8f);
            Assert.AreEqual(color, new ColorCmyk(1, 1, 0, 0, 1));

            // Ensure NaN values are not accepted and throw the proper message
            var ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmyk(Single.NaN, 0, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "alpha");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmyk(0, Single.NaN, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "cyan");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmyk(0, 0, Single.NaN, 0, 0));
            Assert.AreEqual(ex.ParamName, "magenta");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmyk(0, 0, 0, Single.NaN, 0));
            Assert.AreEqual(ex.ParamName, "yellow");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmyk(0, 0, 0, 0, Single.NaN));
            Assert.AreEqual(ex.ParamName, "key");
        }
    }
}
