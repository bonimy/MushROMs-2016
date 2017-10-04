using System;
using System.Collections.Generic;
using System.Text;

namespace MushROMs.Controls
{
    public class TileMapEditorForm : TileMapForm
    {
        public IEditor Editor
        {
            get;
            private set;
        }

        protected TileMapEditorForm(IEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            Editor = editor;
        }

        protected override void OnLoad(EventArgs e)
        {
            Editor.NameChanged      += Editor_SetFormTitle;
            Editor.ExtensionChanged += Editor_SetFormTitle;
            Editor.DataInitialized  += Editor_SetFormTitle;
            Editor.DataModified     += Editor_SetFormTitle;
            Editor.FileSaved        += Editor_SetFormTitle;
            SetFormTitleFromEditor();

            SetTileMapPadding();

            base.OnLoad(e);
        }

        private void Editor_SetFormTitle(object sender, EventArgs e)
        {
            SetFormTitleFromEditor();
        }

        protected virtual void SetFormTitleFromEditor()
        {
            var title = Editor.Name + Editor.Extension;
            if (!Editor.Saved)
                title += '*';
            Text = title;
        }
    }
}
