namespace MushROMs.SNES
{
    public class LibraryPlugin : ILibraryPlugin
    {
        public IFileAssociation[] GetFileAssociations()
        {
            // Don't make this a field. We want a new copy each time it is called.
            return new IFileAssociation[]
               {
                RPFFile.FileAssociation,
                TPLFile.FileAssociation,
                PALFile.FileAssociation,
                CHRFile.FileAssociation
               };
        }

        public ITypeInfo[] GetEditorInfoList()
        {
            return new ITypeInfo[]
            {
                PaletteEditor.EditorInfo,
                GFXEditor.EditorInfo
            };
        }
    }
}
