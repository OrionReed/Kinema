using UnityEngine;

public class TargetNode : IKeyframeNode
{
    public Transform transform { get; private set; }
    public Renderer renderer { get; private set; }

    public TargetNode() { }

    public void Init(Transform Transform, Renderer Renderer)
    {
        transform = Transform;
        renderer = Renderer;
    }

    public KeyframeNode GetNodeKeyframe()
    {
        KeyframeNode keyframeNode = new KeyframeNode(Vector3.zero, transform.position, transform.rotation);
        return keyframeNode;
    }

    public void SetNodeKeyframe(KeyframeNode keyframeNode)
    {
        transform.position = keyframeNode.position;
        transform.rotation = keyframeNode.rotation;
    }
}