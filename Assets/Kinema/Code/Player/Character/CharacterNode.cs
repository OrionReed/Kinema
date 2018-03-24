using UnityEngine;
using System;

public class CharacterNode
{
    public Character character { get; private set; }
    public float spring { get; private set; }
    public float damper { get; private set; }
    public Quaternion originalRotation { get; private set; }
    public float maxImpactForce { get; private set; }
    public int coordX { get; private set; }
    public int coordY { get; private set; }
    public Transform transform { get; private set; }
    public Rigidbody rigidbody { get; private set; }
    public Renderer renderer { get; private set; }
    public ConfigurableJoint joint { get; private set; }
    public float damage { get; private set; } = 0;
    public NodeSelectionState selectionState { get; private set; } = NodeSelectionState.None;

    public void SetDamage(float Damage) { damage = Damage; }
    public void SetSelectionState(NodeSelectionState state) { selectionState = state; }

    public void Init(Character Character, float Spring, float Damper, float MaxImpactForce, int CoordX, int CoordY, Transform Transform)
    {
        character = Character;
        spring = Spring;
        damper = Damper;
        maxImpactForce = MaxImpactForce;
        coordX = CoordX;
        coordY = CoordY;
        transform = Transform;
        originalRotation = transform.rotation;
        rigidbody = transform.GetComponent<Rigidbody>();
        renderer = transform.GetComponent<Renderer>();
        if (transform.GetComponent<ConfigurableJoint>() != null)
            joint = transform.GetComponent<ConfigurableJoint>();
    }
}