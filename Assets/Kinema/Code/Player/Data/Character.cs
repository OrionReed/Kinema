using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Kinema/New Character")]
[Serializable]
public class Character : ScriptableObject
{
    private Transform rootTransform;
    public CharacterPose StartPose;
    public TreeNode<CharacterNode> root { get; private set; }
    public List<TreeNode<CharacterNode>> nodeList { get; private set; }

    [SerializeField]
    private float springDefault = 30;
    [SerializeField]
    private float damperDefault = 5;
    [SerializeField]
    private float maxImpact = 20;

    private CharacterNode Head = new CharacterNode();
    private CharacterNode Chest = new CharacterNode();
    private CharacterNode Stomach = new CharacterNode();
    private CharacterNode UpperArmLeft = new CharacterNode();
    private CharacterNode LowerArmLeft = new CharacterNode();
    private CharacterNode HandLeft = new CharacterNode();
    private CharacterNode UpperArmRight = new CharacterNode();
    private CharacterNode LowerArmRight = new CharacterNode();
    private CharacterNode HandRight = new CharacterNode();
    private CharacterNode UpperLegLeft = new CharacterNode();
    private CharacterNode LowerLegLeft = new CharacterNode();
    private CharacterNode FootLeft = new CharacterNode();
    private CharacterNode UpperLegRight = new CharacterNode();
    private CharacterNode LowerLegRight = new CharacterNode();
    private CharacterNode FootRight = new CharacterNode();

    Character()
    {
        root = new TreeNode<CharacterNode>(Stomach);
        root.AddChild(UpperLegLeft).AddChild(LowerLegLeft).AddChild(FootLeft);
        root.AddChild(UpperLegRight).AddChild(LowerLegRight).AddChild(FootRight);
        var c = root.AddChild(Chest);
        c.AddChild(Head);
        c.AddChild(UpperArmLeft).AddChild(LowerArmLeft).AddChild(HandLeft);
        c.AddChild(UpperArmRight).AddChild(LowerArmRight).AddChild(HandRight);

        nodeList = new List<TreeNode<CharacterNode>>();
        nodeList.Add(root);
        nodeList.AddRange(CharacterUtils.GetChildren(root));
    }
    public void Init(Transform RootTransform)
    {
        rootTransform = RootTransform;
        Head.Init(this, springDefault, damperDefault, maxImpact, 0, 1, rootTransform.Find("Head"));
        Chest.Init(this, springDefault, damperDefault, maxImpact, 0, 1, rootTransform.Find("Chest"));
        Stomach.Init(this, springDefault, damperDefault, maxImpact, 0, 0, rootTransform.Find("Stomach"));

        UpperArmLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, rootTransform.Find("UpperArmLeft"));
        LowerArmLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, rootTransform.Find("LowerArmLeft"));
        HandLeft.Init(this, springDefault, damperDefault, maxImpact, -1, 1, rootTransform.Find("HandLeft"));

        UpperArmRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, rootTransform.Find("UpperArmRight"));
        LowerArmRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, rootTransform.Find("LowerArmRight"));
        HandRight.Init(this, springDefault, damperDefault, maxImpact, 1, 1, rootTransform.Find("HandRight"));

        UpperLegLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, rootTransform.Find("UpperLegLeft"));
        LowerLegLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, rootTransform.Find("LowerLegLeft"));
        FootLeft.Init(this, springDefault, damperDefault, maxImpact, -1, -1, rootTransform.Find("FootLeft"));

        UpperLegRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, rootTransform.Find("UpperLegRight"));
        LowerLegRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, rootTransform.Find("LowerLegRight"));
        FootRight.Init(this, springDefault, damperDefault, maxImpact, 1, -1, rootTransform.Find("FootRight"));

        InitJoints();
    }
    void InitJoints()
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].Parent != null)
            {
                CharacterNode root = nodeList[i].Data;
                CharacterNode parent = nodeList[i].Parent.Data;
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
}