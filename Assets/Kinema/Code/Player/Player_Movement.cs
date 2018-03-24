using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Movement : MonoBehaviour
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
    private Player_NodeSelection selection;
    private Quaternion rootTargetRotation = Quaternion.identity;
    private GameObject ghostLimb;
    private Pose startPose;

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
        selection = GetComponent<Player_NodeSelection>();
        startPose = GetPlayerPose(CharacterSelection.currentCharacter);
        _LevelState.OnPlay += ResetPose;
        _Input.OnKeyForceMode += UpdateForceMode;
    }
    private void OnDisable()
    {
        _Input.OnKeyForceMode -= UpdateForceMode;
        _LevelState.OnPlay -= ResetPose;
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
        Pose modifiedStartPose = startPose;
        ApplyForceToPose(modifiedStartPose, ThrowDirection, ThrowForce, ThrowRandomness);
        ApplyPose(modifiedStartPose, CharacterSelection.currentCharacter);
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

    private void ApplyPose(Pose Pose, Character character)
    {
        for (int i = 0; i < Pose.tree.nodeList.Count; i++)
        {
            character.tree.nodeList[i].Data.transform.position = Pose.tree.nodeList[i].Data.position;
            character.tree.nodeList[i].Data.transform.rotation = Pose.tree.nodeList[i].Data.rotation;
            character.tree.nodeList[i].Data.rigidbody.velocity = Pose.tree.nodeList[i].Data.velocity;
        }
    }
    private Pose GetPlayerPose(Character character)
    {
        Pose currentPose = new Pose();
        for (int i = 0; i < character.tree.nodeList.Count; i++)
        {
            currentPose.tree.nodeList[i].Data.position = character.tree.nodeList[i].Data.transform.position;
            currentPose.tree.nodeList[i].Data.rotation = character.tree.nodeList[i].Data.transform.rotation;
            currentPose.tree.nodeList[i].Data.velocity = character.tree.nodeList[i].Data.rigidbody.velocity;
        }
        return currentPose;
    }
    private void ApplyForceToPose(Pose pose, Vector3 Direction, float Force, float Randomness)
    {
        foreach (TreeNode<PoseNode> node in pose.tree.nodeList)
            node.Data.velocity = Direction.normalized * UnityEngine.Random.Range(Force - Randomness, Force + Randomness);
    }

}
