using System;
using Helper;

namespace MushROMs
{
    public sealed class TileMapSingleSelection2D : TileMapSelection2D
    {
        public override int NumTiles
        {
            get { return 1; }
        }

        public TileMapSingleSelection2D(Position index)
        {
            if (index.X < 0 || index.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(index),
                    SR.ErrorInvalidClosedLowerBound(nameof(index), index, Position.Empty));
            StartIndex = index;
        }

        public override void IterateIndexes(TileMethod2D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            method(StartIndex);
        }

        public override bool ContainsIndex(Position index)
        {
            return index == StartIndex;
        }

        protected override Position[] InitializeSelectedIndexes()
        {
            return new Position[] { Position.Empty };
        }

        public override ITileMapSelection2D Copy(Position startIndex)
        {
            return new TileMapSingleSelection2D(startIndex);
        }
    }
}