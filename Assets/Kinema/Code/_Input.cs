using System;
using UnityEngine;

public class _Input : MonoBehaviour
{
    /// Change selection mode
    public static event Action OnKeySelectionMode = delegate { };
    /// Change force mode
    public static event Action OnKeyForceMode = delegate { };
    /// Change camera mode
    public static event Action OnKeyCameraMode = delegate { };
    /// Reset scene to beginning
    public static event Action OnKeyResetScene = delegate { };
    /// Toggle letterbox
    public static event Action OnKeyLetterbox = delegate { };
    /// Speed up time
    public static event Action OnKeyTimeSpeedUp = delegate { };
    /// Slow down time
    public static event Action OnKeyTimeSpeedDown = delegate { };
    /// Select something with click
    public static event Action OnClickSelect = delegate { };
    /// Throw character
    public static event Action OnKeyThrow = delegate { };

    /// Inputting character movement?
    public static bool InputCharacter { get; private set; }
    /// Change selection mode
    public static Quaternion InputCharacterRotation { get; private set; }
    /// Controlling the main camera?
    public static bool ControlCamera { get; private set; }
    /// Moving camera fast?
    public static bool FastCamera { get; private set; }
    /// Direction of mouse orbit rotation
    public static Vector2 CameraOrbitDirection { get; private set; }
    /// Camera input movement vector
    public static Vector3 InputCameraDirection { get; private set; }
    /// Direction of mouse movement
    public static Vector2 MouseAxis { get; private set; }

    [SerializeField]
    private KeyCode selectionMode;
    [SerializeField]
    private KeyCode forceMode;
    [SerializeField]
    private KeyCode cameraMode;
    [SerializeField]
    private KeyCode resetScene;
    [SerializeField]
    private KeyCode letterbox;
    [SerializeField]
    private KeyCode fastCamera;
    [SerializeField]
    private KeyCode timeSpeedUp;
    [SerializeField]
    private KeyCode timeSpeedDown;
    [SerializeField]
    private KeyCode throwPlayer;

    private void Awake()
    {
        InputCameraDirection = Vector3.zero;
        InputCharacterRotation = Quaternion.identity;
        ControlCamera = false;
        InputCharacter = false;
        FastCamera = false;
    }

    private void Update()
    {
        if (_LevelState.CurrentState != _LevelState.States.Dead)
        {
            if (Input.GetKeyDown(selectionMode)) OnKeySelectionMode();
            if (Input.GetKeyDown(forceMode)) OnKeyForceMode();
            if (Input.GetKeyDown(letterbox)) OnKeyLetterbox();
            if (Input.GetKeyDown(throwPlayer)) OnKeyThrow();
            if (InputCharacterRotation != Quaternion.identity) InputCharacter = true; else InputCharacter = false;
        }
        InputCameraDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        CameraOrbitDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(cameraMode)) OnKeyCameraMode();
        if (Input.GetKeyDown(resetScene)) OnKeyResetScene();
        if (Input.GetKeyDown(timeSpeedUp)) OnKeyTimeSpeedUp();
        if (Input.GetKeyDown(timeSpeedDown)) OnKeyTimeSpeedDown();
        if (Input.GetMouseButtonDown(0)) OnClickSelect();

        if (Input.GetMouseButton(1)) ControlCamera = true; else ControlCamera = false;
        if (Input.GetKey(fastCamera)) FastCamera = true; else FastCamera = false;

        MouseAxis = new Vector2(Input.GetAxisRaw("Mouse X") / Time.timeScale, Input.GetAxisRaw("Mouse Y") / Time.timeScale);
    }

    void FixedUpdate()
    {
        InputCharacterRotation =
        Quaternion.AngleAxis(Input.GetAxisRaw("Yaw") * 45, Vector3.right) *
        Quaternion.AngleAxis(-Input.GetAxisRaw("Pitch") * 45, Vector3.forward) *
        Quaternion.AngleAxis(Input.GetAxisRaw("Roll") * 45, Vector3.up);
    }
}
