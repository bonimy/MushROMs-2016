namespace MushROMs.Controls
{
    public class TileMapForm1D : TileMapForm
    {
        public new TileMapControl1D TileMapControl
        {
            get { return (TileMapControl1D)base.TileMapControl; }
            set { base.TileMapControl = value; }
        }

        public new TileMap1D TileMap
        {
            get { return (TileMap1D)base.TileMap; }
        }
    }
}
