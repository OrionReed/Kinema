using UnityEngine;

public class PoseNode
{
    public Vector3 velocity;
    public Vector3 position;
    public Quaternion rotation;

    public PoseNode() { }
    public PoseNode(Vector3 Velocity, Vector3 Position, Quaternion Rotation)
    {
        velocity = Velocity;
        position = Position;
        rotation = Rotation;
    }
}