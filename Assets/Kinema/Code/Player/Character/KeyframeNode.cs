using UnityEngine;

public class KeyframeNode : IKeyframeNode
{
    public Vector3 Velocity { get; set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    public KeyframeNode() { }
    public KeyframeNode(Vector3 velocity, Vector3 position, Quaternion rotation)
    {
        this.Velocity = velocity;
        this.Position = position;
        this.Rotation = rotation;
    }

    public KeyframeNode GetNodeKeyframe()
    {
        return this;
    }

    public void SetNodeKeyframe(KeyframeNode keyframeNode)
    {
        Position = keyframeNode.Position;
        Rotation = keyframeNode.Rotation;
        Velocity = keyframeNode.Velocity;
    }
}