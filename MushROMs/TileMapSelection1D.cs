namespace MushROMs
{
    public abstract class TileMapSelection1D : ITileMapSelection1D
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly TileMapSelection1D Empty = new TileMapEmptySelection1D();

        public int StartIndex
        {
            get;
            protected set;
        }

        public abstract int NumTiles
        {
            get;
        }

        private int[] SelectedIndexes
        {
            get;
            set;
        }

        public int[] GetSelectedIndexes()
        {
            if (SelectedIndexes == null)
            {
                SelectedIndexes = InitializeSelectedIndexes();
            }

            return SelectedIndexes;
        }

        public abstract void IterateIndexes(TileMethod1D method);

        protected abstract int[] InitializeSelectedIndexes();

        public abstract bool ContainsIndex(int index);

        public abstract ITileMapSelection1D Copy(int startIndex);

        public ITileMapSelection1D LogicalAnd(ITileMapSelection1D value)
        {
            return new TileMapGateSelection1D(this, value, (left, right) => left & right);
        }

        public ITileMapSelection1D LogicalOr(ITileMapSelection1D value)
        {
            return new TileMapGateSelection1D(this, value, (left, right) => left | right);
        }

        public ITileMapSelection1D LogicalXor(ITileMapSelection1D value)
        {
            return new TileMapGateSelection1D(this, value, (left, right) => left ^ right);
        }

        public ITileMapSelection1D LogicalNegate(ITileMapSelection1D value)
        {
            return new TileMapGateSelection1D(this, value, (left, right) => left & !right);
        }
    }
}
