using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Helper
{
    [Serializable]
    public class ExtensionDictionary<T> : Dictionary<string, T>
    {
        public ExtensionDictionary() :
            base(new ExtensionComparer())
        { }

        public ExtensionDictionary(ExtensionDictionary<T> dictionary) :
            base(dictionary, new ExtensionComparer())
        { }

        public ExtensionDictionary(int capacity) :
            base(capacity, new ExtensionComparer())
        { }

        protected ExtensionDictionary(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }

        public new T this[string key]
        {
            get
            {
                AssertExtension(key);
                return base[key];
            }

            set
            {
                AssertExtension(key);
                base[key] = value;
            }
        }

        public new void Add(string key, T value)
        {
            AssertExtension(key);
            base.Add(key, value);
        }

        public new bool ContainsKey(string key)
        {
            AssertExtension(key);
            return base.ContainsKey(key);
        }

        public new bool Remove(string key)
        {
            AssertExtension(key);
            return base.Remove(key);
        }

        public new bool TryGetValue(string key, out T value)
        {
            AssertExtension(key);
            return base.TryGetValue(key, out value);
        }

        private static void AssertExtension(string key)
        {
            if (!IOHelper.IsValidExtension(key))
            {
                throw new ArgumentException(SR.ErrorInvalidExtensionName(key), nameof(key));
            }
        }
    }
}
