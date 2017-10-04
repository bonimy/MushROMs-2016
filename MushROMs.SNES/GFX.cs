using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.SNES
{
    public class GFX
    {
        private const GraphicsFormat FallbackGraphicsFormat = GraphicsFormat.Format1Bpp8x8;
        
        private int _startAddress;
        private GraphicsFormat _graphicsFormat;

        public event EventHandler StartAddressChanged;
        public event EventHandler GraphicsFormatChanged;

        private byte[] Data
        {
            get;
            set;
        }

        public int StartAddress
        {
            get { return _startAddress; }
            private set
            {
                if (StartAddress == value)
                    return;

                _startAddress = value;
                OnStartAddressChanged(EventArgs.Empty);
            }
        }

        public GraphicsFormat GraphicsFormat
        {
            get { return _graphicsFormat; }
            set
            {
                if (GraphicsFormat == value)
                    return;

                _graphicsFormat = value;
                OnGraphicsFormatChanged(EventArgs.Empty);
            }
        }
        public int TileDataSize
        {
            get { return GFXTile.GetTileDataSize(GraphicsFormat); }
        }
        public int BitsPerPixel
        {
            get { return GFXTile.GetBitsPerPixel(GraphicsFormat); }
        }
        public int ColorsPerPixel
        {
            get { return GFXTile.GetColorsPerPixel(GraphicsFormat); }
        }

        public GFX(int numTiles, GraphicsFormat graphicsFormat)
        {
            Data = new byte[numTiles * GFXTile.GetTileDataSize(graphicsFormat)];
        }

        public GFX(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (!CHRFile.IsValidData(data))
                throw new ArgumentException(nameof(data));

            Data = new byte[data.Length];
            Array.Copy(data, Data, Data.Length);
        }

        public byte[] GetData()
        {
            return Data;
        }

        protected virtual void OnStartAddressChanged(EventArgs e)
        {
            StartAddressChanged?.Invoke(this, e);
        }

        protected virtual void OnGraphicsFormatChanged(EventArgs e)
        {
            GraphicsFormatChanged?.Invoke(this, e);
        }

        public int GetAddressFromIndex(int index)
        {
            return GetAddressFromIndex(index, StartAddress, GraphicsFormat);
        }
        public static int GetAddressFromIndex(int index, int startAddress, GraphicsFormat format)
        {
            return (index * GFXTile.GetTileDataSize(format)) + startAddress;
        }
        public static int GetIndexFromAddress(int address, GraphicsFormat format)
        {
            return address / GFXTile.GetTileDataSize(format);
        }
    }
}
