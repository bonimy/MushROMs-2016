using System;
using System.Collections.Generic;
using System.Text;
using Helper;

namespace MushROMs
{
    public class EditorDictionary : PathDictionary<IEditor>
    {
        private MasterEditor _owner;

        public MasterEditor Owner
        {
            get { return _owner; }
        }

        public EditorDictionary(MasterEditor owner)
        {
            _owner = owner;
        }
    }
}
