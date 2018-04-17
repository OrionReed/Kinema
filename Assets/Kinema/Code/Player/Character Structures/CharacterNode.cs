using UnityEngine;
using System;

public class CharacterNode : IKeyframeNode
{
    public int Index { get; private set; }
    public Character Character { get; private set; }
    public Quaternion OriginalRotation { get; private set; }
    public int CoordX { get; private set; }
    public int CoordY { get; private set; }
    public Transform Transform { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Renderer Renderer { get; private set; }
    public ConfigurableJoint Joint { get; private set; }
    public Vector3 ControlAxisPrimary { get; private set; }
    public Vector3 ControlAxisSecondary { get; private set; }
    public float DamageMax { get; private set; }

    public float DamageCurrent { get; private set; }
    public float Spring { get; private set; }
    public float Damper { get; private set; }

    public void SetDamage(float damage) { DamageCurrent = Mathf.Clamp(damage, 0, DamageMax); }
    public void SetIndex(int index) { Index = index; }

    public void Init(Character character, float damageMax, int coordX, int coordY, Vector3 axisPrimary, Vector3 axisSecondary, Transform transform)
    {
        Character = character;
        DamageMax = damageMax;
        CoordX = coordX;
        CoordY = coordY;
        ControlAxisPrimary = axisPrimary;
        ControlAxisSecondary = axisSecondary;
        Transform = transform;
        OriginalRotation = Transform.rotation;
        Rigidbody = Transform.GetComponent<Rigidbody>();
        Renderer = Transform.GetComponent<Renderer>();
        if (Transform.GetComponent<ConfigurableJoint>() != null)
            Joint = Transform.GetComponent<ConfigurableJoint>();
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