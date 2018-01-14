/* Refer to https://en.wikipedia.org/wiki/HSL_and_HSV for more information on the
 * hue-sat-val color model.
 */

using System;
using System.Drawing;
using System.Text;

namespace Helper.ColorSpaces
{
    /// <summary>
    /// Represents a color with <see cref="Single"/> representations
    /// of alpha, hue, saturation, and value channels ranging from 0
    /// to 1, inclusive.
    /// </summary>
    public struct ColorHsv
    {
        /// <summary>
        /// Represents a <see cref="ColorHsv"/> that has its <see cref="Alpha"/>,
        /// <see cref="Hue"/>, <see cref="Saturation"/>, and <see cref="Value"/>
        /// channels all set to zero.
        /// </summary>
        public static readonly ColorHsv Empty = new ColorHsv();

        /// <summary>
        /// The minimum rounding error two components can have to be considered equal.
        /// This field is constant.
        /// </summary>
        private const float Tolerance = ColorRgb.Tolerance;

        /// <summary>
        /// Gets the alpha channel of this <see cref="ColorHsv"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Alpha
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the hue channel of this <see cref="ColorHsv"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Hue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the saturation channel of this <see cref="ColorHsv"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Saturation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value channel of this <see cref="ColorHsv"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorHsv"/> structure using the given
        /// color channels and an alpha component of its maximum value, 1.
        /// </summary>
        /// <param name="hue">
        /// The intensity of the <see cref="Hue"/> channel.
        /// </param>
        /// <param name="saturation">
        /// The intensity of the <see cref="Saturation"/> channel.
        /// </param>
        /// <param name="value">
        /// The intensity of the <see cref="Value"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="hue"/> is NaN. -or-
        /// <paramref name="saturation"/> is NaN. -or-
        /// <paramref name="value"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorHsv(float hue, float saturation, float value)
            : this(1, hue, saturation, value)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorHsv"/> structure using the given
        /// color channels.
        /// </summary>
        /// <param name="alpha">
        /// The intensity of the <see cref="Alpha"/> channel.
        /// </param>
        /// <param name="hue">
        /// The intensity of the <see cref="Hue"/> channel.
        /// </param>
        /// <param name="saturation">
        /// The intensity of the <see cref="Saturation"/> channel.
        /// </param>
        /// <param name="value">
        /// The intensity of the <see cref="Value"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/> is NaN. -or-
        /// <paramref name="hue"/> is NaN. -or-
        /// <paramref name="saturation"/> is NaN. -or-
        /// <paramref name="value"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorHsv(float alpha, float hue, float saturation, float value)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            }

            if (Single.IsNaN(saturation))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(saturation)), nameof(saturation));
            }

            if (Single.IsNaN(value))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(value)), nameof(value));
            }

            Alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            Hue = MathHelper.Clamp(hue, 0, 1, Tolerance);
            Saturation = MathHelper.Clamp(saturation, 0, 1, Tolerance);
            Value = MathHelper.Clamp(value, 0, 1, Tolerance);
        }

        /// <summary>
        /// Converts a <see cref="ColorHsv"/> structure to a <see cref="Color"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsv"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/> the results from the conversion.
        /// </returns>
        public static explicit operator Color(ColorHsv color)
        {
            return (Color)color;
        }

        /// <summary>
        /// Converts a <see cref="Color"/> structure to a <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHsv(Color color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorRgb"/> structure to a <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHsv(ColorRgb color)
        {
            var value = color.Max;
            var chroma = value - color.Min;
            var hue = 0f;
            var saturation = 0f;

            if (chroma != 0)
            {
                if (value == color.Red)
                {
                    hue = (color.Green - color.Blue) / chroma;
                    if (hue < 0)
                    {
                        hue += 6;
                    }
                }
                else if (value == color.Green)
                {
                    hue = (color.Blue - color.Red) / chroma + 2;
                }
                else //if (value == color.Blue)
                {
                    hue = (color.Red - color.Green) / chroma + 4;
                }

                hue /= 6;
            }
            if (value != 0)
            {
                saturation = chroma / value;
            }

            return new ColorHsv(color.Alpha, hue, saturation, value);
        }

        /// <summary>
        /// Converts a <see cref="ColorCmy"/> structure to a <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHsv(ColorCmy color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorCmyk"/> structure to a <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmyk"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHsv(ColorCmyk color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHcy"/> structure to a <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHcy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHsv(ColorHcy color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHsl"/> structure to a <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsl"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHsv(ColorHsl color)
        {
            return color;
        }

        /// <summary>
        /// Compares two <see cref="ColorHsv"/> objects. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorHsv right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares two <see cref="ColorHsv"/> objects. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorHsv right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, Color right)
        {
            return (ColorRgb)left == right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, Color right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(Color left, ColorHsv right)
        {
            return right == left;
        }

        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(Color left, ColorHsv right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorRgb right)
        {
            return right == left;
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorRgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorCmyk right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorCmyk right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorHcy right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorHcy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorHsl right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorHsl right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorCmy right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHsv"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorCmy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="ColorHsv"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is the same color as this <see cref="ColorHsv"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorHsv))
            {
                return false;
            }

            return (ColorHsv)obj == this;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorHsv"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="ColorHsv"/>.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="ColorHsv"/> is equal to the hash code for its
        /// <see cref="ColorRgb"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            return ((ColorRgb)this).GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="ColorHsv"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="ColorHsv"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Alpha));
            sb.Append(": ");
            sb.Append(SR.GetString(Alpha));
            sb.Append(", ");
            sb.Append(nameof(Hue));
            sb.Append(": ");
            sb.Append(SR.GetString(Hue));
            sb.Append(", ");
            sb.Append(nameof(Saturation));
            sb.Append(": ");
            sb.Append(SR.GetString(Saturation));
            sb.Append(", ");
            sb.Append(nameof(Value));
            sb.Append(": ");
            sb.Append(SR.GetString(Value));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
