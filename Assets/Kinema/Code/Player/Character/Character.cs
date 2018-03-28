using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Kinema/New Character")]
[Serializable]
public class Character : ScriptableObject
{
    public _CharacterTree<CharacterNode> tree { get; private set; } = new _CharacterTree<CharacterNode>();

    [SerializeField]
    private float springDefault = 30;
    [SerializeField]
    private float damperDefault = 5;
    [SerializeField]
    private float maxImpact = 20;

    public void Init(Transform RootTransform)
    {
        tree.Head.Init(this, springDefault, damperDefault, maxImpact, 0, 1, RootTransform.Find(tree.StringHead));
        tree.Chest.Init(this, springDefault, damperDefault, maxImpact, 0, 1, RootTransform.Find(tree.StringChest));
        tree.Stomach.Init(this, springDefault, damperDefault, maxImpact, 0, 0, RootTransform.Find(tree.StringStomach));

        tree.UpperArmLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, RootTransform.Find(tree.StringUpperArmLeft));
        tree.LowerArmLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, RootTransform.Find(tree.StringLowerArmLeft));
        tree.HandLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, RootTransform.Find(tree.StringHandLeft));

        tree.UpperArmRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, RootTransform.Find(tree.StringUpperArmRight));
        tree.LowerArmRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, RootTransform.Find(tree.StringLowerArmRight));
        tree.HandRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, RootTransform.Find(tree.StringHandRight));

        tree.UpperLegLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, RootTransform.Find(tree.StringUpperLegLeft));
        tree.LowerLegLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, RootTransform.Find(tree.StringLowerLegLeft));
        tree.FootLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, RootTransform.Find(tree.StringFootLeft));

        tree.UpperLegRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, RootTransform.Find(tree.StringUpperLegRight));
        tree.LowerLegRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, RootTransform.Find(tree.StringLowerLegRight));
        tree.FootRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, RootTransform.Find(tree.StringFootRight));

        for (int i = 0; i < tree.nodeList.Count; i++)
        {
            if (tree.nodeList[i].Parent != null)
            {
                CharacterNode root = tree.nodeList[i].Data;
                CharacterNode parent = tree.nodeList[i].Parent.Data;
                root.joint.connectedBody = parent.rigidbody;
                JointDrive d = new JointDrive();
                d.positionSpring = root.spring;
                d.positionDamper = root.damper;
                d.maximumForce = Mathf.Infinity;
                root.joint.slerpDrive = d;
                root.joint.xMotion = ConfigurableJointMotion.Locked;
                root.joint.yMotion = ConfigurableJointMotion.Locked;
                root.joint.zMotion = ConfigurableJointMotion.Locked;
                root.joint.rotationDriveMode = RotationDriveMode.Slerp;
            }
        }
    }

    // UTILITIES //

    /// Center of mass in world-space.
    public Vector3 GetCenterOfMass()
    {
        Vector3 CoM = Vector3.zero;
        float sumOfWeights = 0f;

        foreach (TreeNode<CharacterNode> node in tree.nodeList)
        {
            Rigidbody r = node.Data.rigidbody;
            CoM += r.worldCenterOfMass * r.mass;
            sumOfWeights += r.mass;
        }
        CoM /= sumOfWeights;
        return CoM;
    }
    /// Returns node in character this transform belongs to.
    public TreeNode<CharacterNode> ContainsTransform(Transform transform)
    {
        for (int i = 0; i < tree.nodeList.Count; i++)
        {
            if (tree.nodeList[i].Data.transform == transform)
                return tree.nodeList[i];
        }
        return null;
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