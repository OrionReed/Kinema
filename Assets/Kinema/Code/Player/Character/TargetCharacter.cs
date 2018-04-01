using UnityEngine;
using System.Collections.Generic;

public class TargetCharacter
{
    public _CharacterTree<TargetNode> Tree { get; private set; } = new _CharacterTree<TargetNode>();
    public List<TreeNode<TargetNode>> List { get; private set; } = new List<TreeNode<TargetNode>>();
    public void Init(Transform rootTransform)
    {
        Tree.Head.Init(rootTransform.Find(Tree.StringHead), rootTransform.Find(Tree.StringHead).GetComponent<Renderer>());
        Tree.Chest.Init(rootTransform.Find(Tree.StringChest), rootTransform.Find(Tree.StringChest).GetComponent<Renderer>());
        Tree.Stomach.Init(rootTransform.Find(Tree.StringStomach), rootTransform.Find(Tree.StringStomach).GetComponent<Renderer>());

        Tree.UpperArmLeft.Init(rootTransform.Find(Tree.StringUpperArmLeft), rootTransform.Find(Tree.StringUpperArmLeft).GetComponent<Renderer>());
        Tree.LowerArmLeft.Init(rootTransform.Find(Tree.StringLowerArmLeft), rootTransform.Find(Tree.StringLowerArmLeft).GetComponent<Renderer>());
        Tree.HandLeft.Init(rootTransform.Find(Tree.StringHandLeft), rootTransform.Find(Tree.StringHandLeft).GetComponent<Renderer>());

        Tree.UpperArmRight.Init(rootTransform.Find(Tree.StringUpperArmRight), rootTransform.Find(Tree.StringUpperArmRight).GetComponent<Renderer>());
        Tree.LowerArmRight.Init(rootTransform.Find(Tree.StringLowerArmRight), rootTransform.Find(Tree.StringLowerArmRight).GetComponent<Renderer>());
        Tree.HandRight.Init(rootTransform.Find(Tree.StringHandRight), rootTransform.Find(Tree.StringHandRight).GetComponent<Renderer>());

        Tree.UpperLegLeft.Init(rootTransform.Find(Tree.StringUpperLegLeft), rootTransform.Find(Tree.StringUpperLegLeft).GetComponent<Renderer>());
        Tree.LowerLegLeft.Init(rootTransform.Find(Tree.StringLowerLegLeft), rootTransform.Find(Tree.StringLowerLegLeft).GetComponent<Renderer>());
        Tree.FootLeft.Init(rootTransform.Find(Tree.StringFootLeft), rootTransform.Find(Tree.StringFootLeft).GetComponent<Renderer>());

        Tree.UpperLegRight.Init(rootTransform.Find(Tree.StringUpperLegRight), rootTransform.Find(Tree.StringUpperLegRight).GetComponent<Renderer>());
        Tree.LowerLegRight.Init(rootTransform.Find(Tree.StringLowerLegRight), rootTransform.Find(Tree.StringLowerLegRight).GetComponent<Renderer>());
        Tree.FootRight.Init(rootTransform.Find(Tree.StringFootRight), rootTransform.Find(Tree.StringFootRight).GetComponent<Renderer>());

        List = Tree.NodeList;
    }

    /// Applies a world-space keyframe to this character.
    public void SetKeyframe(Keyframe keyframe)
    {
        for (int i = 0; i < keyframe.Tree.NodeList.Count; i++)
            Tree.NodeList[i].Data.SetNodeKeyframe(keyframe.Tree.NodeList[i].Data.GetNodeKeyframe());
    }

    /// Gets the current world-space keyframe of this character.
    public Keyframe GetKeyframe()
    {
        Keyframe currentKeyframe = new Keyframe();
        for (int i = 0; i < Tree.NodeList.Count; i++)
            currentKeyframe.Tree.NodeList[i].Data.SetNodeKeyframe(Tree.NodeList[i].Data.GetNodeKeyframe());

        return currentKeyframe;
    }
}