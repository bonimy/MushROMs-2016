using System;
using Helper;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public class PALFile
    {
        public const string DefaultExtension = ".pal";

        public static readonly FileAssociation FileAssociation =
            new FileAssociation(DefaultExtension, InitializeEditor, SaveFileData_Internal, FileVisibilityFilters.Any);

        public static Palette InitializeEditor(byte[] data)
        {
            if (!IsValidData(data))
                return null;

            return new Palette(GetColors(data));
        }

        public static byte[] SaveFileData(Palette palette)
        {
            var colors = palette.GetColors();
            return GetData(colors);
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

        public static bool IsValidSize(int length)
        {
            return length >= 0 && length % Color24BppRgb.SizeOf == 0;
        }

        public static bool IsValidData(byte[] data)
        {
            if (data == null)
                return false;
            return IsValidSize(data.Length);
        }

        public static int GetNumColorsFromSize(int length)
        {
            return length / Color24BppRgb.SizeOf;
        }

        public static int GetSizeFromNumColors(int numColors)
        {
            return numColors * Color24BppRgb.SizeOf;
        }

        public static Color15BppBgr[] GetColors(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var colors = new Color15BppBgr[GetNumColorsFromSize(data.Length)];
            unsafe
            {
                fixed (byte* ptr = data)
                fixed (Color15BppBgr* dest = colors)
                {
                    var src = (Color24BppRgb*)ptr;
                    for (int i = colors.Length; --i >= 0;)
                        dest[i] = (Color15BppBgr)src[i];
                }
            }

            return colors;
        }

        public static ref int Find3(int[,] matrix, Func<int, bool> predicate)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (predicate(matrix[i, j]))
                        return ref matrix[i, j];
            throw new InvalidOperationException("Not found");
        }

        public static byte[] GetData(Color15BppBgr[] colors)
        {
            if (colors == null)
                throw new ArgumentNullException(nameof(colors));

            var data = new byte[GetSizeFromNumColors(colors.Length)];
            unsafe
            {
                fixed (byte* ptr = data)
                fixed (Color15BppBgr* src = colors)
                {
                    var dest = (Color24BppRgb*)ptr;
                    for (int i = colors.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }
            return data;
        }
    }
}