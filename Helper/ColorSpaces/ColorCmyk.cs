/* Refer to https://en.wikipedia.org/wiki/CMYK_color_model for more information on
 * the CMY color model.
 */

using System;
using System.Drawing;
using System.Text;

namespace Helper.ColorSpaces
{
    /// <summary>
    /// Represents a color with <see cref="Single"/> representations
    /// of alpha, cyan, magenta, yellow, and black channels ranging
    /// from 0 to 1, inclusive.
    /// </summary>
    public struct ColorCmyk
    {
        /// <summary>
        /// Represents a <see cref="ColorCmyk"/> that has its <see cref="Alpha"/>,
        /// <see cref="Cyan"/>, <see cref="Magenta"/>, <see cref="Yellow"/>, and
        /// <see cref="Key"/> channels all set to zero.
        /// </summary>
        public static readonly ColorCmyk Empty = new ColorCmyk();

        /// <summary>
        /// The minimum rounding error two components can have to be considered equal.
        /// This field is constant.
        /// </summary>
        private const float Tolerance = ColorRgb.Tolerance;

        /// <summary>
        /// Gets the alpha channel of this <see cref="ColorCmy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Alpha
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the cyan channel of this <see cref="ColorCmy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Cyan
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the magenta channel of this <see cref="ColorCmy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Magenta
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the yellow channel of this <see cref="ColorCmy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Yellow
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the black channel of this <see cref="ColorCmy"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorCmy"/> structure using the given
        /// color channels and an alpha component of its maximum value, 1.
        /// </summary>
        /// <param name="cyan">
        /// The intensity of the <see cref="Cyan"/> channel.
        /// </param>
        /// <param name="magenta">
        /// The intensity of the <see cref="Magenta"/> channel.
        /// </param>
        /// <param name="yellow">
        /// The intensity of the <see cref="Yellow"/> channel.
        /// </param>
        /// <param name="key">
        /// The intensity of the <see cref="Key"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="cyan"/> is NaN. -or-
        /// <paramref name="magenta"/> is NaN. -or-
        /// <paramref name="yellow"/> is NaN. -or-
        /// <paramref name="key"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorCmyk(float cyan, float magenta, float yellow, float key)
            : this(1, cyan, magenta, yellow, key)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorCmyk"/> structure using the given
        /// color channels and an alpha component of its maximum value, 1.
        /// </summary>
        /// <param name="alpha">
        /// The intensity of the <see cref="Alpha"/> channel.
        /// </param>
        /// <param name="cyan">
        /// The intensity of the <see cref="Cyan"/> channel.
        /// </param>
        /// <param name="magenta">
        /// The intensity of the <see cref="Magenta"/> channel.
        /// </param>
        /// <param name="yellow">
        /// The intensity of the <see cref="Yellow"/> channel.
        /// </param>
        /// <param name="key">
        /// The intensity of the <see cref="Key"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/> is NaN. -or-
        /// <paramref name="cyan"/> is NaN. -or-
        /// <paramref name="magenta"/> is NaN. -or-
        /// <paramref name="yellow"/> is NaN. -or-
        /// <paramref name="key"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorCmyk(float alpha, float cyan, float magenta, float yellow, float key)
        {
            if (Single.IsNaN(alpha))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            }

            if (Single.IsNaN(cyan))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(cyan)), nameof(cyan));
            }

            if (Single.IsNaN(magenta))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(magenta)), nameof(magenta));
            }

            if (Single.IsNaN(yellow))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(yellow)), nameof(yellow));
            }

            if (Single.IsNaN(key))
            {
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(key)), nameof(key));
            }

            Alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            Cyan = MathHelper.Clamp(cyan, 0, 1, Tolerance);
            Yellow = MathHelper.Clamp(yellow, 0, 1, Tolerance);
            Magenta = MathHelper.Clamp(magenta, 0, 1, Tolerance);
            Key = MathHelper.Clamp(key, 0, 1, Tolerance);
        }

        /// <summary>
        /// Converts a <see cref="ColorCmyk"/> structure to a <see cref="Color"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmyk"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/> the results from the conversion.
        /// </returns>
        public static explicit operator Color(ColorCmyk color)
        {
            return (Color)color;
        }

        /// <summary>
        /// Converts a <see cref="Color"/> structure to a <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorCmyk(Color color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorRgb"/> structure to a <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorCmyk(ColorRgb color)
        {
            var white = color.Max;
            if (white == 0)
            {
                return new ColorCmyk(color.Alpha, 0, 0, 0, 1);
            }

            return new ColorCmyk(
                color.Alpha,
                (white - color.Red) / white,
                (white - color.Green) / white,
                (white - color.Blue) / white,
                1 - white);
        }

        /// <summary>
        /// Converts a <see cref="ColorCmy"/> structure to a <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorCmyk(ColorCmy color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHcy"/> structure to a <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHcy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorCmyk(ColorHcy color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHsl"/> structure to a <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsl"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorCmyk(ColorHsl color)
        {
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorHsv"/> structure to a <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsv"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorCmyk(ColorHsv color)
        {
            return color;
        }

        /// <summary>
        /// Compares two <see cref="ColorCmyk"/> objects. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorCmyk right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares two <see cref="ColorCmyk"/> objects. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorCmyk right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, Color right)
        {
            return (ColorRgb)left == right;
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, Color right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(Color left, ColorCmyk right)
        {
            return right == left;
        }

        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(Color left, ColorCmyk right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorRgb right)
        {
            return right == left;
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorRgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorCmy right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorCmy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorHcy right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorHcy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorHsl right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorHsl right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorHsv right)
        {
            return left == (ColorRgb)right;
        }

        /// <summary>
        /// Compares a <see cref="ColorCmyk"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorHsv right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="ColorCmyk"/> is the same color as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is the same color as this <see cref="ColorCmyk"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorCmyk))
            {
                return false;
            }

            return (ColorCmyk)obj == this;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorCmyk"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="ColorCmyk"/>.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="ColorCmyk"/> is equal to the hash code for its
        /// <see cref="ColorRgb"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            return ((ColorRgb)this).GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="ColorCmyk"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> the represent this <see cref="ColorCmyk"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Alpha));
            sb.Append(": ");
            sb.Append(SR.GetString(Alpha));
            sb.Append(", ");
            sb.Append(nameof(Cyan));
            sb.Append(": ");
            sb.Append(SR.GetString(Cyan));
            sb.Append(", ");
            sb.Append(nameof(Magenta));
            sb.Append(": ");
            sb.Append(SR.GetString(Magenta));
            sb.Append(", ");
            sb.Append(nameof(Yellow));
            sb.Append(": ");
            sb.Append(SR.GetString(Yellow));
            sb.Append(", ");
            sb.Append(nameof(Key));
            sb.Append(": ");
            sb.Append(SR.GetString(Key));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
