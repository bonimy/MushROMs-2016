using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public abstract class Node : IEnumerable<Node>
    {
        public Node Parent
        {
            get;
            set;
        }

        private List<Node> Children
        {
            get;
            set;
        }

        public Node Root
        {
            get
            {
                var root = this;
                while (root.IsChildNode)
                {
                    root = root.Parent;
                }

                return root;
            }
        }

        public int Count
        {
            get
            {
                var count = Children.Count;
                foreach (var node in Children)
                {
                    count += node.Count;
                }

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
                {
                    return 0;
                }

                var height = 0;
                foreach (var node in Children)
                {
                    height = Math.Max(height, node.Height);
                }

                return 1 + height;
            }
        }

        protected Node()
        {
            Children = new List<Node>();
        }

        protected Node(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            Children = node.Children;
        }

        protected Node(ICollection<Node> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }

            Children = new List<Node>(children);
        }

        public bool IsSibling(Node sibling)
        {
            if (sibling == null)
            {
                throw new ArgumentNullException(nameof(sibling));
            }

            if (IsRootNode)
            {
                return false;
            }

            return Parent.Children.Contains(sibling);
        }

        public bool IsAncestor(Node ancestor)
        {
            if (ancestor == null)
            {
                throw new ArgumentNullException(nameof(ancestor));
            }

            var root = this;
            while (root.IsChildNode)
            {
                if ((root = root.Parent) == ancestor)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsDescendant(Node descendant)
        {
            return Contains(descendant);
        }

        public bool Contains(Node value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.Root != Root)
            {
                return false;
            }

            return value.Level < Level;
        }

        public bool Remove(Node value)
        {
            if (!Contains(value))
            {
                return false;
            }

            if (!value.Parent.Children.Remove(value))
            {
                return false;
            }

            return true;
        }

        public void Clear()
        {
            foreach (var node in Children)
            {
                node.Clear();
            }

            Children.Clear();
        }

        public Node[] GetChildren()
        {
            return Children.ToArray();
        }

        public Node[] GetPath(Node descendant)
        {
            if (descendant == null)
            {
                throw new ArgumentNullException(nameof(descendant));
            }

            if (!IsDescendant(descendant))
            {
                return null;
            }

            var path = new List<Node>();
            do
            {
                path.Add(descendant);
            }
            while ((descendant = descendant.Parent) != this);
            path.Add(this);
            return path.ToArray();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<Node>
        {
            private Node Tree
            {
                get;
                set;
            }

            public Node Current
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

            internal Enumerator(Node tree)
            {
                Tree = tree ?? throw new ArgumentNullException(nameof(tree));
                Current = default(Node);
                Stack = new Stack<Position>();
                Reset();
            }

            public bool MoveNext()
            {
                if (Stack.Count == 0)
                {
                    return false;
                }

                var path = Stack.Pop();
                Current = path.Node;
                if (Current.IsBranch)
                {
                    PushNextChild(path);
                }
                else
                {
                    while (Stack.Count > 0)
                    {
                        path = Stack.Pop();
                        if (path.Index < path.Node.Degree)
                        {
                            PushNextChild(path);
                            break;
                        }
                    }
                }

                return true;
            }

            private void PushNextChild(Position path)
            {
                Stack.Push(path);
                Stack.Push(new Position(path.Node.Children[path.Index++], 0));
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
                public Node Node
                {
                    get;
                    private set;
                }

                public int Index
                {
                    get;
                    set;
                }

                public Position(Node node, int index)
                {
                    Node = node;
                    Index = index;
                }
            }
        }
    }
}
