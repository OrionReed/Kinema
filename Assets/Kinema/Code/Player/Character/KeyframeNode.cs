using UnityEngine;

public class KeyframeNode : IKeyframeNode
{
    public Vector3 velocity;
    public Vector3 position;
    public Quaternion rotation;

    public KeyframeNode() { }
    public KeyframeNode(Vector3 Velocity, Vector3 Position, Quaternion Rotation)
    {
        velocity = Velocity;
        position = Position;
        rotation = Rotation;
    }

    public KeyframeNode GetNodeKeyframe()
    {
        return this;
    }

    public void SetNodeKeyframe(KeyframeNode keyframeNode)
    {
        position = keyframeNode.position;
        rotation = keyframeNode.rotation;
        velocity = keyframeNode.velocity;
    }
}