using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class _PersistentObjects : MonoBehaviour
{
    private static _PersistentObjects Instance;

    [SerializeField]
    private GameObject[] spawnObjects;
    private IList<IInitializeOnReload> reinitializeOnLoad;

    void OnEnable() { SceneManager.sceneLoaded += OnLevelFinishedLoading; }
    void OnDisable() { SceneManager.sceneLoaded -= OnLevelFinishedLoading; }

    void Awake()
    {
        if (Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
            SpawnObjects();
        }
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        foreach (IInitializeOnReload init in reinitializeOnLoad)
            init.InitOnSceneLoad();
    }

    void SpawnObjects()
    {
        foreach (GameObject obj in spawnObjects)
            Instantiate(obj, obj.transform.position, obj.transform.rotation);

        reinitializeOnLoad = InterfaceFinder.FindObjects<IInitializeOnReload>();
    }
}
