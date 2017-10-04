using System;
using System.Windows.Forms;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public class CreateEditorDialog : DialogProxy
    {
        private readonly CreateEditorForm _baseForm = new CreateEditorForm();

        protected override DialogForm BaseForm
        {
            get { return _baseForm; }
        }

        public Editor CreateEditor()
        {
            return _baseForm.CreateEditor();
        }
    }
}
