using UnityEngine;

[RequireComponent (typeof (Camera))]
public class _Letterbox : MonoBehaviour
{
    [SerializeField]
    private float letterboxAspect = 2.35f;
    private Camera cam;

    private void Start ()
    {
        cam = GetComponent<Camera> ();
        _Input.OnKeyLetterbox += ToggleLetterbox;
        //_LevelState.OnPlay += delegate { ShowLetterbox(false); };
        //_LevelState.OnDead += delegate { ShowLetterbox(true); };
    }

    private void OnDisable ()
    {
        _Input.OnKeyLetterbox -= ToggleLetterbox;
    }
    private void ToggleLetterbox ()
    {

        float screenAspect = (float) Screen.width / (float) Screen.height;
        float aspectHeight = screenAspect / letterboxAspect;
        Rect rect = cam.rect;
        bool isLetterboxed = (cam.rect.height == aspectHeight);
        rect.width = 1.0f;
        rect.x = 0;
        rect.height = isLetterboxed ? (float) Screen.height : aspectHeight;
        rect.y = (1 - (isLetterboxed ? (float) Screen.height : aspectHeight)) / 2;
        cam.rect = rect;
    }

    private void ShowLetterbox (bool showLetterbox)
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float aspectHeight = screenAspect / letterboxAspect;
        Rect rect = cam.rect;

        rect.width = 1.0f;
        rect.x = 0;
        rect.height = showLetterbox ? aspectHeight : (float) Screen.height;
        rect.y = (1 - (showLetterbox ? aspectHeight : (float) Screen.height)) / 2;
        cam.rect = rect;
    }
}