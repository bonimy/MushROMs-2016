using System;

namespace MushROMs
{
    public sealed class TileMapEmptySelection1D : TileMapSelection1D
    {
        public override int NumTiles
        {
            get { return 0; }
        }

        internal TileMapEmptySelection1D()
        {
            StartIndex = -1;
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
        }

        public override bool ContainsIndex(int index)
        {
            return false;
        }

        protected override int[] InitializeSelectedIndexes()
        {
            return new int[0];
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            return new TileMapEmptySelection1D();
        }
    }
}
