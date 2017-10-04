using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.Assembler
{
    internal class FreeSpaceData
    {
        public int Size
        {
            get;
            private set;
        }

        private byte[] Data
        {
            get;
            set;
        }

        private struct LabelReference
        {
            public int RelativeAddress
            {
                get;
                private set;
            }


        }
    }
}
