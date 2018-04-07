using UnityEngine;

public class CharacterKeyframe
{
    public _CharacterTree<CharacterKeyframeNode> Tree { get; private set; } = new _CharacterTree<CharacterKeyframeNode>();

    public static void ModifyForce(CharacterKeyframe keyframe, Vector3 direction, float force, float randomness)
    {
        foreach (TreeNode<CharacterKeyframeNode> node in keyframe.Tree.NodeList)
            node.Data.Velocity = direction.normalized * UnityEngine.Random.Range(force - randomness, force + randomness);
    }
}
