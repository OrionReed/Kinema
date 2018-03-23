using UnityEngine;
using System;

/// Moves the main camera
[RequireComponent(typeof(Camera))]
public class _Camera : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum CameraModeEnum { Follow, Fixed, Free, MAX }
    public CameraModeEnum cameraMode { get; private set; }

    [SerializeField]
    private float mouseSensitivity = 180f;
    [SerializeField]
    private float freeNormalSpeed = 5.0f;
    [SerializeField]
    private float freeFastSpeed = 20.0f;
    [SerializeField]
    private float followRotateSpeed = 4f;
    [SerializeField]
    private float followMoveSpeed = 4f;
    [SerializeField]
    private float followDistance = 8f;
    [SerializeField]
    private float fixedRotateSpeed = 5f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Awake() { DontDestroyOnLoad(this); }
    private void Start()
    {
        _Input.OnCameraMode += UpdateCameraMode;
    }
    private void OnDisable() { _Input.OnCameraMode -= UpdateCameraMode; }

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
                if (_Input.controlCamera)
                    FollowControlCamera(target);
                else
                    FollowAutoCamera(target);
                break;
            case CameraModeEnum.Fixed:
                FixedAutoCamera(target);
                break;
            case CameraModeEnum.Free:
                if (_Input.controlCamera)
                    FreeControlCamera();
                break;
        }
    }

    private void FollowAutoCamera(Vector3 target)
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(target - transform.position),
            (Time.deltaTime / Time.timeScale) * followRotateSpeed);

        transform.position = Vector3.Slerp(
            transform.position,
            VectorTools.PointInDirection(target, transform.position, followDistance),
            (Time.deltaTime / Time.timeScale) * followMoveSpeed);
        if (transform.position.y <= target.y)
        {
            transform.position = Vector3.Slerp(
                transform.position,
                new Vector3(transform.position.x, target.y, transform.position.z),
                (Time.deltaTime / Time.timeScale) * followMoveSpeed);
        }
    }
    private void FollowControlCamera(Vector3 target)
    {
        // Rotate camera around player with mouse

        transform.rotation = Quaternion.LookRotation(target - transform.position);

        transform.position = Vector3.Slerp(
            transform.position,
            VectorTools.PointInDirection(target, transform.position, followDistance),
            (Time.deltaTime / Time.timeScale) * followMoveSpeed);
    }
    private void FixedAutoCamera(Vector3 target)
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(target - transform.position),
            (Time.deltaTime / Time.timeScale) * fixedRotateSpeed);
    }
    private void FreeControlCamera()
    {
        if (_Input.fastCamera)
            transform.Translate(_Input.inputDirection * freeFastSpeed * Time.deltaTime / Time.timeScale);
        else
            transform.Translate(_Input.inputDirection * freeNormalSpeed * Time.deltaTime / Time.timeScale);
        Vector3 newPosition = transform.position;
        rotationX += _Input.mouseAxis.x * mouseSensitivity * Time.deltaTime;
        rotationY += _Input.mouseAxis.y * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }
}
