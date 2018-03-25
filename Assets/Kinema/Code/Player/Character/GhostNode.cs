using UnityEngine;

public class GhostNode
{
    public Transform transform { get; private set; }
    public Renderer renderer { get; private set; }

    public GhostNode() { }

    public void Init(Transform Transform, Renderer Renderer)
    {
        transform = Transform;
        renderer = Renderer;
    }
}