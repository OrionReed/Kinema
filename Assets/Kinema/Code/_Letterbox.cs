using UnityEngine;

[RequireComponent(typeof(Camera))]
public class _Letterbox : MonoBehaviour
{
    [SerializeField]
    private float letterboxAspect = 2.35f;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        _Input.OnKeyLetterbox += ToggleLetterbox;
        Node_Health.OnDeath += ToggleLetterbox;
    }

    private void OnDisable()
    {
        _Input.OnKeyLetterbox -= ToggleLetterbox;
    }
    private void ToggleLetterbox()
    {
        if (!Node_Health.dead)
            ShowLetterbox(false, true);
        else
            ShowLetterbox(true);
    }

    private void ShowLetterbox(bool showLetterbox, bool toggle = false)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float aspectHeight = screenAspect / letterboxAspect;
        Rect rect = cam.rect;
        if (toggle)
        {
            bool isLetterboxed = (cam.rect.height == aspectHeight);
            rect.width = 1.0f;
            rect.x = 0;
            rect.height = isLetterboxed ? (float)Screen.height : aspectHeight;
            rect.y = (1 - (isLetterboxed ? (float)Screen.height : aspectHeight)) / 2;
            cam.rect = rect;
        }
        else
        {
            rect.width = 1.0f;
            rect.x = 0;
            rect.height = showLetterbox ? (float)Screen.height : aspectHeight;
            rect.y = (1 - (showLetterbox ? (float)Screen.height : aspectHeight)) / 2;
            cam.rect = rect;
        }

    }
}
