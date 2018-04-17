using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player_Movement : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum ForceModeEnum { Position, Speed, Contact, MAX };
    public ForceModeEnum ForceMode { get; private set; }

    [SerializeField]
    private Vector3 throwDirection;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float throwRandomness;
    [SerializeField]
    private float jointSpring;
    [SerializeField]
    private float activeDamper;
    [SerializeField]
    private float inactiveDamper;

    private Character character;
    private Player_NodeSelection selection;
    private CharacterKeyframe startKeyframe;

    public void Throw(float force)
    {
        throwForce = force;
        ThrowPlayer();
    }
    public void Throw(float force, Vector3 direction)
    {
        throwDirection = direction;
        throwForce = force;
        ThrowPlayer();
    }

    private void Start()
    {
        character = FindObjectOfType<Player_Character>().PlayerCharacter;
        selection = GetComponent<Player_NodeSelection>();
        startKeyframe = character.GetKeyframe();
        _LevelState.OnPlay += SetTargetToCurrent;
        _LevelState.OnPlay += ApplyStartKeyframe;
        _Input.OnKeyForceMode += UpdateForceMode;
        _Input.OnKeyThrow += ThrowPlayer;
        selection.OnNodeSelection += SetNodeDrive;
    }
    private void OnDisable()
    {
        _LevelState.OnPlay -= SetTargetToCurrent;
        _Input.OnKeyForceMode -= UpdateForceMode;
        _LevelState.OnPlay -= ApplyStartKeyframe;
        _Input.OnKeyThrow -= ThrowPlayer;
        selection.OnNodeSelection -= SetNodeDrive;
    }

    private void UpdateForceMode()
    {
        ForceMode += 1; if (ForceMode == ForceModeEnum.MAX) ForceMode = 0; OnModeUpdate();
    }

    private void ApplyStartKeyframe()
    {
        character.SetKeyframe(startKeyframe);
    }
    private void ThrowPlayer()
    {
        CharacterKeyframe thrownKeyframe = character.GetKeyframe();
        CharacterKeyframe.AddForce(thrownKeyframe, throwDirection, throwForce, throwRandomness);
        character.SetKeyframe(thrownKeyframe);
    }

    private void FixedUpdate()
    {
        if (selection.ChainSelected.Any() == true)
            ChainMovement();
        if (selection.ChainMirror.Any() == true)
            MirrorMovement();
        if (selection.ChainFollow.Any() == true)
            FollowMovement();
    }

    private void ChainMovement()
    {
        JointDrive chainDrive = new JointDrive();
        chainDrive.maximumForce = Mathf.Infinity;
        List<TreeNode<CharacterNode>> chain = selection.ChainSelected;
        for (int i = 0; i < selection.ChainSelected.Count; i++)
        {
            if (_Input.InputCharacter)
            {
                chainDrive.positionDamper = activeDamper;
                chainDrive.positionSpring = jointSpring;
            }
            else
            {
                chainDrive.positionDamper = inactiveDamper;
                chainDrive.positionSpring = 0;
            }

            chain[i].Data.Joint.slerpDrive = chainDrive;
            chain[i].Data.Joint.SetTargetRotationLocal(
                chain[i].Data.Transform.rotation *
                _Input.InputCharacterRotation,
                chain[i].Data.OriginalRotation);
        }
    }
    private void MirrorMovement()
    {
        List<TreeNode<CharacterNode>> chainSelected = selection.ChainMirror;
        List<TreeNode<CharacterNode>> chainMirror = selection.ChainMirror;
        for (int i = 0; i < selection.ChainMirror.Count; i++)
        {
            chainMirror[i].Data.Joint.SetTargetRotationLocal(
                chainSelected[i].Data.Joint.targetRotation,
                chainMirror[i].Data.OriginalRotation);
        }

    }
    private void FollowMovement()
    {

    }

    private void SetNodeDrive()
    {
        JointDrive drive = new JointDrive();
        drive.maximumForce = Mathf.Infinity;
        drive.positionDamper = inactiveDamper;

        foreach (TreeNode<CharacterNode> node in selection.AllDeselected)
            if (node.IsRoot == false) node.Data.Joint.slerpDrive = drive;

        drive.positionDamper = activeDamper;
        drive.positionSpring = jointSpring;

        foreach (TreeNode<CharacterNode> node in selection.ChainSelected)
            if (node.IsRoot == false) node.Data.Joint.slerpDrive = drive;
        foreach (TreeNode<CharacterNode> node in selection.ChainMirror)
            if (node.IsRoot == false) node.Data.Joint.slerpDrive = drive;
        foreach (TreeNode<CharacterNode> node in selection.ChainFollow)
            if (node.IsRoot == false) node.Data.Joint.slerpDrive = drive;
    }
    private void SetTargetToCurrent()
    {
        foreach (TreeNode<CharacterNode> node in character.CharacterTree.NodeList)
            if (node.IsRoot == false) node.Data.Joint.SetTargetRotationLocal(node.Data.Transform.rotation, node.Data.OriginalRotation);
    }
}
