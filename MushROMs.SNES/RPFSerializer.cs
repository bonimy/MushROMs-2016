/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public sealed class RPFSerializer : PaletteSerializer
    {
        public static bool IsValidSize(int length)
        {
            return length >= 0 && length % Color15BppBgr.SizeOf == 0;
        }

        public static bool IsValidData(byte[] data)
        {
            if (data == null)
                return false;
            if (!IsValidSize(data.Length))
                return false;

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    for (int i = GetNumColorsFromSize(data.Length); --i >= 0;)
                        if ((((Color15BppBgr*)ptr)[i] & 0x8000) != 0)
                            return false;
                }
            }
            return true;
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
*/
