using System;
using Helper.ColorSpaces;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public abstract class RGBDialog : DialogProxy
    {
        protected abstract RGBForm RGBForm
        {
            get;
        }

        protected sealed override DialogForm BaseForm
        {
            get { return RGBForm; }
        }

        public event EventHandler ValueChanged
        {
            add { RGBForm.ColorValueChanged += value; }
            remove { RGBForm.ColorValueChanged -= value; }
        }

        public float Red
        {
            get { return RGBForm.Red; }
            set { RGBForm.Red = value; }
        }

        public float Green
        {
            get { return RGBForm.Green; }
            set { RGBForm.Green = value; }
        }

        public float Blue
        {
            get { return RGBForm.Blue; }
            set { RGBForm.Blue = value; }
        }

        public ColorRgb Color
        {
            get { return RGBForm.Color; }
            set { RGBForm.Color = value; }
        }

        public bool Preview
        {
            get { return RGBForm.Preview; }
            set { RGBForm.Preview = value; }
        }

        public void ResetValues()
        {
            RGBForm.ResetValues();
        }
    }
}
