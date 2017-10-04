/* Refer to https://en.wikipedia.org/wiki/RGB_color_model for a detailed
 * explanation of the RGB color model, which may prove useful to better understand
 * the conversion algorithms to other color models.
 * 
 * Refer to https://en.wikipedia.org/wiki/Blend_modes for information on all of the blend algorithms.
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Helper.ColorSpaces
{
    /// <summary>
    /// Represents a color with <see cref="float"/> representations
    /// of alpha, red, green, and blue channels ranging from 0 to 1, inclusive.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct ColorRgb
    {
        /// <summary>
        /// Represents a <see cref="ColorRgb"/> that has its <see cref="Alpha"/>,
        /// <see cref="Red"/>, <see cref="Green"/>, and <see cref="Blue"/> components
        /// all set to zero.
        /// </summary>
        public static readonly ColorRgb Empty = new ColorRgb();

        /// <summary>
        /// The number of channels that specifies a <see cref="ColorRgb"/> value.
        /// This field is constant.
        /// </summary>
        private const int NumberOfChannels = 4;
        /// <summary>
        /// The number of channels, excluding the alpha channel.
        /// </summary>
        private const int NumberOfColorChannels = NumberOfChannels - 1;

        /// <summary>
        /// The index of the <see cref="Alpha"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int AlphaIndex = 3;
        /// <summary>
        /// The index of the <see cref="Red"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int RedIndex = 2;
        /// <summary>
        /// The index of the <see cref="Green"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int GreenIndex = 1;
        /// <summary>
        /// The index of the <see cref="Blue"/> channel in the <see cref="ColorRgb"/> components array.
        /// This field is constant.
        /// </summary>
        private const int BlueIndex = 0;
        
        /// <summary>
        /// The minimum allowable difference between two values to consider a channel comparison of two
        /// <see cref="ColorRgb"/> channels as not equal.
        /// </summary>
        internal const float Tolerance = 1e-7f;

        /// <summary>
        /// The channels of the <see cref="ColorRgb"/> structure given as an array. The range of
        /// each channel is 0 to 1 inclusive. The size of this array is equal to
        /// <see cref="NumberOfChannels"/>.
        /// </summary>
        [FieldOffset(0)]
        private fixed float _channels[NumberOfChannels];

        /// <summary>
        /// The alpha channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        [FieldOffset(AlphaIndex * sizeof(float))]
        private float _alpha;
        /// <summary>
        /// The red channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        [FieldOffset(RedIndex * sizeof(float))]
        private float _red;
        /// <summary>
        /// The green channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        [FieldOffset(GreenIndex * sizeof(float))]
        private float _green;
        /// <summary>
        /// The blue channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        [FieldOffset(BlueIndex * sizeof(float))]
        private float _blue;

        /// <summary>
        /// Gets the alpha channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
            private set { _alpha = value; }
        }
        /// <summary>
        /// Gets the red channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        public float Red
        {
            get { return _red; }
            private set { _red = value; }
        }
        /// <summary>
        /// Gets the green channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        public float Green
        {
            get { return _green; }
            private set { _green = value; }
        }
        /// <summary>
        /// Gets the blue channel of this <see cref="ColorRgb"/> structure. The range of this
        /// channel is 0 to 1 inclusive.
        /// </summary>
        public float Blue
        {
            get { return _blue; }
            private set { _blue = value; }
        }

        /// <summary>
        /// Gets or sets the channel of this <see cref="ColorRgb"/> structure at the specified
        /// index. The range of each channel is 0 to 1 inclusive.
        /// </summary>
        private float this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < NumberOfChannels);
                fixed(float* ptr = _channels)
                    return ptr[index];
            }
            set
            {
                Debug.Assert(index >= 0 && index < NumberOfChannels);
                fixed (float* ptr = _channels)
                    ptr[index] = value;
            }
        }

        /// <summary>
        /// Gets the value of the non-alpha channel of this <see cref="ColorRgb"/>
        /// structure with the highest value.
        /// </summary>
        internal float Max
        {
            get { return MathHelper.Max(Red, Green, Blue); }
        }
        /// <summary>
        /// Gets the value of the non-alpha channel of this <see cref="ColorRgb"/>
        /// structure with the lowest value.
        /// </summary>
        internal float Min
        {
            get { return MathHelper.Min(Red, Green, Blue); }
        }

        /// <summary>
        /// Gets the lightness of this <see cref="ColorRgb"/> structure.
        /// </summary>
        internal float Lightness
        {
            get { return (Max + Min) / 2; }
        }
        /// <summary>
        /// Gets the luma of this <see cref="ColorRgb"/> structure.
        /// </summary>
        internal float Luma
        {
            get { return ColorHcy.RedWeight * Red + ColorHcy.GreenWeight * Green + ColorHcy.BlueWeight * Blue; }
        }
        /// <summary>
        /// Gets the chroma of this <see cref="ColorRgb"/> structure.
        /// </summary>
        internal float Chroma
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRgb"/> structure using the given
        /// color channels and an alpha component of its maximum value, 1.
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
        /// <exception cref="ArgumentException">
        /// <paramref name="red"/> is NaN. -or-
        /// <paramref name="green"/> is NaN. -or-
        /// <paramref name="blue"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorRgb(float red, float green, float blue) :
            this(1, red, green, blue)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRgb"/> structure using the given
        /// color channels.
        /// </summary>
        /// <param name="alpha">
        /// The intensity of the <see cref="Alpha"/> channel.
        /// </param>
        /// <param name="red">
        /// The intensity of the <see cref="Red"/> channel.
        /// </param>
        /// <param name="green">
        /// The intensity of the <see cref="Green"/> channel.
        /// </param>
        /// <param name="blue">
        /// The intensity of the <see cref="Blue"/> channel.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="alpha"/> is NaN. -or-
        /// <paramref name="red"/> is NaN. -or-
        /// <paramref name="green"/> is NaN. -or-
        /// <paramref name="blue"/> is Nan.
        /// </exception>
        /// <remarks>
        /// The channel parameters can be any real value. If the channel value is less than 0,
        /// or near it to within <see cref="Tolerance"/>, it is converted to zero. The value
        /// is similarly converted to 1 if it is near or exceeds it.
        /// </remarks>
        public ColorRgb(float alpha, float red, float green, float blue) : this()
        {
            if (Single.IsNaN(alpha))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(alpha)), nameof(alpha));
            if (Single.IsNaN(red))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(red)), nameof(red));
            if (Single.IsNaN(green))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(green)), nameof(green));
            if (Single.IsNaN(blue))
                throw new ArgumentException(SR.ErrorValueIsNaN(nameof(blue)), nameof(blue));

            Alpha = MathHelper.Clamp(alpha, 0, 1, Tolerance);
            Red = MathHelper.Clamp(red, 0, 1, Tolerance);
            Green = MathHelper.Clamp(green, 0, 1, Tolerance);
            Blue = MathHelper.Clamp(blue, 0, 1, Tolerance);
        }

        public ColorRgb AverageWith(ColorRgb color)
        {
            return AverageWith(color, 0.5f);
        }

        public ColorRgb AverageWith(ColorRgb color, float weight)
        {
            return AverageWith(color, new ColorWeight(weight, weight, weight));
        }

        public ColorRgb AverageWith(ColorRgb color, ColorWeight weight)
        {
            return new ColorRgb(color.Alpha,
                weight.Red * color.Red + (1 - weight.Red) * Red,
                weight.Green * color.Green + (1 - weight.Green) * Green,
                weight.Blue * color.Blue + (1 - weight.Blue) * Blue);
        }

        public ColorRgb Grayscale()
        {
            return Grayscale(ColorWeight.Balanced);
        }
        public ColorRgb LumaGrayscale()
        {
            return Grayscale(ColorWeight.Luma);
        }
        public ColorRgb Grayscale(ColorWeight weight)
        {
            var value = Red * weight.Red + Green * weight.Green + Blue * weight.Blue;
            return new ColorRgb(Alpha, value, value, value);            
        }
        public ColorRgb Invert()
        {
            return new ColorRgb(Alpha, 1 - Red, 1 - Green, 1 - Blue);
        }

        public ColorRgb BlendWith(ColorRgb bottom, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Multiply:
                    return Multiply(bottom);
                case BlendMode.Screen:
                    return ScreenBlend(bottom);
                case BlendMode.Overlay:
                    return Overlay(bottom);
                case BlendMode.HardLight:
                    return HardLightBlend(bottom);
                case BlendMode.SoftLight:
                    return SoftLightBlend(bottom);
                case BlendMode.ColorDodge:
                    return ColorDodge(bottom);
                case BlendMode.LinearDodge:
                    return LinearDodge(bottom);
                case BlendMode.ColorBurn:
                    return ColorBurn(bottom);
                case BlendMode.LinearBurn:
                    return LinearBurn(bottom);
                case BlendMode.Difference:
                    return Difference(bottom);
                case BlendMode.Darken:
                    return Darken(bottom);
                case BlendMode.Lighten:
                    return Lighten(bottom);
                case BlendMode.VividLight:
                    return VividLightBlend(bottom);
                case BlendMode.LinearLight:
                    return LinearLightBlend(bottom);
                case BlendMode.DarkerColor:
                    return DarkerColorBlend(bottom);
                case BlendMode.LighterColor:
                    return LighterColorBlend(bottom);
                case BlendMode.Hue:
                    return HueBlend(bottom);
                case BlendMode.Saturation:
                    return SaturationBlend(bottom);
                case BlendMode.Luminosity:
                    return LuminosityBlend(bottom);
                case BlendMode.Divide:
                    return Divide(bottom);
                default:
                    return Empty;
            }
        }

        public static ColorRgb operator &(ColorRgb left, ColorRgb right)
        {
            var result = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                result[i] = left[i] + right[i] * left.Alpha;
            }
            result.Alpha = left.Alpha + right.Alpha * (1 - left.Alpha);
            return result;
        }

        public static ColorRgb operator /(ColorRgb left, ColorRgb right)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (left[i] == 0)
                    color[i] = 1;
                else
                    color[i] = Math.Min(right[i] / left[i], 1);
            }
            color.Alpha = left.Alpha;
            return color;
        }

        public ColorRgb Divide(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] == 0)
                    color[i] = 1;
                else
                    color[i] = Math.Min(bottom[i] / this[i], 1);
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb HueBlend(ColorRgb bottom)
        {
            ColorHcy _t = this;
            ColorHcy _b = bottom;
            return new ColorHcy(_t.Hue, _b.Chroma, _b.Luma);
        }

        public ColorRgb SaturationBlend(ColorRgb bottom)
        {
            ColorHcy _t = this;
            ColorHcy _b = bottom;
            return new ColorHcy(_b.Hue, _t.Chroma, _b.Luma);
        }

        public ColorRgb LuminosityBlend(ColorRgb bottom)
        {
            ColorHsl bhsl = this;
            ColorHsl thsl = bottom;
            return new ColorHsl(thsl.Hue, bhsl.Saturation, bhsl.Lightness);
        }

        public ColorRgb DarkerColorBlend(ColorRgb bottom)
        {
            return Luma < bottom.Luma ? this : bottom;
        }

        public ColorRgb LighterColorBlend(ColorRgb bottom)
        {
            return Luma > bottom.Luma ? this : bottom;
        }

        public ColorRgb Multiply(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
                color[i] = this[i] * bottom[i];
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb ScreenBlend(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
                color[i] = 1 - (1 - this[i] ) * (1 - bottom[i]);
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb Overlay(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] < 0.5f)
                    color[i] = 2 * bottom[i] * this[i];
                else
                    color[i] = 1 - 2 * (1 - bottom[i]) * (1 - this[i]);
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb HardLightBlend(ColorRgb bottom)
        {
            return bottom.Overlay(this);
        }

        public ColorRgb SoftLightBlend(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (bottom[i] < 0.5f)
                    color[i] = (2 * bottom[i] * this[i]) + (bottom[i] * bottom[i] * (1 - 2 * this[i]));
                else
                    color[i] = (2 * bottom[i] * (1 - this[i])) + ((float)Math.Sqrt(bottom[i]) * (2 * this[i] - 1));
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb ColorDodge(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] < 1)
                    color[i] = Math.Min(bottom[i] / (1 - this[i]), 1);
                else
                    color[i] = 1;
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb LinearDodge(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Min(this[i] + bottom[i], 1);
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb ColorBurn(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > 0)
                    color[i] = Math.Max(1 - ((1 - bottom[i]) / this[i]), 0);
                else
                    color[i] = 0;
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb LinearBurn(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Max(this[i] + bottom[i] - 1, 0);
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb Difference(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
            {
                if (this[i] > bottom[i])
                    color[i] = this[i] - bottom[i];
                else
                    color[i] = bottom[i] - this[i];
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb Darken(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Min(this[i], bottom[i]);
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb Lighten(ColorRgb bottom)
        {
            var color = Empty;
            for (int i = NumberOfColorChannels; --i >= 0;)
                color[i] = Math.Max(this[i], bottom[i]);
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb VividLightBlend(ColorRgb bottom)
        {
            var color = Empty;

            for (int i = NumberOfColorChannels; --i >= 0; )
            {
                if (this[i] > 0.5f)
                {
                    if (this[i] < 1)
                        color[i] = Math.Min(bottom[i] / (1 - this[i]), 1);
                    else
                        color[i] = 1;
                }
                else if (this[i] > 0)
                    color[i] = Math.Max(1 - (1 - bottom[i]) / this[i], 0);
                else
                    color[i] = 0;
            }
            color.Alpha = Alpha;
            return color;
        }

        public ColorRgb LinearLightBlend(ColorRgb bottom)
        {
            var color = Empty;

            for (int i = NumberOfColorChannels; --i >= 0; )
            {
                color[i] = MathHelper.Clamp(2 * this[i] + bottom[i] - 1, 0, 1);
            }
            color.Alpha = Alpha;
            return color;
        }

        /// <summary>
        /// Converts a <see cref="ColorRgb"/> structure to a <see cref="Color"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorRgb"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/> the results from the conversion.
        /// </returns>
        public static explicit operator Color(ColorRgb color)
        {
            var scale = (float)Byte.MaxValue;
            return Color.FromArgb(
                (int)((scale * color.Alpha) + 0.5f),
                (int)((scale * color.Red) + 0.5f),
                (int)((scale * color.Green) + 0.5f),
                (int)((scale * color.Blue) + 0.5f));
        }
        /// <summary>
        /// Converts a <see cref="Color"/> structure to a <see cref="ColorRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="Color"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorRgb"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorRgb(Color color)
        {
            var scale = 1 / (float)Byte.MaxValue;
            return new ColorRgb(
                scale * color.A,
                scale * color.R,
                scale * color.G,
                scale * color.B);
        }

        /// <summary>
        /// Converts a <see cref="ColorCmy"/> structure to a <see cref="ColorRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorRgb"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorRgb(ColorCmy color)
        {
            return new ColorRgb(
                color.Alpha,
                1 - color.Cyan,
                1 - color.Magenta,
                1 - color.Yellow);
        }
        /// <summary>
        /// Converts a <see cref="ColorCmyk"/> structure to a <see cref="ColorRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorCmyk"/> to be converted.
        /// </param> 
        /// <returns>
        /// The <see cref="ColorRgb"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorRgb(ColorCmyk color)
        {
            var white = 1 - color.Key;
            return new ColorRgb(
                color.Alpha,
                white * (1 - color.Cyan),
                white * (1 - color.Magenta),
                white * (1 - color.Yellow));
        }
        /// <summary>
        /// Converts a <see cref="ColorHcy"/> structure to a <see cref="ColorRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHcy"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorRgb"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorRgb(ColorHcy color)
        {
            var hue = color.Hue * 6;
            var r = 0f;
            var g = 0f;
            var b = 0f;

            if (color.Chroma > 0)
            {
                if (hue >= 0 && hue < 1)
                {
                    r = color.Chroma;
                    g = color.Chroma * hue;
                }
                else if (hue >= 1 && hue < 2)
                {
                    r = color.Chroma * (2 - hue);
                    g = color.Chroma;
                }
                else if (hue >= 2 && hue < 3)
                {
                    g = color.Chroma;
                    b = color.Chroma * (hue - 2);
                }
                else if (hue >= 3 && hue < 4)
                {
                    g = color.Chroma * (4 - hue);
                    b = color.Chroma;
                }
                else if (hue >= 4 && hue < 5)
                {
                    r = color.Chroma * (hue - 4);
                    b = color.Chroma;
                }
                else //if (hue >= 5 && hue < 6)
                {
                    r = color.Chroma;
                    b = color.Chroma * (6 - hue);
                }
            }

            var match = color.Luma - (ColorHcy.RedWeight * r + ColorHcy.GreenWeight * g + ColorHcy.BlueWeight * b);
            return new ColorRgb(color.Alpha, r + match, g + match, b + match);
        }
        /// <summary>
        /// Converts a <see cref="ColorHsl"/> structure to a <see cref="ColorRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsl"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorRgb"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorRgb(ColorHsl color)
        {
            var chroma = (1 - Math.Abs(2 * color.Lightness - 1)) * color.Saturation;
            var hue = color.Hue * 6;
            var r = 0f;
            var g = 0f;
            var b = 0f;

            if (chroma > 0)
            {
                if (hue >= 0 && hue < 1)
                {
                    r = chroma;
                    g = chroma * hue;
                }
                else if (hue >= 1 && hue < 2)
                {
                    r = chroma * (2 - hue);
                    g = chroma;
                }
                else if (hue >= 2 && hue < 3)
                {
                    g = chroma;
                    b = chroma * (hue - 2);
                }
                else if (hue >= 3 && hue < 4)
                {
                    g = chroma * (4 - hue);
                    b = chroma;
                }
                else if (hue >= 4 && hue < 5)
                {
                    r = chroma * (hue - 4);
                    b = chroma;
                }
                else //if (hue >= 5 && hue < 6)
                {
                    r = chroma;
                    b = chroma * (6 - hue);
                }
            }

            var match = color.Lightness - 0.5f * chroma;
            return new ColorRgb(color.Alpha, r + match, g + match, b + match);
        }
        /// <summary>
        /// Converts a <see cref="ColorHsv"/> structure to a <see cref="ColorRgb"/>
        /// structure.
        /// </summary>
        /// <param name="color">
        /// The <see cref="ColorHsv"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="ColorRgb"/> the results from the conversion.
        /// </returns>
        public static implicit operator ColorRgb(ColorHsv color)
        {
            var chroma = color.Value * color.Saturation;
            var hue = color.Hue * 6;
            var r = 0f;
            var g = 0f;
            var b = 0f;

            if (chroma > 0)
            {
                if (hue >= 0 && hue < 1)
                {
                    r = chroma;
                    g = chroma * hue;
                }
                else if (hue >= 1 && hue < 2)
                {
                    r = chroma * (2 - hue);
                    g = chroma;
                }
                else if (hue >= 2 && hue < 3)
                {
                    g = chroma;
                    b = chroma * (hue - 2);
                }
                else if (hue >= 3 && hue < 4)
                {
                    g = chroma * (4 - hue);
                    b = chroma;
                }
                else if (hue >= 4 && hue < 5)
                {
                    r = chroma * (hue - 4);
                    b = chroma;
                }
                else //if (hue >= 5 && hue < 6)
                {
                    r = chroma;
                    b = chroma * (6 - hue);
                }
            }

            var match = color.Value - chroma;
            return new ColorRgb(color.Alpha, r + match, b + match, g + match);
        }

        /// <summary>
        /// Compares two <see cref="ColorRgb"/> objects. The result specifies whether
        /// the <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels are all equal within <see cref="Tolerance"/>.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/> have equal
        /// <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels within <see cref="Tolerance"/>; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, ColorRgb right)
        {
            for (int i = NumberOfChannels; --i >= 0;)
                if (!MathHelper.NearlyEquals(left[i], right[i], Tolerance))
                    return false;
            return true;
        }
        /// <summary>
        /// Compares two <see cref="ColorRgb"/> objects. The result specifies whether
        /// the <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, and
        /// <see cref="Blue"/> channels are all unequal within <see cref="Tolerance"/>.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/> have unequal
        /// <see cref="Alpha"/>, <see cref="Red"/>, <see cref="Green"/>, or
        /// <see cref="Blue"/> channels within <see cref="Tolerance"/>; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, ColorRgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, Color right)
        {
            return (Color)left == right;
        }
        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="Color"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, Color right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to equal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(Color left, ColorRgb right)
        {
            return right == left;
        }
        /// <summary>
        /// Compares a <see cref="Color"/> object to a
        /// <see cref="ColorRgb"/> object. The result specifies whether
        /// they convert to unequal <see cref="Color"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="Color"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="Color"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(Color left, ColorRgb right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, ColorCmy right)
        {
            return left == (ColorRgb)right;
        }
        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorCmy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, ColorCmy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, ColorCmyk right)
        {
            return left == (ColorRgb)right;
        }
        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorCmyk"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorCmyk"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, ColorCmyk right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, ColorHcy right)
        {
            return left == (ColorRgb)right;
        }
        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorHcy"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHcy"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, ColorHcy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, ColorHsl right)
        {
            return left == (ColorRgb)right;
        }
        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorHsl"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsl"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, ColorHsl right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to equal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are equal; otherwise false.
        /// </returns>
        public static bool operator ==(ColorRgb left, ColorHsv right)
        {
            return left == (ColorRgb)right;
        }
        /// <summary>
        /// Compares a <see cref="ColorRgb"/> object to a
        /// <see cref="ColorHsv"/> object. The result specifies whether
        /// they convert to unequal <see cref="ColorRgb"/> values.
        /// </summary>
        /// <param name="left">
        /// A <see cref="ColorRgb"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ColorHsv"/> to compare.
        /// </param>
        /// <returns>
        /// true if <paramref name="left"/> and <paramref name="right"/>, when
        /// converted to <see cref="ColorRgb"/>, are unequal; otherwise false.
        /// </returns>
        public static bool operator !=(ColorRgb left, ColorHsv right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="ColorRgb"/> is the same color as
        /// the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj"/> is the same color as this <see cref="ColorRgb"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorRgb))
                return false;

            return (ColorRgb)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="ColorRgb"/>.
        /// </summary>
        /// <returns>
        /// An integer value that specifies a hash value for this <see cref="ColorRgb"/>.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="ColorRgb"/> is equal to the hash code for its
        /// <see cref="Color"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            return ((Color)this).GetHashCode();
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
            sb.Append(nameof(Alpha));
            sb.Append(": ");
            sb.Append(SR.GetString(Alpha));
            sb.Append(", ");
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