using UnityEngine;
using System;

public class Node_Collision : MonoBehaviour
{

    public static event Action<Collision, TreeNode<CharacterNode>> OnCollision = delegate { };

    public TreeNode<CharacterNode> node;

    private void OnCollisionEnter(Collision other)
    {
        OnCollision(other, node);
    }
}
