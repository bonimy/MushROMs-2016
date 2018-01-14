using System;
using Helper.ColorSpaces;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public sealed class ColorizeDialog : DialogProxy
    {
        private readonly ColorizeForm _baseForm;

        protected override DialogForm BaseForm
        {
            get { return _baseForm; }
        }

        public event EventHandler ValueChanged
        {
            add { _baseForm.ColorValueChanged += value; }
            remove { _baseForm.ColorValueChanged -= value; }
        }

        public float Hue
        {
            get { return _baseForm.Hue; }
            set { _baseForm.Hue = value; }
        }

        public float Saturation
        {
            get { return _baseForm.Saturation; }
            set { _baseForm.Saturation = value; }
        }

        public float Lightness
        {
            get { return _baseForm.Lightness; }
            set { _baseForm.Lightness = value; }
        }

        public float Weight
        {
            get { return _baseForm.Weight; }
            set { _baseForm.Weight = value; }
        }

        public ColorizerMode ColorizerMode
        {
            get { return _baseForm.ColorizerMode; }
            set { _baseForm.ColorizerMode = value; }
        }

        public bool Luma
        {
            get { return _baseForm.Luma; }
            set { _baseForm.Luma = value; }
        }

        public bool Preview
        {
            get { return _baseForm.Preview; }
            set { _baseForm.Preview = value; }
        }

        public ColorizeDialog()
        {
            _baseForm = new ColorizeForm(this);
        }

        public void ResetValues()
        {
            _baseForm.ResetValues();
        }
    }
}
