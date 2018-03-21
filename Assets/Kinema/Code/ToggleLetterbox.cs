using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ToggleLetterbox : MonoBehaviour
{
    [SerializeField]
    private float letterboxAspect = 2.35f;

    private _Input input;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        input = FindObjectOfType<_Input>();
        input.OnLetterbox += UpdateLetterbox;
    }

    private void OnDisable()
    {
        input.OnLetterbox -= UpdateLetterbox;
    }

    private void UpdateLetterbox()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float aspectHeight = screenAspect / letterboxAspect;
        Rect rect = cam.rect;
        bool isLetterboxed = (cam.rect.height == aspectHeight);
        rect.width = 1.0f;
        rect.x = 0;
        rect.height = isLetterboxed ? (float)Screen.height : aspectHeight;
        rect.y = (1 - (isLetterboxed ? (float)Screen.height : aspectHeight)) / 2;
        cam.rect = rect;
    }
}
