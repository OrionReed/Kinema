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
    private Keyframe startKeyframe;

    private void Start()
    {
        targetCharacter.Init(targetCharacterRoot);
        character = FindObjectOfType<Character_Installer>().CurrentCharacter;
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
        Keyframe thrownKeyframe = character.GetKeyframe();
        Keyframe.ModifyForce(thrownKeyframe, throwDirection, throwForce, throwRandomness);
        character.SetKeyframe(thrownKeyframe);
    }

    private void FixedUpdate()
    {
        if (_Input.InputCharacter)
        {
            if (selection.Chain.Any() == true)
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
        for (int i = 0; i < targetCharacter.List.Count; i++)
        {
            targetCharacter.List[i].Data.Transform.gameObject.SetActive(false);
        }
    }

    private void ChainMovement()
    {
        for (int i = 0; i < character.List.Count; i++)
        {
            if (character.List[i] == selection.Chain[0])
            {
                for (int j = i; j < selection.Chain.Count + i; j++)
                {
                    targetCharacter.List[j].Data.Transform.gameObject.SetActive(true);
                    targetCharacter.List[j].Data.Transform.position =
                        character.List[j].Data.Transform.position +
                        character.List[j].Data.Transform.up.normalized;

                    targetCharacter.List[j].Data.Transform.rotation =
                        character.List[j].Data.Transform.rotation;
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
