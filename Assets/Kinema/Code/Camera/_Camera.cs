using UnityEngine;
using System;

/// Moves the main camera
[RequireComponent(typeof(Camera))]
public class _Camera : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum CameraModeEnum { Follow, Fixed, Free, MAX }
    public CameraModeEnum cameraMode { get; private set; }
    public float mouseSensitivity { get; private set; } = 180f;

    private CameraMode_AutoFollow modeFollow;
    private CameraMode_Fixed modeFixed;
    private CameraMode_Free modeFree;

    private void Awake()
    {
        modeFollow = new CameraMode_AutoFollow();
        modeFixed = new CameraMode_Fixed();
        modeFree = new CameraMode_Free();
    }
    private void Start() { _Input.OnKeyCameraMode += UpdateCameraMode; }
    private void OnDisable() { _Input.OnKeyCameraMode -= UpdateCameraMode; }

    private void UpdateCameraMode()
    {
        cameraMode += 1;
        if (cameraMode == CameraModeEnum.MAX)
            cameraMode = 0;
        OnModeUpdate();
    }

    private void Update()
    {
        Vector3 target = CharacterUtils.GetCenterOfMass(CharacterSelection.currentCharacter);
        switch (cameraMode)
        {
            case CameraModeEnum.Follow:
                modeFollow.ControlCamera(this, target, _Input.controlCamera);
                break;
            case CameraModeEnum.Fixed:
                modeFixed.ControlCamera(this, target, _Input.controlCamera);
                break;
            case CameraModeEnum.Free:
                modeFree.ControlCamera(this, target, _Input.controlCamera);
                break;
        }
    }
}
