using System;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public sealed class RPFSerializer : PaletteSerializer
    {
        public static bool IsValidSize(int length)
        {
            return length >= 0 && length % Color15BppBgr.SizeOf == 0;
        }

        public RPFSerializer(byte[] data) : base(data)
        {
        }

        public override bool IsValidByteData
        {
            get
            {
                if (Data == null)
                {
                    return false;
                }

                if (!IsValidSize(Data.Length))
                {
                    return false;
                }

                unsafe
                {
                    fixed (byte* ptr = Data)
                    {
                        for (var i = GetNumColorsFromSize(Data.Length); --i >= 0;)
                        {
                            if ((((Color15BppBgr*)ptr)[i] & 0x8000) != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        public override Color15BppBgr[] GetColors()
        {
            throw new NotImplementedException();
        }

        public override int GetNumColorsFromSize(int length)
        {
            return length / Color15BppBgr.SizeOf;
        }

        public override int GetSizeFromNumColors(int numColors)
        {
            return numColors * Color15BppBgr.SizeOf;
        }
    }
}
