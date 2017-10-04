using System;

namespace MushROMs.SNES
{
    public class PaletteSelection : ISelection
    {
        public ITileMapSelection1D TileMapSelection
        {
            get;
            private set;
        }

        public int StartAddress
        {
            get;
            private set;
        }
        public int StartIndex
        {
            get { return TileMapSelection.StartIndex; }
        }
        public int NumTiles
        {
            get { return TileMapSelection.NumTiles; }
        }

        public PaletteSelection(int startAddress, ITileMapSelection1D selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            StartAddress = startAddress;
            TileMapSelection = selection;
        }
        internal PaletteSelection(int startAddress, int index, GraphicsFormat graphicsFormat)
        {
            StartAddress = startAddress;
            TileMapSelection = new TileMapLineSelection1D(index,
                index + GFXTile.GetColorsPerPixel(graphicsFormat) - 1);
        }

        public PaletteData GetPaletteData(PaletteEditor editor)
        {
            return new PaletteData(editor, this);
        }

        ISelectionData ISelection.GetSelectionData(IEditor editor)
        {
            return GetPaletteData((PaletteEditor)editor);
        }
    }
}