using UnityEngine;

public class Keyframe
{
    public _CharacterTree<KeyframeNode> tree { get; private set; } = new _CharacterTree<KeyframeNode>();

    public static void ModifyForce(Keyframe keyframe, Vector3 Direction, float Force, float Randomness)
    {
        foreach (TreeNode<KeyframeNode> node in keyframe.tree.nodeList)
            node.Data.velocity = Direction.normalized * UnityEngine.Random.Range(Force - Randomness, Force + Randomness);
    }
}
