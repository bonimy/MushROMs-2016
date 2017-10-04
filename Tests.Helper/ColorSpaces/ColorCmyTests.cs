using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper.ColorSpaces;

namespace Tests.Helper.ColorSpaces
{
    [TestClass]
    public class ColorCmyTests
    {
        [TestMethod]
        public void ColorCmyTestConstructors()
        {
            float a = 0, c = 0.25f, m = 0.5f, y = 0.75f;

            // test color properly assigns values
            var color = new ColorCmy(a, c, m, y);
            Assert.AreEqual(color.Alpha, a);
            Assert.AreEqual(color.Cyan, c);
            Assert.AreEqual(color.Magenta, m);
            Assert.AreEqual(color.Yellow, y);
            Assert.AreEqual(color, new ColorCmy(a, c, m, y));

            // Test color properly assigns values with alpha assumed
            color = new ColorCmy(c, m, y);
            Assert.AreEqual(color, new ColorCmy(1, c, m, y));

            // test color clamps out of bounds values
            color = new ColorCmy(-1, 2, y);
            Assert.AreEqual(color, new ColorCmy(0, 1, y));

            // Test color clamps extremes and snaps near to 0 or 1
            color = new ColorCmy(Single.PositiveInfinity, Single.Epsilon, Single.MinValue, 1-1e-8f);
            Assert.AreEqual(color, new ColorCmy(1, 0, 0, 1));

            // Ensure NaN values are not accepted and throw the proper message
            var ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmy(Single.NaN, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "alpha");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmy(0, Single.NaN, 0, 0));
            Assert.AreEqual(ex.ParamName, "cyan");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmy(0, 0, Single.NaN, 0));
            Assert.AreEqual(ex.ParamName, "magenta");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorCmy(0, 0, 0, Single.NaN));
            Assert.AreEqual(ex.ParamName, "yellow");
        }
    }
}
