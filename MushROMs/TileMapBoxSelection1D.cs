﻿using System;
using Helper;

namespace MushROMs
{
    public sealed class TileMapBoxSelection1D : TileMapSelection1D
    {
        public override int NumTiles
        {
            get { return Range.Area; }
        }

        public int ViewWidth
        {
            get;
            private set;
        }

        public Range Range
        {
            get;
            private set;
        }

        private TileMapBoxSelection1D(int startIndex, int viewWidth, Range range)
        {
            if (viewWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(viewWidth),
                    SR.ErrorInvalidOpenLowerBound(nameof(viewWidth), viewWidth, 0));
            }

            if (range.Horizontal <= 0 || range.Vertical <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(range),
                    SR.ErrorInvalidOpenLowerBound(nameof(range), range, 0));
            }

            StartIndex = startIndex;
            ViewWidth = viewWidth;
            Range = range;
        }

        public TileMapBoxSelection1D(int viewWidth, int zeroIndex, Position viewPoint1, Position viewPoint2)
        {
            if (viewWidth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(viewWidth),
                    SR.ErrorInvalidClosedLowerBound(nameof(viewWidth), viewWidth, 0));
            }

            ViewWidth = viewWidth;

            var min = Position.TopLeft(viewPoint1, viewPoint2);
            var max = Position.BottomRight(viewPoint1, viewPoint2);

            StartIndex = TileMap1D.GetGridTile(min, ViewWidth, zeroIndex);
            Range = max - min + new Position(1, 1);
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            for (var y = Range.Vertical; --y >= 0;)
            {
                for (var x = Range.Horizontal; --x >= 0;)
                {
                    method(TileMap1D.GetGridTile(x, y, ViewWidth, StartIndex));
                }
            }
        }

        public override bool ContainsIndex(int index)
        {
            return Range.Contains(TileMap1D.GetViewTile(index, ViewWidth, StartIndex));
        }

        protected override int[] InitializeSelectedIndexes()
        {
            var indexes = new int[Range.Horizontal * Range.Vertical];
            for (var y = Range.Vertical; --y >= 0;)
            {
                for (var x = Range.Horizontal; --x >= 0;)
                {
                    indexes[(y * Range.Horizontal) + x] =
                        TileMap1D.GetGridTile(x, y, ViewWidth);
                }
            }

            return indexes;
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            return new TileMapBoxSelection1D(startIndex, ViewWidth, Range);
        }
    }
}
