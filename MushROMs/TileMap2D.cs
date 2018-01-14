using System;
using Helper;

namespace MushROMs
{
    public class TileMap2D : TileMap
    {
        private Range _gridSize;
        private Position _zeroTile;
        private Position _activeGridTile;

        private ITileMapSelection2D _selection;

        public Range GridSize
        {
            get
            {
                return _gridSize;
            }

            set
            {
                if (GridSize == value)
                {
                    return;
                }

                if (GridSize.Horizontal < 0 || GridSize.Vertical < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value),
                        SR.ErrorInvalidClosedLowerBound(nameof(value), value, 0));
                }

                _gridSize = value;
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        public int GridWidth
        {
            get { return GridSize.Horizontal; }
            set { GridSize = new Range(value, GridHeight); }
        }

        public int GridHeight
        {
            get { return GridSize.Vertical; }
            set { GridSize = new Range(GridWidth, value); }
        }

        public Position ZeroTile
        {
            get
            {
                return _zeroTile;
            }

            set
            {
                if (ZeroTile == value)
                {
                    return;
                }

                _zeroTile = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        public int ZeroTileX
        {
            get { return ZeroTile.X; }
            set { ZeroTile = new Position(value, ZeroTileY); }
        }

        public int ZeroTileY
        {
            get { return ZeroTile.Y; }
            set { ZeroTile = new Position(ZeroTileX, value); }
        }

        public Position ActiveGridTile
        {
            get
            {
                return _activeGridTile;
            }

            set
            {
                if (ActiveGridTile == value)
                {
                    return;
                }

                _activeGridTile = value;
                OnActiveGridTileChanged(EventArgs.Empty);
            }
        }

        public override Position ActiveViewTile
        {
            get { return GetViewTile(ActiveGridTile); }
            set { ActiveGridTile = GetGridTile(value); }
        }

        public ITileMapSelection2D Selection
        {
            get
            {
                return _selection;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (Selection == value)
                {
                    return;
                }

                _selection = value;
                OnSelectionChanged(EventArgs.Empty);
            }
        }

        public TileMap2D()
        {
            _selection = new TileMapEmptySelection2D();
        }

        public bool TileIsInGrid(Position tile)
        {
            return GridSize.Contains(tile);
        }

        public void InitializeSelection(ITileMapSelection2D selection)
        {
            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            OnSelectionInitialized(EventArgs.Empty);
        }

        public void CreateSelection(ITileMapSelection2D selection)
        {
            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            OnSelectionCreated(EventArgs.Empty);
        }

        public int GetGridTileX(int viewTileX)
        {
            return GetGridTileX(viewTileX, ZeroTileX);
        }

        public int GetGridTileY(int viewTileY)
        {
            return GetGridTileY(viewTileY, ZeroTileY);
        }

        public Position GetGridTile(Position viewTile)
        {
            return GetGridTile(viewTile, ZeroTile);
        }

        public static int GetGridTileX(int viewTileX, int zeroTileX)
        {
            return viewTileX + zeroTileX;
        }

        public static int GetGridTileY(int viewTileY, int zeroTileY)
        {
            return viewTileY + zeroTileY;
        }

        public static Position GetGridTile(Position viewTile, Position zeroTile)
        {
            return new Position(GetGridTileX(viewTile.X, zeroTile.X),
                GetGridTileY(viewTile.Y, zeroTile.Y));
        }

        public int GetViewTileX(int gridTileX)
        {
            return GetViewTileX(gridTileX, ZeroTileX);
        }

        public int GetViewTileY(int gridTileY)
        {
            return GetViewTileY(gridTileY, ZeroTileY);
        }

        public Position GetViewTile(Position gridTile)
        {
            return GetViewTile(gridTile, ZeroTile);
        }

        public static int GetViewTileX(int gridTileX, int zeroTileX)
        {
            return gridTileX - zeroTileX;
        }

        public static int GetViewTileY(int gridTileY, int zeroTileY)
        {
            return gridTileY - zeroTileY;
        }

        public static Position GetViewTile(Position gridTile, Position zeroTile)
        {
            return new Position(GetViewTileX(gridTile.X, zeroTile.X),
                GetViewTileY(gridTile.Y, zeroTile.Y));
        }
    }
}
