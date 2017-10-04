using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.SNES
{
    public class Obj16Selection : ISelection
    {
        public ITileMapSelection1D TileMapSelection
        {
            get;
            private set;
        }

        public int StartAddress
        {
            get;
            private set;
        }
        public int StartIndex
        {
            get { return TileMapSelection.StartIndex; }
        }
        public int NumTiles
        {
            get { return TileMapSelection.NumTiles; }
        }

        public Obj16Selection(int startAddress, ITileMapSelection1D selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            StartAddress = startAddress;
            TileMapSelection = selection;
        }

        public Obj16Data GetEditorData(Obj16Editor editor)
        {
            return new Obj16Data(editor, this);
        }

        ISelectionData ISelection.GetSelectionData(IEditor editor)
        {
            return GetEditorData((Obj16Editor)editor);
        }
    }
}
