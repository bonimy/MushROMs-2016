using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    [Serializable]
    public class DirectWriter
    {
        public int Address
        {
            get;
            set;
        }

        public bool Resolved
        {
            get { return Address == 0; }
        }

        public int Size
        {
            get { return Data.Count; }
        }

        internal List<byte> Data
        {
            get;
            private set;
        }

        public DirectWriter(int address)
        {
            Address = address;
            Data = new List<byte>();
        }
    }
}
