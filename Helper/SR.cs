using System;
using System.Globalization;
using System.IO;
using Helper.Properties;

namespace Helper
{
    public static class SR
    {
        public static CultureInfo CurrentCulture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        public static string ErrorInvalidOpenLowerBound(string paramName, object value, object valid)
        {
            return GetString(Resources.ErrorInvalidOpenLowerBound, paramName, value, valid);
        }

        public static string ErrorInvalidOpenUpperBound(string paramName, object value, object valid)
        {
            return GetString(Resources.ErrorInvalidOpenUpperBound, paramName, value, valid);
        }

        public static string ErrorInvalidClosedLowerBound(string paramName, object value, object valid)
        {
            return GetString(Resources.ErrorInvalidClosedLowerBound, paramName, value, valid);
        }

        public static string ErrorInvalidClosedUpperBound(string paramName, object value, object valid)
        {
            return GetString(Resources.ErrorInvalidClosedUpperBound, paramName, value, valid);
        }

        public static string ErrorArrayBounds(string paramName, int value, int valid)
        {
            return GetString(Resources.ErrorArrayBounds, paramName, value, valid);
        }

        public static string ErrorArrayRange(string paramName, int value, string arrayName, int arrayLength, int startIndex)
        {
            return GetString(Resources.ErrorArrayRange, paramName, value, arrayName, arrayLength, startIndex);
        }

        public static string ErrorEmptyOrNullArray(string paramName)
        {
            return GetString(Resources.ErrorEmptyOrNullArray, paramName);
        }

        public static string ErrorFileFormat(string path)
        {
            var name = Path.GetFileName(path);
            if (String.IsNullOrEmpty(name))
            {
                name = Resources.ErrorFileFormatName;
            }

            return GetString(Resources.ErrorFileFormat, name);
        }

        public static string ErrorValueIsNaN(string paramName)
        {
            return GetString(Resources.ErrorValueIsNaN, paramName);
        }

        public static string ErrorValueIsInfinite(string paramName)
        {
            return GetString(Resources.ErrorValueIsInfinity, paramName);
        }

        public static string ErrorInvalidExtensionName(string ext)
        {
            return GetString(Resources.ErrorInvalidExtensionName, ext);
        }

        public static string ErrorInvalidPathName(string path)
        {
            return GetString(Resources.ErrorInvalidPathName, path);
        }

        public static string GetUntitledName(int number, string ext)
        {
            if (ext == null)
            {
                ext = String.Empty;
            }

            return GetString(Resources.UntitledName, GetString(number)) + ext;
        }

        public static string GetString(string format, params object[] args)
        {
            return String.Format(CurrentCulture, format, args);
        }

        public static string GetString(IFormattable value)
        {
            return GetString(value, null);
        }

        public static string GetString(IFormattable value, string format)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString(format, CurrentCulture);
        }
    }
}
