using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Helper;

namespace MushROMs.Controls
{
    public abstract class TileMapControl : DesignControl
    {
        private TileMap _tileMap;

        private ScrollBar _vScrollBar;
        private ScrollBar _hScrollBar;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal TileMapResizeMode TileMapResizeMode
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TileMap TileMap
        {
            get { return _tileMap; }
            protected set
            {
                if (TileMap == value)
                    return;

                if (TileMap != null)
                {
                    TileMap.TileSizeChanged -= TileMap_CellSizeChanged;
                    TileMap.ZoomSizeChanged -= TileMap_CellSizeChanged;
                    TileMap.ViewSizeChanged -= TileMap_ViewSizeChanged;
                    TileMap.GridSizeChanged -= TileMap_GridLengthChanged;
                    TileMap.ZeroTileChanged -= TileMap_ZeroIndexChanged;
                }

                _tileMap = value;

                if (TileMap != null)
                {
                    TileMap.TileSizeChanged += TileMap_CellSizeChanged;
                    TileMap.ZoomSizeChanged += TileMap_CellSizeChanged;
                    TileMap.ViewSizeChanged += TileMap_ViewSizeChanged;
                    TileMap.GridSizeChanged += TileMap_GridLengthChanged;
                    TileMap.ZeroTileChanged += TileMap_ZeroIndexChanged;
                }

                ResetScrollBars();
            }
        }

        public ScrollBar VerticalScrollBar
        {
            get { return _vScrollBar; }
            set
            {
                if (VerticalScrollBar == value)
                    return;

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll -= VerticalScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
                }

                _vScrollBar = value;

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll += VerticalScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
                }

                ResetVerticalScrollBar();
            }
        }

        public ScrollBar HorizontalScrollBar
        {
            get { return _hScrollBar; }
            set
            {
                if (HorizontalScrollBar == value)
                    return;

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll -= HorizontalScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged -= HorizontalScrollBar_ValueChanged;
                }

                _hScrollBar = value;

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll += HorizontalScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged += HorizontalScrollBar_ValueChanged;
                }

                ResetHorizontalScrollBar();
            }
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            if (TileMapResizeMode == TileMapResizeMode.ControlResize)
                return;

            if (TileMapResizeMode == TileMapResizeMode.None)
                TileMapResizeMode = TileMapResizeMode.ControlResize;

            base.SetClientSizeCore(x, y);

            if (TileMapResizeMode == TileMapResizeMode.ControlResize)
                TileMapResizeMode = TileMapResizeMode.None;
        }

        public void ResetScrollBars()
        {
            ResetVerticalScrollBar();
            ResetHorizontalScrollBar();
        }
        protected abstract void ResetHorizontalScrollBar();
        protected abstract void ResetVerticalScrollBar();
        protected abstract void AdjustScrollBarPositions();

        protected abstract void ScrollTileMapVertical(int value);
        protected abstract void ScrollTileMapHorizontal(int value);

        private void SetClientSizeFromTileMap()
        {
            if ((Size)TileMap.Size == ClientSize || TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
                return;

            if (TileMapResizeMode == TileMapResizeMode.None)
                TileMapResizeMode = TileMapResizeMode.TileMapCellResize;
            
            SetClientSizeCore(TileMap.Width, TileMap.Height);

            if (TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
                TileMapResizeMode = TileMapResizeMode.None;
        }

        public void DrawViewTilePath(GraphicsPath path, Position tile, Padding padding)
        {
            if (TileMap == null)
                return;
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            path.Reset();

            var dot = tile * TileMap.CellSize;
            path.AddRectangle(new Rectangle(
                dot.X + padding.Left,
                dot.Y + padding.Top,
                TileMap.CellWidth - 1 - padding.Horizontal,
                TileMap.CellHeight - 1 - padding.Vertical));
        }

        public abstract void GenerateSelectionPath(GraphicsPath path);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (TileMap != null)
                GetActiveTileFromMouse(e);
            base.OnMouseMove(e);
        }

        protected virtual void GetActiveTileFromMouse(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (!ClientRectangle.Contains(e.Location))
                return;

            if (!MouseHovering)
                TileMap.ActiveViewTile = (Position)e.Location / TileMap.CellSize;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (TileMap != null)
                GetActiveTileFromKeys(e);
            base.OnKeyDown(e);
        }

        protected virtual void GetActiveTileFromKeys(KeyEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var active = TileMap.ActiveViewTile;
            switch (e.KeyCode)
            {
            case Keys.Left:
                active.X--;
                break;
            case Keys.Right:
                active.X++;
                break;
            case Keys.Up:
                active.Y--;
                break;
            case Keys.Down:
                active.Y++;
                break;
            }
            if (TileMap.ActiveViewTile != active)
                TileMap.ActiveViewTile = active;
        }

        private void TileMap_CellSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
        }

        private void TileMap_ViewSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
            ResetScrollBars();
        }

        private void TileMap_GridLengthChanged(object sender, EventArgs e)
        {
            ResetScrollBars();
        }

        private void TileMap_ZeroIndexChanged(object sender, EventArgs e)
        {
            AdjustScrollBarPositions();
        }

        private void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ScrollTileMapHorizontal(e.NewValue);
        }

        private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ScrollTileMapVertical(e.NewValue);
        }

        private void VerticalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapVertical(VerticalScrollBar.Value);
        }

        private void HorizontalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapHorizontal(HorizontalScrollBar.Value);
        }
    }
}