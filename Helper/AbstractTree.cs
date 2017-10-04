using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public abstract class AbstractTree : IEnumerable<AbstractTree>
    {
        public AbstractTree Parent
        {
            get;
            set;
        }

        private List<AbstractTree> Children
        {
            get;
            set;
        }

        public AbstractTree this[int index]
        {
            get { return Children[index]; }
            set { Children[index] = value; }
        }

        public AbstractTree Root
        {
            get
            {
                var root = this;
                while (root.IsChildNode)
                    root = root.Parent;
                return root;
            }
        }

        public int Count
        {
            get
            {
                var count = Children.Count;
                foreach (var child in Children)
                    count += child.Count;
                return count + 1;
            }
        }

        public int Degree
        {
            get { return Children.Count; }
        }

        public bool IsRootNode
        {
            get { return Parent == null; }
        }
        public bool IsChildNode
        {
            get { return !IsRootNode; }
        }
        public bool IsLeaf
        {
            get { return Children.Count == 0; }
        }
        public bool IsExternalNode
        {
            get { return IsLeaf; }
        }
        public bool IsBranch
        {
            get { return !IsLeaf; }
        }
        public bool IsInternalNode
        {
            get { return !IsExternalNode; }
        }
        public int Level
        {
            get
            {
                var level = 1;
                var root = this;
                while (root.IsChildNode)
                {
                    root = root.Parent;
                    level++;
                }
                return level;
            }
        }
        public int Depth
        {
            get { return Level - 1; }
        }

        public int Height
        {
            get
            {
                if (IsLeaf)
                    return 0;

                var height = 0;
                foreach (var child in Children)
                    height = Math.Max(height, child.Height);
                return 1 + height;
            }
        }

        protected AbstractTree()
        {
            Children = new List<AbstractTree>();
        }
        protected AbstractTree(AbstractTree tree)
        {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));

            Children = tree.Children;
        }
        protected AbstractTree(ICollection<AbstractTree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Children = new List<AbstractTree>(children);
            Children.RemoveAll(node => node == null);
        }

        public bool IsSibling(AbstractTree sibling)
        {
            if (sibling == null)
                throw new ArgumentNullException(nameof(sibling));
            if (IsRootNode)
                return false;
            return Parent.Children.Contains(sibling);
        }

        public bool IsAncestor(AbstractTree ancestor)
        {
            if (ancestor == null)
                throw new ArgumentNullException(nameof(ancestor));
            var root = this;
            while (root.IsChildNode)
                if ((root = root.Parent) == ancestor)
                    return true;
            return false;
        }

        public bool IsDescendant(AbstractTree descendant)
        {
            return Contains(descendant);
        }

        public bool Contains(AbstractTree value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Root != Root)
                return false;
            return value.Level < Level;
        }

        public void Add(AbstractTree value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Remove(value);
            Children.Add(value);
        }

        public bool Remove(AbstractTree value)
        {
            if (value == null)
                return false;
            if (!Contains(value))
                return false;
            if (!value.Parent.Children.Remove(value))
                return false;

            return true;
        }

        public void Clear()
        {
            foreach (var tree in Children)
                tree.Clear();
            Children.Clear();
        }

        public AbstractTree[] GetChildren()
        {
            return Children.ToArray();
        }

        public AbstractTree[] GetPath(AbstractTree descendant)
        {
            if (descendant == null)
                throw new ArgumentNullException(nameof(descendant));
            if (!IsDescendant(descendant))
                return null;

            var path = new List<AbstractTree>();
            do
                path.Add(descendant);
            while ((descendant = descendant.Parent) != this);
            path.Add(this);
            return path.ToArray();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        IEnumerator<AbstractTree> IEnumerable<AbstractTree>.GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<AbstractTree>
        {
            private AbstractTree Tree
            {
                get;
                set;
            }

            public AbstractTree Current
            {
                get;
                private set;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            private Stack<Position> Stack
            {
                get;
                set;
            }

            internal Enumerator(AbstractTree tree)
            {
                if (tree == null)
                    throw new ArgumentNullException(nameof(tree));

                Tree = tree;
                Current = default(AbstractTree);
                Stack = new Stack<Position>();
                Reset();
            }

            public bool MoveNext()
            {
                if (Stack.Count == 0)
                    return false;

                var position = Stack.Pop();
                Current = position.Tree;
                if (Current.IsBranch)
                    PushNextChild(position);
                else
                {
                    while (Stack.Count > 0)
                    {
                        position = Stack.Pop();
                        if (position.Index < position.Tree.Degree)
                        {
                            PushNextChild(position);
                            break;
                        }
                    }
                }

                return true;
            }

            private void PushNextChild(Position position)
            {
                Stack.Push(position);
                Stack.Push(new Position(position.Tree.Children[position.Index++], 0));
            }

            public void Dispose()
            {
                // Do nothing
            }

            public void Reset()
            {
                Stack.Clear();
                Stack.Push(new Position(Tree, 0));
            }

            private class Position
            {
                public AbstractTree Tree
                {
                    get;
                    private set;
                }
                public int Index
                {
                    get;
                    set;
                }

                public Position(AbstractTree tree, int index)
                {
                    Tree = tree;
                    Index = index;
                }
            }
        }
    }
}
