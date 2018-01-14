using System;
using Helper;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public class Palette
    {
        private int _startAddress;

        public event EventHandler StartAddressChanged;

        private byte[] Data
        {
            get;
            set;
        }

        public int Length
        {
            get { return Data.Length; }
        }

        public int StartAddress
        {
            get
            {
                return _startAddress;
            }

            private set
            {
                if (StartAddress == value)
                {
                    return;
                }

                _startAddress = value;
                OnStartAddressChanged(EventArgs.Empty);
            }
        }

        public Palette(int numColors)
        {
            Data = new byte[numColors * Color15BppBgr.SizeOf];
        }

        public Palette(Color15BppBgr[] colors)
        {
            if (colors == null)
            {
                throw new ArgumentNullException(nameof(colors));
            }

            var data = new byte[colors.Length * Color15BppBgr.SizeOf];
            unsafe
            {
                fixed (byte* ptr = data)
                fixed (Color15BppBgr* src = colors)
                {
                    var dest = (Color15BppBgr*)ptr;
                    for (var i = colors.Length; --i >= 0;)
                    {
                        dest[i] = src[i];
                    }
                }
            }

            Data = data;
        }

        public byte[] GetData()
        {
            return Data;
        }

        public Color15BppBgr[] GetColors()
        {
            return RPFFile.GetColors(Data);
        }

        public Color15BppBgr GetColorAtAddress(int address)
        {
            if (address < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(address),
                    SR.ErrorInvalidClosedLowerBound(nameof(address), address, 0));
            }

            var max = Data.Length - Color15BppBgr.SizeOf;
            if (address > max)
            {
                throw new ArgumentOutOfRangeException(nameof(address),
                    SR.ErrorInvalidClosedUpperBound(nameof(address), address, max));
            }

            unsafe
            {
                fixed (byte* ptr = &Data[address])
                    return *(Color15BppBgr*)ptr;
            }
        }

        protected virtual void OnStartAddressChanged(EventArgs e)
        {
            StartAddressChanged?.Invoke(this, e);
        }

        public int GetAddressFromIndex(int index)
        {
            return GetAddressFromIndex(index, StartAddress);
        }

        public static int GetAddressFromIndex(int index, int startAddress)
        {
            return (index * Color15BppBgr.SizeOf) + startAddress;
        }

        public static int GetIndexFromAddress(int address)
        {
            return address / Color15BppBgr.SizeOf;
        }
    }
}
