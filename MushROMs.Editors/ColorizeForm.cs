using System;
using MushROMs.Controls;
using Helper.ColorSpaces;
using MushROMs.Editors.Properties;

namespace MushROMs.Editors
{
    public sealed partial class ColorizeForm : DialogForm
    {
        private readonly DialogProxy DialogProxy;

        protected override object ProxySender
        {
            get
            {
                if (DialogProxy != null)
                    return DialogProxy;
                return base.ProxySender;
            }
        }

        public event EventHandler ColorValueChanged;
        
        private static readonly HSL FallbackAdjust = new HSL(0, 0, 0);
        private static readonly HSL FallbackColorize = new HSL(0.25f, 0.50f, 0.50f);
        private const float FallbackEffectiveness = 1.00f;
        
        private HSL SavedAdjust, SavedColorize;
        private bool RunEvent;

        public float Hue
        {
            get { return (float)ltbHue.Value / ltbHue.Maximum; }
            set { ltbHue.Value = (int)(value * ltbHue.Maximum + 0.5f); }
        }

        public float Saturation
        {
            get { return (float)ltbSaturation.Value / ltbSaturation.Maximum; }
            set { ltbSaturation.Value = (int)(value * ltbSaturation.Maximum + 0.5f); }
        }

        public float Lightness
        {
            get { return (float)ltbLightness.Value / ltbLightness.Maximum; }
            set { ltbLightness.Value = (int)(value * ltbLightness.Maximum + 0.5f); }
        }

        private HSL CurrentHSL
        {
            get { return new HSL(Hue, Saturation, Lightness); }
            set
            {
                RunEvent = false;
                Hue = value.Hue;
                Saturation = value.Saturation;
                Lightness = value.Lightness;
                RunEvent = true;
                OnColorValueChanged(EventArgs.Empty);
            }
        }

        public float Weight
        {
            get { return (float)ltnWeight.Value / ltnWeight.Maximum; }
            set { ltnWeight.Value = (int)(value * ltnWeight.Maximum + 0.5f); }
        }

        public ColorizerMode ColorizerMode
        {
            get { return chkColorize.Checked ? ColorizerMode.Colorize : ColorizerMode.Adjust; }
            set { chkColorize.Checked = value == ColorizerMode.Colorize; }
        }

        public bool Luma
        {
            get { return chkLuma.Checked; }
            set { chkLuma.Checked = value; }
        }

        public bool Preview
        {
            get { return chkPreview.Checked; }
            set { chkPreview.Checked = value; }
        }

        public ColorizeForm()
        {
            InitializeComponent();

            SavedAdjust = FallbackAdjust;
            SavedColorize = FallbackColorize;

            ResetValues();

            RunEvent = true;
        }

        public ColorizeForm(DialogProxy dialogProxy) : this()
        {
            DialogProxy = dialogProxy;
        }

        public void ResetValues()
        {
            if (ColorizerMode == ColorizerMode.Colorize)
            {
                CurrentHSL = FallbackColorize;
                Weight = FallbackEffectiveness;
            }
            else
                CurrentHSL = FallbackAdjust;
            
            btnReset.Enabled = false;
        }

        private void SwitchValues()
        {
            if (ColorizerMode == ColorizerMode.Colorize)
            {
                SavedAdjust = CurrentHSL;

                ltbHue.Minimum = 0;
                ltbHue.Maximum = 360;
                ltbSaturation.Minimum = ltbLightness.Minimum = 0;
                ltbSaturation.Maximum = ltbLightness.Maximum = 100;
                ltbSaturation.TickFrequency = ltbLightness.TickFrequency = 5;

                CurrentHSL = SavedColorize;
            }
            else
            {
                SavedColorize = CurrentHSL;

                ltbHue.Maximum = 180;
                ltbHue.Minimum = -ltbHue.Maximum;
                ltbSaturation.Maximum = 100;
                ltbSaturation.Minimum = -ltbSaturation.Maximum;
                ltbLightness.Maximum = 100;
                ltbLightness.Minimum = -ltbLightness.Maximum;
                ltbSaturation.TickFrequency = ltbLightness.TickFrequency = 10;

                CurrentHSL = SavedAdjust;
            }
        }

        private void OnColorValueChanged(EventArgs e)
        {
            if (RunEvent)
                ColorValueChanged?.Invoke(ProxySender, e);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

        private void Colorize_CheckedChanged(object sender, EventArgs e)
        {
            SwitchValues();
        }

        private void Preview_CheckedChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void HSLE_ValueChanged(object sender, EventArgs e)
        {
            btnReset.Enabled = true;
            OnColorValueChanged(EventArgs.Empty);
        }

        private void ColorizeForm_Shown(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void chkLuma_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLuma.Checked)
            {
                lblSaturation.Text = Resources.ChromaText;
                lblLightness.Text = Resources.LumaText;
            }
            else
            {
                lblSaturation.Text = Resources.SaturationText;
                lblLightness.Text = Resources.LightnessText;
            }

            OnColorValueChanged(EventArgs.Empty);
        }

        private struct HSL
        {
            public float Hue
            {
                get;
                set;
            }
            public float Saturation
            {
                get;
                set;
            }
            public float Lightness
            {
                get;
                set;
            }

            public HSL(float hue, float saturation, float luminosity)
            {
                Hue = hue;
                Saturation = saturation;
                Lightness = luminosity;
            }
        }
    }
}