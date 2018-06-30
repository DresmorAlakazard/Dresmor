using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Dresmor.System
{
    public class Node : IEnumerable<Node>
    {
        // Private Fields
        private Node root;
        private Node parent;
        private Node next;
        private Node prev;
        private Node first;
        private Node last;
        private bool roots = false;
        private List<Node> pathNodes;
        private List<int> pathLevels;
        private string name;

        // Public Fields
        public Node Root { get => root; }
        public Node Next { get => next; }
        public Node Prev { get => prev; }
        public Node First { get => first; }
        public Node Last { get => last; }
        public List<Node> PathNodes { get => pathNodes; }
        public List<int> PathLevels { get => pathLevels; }
        public bool Roots {
            get => roots;
            set
            {
                if (roots == value) return;
                roots = value;
                RootsChanged.Call(this, value);
                Layer((Node node) => { node.EnsureRoot(roots ? this : root, false, false); return 0; });
            }
        }
        public virtual Node Parent {
            get => parent;
            set
            {
                if (parent == value || parent == this || IsAncestorOf(value)) return;

                if (parent != null)
                {
                    if (prev == null) parent.first = next;
                    else prev.next = next;
                    if (next == null) parent.last = prev;
                    else next.prev = prev;
                }

                next = null;
                prev = null;

                var oldParent = parent;

                if ((parent = value) != null)
                {
                    pathNodes = new List<Node>(parent.pathNodes);
                    pathLevels = new List<int>(parent.pathLevels);

                    pathNodes.Add(this);

                    if ((prev = parent.last) != null)
                    {
                        prev.next = this;
                        pathLevels.Add(prev.pathLevels.Last() + 1);
                    }
                    else
                    {
                        parent.first = this;
                        pathLevels.Add(0);
                    }

                    parent.last = this;
                }
                else
                {
                    pathNodes = new List<Node>(new[] { this });
                    pathLevels = new List<int>(new[] { 0 });
                }

                ParentChanged.Call(this, new[] { parent, oldParent });

                EnsureRoot(parent?.root);
            }
        }
        public string Name {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                NameChanged.Call(this, value);
            }
        }

        // Public Events
        public DresmorHandler<Node[]> ParentChanged = new DresmorHandler<Node[]>();
        public DresmorHandler<Node[]> RootChanged = new DresmorHandler<Node[]>();
        public DresmorHandler<bool> RootsChanged = new DresmorHandler<bool>();
        public DresmorHandler<string> NameChanged = new DresmorHandler<string>();

        // Private Methods
        private void EnsureRoot(Node newRoot = null, bool reportSubs = true, bool search = true)
        {
            if (newRoot == root && (root != null || !search)) return;
            if (newRoot == null && search)
            {
                for (Node it = parent; it != null; it = it.parent)
                {
                    if (it.roots)
                    {
                        newRoot = it;
                        break;
                    }
                }
            }
            var oldRoot = root;
            root = newRoot;
            RootChanged.Call(this, new [] { root, oldRoot });
            if (reportSubs && !roots) Layer((Node node) => { node.EnsureRoot(root, false, false); return 0; });
        }

        // Public Methods
        public int ComparePath(Node node)
        {
            return ComparePath(node.pathLevels);
        }
        public int ComparePath(List<int> otherLevels)
        {
            int c = Math.Min(otherLevels.Count, pathLevels.Count);
            for (int i = 0; i < c; ++i)
            {
                if (pathLevels[i] > otherLevels[i]) return -1;
                else if (pathLevels[i] < otherLevels[i]) return 1;
            }
            return 0;
        }
        public bool IsAncestorOf(Node node)
        {
            if (node == null || pathNodes.Count >= node.pathNodes.Count) return false;
            for (int i = 0; i < pathNodes.Count; ++i) if (pathNodes[i] != node.pathNodes[i]) return false;
            return true;
        }
        public bool IsDescendantOf(Node node)
        {
            if (node == null) return true;
            return node.IsAncestorOf(this);
        }
        public List<Node> GetNodes() { return GetNodes<Node>(); }
        public List<T> GetNodes<T>() where T : class
        {
            List<T> result = new List<T>();
            foreach (Node it in this) if (it is T) result.Add(it as T);
            return result;
        }
        public void Layer<T>(Func<T, int> func) where T : class
        {
            List<T> nodes = GetNodes<T>();
            for (int i = 0; i < nodes.Count; ++i)
            {
                switch (func(nodes[i]))
                {
                    case 0: foreach (Node it in nodes[i] as Node) if (it is T) nodes.Add(it as T); break;
                    case -1: i = nodes.Count; break;
                }
            }
        }

        public IEnumerator<Node> GetEnumerator()
        {
            for (Node it = first; it != null; it = it.next)
            {
                yield return it;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Public Constructor(s)
        public Node()
        {
            pathNodes = new List<Node>(new[] { this });
            pathLevels = new List<int>(new[] { 0 });
        }

        // Operators
        static public Node operator +(Node a, Node b)
        {
            b.Parent = a;
            return a;
        }

        public Node this[int index]
        {
            get {
                foreach (Node it in this)
                {
                    if (index == 0) return it;
                    --index;
                }
                return null;
            }
        }

        public Node this[string index]
        {
            get
            {
                foreach (Node it in this)
                {
                    if (it.name == index)
                    {
                        return it;
                    }
                }
                return null;
            }
        }
    }
}
