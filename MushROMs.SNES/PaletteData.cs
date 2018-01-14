using System;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public sealed class PaletteData : ISelectionData
    {
        private Color15BppBgr[] Data
        {
            get;
            set;
        }

        public PaletteSelection Selection
        {
            get;
            private set;
        }

        public PaletteData(Palette palette, PaletteSelection selection)
        {
            if (palette == null)
            {
                throw new ArgumentNullException(nameof(palette));
            }

            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            Data = new Color15BppBgr[Selection.NumTiles];
            var startIndex = Selection.StartIndex;
            var data = palette.GetData();
            var length = data.Length;

            unsafe
            {
                fixed (int* indexes = Selection.TileMapSelection.GetSelectedIndexes())
                fixed (byte* ptr = &data[Selection.StartAddress])
                fixed (Color15BppBgr* dest = Data)
                {
                    var src = (Color15BppBgr*)ptr + startIndex;
                    for (var i = Selection.NumTiles; --i >= 0;)
                    {
                        if (indexes[i] + startIndex < length)
                        {
                            dest[i] = src[indexes[i]];
                        }
                    }
                }
            }
        }

        private PaletteData(PaletteData data, PaletteSelection selection)
        {
            Data = new Color15BppBgr[data.Data.Length];
            Array.Copy(data.Data, Data, Data.Length);

            var tilemap = data.Selection.TileMapSelection.Copy(selection.StartIndex);
            Selection = new PaletteSelection(data.Selection.StartAddress, tilemap);
        }

        public Color15BppBgr[] GetData()
        {
            return Data;
        }

        public PaletteData Copy(PaletteSelection selection)
        {
            return new PaletteData(this, selection);
        }

        public void WriteToEditor(PaletteEditor editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            var startIndex = Selection.StartIndex;
            var length = Palette.GetIndexFromAddress(editor.Palette.GetData().Length);

            unsafe
            {
                fixed (int* indexes = Selection.TileMapSelection.GetSelectedIndexes())
                fixed (byte* ptr = &editor.Palette.GetData()[Selection.StartAddress])
                fixed (Color15BppBgr* src = Data)
                {
                    var dest = (Color15BppBgr*)ptr + startIndex;
                    for (var i = Selection.NumTiles; --i >= 0;)
                    {
                        if (indexes[i] + startIndex < length)
                        {
                            dest[indexes[i]] = src[i];
                        }
                    }
                }
            }
        }

        ISelection ISelectionData.Selection
        {
            get { return Selection; }
        }

        ISelectionData ISelectionData.Copy(ISelection selection)
        {
            return Copy((PaletteSelection)selection);
        }

        void ISelectionData.WriteToEditor(IEditor editor)
        {
            WriteToEditor((PaletteEditor)editor);
        }
    }
}
