using UnityEngine;
using System.Collections;
#if UnityEditor
using UnityEditor;
#endif
[ExecuteInEditMode]
public class MatchOrientation : MonoBehaviour
{
    public bool TrackInEditor = false;
    public GameObject ReferenceObj;

    void OnRenderObject()
    {
        if (Application.isPlaying)
        {
            transform.position = CameraLocationTracker.cameraLoc;
        }
#if UnityEditor
        if (TrackInEditor)
        {
            if (CameraLocationTracker.cameraTransform)
                transform.position = CameraLocationTracker.cameraLoc;

        }
#endif
    }

}
