using UnityEngine;
using System;

/// Moves the main camera
[RequireComponent(typeof(Camera))]
public class _Camera : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum CameraModeEnum { Follow, Fixed, Free, MAX }
    public CameraModeEnum CameraMode { get; private set; }

    [SerializeField]
    private CameraMode_AutoFollow modeFollow;
    [SerializeField]
    private CameraMode_Fixed modeFixed;
    [SerializeField]
    private CameraMode_Free modeFree;
    private Character character;

    private void Start()
    {
        _Input.OnKeyCameraMode += UpdateCameraMode;
        character = FindObjectOfType<Player_Character>().PlayerCharacter;
    }
    private void OnDisable() { _Input.OnKeyCameraMode -= UpdateCameraMode; }

    private void UpdateCameraMode()
    {
        CameraMode += 1; if (CameraMode == CameraModeEnum.MAX) CameraMode = 0; OnModeUpdate();
    }

    private void FixedUpdate()
    {
        Vector3 target = character.GetCenterOfMass();
        switch (CameraMode)
        {
            case CameraModeEnum.Follow:
                modeFollow.ControlCamera(this, target);
                break;
        }
    }

    private void Update()
    {
        Vector3 target = character.GetCenterOfMass();
        switch (CameraMode)
        {
            case CameraModeEnum.Fixed:
                modeFixed.ControlCamera(this, target);
                break;
            case CameraModeEnum.Free:
                modeFree.ControlCamera(this, target);
                break;
        }
    }
}
