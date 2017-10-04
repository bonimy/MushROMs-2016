namespace MushROMs.SNES
{
    public class GFXColorSelection : PaletteSelection
    {
        public GraphicsFormat GraphicsFormat
        {
            get;
            private set;
        }

        public GFXColorSelection(int startAddress, int index, GraphicsFormat graphicsFormat) :
            base(startAddress, index, graphicsFormat)
        {
            GraphicsFormat = graphicsFormat;
        }
    }
}
