using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// Moves the main camera
public class _Camera : MonoBehaviour, IInitializeOnReload
{
    public event Action OnModeUpdate = delegate { };
    public enum CameraModeEnum { Follow, Orbit, Free, MAX }
    public CameraModeEnum cameraMode { get; private set; }

    [SerializeField]
    private float sensitivity = 0.3f;

    [Header("Follow Mode")]
    [SerializeField]
    private float followSpeed = 2;

    [Header("Orbit Mode")]
    [SerializeField]
    private float defaultDistance = 5;

    [Header("Free Mode")]
    [SerializeField]
    private float normalSpeed = 5.0f;
    [SerializeField]
    private float fastSpeed = 20.0f;

    private static _Input input;
    private K_NodeMap nodeMap;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Awake() { DontDestroyOnLoad(this); }
    private void Start()
    {
        input = FindObjectOfType<_Input>();
        input.OnCameraMode += UpdateCameraMode;
    }
    public void InitOnSceneLoad() { nodeMap = FindObjectOfType<K_NodeMap>(); }
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
            case CameraModeEnum.Free:
                if (input.controlCamera)
                {
                    FreeRotateCamera();
                    FreeMoveCamera();
                }
                break;

            case CameraModeEnum.Orbit:
                OrbitMoveCamera();
                if (input.controlCamera)
                    OrbitRotateCamera();
                break;

            case CameraModeEnum.Follow:
                FollowMoveCamera();
                FollowRotateCamera();
                break;
        }
    }

    private void FreeMoveCamera()
    {
        if (input.fastCamera)
            transform.Translate(input.inputDirection * fastSpeed * Time.deltaTime / Time.timeScale);
        else
            transform.Translate(input.inputDirection * normalSpeed * Time.deltaTime / Time.timeScale);

        Vector3 newPosition = transform.position;
    }
    private void FreeRotateCamera()
    {
        rotationX += input.mouseAxis.x * sensitivity * Time.deltaTime;
        rotationY += input.mouseAxis.y * sensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }
    private void OrbitMoveCamera()
    {
        transform.LookAt(nodeMap.CenterOfMass());
    }
    private void OrbitRotateCamera()
    {

    }
    private void FollowMoveCamera()
    {

    }
    private void FollowRotateCamera()
    {

    }
}
