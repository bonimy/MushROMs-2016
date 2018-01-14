using System;
using Helper.ColorSpaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper.ColorSpaces
{
    [TestClass]
    public class ColorHcyTests
    {
        [TestMethod]
        public void ColorHcyTestConstructors()
        {
            float a = 0, h = 0.25f, c = 0.5f, y = 0.75f;

            // test color properly assigns values
            var color = new ColorHcy(a, h, c, y);
            Assert.AreEqual(color.Alpha, a);
            Assert.AreEqual(color.Hue, h);
            Assert.AreEqual(color.Chroma, c);
            Assert.AreEqual(color.Luma, y);
            Assert.AreEqual(color, new ColorHcy(a, h, c, y));

            // Test color properly assigns values with alpha assumed
            color = new ColorHcy(h, c, y);
            Assert.AreEqual(color, new ColorHcy(1, h, c, y));

            // test color clamps out of bounds values
            color = new ColorHcy(-1, 2, y);
            Assert.AreEqual(color, new ColorHcy(0, 1, y));

            // Test color clamps extremes and snaps near to 0 or 1
            color = new ColorHcy(Single.PositiveInfinity, Single.Epsilon, Single.MinValue, 1 - 1e-8f);
            Assert.AreEqual(color, new ColorHcy(1, 0, 0, 1));

            // Ensure NaN values are not accepted and throw the proper message
            var ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHcy(Single.NaN, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "alpha");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHcy(0, Single.NaN, 0, 0));
            Assert.AreEqual(ex.ParamName, "hue");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHcy(0, 0, Single.NaN, 0));
            Assert.AreEqual(ex.ParamName, "chroma");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHcy(0, 0, 0, Single.NaN));
            Assert.AreEqual(ex.ParamName, "luma");
        }
    }
}
