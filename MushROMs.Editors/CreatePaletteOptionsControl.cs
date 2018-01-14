using System.Windows.Forms;

namespace MushROMs.Editors
{
    internal partial class CreatePaletteOptionsControl : UserControl
    {
        public int NumColors
        {
            get { return (int)nudNumColors.Value; }
            set { nudNumColors.Value = value; }
        }

        public bool CopyFrom
        {
            get { return chkFromCopy.Enabled && chkFromCopy.Checked; }
            set { chkFromCopy.Checked = value; }
        }

        public CreatePaletteOptionsControl()
        {
            InitializeComponent();
        }
    }
}
