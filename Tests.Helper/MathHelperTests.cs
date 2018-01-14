using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class MathHelperTests
    {
        [TestMethod]
        [Timeout(2000)]
        public void NearlyEquals_Tests()
        {
            AssertNearlyEquals(0, 0, 0, true);
            AssertNearlyEquals(0, 0, Single.Epsilon, true);
            AssertNearlyEquals(0, 0, -Single.Epsilon, false);

            AssertNearlyEquals(0, 1e-7f, 1e-6f, true);
            AssertNearlyEquals(0, 1e-7f, 1e-7f, true);
            AssertNearlyEquals(0, 1e-7f, 1e-8f, false);

            AssertNearlyEquals(1e-7f, 1e-7f + 1e-14f, 1e-13f, true);
            AssertNearlyEquals(1e-7f, 1e-7f + 1e-14f, 1e-14f, true);
            AssertNearlyEquals(1e-7f, 1e-7f + 1e-14f, 1e-15f, false);

            AssertNearlyEquals(0, Single.MaxValue, Single.MaxValue, true);
            AssertNearlyEquals(Single.MinValue, Single.MaxValue, Single.MaxValue - Single.MinValue, true);
            AssertNearlyEquals(Single.MinValue, Single.MaxValue, Single.PositiveInfinity, true);

            AssertNearlyEquals(0, Single.PositiveInfinity, Single.MaxValue, false);
            AssertNearlyEquals(Single.NegativeInfinity, Single.PositiveInfinity, Single.PositiveInfinity, true);

            AssertNearlyEquals(0, 0, Single.NaN, false);
            AssertNearlyEquals(0, Single.NaN, Single.PositiveInfinity, false);
            AssertNearlyEquals(0, Single.NaN, Single.NegativeInfinity, false);
            AssertNearlyEquals(Single.NaN, Single.NaN, 0, false);
            AssertNearlyEquals(Single.NaN, Single.NaN, Single.PositiveInfinity, false);
            AssertNearlyEquals(Single.NaN, Single.NaN, Single.NaN, false);
        }

        private void AssertNearlyEquals(float left, float right, float tolerance, bool result)
        {
            Assert.AreEqual(MathHelper.NearlyEquals(left, right, tolerance), result);
        }

        [TestMethod]
        [Timeout(2000)]
        public void SnapToLimit_Boundaries()
        {
            AssertSnapToLimit(0, 0, 0, 0);
            AssertSnapToLimit(0, 1, 1, 1);
            AssertSnapToLimit(1, 0, 1, 0);
            AssertSnapToLimit(0, 0, -1, 0);
            AssertSnapToLimit(0, 1, 0.5f, 0);

            AssertSnapToLimit(0, Single.MaxValue, Single.MaxValue, Single.MaxValue);
            AssertSnapToLimit(Single.MinValue, 0, Single.MaxValue, 0);
            AssertSnapToLimit(Single.MinValue, Single.MaxValue, Single.MaxValue, Single.MinValue);
            AssertSnapToLimit(Single.MinValue, Single.MaxValue, Single.PositiveInfinity, Single.MaxValue);
            AssertSnapToLimit(Single.MaxValue, Single.MinValue, Single.PositiveInfinity, Single.MinValue);

            AssertSnapToLimit(0, Single.PositiveInfinity, Single.PositiveInfinity, Single.PositiveInfinity);
            AssertSnapToLimit(Single.NegativeInfinity, Single.PositiveInfinity, Single.PositiveInfinity, Single.PositiveInfinity);
            AssertSnapToLimit(Single.PositiveInfinity, Single.NegativeInfinity, Single.PositiveInfinity, Single.NegativeInfinity);

            AssertSnapToLimit(0, 0, Single.NaN, 0);
            AssertSnapToLimit(0, 1, Single.NaN, 0);
            AssertSnapToLimit(0, Single.NaN, Single.PositiveInfinity, 0);
            AssertSnapToLimit(Single.NaN, 0, Single.PositiveInfinity, Single.NaN);
            AssertSnapToLimit(Single.NaN, Single.NaN, Single.NaN, Single.NaN);
        }

        private void AssertSnapToLimit(float value, float limit, float tolerance, float result)
        {
            if (Single.IsNaN(result))
            {
                Assert.IsTrue(Single.IsNaN(MathHelper.SnapToLimit(value, limit, tolerance)));
            }
            else
            {
                Assert.AreEqual(MathHelper.SnapToLimit(value, limit, tolerance), result);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Clamp_Boundaries()
        {
            AssertClamp(0, 0, 0, 0, 0);
            AssertClamp(0, 0, 1, 0, 0);
            AssertClamp(-1, 0, 1, 0, 0);
            AssertClamp(2, 0, 1, 0, 1);
            AssertClamp(0.5f, 0, 1, 0, 0.5f);

            AssertClamp(0, 0, 1, 1, 0.5f);

            AssertClamp(Single.MinValue, 0, 1, 0, 0);
            AssertClamp(Single.MaxValue, 0, 1, 0, 1);
            AssertClamp(Single.PositiveInfinity, 0, 1, 0, 1);
            AssertClamp(Single.NegativeInfinity, 0, 1, 0, 0);

            AssertClamp(Single.Epsilon, 0, 1, 0, Single.Epsilon);
            AssertClamp(Single.Epsilon, 0, 1, Single.Epsilon, 0);
            AssertClamp(-Single.Epsilon, 0, 1, 0, 0);

            AssertClamp(0, 1, 0, 0, Single.NaN);
            AssertClamp(Single.NaN, 0, 1, 0, Single.NaN);
            AssertClamp(Single.NaN, Single.MinValue, Single.MaxValue, 0, Single.NaN);
            AssertClamp(Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity, 0, Single.NaN);
            AssertClamp(Single.NaN, Single.NegativeInfinity, Single.PositiveInfinity, Single.PositiveInfinity, Single.NaN);
            AssertClamp(Single.NaN, 0, 1, Single.PositiveInfinity, Single.NaN);
            AssertClamp(Single.NaN, Single.NaN, Single.NaN, Single.NaN, Single.NaN);
        }

        private void AssertClamp(float value, float min, float max, float tolerance, float result)
        {
            if (Single.IsNaN(result))
            {
                Assert.IsTrue(Single.IsNaN(MathHelper.Clamp(value, min, max, tolerance)));
            }
            else
            {
                Assert.AreEqual(MathHelper.Clamp(value, min, max, tolerance), result);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Max_Tests()
        {
            AssertMax(0, 0, 0);
            AssertMax(1, 0, 1);
            AssertMax(0, 0, 0, 0, 0, 0);
            AssertMax(0, -1, -2, -3, -4, -5, -6, 0, -3, -2, -1);
            AssertMax(3, 0, 3, null);
            AssertMax(-2, -2, -8, new float[] { });
            AssertMax(7, 1, 7, new float[] { 3, 7, 6 });

            AssertMax(Single.NaN, 0, Single.NaN);
            AssertMax(Single.NaN, Single.PositiveInfinity, Single.NaN);
            AssertMax(Single.NaN, Single.NegativeInfinity, Single.NaN);
            AssertMax(Single.NaN, Single.NaN, Single.NaN, Single.NaN, Single.NaN);

            AssertMax(7, new float[] { 0, 6, 7, 2, 3, 1, 4, -9 });
            AssertMax(1, new float[] { 1 });

            try
            {
                var nan = MathHelper.Max(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith(SR.ErrorEmptyOrNullArray("values")));
            }

            try
            {
                var nan = MathHelper.Max(new float[] { });
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith(SR.ErrorEmptyOrNullArray("values")));
            }
        }

        private void AssertMax(float result, float left, float right, params float[] values)
        {
            if (Single.IsNaN(result))
            {
                Assert.IsTrue(Single.IsNaN(MathHelper.Max(left, right, values)));
            }
            else
            {
                Assert.AreEqual(result, MathHelper.Max(left, right, values));
            }
        }

        private void AssertMax(float result, float[] values)
        {
            if (Single.IsNaN(result))
            {
                Assert.IsTrue(Single.IsNaN(MathHelper.Max(values)));
            }
            else
            {
                Assert.AreEqual(result, MathHelper.Max(values));
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Min_Tests()
        {
            AssertMin(0, 0, 0);
            AssertMin(0, 0, 1);
            AssertMin(0, 0, 0, 0, 0, 0);
            AssertMin(-6, -1, -2, -3, -4, -5, -6, 0, -3, -2, -1);
            AssertMin(0, 0, 3, null);
            AssertMin(-8, -2, -8, new float[] { });
            AssertMin(1, 1, 7, new float[] { 3, 7, 6 });

            AssertMin(Single.NaN, 0, Single.NaN);
            AssertMin(Single.NaN, Single.PositiveInfinity, Single.NaN);
            AssertMin(Single.NaN, Single.NegativeInfinity, Single.NaN);
            AssertMin(Single.NaN, Single.NaN, Single.NaN, Single.NaN, Single.NaN);

            AssertMin(-9, new float[] { 0, 6, 7, 2, 3, 1, 4, -9 });
            AssertMin(1, new float[] { 1 });

            try
            {
                var nan = MathHelper.Min(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith(SR.ErrorEmptyOrNullArray("values")));
            }

            try
            {
                var nan = MathHelper.Min(new float[] { });
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith(SR.ErrorEmptyOrNullArray("values")));
            }
        }

        private void AssertMin(float result, float left, float right, params float[] values)
        {
            if (Single.IsNaN(result))
            {
                Assert.IsTrue(Single.IsNaN(MathHelper.Min(left, right, values)));
            }
            else
            {
                Assert.AreEqual(result, MathHelper.Min(left, right, values));
            }
        }

        private void AssertMin(float result, float[] values)
        {
            if (Single.IsNaN(result))
            {
                Assert.IsTrue(Single.IsNaN(MathHelper.Min(values)));
            }
            else
            {
                Assert.AreEqual(result, MathHelper.Min(values));
            }
        }
    }
}
