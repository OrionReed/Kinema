using UnityEngine;
using UnityEngine.SceneManagement;

public class _RestartScene : MonoBehaviour
{
    private _Input input;

    private void Start()
    {
        input = FindObjectOfType<_Input>();
        input.OnRestartScene += RestartScene;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
