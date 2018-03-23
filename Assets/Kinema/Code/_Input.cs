using System;
using UnityEngine;

public class _Input : MonoBehaviour
{
    public static event Action OnKeySelectionMode = delegate { };
    public static event Action OnKeyForceMode = delegate { };
    public static event Action OnKeyCameraMode = delegate { };
    public static event Action OnKeyToggleGravity = delegate { };
    public static event Action OnKeyRestartScene = delegate { };
    public static event Action OnKeyLetterbox = delegate { };
    public static event Action OnKeyTimeSpeedUp = delegate { };
    public static event Action OnKeyTimeSpeedDown = delegate { };
    public static event Action OnKeyClickLeft = delegate { };

    public static bool controlCamera { get; private set; }
    public static bool axisInput { get; private set; }
    public static bool fastCamera { get; private set; }
    public static Vector2 mouseAxis { get; private set; }
    public static Vector2 cameraOrbitDirection { get; private set; }
    public static Vector3 inputDirection { get; private set; }
    public static Quaternion inputRotation { get; private set; }

    [SerializeField]
    private KeyCode SelectionMode;
    [SerializeField]
    private KeyCode ForceMode;
    [SerializeField]
    private KeyCode CameraMode;
    [SerializeField]
    private KeyCode ToggleGravity;
    [SerializeField]
    private KeyCode RestartScene;
    [SerializeField]
    private KeyCode Letterbox;
    [SerializeField]
    private KeyCode FastCamera;
    [SerializeField]
    private KeyCode TimeSpeedUp;
    [SerializeField]
    private KeyCode TimeSpeedDown;

    private void Awake()
    {
        inputDirection = Vector3.zero;
        inputRotation = Quaternion.identity;
        controlCamera = false;
        axisInput = false;
        fastCamera = false;
    }

    private void Update()
    {
        if (!Node_Health.dead)
        {
            if (Input.GetKeyDown(SelectionMode)) OnKeySelectionMode();
            if (Input.GetKeyDown(ForceMode)) OnKeyForceMode();
            if (Input.GetKeyDown(ToggleGravity)) OnKeyToggleGravity();
            if (Input.GetKeyDown(Letterbox)) OnKeyLetterbox();
            if (inputRotation != Quaternion.identity) axisInput = true; else axisInput = false;
        }
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        cameraOrbitDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(CameraMode)) OnKeyCameraMode();
        if (Input.GetKeyDown(RestartScene)) OnKeyRestartScene();
        if (Input.GetKeyDown(TimeSpeedUp)) OnKeyTimeSpeedUp();
        if (Input.GetKeyDown(TimeSpeedDown)) OnKeyTimeSpeedDown();
        if (Input.GetMouseButtonDown(0)) OnKeyClickLeft();

        if (Input.GetMouseButton(1)) controlCamera = true; else controlCamera = false;
        if (Input.GetKey(FastCamera)) fastCamera = true; else fastCamera = false;

        mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X") / Time.timeScale, Input.GetAxisRaw("Mouse Y") / Time.timeScale);
    }

    void FixedUpdate()
    {
        inputRotation =
        Quaternion.AngleAxis(Input.GetAxisRaw("Yaw") * 45, Vector3.right) *
        Quaternion.AngleAxis(-Input.GetAxisRaw("Pitch") * 45, Vector3.forward) *
        Quaternion.AngleAxis(Input.GetAxisRaw("Roll") * 45, Vector3.up);
    }
}
