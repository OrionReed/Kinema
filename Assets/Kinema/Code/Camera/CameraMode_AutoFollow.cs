using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kinema/Auto-Follow Camera")]
public class CameraMode_AutoFollow : ScriptableObject, ICameraMode
{
    [SerializeField]
    private float orbitSpeed = 50;
    [SerializeField]
    private float rotateSpeed = 4f;
    [SerializeField]
    private float movespeed = 4f;
    [SerializeField]
    private float distance = 6f;
    [SerializeField]
    private float relativeHeight = 2f;
    [SerializeField]
    private float maxHeight = 8;
    [SerializeField]
    private float minHeight = -3;
    [SerializeField]
    private float heightAdjustSpeed = 1f;



    public void ControlCamera(_Camera camera, Vector3 target, bool controlCamera)
    {
        if (controlCamera)
            RotateAroundTarget(camera, _Input.cameraOrbitDirection, target);
        SlerpToRelativeHeight(camera, target);
        SlerpMoveTowards(camera, target);
        SlerpRotateTowards(camera, target);
    }
    private void SlerpToRelativeHeight(_Camera camera, Vector3 target)
    {
        camera.transform.position = Vector3.Slerp(
            camera.transform.position,
            new Vector3(camera.transform.position.x, target.y + relativeHeight, camera.transform.position.z),
            (Time.deltaTime / Time.timeScale) * heightAdjustSpeed);
    }
    private void RotateAroundTarget(_Camera camera, Vector3 rotation, Vector3 target)
    {
        camera.transform.RotateAround(target, Vector3.up, -rotation.x * (Time.deltaTime / Time.timeScale) * orbitSpeed);
        relativeHeight += rotation.y * (Time.deltaTime / Time.timeScale) * heightAdjustSpeed;
        relativeHeight = Mathf.Clamp(relativeHeight, minHeight, maxHeight);
    }
    private void SlerpMoveTowards(_Camera camera, Vector3 target)
    {
        camera.transform.position = Vector3.Slerp(
            camera.transform.position,
            VectorTools.PointInDirection(target, camera.transform.position, distance),
            (Time.deltaTime / Time.timeScale) * movespeed);
    }
    private void SlerpRotateTowards(_Camera camera, Vector3 target)
    {
        camera.transform.rotation = Quaternion.Slerp(
             camera.transform.rotation,
            Quaternion.LookRotation(target - camera.transform.position),
            (Time.deltaTime / Time.timeScale) * rotateSpeed);
    }
}
