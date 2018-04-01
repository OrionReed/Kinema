using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Kinema/New Character")]
[Serializable]
public class Character : ScriptableObject
{
    public _CharacterTree<CharacterNode> Tree { get; private set; } = new _CharacterTree<CharacterNode>();
    public List<TreeNode<CharacterNode>> List { get; private set; } = new List<TreeNode<CharacterNode>>();

    [SerializeField]
    private float springDefault = 30;
    [SerializeField]
    private float damperDefault = 5;
    [SerializeField]
    private float maxImpact = 20;

    public void Init(Transform rootTransform)
    {
        Tree.Head.Init(this, springDefault, damperDefault, maxImpact, 0, 1, rootTransform.Find(Tree.StringHead));
        Tree.Chest.Init(this, springDefault, damperDefault, maxImpact, 0, 1, rootTransform.Find(Tree.StringChest));
        Tree.Stomach.Init(this, springDefault, damperDefault, maxImpact, 0, 0, rootTransform.Find(Tree.StringStomach));

        Tree.UpperArmLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, rootTransform.Find(Tree.StringUpperArmLeft));
        Tree.LowerArmLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, rootTransform.Find(Tree.StringLowerArmLeft));
        Tree.HandLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, rootTransform.Find(Tree.StringHandLeft));

        Tree.UpperArmRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, rootTransform.Find(Tree.StringUpperArmRight));
        Tree.LowerArmRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, rootTransform.Find(Tree.StringLowerArmRight));
        Tree.HandRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, rootTransform.Find(Tree.StringHandRight));

        Tree.UpperLegLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, rootTransform.Find(Tree.StringUpperLegLeft));
        Tree.LowerLegLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, rootTransform.Find(Tree.StringLowerLegLeft));
        Tree.FootLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, rootTransform.Find(Tree.StringFootLeft));

        Tree.UpperLegRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, rootTransform.Find(Tree.StringUpperLegRight));
        Tree.LowerLegRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, rootTransform.Find(Tree.StringLowerLegRight));
        Tree.FootRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, rootTransform.Find(Tree.StringFootRight));

        for (int i = 0; i < Tree.NodeList.Count; i++)
        {
            if (Tree.NodeList[i].Parent != null)
            {
                CharacterNode root = Tree.NodeList[i].Data;
                CharacterNode parent = Tree.NodeList[i].Parent.Data;
                root.Joint.connectedBody = parent.Rigidbody;
                JointDrive d = new JointDrive();
                d.positionSpring = root.Spring;
                d.positionDamper = root.Damper;
                d.maximumForce = Mathf.Infinity;
                root.Joint.slerpDrive = d;
                root.Joint.xMotion = ConfigurableJointMotion.Locked;
                root.Joint.yMotion = ConfigurableJointMotion.Locked;
                root.Joint.zMotion = ConfigurableJointMotion.Locked;
                root.Joint.rotationDriveMode = RotationDriveMode.Slerp;
            }
        }
        List = Tree.NodeList;

    }

    // UTILITIES //

    /// Center of mass in world-space.
    public Vector3 GetCenterOfMass()
    {
        Vector3 CoM = Vector3.zero;
        float sumOfWeights = 0f;

        foreach (TreeNode<CharacterNode> node in Tree.NodeList)
        {
            Rigidbody r = node.Data.Rigidbody;
            CoM += r.worldCenterOfMass * r.mass;
            sumOfWeights += r.mass;
        }
        CoM /= sumOfWeights;
        return CoM;
    }
    /// Returns node in character this transform belongs to.
    public TreeNode<CharacterNode> ContainsTransform(Transform transform)
    {
        for (int i = 0; i < Tree.NodeList.Count; i++)
        {
            if (Tree.NodeList[i].Data.Transform == transform)
                return Tree.NodeList[i];
        }
        return null;
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