using System.Collections.Generic;
using UnityEngine;

public class _CharacterTree<T> where T : new()
{
    public TreeNode<T> root { get; private set; }
    public List<TreeNode<T>> nodeList { get; private set; }

    public T Head { get; private set; } = new T();
    public T Chest { get; private set; } = new T();
    public T Stomach { get; private set; } = new T();
    public T UpperArmLeft { get; private set; } = new T();
    public T LowerArmLeft { get; private set; } = new T();
    public T HandLeft { get; private set; } = new T();
    public T UpperArmRight { get; private set; } = new T();
    public T LowerArmRight { get; private set; } = new T();
    public T HandRight { get; private set; } = new T();
    public T UpperLegLeft { get; private set; } = new T();
    public T LowerLegLeft { get; private set; } = new T();
    public T FootLeft { get; private set; } = new T();
    public T UpperLegRight { get; private set; } = new T();
    public T LowerLegRight { get; private set; } = new T();
    public T FootRight { get; private set; } = new T();

    public string StringHead { get; private set; } = "Head";
    public string StringChest { get; private set; } = "Chest";
    public string StringStomach { get; private set; } = "Stomach";
    public string StringUpperArmLeft { get; private set; } = "UpperArmLeft";
    public string StringLowerArmLeft { get; private set; } = "LowerArmLeft";
    public string StringHandLeft { get; private set; } = "HandLeft";
    public string StringUpperArmRight { get; private set; } = "UpperArmRight";
    public string StringLowerArmRight { get; private set; } = "LowerArmRight";
    public string StringHandRight { get; private set; } = "HandRight";
    public string StringUpperLegLeft { get; private set; } = "UpperLegLeft";
    public string StringLowerLegLeft { get; private set; } = "LowerLegLeft";
    public string StringFootLeft { get; private set; } = "FootLeft";
    public string StringUpperLegRight { get; private set; } = "UpperLegRight";
    public string StringLowerLegRight { get; private set; } = "LowerLegRight";
    public string StringFootRight { get; private set; } = "FootRight";

    public _CharacterTree()
    {
        root = new TreeNode<T>(Stomach);
        root.AddChild(UpperLegLeft).AddChild(LowerLegLeft).AddChild(FootLeft);
        root.AddChild(UpperLegRight).AddChild(LowerLegRight).AddChild(FootRight);
        var c = root.AddChild(Chest);
        c.AddChild(Head);
        c.AddChild(UpperArmLeft).AddChild(LowerArmLeft).AddChild(HandLeft);
        c.AddChild(UpperArmRight).AddChild(LowerArmRight).AddChild(HandRight);

        nodeList = new List<TreeNode<T>>();
        nodeList.Add(root);
        nodeList.AddRange(_CharacterTree<T>.GetChildren(root));
    }

    /// Returns an array with all subsequent children of this node, excluding this node.
    public static List<TreeNode<T>> GetChildren(TreeNode<T> node)
    {
        List<TreeNode<T>> nodes = new List<TreeNode<T>>();
        foreach (var childNode in node.Children)
        {
            nodes.Add(childNode);
            nodes.AddRange(GetChildren(childNode));
        }
        return nodes;
    }
}
