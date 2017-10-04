using System;
using Debug = System.Diagnostics.Debug;
using System.Threading.Tasks;
using Helper;
using Helper.ColorSpaces;
using Helper.PixelFormats;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    public class PaletteEditor : Editor, ITileMapEditor1D
    {
        public static readonly EditorInfo EditorInfo =
            new EditorInfo(typeof(PaletteEditor), Resources.PaletteEditorName);

        public Palette Palette
        {
            get;
            private set;
        }

        public TileMap1D TileMap
        {
            get;
            private set;
        }

        public new PaletteSelection Selection
        {
            get { return (PaletteSelection)base.Selection; }
            set { base.Selection = value; }
        }

        public PaletteEditor(Palette palette)
        {
            if (palette == null)
                throw new ArgumentNullException(nameof(palette));
            Palette = palette;

            TileMap = new TileMap1D();
            TileMap.TileSize = 1;
            TileMap.ViewSize = 0x10;
            TileMap.ZoomSize = 0x10;
            TileMap.GridSize = Palette.Length / Color15BppBgr.SizeOf;
            TileMap.SelectionChanged += TileMap_SelectionChanged;
            TileMap.Selection = new TileMapSingleSelection1D(TileMap.ZeroTile);
        }

        public new PaletteData GetSelectionData()
        {
            return (PaletteData)base.GetSelectionData();
        }

        public override bool IsValidSelectionData(ISelectionData data)
        {
            return data is PaletteData;
        }

        public override void Save(string path)
        {
            var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            var fileAssociations = MasterEditor.GetFileAssociations();
            if (!fileAssociations.ContainsKey(ext))
                throw new FileFormatException(path);

            var fileAssociation = fileAssociations[ext];

            SetFileData(fileAssociation.SaveFileDataMethod(this));
            base.Save(path);
        }

        public void EditColor(Color15BppBgr color)
        {
            EditColor(Selection, color);
        }

        public void EditColor(PaletteSelection selection, Color15BppBgr color)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));
            if (!(selection.TileMapSelection is TileMapSingleSelection1D))
                throw new ArgumentException(nameof(selection), Resources.ErrorNotSingleSelection);

            var data = GetEditorData(selection);
            data.GetData()[0] = color;
            WriteData(data);
        }

        public void InvertColors()
        {
            InvertColors(Selection);
        }

        public void InvertColors(PaletteSelection selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
                src[i] ^= 0x7FFF;
            WriteData(data);
        }

        public void Blend(BlendMode blendMode, ColorRgb color)
        {
            Blend(Selection, blendMode, color);
        }

        public void Blend(PaletteSelection selection, BlendMode blendMode, ColorRgb color)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
            {
                var rgb = (ColorRgb)src[i];
                src[i] = (Color15BppBgr)rgb.BlendWith(color, blendMode);
            }
            WriteData(data);
        }

        public void Colorize(float hue, float saturation, float lightness, bool luma, float weight)
        {
            Colorize(Selection, hue, saturation, lightness, luma, weight);
        }

        public void Colorize(PaletteSelection selection, float hue, float saturation, float lightness, bool luma, float weight)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
            {
                if (luma)
                {
                    ColorHcy rgb = (ColorRgb)src[i];
                    src[i] = (Color15BppBgr)rgb.Colorize(hue, saturation, lightness, weight);
                }
                else
                {
                    ColorHsl rgb = (ColorRgb)src[i];
                    src[i] = (Color15BppBgr)rgb.Colorize(hue, saturation, lightness, weight);
                }
            }
            WriteData(data);
        }

        public void Grayscale(ColorWeight color)
        {
            Grayscale(Selection, color);
        }

        public void Grayscale(PaletteSelection selection, ColorWeight color)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
            {
                var rgb = (ColorRgb)src[i];
                src[i] = (Color15BppBgr)rgb.Grayscale(color);
            }
            WriteData(data);
        }

        public void Adjust(float hue, float saturation, float lightness, bool luma, float weight)
        {
            Adjust(Selection, hue, saturation, lightness, luma, weight);
        }

        public void Adjust(PaletteSelection selection, float hue, float saturation, float lightness, bool luma, float weight)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
            {
                if (luma)
                {
                    ColorHcy hsl = (ColorRgb)src[i];
                    src[i] = (Color15BppBgr)hsl.Adjust(hue, saturation, lightness, weight);
                }
                else
                {
                    ColorHsl hsl = (ColorRgb)src[i];
                    src[i] = (Color15BppBgr)hsl.Adjust(hue, saturation, lightness, weight);
                }
            }
            WriteData(data);
        }

        public void HorizontalGradient()
        {
            HorizontalGradient(Selection);
        }

        public void HorizontalGradient(PaletteSelection selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            if (!(data.Selection.TileMapSelection is TileMapBoxSelection1D))
                throw new ArgumentException(nameof(selection));

            var src = data.GetData();
            var tileMap = (TileMapBoxSelection1D)data.Selection.TileMapSelection;
            var width = tileMap.Range.Horizontal;
            var last = width - 1;

            for (int y = tileMap.Range.Vertical; --y >= 0;)
            {
                var c1 = (ColorRgb)src[y * width];
                var c2 = (ColorRgb)src[y * width + last];

                for (int x = width; --x >= 0;)
                {
                    var weight = (float)x / last;
                    src[y * width + x] = (Color15BppBgr)c1.AverageWith(c2, weight);
                }
            }
            WriteData(data);
        }
        public void VerticalGradient()
        {
            VerticalGradient(Selection);
        }

        public void VerticalGradient(PaletteSelection selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var data = GetEditorData(selection);
            if (!(data.Selection.TileMapSelection is TileMapBoxSelection1D))
                throw new ArgumentException(nameof(selection));

            var src = data.GetData();
            var tileMap = (TileMapBoxSelection1D)data.Selection.TileMapSelection;
            var width = tileMap.Range.Horizontal;
            var height = tileMap.Range.Vertical;
            var last = height - 1;

            for (int x = width; --x >= 0;)
            {
                var c1 = (ColorRgb)src[x];
                var c2 = (ColorRgb)src[(last * width) + x];

                for (int y = height; --y >= 0;)
                {
                    var weight = (float)y / last;
                    src[y * width + x] = (Color15BppBgr)c1.AverageWith(c2, weight);
                }
            }
            WriteData(data);
        }

        public override void Delete()
        {
            Delete(Selection);
        }

        public void Delete(PaletteSelection selection)
        {
            var data = selection.GetPaletteData(this);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
                src[i] = 0;
            WriteData(data);
        }

        public override void SelectAll()
        {
            TileMap.Selection = new TileMapLineSelection1D(0, TileMap.GridSize - 1);
        }

        private PaletteData GetEditorData(PaletteSelection selection)
        {
            return PreviewMode ?
                (PaletteData)PreviewData.Copy(selection) : selection.GetPaletteData(this);
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            DisablePreviewMode();
            base.OnSelectionChanged(e);
        }

        private void TileMap_SelectionChanged(object sender, EventArgs e)
        {
            Selection = new PaletteSelection(Palette.StartAddress, TileMap.Selection);
        }

        public void DrawDataAsTileMap(IntPtr scan0, int length)
        {
            var data = Palette.GetData();

            // Assert a working state
            Debug.Assert(TileMap != null);
            Debug.Assert(Selection != null);
            Debug.Assert(data != null);
            Debug.Assert(scan0 != IntPtr.Zero);
            Debug.Assert(length >= 0);

            // Create local copies of data
            var zero = TileMap.ZeroTile;
            var span = TileMap.VisibleGridSpan;
            var cellw = TileMap.CellWidth;
            var cellh = TileMap.CellHeight;
            var vieww = TileMap.ViewWidth;
            var viewh = TileMap.ViewHeight;
            var width = TileMap.Width;
            var height = TileMap.Height;
            var area = width * height;
            var cellr = cellh * width;
            var address = Palette.GetAddressFromIndex(zero);

            Debug.Assert(address >= 0);
            Debug.Assert(address + (span * Color15BppBgr.SizeOf) <= data.Length);
            Debug.Assert(area * Color32BppArgb.SizeOf <= length);

            // Make sure the selection is viewable.
            var validSelection = Selection.StartAddress == Palette.StartAddress;

            // Get working selection
            var selection = validSelection ? Selection.TileMapSelection : null;

            var darkness = selection is TileMapGateSelection1D ? 2 : 1;

            unsafe
            {
                var image = (Color32BppArgb*)scan0;

                fixed (byte* ptr = &data[address])
                {
                    // Get color data to draw
                    var src = (Color15BppBgr*)ptr;

                    // Loop is already capped at data size, so no worry of exceeding array bounds.
                    Parallel.For(0, span, i =>
                    {
                        // Get current color
                        var color = (Color32BppArgb)src[i];

                        // Turn on alpha no matter what
                        color.Alpha = Byte.MaxValue;

                        // Darken regions that are not in the selection
                        if (selection != null && !selection.ContainsIndex(i + zero))
                        {
                            color.Red >>= darkness;
                            color.Green >>= darkness;
                            color.Blue >>= darkness;
                        }

                        // Get destination pointer address.
                        var dest = image +
                            ((i % vieww) * cellh) +
                            ((i / vieww) * cellr);

                        // Draw the tile.
                        for (int h = cellh; --h >= 0; dest += width)
                            for (int w = cellw; --w >= 0;)
                                dest[w] = color;
                    });
                }
            }
        }
    }
}