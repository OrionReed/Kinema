using System.Collections.Generic;
using UnityEngine;
using System;

/// Stores initialized player node map and node information
public class K_NodeMap : MonoBehaviour
{
    public List<TreeNode<PlayerNode>> nodes { get; private set; }
    private TreeNode<PlayerNode> root;
    [SerializeField]
    private bool InitGravity;
    [SerializeField]
    private PlayerNode Head;
    [SerializeField]
    private PlayerNode Chest;
    [SerializeField]
    private PlayerNode Stomach;
    [SerializeField]
    private PlayerNode UpperArmLeft;
    [SerializeField]
    private PlayerNode LowerArmLeft;
    [SerializeField]
    private PlayerNode HandLeft;
    [SerializeField]
    private PlayerNode UpperArmRight;
    [SerializeField]
    private PlayerNode LowerArmRight;
    [SerializeField]
    private PlayerNode HandRight;
    [SerializeField]
    private PlayerNode UpperLegLeft;
    [SerializeField]
    private PlayerNode LowerLegLeft;
    [SerializeField]
    private PlayerNode FootLeft;
    [SerializeField]
    private PlayerNode UpperLegRight;
    [SerializeField]
    private PlayerNode LowerLegRight;
    [SerializeField]
    private PlayerNode FootRight;

    private void Awake()
    {
        root = new TreeNode<PlayerNode>(Stomach);
        root.AddChild(UpperLegLeft).AddChild(LowerLegLeft).AddChild(FootLeft);
        root.AddChild(UpperLegRight).AddChild(LowerLegRight).AddChild(FootRight);
        var c = root.AddChild(Chest);
        c.AddChild(Head);
        c.AddChild(UpperArmLeft).AddChild(LowerArmLeft).AddChild(HandLeft);
        c.AddChild(UpperArmRight).AddChild(LowerArmRight).AddChild(HandRight);

        nodes = new List<TreeNode<PlayerNode>>();
        nodes.Add(root);
        nodes.AddRange(GetChildren(root));

        Head.SetCoords(0, 1);
        Chest.SetCoords(0, 1);
        Stomach.SetCoords(0, 0);

        UpperArmLeft.SetCoords(-1, 1);
        LowerArmLeft.SetCoords(-1, 1);
        HandLeft.SetCoords(-1, 1);

        UpperArmRight.SetCoords(1, 1);
        LowerArmRight.SetCoords(1, 1);
        HandRight.SetCoords(1, 1);

        UpperLegLeft.SetCoords(-1, -1);
        LowerLegLeft.SetCoords(-1, -1);
        FootLeft.SetCoords(-1, -1);

        UpperLegRight.SetCoords(1, -1);
        LowerLegRight.SetCoords(1, -1);
        FootRight.SetCoords(1, -1);

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Data.originalRotation = nodes[i].Data.gameObj.transform.rotation;
            nodes[i].Data.transform = nodes[i].Data.gameObj.transform;
            nodes[i].Data.renderer = nodes[i].Data.gameObj.GetComponent<Renderer>();
            nodes[i].Data.rigidbody = nodes[i].Data.gameObj.GetComponent<Rigidbody>();
            nodes[i].Data.joint = nodes[i].Data.gameObj.GetComponent<ConfigurableJoint>();

            if (nodes[i].Parent != null)
            {
                PlayerNode root = nodes[i].Data;
                PlayerNode parent = nodes[i].Parent.Data;
                root.joint.connectedBody = parent.gameObj.GetComponent<Rigidbody>();
                JointDrive d = new JointDrive();
                d.positionSpring = root.spring;
                d.positionDamper = root.damper;
                d.maximumForce = Mathf.Infinity;
                root.driveMode = d;
                root.joint.slerpDrive = root.driveMode;
                root.joint.xMotion = ConfigurableJointMotion.Locked;
                root.joint.yMotion = ConfigurableJointMotion.Locked;
                root.joint.zMotion = ConfigurableJointMotion.Locked;
                root.joint.rotationDriveMode = RotationDriveMode.Slerp;
            }
            nodes[i].Data.rigidbody.useGravity = InitGravity;
        }
    }
    private void Start()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Data.rigidbody.velocity = nodes[i].Data.initialVelocity;
        }
    }

    #region Utilities
    /// Returns node this Transform belongs to, null if none is found.
    public TreeNode<PlayerNode> GetFromTransform(Transform transform)
    {
        if (transform == null)
            return null;
        if (transform.tag == "Player")
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Data.gameObj == transform.gameObject)
                    return nodes[i];
            }
        }
        return null;
    }
    /// Returns opposite node on X axis (left / right)
    public TreeNode<PlayerNode> GetOpposite(TreeNode<PlayerNode> node)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].Data.coordX == -node.Data.coordX)
                if (nodes[i].Data.coordY == node.Data.coordY)
                    if (nodes[i].Level == node.Level)
                        return nodes[i];
        }
        return node;
    }
    /// Returns an array with all subsequent children of this node, excluding this node.
    public List<TreeNode<PlayerNode>> GetChildren(TreeNode<PlayerNode> node)
    {
        var nodes = new List<TreeNode<PlayerNode>>();
        foreach (var childNode in node.Children)
        {
            nodes.Add(childNode);
            nodes.AddRange(GetChildren(childNode));
        }
        return nodes;
    }
    /// Returns center of mass in world-space.
    public Vector3 CenterOfMass()
    {
        Vector3 CoM = Vector3.zero;
        float sumOfWeights = 0f;

        foreach (TreeNode<PlayerNode> node in nodes)
        {
            Rigidbody r = node.Data.rigidbody;
            CoM += r.worldCenterOfMass * r.mass;
            sumOfWeights += r.mass;
        }
        CoM /= sumOfWeights;
        return CoM;
    }
    #endregion
}