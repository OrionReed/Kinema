using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// Moves the main camera
public class _Camera : MonoBehaviour, IInitializeOnReload
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
    private float fixedRotateSpeed = 8f;

    private _Input input;
    private Node_Map nodeMap;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Awake() { DontDestroyOnLoad(this); }
    private void Start()
    {
        input = FindObjectOfType<_Input>();
        input.OnCameraMode += UpdateCameraMode;
    }
    public void InitOnSceneLoad() { nodeMap = FindObjectOfType<Node_Map>(); }
    private void OnDisable() { input.OnCameraMode -= UpdateCameraMode; }

    private void UpdateCameraMode()
    {
        cameraMode += 1;
        if (cameraMode == CameraModeEnum.MAX)
            cameraMode = 0;
        OnModeUpdate();
    }

    private void Update()
    {
        switch (cameraMode)
        {
            case CameraModeEnum.Follow:
                if (input.controlCamera)
                    FollowControlCamera();
                else
                    FollowAutoCamera();
                break;
            case CameraModeEnum.Fixed:
                FixedAutoCamera();
                break;
            case CameraModeEnum.Free:
                if (input.controlCamera)
                    FreeControlCamera();
                break;
        }
    }

    private void FollowAutoCamera()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(nodeMap.GetCenterOfMass() - transform.position),
            (Time.deltaTime / Time.timeScale) * followRotateSpeed);

        transform.position = Vector3.Slerp(
            transform.position,
            VectorTools.PointInDirection(nodeMap.GetCenterOfMass(), transform.position, followDistance),
            (Time.deltaTime / Time.timeScale) * followMoveSpeed);
    }
    private void FollowControlCamera()
    {
        // Rotate camera around player with mouse

        transform.rotation = Quaternion.LookRotation(nodeMap.GetCenterOfMass() - transform.position);

        transform.position = Vector3.Slerp(
            transform.position,
            VectorTools.PointInDirection(nodeMap.GetCenterOfMass(), transform.position, followDistance),
            (Time.deltaTime / Time.timeScale) * followMoveSpeed);
    }
    private void FixedAutoCamera()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(nodeMap.GetCenterOfMass() - transform.position),
            (Time.deltaTime / Time.timeScale) * followRotateSpeed);
    }
    private void FreeControlCamera()
    {
        if (input.fastCamera)
            transform.Translate(input.inputDirection * freeFastSpeed * Time.deltaTime / Time.timeScale);
        else
            transform.Translate(input.inputDirection * freeNormalSpeed * Time.deltaTime / Time.timeScale);
        Vector3 newPosition = transform.position;
        rotationX += input.mouseAxis.x * mouseSensitivity * Time.deltaTime;
        rotationY += input.mouseAxis.y * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }
}
