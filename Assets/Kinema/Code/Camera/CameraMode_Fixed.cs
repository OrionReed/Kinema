using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kinema/Fixed Camera")]
public class CameraMode_Fixed : ScriptableObject, ICameraMode
{
    [SerializeField]
    private float rotateSpeed = 5f;

    public void ControlCamera(_Camera camera, Vector3 target, bool controlCamera)
    {
        camera.transform.rotation = Quaternion.Slerp(
            camera.transform.rotation,
            Quaternion.LookRotation(target - camera.transform.position),
            (Time.deltaTime / Time.timeScale) * rotateSpeed);
    }
}
