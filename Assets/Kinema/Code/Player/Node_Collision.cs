using UnityEngine;
using System;

public class Node_Collision : MonoBehaviour
{

    public static event Action<Collision, TreeNode<CharacterNode>> OnCollision = delegate { };

    public TreeNode<CharacterNode> Node;

    private void OnCollisionEnter(Collision other)
    {
        OnCollision(other, Node);
    }
}
