﻿using System;
using System.Collections.Generic;

public class TreeNode<T>
{
    private readonly T _data;
    private readonly TreeNode<T> _parent;
    private readonly int _level;
    private readonly List<TreeNode<T>> _children;

    public TreeNode(T data)
    {
        _data = data;
        _children = new List<TreeNode<T>>();
        _level = 0;
    }

    public TreeNode(T data, TreeNode<T> parent) : this(data)
    {
        _parent = parent;
        _level = _parent != null ? _parent.Level + 1 : 0;
    }

    public int Index { get; private set; }
    public int Level { get { return _level; } }
    public int Count { get { return _children.Count; } }
    public bool IsRoot { get { return _parent == null; } }
    public bool IsLeaf { get { return _children.Count == 0; } }
    public T Data { get { return _data; } }
    public TreeNode<T> Parent { get { return _parent; } }
    public List<TreeNode<T>> Children { get { return _children; } }

    public TreeNode<T> this[int key]
    {
        get { return _children[key]; }
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public void Clear()
    {
        _children.Clear();
    }

    public TreeNode<T> AddChild(T value)
    {
        TreeNode<T> node = new TreeNode<T>(value, this);
        _children.Add(node);

        return node;
    }

    public bool HasChild(T data)
    {
        return FindInChildren(data) != null;
    }

    public TreeNode<T> FindInChildren(T data)
    {
        int i = 0, l = Count;
        for (; i < l; ++i)
        {
            TreeNode<T> child = _children[i];
            if (child.Data.Equals(data)) return child;
        }
        return null;
    }

    public bool RemoveChild(TreeNode<T> node)
    {
        return _children.Remove(node);
    }
}