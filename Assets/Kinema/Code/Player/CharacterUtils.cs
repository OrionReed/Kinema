using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterUtils
{
    /// Center of mass in world-space.
    public static Vector3 GetCenterOfMass(Character character)
    {
        Vector3 CoM = Vector3.zero;
        float sumOfWeights = 0f;

        foreach (TreeNode<CharacterNode> node in character.nodeList)
        {
            Rigidbody r = node.Data.rigidbody;
            CoM += r.worldCenterOfMass * r.mass;
            sumOfWeights += r.mass;
        }
        CoM /= sumOfWeights;
        return CoM;
    }
    /// Returns node in character this transform belongs to.
    public static TreeNode<CharacterNode> GetFromTransform(Character character, Transform transform)
    {
        if (transform == null)
            return null;
        if (transform.tag == "Player")
        {
            for (int i = 0; i < character.nodeList.Count; i++)
            {
                if (character.nodeList[i].Data.transform == transform)
                    return character.nodeList[i];
            }
        }
        return null;
    }
    /// Returns opposite node on X axis (left / right)
    public static TreeNode<CharacterNode> GetOpposite(TreeNode<CharacterNode> node)
    {
        List<TreeNode<CharacterNode>> nodeList = node.Data.character.nodeList;

        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Data.coordX == -node.Data.coordX)
                if (nodeList[i].Data.coordY == node.Data.coordY)
                    if (nodeList[i].Level == node.Level)
                        return nodeList[i];
        }
        return node;
    }
    /// Returns an array with all subsequent children of this node, excluding this node.
    public static List<TreeNode<CharacterNode>> GetChildren(TreeNode<CharacterNode> node)
    {
        List<TreeNode<CharacterNode>> nodes = new List<TreeNode<CharacterNode>>();
        foreach (var childNode in node.Children)
        {
            nodes.Add(childNode);
            nodes.AddRange(GetChildren(childNode));
        }
        return nodes;
    }
}