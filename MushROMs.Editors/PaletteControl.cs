using System;
using System.ComponentModel;
using MushROMs.Controls;
using MushROMs.SNES;

namespace MushROMs.Editors
{
    public class PaletteControl : TileMapEditorControl1D
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new PaletteEditor Editor
        {
            get { return (PaletteEditor)base.Editor; }
            set
            {
                if (Editor == value)
                    return;

                if (Editor != null)
                    Editor.Palette.StartAddressChanged -= Editor_Redraw;

                base.Editor = value;

                if (Editor != null)
                    Editor.Palette.StartAddressChanged += Editor_Redraw;
            }
        }

        protected override void DrawDataAsTileMap(IntPtr scan0, int length)
        {
            Editor.DrawDataAsTileMap(scan0, length);
        }

        private void Editor_Redraw(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
