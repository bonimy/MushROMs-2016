using Helper;

namespace MushROMs
{
    public abstract class TileMapSelection2D : ITileMapSelection2D
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TileMapSelection2D Empty = new TileMapEmptySelection2D();

        public Position StartIndex
        {
            get;
            protected set;
        }

        public abstract int NumTiles
        {
            get;
        }

        private Position[] SelectedIndexes
        {
            get;
            set;
        }

        public Position[] GetSelectedIndexes()
        {
            if (SelectedIndexes == null)
            {
                SelectedIndexes = InitializeSelectedIndexes();
            }

            return SelectedIndexes;
        }

        public abstract void IterateIndexes(TileMethod2D method);

        protected abstract Position[] InitializeSelectedIndexes();

        public abstract bool ContainsIndex(Position index);

        public abstract ITileMapSelection2D Copy(Position startIndex);

        public ITileMapSelection2D LogicalAnd(ITileMapSelection2D value)
        {
            return new TileMapGateSelection2D(this, value, (left, right) => left & right);
        }

        public ITileMapSelection2D LogicalOr(ITileMapSelection2D value)
        {
            return new TileMapGateSelection2D(this, value, (left, right) => left | right);
        }

        public ITileMapSelection2D LogicalXor(ITileMapSelection2D value)
        {
            return new TileMapGateSelection2D(this, value, (left, right) => left ^ right);
        }

        public ITileMapSelection2D LogicalNegate(ITileMapSelection2D value)
        {
            return new TileMapGateSelection2D(this, value, (left, right) => left & !right);
        }
    }
}
