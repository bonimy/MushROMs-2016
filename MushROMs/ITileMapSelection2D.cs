using Helper;

namespace MushROMs
{
    public delegate void TileMethod2D(Position index);

    public interface ITileMapSelection2D
    {
        Position StartIndex
        {
            get;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
        int NumTiles
        {
            get;
        }

        void IterateIndexes(TileMethod2D method);
        Position[] GetSelectedIndexes();
        bool ContainsIndex(Position index);

        ITileMapSelection2D Copy(Position startIndex);

        ITileMapSelection2D LogicalAnd(ITileMapSelection2D value);
        ITileMapSelection2D LogicalOr(ITileMapSelection2D value);
        ITileMapSelection2D LogicalXor(ITileMapSelection2D value);
        ITileMapSelection2D LogicalNegate(ITileMapSelection2D value);
    }
}