using UnityEngine;

public interface ICameraMode
{
    void ControlCamera(_Camera camera, Vector3 target);
}
