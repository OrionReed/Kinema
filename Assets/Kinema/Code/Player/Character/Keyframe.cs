using UnityEngine;

public class Keyframe
{
    public _CharacterTree<KeyframeNode> Tree { get; private set; } = new _CharacterTree<KeyframeNode>();

    public static void ModifyForce(Keyframe keyframe, Vector3 direction, float force, float randomness)
    {
        foreach (TreeNode<KeyframeNode> node in keyframe.Tree.NodeList)
            node.Data.Velocity = direction.normalized * UnityEngine.Random.Range(force - randomness, force + randomness);
    }
}
