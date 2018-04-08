using UnityEngine;

public class CharacterKeyframeNode : IKeyframeNode
{
    public Vector3 Velocity { get; set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    public CharacterKeyframeNode() { }
    public CharacterKeyframeNode(Vector3 velocity, Vector3 position, Quaternion rotation)
    {
        this.Velocity = velocity;
        this.Position = position;
        this.Rotation = rotation;
    }

    public CharacterKeyframeNode GetNodeKeyframe()
    {
        return this;
    }

    public void SetNodeKeyframe(CharacterKeyframeNode keyframeNode)
    {
        Position = keyframeNode.Position;
        Rotation = keyframeNode.Rotation;
        Velocity = keyframeNode.Velocity;
    }
}