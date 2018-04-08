using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth_Damage : MonoBehaviour
{
    public event Action<CharacterNode> OnNodeDamaged = delegate { };

    [SerializeField]
    private float minDamageRegistered = 0.1f;

    private void Start()
    {
        Node_Collision.OnCollision += Collided;
    }

    public void Collided(Collision collision, TreeNode<CharacterNode> node)
    {
        float collisionMagnitude = collision.relativeVelocity.magnitude;
        if (collisionMagnitude > minDamageRegistered &&
            collisionMagnitude > node.Data.DamageCurrent)
        {
            node.Data.SetDamage(node.Data.DamageCurrent + collisionMagnitude);
            OnNodeDamaged(node.Data);
        }

    }
}
