using UnityEngine;

public class Pose
{
    public _CharacterTree<PoseNode> tree { get; private set; } = new _CharacterTree<PoseNode>();

    public static void ApplyPose(Pose Pose, Character character)
    {
        for (int i = 0; i < Pose.tree.nodeList.Count; i++)
        {
            character.tree.nodeList[i].Data.transform.position = Pose.tree.nodeList[i].Data.position;
            character.tree.nodeList[i].Data.transform.rotation = Pose.tree.nodeList[i].Data.rotation;
            character.tree.nodeList[i].Data.rigidbody.velocity = Pose.tree.nodeList[i].Data.velocity;
        }
    }
    public static Pose GetCharacterPose(Character character)
    {
        Pose currentPose = new Pose();
        for (int i = 0; i < character.tree.nodeList.Count; i++)
        {
            currentPose.tree.nodeList[i].Data.position = character.tree.nodeList[i].Data.transform.position;
            currentPose.tree.nodeList[i].Data.rotation = character.tree.nodeList[i].Data.transform.rotation;
            currentPose.tree.nodeList[i].Data.velocity = character.tree.nodeList[i].Data.rigidbody.velocity;
        }
        return currentPose;
    }
    public static void ModifyPoseForce(Pose pose, Vector3 Direction, float Force, float Randomness)
    {
        foreach (TreeNode<PoseNode> node in pose.tree.nodeList)
            node.Data.velocity = Direction.normalized * UnityEngine.Random.Range(Force - Randomness, Force + Randomness);
    }
}
