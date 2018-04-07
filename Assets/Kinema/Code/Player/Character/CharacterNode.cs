using UnityEngine;
using System;

public class CharacterNode : IKeyframeNode
{
    public int Index { get; private set; }
    public Character Character { get; private set; }
    public float Spring { get; private set; }
    public float Damper { get; private set; }
    public Quaternion OriginalRotation { get; private set; }
    public float MaxImpactForce { get; private set; }
    public int CoordX { get; private set; }
    public int CoordY { get; private set; }
    public Transform Transform { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Renderer Renderer { get; private set; }
    public ConfigurableJoint Joint { get; private set; }
    public float Damage { get; private set; } = 0;

    public void SetDamage(float damage) { Damage = damage; }
    public void SetIndex(int index) { Index = index; }

    public void Init(Character Character, float Spring, float Damper, float MaxImpactForce, int CoordX, int CoordY, Transform Transform)
    {
        this.Character = Character;
        this.Spring = Spring;
        this.Damper = Damper;
        this.MaxImpactForce = MaxImpactForce;
        this.CoordX = CoordX;
        this.CoordY = CoordY;
        this.Transform = Transform;
        OriginalRotation = this.Transform.rotation;
        Rigidbody = this.Transform.GetComponent<Rigidbody>();
        Renderer = this.Transform.GetComponent<Renderer>();
        if (this.Transform.GetComponent<ConfigurableJoint>() != null)
            Joint = this.Transform.GetComponent<ConfigurableJoint>();
    }

    public CharacterKeyframeNode GetNodeKeyframe()
    {
        CharacterKeyframeNode keyframeNode = new CharacterKeyframeNode(Vector3.zero, Transform.position, Transform.rotation);
        return keyframeNode;
    }

    public void SetNodeKeyframe(CharacterKeyframeNode keyframeNode)
    {
        Transform.position = keyframeNode.Position;
        Transform.rotation = keyframeNode.Rotation;
        Rigidbody.velocity = keyframeNode.Velocity;
    }
}