using System;
using System.Diagnostics;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Represents the start index, end index, and length of a substring.
    /// </summary>
    [DebuggerDisplay("Start = {Start}, Length = {Length}")]
    public struct SubstringPointer
    {
        /// <summary>
        /// Specifies an index the extends to the end of a <see cref="string"/>.
        /// This field is constant.
        /// </summary>
        public const int EndOfString = -1;

        /// <summary>
        /// Represents a <see cref="SubstringPointer"/> that has <see cref="Start"/> and
        /// <see cref="End"/> set to zero.
        /// </summary>
        public static readonly SubstringPointer Empty = new SubstringPointer();

        /// <summary>
        /// Gets the start index to be used for specifying a substring.
        /// </summary>
        public int Start
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the end index to be used for specifying a substring.
        /// </summary>
        public int End
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the length of the resulting substring described by this <see cref="SubstringPointer"/>.
        /// If <see cref="End"/> is set to <see cref="EndOfString"/>, then <see cref="EndOfString"/> is
        /// returned.
        /// </summary>
        public int Length
        {
            get { return End == EndOfString ? EndOfString : (End - Start); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstringPointer"/> structure using the given
        /// start and end indexes.
        /// </summary>
        /// <param name="start">
        /// The start index of the substring.
        /// </param>
        /// <param name="end">
        /// The end index of the substring. Set to <see cref="EndOfString"/> to specify the susbtring
        /// extends to the end of the <see cref="string"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="end"/> is less than <paramref name="start"/> and not set to <see cref="EndOfString"/>.
        /// </exception>
        public SubstringPointer(int start, int end)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), 
                    SR.ErrorInvalidClosedLowerBound(nameof(start), start, 0));
            if (end < start && end != EndOfString)
                throw new ArgumentException(nameof(end));
            
            Start = start;
            End = end;
        }

        /// <summary>
        /// Creates a new <see cref="SubstringPointer"/> from the specified start index
        /// and length.
        /// </summary>
        /// <param name="start">
        /// The start index of the substring.
        /// </param>
        /// <param name="length">
        /// The length of the substring. Set to <see cref="EndOfString"/> to specify the susbtring
        /// extends to the end of the <see cref="string"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="SubstringPointer"/> with the specified start index and length.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="length"/> is less than 0 and not set to <see cref="EndOfString"/>.
        /// </exception>
        public static SubstringPointer FromStartAndLength(int start, int length)
        {
            if (length == EndOfString)
                return new SubstringPointer(start, EndOfString);

            if (length < 0)
                throw new ArgumentException(nameof(length));

            return new SubstringPointer(start, start + length);
        }

        /// <summary>
        /// Gets a substring form <paramref name="value"/> using the specified information of this
        /// <see cref="SubstringPointer"/>.
        /// </summary>
        /// <param name="value">
        /// The <see cref="string"/> to get a substring from.
        /// </param>
        /// <returns>
        /// The substring specified by this <see cref="SubstringPointer"/> of <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Lenght of <paramref name="value"/> is not sufficient for <see cref="Length"/> of this
        /// <see cref="SubstringPointer"/>.
        /// </exception>
        public string GetSubstirng(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (Length == EndOfString)
                return value.Substring(Start);

            if (Length > value.Length)
                throw new ArgumentException(nameof(value));

            return value.Substring(Start, Length);
        }

        /// <summary>
        /// Compares two <see cref="SubstringPointer"/> objects. The result specifies
        /// whether <see cref="Start"/> and <see cref="End"/> of the two <see cref="SubstringPointer"/>
        /// objects are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="SubstringPointer"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="SubstringPointer"/> to compare.
        /// </param>
        /// <returns>
        /// true if <see cref="Start"/> and <see cref="End"/> of <paramref name="left"/> and
        /// <paramref name="right"/> are unequal; otherwise false.
        /// </returns>
        public static bool operator ==(SubstringPointer left, SubstringPointer right)
        {
            return left.Start == right.Start && left.End == right.End;
        }
        /// <summary>
        /// Compares two <see cref="SubstringPointer"/> objects. The result specifies
        /// whether <see cref="Start"/> and <see cref="End"/> of the two <see cref="SubstringPointer"/>
        /// objects are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="SubstringPointer"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="SubstringPointer"/> to compare.
        /// </param>
        /// <returns>
        /// true if <see cref="Start"/> and <see cref="End"/> of <paramref name="left"/> and
        /// <paramref name="right"/> are equal; otherwise false.
        /// </returns>
        public static bool operator !=(SubstringPointer left, SubstringPointer right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="SubstringPointer"/> has the same start and end index as
        /// the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> has the same start and end index as this <see cref="SubstringPointer"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SubstringPointer))
                return false;

            return (SubstringPointer)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="SubstringPointer"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="SubstringPointer"/>.
        /// </returns>
        /// <remarks>
        public override int GetHashCode()
        {
            return Start ^ End;
        }
        /// <summary>
        /// Converts this <see cref="ColorRgb"/> to a human-readable <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> the represent this <see cref="ColorRgb"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append(nameof(Start));
            sb.Append(": ");
            sb.Append(Start);
            sb.Append(", ");
            sb.Append(nameof(Length));
            sb.Append(": ");
            if (Length == EndOfString)
                sb.Append("End of String");
            else
                sb.Append(Length);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
