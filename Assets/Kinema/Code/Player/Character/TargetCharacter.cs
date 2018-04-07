using UnityEngine;
using System.Collections.Generic;

public class TargetCharacter
{
    public _CharacterTree<TargetNode> CharacterTree { get; private set; } = new _CharacterTree<TargetNode>();
    public void Init(Transform rootTransform)
    {
        CharacterTree.Head.Init(rootTransform.Find(CharacterTree.StringHead), rootTransform.Find(CharacterTree.StringHead).GetComponent<Renderer>());
        CharacterTree.Chest.Init(rootTransform.Find(CharacterTree.StringChest), rootTransform.Find(CharacterTree.StringChest).GetComponent<Renderer>());
        CharacterTree.Stomach.Init(rootTransform.Find(CharacterTree.StringStomach), rootTransform.Find(CharacterTree.StringStomach).GetComponent<Renderer>());

        CharacterTree.UpperArmLeft.Init(rootTransform.Find(CharacterTree.StringUpperArmLeft), rootTransform.Find(CharacterTree.StringUpperArmLeft).GetComponent<Renderer>());
        CharacterTree.LowerArmLeft.Init(rootTransform.Find(CharacterTree.StringLowerArmLeft), rootTransform.Find(CharacterTree.StringLowerArmLeft).GetComponent<Renderer>());
        CharacterTree.HandLeft.Init(rootTransform.Find(CharacterTree.StringHandLeft), rootTransform.Find(CharacterTree.StringHandLeft).GetComponent<Renderer>());

        CharacterTree.UpperArmRight.Init(rootTransform.Find(CharacterTree.StringUpperArmRight), rootTransform.Find(CharacterTree.StringUpperArmRight).GetComponent<Renderer>());
        CharacterTree.LowerArmRight.Init(rootTransform.Find(CharacterTree.StringLowerArmRight), rootTransform.Find(CharacterTree.StringLowerArmRight).GetComponent<Renderer>());
        CharacterTree.HandRight.Init(rootTransform.Find(CharacterTree.StringHandRight), rootTransform.Find(CharacterTree.StringHandRight).GetComponent<Renderer>());

        CharacterTree.UpperLegLeft.Init(rootTransform.Find(CharacterTree.StringUpperLegLeft), rootTransform.Find(CharacterTree.StringUpperLegLeft).GetComponent<Renderer>());
        CharacterTree.LowerLegLeft.Init(rootTransform.Find(CharacterTree.StringLowerLegLeft), rootTransform.Find(CharacterTree.StringLowerLegLeft).GetComponent<Renderer>());
        CharacterTree.FootLeft.Init(rootTransform.Find(CharacterTree.StringFootLeft), rootTransform.Find(CharacterTree.StringFootLeft).GetComponent<Renderer>());

        CharacterTree.UpperLegRight.Init(rootTransform.Find(CharacterTree.StringUpperLegRight), rootTransform.Find(CharacterTree.StringUpperLegRight).GetComponent<Renderer>());
        CharacterTree.LowerLegRight.Init(rootTransform.Find(CharacterTree.StringLowerLegRight), rootTransform.Find(CharacterTree.StringLowerLegRight).GetComponent<Renderer>());
        CharacterTree.FootRight.Init(rootTransform.Find(CharacterTree.StringFootRight), rootTransform.Find(CharacterTree.StringFootRight).GetComponent<Renderer>());
    }

    /// Applies a world-space keyframe to this character.
    public void SetKeyframe(CharacterKeyframe keyframe)
    {
        for (int i = 0; i < keyframe.Tree.NodeList.Count; i++)
            CharacterTree.NodeList[i].Data.SetNodeKeyframe(keyframe.Tree.NodeList[i].Data.GetNodeKeyframe());
    }

    /// Gets the current world-space keyframe of this character.
    public CharacterKeyframe GetKeyframe()
    {
        CharacterKeyframe currentKeyframe = new CharacterKeyframe();
        for (int i = 0; i < CharacterTree.NodeList.Count; i++)
            currentKeyframe.Tree.NodeList[i].Data.SetNodeKeyframe(CharacterTree.NodeList[i].Data.GetNodeKeyframe());

        return currentKeyframe;
    }
}