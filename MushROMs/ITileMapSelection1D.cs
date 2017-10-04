namespace MushROMs
{
    public delegate void TileMethod1D(int index);

    public interface ITileMapSelection1D
    {
        int StartIndex
        {
            get;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
        int NumTiles
        {
            get;
        }

        void IterateIndexes(TileMethod1D method);
        int[] GetSelectedIndexes();
        bool ContainsIndex(int index);

        ITileMapSelection1D Copy(int startIndex);

        ITileMapSelection1D LogicalAnd(ITileMapSelection1D value);
        ITileMapSelection1D LogicalOr(ITileMapSelection1D value);
        ITileMapSelection1D LogicalXor(ITileMapSelection1D value);
        ITileMapSelection1D LogicalNegate(ITileMapSelection1D value);
    }
}