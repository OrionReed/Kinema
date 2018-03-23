using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class _RestartScene : MonoBehaviour
{
    public static event Action EventRestartScene = delegate { };
    private void Start()
    {
        _Input.OnRestartScene += RestartScene;
    }

    private void RestartScene()
    {
        EventRestartScene();
    }
}
