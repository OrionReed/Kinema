using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _SceneManager : MonoBehaviour
{
    [SerializeField]
    private List<SceneField> avaiableScenes;

    public void LoadScene()
    {
        int index = 0;
        SceneManager.LoadScene(avaiableScenes[index]);
    }
}
