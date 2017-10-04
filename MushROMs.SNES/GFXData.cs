using System;

namespace MushROMs.SNES
{
    public sealed class GFXData : ISelectionData
    {
        private GFXTile[] Data
        {
            get;
            set;
        }

        public GraphicsFormat GraphicsFormat
        {
            get;
            private set;
        }
        public int TileDataSize
        {
            get { return GFXTile.GetTileDataSize(GraphicsFormat); }
        }
        public int BitsPerPixel
        {
            get { return GFXTile.GetBitsPerPixel(GraphicsFormat); }
        }
        public int ColorsPerPixel
        {
            get { return GFXTile.GetColorsPerPixel(GraphicsFormat); }
        }

        public GFXSelection Selection
        {
            get;
            private set;
        }

        public GFXData(GFXEditor editor, GFXSelection selection)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            Selection = selection;
            GraphicsFormat = editor.GraphicsFormat;
            Data = new GFXTile[Selection.NumTiles];

            var startIndex = Selection.StartIndex;
            var data = editor.GetData();
            var length = editor.GetData().Length;

            unsafe
            {
                fixed (int* indexes = Selection.TileMapSelection.GetSelectedIndexes())
                fixed (byte* src = &editor.GetData()[Selection.StartAddress])
                fixed (GFXTile* dest = Data)
                {
                    for (int i = Selection.NumTiles; --i >= 0;)
                    {
                        var address = (indexes[i] + startIndex) * TileDataSize;
                        if (address + TileDataSize <= length)
                                dest[i].GetTileData(src + address, GraphicsFormat);
                    }
                }
            }
        }

        private GFXData(GFXData data, GFXSelection selection)
        {
            Data = new GFXTile[data.Data.Length];
            Array.Copy(data.Data, Data, Data.Length);

            var tilemap = data.Selection.TileMapSelection.Copy(selection.StartIndex);
            Selection = new GFXSelection(data.Selection.StartAddress, tilemap);
        }

        public GFXTile[] GetData()
        {
            return Data;
        }

        public GFXData Copy(GFXSelection selection)
        {
            return new GFXData(this, selection);
        }

        public void WriteToEditor(GFXEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            var startIndex = Selection.StartIndex;
            var length = editor.GetData().Length;

            unsafe
            {
                fixed (int* indexes = Selection.TileMapSelection.GetSelectedIndexes())
                fixed (byte* dest = editor.GetData())
                fixed (GFXTile* src = Data)
                {
                    for (int i = Selection.NumTiles; --i >= 0;)
                    {
                        int srcAddress = i * TileDataSize;
                        int destAddress = GetAddress(indexes[i] + startIndex);
                        //FINISH
                    }
                }
            }
        }

        private int GetAddress(int index)
        {
            return Selection.StartAddress + TileDataSize * index;
        }

        ISelection ISelectionData.Selection
        {
            get { return Selection; }
        }

        ISelectionData ISelectionData.Copy(ISelection selection)
        {
            return Copy((GFXSelection)selection);
        }

        void ISelectionData.WriteToEditor(IEditor editor)
        {
            WriteToEditor((GFXEditor)editor);
        }
    }
}
