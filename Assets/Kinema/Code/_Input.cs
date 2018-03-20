using System;
using UnityEngine;

public class _Input : MonoBehaviour
{
    public event Action OnSelectionMode = delegate { };
    public event Action OnForceMode = delegate { };
    public event Action OnCameraMode = delegate { };
    public event Action OnToggleGravity = delegate { };
    public event Action OnRestartScene = delegate { };
    public event Action OnLetterbox = delegate { };
    public event Action OnTimeSpeedUp = delegate { };
    public event Action OnTimeSpeedDown = delegate { };

    public event Action OnClickLeft = delegate { };

    public bool controlCamera { get; private set; }
    public bool axisInput { get; private set; }
    public bool fastCamera { get; private set; }
    public Vector2 mouseAxis { get; private set; }
    public Vector3 inputDirection { get; private set; }
    public Quaternion inputRotation { get; private set; }

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

    public void Init()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(SelectionMode))
            OnSelectionMode();
        if (Input.GetKeyDown(ForceMode))
            OnForceMode();
        if (Input.GetKeyDown(CameraMode))
            OnCameraMode();
        if (Input.GetKeyDown(ToggleGravity))
            OnToggleGravity();
        if (Input.GetKeyDown(RestartScene))
            OnRestartScene();
        if (Input.GetKeyDown(Letterbox))
            OnLetterbox();
        if (Input.GetKeyDown(TimeSpeedUp))
            OnTimeSpeedUp();
        if (Input.GetKeyDown(TimeSpeedDown))
            OnTimeSpeedDown();
        if (Input.GetMouseButtonDown(0))
            OnClickLeft();

        if (Input.GetMouseButton(1)) controlCamera = true; else controlCamera = false;
        if (Input.GetKey(FastCamera)) fastCamera = true; else fastCamera = false;
        if (inputRotation != Quaternion.identity) axisInput = true; else axisInput = false;

        mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X") / Time.timeScale, Input.GetAxisRaw("Mouse Y") / Time.timeScale);
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        inputRotation =
        Quaternion.AngleAxis(Input.GetAxisRaw("Yaw") * 45, Vector3.right) *
        Quaternion.AngleAxis(-Input.GetAxisRaw("Pitch") * 45, Vector3.forward) *
        Quaternion.AngleAxis(Input.GetAxisRaw("Roll") * 45, Vector3.up);
    }
}
