using System.Text;

namespace Helper.ColorSpaces
{
    public struct Colorizer
    {
        public int Hue
        {
            get;
            private set;
        }

        public int Saturation
        {
            get;
            private set;
        }

        public int Lightness
        {
            get;
            private set;
        }

        public int Effectiveness
        {
            get;
            private set;
        }

        public ColorizerMode ColorizerMode
        {
            get;
            private set;
        }

        public Colorizer(int hue, int saturation, int lightness, int effectiveness, ColorizerMode colorizerMode)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
            Effectiveness = effectiveness;
            ColorizerMode = colorizerMode;
        }

        public static bool operator ==(Colorizer left, Colorizer right)
        {
            return left.Hue == right.Hue &&
                left.Saturation == right.Saturation &&
                left.Lightness == right.Lightness &&
                left.Effectiveness == right.Effectiveness &&
                left.ColorizerMode == right.ColorizerMode;
        }

        public static bool operator !=(Colorizer left, Colorizer right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Colorizer))
            {
                return false;
            }

            return (Colorizer)obj == this;
        }

        public override int GetHashCode()
        {
            return Hash.Generate(Hue, Saturation, Lightness, Effectiveness, (int)ColorizerMode);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
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
            sb.Append(", ");
            sb.Append(nameof(Effectiveness));
            sb.Append(": ");
            sb.Append(SR.GetString(Effectiveness));
            sb.Append(", ");
            sb.Append(nameof(ColorizerMode));
            sb.Append(": ");
            sb.Append(SR.GetString(ColorizerMode));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
