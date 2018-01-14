using System;
using Helper;
using Helper.PixelFormats;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    public class GFXEditor : Editor, ITileMapEditor1D
    {
        private readonly TileMap1D _tileMap = new TileMap1D();

        public static readonly EditorInfo EditorInfo =
            new EditorInfo(typeof(GFXEditor), Resources.GFXEditorName);

        private const GraphicsFormat FallbackGraphicsFormat = GraphicsFormat.Format1Bpp8x8;

        private byte[] _data;
        private int _startAddress;
        private GraphicsFormat _graphicsFormat;

        public event EventHandler StartAddressChanged;

        public event EventHandler GraphicsFormatChanged;

        private byte[] Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
                OnDataInitialized(EventArgs.Empty);
            }
        }

        public int StartAddress
        {
            get
            {
                return _startAddress;
            }

            private set
            {
                if (StartAddress == value)
                {
                    return;
                }

                _startAddress = value;
                OnStartAddressChanged(EventArgs.Empty);
            }
        }

        public GraphicsFormat GraphicsFormat
        {
            get
            {
                return _graphicsFormat;
            }

            set
            {
                if (GraphicsFormat == value)
                {
                    return;
                }

                _graphicsFormat = value;
                OnGraphicsFormatChanged(EventArgs.Empty);
            }
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

        public new GFXSelection Selection
        {
            get { return (GFXSelection)base.Selection; }
            set { base.Selection = value; }
        }

        public TileMap1D TileMap
        {
            get { return _tileMap; }
        }

        public GFXEditor()
        {
            TileMap.TileSize = 8;
            TileMap.ViewSize = 0x10;
            TileMap.ZoomSize = 2;
            TileMap.SelectionChanged += TileMap_SelectionChanged;
            TileMap.Selection = new TileMapSingleSelection1D(TileMap.ZeroTile);
        }

        public void InitializeData(int numTiles, GraphicsFormat graphicsFormat)
        {
            Data = new byte[numTiles * GFXTile.GetTileDataSize(graphicsFormat)];
        }

        public void InitializeData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!CHRFile.IsValidData(data))
            {
                throw new ArgumentException(nameof(data));
            }

            Data = new byte[data.Length];
            Array.Copy(data, Data, Data.Length);
        }

        public byte[] GetData()
        {
            return Data;
        }

        public new GFXData GetSelectionData()
        {
            return (GFXData)base.GetSelectionData();
        }

        public override bool IsValidSelectionData(ISelectionData data)
        {
            return data is GFXData;
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void SelectAll()
        {
            TileMap.Selection = new TileMapLineSelection1D(0, TileMap.GridSize - 1);
        }

        protected override void OnDataInitialized(EventArgs e)
        {
            GraphicsFormat = FallbackGraphicsFormat;
            base.OnDataInitialized(e);
        }

        protected virtual void OnStartAddressChanged(EventArgs e)
        {
            StartAddressChanged?.Invoke(this, e);
        }

        protected virtual void OnGraphicsFormatChanged(EventArgs e)
        {
            TileMap.GridSize = Data.Length / TileDataSize;
            GraphicsFormatChanged?.Invoke(this, e);
        }

        private void TileMap_SelectionChanged(object sender, EventArgs e)
        {
            Selection = new GFXSelection(StartAddress, TileMap.Selection);
        }

        public int GetAddressFromIndex(int index)
        {
            return GetAddressFromIndex(index, StartAddress, GraphicsFormat);
        }

        public static int GetAddressFromIndex(int index, int startAddress, GraphicsFormat format)
        {
            return (index * GFXTile.GetTileDataSize(format)) + startAddress;
        }

        public static int GetIndexFromAddress(int address, GraphicsFormat format)
        {
            return address / GFXTile.GetTileDataSize(format);
        }

        public virtual void DrawDataAsTileMap(IntPtr scan0, int length, PaletteData palette)
        {
            if (scan0 == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(scan0));
            }

            if (palette == null)
            {
                throw new ArgumentNullException(nameof(palette));
            }

            var colors = palette.GetData();
            if (colors.Length < ColorsPerPixel)
            {
                throw new ArgumentOutOfRangeException(nameof(palette));
            }

            // The total image size
            var width = TileMap.CellWidth * TileMap.ViewWidth;
            var height = TileMap.CellHeight * TileMap.ViewHeight;

            // The size, in bytes, of the pixels to be drawn
            var alloc = width * height * Color32BppArgb.SizeOf;

            // Ensure the drawing size does not exceed the IntPtr size.
            if (alloc > length)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                    SR.ErrorArrayBounds(nameof(length), length, alloc));
            }

            // Get the number of tiles being drawn (limited to the data size).
            var tiles = Math.Min(TileMap.GridSize - TileMap.ZeroTile, TileMap.ViewSize.Area);

            // The address to begin reading data at.
            var address = GetAddressFromIndex(TileMap.ZeroTile);

            // Make sure the passed selection is viewable.
            var validSelection = Selection.StartAddress == StartAddress;

            // Determine if it is a gated selection (to darken the excluded regions).
            var gateSelection = validSelection ? Selection.TileMapSelection : null;

            //if (!(gateSelection is TileMapGateSelection1D))
            //    gateSelection = null;

            var plane = width * TileMap.ZoomHeight - TileMap.CellWidth;
            var pixel = width * TileMap.ZoomHeight - TileMap.ZoomWidth;

            var normal = new Color32BppArgb[colors.Length];
            for (var i = normal.Length; --i >= 0;)
            {
                normal[i] = colors[i];
                normal[i].Alpha = Byte.MaxValue;
            }

            var dark = new Color32BppArgb[normal.Length];
            for (var i = dark.Length; --i >= 0;)
            {
                dark[i].Alpha = normal[i].Alpha;
                for (var j = 3; --j >= 0;)
                {
                    dark[i][j] = (byte)(normal[i][j] >> 1);
                }
            }

            unsafe
            {
                fixed (byte* ptr = &Data[address])
                {
                    var tile = new GFXTile();

                    // Loop is already capped at data size, so no worry of exceeding array bounds.
                    for (var i = tiles; --i >= 0;)
                    {
                        // Darken regions that are not in gated selections.
                        var colors32 = gateSelection != null && !gateSelection.ContainsIndex(i + TileMap.ZeroTile) ?
                            dark : normal;

                        // Get destination pointer address.
                        var dest = (Color32BppArgb*)scan0 +
                            ((i % TileMap.ViewWidth) * TileMap.CellWidth) +
                            ((i / TileMap.ViewWidth) * TileMap.CellHeight * width);

                        tile.GetTileData(ptr, i, GraphicsFormat);

                        var pixels = tile.UnsafeData;

                        for (var y = GFXTile.PlanesPerTile; --y >= 0; dest += plane)
                        {
                            for (var x = GFXTile.DotsPerPlane; --x >= 0; dest -= pixel, pixels++)
                            {
                                var color = colors32[*pixels];
                                for (var j = TileMap.ZoomHeight; --j >= 0; dest += width)
                                {
                                    for (var k = TileMap.ZoomWidth; --k >= 0;)
                                    {
                                        dest[k] = color;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
