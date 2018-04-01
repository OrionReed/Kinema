using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class _SceneManager : MonoBehaviour
{
    public event Action<SceneField> OnLoadScene;

    [SerializeField]
    private int sceneIndex = 0;
    [SerializeField]
    private List<SceneField> avaiableScenes;

    public void LoadSelectedScene()
    {
        OnLoadScene(avaiableScenes[sceneIndex]);
        StartCoroutine(LoadSceneAsync(avaiableScenes[sceneIndex]));
    }

    private IEnumerator LoadSceneAsync(SceneField scene)
    {
        yield return new WaitForSeconds(0);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
            yield return null;
    }
}