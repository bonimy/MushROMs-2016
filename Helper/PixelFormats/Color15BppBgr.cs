using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Helper.ColorSpaces;

namespace Helper.PixelFormats
{
    /// <summary>
    /// Represents 15-bit RGB color in the format
    /// [? b b b b b g g - g g g r r r r r]. The most
    /// significant bit is ignored.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color15BppBgr
    {
        /// <summary>
        /// The size, in bytes, of <see cref="Color15BppBgr"/>.
        /// This field is constant.
        /// </summary>
        public const int SizeOf = sizeof(ushort);

        /// <summary>
        /// Represents a <see cref="Color15BppBgr"/> that has its <see cref="Value"/>
        /// set to 0.
        /// </summary>
        public static readonly Color15BppBgr Empty = new Color15BppBgr();

        /// <summary>
        /// The number of channels that specifies a <see cref="Color15BppBgr"/> value.
        /// This field is constant.
        /// </summary>
        private const int NumberOfChannels = 3;

        /// <summary>
        /// The index of the <see cref="Red"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int RedIndex = 0;
        /// <summary>
        /// The index of the <see cref="Green"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int GreenIndex = 1;
        /// <summary>
        /// The index of the <see cref="Blue"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int BlueIndex = 2;

        /// <summary>
        /// The number of bits each channel consumes.
        /// This field is constant.
        /// </summary>
        private const int BitsPerChannel = 5;
        /// <summary>
        /// The number of bits the <see cref="Red"/> channel is shifted by.
        /// This field is constant.
        /// </summary>
        private const int RedShift = BitsPerChannel * RedIndex;
        /// <summary>
        /// The number of bits the <see cref="Green"/> channel is shifted by.
        /// This field is constant.
        /// </summary>
        private const int GreenShift = BitsPerChannel * GreenIndex;
        /// <summary>
        /// The number of bits the <see cref="Blue"/> channel is shifted by.
        /// This field is constant.
        /// </summary>
        private const int BlueShift = BitsPerChannel * BlueIndex;
        /// <summary>
        /// The bit-mask for the lowest channel.
        /// This field is constant.
        /// </summary>
        private const int ChannelMask = (1 << BitsPerChannel) - 1;
        /// <summary>
        /// The bit-mask of the <see cref="Red"/> channel.
        /// This field is constant.
        /// </summary>
        private const int RedMask = ChannelMask << RedShift;
        /// <summary>
        /// The bit-mask of the <see cref="Green"/> channel.
        /// This field is constant.
        /// </summary>
        private const int GreenMask = ChannelMask << GreenShift;
        /// <summary>
        /// The bit-mask of the <see cref="Blue"/> channel.
        /// This field is constant.
        /// </summary>
        private const int BlueMask = ChannelMask << BlueShift;
        /// <summary>
        /// The bit-mask for the color.
        /// This field is constant.
        /// </summary>
        private const int ColorMask = RedMask | GreenMask | BlueMask;

        /// <summary>
        /// The color value, as a 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// This is the fundamental type of this <see cref="Color15BppBgr"/>.
        /// </remarks>
        [FieldOffset(0)]
        private ushort _value;
        /// <summary>
        /// The high <see cref="byte"/> of <see cref="_value"/>.
        /// </summary>
        [FieldOffset(0)]
        private byte _high;
        /// <summary>
        /// The low <see cref="byte"/> of <see cref="_value"/>.
        /// </summary>
        [FieldOffset(1)]
        private byte _low;

        /// <summary>
        /// Gets or sets the color value, as a 16-bit unsigned integer. The most
        /// significant bit has no meaning.
        /// </summary>
        /// <remarks>
        /// The most significant bit can be either 0 or 1. It does not affect operations on
        /// this <see cref="Color15BppBgr"/>. However, its value will be preserved through this
        /// object's lifetime.
        /// </remarks>
        public int Value
        {
            get { return _value; }
            set { _value = (ushort)value; }
        }

        /// <summary>
        /// Gets or sets the color value, as a 16-bit unsigned integer. The most significant
        /// bit is always cleared.
        /// </summary>
        public int PreservedValue
        {
            get { return Value & ColorMask; }
            set { Value &= ~ColorMask; Value |= (value & ColorMask); }
        }

        /// <summary>
        /// Gets or sets the high <see cref="byte"/> of <see cref="Value"/>.
        /// </summary>
        public byte High
        {
            get { return _high; }
            set { _high = value; }
        }
        /// <summary>
        /// Gets or sets the low <see cref="byte"/> of <see cref="Value"/>.
        /// </summary>
        public byte Low
        {
            get { return _low; }
            set { _low = value; }
        }

        /// <summary>
        /// Gets or sets the red channel of this <see cref="Color15BppBgr"/> structure.
        /// Only the lowest 5 bits are considered.
        /// </summary>
        public byte Red
        {
            get { return this[RedIndex]; }
            set { this[RedIndex] = value; }
        }
        /// <summary>
        /// Gets or sets the green channel of this <see cref="Color15BppBgr"/> structure.
        /// Only the lowest 5 bits are considered.
        /// </summary>
        public byte Green
        {
            get { return this[GreenIndex]; }
            set { this[GreenIndex] = value; }
        }
        /// <summary>
        /// Gets or sets the blue channel of this <see cref="Color15BppBgr"/> structure.
        /// Only the lowest 5 bits are considered.
        /// </summary>
        public byte Blue
        {
            get { return this[BlueIndex]; }
            set { this[BlueIndex] = value; }
        }

        /// <summary>
        /// Gets or sets the channel of this <see cref="Color15BppBgr"/> structure at the
        /// specified index. Only the lowest 5 bits are considered.
        /// </summary>
        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                case RedIndex:
                    return (byte)((Value & RedMask) >> RedShift);
                case GreenIndex:
                    return (byte)((Value & GreenMask) >> GreenShift);
                case BlueIndex:
                    return (byte)((Value & BlueMask) >> BlueShift);
                default:
                    throw new ArgumentOutOfRangeException(nameof(index),
                        SR.ErrorArrayBounds(nameof(index), index, NumberOfChannels));
                }
            }
            set
            {
                unchecked
                {
                    switch (index)
                    {
                    case RedIndex:
                        Value &= (ushort)(~RedMask);
                        Value |= (ushort)((value & ChannelMask) << RedShift);
                        return;
                    case GreenIndex:
                        Value &= (ushort)(~GreenMask);
                        Value |= (ushort)((value & ChannelMask) << GreenShift);
                        return;
                    case BlueIndex:
                        Value &= (ushort)(~BlueMask);
                        Value |= (ushort)((value & ChannelMask) << BlueShift);
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index),
                            SR.ErrorArrayBounds(nameof(index), index, NumberOfChannels));
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppBgr"/> structure using the
        /// given color value.
        /// </summary>
        /// <param name="value">
        /// The color value.
        /// </param>
        private Color15BppBgr(ushort value) : this()
        {
            Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Color15BppBgr"/> structure using the
        /// given color channels. Only the lowest 5 bits of each channel are used.
        /// </summary>
        /// <param name="red">
        /// The intensity of the <see cref="Red"/> channel.
        /// </param>
        /// <param name="green">
        /// The intensity of the <see cref="Green"/> channel.
        /// </param>
        /// <param name="blue">
        /// The intensity of the <see cref="Blue"/> channel.
        /// </param>
        public Color15BppBgr(int red, int green, int blue)
            : this(
                (ushort)(
                ((red & ChannelMask) << RedShift) |
                ((green & ChannelMask) << GreenShift) |
                ((blue & ChannelMask) << BlueShift)))
        { }

        /// <summary>
        /// Converts an <see cref="int"/> data type to a <see cref="Color15BppBgr"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="int"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppBgr"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color15BppBgr(int value)
        {
            return new Color15BppBgr((ushort)value);
        }

        /// <summary>
        /// Converts a <see cref="Color15BppBgr"/> structure to an <see cref="int"/>
        /// data type.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color15BppBgr"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> that results from the conversion.
        /// </returns>
        public static implicit operator int (Color15BppBgr color15)
        {
            return color15.Value;
        }

        /// <summary>
        /// Converts a <see cref="Color"/> structure to a <see cref="Color15BppBgr"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppBgr"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color15BppBgr(Color color)
        {
            return (Color15BppBgr)(Color32BppArgb)color;
        }
        /// <summary>
        /// Converts a <see cref="Color15BppBgr"/> structure to a <see cref="Color"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color15BppBgr"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color(Color15BppBgr color15)
        {
            return (Color32BppArgb)color15;
        }

        /// <summary>
        /// Converts a <see cref="Color24BppRgb"/> structure to a <see cref="Color15BppBgr"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color24BppRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppBgr"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color15BppBgr(Color24BppRgb color24)
        {
            // Each channel goes from 8 bits of sensitivity to 5
            // So we shift right 3 bytes for the conversion.
            return new Color15BppBgr(
                color24.Red >> (BitArray.BitsPerByte - BitsPerChannel),
                color24.Green >> (BitArray.BitsPerByte - BitsPerChannel),
                color24.Blue >> (BitArray.BitsPerByte - BitsPerChannel));
        }
        /// <summary>
        /// Converts a <see cref="Color15BppBgr"/> structure to a <see cref="Color24BppRgb"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color15BppBgr"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color24BppRgb"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color24BppRgb(Color15BppBgr color15)
        {
            // Each channel goes from 5 bits of sensitivity to 8
            // So we shift left 3 bytes for the conversion.
            return new Color24BppRgb(
                color15.Red << (BitArray.BitsPerByte - BitsPerChannel),
                color15.Green << (BitArray.BitsPerByte - BitsPerChannel),
                color15.Blue << (BitArray.BitsPerByte - BitsPerChannel));
        }

        /// <summary>
        /// Converts a <see cref="Color32BppRgb"/> structure to a <see cref="Color15BppBgr"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color32BppRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color15BppBgr"/> that results from the conversion.
        /// </returns>
        public static explicit operator Color15BppBgr(Color32BppArgb color32)
        {
            // Same as the 24-bit color conversion; we ignore the alpha component.
            return new Color15BppBgr(
                color32.Red >> (BitArray.BitsPerByte - BitsPerChannel),
                color32.Green >> (BitArray.BitsPerByte - BitsPerChannel),
                color32.Blue >> (BitArray.BitsPerByte - BitsPerChannel));
        }
        /// <summary>
        /// Converts a <see cref="Color15BppBgr"/> structure to a <see cref="Color32BppRgb"/>
        /// structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Color15BppBgr"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color32BppRgb"/> that results from the conversion.
        /// </returns>
        public static implicit operator Color32BppArgb(Color15BppBgr color15)
        {
            // Same as the 24-bit color conversion; we ignore the alpha component.
            return new Color32BppArgb(
                color15.Red << (BitArray.BitsPerByte - BitsPerChannel),
                color15.Green << (BitArray.BitsPerByte - BitsPerChannel),
                color15.Blue << (BitArray.BitsPerByte - BitsPerChannel));
        }

        public static explicit operator Color15BppBgr(ColorRgb color)
        {
            return (Color15BppBgr)((Color24BppRgb)color);
        }

        public static implicit operator ColorRgb(Color15BppBgr pixel)
        {
            return (Color24BppRgb)pixel;
        }

        public static bool operator ==(Color15BppBgr left, Color15BppBgr right)
        {
            // We compare the preserved values because we do not care about
            // the most significant bit.
            return left.PreservedValue == right.PreservedValue;
        }
        public static bool operator !=(Color15BppBgr left, Color15BppBgr right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="Color15BppBgr"/> is the same color as
        /// the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is the same color as this <see cref="Color15BppBgr"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Color15BppBgr))
                return false;

            return (Color15BppBgr)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="Color15BppBgr"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="Color15BppBgr"/>.
        /// </returns>
        public override int GetHashCode()
        {
            // The hash code should not contain the most significant bit.
            return PreservedValue;
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