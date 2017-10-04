using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MushROMs.Controls
{
    public class TileMapEditorControl2D : TileMapControl2D
    {
        private const float FallbackAnimationTime = 300;

        private static readonly Size FallbackZoomSize = new Size(0x10, 0x10);
        private static readonly Size FallbackViewSize = new Size(0x10, 0x10);

        private static readonly DashedPenPair FallbackSelectionPens =
            new DashedPenPair(SystemColors.Highlight, SystemColors.HighlightText, 4, 4);
        private static readonly DashedPenPair FallbackActiveTilePens =
            new DashedPenPair(SystemColors.Highlight, SystemColors.HighlightText, 2, 2);

        private int _dashOffset;
        private ITileMapEditor2D _editor;

        private DashedPenPair _selectionPens;
        private DashedPenPair _activeTilePens;

        private IContainer Components
        {
            get; set;
        }

        private Timer Timer
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITileMapEditor2D Editor
        {
            get { return _editor; }
            set
            {
                if (Editor == value)
                    return;

                if (Editor != null)
                {
                    Editor.DataModified -= Editor_Redraw;

                    TileMap = null;
                }

                _editor = value;

                if (Editor != null)
                {
                    Editor.DataModified += Editor_Redraw;

                    TileMap = Editor.TileMap;
                }
            }
        }

        public new TileMap2D TileMap
        {
            get { return base.TileMap; }
            protected set
            {
                if (TileMap == value)
                    return;

                if (TileMap != null)
                {
                    TileMap.ActiveGridTileChanged -= Editor_Redraw;
                    TileMap.ZeroTileChanged -= Editor_Redraw;
                }

                base.TileMap = value;

                if (TileMap != null)
                {
                    TileMap.ActiveGridTileChanged += Editor_Redraw;
                    TileMap.ZeroTileChanged += Editor_Redraw;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DashedPenPair SelectionPens
        {
            get { return _selectionPens; }
            set
            {
                _selectionPens = value;
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DashedPenPair ActiveTilePens
        {
            get { return _activeTilePens; }
            set
            {
                _activeTilePens = value;
                Invalidate();
            }
        }

        private int DashOffset
        {
            get { return _dashOffset; }
            set
            {
                _dashOffset = value;
                Invalidate();
            }
        }

        protected TileMapEditorControl2D()
        {
            SelectionPens = FallbackSelectionPens;
            ActiveTilePens = FallbackActiveTilePens;

            Components = new Container();

            Timer = new Timer();
            Timer.Interval = FallbackAnimationTime;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();

            Components.Add(Timer);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Editor != null)
            {
                DrawEditorData(e);
                DrawActiveTileBorder(e);
                if (Editor.Selection != null)
                    DrawSelection(e);
            }
            base.OnPaint(e);
        }

        protected virtual void DrawEditorData(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            using (Bitmap bmp = new Bitmap(
                TileMap.Width, TileMap.Height, PixelFormat.Format32bppArgb))
            {
                var data = bmp.LockBits(
                    new Rectangle(Point.Empty, bmp.Size),
                    ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                DrawDataAsTileMap(data.Scan0, data.Height * data.Stride);

                bmp.UnlockBits(data);

                e.Graphics.DrawImageUnscaled(bmp, Point.Empty);
            }
        }

        protected virtual void DrawDataAsTileMap(IntPtr scan0, int length)
        {

        }

        protected virtual void DrawActiveTileBorder(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            using (GraphicsPath path = new GraphicsPath())
            {
                using (Pen pen1 = new Pen(Color.Empty, 1),
                           pen2 = new Pen(Color.Empty, 1))
                {
                    DrawViewTilePath(
                        path, TileMap.ActiveViewTile, new Padding(2));

                    ActiveTilePens.SetPenProperties(pen1, pen2);
                    pen1.DashOffset += DashOffset;
                    pen2.DashOffset += DashOffset;

                    e.Graphics.DrawPath(pen1, path);
                    e.Graphics.DrawPath(pen2, path);
                }
            }
        }

        protected virtual void DrawSelection(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            using (GraphicsPath path = new GraphicsPath())
            {
                using (Pen pen1 = new Pen(Color.Empty, 1),
                           pen2 = new Pen(Color.Empty, 1))
                {
                    GenerateSelectionPath(path);

                    SelectionPens.SetPenProperties(pen1, pen2);
                    pen1.DashOffset -= DashOffset;
                    pen2.DashOffset -= DashOffset;

                    e.Graphics.DrawPath(pen1, path);
                    e.Graphics.DrawPath(pen2, path);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Components != null)
                Components.Dispose();
            base.Dispose(disposing);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Enabled && Visible)
                DashOffset++;
        }

        private void Editor_Redraw(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
