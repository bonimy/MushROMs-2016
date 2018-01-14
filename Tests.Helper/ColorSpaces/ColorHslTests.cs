using System;
using Helper.ColorSpaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper.ColorSpaces
{
    [TestClass]
    public class ColorHslTests
    {
        [TestMethod]
        public void ColorHslTestConstructors()
        {
            float a = 0, h = 0.25f, s = 0.5f, l = 0.75f;

            // test color properly assigns values
            var color = new ColorHsl(a, h, s, l);
            Assert.AreEqual(color.Alpha, a);
            Assert.AreEqual(color.Hue, h);
            Assert.AreEqual(color.Saturation, s);
            Assert.AreEqual(color.Lightness, l);
            Assert.AreEqual(color, new ColorHsl(a, h, s, l));

            // Test color properly assigns values with alpha assumed
            color = new ColorHsl(h, s, l);
            Assert.AreEqual(color, new ColorHsl(1, h, s, l));

            // test color clamps out of bounds values
            color = new ColorHsl(-1, 2, l);
            Assert.AreEqual(color, new ColorHsl(0, 1, l));

            // Test color clamps extremes and snaps near to 0 or 1
            color = new ColorHsl(Single.PositiveInfinity, Single.Epsilon, Single.MinValue, 1 - 1e-8f);
            Assert.AreEqual(color, new ColorHsl(1, 0, 0, 1));

            // Ensure NaN values are not accepted and throw the proper message
            var ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsl(Single.NaN, 0, 0, 0));
            Assert.AreEqual(ex.ParamName, "alpha");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsl(0, Single.NaN, 0, 0));
            Assert.AreEqual(ex.ParamName, "hue");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsl(0, 0, Single.NaN, 0));
            Assert.AreEqual(ex.ParamName, "saturation");

            ex = Assert.ThrowsException<ArgumentException>(
                    () => new ColorHsl(0, 0, 0, Single.NaN));
            Assert.AreEqual(ex.ParamName, "lightness");
        }
    }
}
