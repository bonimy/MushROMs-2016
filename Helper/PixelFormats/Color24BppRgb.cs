using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Helper.ColorSpaces;

namespace Helper.PixelFormats
{
    [StructLayout(LayoutKind.Explicit)]
    [TypeConverter(typeof(Color24BppRgbConverter))]
    public unsafe struct Color24BppRgb
    {
        public const int SizeOf = 3 * sizeof(byte);

        public static readonly Color24BppRgb Empty = new Color24BppRgb();

        private const int RedFieldOffset = 2;
        private const int GreenFieldOffset = 1;
        private const int BlueFieldOffset = 0;

        [FieldOffset(0)]
        private fixed byte _components[SizeOf];

        [FieldOffset(RedFieldOffset)]
        private byte _red;

        [FieldOffset(GreenFieldOffset)]
        private byte _green;

        [FieldOffset(BlueFieldOffset)]
        private byte _blue;

        public byte Red
        {
            get { return _red; }
            set { _red = value; }
        }

        public byte Green
        {
            get { return _green; }
            set { _green = value; }
        }

        public byte Blue
        {
            get { return _blue; }
            set { _blue = value; }
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= SizeOf)
                {
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);
                }

                fixed (byte* components = _components)
                    return components[index];
            }

            set
            {
                if (index < 0 || index >= SizeOf)
                {
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);
                }

                fixed (byte* components = _components)
                    components[index] = value;
            }
        }

        public int Value
        {
            get
            {
                return
                    (Red << (BitArray.BitsPerByte * RedFieldOffset)) |
                    (Green << (BitArray.BitsPerByte * GreenFieldOffset)) |
                    (Blue << (BitArray.BitsPerByte * BlueFieldOffset));
            }

            set
            {
                this = new Color24BppRgb(value);
            }
        }

        [Browsable(false)]
        public Color Color
        {
            get { return Color.FromArgb(Value); }
            set { Value = value.ToArgb(); }
        }

        private Color24BppRgb(int value)
            : this(
                value >> (BitArray.BitsPerByte * RedFieldOffset),
                value >> (BitArray.BitsPerByte * GreenFieldOffset),
                value >> (BitArray.BitsPerByte * BlueFieldOffset))
        { }

        private Color24BppRgb(Color color) : this(color.ToArgb())
        { }

        public Color24BppRgb(int red, int green, int blue)
        {
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        public static explicit operator Color24BppRgb(int value)
        {
            return new Color24BppRgb(value);
        }

        public static implicit operator int(Color24BppRgb color24)
        {
            return color24.Value;
        }

        public static explicit operator Color24BppRgb(Color color)
        {
            return new Color24BppRgb(color);
        }

        public static implicit operator Color(Color24BppRgb color24)
        {
            return color24.Color;
        }

        public static explicit operator Color24BppRgb(Color32BppArgb color32)
        {
            return new Color24BppRgb(color32.Value);
        }

        public static implicit operator Color32BppArgb(Color24BppRgb color24)
        {
            return color24.Value;
        }

        public static explicit operator Color24BppRgb(ColorRgb color)
        {
            return new Color24BppRgb(
                (int)(color.Red * Byte.MaxValue + 0.5f),
                (int)(color.Green * Byte.MaxValue + 0.5f),
                (int)(color.Blue * Byte.MaxValue + 0.5f));
        }

        public static implicit operator ColorRgb(Color24BppRgb pixel)
        {
            return new ColorRgb(
                pixel.Red / (float)Byte.MaxValue,
                pixel.Green / (float)Byte.MaxValue,
                pixel.Blue / (float)Byte.MaxValue);
        }

        public static bool operator ==(Color24BppRgb left, Color24BppRgb right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(Color24BppRgb left, Color24BppRgb right)
        {
            return !(left.Value == right.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Color24BppRgb))
            {
                return false;
            }

            return (Color24BppRgb)obj == this;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Color.ToString();
        }
    }
}
