using UnityEngine;
using System.Collections.Generic;

public class TargetCharacter
{
    public _CharacterTree<TargetNode> tree { get; private set; } = new _CharacterTree<TargetNode>();
    public List<TreeNode<TargetNode>> nodeList { get; private set; } = new List<TreeNode<TargetNode>>();
    public void Init(Transform RootTransform)
    {
        tree.Head.Init(RootTransform.Find(tree.StringHead), RootTransform.Find(tree.StringHead).GetComponent<Renderer>());
        tree.Chest.Init(RootTransform.Find(tree.StringChest), RootTransform.Find(tree.StringChest).GetComponent<Renderer>());
        tree.Stomach.Init(RootTransform.Find(tree.StringStomach), RootTransform.Find(tree.StringStomach).GetComponent<Renderer>());

        tree.UpperArmLeft.Init(RootTransform.Find(tree.StringUpperArmLeft), RootTransform.Find(tree.StringUpperArmLeft).GetComponent<Renderer>());
        tree.LowerArmLeft.Init(RootTransform.Find(tree.StringLowerArmLeft), RootTransform.Find(tree.StringLowerArmLeft).GetComponent<Renderer>());
        tree.HandLeft.Init(RootTransform.Find(tree.StringHandLeft), RootTransform.Find(tree.StringHandLeft).GetComponent<Renderer>());

        tree.UpperArmRight.Init(RootTransform.Find(tree.StringUpperArmRight), RootTransform.Find(tree.StringUpperArmRight).GetComponent<Renderer>());
        tree.LowerArmRight.Init(RootTransform.Find(tree.StringLowerArmRight), RootTransform.Find(tree.StringLowerArmRight).GetComponent<Renderer>());
        tree.HandRight.Init(RootTransform.Find(tree.StringHandRight), RootTransform.Find(tree.StringHandRight).GetComponent<Renderer>());

        tree.UpperLegLeft.Init(RootTransform.Find(tree.StringUpperLegLeft), RootTransform.Find(tree.StringUpperLegLeft).GetComponent<Renderer>());
        tree.LowerLegLeft.Init(RootTransform.Find(tree.StringLowerLegLeft), RootTransform.Find(tree.StringLowerLegLeft).GetComponent<Renderer>());
        tree.FootLeft.Init(RootTransform.Find(tree.StringFootLeft), RootTransform.Find(tree.StringFootLeft).GetComponent<Renderer>());

        tree.UpperLegRight.Init(RootTransform.Find(tree.StringUpperLegRight), RootTransform.Find(tree.StringUpperLegRight).GetComponent<Renderer>());
        tree.LowerLegRight.Init(RootTransform.Find(tree.StringLowerLegRight), RootTransform.Find(tree.StringLowerLegRight).GetComponent<Renderer>());
        tree.FootRight.Init(RootTransform.Find(tree.StringFootRight), RootTransform.Find(tree.StringFootRight).GetComponent<Renderer>());

        nodeList = tree.nodeList;
    }

    /// Applies a world-space keyframe to this character.
    public void SetKeyframe(Keyframe keyframe)
    {
        for (int i = 0; i < keyframe.tree.nodeList.Count; i++)
            tree.nodeList[i].Data.SetNodeKeyframe(keyframe.tree.nodeList[i].Data.GetNodeKeyframe());
    }

    /// Gets the current world-space keyframe of this character.
    public Keyframe GetKeyframe()
    {
        Keyframe currentKeyframe = new Keyframe();
        for (int i = 0; i < tree.nodeList.Count; i++)
            currentKeyframe.tree.nodeList[i].Data.SetNodeKeyframe(tree.nodeList[i].Data.GetNodeKeyframe());

        return currentKeyframe;
    }
}