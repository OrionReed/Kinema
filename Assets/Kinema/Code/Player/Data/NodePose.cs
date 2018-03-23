using UnityEngine;
using System;

public class NodePose
{
    public TreeNode<CharacterNode> node;
    public Vector3 velocity;
    public Vector3 position;
    public Quaternion rotation;

    public NodePose(TreeNode<CharacterNode> Node, Vector3 Velocity, Vector3 Position, Quaternion Rotation)
    {
        node = Node;
        velocity = Velocity;
        position = Position;
        rotation = Rotation;
    }
}