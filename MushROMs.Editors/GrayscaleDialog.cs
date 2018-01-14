namespace MushROMs.Editors
{
    public class GrayscaleDialog : RGBDialog
    {
        private readonly GrayscaleForm _baseForm;

        protected override RGBForm RGBForm
        {
            get { return _baseForm; }
        }

        public GrayscaleDialog()
        {
            _baseForm = new GrayscaleForm(this);
        }
    }
}
