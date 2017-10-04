using System;
using System.Text;

namespace Helper.ColorSpaces
{
    public struct ColorWeight
    {
        public static readonly ColorWeight Empty = new ColorWeight();
        public static readonly ColorWeight Balanced = new ColorWeight(1, 1, 1);
        public static readonly ColorWeight Luma = new ColorWeight(ColorHcy.RedWeight, ColorHcy.GreenWeight, ColorHcy.BlueWeight);

        private const float Tolerance = ColorRgb.Tolerance;

        public float Red
        {
            get;
            private set;
        }
        public float Green
        {
            get;
            private set;
        }
        public float Blue
        {
            get;
            private set;
        }

        public ColorWeight(float red, float green, float blue)
        {
            if (Single.IsNaN(red))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(red)), nameof(red));
            if (Single.IsNaN(green))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(green)), nameof(green));
            if (Single.IsNaN(blue))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(blue)), nameof(blue));

            red = MathHelper.SnapToLimit(red, 0, Tolerance);
            green = MathHelper.SnapToLimit(green, 0, Tolerance);
            blue = MathHelper.SnapToLimit(blue, 0, Tolerance);

            if (red < 0)
                throw new ArgumentOutOfRangeException(nameof(red),
                    SR.ErrorInvalidClosedLowerBound(nameof(red), red, 0));
            if (green < 0)
                throw new ArgumentOutOfRangeException(nameof(green),
                    SR.ErrorInvalidClosedLowerBound(nameof(green), green, 0));
            if (blue < 0)
                throw new ArgumentOutOfRangeException(nameof(blue),
                    SR.ErrorInvalidClosedLowerBound(nameof(blue), blue, 0));

            var total = red + green + blue;

            if (total > 0)
            {
                Red = red / total;
                Green = green / total;
                Blue = blue / total;
            }
            else
                this = Empty;
        }

        public static explicit operator ColorWeight(ColorRgb color)
        {
            return new ColorWeight(color.Red, color.Green, color.Blue);
        }

        public static bool operator ==(ColorWeight left, ColorWeight right)
        {
            return left.Red == right.Red &&
                left.Green == right.Green &&
                left.Blue == right.Blue;
        }
        public static bool operator !=(ColorWeight left, ColorWeight right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ColorWeight))
                return false;

            return (ColorWeight)obj == this;
        }
        public override int GetHashCode()
        {
            return Hash.Generate(Red, Green, Blue);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Red));
            sb.Append(": ");
            sb.Append(SR.GetString(Red));
            sb.Append(", ");
            sb.Append(nameof(Green));
            sb.Append(": ");
            sb.Append(SR.GetString(Green));
            sb.Append(", ");
            sb.Append(nameof(Blue));
            sb.Append(": ");
            sb.Append(SR.GetString(Blue));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
