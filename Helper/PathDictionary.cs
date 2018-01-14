using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Helper
{
    [Serializable]
    public class PathDictionary<T> : Dictionary<string, T>
    {
        public PathDictionary() :
            base(new PathComparer())
        { }

        public PathDictionary(PathDictionary<T> dictionary) :
            base(dictionary, new PathComparer())
        { }

        public PathDictionary(int capacity) :
            base(capacity, new PathComparer())
        { }

        protected PathDictionary(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }

        public new T this[string key]
        {
            get
            {
                Assert(key);
                return base[key];
            }

            set
            {
                Assert(key);
                base[key] = value;
            }
        }

        public new void Add(string key, T value)
        {
            Assert(key);
            base.Add(key, value);
        }

        public new bool ContainsKey(string key)
        {
            Assert(key);
            return base.ContainsKey(key);
        }

        public new bool Remove(string key)
        {
            Assert(key);
            return base.Remove(key);
        }

        public new bool TryGetValue(string key, out T value)
        {
            Assert(key);
            return base.TryGetValue(key, out value);
        }

        private static void Assert(string key)
        {
            Path.GetFullPath(key);
        }
    }
}
