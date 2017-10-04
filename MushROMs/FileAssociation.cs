using System;
using Helper;
using MushROMs.Properties;

namespace MushROMs
{
    public class FileAssociation : IFileAssociation
    {
        public string Extension
        {
            get;
            private set;
        }
        public InitializeEditorMethod InitializeEditorMethod
        {
            get;
            private set;
        }
        public SaveFileDataMethod SaveFileDataMethod
        {
            get;
            private set;
        }
        public FileVisibilityFilters Filter
        {
            get;
            private set;
        }

        public FileAssociation(string extension, InitializeEditorMethod init, SaveFileDataMethod save, FileVisibilityFilters filter)
        {
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));
            if (!IOHelper.IsValidExtension(extension))
                throw new ArgumentException(SR.GetString(Resources.ErrorBadExtension, extension), nameof(extension));
            if (init == null)
                throw new ArgumentNullException(nameof(init));
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            
            Extension = extension;
            InitializeEditorMethod = init;
            SaveFileDataMethod = save;
            Filter = filter;
        }
    }
}