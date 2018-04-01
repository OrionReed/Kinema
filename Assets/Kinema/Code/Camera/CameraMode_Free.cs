using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kinema/Free Camera")]
public class CameraMode_Free : ScriptableObject, ICameraMode
{
    [SerializeField]
    private float mouseSensitivity = 180f;
    [SerializeField]
    private float freeNormalSpeed = 5.0f;
    [SerializeField]
    private float freeFastSpeed = 20.0f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool rotationInitialized = false;

    public void ControlCamera(_Camera camera, Vector3 target, bool controlCamera)
    {
        if (controlCamera)
        {
            InitializeRotation(camera);
            MoveCameraLocal(camera, target);
            RotateCamera(camera, target);
        }
        else
            rotationInitialized = false;
    }
    void InitializeRotation(_Camera camera)
    {
        if (!rotationInitialized)
        {
            rotationX = camera.transform.rotation.eulerAngles.y;
            rotationY = -camera.transform.rotation.eulerAngles.x;
            rotationInitialized = true;
        }
    }
    void MoveCameraLocal(_Camera camera, Vector3 target)
    {
        if (_Input.FastCamera)
            camera.transform.Translate(_Input.InputCameraDirection * freeFastSpeed * Time.deltaTime / Time.timeScale);
        else
            camera.transform.Translate(_Input.InputCameraDirection * freeNormalSpeed * Time.deltaTime / Time.timeScale);
    }
    void RotateCamera(_Camera camera, Vector3 target)
    {
        Vector3 newPosition = camera.transform.position;
        rotationX += _Input.MouseAxis.x * mouseSensitivity * Time.deltaTime;
        rotationY += _Input.MouseAxis.y * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        camera.transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        camera.transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }

}
