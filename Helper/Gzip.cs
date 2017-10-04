using System;
using SuppressMessage = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
using System.IO;
using System.IO.Compression;

namespace Helper
{
    public static class GZip
    {
        private static readonly byte[] MagicNumber = new byte[] { 0x1F, 0x8B };

        public static bool HasMagicNumber(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length < MagicNumber.Length)
                return false;

            for (int i = MagicNumber.Length; --i >= 0;)
                if (data[i] != MagicNumber[i])
                    return false;

            return true;
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times",
            Justification = "Code Analysis error. Object is properly disposed.")]
        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
                return null;

            // Return copy of data if no compression header exists.
            if (!HasMagicNumber(data))
            {
                var copy = new byte[data.Length];
                Array.Copy(data, copy, data.Length);
                return copy;
            }

            var count = 0;
            const int BufferSize = 0x1000;
            var buffer = new byte[BufferSize];

            using (MemoryStream memory = new MemoryStream())
            using (MemoryStream decompress = new MemoryStream(data))
            using (GZipStream gzip = new GZipStream(decompress, CompressionMode.Decompress, true))
            {
                while ((count = gzip.Read(buffer, 0, BufferSize)) > 0)
                    memory.Write(buffer, 0, count);
                return memory.ToArray();
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times",
            Justification = "Code Analysis error. Object is properly disposed.")]
        public static byte[] Compress(byte[] data)
        {
            if (data == null)
                return null;

            using (MemoryStream memory = new MemoryStream())
            using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
            {
                gzip.Write(data, 0, data.Length);
                return memory.ToArray();
            }
        }
    }
}