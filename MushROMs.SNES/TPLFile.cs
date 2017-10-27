using System;
using Helper;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public static class TPLFile
    {
        public const string DefaultExtension = ".tpl";

        public static readonly FileAssociation FileAssociation =
            new FileAssociation(DefaultExtension, InitializeEditor, SaveFileData_Internal, FileVisibilityFilters.Any);

        private static readonly byte[] TPLHeader = {
            (byte)'T',
            (byte)'P',
            (byte)'L',
            2 };

        public static PaletteEditor InitializeEditor(byte[] data)
        {
            if (!IsValidData(data))
                return null;

            return new PaletteEditor(new Palette(GetColors(data)));
        }

        public static byte[] SaveFileData(PaletteEditor editor)
        {
            return GetData(editor.GetColors());
        }

        private static byte[] SaveFileData_Internal(IEditor editor)
        {
            return SaveFileData((PaletteEditor)editor);
        }

        public static bool IsValidExtension(string ext)
        {
            if (String.IsNullOrEmpty(ext))
                return false;

            return IOHelper.CompareExtensions(ext, DefaultExtension) == 0;
        }

        private static bool IsValidSize(int length)
        {
            return length >= TPLHeader.Length &&
                (length - TPLHeader.Length) % Color15BppBgr.SizeOf == 0;
        }

        public static bool IsValidData(byte[] data)
        {
            if (data == null)
                return false;
            if (!IsValidSize(data.Length))
                return false;
            for (int i = TPLHeader.Length; --i >= 0;)
                if (data[i] != TPLHeader[i])
                    return false;
            unsafe
            {
                fixed (byte* ptr = &data[TPLHeader.Length])
                {
                    for (int i = GetNumColorsFromSize(data.Length); --i >= 0;)
                        if ((((Color15BppBgr*)ptr)[i] & 0x8000) != 0)
                            return false;
                }
            }
            return true;
        }

        public static int GetNumColorsFromSize(int length)
        {
            return (length - TPLHeader.Length) / Color15BppBgr.SizeOf;
        }

        public static int GetSizeFromNumColors(int numColors)
        {
            return (numColors * Color15BppBgr.SizeOf) + TPLHeader.Length;
        }

        public static Color15BppBgr[] GetColors(byte[] data)
        {
            if (data == null)
                return null;

            var colors = new Color15BppBgr[GetNumColorsFromSize(data.Length)];
            unsafe
            {
                fixed (byte* src = &data[TPLHeader.Length])
                fixed (Color15BppBgr* dest = colors)
                {
                    for (int i = data.Length; --i >= TPLHeader.Length;)
                        ((byte*)dest)[i] = src[i];
                }
            }
            return colors;
        }

        public static byte[] GetData(Color15BppBgr[] colors)
        {
            if (colors == null)
                return null;

            var data = new byte[GetSizeFromNumColors(colors.Length)];
            for (int i = TPLHeader.Length; --i >= 0;)
                data[i] = TPLHeader[i];

            unsafe
            {
                fixed (byte* dest = &data[TPLHeader.Length])
                fixed (Color15BppBgr* src = colors)
                {
                    for (int i = data.Length; --i >= 0;)
                        dest[i] = ((byte*)src)[i];
                }
            }
            return data;
        }
    }
}
