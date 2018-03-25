using UnityEngine;

public class Ghost
{
    public _CharacterTree<GhostNode> tree { get; private set; } = new _CharacterTree<GhostNode>();
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
    }
    public GhostNode GetNode(int index)
    {
        return tree.nodeList[index].Data;
    }
    public TreeNode<GhostNode> GetTreeNode(int index)
    {
        return tree.nodeList[index];
    }
}