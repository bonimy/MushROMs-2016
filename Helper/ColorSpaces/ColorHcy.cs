/* Refer to https://en.wikipedia.org/wiki/Luma_(video) for more information on luma
 * and its utility in color modelling.
 *
 * Refer to https://en.wikipedia.org/wiki/HSL_and_HSV for more information on the hue-chroma-luma
 * color model in general.
 */

using System;
using System.Drawing;
using System.Text;

namespace Helper.ColorSpaces
{
    /// <summary>
    /// Represents a color with <see cref="Single"/> representations
    /// of alpha, hue, chroma, and luma channels ranging from 0
    /// to 1, inclusive.
    /// </summary>
    public struct ColorHcy
    {
        /// <summary>
        /// Represents a <see cref="ColorHcy"/> that has its <see cref="Alpha"/>,
        /// <see cref="Hue"/>, <see cref="Chroma"/>, and <see cref="Luma"/>
        /// channels all set to zero.
        /// </summary>
        /// <remarks>
        /// By definition, <see cref="ColorRgb.Empty"/> is black and <see cref="Empty"/>
        /// is white. Therefore, a default equality comparison between the two values
        /// returns false.
        /// </remarks>
        public static readonly ColorHcy Empty = new ColorHcy();

        /// <summary>
        /// The minimum rounding error two components can have to be considered equal.
        /// This field is constant.
        /// </summary>
        private const float Tolerance = ColorRgb.Tolerance;

        /// <summary>
        /// The weight of the color's red channel in determining its <see cref="Luma"/>.
        /// This field is constant.
        /// </summary>
        public const float RedWeight = 0.299f;

        /// <summary>
        /// The weight of the color's green channel in determining its <see cref="Luma"/>.
        /// This field is constant.
        /// </summary>
        public const float GreenWeight = 0.587f;

        /// <summary>
        /// The weight of the color's blue channel in determining its <see cref="Luma"/>.
        /// This field is constant.
        /// </summary>
        public const float BlueWeight = 0.114f;

        /// <summary>
        /// Gets the alpha channel of this <see cref="ColorHcy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Alpha
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the hue channel of this <see cref="ColorHcy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Hue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the chroma channel of this <see cref="ColorHcy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Chroma
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the luma channel of this <see cref="ColorHcy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Luma
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorHcy"/> structure using the given
        /// color channels and an alpha component of its maximum value, 1.
        /// </summary>
        /// <param name="hue">
        /// The intensity of the <see cref="Hue"/> channel.
        /// </param>
        /// <param name="chroma">
        /// The intensity of the <see cref="Chroma"/> channel.
        /// </param>
        /// <param name="luma">
        /// The intensity of the <see cref="Luma"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="hue"/> is NaN. -or-
        /// <paramref name="chroma"/> is NaN. -or-
        /// <paramref name="luma"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorHcy(float hue, float chroma, float luma)
            : this(1, hue, chroma, luma)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorHcy"/> structure using the given
        /// color channels.
        /// </summary>
        /// <param name="alpha">
        /// The intensity of the <see cref="Alpha"/> channel.
        /// </param>
        /// <param name="hue">
        /// The intensity of the <see cref="Hue"/> channel.
        /// </param>
        /// <param name="chroma">
        /// The intensity of the <see cref="Chroma"/> channel.
        /// </param>
        /// <param name="luma">
        /// The intensity of the <see cref="Luma"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/> is NaN. -or-
        /// <paramref name="hue"/> is NaN. -or-
        /// <paramref name="chroma"/> is NaN. -or-
        /// <paramref name="luma"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorHcy(float alpha, float hue, float chroma, float luma)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(hue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(chroma)), nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(luma)), nameof(luma));
            }

            Alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            Hue = MathHelper.Clamp(hue, 0, 1, Tolerance);
            Chroma = MathHelper.Clamp(chroma, 0, 1, Tolerance);
            Luma = MathHelper.Clamp(luma, 0, 1, Tolerance);
        }

        public ColorRgb Colorize(float hue, float chroma, float luma, float weight)
        {
            if (Single.IsNaN(hue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(chroma)), nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(luma)), nameof(luma));
            }

            if (Single.IsNaN(weight))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(weight)), nameof(weight));
            }

            hue = MathHelper.Clamp(hue, 0, 1, Tolerance);
            chroma = MathHelper.Clamp(chroma, 0, 1, Tolerance);
            luma = MathHelper.Clamp(luma, 0, 1, Tolerance);
            weight = MathHelper.Clamp(weight, 0, 1, Tolerance);

            luma = Luma + (2 * luma - 1) *
                (luma > 0.5f ? 1 - Luma : Luma);
            var hcy = new ColorHcy(hue, chroma, luma);
            return ((ColorRgb)this).AverageWith(hcy, weight);
        }

        public ColorRgb Adjust(float hue, float chroma, float luma, float weight)
        {
            if (Single.IsNaN(hue))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            }

            if (Single.IsNaN(chroma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(chroma)), nameof(chroma));
            }

            if (Single.IsNaN(luma))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(luma)), nameof(luma));
            }

            if (Single.IsNaN(weight))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(weight)), nameof(weight));
            }

            hue = MathHelper.Clamp(hue, -1, 1, Tolerance);
            chroma = MathHelper.Clamp(chroma, -1, 1, Tolerance);
            luma = MathHelper.Clamp(luma, -1, 1, Tolerance);
            weight = MathHelper.Clamp(weight, 0, 1, Tolerance);

            hue /= 2;
            hue += Hue;
            while (hue < 0)
            {
                hue += 1;
            }

            while (hue > 1)
            {
                hue -= 1;
            }

            chroma = Chroma + chroma * (chroma > 0 ? 1 - Chroma : Chroma);
            luma = Luma + luma * (luma > 0 ? 1 - Luma : Luma);
            var hcy = new ColorHcy(hue, chroma, luma);
            return ((ColorRgb)this).AverageWith(hcy, weight);
        }

        /// <summary>
        /// Converts a <see cref="ColorHcy"/> structure to a <see cref="Color"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHcy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/> the results from the conversion.
        /// </returns>
        public static explicit operator Color(ColorHcy color)
        {
            return (Color)color;
        }

        /// <summary>
        /// Converts a <see cref="Color"/> structure to a <see cref="ColorHcy"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHcy"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHcy(Color color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorRgb"/> structure to a <see cref="ColorHcy"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHcy"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHcy(ColorRgb color)
        {
            var max = color.Max;
            var luma = color.Luma;
            var chroma = color.Chroma;
            var hue = 0f;

            if (chroma != 0)
            {
                if (max == color.Red)
                {
                    hue = (color.Green - color.Blue) / chroma;
                }
                else if (max == color.Green)
                {
                    hue = (color.Blue - color.Red) / chroma + 2;
                }
                else //if (max == color.Blue)
                {
                    hue = (color.Red - color.Green) / chroma + 4;
                }

                if (hue < 0)
                {
                    hue += 6;
                }

                hue /= 6;
            }

            return new ColorHcy(color.Alpha, hue, chroma, luma);
        }

        /// <summary>
        /// Converts a <see cref="ColorCmy"/> structure to a <see cref="ColorHcy"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHcy"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHcy(ColorCmy color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorCmyk"/> structure to a <see cref="ColorHcy"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmyk"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHcy"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHcy(ColorCmyk color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHsl"/> structure to a <see cref="ColorHcy"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsl"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHcy"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHcy(ColorHsl color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHsv"/> structure to a <see cref="ColorHcy"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsv"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHcy"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorHcy(ColorHsv color)
        {
            return color;
        }

        /// <summary>
        /// Compares two <see cref="ColorHcy"/> objects. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, ColorHcy right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares two <see cref="ColorHcy"/> objects. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, ColorHcy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, Color right)
        {
            return (ColorRgb)left == right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, Color right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(Color left, ColorHcy right)
        {
            return right == left;
        }

        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(Color left, ColorHcy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, ColorRgb right)
        {
            return right == left;
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, ColorRgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, ColorCmyk right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, ColorCmyk right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, ColorCmy right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, ColorCmy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, ColorHsl right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, ColorHsl right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorHcy left, ColorHsv right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorHcy"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorHcy left, ColorHsv right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="ColorHcy"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is the same color as this <see cref="ColorHcy"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorHcy))
            {
                return false;
            }

            return (ColorHcy)obj == this;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorHcy"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="ColorHcy"/>.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="ColorHcy"/> is equal to the hash code for its
        /// <see cref="ColorRgb"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            return ((ColorRgb)this).GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="ColorHcy"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="ColorHcy"/>.
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
            sb.Append(nameof(Chroma));
            sb.Append(": ");
            sb.Append(SR.GetString(Chroma));
            sb.Append(", ");
            sb.Append(nameof(Luma));
            sb.Append(": ");
            sb.Append(SR.GetString(Luma));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
