using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Helper.ColorSpaces;

namespace Helper.PixelFormats
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Color32BppArgb
    {
        public const int SizeOf = sizeof(int);

        public static readonly Color32BppArgb Empty = new Color32BppArgb();

        private const int AlphaFieldOffset = 3;
        private const int RedFieldOffset = 2;
        private const int GreenFieldOffset = 1;
        private const int BlueFieldOffset = 0;

        [FieldOffset(0)]
        private int _value;
        [FieldOffset(0)]
        private fixed byte _components[SizeOf];
        [FieldOffset(AlphaFieldOffset)]
        private byte _alpha;
        [FieldOffset(RedFieldOffset)]
        private byte _red;
        [FieldOffset(GreenFieldOffset)]
        private byte _green;
        [FieldOffset(BlueFieldOffset)]
        private byte _blue;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public Color Color
        {
            get { return Color.FromArgb(Value); }
            set { Value = value.ToArgb(); }
        }
        public byte Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }
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
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);

                fixed (byte* components = _components)
                    return components[index];
            }
            set
            {
                if (index < 0 || index >= SizeOf)
                    SR.ErrorArrayBounds(nameof(index), index, SizeOf);

                fixed (byte* components = _components)
                    components[index] = value;
            }
        }

        private Color32BppArgb(int value)
            : this()
        {
            _value = value;
        }

        private Color32BppArgb(Color color) : this(color.ToArgb())
        { }

        public Color32BppArgb(int red, int green, int blue) : this(Byte.MaxValue, red, green, blue)
        { }

        public Color32BppArgb(int alpha, int red, int green, int blue)
            : this()
        {
            _alpha = (byte)alpha;
            _red = (byte)red;
            _green = (byte)green;
            _blue = (byte)blue;
        }

        public static implicit operator Color32BppArgb(int value)
        {
            return new Color32BppArgb(value);
        }
        public static implicit operator int (Color32BppArgb pixel)
        {
            return pixel.Value;
        }

        public static implicit operator Color32BppArgb(Color color)
        {
            return new Color32BppArgb(color);
        }
        public static implicit operator Color(Color32BppArgb pixel)
        {
            return pixel.Color;
        }

        public static implicit operator Color32BppArgb(ColorRgb color)
        {
            return new Color32BppArgb(
                (int)(color.Alpha * Byte.MaxValue + 0.5f),
                (int)(color.Red * Byte.MaxValue + 0.5f),
                (int)(color.Green * Byte.MaxValue + 0.5f),
                (int)(color.Blue * Byte.MaxValue + 0.5f));
        }

        public static implicit operator ColorRgb(Color32BppArgb pixel)
        {
            return new ColorRgb(
                pixel.Alpha / (float)Byte.MaxValue,
                pixel.Red / (float)Byte.MaxValue,
                pixel.Green / (float)Byte.MaxValue,
                pixel.Blue / (float)Byte.MaxValue);
        }

        public static bool operator ==(Color32BppArgb left, Color32BppArgb right)
        {
            return left.Value == right.Value;
        }
        public static bool operator !=(Color32BppArgb left, Color32BppArgb right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Color32BppArgb))
                return false;

            return (Color32BppArgb)obj == this;
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