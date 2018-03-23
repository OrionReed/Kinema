using UnityEngine;
using System;

public class Node_Health : MonoBehaviour
{
    public event Action<CharacterNode> OnImpact = delegate { };
    public static event Action OnDeath = delegate { };
    public static bool dead = false;
    [SerializeField]
    private AnimationCurve impactForce;

    private void Start()
    {
        _Input.OnKeyRestartScene += delegate { dead = false; };
        for (int i = 0; i < CharacterSelection.currentCharacter.nodeList.Count; i++)
            CharacterSelection.currentCharacter.nodeList[i].Data.transform.gameObject.AddComponent<NodeHealth_CollisionDetection>().health = this;
    }

    public void Collided(Collision collision, GameObject obj)
    {
        CharacterNode node = CharacterUtils.GetFromTransform(CharacterSelection.currentCharacter, obj.transform).Data;
        node.SetDamage(Mathf.Clamp(collision.relativeVelocity.magnitude / node.maxImpactForce, 0, 1));
        OnImpact(node);
        if (node.damage == 1)
        {
            dead = true;
            OnDeath();
        }
    }
}
