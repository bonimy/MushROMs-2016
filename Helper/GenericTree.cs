using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Helper
{
    [DebuggerDisplay("{Value}")]
    public class GenericTree<T> : AbstractTree
    {
        public T Value
        {
            get;
            set;
        }

        public GenericTree() : base()
        { }

        public GenericTree(T value) : base()
        {
            Value = value;
        }

        public GenericTree(GenericTree<T> tree) : base(tree)
        {
            if (tree == null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            Value = tree.Value;
        }

        public GenericTree(ICollection<AbstractTree> children) : base(children)
        { }

        public GenericTree(T value, ICollection<AbstractTree> children) : this(children)
        {
            Value = value;
        }

        public void Add(T value)
        {
            Add(new GenericTree<T>(value));
        }
    }
}
