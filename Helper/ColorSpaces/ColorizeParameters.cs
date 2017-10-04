using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.ColorSpaces
{
    public struct ColorizeParameters
    {
        /// <summary>
        /// Represents a <see cref="ColorizeParameters"/> that has its <see cref="Alpha"/>,
        /// <see cref="Hue"/>, <see cref="Saturation"/>, and <see cref="Lightness"/>
        /// channels all set to zero.
        /// </summary>
        public static readonly ColorizeParameters Empty = new ColorizeParameters();

        /// <summary>
        /// The minimum rounding error two components can have to be considered equal.
        /// This field is constant.
        /// </summary>
        private const float Tolerance = ColorRgb.Tolerance;

        /// <summary>
        /// Gets the alpha blend of this <see cref="ColorizeParameters"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Alpha
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the hue colorization of this <see cref="ColorizeParameters"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Hue
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the saturation colorization of this <see cref="ColorizeParameters"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Saturation
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the lightness colorization of this <see cref="ColorizeParameters"/> structure.
        /// Valid values range from 0 to 1 inclusive.
        /// </summary>
        public float Lightness
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorizeParameters"/> structure using the given
        /// colorizations and an alpha component of its maximum value, 1.
        /// </summary>
        /// <param name="hue">
        /// The <see cref="Hue"/> colorization value.
        /// </param>
        /// <param name="saturation">
        /// The <see cref="Saturation"/> colorization value.
        /// </param>
        /// <param name="lightness">
        /// The <see cref="Lightness"/> colorization value.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="hue"/> is NaN. -or-
        /// <paramref name="saturation"/> is NaN. -or-
        /// <paramref name="lightness"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The colorization parameters can be any real value. If the value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorizeParameters(float hue, float saturation, float lightness)
            : this(1, hue, saturation, lightness)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorizeParameters"/> structure using the given
        /// colorization value.
        /// </summary>
        /// <param name="alpha">
        /// The <see cref="Alpha"/> blend value.
        /// </param>
        /// <param name="hue">
        /// The <see cref="Hue"/> colorization value.
        /// </param>
        /// <param name="saturation">
        /// The <see cref="Saturation"/> colorization value.
        /// </param>
        /// <param name="lightness">
        /// The <see cref="Lightness"/> colorization value.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/> is NaN. -or-
        /// <paramref name="hue"/> is NaN. -or-
        /// <paramref name="saturation"/> is NaN. -or-
        /// <paramref name="lightness"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The colorization parameters can be any real value. If the value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorizeParameters(float alpha, float hue, float saturation, float lightness)
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(hue))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(hue)), nameof(hue));
            if (Single.IsNaN(saturation))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(saturation)), nameof(saturation));
            if (Single.IsNaN(lightness))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(lightness)), nameof(lightness));

            Alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            Hue = MathHelper.Clamp(hue, 0, 1, Tolerance);
            Saturation = MathHelper.Clamp(saturation, 0, 1, Tolerance);
            Lightness = MathHelper.Clamp(lightness, 0, 1, Tolerance);
        }

        /// <summary>
        /// Compares two <see cref="ColorizeParameters"/> objects. The result specifies whether
        /// their <see cref="Alpha"/>, <see cref="Hue"/>, <see cref="Saturation"/>, and
        /// <see cref="Lightness"/> values are equal within <see cref="Tolerance"/> to their
        /// comparing <see cref="ColorizeParameters"/>.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorizeParameters"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorizeParameters"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/> and have equal
        /// <see cref="Alpha"/>, <see cref="Hue"/>, <see cref="Saturation"/>, and
        /// <see cref="Lightness"/> values within <see cref="Tolerance"/>; otherwise false.
        /// </returns>
        public static bool operator ==(ColorizeParameters left, ColorizeParameters right)
        {
            return
                MathHelper.NearlyEquals(left.Alpha, right.Alpha, Tolerance) &&
                MathHelper.NearlyEquals(left.Hue, right.Hue, Tolerance) &&
                MathHelper.NearlyEquals(left.Saturation, right.Saturation, Tolerance) &&
                MathHelper.NearlyEquals(left.Lightness, right.Lightness, Tolerance);
        }
        /// <summary>
        /// Compares two <see cref="ColorizeParameters"/> objects. The result specifies whether
        /// their <see cref="Alpha"/>, <see cref="Hue"/>, <see cref="Saturation"/>, or
        /// <see cref="Lightness"/> values are unequal within <see cref="Tolerance"/> to their
        /// comparing <see cref="ColorizeParameters"/>.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorizeParameters"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorizeParameters"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/> and have unequal
        /// <see cref="Alpha"/>, <see cref="Hue"/>, <see cref="Saturation"/>, or
        /// <see cref="Lightness"/> values within <see cref="Tolerance"/>; otherwise false.
        /// </returns>
        public static bool operator !=(ColorizeParameters left, ColorizeParameters right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="ColorizeParameters"/> is the same value as
        /// the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is the same value as this <see cref="ColorizeParameters"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorizeParameters))
                return false;

            return (ColorizeParameters)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="ColorizeParameters"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="ColorizeParameters"/>.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="ColorizeParameters"/> is equal to the hash code for its
        /// <see cref="ColorRgb"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            return Hash.Generate(Alpha, Hue, Saturation, Lightness);
        }
        /// <summary>
        /// Converts this <see cref="ColorizeParameters"/> to a human-readable <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> the represent this <see cref="ColorizeParameters"/>.
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
            sb.Append(nameof(Lightness));
            sb.Append(": ");
            sb.Append(SR.GetString(Lightness));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
