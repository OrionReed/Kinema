using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Kinema/New Character")]
[Serializable]
public class Character : ScriptableObject
{
    public _CharacterTree<CharacterNode> tree { get; private set; }

    [SerializeField]
    private float springDefault = 30;
    [SerializeField]
    private float damperDefault = 5;
    [SerializeField]
    private float maxImpact = 20;

    Character()
    {
        tree = new _CharacterTree<CharacterNode>();
    }

    public void Init(Transform RootTransform)
    {
        Debug.Log("Tree?" + (tree != null));
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
    public void ResetNodeData()
    {
        for (int i = 0; i < tree.nodeList.Count; i++)
        {
            tree.nodeList[i].Data.SetDamage(0);
            tree.nodeList[i].Data.SetSelectionState(NodeSelectionState.None);
        }
    }
}