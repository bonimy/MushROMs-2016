using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;
using Helper.ColorSpaces;

namespace Tests.Helper.ColorSpaces
{
    [TestClass]
    public class ColorRgbTests
    {
        [TestMethod]
        public void ColorRgbTestConstructors()
        {
            float a = 0, r = 0.25f, g = 0.5f, b = 0.75f;

            // test color properly assigns values
            var color = new ColorRgb(a, r, g, b);
            Assert.AreEqual(color.Alpha, a);
            Assert.AreEqual(color.Red, r);
            Assert.AreEqual(color.Green, g);
            Assert.AreEqual(color.Blue, b);
            Assert.AreEqual(color, new ColorRgb(a, r, g, b));

            // Test color properly assigns values with alpha assumed
            color = new ColorRgb(r, g, b);
            Assert.AreEqual(color, new ColorRgb(1, r, g, b));

            // test color clamps out of bounds values
            color = new ColorRgb(-1, 2, b);
            Assert.AreEqual(color, new ColorRgb(0, 1, b));

            // Test color clamps extremes and snaps near to 0 or 1
            color = new ColorRgb(Single.PositiveInfinity, Single.Epsilon, Single.MinValue, 1-1e-8f);
            Assert.AreEqual(color, new ColorRgb(1, 0, 0, 1));

            // Ensure NaN values are not accepted and throw the proper message
            var ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorRgb(Single.NaN, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "alpha");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorRgb(0, Single.NaN, 0, 0));
            Assert.AreEqual(ex.ParamName, "red");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorRgb(0, 0, Single.NaN, 0));
            Assert.AreEqual(ex.ParamName, "green");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorRgb(0, 0, 0, Single.NaN));
            Assert.AreEqual(ex.ParamName, "blue");
        }
    }
}
