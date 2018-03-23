using System.Collections.Generic;
using UnityEngine;
using System;

public class Node_Movement : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum ForceModeEnum { Position, Speed, Contact, MAX };
    public ForceModeEnum forceMode;

    [SerializeField]
    private Material GhostMaterial;
    [SerializeField]
    private Vector3 ThrowDirection;
    [SerializeField]
    private float ThrowForce;
    [SerializeField]
    private float ThrowRandomness;
    private Node_Selection selection;
    private Quaternion rootTargetRotation = Quaternion.identity;
    private GameObject ghostLimb;
    private CharacterPose startPose;

    private void Awake()
    {
        ghostLimb = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ghostLimb.name = "Ghost_Limb";
        ghostLimb.transform.parent = this.transform;
        ghostLimb.GetComponent<Collider>().enabled = false;
        ghostLimb.transform.localScale = transform.localScale;
        ghostLimb.GetComponent<Renderer>().material = GhostMaterial;
    }

    private void Start()
    {
        selection = GetComponent<Node_Selection>();
        _Input.OnForceMode += UpdateForceMode;
        _RestartScene.EventRestartScene += ResetPose;
        startPose = GetPlayerPose(CharacterSelection.currentCharacter);
    }
    private void OnDisable()
    {
        _Input.OnForceMode -= UpdateForceMode;
        _RestartScene.EventRestartScene -= ResetPose;
    }

    private void UpdateForceMode()
    {
        forceMode += 1;
        if (forceMode == ForceModeEnum.MAX)
            forceMode = 0;
        OnModeUpdate();
    }

    private void ResetPose()
    {
        CharacterPose thrownPose = startPose;
        ApplyForceToPose(thrownPose, ThrowDirection, ThrowForce, ThrowRandomness);
        ApplyPose(thrownPose);
    }
    private void FixedUpdate()
    {
        TreeNode<CharacterNode> rootChain = selection.root;
        List<TreeNode<CharacterNode>> childrenChain = selection.childrenChain;
        if (rootChain != null)
            ChainMovement(rootChain, childrenChain);

        TreeNode<CharacterNode> rootMirror = selection.rootMirror;
        List<TreeNode<CharacterNode>> childrenMirror = selection.childrenMirror;
        if (rootMirror != null)
            MirrorMovement(rootMirror, childrenMirror);

        TreeNode<CharacterNode> rootFollow = selection.rootFollow;
        List<TreeNode<CharacterNode>> childrenFollow = selection.childrenFollow;
        if (rootFollow != null)
            FollowMovement(rootFollow, childrenFollow);
        if (rootChain != null)
            UpdateGhost(rootChain);
    }

    private void ChainMovement(TreeNode<CharacterNode> root, List<TreeNode<CharacterNode>> chain)
    {
        rootTargetRotation = Quaternion.Inverse(root.Parent.Data.transform.rotation) * (root.Data.transform.rotation * _Input.inputRotation);
        QuaternionUtils.SetTargetRotationLocal(root.Data.joint, rootTargetRotation, root.Data.originalRotation);

        if (chain != null)
            for (int i = 0; i < chain.Count; i++)
            {

            }
    }
    private void MirrorMovement(TreeNode<CharacterNode> root, List<TreeNode<CharacterNode>> chain)
    {

    }
    private void FollowMovement(TreeNode<CharacterNode> root, List<TreeNode<CharacterNode>> chain)
    {

    }
    private void UpdateGhost(TreeNode<CharacterNode> root)
    {
        ghostLimb.transform.localScale = root.Data.transform.localScale;
        ghostLimb.transform.rotation = rootTargetRotation;
        ghostLimb.transform.position = root.Data.transform.TransformPoint(root.Data.joint.anchor);
        ghostLimb.transform.Translate(Vector3.up * Vector3.Distance(
                                        root.Data.transform.TransformPoint(root.Data.joint.anchor),
                                        root.Data.transform.position
                                        ),
                                        Space.Self);
    }
    private void ApplyPose(CharacterPose Pose)
    {
        foreach (NodePose pose in Pose.poses)
        {
            pose.node.Data.transform.position = pose.position;
            pose.node.Data.transform.rotation = pose.rotation;
            pose.node.Data.rigidbody.velocity = pose.velocity;
        }
    }
    private CharacterPose GetPlayerPose(Character character)
    {
        CharacterPose currentPose = new CharacterPose();
        foreach (TreeNode<CharacterNode> node in character.nodeList)
        {
            NodePose nodePose = new NodePose(
                node,
                node.Data.rigidbody.velocity,
                node.Data.transform.position,
                node.Data.transform.rotation);

            currentPose.poses.Add(nodePose);
        }
        return currentPose;
    }
    private void ApplyForceToPose(CharacterPose pose, Vector3 Direction, float Force, float Randomness)
    {
        foreach (NodePose nodePose in pose.poses)
            nodePose.velocity = Direction.normalized * UnityEngine.Random.RandomRange(Force - Randomness, Force + Randomness);
    }

}
