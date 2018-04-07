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
    private Transform targetCharacterRoot;
    [SerializeField]
    private Material ghostMaterial;
    [SerializeField]
    private Color ghostColor;
    [SerializeField]
    private Vector3 throwDirection;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float throwRandomness;

    private Character character;
    private TargetCharacter targetCharacter = new TargetCharacter();
    private Player_NodeSelection selection;
    private CharacterKeyframe startKeyframe;

    private void Start()
    {
        targetCharacter.Init(targetCharacterRoot);
        character = FindObjectOfType<Player_Character>().PlayerCharacter;
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
        ForceMode += 1; if (ForceMode == ForceModeEnum.MAX) ForceMode = 0; OnModeUpdate();
    }

    private void ApplyStartKeyframe()
    {
        character.SetKeyframe(startKeyframe);
    }
    private void ThrowPlayer()
    {
        CharacterKeyframe thrownKeyframe = character.GetKeyframe();
        CharacterKeyframe.ModifyForce(thrownKeyframe, throwDirection, throwForce, throwRandomness);
        character.SetKeyframe(thrownKeyframe);
    }

    private void FixedUpdate()
    {
        if (_Input.InputCharacter)
        {
            if (selection.ChainSelected.Any() == true)
                ChainMovement();
            if (selection.ChainMirror.Any() == true)
                MirrorMovement();
            if (selection.ChainFollow.Any() == true)
                FollowMovement();
        }
        else
            TargetPlayerMovement();
    }

    private void TargetPlayerMovement()
    {
        for (int i = 0; i < targetCharacter.CharacterTree.NodeList.Count; i++)
        {
            targetCharacter.CharacterTree.NodeList[i].Data.Transform.gameObject.SetActive(false);
        }
    }

    private void ChainMovement()
    {
        for (int i = 0; i < character.CharacterTree.NodeList.Count; i++)
        {
            if (character.CharacterTree.NodeList[i] == selection.ChainSelected[0])
            {
                for (int j = i; j < selection.ChainSelected.Count + i; j++)
                {
                    targetCharacter.CharacterTree.NodeList[j].Data.Transform.gameObject.SetActive(true);
                    targetCharacter.CharacterTree.NodeList[j].Data.Transform.position =
                        character.CharacterTree.NodeList[j].Data.Transform.position +
                        character.CharacterTree.NodeList[j].Data.Transform.up.normalized;

                    targetCharacter.CharacterTree.NodeList[j].Data.Transform.rotation =
                        character.CharacterTree.NodeList[j].Data.Transform.rotation;
                }
                return;
            }
        }
    }
    private void MirrorMovement()
    {

    }
    private void FollowMovement()
    {

    }
}
