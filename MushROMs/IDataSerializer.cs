using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs
{
    public interface IDataSerializer
    {
        bool IsValidByteData { get; }
        IEditorData EditorData { get; }

        byte[] SerializeData();

    }
}
