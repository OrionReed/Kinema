using System;
using UnityEngine;

public class _Input : MonoBehaviour
{
    public static event Action OnSelectionMode = delegate { };
    public static event Action OnForceMode = delegate { };
    public static event Action OnCameraMode = delegate { };
    public static event Action OnToggleGravity = delegate { };
    public static event Action OnRestartScene = delegate { };
    public static event Action OnLetterbox = delegate { };
    public static event Action OnTimeSpeedUp = delegate { };
    public static event Action OnTimeSpeedDown = delegate { };
    public static event Action OnClickLeft = delegate { };

    public static bool controlCamera { get; private set; }
    public static bool axisInput { get; private set; }
    public static bool fastCamera { get; private set; }
    public static Vector2 mouseAxis { get; private set; }
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
        DontDestroyOnLoad(this);
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
            if (Input.GetKeyDown(SelectionMode)) OnSelectionMode();
            if (Input.GetKeyDown(ForceMode)) OnForceMode();
            if (Input.GetKeyDown(ToggleGravity)) OnToggleGravity();
            if (Input.GetKeyDown(Letterbox)) OnLetterbox();
            if (inputRotation != Quaternion.identity) axisInput = true; else axisInput = false;
        }
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(CameraMode)) OnCameraMode();
        if (Input.GetKeyDown(RestartScene)) OnRestartScene();
        if (Input.GetKeyDown(TimeSpeedUp)) OnTimeSpeedUp();
        if (Input.GetKeyDown(TimeSpeedDown)) OnTimeSpeedDown();
        if (Input.GetMouseButtonDown(0)) OnClickLeft();

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
