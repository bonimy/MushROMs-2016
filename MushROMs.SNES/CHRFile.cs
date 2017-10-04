using System;
using Helper;

namespace MushROMs.SNES
{
    public static class CHRFile
    {
        public const string DefaultExtension = ".chr";

        public static readonly FileAssociation FileAssociation =
            new FileAssociation(DefaultExtension, InitializeEditor, SaveFileData_Internal, FileVisibilityFilters.Any);

        public static GFXEditor InitializeEditor(byte[] data)
        {
            if (!IsValidData(data))
                return null;

            GFXEditor gfx = new GFXEditor();
            gfx.InitializeData(data);
            return gfx;
        }

        public static byte[] SaveFileData(GFXEditor editor)
        {
            return null;
        }

        private static byte[] SaveFileData_Internal(IEditor editor)
        {
            return SaveFileData((GFXEditor)editor);
        }

        public static bool IsValidExtension(string ext)
        {
            if (String.IsNullOrEmpty(ext))
                return false;

            return IOHelper.CompareExtensions(ext, DefaultExtension) == 0;
        }

        public static bool IsValidSize(int length)
        {
            return length % GFXTile.GetTileDataSize(GraphicsFormat.Format1Bpp8x8) == 0;
        }

        public static bool IsValidData(byte[] data)
        {
            if (data == null)
                return false;
            return IsValidSize(data.Length);
        }
    }
}
