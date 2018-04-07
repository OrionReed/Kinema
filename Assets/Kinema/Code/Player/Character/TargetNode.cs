using UnityEngine;

public class TargetNode : IKeyframeNode
{
    public Transform Transform { get; private set; }
    public Renderer Renderer { get; private set; }

    public TargetNode() { }

    public void Init(Transform transform, Renderer renderer)
    {
        this.Transform = transform;
        this.Renderer = renderer;
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
    }
}