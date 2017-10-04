using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Helper
{
    [Serializable]
    public class FileFormatException : Exception
    {
        public string Path
        {
            get;
            private set;
        }

        public FileFormatException() :
            this(null)
        { }
        public FileFormatException(string path) :
            this(path, SR.ErrorFileFormat(path))
        { }
        public FileFormatException(string path, string message) :
            base(message)
        {
            Path = path;
        }

        public FileFormatException(string message, Exception innerException) :
            base(message, innerException)
        { }
        protected FileFormatException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Path = info.GetString(nameof(Path));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);
            info.AddValue(nameof(Path), Path, Path.GetType());
        }
    }
}
