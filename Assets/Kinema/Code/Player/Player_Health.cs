using UnityEngine;
using System;

public class Player_Health : MonoBehaviour
{
    public event Action<CharacterNode> OnNodeImpact = delegate { };
    public event Action OnPlayerDeath = delegate { };
    [SerializeField]
    private AnimationCurve impactForce;

    private void Start()
    {
        for (int i = 0; i < CharacterSelection.currentCharacter.tree.nodeList.Count; i++)
            CharacterSelection.currentCharacter.tree.nodeList[i].Data.transform.gameObject.AddComponent<NodeHealth_CollisionDetection>().health = this;
    }

    public void Collided(Collision collision, GameObject obj)
    {
        CharacterNode node = _CharacterUtils.GetFromTransform(CharacterSelection.currentCharacter, obj.transform).Data;
        node.SetDamage(Mathf.Clamp(collision.relativeVelocity.magnitude / node.maxImpactForce, 0, 1));
        OnNodeImpact(node);
        if (node.damage == 1)
            OnPlayerDeath();
    }
}
