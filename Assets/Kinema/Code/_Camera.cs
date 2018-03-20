using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// Moves the main camera
public class _Camera : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum CameraModeEnum { Follow, Orbit, Free, MAX }
    public CameraModeEnum cameraMode { get; private set; }

    [SerializeField]
    private float normalSpeed = 5.0f;
    [SerializeField]
    private float shiftSpeed = 20.0f;
    [SerializeField]
    private float sensitivity = 0.3f;
    [SerializeField]
    private bool resetPosition = false;
    [SerializeField]
    private float letterboxAspect = 21.0f / 9.0f;

    private static _Input input;
    private Camera cam;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        input = FindObjectOfType<_Input>();
        input.OnCameraMode += UpdateCameraMode;
        input.OnLetterbox += ToggleLetterbox;
    }

    public void Init()
    {
    }
    private void OnDisable()
    {
        input.OnCameraMode -= UpdateCameraMode;
        input.OnLetterbox -= ToggleLetterbox;
    }

    private void UpdateCameraMode()
    {
        cameraMode += 1;
        if (cameraMode == CameraModeEnum.MAX)
            cameraMode = 0;
        OnModeUpdate();
        Debug.Log("Updating Camera Mode...");
    }

    private void Update()
    {
        if (input.controlCamera)
        {
            RotateCamera();
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        if (input.fastCamera)
            transform.Translate(input.inputDirection * shiftSpeed * Time.deltaTime / Time.timeScale);
        else
            transform.Translate(input.inputDirection * normalSpeed * Time.deltaTime / Time.timeScale);

        Vector3 newPosition = transform.position;
    }
    private void RotateCamera()
    {
        rotationX += input.mouseAxis.x * sensitivity * Time.deltaTime;
        rotationY += input.mouseAxis.y * sensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }

    private void ToggleLetterbox()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float aspectHeight = screenAspect / letterboxAspect;
        Rect rect = cam.rect;
        bool isLetterboxed = (cam.rect.height == aspectHeight);
        rect.width = 1.0f;
        rect.x = 0;
        rect.height = isLetterboxed ? (float)Screen.height : aspectHeight;
        rect.y = (1 - (isLetterboxed ? (float)Screen.height : aspectHeight)) / 2;
        cam.rect = rect;
    }
}
