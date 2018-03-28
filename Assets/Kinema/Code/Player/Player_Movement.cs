using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player_Movement : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum ForceModeEnum { Position, Speed, Contact, MAX };
    public ForceModeEnum forceMode;

    [SerializeField]
    private Transform TargetCharacterRoot;
    [SerializeField]
    private Material GhostMaterial;
    [SerializeField]
    private Vector3 ThrowDirection;
    [SerializeField]
    private float ThrowForce;
    [SerializeField]
    private float ThrowRandomness;
    private Character character;
    private TargetCharacter targetCharacter = new TargetCharacter();
    private Player_NodeSelection selection;
    private Keyframe startKeyframe;

    private Quaternion rootTargetRotation = Quaternion.identity;

    private void Start()
    {
        targetCharacter.Init(TargetCharacterRoot);
        character = FindObjectOfType<Character_Installer>().currentCharacter;
        selection = GetComponent<Player_NodeSelection>();
        startKeyframe = character.GetKeyframe();
        _LevelState.OnPlay += ApplyStartKeyframe;
        _Input.OnKeyForceMode += UpdateForceMode;
        _Input.OnKeyThrow += ThrowPlayer;
    }
    private void OnDisable()
    {
        _Input.OnKeyForceMode -= UpdateForceMode;
        _LevelState.OnPlay -= ApplyStartKeyframe;
        _Input.OnKeyThrow -= ThrowPlayer;
    }

    private void UpdateForceMode()
    {
        forceMode += 1; if (forceMode == ForceModeEnum.MAX) forceMode = 0; OnModeUpdate();
    }

    private void ApplyStartKeyframe()
    {
        character.SetKeyframe(startKeyframe);
    }
    private void ThrowPlayer()
    {
        Keyframe thrownKeyframe = character.GetKeyframe();
        Keyframe.ModifyForce(thrownKeyframe, ThrowDirection, ThrowForce, ThrowRandomness);
        character.SetKeyframe(thrownKeyframe);
    }

    private void FixedUpdate()
    {
        if (_Input.axisInput)
        {
            if (selection.chain.Any() == true)
                ChainMovement(selection.chain);
            if (selection.chainMirror.Any() == true)
                MirrorMovement(selection.chainMirror);
            if (selection.chainFollow.Any() == true)
                FollowMovement(selection.chainFollow);
        }
    }

    private void ChainMovement(List<TreeNode<CharacterNode>> chain)
    {
        for (int i = 0; i < chain.Count; i++)
        {

        }
    }
    private void MirrorMovement(List<TreeNode<CharacterNode>> chain)
    {

    }
    private void FollowMovement(List<TreeNode<CharacterNode>> chain)
    {

    }
}
