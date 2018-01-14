using System;
using Helper.ColorSpaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper.ColorSpaces
{
    [TestClass]
    public class ColorHsvTests
    {
        [TestMethod]
        public void ColorHsvTestConstructors()
        {
            float a = 0, h = 0.25f, s = 0.5f, v = 0.75f;

            // test color properly assigns values
            var color = new ColorHsv(a, h, s, v);
            Assert.AreEqual(color.Alpha, a);
            Assert.AreEqual(color.Hue, h);
            Assert.AreEqual(color.Saturation, s);
            Assert.AreEqual(color.Value, v);
            Assert.AreEqual(color, new ColorHsv(a, h, s, v));

            // Test color properly assigns values with alpha assumed
            color = new ColorHsv(h, s, v);
            Assert.AreEqual(color, new ColorHsv(1, h, s, v));

            // test color clamps out of bounds values
            color = new ColorHsv(-1, 2, v);
            Assert.AreEqual(color, new ColorHsv(0, 1, v));

            // Test color clamps extremes and snaps near to 0 or 1
            color = new ColorHsv(Single.PositiveInfinity, Single.Epsilon, Single.MinValue, 1 - 1e-8f);
            Assert.AreEqual(color, new ColorHsv(1, 0, 0, 1));

            // Ensure NaN values are not accepted and throw the proper message
            var ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsv(Single.NaN, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "alpha");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsv(0, Single.NaN, 0, 0));
            Assert.AreEqual(ex.ParamName, "hue");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsv(0, 0, Single.NaN, 0));
            Assert.AreEqual(ex.ParamName, "saturation");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsv(0, 0, 0, Single.NaN));
            Assert.AreEqual(ex.ParamName, "value");
        }
    }
}
