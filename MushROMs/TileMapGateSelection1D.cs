using System;
using System.Collections.Generic;

namespace MushROMs
{
    public delegate bool GateMethod(bool left, bool right);

    public sealed class TileMapGateSelection1D : TileMapSelection1D
    {
        public ITileMapSelection1D Left
        {
            get;
            private set;
        }

        public ITileMapSelection1D Right
        {
            get;
            private set;
        }

        public GateMethod Rule
        {
            get;
            private set;
        }

        public override int NumTiles
        {
            get { return GetSelectedIndexes().Length; }
        }

        public TileMapGateSelection1D(ITileMapSelection1D left, ITileMapSelection1D right, GateMethod rule)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.StartIndex == Empty.StartIndex)
            {
                StartIndex = right.StartIndex;
            }
            else if (right.StartIndex == Empty.StartIndex)
            {
                StartIndex = left.StartIndex;
            }
            else
            {
                StartIndex = Math.Min(left.StartIndex, right.StartIndex);
            }

            Left = left;
            Right = right;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var indexes = GetSelectedIndexes();
            unsafe
            {
                fixed (int* src = indexes)
                {
                    for (var i = indexes.Length; --i >= 0;)
                    {
                        method(src[i] + StartIndex);
                    }
                }
            }
        }

        public override bool ContainsIndex(int index)
        {
            return Rule(Left.ContainsIndex(index), Right.ContainsIndex(index));
        }

        protected override int[] InitializeSelectedIndexes()
        {
            var lIndexes = Left.GetSelectedIndexes();
            var rIndexes = Right.GetSelectedIndexes();
            var indexes = new List<int>();

            var lDelta = Left.StartIndex - StartIndex;
            var rDelta = Right.StartIndex - StartIndex;

            unsafe
            {
                fixed (int* lPtr = lIndexes)
                fixed (int* rPtr = rIndexes)
                {
                    for (var i = lIndexes.Length; --i >= 0;)
                    {
                        var lIndex = lPtr[i] + lDelta;
                        if (Rule(true, Right.ContainsIndex(lIndex)))
                        {
                            indexes.Add(lIndex);
                        }
                    }

                    for (var i = rIndexes.Length; --i >= 0;)
                    {
                        var rIndex = rPtr[i] + rDelta;
                        if (Rule(Left.ContainsIndex(rIndex), true) && !indexes.Contains(rIndex - StartIndex))
                        {
                            indexes.Add(rIndex);
                        }
                    }
                }
            }

            return indexes.ToArray();
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            var lDelta = Left.StartIndex - StartIndex;
            var rDelta = Right.StartIndex - StartIndex;
            return new TileMapGateSelection1D(Left.Copy(startIndex + lDelta), Right.Copy(startIndex + rDelta), Rule);
        }
    }
}
