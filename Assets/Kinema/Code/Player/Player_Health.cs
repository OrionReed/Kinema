using UnityEngine;
using System;

public class Player_Health : MonoBehaviour
{
    public event Action<CharacterNode> OnNodeDamaged = delegate { };
    public event Action OnPlayerDeath = delegate { };
    [SerializeField]
    private AnimationCurve impactForce;

    private void Start()
    {
        Node_Collision.OnCollision += Collided;
    }

    public void Collided(Collision collision, TreeNode<CharacterNode> node)
    {
        node.Data.SetDamage(Mathf.Clamp(collision.relativeVelocity.magnitude / node.Data.maxImpactForce, 0, 1));
        OnNodeDamaged(node.Data);
        if (node.Data.damage == 1)
            OnPlayerDeath();
    }
}
