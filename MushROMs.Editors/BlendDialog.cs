using Helper.ColorSpaces;

namespace MushROMs.Editors
{
    public sealed class BlendDialog : RGBDialog
    {
        private readonly BlendForm _baseForm;

        protected override RGBForm RGBForm
        {
            get { return _baseForm; }
        }

        public BlendMode BlendMode
        {
            get { return _baseForm.BlendMode; }
            set { _baseForm.BlendMode = value; }
        }

        public BlendDialog()
        {
            _baseForm = new BlendForm(this);
        }
    }
}
