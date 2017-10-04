using System;

namespace MushROMs.SNES
{
    public sealed class GFXSelection : ISelection
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

        public GFXSelection(int startAddress, ITileMapSelection1D selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            StartAddress = startAddress;
            TileMapSelection = selection;
        }

        public GFXData GetEditorData(GFXEditor editor)
        {
            return new GFXData(editor, this);
        }

        ISelectionData ISelection.GetSelectionData(IEditor editor)
        {
            return GetEditorData((GFXEditor)editor);
        }
    }
}
