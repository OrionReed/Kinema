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
    private Pose startPose;

    private void Start()
    {
        selection = GetComponent<Player_NodeSelection>();
        startPose = Pose.GetCharacterPose(Player_Installer.currentCharacter);
        _LevelState.OnPlay += ResetPose;
        _Input.OnKeyForceMode += UpdateForceMode;
        _Input.OnKeyThrow += ThrowPlayer;
    }
    private void OnDisable()
    {
        _Input.OnKeyForceMode -= UpdateForceMode;
        _LevelState.OnPlay -= ResetPose;
        _Input.OnKeyThrow -= ThrowPlayer;
    }

    private void UpdateForceMode()
    {
        forceMode += 1; if (forceMode == ForceModeEnum.MAX) forceMode = 0; OnModeUpdate();
    }

    private void ResetPose()
    {
        Pose.ApplyPose(startPose, Player_Installer.currentCharacter);
    }
    private void ThrowPlayer()
    {
        Pose thrownPose = Pose.GetCharacterPose(Player_Installer.currentCharacter);
        Pose.ModifyPoseForce(thrownPose, ThrowDirection, ThrowForce, ThrowRandomness);
        Pose.ApplyPose(thrownPose, Player_Installer.currentCharacter);
    }

    private void FixedUpdate()
    {
        if (selection.root != null)
            ChainMovement(selection.root, selection.childrenChain);

        if (selection.rootMirror != null)
            MirrorMovement(selection.rootMirror, selection.childrenMirror);

        if (selection.rootFollow != null)
            FollowMovement(selection.rootFollow, selection.childrenFollow);
    }

    private void ChainMovement(TreeNode<CharacterNode> root, List<TreeNode<CharacterNode>> chain)
    {
        rootTargetRotation = Quaternion.Inverse(root.Parent.Data.transform.rotation) * (root.Data.transform.rotation * _Input.inputRotation);
        Utilities_Joint.SetTargetRotationLocal(root.Data.joint, rootTargetRotation, root.Data.originalRotation);
    }
    private void MirrorMovement(TreeNode<CharacterNode> root, List<TreeNode<CharacterNode>> chain)
    {

    }
    private void FollowMovement(TreeNode<CharacterNode> root, List<TreeNode<CharacterNode>> chain)
    {

    }
}
