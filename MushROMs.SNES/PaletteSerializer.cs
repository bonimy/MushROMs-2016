/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public abstract class PaletteSerializer : IDataSerializer
    {
        public abstract bool IsValidByteData { get; }

        public Palette Palette
        {
            get;
            private set;
        }

        internal byte[] Data
        {
            get;
            set;
        }

        protected PaletteSerializer(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Data = data;

            if (!IsValidByteData)
                throw new ArgumentException(nameof(data));

            Palette = new Palette(GetColors());
        }

        public abstract int GetNumColorsFromSize(int length);
        public abstract int GetSizeFromNumColors(int numColors);
        public abstract Color15BppBgr[] GetColors();
    }
}
*/
