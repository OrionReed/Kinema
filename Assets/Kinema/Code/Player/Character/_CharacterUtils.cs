using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class _CharacterUtils
{
    /// Center of mass in world-space.
    public static Vector3 GetCenterOfMass(Character character)
    {
        Vector3 CoM = Vector3.zero;
        float sumOfWeights = 0f;

        foreach (TreeNode<CharacterNode> node in character.tree.nodeList)
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
            for (int i = 0; i < character.tree.nodeList.Count; i++)
            {
                if (character.tree.nodeList[i].Data.transform == transform)
                    return character.tree.nodeList[i];
            }
        }
        return null;
    }
    /// Returns opposite node on X axis (left / right)
    public static TreeNode<CharacterNode> GetOpposite(TreeNode<CharacterNode> node)
    {
        List<TreeNode<CharacterNode>> nodeList = node.Data.character.tree.nodeList;

        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Data.coordX == -node.Data.coordX)
                if (nodeList[i].Data.coordY == node.Data.coordY)
                    if (nodeList[i].Level == node.Level)
                        return nodeList[i];
        }
        return node;
    }
}